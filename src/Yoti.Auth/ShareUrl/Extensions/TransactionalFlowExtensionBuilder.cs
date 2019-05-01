namespace Yoti.Auth.ShareUrl.Extensions
{
    /// <summary>
    /// Allows you to provide a non-null object representing the content to be submitted in the TRANSACTIONAL_FLOW extension.
    /// The object will be mapped to a Json representation using Jackson ObjectMapper.
    /// </summary>
    /// <typeparam name="T">The type of the content</typeparam>
    public class TransactionalFlowExtensionBuilder<T>
    {
        private T _content;

        public TransactionalFlowExtensionBuilder<T> WithContent(T content)
        {
            Validation.NotNull(content, nameof(content));

            _content = content;
            return this;
        }

        public Extension<T> Build()
        {
            return new Extension<T>(Constants.Extension.TransactionalFlow, _content);
        }
    }
}