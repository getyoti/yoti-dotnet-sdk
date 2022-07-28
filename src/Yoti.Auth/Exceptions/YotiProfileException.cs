using Newtonsoft.Json.Linq;
using System;

namespace Yoti.Auth.Exceptions
{
	public class YotiProfileException : YotiException
	{
		public YotiProfileException()
			: base()
		{
		}

		public YotiProfileException(string message)
			: base(message)
		{
		}

		public YotiProfileException(string message, string responseContent)
		 : base(message)
		{
			ResponseContent = responseContent;
			dynamic jsonResponse = JObject.Parse(responseContent);
			if (jsonResponse.error_details != null && jsonResponse.error_details.error_code != null)
				ErrorCode = jsonResponse.error_details.error_code;
		}

		public YotiProfileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public string ResponseContent { get; private set; }
		public string ErrorCode { get; private set; }
	}
}