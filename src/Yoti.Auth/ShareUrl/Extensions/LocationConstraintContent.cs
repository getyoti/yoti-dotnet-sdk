using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Extensions
{
    public class LocationConstraintContent
    {
        [JsonProperty(PropertyName = "expected_device_location")]
        private readonly DeviceLocation _expectedDeviceLocation;

        public LocationConstraintContent(double latitude, double longitude, double radius, double maxUncertainty)
        {
            _expectedDeviceLocation = new DeviceLocation(latitude, longitude, radius, maxUncertainty);
        }

        public DeviceLocation ExpectedDeviceLocation
        {
            get
            {
                return _expectedDeviceLocation;
            }
        }
    }
}