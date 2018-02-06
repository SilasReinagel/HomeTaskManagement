
namespace HomeTaskManagement.App.Common
{
    public sealed class JsonRpcSuccessResponse
    {
        public string JsonRpc { get; } = "2.0";
        public object Result { get; set; }
        public string Id { get; set; }
    }
}
