namespace Yoti.Auth.ShareUrl.Policy
{
    public class WantedAnchorBuilder
    {
        private string _name;
        private string _subType;

        /// <summary>
        /// WithValue sets the anchor's name
        /// </summary>
        /// <param name="name">Anchor name</param>
        public WantedAnchorBuilder WithValue(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// WithSubType sets the anchor's sub-type
        /// </summary>
        /// <param name="subType">Anchor sub-type</param>
        public WantedAnchorBuilder WithSubType(string subType)
        {
            _subType = subType;
            return this;
        }

        /// <summary>
        /// Builds the WantedAnchor
        /// </summary>
        /// <returns><see cref="WantedAnchor"/></returns>
        public WantedAnchor Build()
        {
            return new WantedAnchor(_name, _subType);
        }
    }
}