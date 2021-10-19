using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Create.Check;
using Yoti.Auth.DocScan.Session.Create.Check.Advanced;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedWatchlistAdvancedCaCheckBuilderTests
    {
        [TestMethod]
        public void ShouldBuildForYotiAccount()
        {
            RequestedWatchlistAdvancedCaCheck check =
                new RequestedWatchlistAdvancedCaCheckBuilderYotiAccount()
                .Build();

            Assert.AreEqual(DocScanConstants.WatchlistAdvancedCa, check.Type);
            Assert.IsNotNull(check.Config);
        }

        [TestMethod]
        public void ShouldBuildForCustomAccount()
        {
            RequestedWatchlistAdvancedCaCheck check =
                new RequestedWatchlistAdvancedCaCheckBuilderCustomAccount()
                .Build();

            Assert.AreEqual(DocScanConstants.WatchlistAdvancedCa, check.Type);
            Assert.IsNotNull(check.Config);
        }

        [TestMethod]
        public void ShouldBuildForYotiAccountWithCorrectProperties()
        {
            bool removeDeceased = true;
            bool shareUrl = false;

            List<string> sourcesTypesList = new List<string> { "type1", "type2" };
            RequestedCaSources sources = new RequestedTypeListSources(sourcesTypesList);
            RequestedCaMatchingStrategy matchingStrategy = new RequestedExactMatchingStrategy();

            RequestedWatchlistAdvancedCaCheck check =
                new RequestedWatchlistAdvancedCaCheckBuilderYotiAccount()
                .WithRemoveDeceased(removeDeceased)
                .WithShareUrl(shareUrl)
                .WithSources(sources)
                .WithMatchingStrategy(matchingStrategy)
                .Build();

            Assert.IsNotNull(check);
            Assert.AreEqual(DocScanConstants.WatchlistAdvancedCa, check.Type);
            Assert.AreEqual(removeDeceased, check.Config.RemoveDeceased);
            Assert.AreEqual(shareUrl, check.Config.ShareUrl);
            Assert.IsInstanceOfType(check.Config.Sources, typeof(RequestedTypeListSources));
            Assert.IsInstanceOfType(check.Config.MatchingStrategy, typeof(RequestedExactMatchingStrategy));
            Assert.AreEqual(DocScanConstants.WithYotiAccount, check.Config.Type);
        }

        [TestMethod]
        public void ShouldSerialiseAndDeserialiseForYotiAccountWithCorrectConfigPropertyTypes()
        {
            bool removeDeceased = true;
            bool shareUrl = false;

            List<string> sourcesTypesList = new List<string> { "type1", "type2" };
            RequestedCaSources sources = new RequestedTypeListSources(sourcesTypesList);
            RequestedCaMatchingStrategy matchingStrategy = new RequestedExactMatchingStrategy();

            RequestedWatchlistAdvancedCaCheck check =
                new RequestedWatchlistAdvancedCaCheckBuilderYotiAccount()
                .WithRemoveDeceased(removeDeceased)
                .WithShareUrl(shareUrl)
                .WithSources(sources)
                .WithMatchingStrategy(matchingStrategy)
                .Build();

            string jsonData = JsonConvert.SerializeObject(check);
            var jsonRoundTripObj = JsonConvert.DeserializeObject<RequestedWatchlistAdvancedCaCheck>(jsonData);


            Assert.AreEqual(DocScanConstants.WatchlistAdvancedCa, check.Type);
            Assert.AreEqual(removeDeceased, jsonRoundTripObj.Config.RemoveDeceased);
            Assert.AreEqual(shareUrl, jsonRoundTripObj.Config.ShareUrl);
            Assert.IsInstanceOfType(check.Config.Sources, typeof(RequestedTypeListSources));
            Assert.IsInstanceOfType(check.Config.MatchingStrategy, typeof(RequestedExactMatchingStrategy));
            Assert.AreEqual(DocScanConstants.TypeList, jsonRoundTripObj.Config.Sources.Type);
            Assert.AreEqual(DocScanConstants.Exact, jsonRoundTripObj.Config.MatchingStrategy.Type);
            CollectionAssert.AreEqual(sourcesTypesList, ((RequestedTypeListSources)jsonRoundTripObj.Config.Sources).Types);
            Assert.IsTrue(((RequestedExactMatchingStrategy)jsonRoundTripObj.Config.MatchingStrategy).ExactMatch);
            Assert.AreEqual(DocScanConstants.WithYotiAccount, jsonRoundTripObj.Config.Type);
        }

        [TestMethod]
        public void ShouldSerialiseAndDeserialiseForYotiAccountWithCorrectConfigPropertyTypesVariation()
        {
            bool removeDeceased = false;
            bool shareUrl = true;

            string searchProfile = "someOtherSearchProfile";
            RequestedCaSources sources = new RequestedSearchProfileSources(searchProfile);
            double someFuzziness = 0.8;
            RequestedCaMatchingStrategy matchingStrategy = new RequestedFuzzyMatchingStrategy(someFuzziness);

            RequestedWatchlistAdvancedCaCheck check =
                new RequestedWatchlistAdvancedCaCheckBuilderYotiAccount()
                .WithRemoveDeceased(removeDeceased)
                .WithShareUrl(shareUrl)
                .WithSources(sources)
                .WithMatchingStrategy(matchingStrategy)
                .Build();

            string jsonData = JsonConvert.SerializeObject(check);
            var jsonRoundTripObj = JsonConvert.DeserializeObject<RequestedWatchlistAdvancedCaCheck>(jsonData);

            Assert.AreEqual(DocScanConstants.WatchlistAdvancedCa, check.Type);
            Assert.AreEqual(removeDeceased, jsonRoundTripObj.Config.RemoveDeceased);
            Assert.AreEqual(shareUrl, jsonRoundTripObj.Config.ShareUrl);
            Assert.IsInstanceOfType(check.Config.Sources, typeof(RequestedSearchProfileSources));
            Assert.IsInstanceOfType(check.Config.MatchingStrategy, typeof(RequestedFuzzyMatchingStrategy));
            Assert.AreEqual(DocScanConstants.Profile, jsonRoundTripObj.Config.Sources.Type);
            Assert.AreEqual(DocScanConstants.Fuzzy, jsonRoundTripObj.Config.MatchingStrategy.Type);
            Assert.AreEqual(searchProfile, ((RequestedSearchProfileSources)jsonRoundTripObj.Config.Sources).SearchProfile);
            Assert.AreEqual(someFuzziness, ((RequestedFuzzyMatchingStrategy)jsonRoundTripObj.Config.MatchingStrategy).Fuzziness);
            Assert.AreEqual(DocScanConstants.WithYotiAccount, jsonRoundTripObj.Config.Type);
        }

        [TestMethod]
        public void ShouldSerialiseAndDeserialiseForCustomAccountWithCorrectConfigPropertyTypes()
        {
            bool removeDeceased = true;
            bool shareUrl = false;

            List<string> sourcesTypesList = new List<string> { "type1", "type2" };
            RequestedCaSources sources = new RequestedTypeListSources(sourcesTypesList);
            RequestedCaMatchingStrategy matchingStrategy = new RequestedExactMatchingStrategy();

            string apiKey = "someApiKey";
            bool monitoring = true;
            Dictionary<string, string> tags = new Dictionary<string, string> { { "tag1", "value1" }, { "tag2", "value2" } };
            string clientRef = "someClientRef";

            RequestedWatchlistAdvancedCaCheck check =
                new RequestedWatchlistAdvancedCaCheckBuilderCustomAccount()
                .WithApiKey(apiKey)
                .WithMonitoring(monitoring)
                .WithTags(tags)
                .WithClientRef(clientRef)
                //from base
                .WithRemoveDeceased(removeDeceased)
                .WithShareUrl(shareUrl)
                .WithSources(sources)
                .WithMatchingStrategy(matchingStrategy)
                .Build();

            string jsonData = JsonConvert.SerializeObject(check);
            var jsonRoundTripObj = JsonConvert.DeserializeObject<RequestedWatchlistAdvancedCaCheck>(jsonData);

            Assert.AreEqual(DocScanConstants.WatchlistAdvancedCa, check.Type);
            Assert.AreEqual(removeDeceased, jsonRoundTripObj.Config.RemoveDeceased);
            Assert.AreEqual(shareUrl, jsonRoundTripObj.Config.ShareUrl);
            Assert.IsInstanceOfType(check.Config.Sources, typeof(RequestedTypeListSources));
            Assert.IsInstanceOfType(check.Config.MatchingStrategy, typeof(RequestedExactMatchingStrategy));
            Assert.AreEqual(DocScanConstants.TypeList, jsonRoundTripObj.Config.Sources.Type);
            Assert.AreEqual(DocScanConstants.Exact, jsonRoundTripObj.Config.MatchingStrategy.Type);
            CollectionAssert.AreEqual(sourcesTypesList, ((RequestedTypeListSources)jsonRoundTripObj.Config.Sources).Types);
            Assert.IsTrue(((RequestedExactMatchingStrategy)jsonRoundTripObj.Config.MatchingStrategy).ExactMatch);

            Assert.AreEqual(apiKey, ((RequestedWatchlistAdvancedCaConfigCustomAccount)jsonRoundTripObj.Config).ApiKey);
            Assert.AreEqual(monitoring, ((RequestedWatchlistAdvancedCaConfigCustomAccount)jsonRoundTripObj.Config).Monitoring);
            CollectionAssert.AreEqual(tags, ((RequestedWatchlistAdvancedCaConfigCustomAccount)jsonRoundTripObj.Config).Tags);
            Assert.AreEqual(clientRef, ((RequestedWatchlistAdvancedCaConfigCustomAccount)jsonRoundTripObj.Config).ClientRef);
            Assert.AreEqual(DocScanConstants.WithCustomAccount, jsonRoundTripObj.Config.Type);
        }
    }
}