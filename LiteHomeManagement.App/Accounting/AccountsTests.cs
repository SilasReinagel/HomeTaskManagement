using LiteHomeManagement.App.Accounting;
using LiteHomeManagement.App.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiteHomeManagement.App.Accounting
{
    [TestClass]
    public sealed class AccountsTests
    {
        private const string SampleAccountId = "accountId";
        private const string SampleAccountId2 = "accountId2";

        private Accounts _accounts;

        [TestInitialize]
        public void Init()
        {
            _accounts = new Accounts(new InMemoryEventStore());
        }

        [TestMethod]
        public void Accounts_EmptyAccount_BalanceIsZero()
        {
            var account = _accounts.Get(SampleAccountId);

            Assert.AreEqual(0, account.Balance);
            Assert.AreEqual(0, account.Transactions.Count);
        }

        [TestMethod]
        public void Accounts_CreditAccount_BalanceCorrect()
        {
            var resp = _accounts.Apply(new TransactionRequest(SampleAccountId, "description", 2m));

            Assert.IsTrue(resp.Succeeded);
            Assert.AreEqual(2m, _accounts.Get(SampleAccountId).Balance);
        }

        [TestMethod]
        public void Accounts_DebitMoreThanAccountBalanced_InvalidState()
        {
            var resp = _accounts.Apply(new TransactionRequest(SampleAccountId, "description", -2m));

            resp.AssertStatusIs(ResponseStatus.InvalidState);
        }

        [TestMethod]
        public void Accounts_ApplyMultipleTransactions_BalanceCorrect()
        {
            _accounts.Apply(new TransactionRequest(SampleAccountId, "description", 2m));
            _accounts.Apply(new TransactionRequest(SampleAccountId, "description", 3m));
            _accounts.Apply(new TransactionRequest(SampleAccountId, "description", -4m));
            
            Assert.AreEqual(1m, _accounts.Get(SampleAccountId).Balance);
        }

        [TestMethod]
        public void Accounts_SetCanOverdraft_CanHaveNegativeBalance()
        {
            _accounts.Apply(new SetOverdraftPolicy(SampleAccountId, true));
            var resp = _accounts.Apply(new TransactionRequest(SampleAccountId, "description", -4m));

            Assert.IsTrue(resp.Succeeded);
            Assert.AreEqual(-4m, _accounts.Get(SampleAccountId).Balance);
        }

        [TestMethod]
        public void Accounts_Transfer_BalancesCorrect()
        {
            _accounts.Apply(new TransactionRequest(SampleAccountId, "description", 3m));
            var resp = _accounts.Apply(new TransferRequest(SampleAccountId, SampleAccountId2, "description", 1.01m));

            Assert.IsTrue(resp.Succeeded);
            Assert.AreEqual(1.99m, _accounts.Get(SampleAccountId).Balance);
            Assert.AreEqual(1.01m, _accounts.Get(SampleAccountId2).Balance);
        }

        [TestMethod]
        public void Accounts_TransferWithInsufficientBalance_RequestRejected()
        {
            var resp = _accounts.Apply(new TransferRequest(SampleAccountId, SampleAccountId2, "description", 1.01m));

            resp.AssertStatusIs(ResponseStatus.InvalidState);
            Assert.AreEqual(0m, _accounts.Get(SampleAccountId).Balance);
            Assert.AreEqual(0m, _accounts.Get(SampleAccountId2).Balance);
        }
    }
}
