using System.Collections.Generic;

namespace HomeTaskManagement.App.Common
{
    public sealed class InMemoryBlobStore : IBlobStore
    {
        private readonly Dictionary<string, byte[]> _blobs = new Dictionary<string, byte[]>();

        public Maybe<byte[]> Get(string id)
        {
            if (!_blobs.ContainsKey(id))
                return Maybe<byte[]>.Missing;
            return _blobs[id];
        }

        public void Put(string id, byte[] bytes)
        {
            _blobs[id] = bytes;
        }
    }
}
