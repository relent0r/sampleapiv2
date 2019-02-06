using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Containerpoc.Health
{
    public class HealthStatusData
    {
        public bool IsReady { get; set; } = true;
        public bool IsLiveness { get; set; } = true;
    }
}
