﻿using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Extensions
{
    public class LocationConstraintContent
    {
        [JsonProperty(PropertyName = "expected_device_location")]
        private readonly DeviceLocation _expectedDeviceLocation;

        public LocationConstraintContent(double latitude, double longitude, double radius, double maxUncertainty)
        {
            _expectedDeviceLocation = new DeviceLocation(latitude, longitude, radius, maxUncertainty);
        }

        [JsonIgnore]
        public DeviceLocation ExpectedDeviceLocation
        {
            get
            {
                return _expectedDeviceLocation;
            }
        }
    }
}