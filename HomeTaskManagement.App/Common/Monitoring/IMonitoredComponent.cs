
namespace HomeTaskManagement.App.Common
{
    public interface IMonitoredComponent
    {
        string Name { get; }
        HealthStatus GetStatus();
    }
}
