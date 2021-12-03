using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Create.Check;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass] 
    public class RequestedWatchlistScreeningCheckBuilderTests
    {
        [TestMethod]
        public void ShouldBuild()
        {
            RequestedWatchlistScreeningCheck check =
              new RequestedWatchlistScreeningCheckBuilder()
              .Build();

            Assert.AreEqual(DocScanConstants.WatchlistScreening, check.Type);
            Assert.IsNotNull(check.Config);
        }

        [TestMethod]
        public void ShouldBuildForSanctions()
        {
            RequestedWatchlistScreeningCheck check =
              new RequestedWatchlistScreeningCheckBuilder()
              .ForSanctions()
              .Build();

            Assert.AreEqual(DocScanConstants.WatchlistScreening, check.Type);
            Assert.AreEqual(DocScanConstants.Sanctions, check.Config.Categories.First());
        }

        [TestMethod]
        public void ShouldBuildForAdverseMedia()
        {
            RequestedWatchlistScreeningCheck check =
              new RequestedWatchlistScreeningCheckBuilder()
              .ForAdverseMedia()
              .Build();

            Assert.AreEqual(DocScanConstants.WatchlistScreening, check.Type);
            Assert.AreEqual(DocScanConstants.AdverseMedia, check.Config.Categories.First());
        }

        [TestMethod]
        public void ShouldBuildWithCategory()
        {
            string withCategory = "someCategory";

            RequestedWatchlistScreeningCheck check =
              new RequestedWatchlistScreeningCheckBuilder()
              .WithCategory(withCategory)
              .Build();

            Assert.AreEqual(DocScanConstants.WatchlistScreening, check.Type);
            Assert.AreEqual(withCategory, check.Config.Categories.First());
        }

        [TestMethod]
        public void ShouldNotBuildWithNullCategory()
        {
            string withCategory = null;

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                RequestedWatchlistScreeningCheck check =
                  new RequestedWatchlistScreeningCheckBuilder()
                  .WithCategory(withCategory)
                  .Build();
            });
        }

        [TestMethod]
        public void ShouldNotBuildWithEmptyStringCategory()
        {
            string withCategory = "";

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                RequestedWatchlistScreeningCheck check =
                  new RequestedWatchlistScreeningCheckBuilder()
                  .WithCategory(withCategory)
                  .Build();
            });
        }

        [TestMethod]
        public void ShouldNotBuildWithWhitespaceStringCategory()
        {
            string withCategory = "     ";

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                RequestedWatchlistScreeningCheck check =
                  new RequestedWatchlistScreeningCheckBuilder()
                  .WithCategory(withCategory)
                  .Build();
            });
        }

        [TestMethod]
        public void ShouldBuildWithCategoryUnknownToTheSdk()
        {
            string withCategory = "SOME_UNKNOWN_CATEGORY";

            RequestedWatchlistScreeningCheck check =
              new RequestedWatchlistScreeningCheckBuilder()
              .WithCategory(withCategory)
              .Build();

            Assert.AreEqual(DocScanConstants.WatchlistScreening, check.Type);
            Assert.AreEqual(withCategory, check.Config.Categories.First());
        }

        [TestMethod]
        public void ShouldBuildWithMultipleCategories()
        {
            RequestedWatchlistScreeningCheck check =
            new RequestedWatchlistScreeningCheckBuilder()
                .ForAdverseMedia()
                .ForSanctions()
                .Build();

            Assert.AreEqual(DocScanConstants.WatchlistScreening, check.Type);
            Assert.IsTrue(check.Config.Categories.Contains(DocScanConstants.AdverseMedia));
            Assert.IsTrue(check.Config.Categories.Contains(DocScanConstants.Sanctions));
        }

        [TestMethod]
        public void ShouldBuildWithCategoryAddedOnceDespiteMultipleCalls()
        {
            RequestedWatchlistScreeningCheck check =
            new RequestedWatchlistScreeningCheckBuilder()
                .ForAdverseMedia()
                .ForAdverseMedia()
                .Build();

            Assert.AreEqual(DocScanConstants.WatchlistScreening, check.Type);
            Assert.IsTrue(check.Config.Categories.Contains(DocScanConstants.AdverseMedia));
            Assert.AreEqual(1, check.Config.Categories.Count);
        }
    }
}