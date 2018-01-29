using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HomeTaskManagement.App.Common
{
    public sealed class AppHealth : IDisposable
    {
        private readonly Timer _statusTimer; 
        private readonly IEnumerable<IMonitoredComponent> _components;

        public long LastUpdate { get; private set; }
        public string OverallStatus { get; private set; } = HealthStatus.Healthy.ToString();
        public Dictionary<string, string> Components { get; } = new Dictionary<string, string>();

        public AppHealth(TimeSpan statusInterval, params IMonitoredComponent[] components)
            : this (statusInterval, (IEnumerable<IMonitoredComponent>)components) { }

        public AppHealth(TimeSpan statusInterval, IEnumerable<IMonitoredComponent> components)
        {
            _components = components;
            _statusTimer = new Timer(x => UpdateComponentStatuses(), null, TimeSpan.Zero, statusInterval);
        }

        private void UpdateComponentStatuses()
        {
            _components.ForEach(x => Components[x.Name] = x.GetStatus().ToString());
            OverallStatus = Components.Values.All(x => x.Matches(HealthStatus.Healthy.ToString()))
                ? HealthStatus.Healthy.ToString()
                : HealthStatus.Unhealthy.ToString();
            LastUpdate = Clock.UnixUtcNow;
        }

        public void Dispose()
        {
            _statusTimer.Dispose();
        }
    }
}
