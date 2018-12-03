using System;

namespace Yoti.Auth.Document
{
    public class DocumentDetails
    {
        public string IssuingCountry { get; private set; }
        public string DocumentNumber { get; private set; }
        public DateTime? ExpirationDate { get; private set; }
        public string DocumentType { get; private set; }
        public string IssuingAuthority { get; private set; }

        public DocumentDetails(string documentType, string issuingCountry, string documentNumber, DateTime? expirationDate, string issuingAuthority)
        {
            DocumentType = documentType;
            IssuingCountry = issuingCountry;
            DocumentNumber = documentNumber;
            ExpirationDate = expirationDate;
            IssuingAuthority = issuingAuthority;
        }

        public override string ToString()
        {
            string result = string.Format("{0} {1} {2}", DocumentType, IssuingCountry, DocumentNumber);

            if (ExpirationDate != null)
            {
                result = result + " " + ExpirationDate.Value.ToString("yyyy-MM-dd");
            }

            if (!string.IsNullOrEmpty(IssuingAuthority))
            {
                string separator = ExpirationDate == null ? " - " : " ";
                result = result + separator + IssuingAuthority;
            }

            return result;
        }
    }
}