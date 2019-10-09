using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Extensions
{
    /// <summary>
    /// Type and content of a feature for an application. Implemented <see cref="BaseExtension"/>,
    /// and adds generic content on top
    /// </summary>
    /// <typeparam name="T">Type of the extension's content</typeparam>
    public class Extension<T> : BaseExtension
    {
        [JsonProperty(PropertyName = "content")]
        private readonly T _content;

        public Extension(string type, T content) : base(type)
        {
            _content = content;
        }

        /// <summary>
        /// Get the feature's details
        /// </summary>
        /// <returns>The payload of the operation</returns>
        [JsonIgnore]
        public T Content
        {
            get
            {
                return _content;
            }
        }
    }
}