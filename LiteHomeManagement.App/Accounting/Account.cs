using LiteHomeManagement.App.Common;
using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.Accounting
{
    public sealed class Account
    {
        public string Id { get; }
        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();
        public decimal Balance => Transactions.Sum(x => x.Amount);
        public bool CanOverdraft { get; private set; }

        public Account(string userId, IEnumerable<Event> events)
        {
            Id = userId;
            events.ForEach(Apply);
        }

        public ValidationResult ValidateProposed(Event e)
        {
            var issues = new List<string>();

            Apply(e);
            if (!CanOverdraft && Balance < 0)
                issues.Add($"Account {Id} cannot withdraw more than its current balance.");
            
            return new ValidationResult(issues);
        }

        private void Apply(Event e)
        {
            if (e.Name.Matches(nameof(Transaction)))
                Transactions.Add(e.PayloadAs<Transaction>());
            if (e.Name.Matches(nameof(OverdraftPolicySet)))
                CanOverdraft = e.PayloadAs<OverdraftPolicySet>().CanOverdraft;
        }
    }
}
