using System;

namespace HomeTaskManagement.App.Shopping
{
    public sealed class RecordExpenditure
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
        public string Base64EncodedDocument { get; set; }

        public RecordExpenditure(decimal amount, string description, string documentName, string base64EncodedDocument)
        {
            Amount = amount;
            Description = description;
            DocumentName = documentName;
            Base64EncodedDocument = base64EncodedDocument;
        }

        public byte[] DocumentBytes()
        {
            return Convert.FromBase64String(Base64EncodedDocument);
        }
    }
}
