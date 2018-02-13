
namespace HomeTaskManagement.App.Common
{
    public interface IBlobStore
    {
        Maybe<byte[]> Get(string id);
        void Put(string id, byte[] bytes);
    }
}
