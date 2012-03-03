using System.Globalization;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public sealed class SearchSubjectCommand : Command<SearchResult>
    {

        private readonly string keyword;
        private readonly int type;
        private readonly int start;
        private readonly int count;

        public SearchSubjectCommand(string keyword, int type = 1, int start = 1, int count = 20)
        {
            this.keyword = keyword;
            this.type = type;
            this.start = start;
            this.count = count;
        }

        private const string Uri = @"http://api.bgm.tv/search/subject/{0}";
        //http://api.bgm.tv/search/subject/{0}?type=1&start=1&max_results=20

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(string.Format(Uri, keyword));

            request.AddQueryString("type", type.ToString(CultureInfo.InvariantCulture));
            request.AddQueryString("start", start.ToString(CultureInfo.InvariantCulture));
            request.AddQueryString("max_results", count.ToString(CultureInfo.InvariantCulture));

            request.AddQueryString("responseGroup", "large");
            request.AddQueryString("source", ApiKeyNames.Source);
            //request.AddQueryString("sysuid", auth.Id);
            //request.AddQueryString("sysusername", auth.UserName);
            //request.AddQueryString("auth", auth.AuthEncode);
            return request;
        }
    }
}