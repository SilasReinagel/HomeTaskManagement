using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Accounting
{
    public sealed class SetOverdraftPolicy : IConvertToEvent
    {
        public string AccountId { get; }
        public bool CanOverdraft { get; }

        public SetOverdraftPolicy(string accountId, bool canOverdraft)
        {
            AccountId = accountId;
            CanOverdraft = canOverdraft;
        }

        public Event ToEvent()
        {
            var json = new JsonObjectString()
                .With(nameof(AccountId), AccountId)
                .With(nameof(CanOverdraft), CanOverdraft).ToString();
            return new Event(nameof(Account), AccountId, nameof(OverdraftPolicySet), 1, json, Clock.UnixUtcNow);
        }
    }
}
