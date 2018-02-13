using HomeTaskManagement.App.Accounting;
using HomeTaskManagement.App.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HomeTaskManagement.App.Shopping
{
    [TestClass]
    public sealed class TreasuryTests
    {
        private IBlobStore _blobStore;
        private Accounts _accounts;
        private Treasury _treasury;

        [TestInitialize]
        public void Init()
        {
            Clock.Freeze();
            _blobStore = new InMemoryBlobStore();
            _accounts = new Accounts(new InMemoryEventStore());
            _treasury = new Treasury(_blobStore, _accounts);
        }

        [TestMethod]
        public void Treasury_RecordExpenditure_AccountBalanceCorrect()
        {
            _accounts.Apply(new TransactionRequest(PoolAccounts.TreasuryAccountId, "Deposit", 500));

            var resp = _treasury.Apply(new RecordExpenditure(123, "Costco Shopping", "receipt.jpg", Convert.ToBase64String(new byte[0])));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(500 - 123, _accounts.Get(PoolAccounts.TreasuryAccountId).Balance);
        }

        [TestMethod]
        public void Treasury_StoreDateStampedDocument_DocumentCorrect()
        {
            var docBytes = Convert.ToBase64String(new byte[0]);
            var resp = _treasury.Apply(new RecordExpenditure(123, "Costco Shopping", "receipt.jpg", docBytes));

            var receipt = _blobStore.Get($"{Clock.UnixUtcNow.Millis}-receipt.jpg");

            Assert.IsTrue(receipt.IsPresent);
            Assert.AreEqual(docBytes, Convert.ToBase64String(receipt.Value));
        }
    }
}
