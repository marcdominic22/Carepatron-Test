using System;
using System.Collections.Generic;

namespace Application.Common.Models
{
    public class HealthCheckResponse
    {
        public string GitVersion { get; set; }
        public Dictionary<string, string> Entries { get; set; }
        public TimeSpan HealthCheckDuration { get; set; }
    }
}