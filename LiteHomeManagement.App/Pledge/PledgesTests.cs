using LiteHomeManagement.App.Accounting;
using LiteHomeManagement.App.Common;
using LiteHomeManagement.App.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LiteHomeManagement.App.Pledge
{
    [TestClass]
    public sealed class PledgesTests
    {
        private const decimal User1StartBalance = 500;
        private const int PledgeAmount = 4;

        private PledgeFundingSettings _settings;
        private Accounts _accounts;
        private SampleUsers _sampleUsers;
        private Pledges _pledges;
        private UnixUtcTime _now;
        private string User1;

        [TestInitialize]
        public void Init()
        {
            Clock.Freeze();
            _now = Clock.UnixUtcNow;
            var eventStore = new InMemoryEventStore();
            _settings = new PledgeFundingSettings();
            _accounts = new Accounts(eventStore);
            _sampleUsers = new SampleUsers();
            _pledges = new Pledges(eventStore, _sampleUsers.Users, _accounts, _settings);
            _accounts.Apply(new TransactionRequest(_sampleUsers.User1, "Deposit", User1StartBalance));
            User1 = _sampleUsers.User1;
        }

        [TestMethod]
        public void Pledges_EmptyPledge_IsCorrect()
        {
            var pledge = _pledges.Get(User1);

            Assert.AreEqual(0, pledge.AmountAt(_now));
            Assert.AreEqual(_now, pledge.FundedThrough);
        }

        [TestMethod]
        public void Pledges_SetUserPledgeForUnknownUser_UnknownEntity()
        {
            var resp = _pledges.Set(new SetPledge(_sampleUsers.UnknownUser, PledgeAmount, _now));

            resp.AssertStatusIs(ResponseStatus.UnknownEntity);
        }

        [TestMethod]
        public void Pledges_SetUserPledgeForTimeBeforeNow_InvalidState()
        {
            var resp = _pledges.Set(new SetPledge(User1, PledgeAmount, _now.Minus(TimeSpan.FromHours(1))));

            resp.AssertStatusIs(ResponseStatus.InvalidState);
        }

        [TestMethod]
        public void Pledges_SetUserPledge_PledgeCorrect()
        {
            var resp = _pledges.Set(new SetPledge(User1, PledgeAmount, _now));

            var pledge = _pledges.Get(_sampleUsers.User1);
            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(4, pledge.AmountAt(_now));
            Assert.AreEqual(_now, pledge.FundedThrough);
        }

        [TestMethod]
        public void Pledges_FundPledges_PledgesFunded()
        {
            _pledges.Set(new SetPledge(User1, 4, _now));

            var resp = _pledges.Fund(new FundPledges(_now.Plus(_settings.Frequency)));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(_now.Plus(_settings.Frequency), _pledges.Get(User1).FundedThrough);
            Assert.AreEqual(User1StartBalance - _settings.RatePerUnit * PledgeAmount, _accounts.Get(User1).Balance);
        }

        [TestMethod]
        public void Pledges_FundPledgesCalledAgain_PledgeFundingStaysTheSame()
        {
            _pledges.Set(new SetPledge(_sampleUsers.User1, 4, _now));

            _pledges.Fund(new FundPledges(_now.Plus(_settings.Frequency)));
            var resp = _pledges.Fund(new FundPledges(_now.Plus(_settings.Frequency)));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(_now.Plus(_settings.Frequency), _pledges.Get(User1).FundedThrough);
            Assert.AreEqual(User1StartBalance - _settings.RatePerUnit * PledgeAmount, _accounts.Get(User1).Balance);
        }

        [TestMethod]
        public void Pledges_FundPledgesAtNonIntervalTimes_PledgeFundingExtendedFullInterval()
        {
            var firstFundingTime = _now;
            _pledges.Set(new SetPledge(User1, PledgeAmount, firstFundingTime));
            _pledges.Fund(new FundPledges(firstFundingTime.Plus(_settings.Frequency)));
            Clock.Advance(TimeSpan.FromHours(1));

            var secondFundingTime = Clock.UnixUtcNow;
            var resp = _pledges.Fund(new FundPledges(secondFundingTime.Plus(_settings.Frequency)));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(firstFundingTime.Plus(_settings.Frequency * 2), _pledges.Get(User1).FundedThrough);
            Assert.AreEqual(User1StartBalance - _settings.RatePerUnit * PledgeAmount * 2, _accounts.Get(User1).Balance);
        }

        [TestMethod]
        public void Pledges_FundPledgesThroughDistantFutureTime_PledgesFunded()
        {
            _pledges.Set(new SetPledge(User1, PledgeAmount, _now));

            var resp = _pledges.Fund(new FundPledges(_now.Plus(_settings.Frequency * 3)));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(_now.Plus(_settings.Frequency * 3), _pledges.Get(User1).FundedThrough);
            Assert.AreEqual(User1StartBalance - _settings.RatePerUnit * PledgeAmount * 3, _accounts.Get(User1).Balance);
        }
    }
}
