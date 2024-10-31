using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Interfaces;

namespace AetherFlow.Framework
{
    public class DataverseContainer : IDataverseContainer
    {
        private readonly IDictionary<Type, List<Type>> _implementations = new Dictionary<Type, List<Type>>();
        private readonly IList<object> _services = new List<object>();
        private readonly IList<Type> _toMock = new List<Type>();
        private readonly IDictionary<Type, string> _useVariant = new Dictionary<Type, string>();

        public void Initialize(Assembly assembly, string rootNamespace) 
            => Initialize(assembly, new[] { rootNamespace });

        public void Initialize(Assembly assembly, string[] rootNamespaces)
        {
            // Use reflection to get a list of types
            var types = assembly.GetTypes()
                .Where(t => t.Namespace != null && t.IsInterface)
                .Where(t => rootNamespaces.Any(ns => t.Namespace.StartsWith(ns)))
                .Distinct()
                .ToArray();

            // Loop through the types and register an implementation
            // We want to avoid abstract classes, and any classes that include the word "Mock" or "Base"
            foreach (var type in types)
            {
                // Get generic types
                if (type.IsGenericTypeDefinition)
                {
                    var implementations = GetGenericImplementationsOf(assembly, type);
                    if (implementations != null && implementations.Length > 0)
                    {
                        // We have some generic implementations, however, we may need to remove
                        // some duplicates where implementations have been added by default
                        if (_implementations.TryGetValue(type, out var current))
                        {
                            // We have some already added... lets remove duplicates!
                            var newItems = implementations
                                .ToList()
                                .Where(a => !current.Contains(a))
                                .ToArray();

                            // Add to implementations
                            _implementations[type].AddRange(newItems);
                        }
                        _implementations.Add(type, implementations.ToList());
                    }

                    // No further action, continue
                    continue;
                }

                // Get the implementation of the interface
                var allImplementations = GetImplementationsOf(assembly, type);

                // If we have an implementation, add it to the dictionary
                // We can use this to build it later should it be needed!
                if (allImplementations != null)
                    _implementations.Add(type, allImplementations);
            }
        }

        /// <summary>
        /// Get multiple generic implementations of a given generic type
        /// from a defined assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type[] GetGenericImplementationsOf(Assembly assembly, Type type) =>
            assembly
                .GetTypes()
                .Where(t => 
                    !t.IsInterface 
                    && !t.IsAbstract
                )
                .SelectMany(t => t.GetInterfaces(), (t, @interface) => new { Type = t, Interface = @interface })
                .Where(t => t.Interface.IsGenericType)
                .Where(t => t.Interface.GetGenericTypeDefinition().Namespace == type.GetGenericTypeDefinition().Namespace)
                .Where(t => t.Interface.GetGenericTypeDefinition().Name == type.GetGenericTypeDefinition().Name)
                .Select(t => t.Type)
                .ToArray();

        /// <summary>
        /// Get an implementation for a given interface from a defined
        /// assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<Type> GetImplementationsOf(Assembly assembly, Type type) => 
            assembly
                .GetTypes()
                    .Where(t =>
                        t.Namespace != null
                        && t.IsClass
                        && !t.IsAbstract
                    )
                    .Where(
                        t => type.IsGenericTypeDefinition
                            ? type.MakeGenericType(t.GetGenericArguments()) == t
                            : type.IsAssignableFrom(t)
                    )
                    .ToList();

        /// <summary>
        /// Get the best constructor for an implementation, based on the
        /// number of parameters and if they can be satisfied via CI/CD
        /// </summary>
        /// <param name="constructors"></param>
        /// <returns></returns>
        private ConstructorInfo GetBestConstructor(IEnumerable<ConstructorInfo> constructors) => 
            constructors
                .Where(
                    a => a.GetParameters()
                        .Where(b => b.ParameterType.IsInterface)
                        .All(c => _implementations.ContainsKey(c.ParameterType) || _services.Any(d => c.ParameterType.IsInstanceOfType(d)))
                )
                .OrderByDescending(a => a.GetParameters().Length)
                .FirstOrDefault();

        public void Add<TKey, T>()
        {
            // Handle generics!
            if (typeof(TKey).IsGenericTypeDefinition)
            {
                // Add to the existing list, if one exists
                if (_implementations.ContainsKey(typeof(TKey))) _implementations[typeof(TKey)].Add(typeof(T));
                else _implementations.Add(typeof(TKey), new List<Type> { typeof(T) });
                return;
            }

            // This is used to allow us to override the default
            // implementation of a service with a MockService
            if (!_implementations.ContainsKey(typeof(TKey)))
                _implementations[typeof(TKey)] = new List<Type> { typeof(T) };
        }

        public void Add<TKey>(object instance)
        {
            // This is to be able to register a new object into the container
            // Only do this if the instance has not already been added!
            if (!_services.Any(a => a is TKey))
                _services.Add(instance);
        }

        public T Get<T>()
        {
            // We want to get a singleton of the instance if an interface,
            // however for a concrete class, we want to return a new instance 
            // every time.  We don't want to duplicate configuration - so we 
            // exclude classes assignable from IConfiguration from this rule.
            if (ShouldUseSingleton(typeof(T)))
            {
                // This is an interface or configuration, so attempt to get
                // the singleton from our services
                var service = (T)GetServiceSingleton(typeof(T));
                if (service != null) return service;
            }

            // We did not find a service, therefore, we should create
            // a new instance of the service through dependency injection
            // and then return it!
            return (T)Get(typeof(T));
        }

        public void UseMock<T>()
        {
            _toMock.Add(typeof(T));
        }

        public void UseVariant<T>(string variant)
        {
            var type = typeof(T);
            if (!_useVariant.ContainsKey(type))
                _useVariant.Add(type, variant);
            else
                _useVariant[type] = variant;
        }

        private object Get(Type type)
        {
            // Store the type of the implementation.  We will need this to understand
            // what type of object we are creating
            Type implementation;
            
            // We need to once again attempt to get the implementation, as this
            // function is recursive, however, we only want this to happen for interfaces
            // where it's a concrete implementation we want a new instance every time!
            if (ShouldUseSingleton(type))
            {
                var service = GetServiceSingleton(type);
                if (service != null) return service;
            }

            if (type.IsInterface)
            {
                // We now need to get the implementation of the interface
                // We should check we actually have one first
                if ((type.IsGenericType && GetImplementationForGenericType(type) == null) || (!type.IsGenericType && !_implementations.ContainsKey(type)))
                    throw new InvalidOperationException($"No implementation found for {type.FullName}");

                // Get the implementation
                implementation = type.IsGenericType 
                    ? GetImplementationForGenericType(type)
                    : GetImplementation(type);
            } 
            else
            {
                // The implementation IS the type that's being passed
                // as this is not an interface.
                implementation = type;
            }

            // Create an instance of the implementation
            var constructor = GetBestConstructor(implementation.GetConstructors());
            var parameters = constructor?.GetParameters().Select(a => Get(a.ParameterType)).ToArray();
            var instance = Activator.CreateInstance(implementation, parameters);

            // Register the instance as a singleton in the 
            // services list - however, only if the request is an interface
            if (type.IsInterface) _services.Add(instance);

            // return the instance
            return instance;
        }

        private bool ShouldUseSingleton(Type type) =>
            type.IsInterface || type.GetCustomAttributes(typeof(DataContractAttribute), true).Length > 0;

        private Type GetImplementation(Type type)
        {
            var useMock = _toMock.Contains(type);
            var useVariant = _useVariant.ContainsKey(type) ? _useVariant[type] : null;
            var implementations = _implementations.Where(a => a.Key == type).SelectMany(a => a.Value).ToList();

            // We might have a situation, where we don't have any implementations, but we do
            // have a service... this is a special case, where we should return the service
            if (implementations.Count == 0 && _services.Any(type.IsInstanceOfType))
                return _services.First(type.IsInstanceOfType).GetType();

            // Now lets handle mocks!
            if (useMock)
            {
                // Return the first implementation that has a Mock Attribute
                return implementations
                    .FirstOrDefault(impl => impl.GetCustomAttributes(typeof(MockAttribute), true).Any());
            }

            if (useVariant != null)
            {
                // Return the first implementation that has a Variant Attribute with the specified name
                return implementations
                    .FirstOrDefault(impl =>
                        impl.GetCustomAttributes(typeof(VariantAttribute), true)
                            .OfType<VariantAttribute>()
                            .Any(attr => attr.Name == useVariant));
            }

            // Else - return the first "default" implementation, or if only a single implementation that is NOT a mock
            // return that instead.
            var main = implementations.FirstOrDefault(impl => impl.GetCustomAttributes(typeof(MainAttribute), true).Any());
            if (main != null) return main;

            // Check for non-mock implementations
            var nonMock = implementations.Where(impl => !impl.GetCustomAttributes(typeof(MockAttribute), true).Any()).ToList();
            if (nonMock.Count == 1) return nonMock.First();
            throw new InvalidOperationException($"No suitable implementation found for type {type.FullName}. Please ensure there is a main implementation or a single non-mock implementation.");
        }

        private Type GetImplementationForGenericType(Type type)
        {
            var useMock = _toMock.Contains(type);
            var useVariant = _useVariant.ContainsKey(type);

            // Find all implementations for the specified generic type
            var implementations = _implementations
                .First(x => x.Key.FullName == type.Namespace + "." + type.Name).Value
                .Where(a => a.GetInterfaces().Any(t => t == type));

            if (useMock)
            {
                // Return the first implementation that has a Mock Attribute
                return implementations
                    .FirstOrDefault(impl => impl.GetCustomAttributes(typeof(MockAttribute), true).Any());
            }

            if (useVariant)
            {
                var variantName = _useVariant[type];
                // Return the first implementation that has a Variant Attribute with the specified name
                return implementations
                    .FirstOrDefault(impl =>
                        impl.GetCustomAttributes(typeof(VariantAttribute), true)
                            .OfType<VariantAttribute>()
                            .Any(attr => attr.Name == variantName));
            }

            // Else - return the first "default" implementation, or if only a single implementation that is NOT a mock
            // return that instead.
            return implementations
                .FirstOrDefault(impl =>
                    impl.GetCustomAttributes(typeof(MainAttribute), true).Any() ||
                    !impl.GetCustomAttributes(typeof(MockAttribute), true).Any());
        }



        private object GetServiceSingleton(Type type)
        {
            if (type.IsGenericType)
            {
                var implementation = GetImplementationForGenericType(type);
                if (implementation == null) return null;
                var gService = _services.FirstOrDefault(implementation.IsInstanceOfType);
                if (gService != null && gService.GetType().GetCustomAttributes(typeof(UniqueAttribute), true).Length > 0) return null;
                if (gService != null) return gService;
            }
            else if (type.IsInterface)
            {
                // Not a generic
                // Get the service from the services list if
                // and return it, but only if it exists
                var implementation = GetImplementation(type);
                var service = _services.FirstOrDefault(implementation.IsInstanceOfType);
                if (service != null && service.GetType().GetCustomAttributes(typeof(UniqueAttribute), true).Length > 0) return null;
                if (service != null) return service;
            }
            else
            {
                // Not an interface, so see if we have a copy
                // of the service in the services list
                var service = _services.FirstOrDefault(type.IsInstanceOfType);
                if (service != null) return service;
            }

            return null;
        }
    }
}
