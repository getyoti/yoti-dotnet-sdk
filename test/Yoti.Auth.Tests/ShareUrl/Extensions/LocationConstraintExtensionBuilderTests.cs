using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.ShareUrl.Extensions;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Yoti.Auth.Tests.ShareUrl.Extensions
{
    [TestClass]
    public class LocationConstraintExtensionBuilderTests
    {
        private const double _someLatitude = 1d;
        private const double _someLongitude = 2d;
        private const double _someRadius = 3d;
        private const double _someUncertainty = 4d;

        [DataTestMethod]
        [DataRow(-91)]
        [DataRow(91)]
        [TestMethod]
        public void ShouldFailForLatitudesOutsideOfRange(double latitude)
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new LocationConstraintExtensionBuilder()
                .WithLatitude(latitude)
                .Build();
            });
        }

        [DataTestMethod]
        [DataRow(-181)]
        [DataRow(181)]
        [TestMethod]
        public void ShouldFailForLongitudesOutsideOfRange(double longitude)
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new LocationConstraintExtensionBuilder()
                .WithLongitude(longitude)
                .Build();
            });
        }

        [TestMethod]
        public void ShouldFailForRadiusLessThanZero()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new LocationConstraintExtensionBuilder()
                .WithRadius(-1)
                .Build();
            });
        }

        [TestMethod]
        public void ShouldFailForUncertaintyLessThanZero()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new LocationConstraintExtensionBuilder()
                .WithMaxUncertainty(-1)
                .Build();
            });
        }

        [TestMethod]
        public void ShouldBuildLocationConstraintWithGivenValues()
        {
            Extension<LocationConstraintContent> extension = new LocationConstraintExtensionBuilder()
                .WithLatitude(_someLatitude)
                .WithLongitude(_someLongitude)
                .WithRadius(_someRadius)
                .WithMaxUncertainty(_someUncertainty)
                .Build();

            Assert.AreEqual(Constants.Extension.LocationConstraint, extension.ExtensionType);
            DeviceLocation deviceLocation = extension.Content.ExpectedDeviceLocation;
            Assert.AreEqual(_someLatitude, deviceLocation.Latitude);
            Assert.AreEqual(_someLongitude, deviceLocation.Longitude);
            Assert.AreEqual(_someRadius, deviceLocation.Radius);
            Assert.AreEqual(_someUncertainty, deviceLocation.MaxUncertainty);
        }

        [TestMethod]
        public void ShouldBuildLocationConstraintWithDefaultValues()
        {
            Extension<LocationConstraintContent> extension = new LocationConstraintExtensionBuilder()
                .WithLatitude(_someLatitude)
                .WithLongitude(_someLongitude)
                .Build();

            Assert.AreEqual(Constants.Extension.LocationConstraint, extension.ExtensionType);
            DeviceLocation deviceLocation = extension.Content.ExpectedDeviceLocation;
            Assert.AreEqual(_someLatitude, deviceLocation.Latitude);
            Assert.AreEqual(_someLongitude, deviceLocation.Longitude);
            Assert.AreEqual(150d, deviceLocation.Radius);
            Assert.AreEqual(150d, deviceLocation.MaxUncertainty);
        }
    }
}