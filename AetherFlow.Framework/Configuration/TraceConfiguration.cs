using System.Runtime.Serialization;
using AetherFlow.Framework.Interfaces;

namespace AetherFlow.Framework.Configuration
{
    [DataContract]
    public class TraceConfiguration : IConfiguration
    {
        public bool ShouldLogInfo { get; set; } = false;
        public bool ShouldLogDebug { get; set; } = true;
        public bool ShouldLogError { get; set; } = true;
        public bool ShouldLogFatal { get; set; } = true;
    }
}
