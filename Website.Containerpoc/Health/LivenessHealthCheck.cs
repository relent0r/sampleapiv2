using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Website.Containerpoc.Health
{
    internal class LivenessHealthCheck : IHealthCheck
    {
        private HealthStatusData data;

        public LivenessHealthCheck(HealthStatusData data)
        {
            this.data = data;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data.IsLiveness)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Error"));
            }
        }
    }
}
