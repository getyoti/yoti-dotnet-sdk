namespace Yoti.Auth.ShareUrl.Extensions
{
    public class ExtensionBuilder<T>
    {
        private string _type;
        private T _content;

        public ExtensionBuilder<T> WithType(string type)
        {
            _type = type;
            return this;
        }

        public ExtensionBuilder<T> WithContent(T content)
        {
            _content = content;
            return this;
        }

        public Extension<T> Build()
        {
            return new Extension<T>(_type, _content);
        }
    }
}