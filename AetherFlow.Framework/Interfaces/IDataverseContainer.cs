using System.Reflection;

namespace AetherFlow.Framework.Interfaces
{
    public interface IDataverseContainer
    {
        /// <summary>
        /// Initialize a collection of interfaces with implementations from a
        /// given assembly, starting with a given namespace
        /// </summary>
        /// <param name="assembly">The Assembly to identify interfaces and implementations from</param>
        /// <param name="rootNamespace">The start of the namespace for Interfaces</param>
        void Initialize(Assembly assembly, string rootNamespace);

        /// <summary>
        /// Initialize a collection of interfaces with implementations from a
        /// given assembly, starting with one of the provided namespaces
        /// </summary>
        /// <param name="assembly">The Assembly to identify interfaces and implementations from</param>
        /// <param name="rootNamespaces">An array of namespaces for Interfaces</param>
        void Initialize(Assembly assembly, string[] rootNamespaces);

        /// <summary>
        /// Add a custom implementation for a given type of interface
        /// </summary>
        /// <typeparam name="TKey">Interface to provide the implementation for</typeparam>
        /// <typeparam name="T">The class which implements the interface provided</typeparam>
        void Add<TKey, T>();

        /// <summary>
        /// Adds a singleton to the services object as a given implementation of the associated interface
        /// </summary>
        /// <typeparam name="TKey">Interface this object is an implementation for</typeparam>
        /// <param name="instance">Implementation object for the given interface</param>
        void Add<TKey>(object instance);

        /// <summary>
        /// Get a service from this container
        /// </summary>
        /// <typeparam name="T">Interface to return an implementation of</typeparam>
        /// <returns></returns>
        T Get<T>();
    }
}