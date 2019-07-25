using System.Collections.Generic;
using Yoti.Auth.ShareUrl.Extensions;
using Yoti.Auth.ShareUrl.Policy;

namespace Yoti.Auth.ShareUrl
{
    public class DynamicScenarioBuilder
    {
        private string _callbackEndpoint;
        private DynamicPolicy _dynamicPolicy;
        private readonly List<BaseExtension> _extensions = new List<BaseExtension>();

        /// <summary>
        /// The device's callback endpoint. Must be a URL relative to the Application Domain
        /// specified in your Dashboard
        /// </summary>
        /// <param name="callbackEndpoint"></param>
        /// <returns><see cref="DynamicScenarioBuilder"/> with a Callback Endpoint added</returns>
        public DynamicScenarioBuilder WithCallbackEndpoint(string callbackEndpoint)
        {
            _callbackEndpoint = callbackEndpoint;
            return this;
        }

        /// <summary>
        /// The customisable <see cref="DynamicPolicy"/> to use in the share
        /// </summary>
        /// <param name="dynamicPolicy"></param>
        /// <returns><see cref="DynamicScenarioBuilder"/> with a Dynamic Policy added</returns>
        public DynamicScenarioBuilder WithPolicy(DynamicPolicy dynamicPolicy)
        {
            _dynamicPolicy = dynamicPolicy;
            return this;
        }

        /// <summary>
        /// List of <see cref="Extension{T}"/> to be activated for the application
        /// </summary>
        /// <param name="extension"><see cref="Extension{T}"/> to add</param>
        /// <returns><see cref="DynamicScenarioBuilder"/> with an extension added</returns>
        public DynamicScenarioBuilder WithExtension(BaseExtension extension)
        {
            _extensions.Add(extension);
            return this;
        }

        public DynamicScenario Build()
        {
            return new DynamicScenario(_callbackEndpoint, _dynamicPolicy, _extensions);
        }
    }
}