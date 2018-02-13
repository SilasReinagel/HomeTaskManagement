using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeTaskManagement.Sql.BlobStore
{
    [Ignore]
    [TestClass]
    public sealed class SqlBlobStoreTests
    {
        private SqlBlobStore _store;

        [TestInitialize]
        public void Init()
        {
            _store = new SqlBlobStore(new SqlDatabase(), "HomeTask.Blobs");
        }

        [TestMethod]
        public void SqlBlobStore_PutGet_Works()
        {
            var bytes = new byte[2] { 0, 1 };
            _store.Put("Test", bytes);

            CollectionAssert.AreEqual(bytes, _store.Get("Test").Value);

            _store.Delete("Test");
        }
    }
}
