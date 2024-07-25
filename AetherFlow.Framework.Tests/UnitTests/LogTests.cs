using System;
using System.Linq;
using AetherFlow.Framework.Configuration;
using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using Microsoft.Xrm.Sdk;
using Moq;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests
{
    public class LogTests : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseFakeXrmEasy();
            RunSpecification();
        }

        // ARRANGE variables
        private ILog _log;
        private string _tracingLevel;
        private string _message;
        private string _exception;
        public override void Arrange()
        {
            var mockTracingService = new Mock<ITracingService>();
            mockTracingService
                .Setup(service => service.Trace(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((format, message) =>
                {
                    if (message == null || message.Length == 0)
                    {
                        _exception = format;
                    }
                    else
                    {
                        _tracingLevel = message[1]?.ToString() ?? "";
                        _message = message[2]?.ToString() ?? "";
                    }
                });

            _log = new Log(
                mockTracingService.Object, 
                new TraceConfiguration { ShouldLogInfo = true }
            );
        }

        [Test]
        [TestCase("Info", "INFO", "InfoTest")]
        [TestCase("Debug", "DEBUG", "DebugTest")]
        [TestCase("Error", "ERROR", "ErrorTest")]
        [TestCase("Fatal", "FATAL", "FatalTest")]
        public void EnsureCanLogInfo(string method, string level, string message)
        {
            CallMethod(_log, method, new object[] { message });

            Assert.That(_tracingLevel, Is.EqualTo(level));
            Assert.That(_message, Is.EqualTo(message));
        }

        [Test]
        [TestCase("InfoFormat", "INFO", "InfoTest")]
        [TestCase("DebugFormat", "DEBUG", "DebugTest")]
        [TestCase("ErrorFormat", "ERROR", "ErrorTest")]
        [TestCase("FatalFormat", "FATAL", "FatalTest")]
        public void EnsureCanLogFormattedInfo(string method, string level, string message)
        {
            CallMethod(_log, method, new object[] { "{0} {1}", new object[] { message, 10 } });

            Assert.That(_tracingLevel, Is.EqualTo(level));
            Assert.That(_message, Is.EqualTo(message + " 10"));
        }

        [Test]
        [TestCase("Info", "INFO", "InfoTest")]
        [TestCase("Debug", "DEBUG", "DebugTest")]
        [TestCase("Error", "ERROR", "ErrorTest")]
        [TestCase("Fatal", "FATAL", "FatalTest")]
        public void EnsureCanLogInfoWithException(string method, string level, string message)
        {
            var exception = new Exception(message + " Exception");
            CallMethod(_log, method, new object[] { message, exception });

            Assert.That(_tracingLevel, Is.EqualTo(level));
            Assert.That(_message, Is.EqualTo(message));
            Assert.That(_exception, Is.EqualTo("System.Exception: " + message + " Exception ----> "));
        }

        [Test]
        public void EnsureInnerExceptionLogged() {
            var exception = new Exception("Outer Exception", new Exception("Inner Exception"));
            _log.Error("EnsureInnerExceptionLogged", exception);

            Assert.That(_message, Is.EqualTo("EnsureInnerExceptionLogged"));
            Assert.That(_exception, Is.EqualTo("--- End of inner exception stack trace ---"));
        }

        static object CallMethod(object obj, string methodName, object[] parameters)
        {
            // Get the type of the object
            var type = obj.GetType();
            var paramTypes = parameters.Select(p => p.GetType()).ToArray();
            var method = type.GetMethod(methodName, paramTypes);

            if (method == null)
            {
                throw new ArgumentException($"Method {methodName} not found on type {type.FullName}");
            }
            
            return method.Invoke(obj, parameters);
        }
    }
}
