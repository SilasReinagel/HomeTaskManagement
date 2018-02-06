
namespace HomeTaskManagement.App.Common
{
    public sealed class JsonRpcRequest
    {
        public string JsonRpc { get; set; }
        public string Method { get; set; }
        public dynamic Params { get; set; }
        public string Id { get; set; }
    }
}
