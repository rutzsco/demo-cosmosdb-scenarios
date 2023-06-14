using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Services.ConnectedFactory.TVD
{
    public class Diagnostics
    {
        public class ActivityCode
        {
            public static string HealthCheckLiveness = "HealthCheckLiveness";
            public static string HealthCheckReadiness = "HealthCheckReadiness";
            public static string MessageProcessingStart = "MessageProcessingStart";
        }
    }
}
