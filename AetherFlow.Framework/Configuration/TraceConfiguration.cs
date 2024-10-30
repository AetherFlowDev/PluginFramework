using System.Runtime.Serialization;
using AetherFlow.Framework.Interfaces;
using System.Runtime.Serialization;

namespace AetherFlow.Framework.Configuration
{
    [DataContract]
    public class TraceConfiguration
    {
        public bool ShouldLogInfo { get; set; } = false;
        public bool ShouldLogDebug { get; set; } = true;
        public bool ShouldLogError { get; set; } = true;
        public bool ShouldLogFatal { get; set; } = true;
    }
}
