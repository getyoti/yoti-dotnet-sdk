using System;
using System.Globalization;

namespace Yoti.Auth.Document
{
    public class DocumentDetails
    {
        /// <summary>
        /// 3 digit country code, e.g. “GBR“
        /// </summary>
        public string IssuingCountry { get; private set; }

        /// <summary>
        /// Document number (may include letters) from the document.
        /// </summary>
        public string DocumentNumber { get; private set; }

        /// <summary>
        /// Expiration date of the document in DateTime format.
        /// If the document does not expire, this field will not be present.
        /// The time part of this DateTime will default to 00:00:00.
        /// </summary>
        public DateTime? ExpirationDate { get; private set; }

        /// <summary>
        ///  Type of the document e.g. PASSPORT | DRIVING_LICENCE | NATIONAL_ID | PASS_CARD
        /// </summary>
        public string DocumentType { get; private set; }

        /// <summary>
        /// Can either be a country code (for a state), or the name of the issuing authority.
        /// </summary>
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
            string result = $"{DocumentType} {IssuingCountry} {DocumentNumber}";

            if (ExpirationDate != null)
            {
                result = result + " " + ExpirationDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
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