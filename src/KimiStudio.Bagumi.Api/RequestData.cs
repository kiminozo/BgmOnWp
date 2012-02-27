using System;
using System.Collections.Generic;
using System.Text;

namespace KimiStudio.Bagumi.Api
{
    public sealed class RequestData
    {
        private readonly string uri;
        private readonly string data;
        private readonly Dictionary<string, string> parameters;

        public RequestData(string uri, string data)
        {
            this.uri = uri;
            this.data = data;
            parameters = new Dictionary<string, string>();
        }

        public RequestData(string uri)
            : this(uri, null)
        {

        }

        public string Data
        {
            get { return data; }
        }

        public void AddParameter(string key, string value)
        {
            parameters[key] = value;
        }

        public Uri BuildUri()
        {
            var uriBuilder = new StringBuilder(uri, 100);
            if (parameters.Count > 0)
            {
                uriBuilder.Append('?');
                foreach (var item in parameters)
                {
                    uriBuilder.Append(item.Key);
                    uriBuilder.Append('=');
                    uriBuilder.Append(item.Value);
                    uriBuilder.Append('&');
                }
                uriBuilder.Length--;
            }
            return new Uri(uriBuilder.ToString());
        }

        public override string ToString()
        {
            return BuildUri().ToString();
        }
    }
}