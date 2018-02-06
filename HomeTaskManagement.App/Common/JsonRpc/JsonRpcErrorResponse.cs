
namespace HomeTaskManagement.App.Common
{
    public sealed class JsonRpcErrorResponse
    {
        public string JsonRpc { get; } = "2.0";
        public JsonRpcError Error { get; set; }
        public string Id { get; set; }
    }
}
