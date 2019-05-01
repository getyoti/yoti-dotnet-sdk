using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Extensions
{
    public class DeviceLocation
    {
        [JsonProperty(PropertyName = "latitude")]
        private readonly double _latitude;

        [JsonProperty(PropertyName = "longitude")]
        private readonly double _longitude;

        [JsonProperty(PropertyName = "radius")]
        private readonly double _radius;

        [JsonProperty(PropertyName = "max_uncertainty_radius")]
        private readonly double _maxUncertainty;

        public DeviceLocation(double latitude, double longitude, double radius, double maxUncertainty)
        {
            _latitude = latitude;
            _longitude = longitude;
            _radius = radius;
            _maxUncertainty = maxUncertainty;
        }

        public double Latitude
        {
            get
            {
                return _latitude;
            }
        }

        public double Longitude
        {
            get
            {
                return _longitude;
            }
        }

        public double Radius
        {
            get
            {
                return _radius;
            }
        }

        public double MaxUncertainty
        {
            get
            {
                return _maxUncertainty;
            }
        }
    }
}