using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace KimiStudio.Bangumi.Api
{
    public sealed class RequestData
    {
        private readonly string uri;
        private readonly Dictionary<string, string> queryStrings;
        private readonly Dictionary<string, string> bodys;

        public RequestData(string uri)
        {
            this.uri = uri;
            queryStrings = new Dictionary<string, string>();
            bodys = new Dictionary<string, string>();
        }


        public void AddQueryString(string name, string value)
        {
            queryStrings[name] = value;
        }

        /// <summary>
        /// 添加随机参数，防止缓存
        /// </summary>
        public void AddRandQueryString()
        {
            var rnd = new Random();
            AddQueryString("rand", rnd.NextDouble().ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 添加时间参数，防止缓存
        /// </summary>
        public void AddTimetamp(int seconds = 30)
        {
            var tick = DateTime.Now.Ticks/TimeSpan.FromSeconds(seconds).Ticks;
            AddQueryString("timetamp", tick.ToString(CultureInfo.InvariantCulture));
        }

        public void AddBody(string name, string value)
        {
            bodys[name] = value;
        }

        public Uri BuildUri()
        {
            var uriBuilder = new StringBuilder(uri, 100);
            if (queryStrings.Count > 0)
            {
                uriBuilder.Append('?');
                foreach (var item in queryStrings)
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

        public bool IsPost
        {
            get { return bodys.Count > 0; }
        }

        public string BuildBody()
        {
            var builder = new StringBuilder(100);
            if (bodys.Count > 0)
            {
                foreach (var item in bodys)
                {
                    builder.Append(item.Key);
                    builder.Append('=');
                    builder.Append(item.Value);
                    builder.Append('&');
                }
                builder.Length--;
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            return BuildUri().ToString();
        }
    }
}