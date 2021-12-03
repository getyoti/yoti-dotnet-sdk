using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary;

namespace DocScanExample.Models
{
	public static class DisplayHelper
    {
		public static string GetSummaryResponseKindText(ReportResponseWithSummary advancedReport)
		{
			var searchConfigType = advancedReport.WatchlistSummary.SearchConfig.GetType();
			if (searchConfigType == typeof(WatchlistAdvancedCaSearchConfigResponseYotiAccount))
			{
				return "Yoti Account";
			}
			if (searchConfigType == typeof(WatchlistAdvancedCaSearchConfigResponseCustomAccount))
			{
				return "Custom Account";
			}
			if (searchConfigType == typeof(WatchlistScreeningConfig))
			{
				return "Watchlist Screening";
			}
			return "N/A";
		}
		public static string GetPrettifiedWatchlistSearchConfig(ReportResponseWithSummary advancedReport)
		{
			var json = JsonConvert.SerializeObject(advancedReport.WatchlistSummary.SearchConfig);
			return JValue.Parse(json).ToString(Formatting.Indented);
		}
	}
}