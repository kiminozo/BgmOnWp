using System.Globalization;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public class ProgressUpdateEndCommand : Command<DefaultResult>
    {
        private readonly int subjectId;
        private readonly int sort;
        private readonly AuthUser auth;

        public ProgressUpdateEndCommand(int subjectId, int sort, AuthUser auth)
        {
            this.subjectId = subjectId;
            this.sort = sort;
            this.auth = auth;
        }

        private const string Uri = @"http://api.bgm.tv/subject/{0}/update/watched_eps";


        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(string.Format(Uri, subjectId));
            request.AddQueryString("source", ApiKeyNames.Source);
            request.AddBody("watched_eps", sort.ToString(CultureInfo.InvariantCulture));
            request.AddBody("source", ApiKeyNames.Source);
            request.AddBody("auth", auth.AuthEncode);

            return request;
        }
    }
}