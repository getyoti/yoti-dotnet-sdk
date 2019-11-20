using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Yoti.Auth.Share.ThirdParty;

namespace Yoti.Auth.Share
{
    public class ExtraData
    {
        /// <summary>
        /// Return the credential issuance details associated with the
        /// extra data in a receipt. Null if not available.
        /// </summary>
        public AttributeIssuanceDetails AttributeIssuanceDetails { get; private set; }

        public ExtraData()
        {
            AttributeIssuanceDetails = null;
        }

        public ExtraData(List<object> parsedDataEntries)
        {
            Validation.NotNull(parsedDataEntries, nameof(parsedDataEntries));

            ReadOnlyCollection<AttributeIssuanceDetails> attributeIssuanceDetailsList = FilterForType<AttributeIssuanceDetails>(parsedDataEntries);

            if (attributeIssuanceDetailsList.Count > 0)
                AttributeIssuanceDetails = attributeIssuanceDetailsList.First();
            else
                AttributeIssuanceDetails = null;
        }

        private static ReadOnlyCollection<T> FilterForType<T>(List<object> values)
        {
            List<T> filteredList = new List<T>();

            foreach (object value in values)
            {
                if (value is T)
                {
                    filteredList.Add((T)value);
                }
            }

            return filteredList.AsReadOnly();
        }
    }
}