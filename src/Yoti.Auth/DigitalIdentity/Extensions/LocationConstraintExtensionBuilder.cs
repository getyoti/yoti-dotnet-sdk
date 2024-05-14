namespace Yoti.Auth.DigitalIdentity.Extensions
{
    public class LocationConstraintExtensionBuilder
    {
        private double _latitude;
        private double _longitude;
        private double _radius = 150d;
        private double _maxUncertainty = 150d;

        /// <summary>
        /// Allows you to specify the Latitude of the user's expected location.
        /// </summary>
        /// <param name="latitude"></param>
        /// <returns>This LocationConstraintExtensionBuilder</returns>
        public LocationConstraintExtensionBuilder WithLatitude(double latitude)
        {
            Validation.WithinRange(latitude, -90d, 90d, nameof(latitude));
            _latitude = latitude;
            return this;
        }

        /// <summary>
        /// Allows you to specify the Longitude of the user's expected location.
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns>This LocationConstraintExtensionBuilder</returns>
        public LocationConstraintExtensionBuilder WithLongitude(double longitude)
        {
            Validation.WithinRange(longitude, -180d, 180d, nameof(longitude));
            _longitude = longitude;
            return this;
        }

        /// <summary>
        /// Radius of the circle, centred on the specified location coordinates, where the device is
        /// allowed to perform the share. If not provided, a default value of 150m will be used.
        /// </summary>
        /// <param name="radius">The allowable distance, in metres, from the given lat/long location</param>
        /// <returns>This LocationConstraintExtensionBuilder</returns>
        public LocationConstraintExtensionBuilder WithRadius(double radius)
        {
            Validation.NotLessThan(radius, 0d, nameof(radius));
            _radius = radius;
            return this;
        }

        /// <summary>
        /// Maximum acceptable distance, in metres, of the area of uncertainty associated with the
        /// device location coordinates. If not provided, a default value of 150m will be used.
        /// </summary>
        /// <param name="maxUncertainty">Maximum allowed measurement uncertainty, in metres</param>
        /// <returns>This LocationConstraintExtensionBuilder</returns>
        public LocationConstraintExtensionBuilder WithMaxUncertainty(double maxUncertainty)
        {
            Validation.NotLessThan(maxUncertainty, 0d, nameof(maxUncertainty));
            _maxUncertainty = maxUncertainty;
            return this;
        }

        public Extension<LocationConstraintContent> Build()
        {
            LocationConstraintContent content = new LocationConstraintContent(_latitude, _longitude, _radius, _maxUncertainty);
            return new Extension<LocationConstraintContent>(Constants.Extension.LocationConstraint, content);
        }
    }
}