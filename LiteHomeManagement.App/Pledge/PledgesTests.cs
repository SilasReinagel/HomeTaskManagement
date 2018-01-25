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
        private SampleUsers _sampleUsers;
        private Pledges _pledges;

        [TestInitialize]
        public void Init()
        {
            Clock.Freeze();
            var eventStore = new InMemoryEventStore();
            _sampleUsers = new SampleUsers();
            _pledges = new Pledges(eventStore, _sampleUsers.Users, new Accounts(eventStore));
        }

        [TestMethod]
        public void Pledges_SetUserPledgeForUnknownUser_UnknownEntity()
        {
            var resp = _pledges.Set(new SetPledge(_sampleUsers.UnknownUser, 4, Clock.UnixUtcNow));

            resp.AssertStatusIs(ResponseStatus.UnknownEntity);
        }

        [TestMethod]
        public void Pledges_SetUserPledgeForTimeBeforeNow_InvalidState()
        {
            var resp = _pledges.Set(new SetPledge(_sampleUsers.User1, 4, Clock.UnixUtcNow.Minus(TimeSpan.FromHours(1))));

            resp.AssertStatusIs(ResponseStatus.InvalidState);
        }

        [TestMethod]
        public void Pledges_SetUserPledge_PledgeCorrect()
        {
            var now = Clock.UnixUtcNow;
            var resp = _pledges.Set(new SetPledge(_sampleUsers.User1, 4, now));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(4, _pledges.Get(_sampleUsers.User1).AmountAt(now));
        }
    }
}
