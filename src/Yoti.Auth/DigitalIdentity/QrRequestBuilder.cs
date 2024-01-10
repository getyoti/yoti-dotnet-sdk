using System.Collections.Generic;
using Yoti.Auth.DigitalIdentity.Extensions;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.DigitalIdentity
{
    public class QrRequestBuilder
    {
        private string _transport = "";
        private string _displayMode = "";

        /// <summary>
        /// Transport property. Optional - default is 'INLINE'
        /// </summary>
        /// <param name="transport"></param>
        /// <returns><see cref="QrRequestBuilder"/> with a Transport added</returns>
        public QrRequestBuilder WithTransport(string transport)
        {
            _transport = transport;
            return this;
        }

        /// <summary>
        /// DisplayMode property. Optional - default is 'QR_CODE'
        /// </summary>
        /// <param name="displayMode"></param>
        /// <returns><see cref="QrRequestBuilder"/> with a Display Mode added</returns>
        public QrRequestBuilder WithDisplayMode(string displayMode)
        {
            _displayMode = displayMode ;
            return this;
        }

        public QrRequest Build()
        {
            return new QrRequest(_transport,_displayMode);
        }
    }
}
