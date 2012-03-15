using System.Collections.Generic;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.Bangumi.Api.Commands
{
    public sealed class GetWatchedCommand : Command<List<BagumiData>>
    {
        private readonly AuthUser auth;

        public GetWatchedCommand(AuthUser auth)
        {
            this.auth = auth;
        }

        private const string Uri = @"http://api.bgm.tv/user/{0}/collection";

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(string.Format(Uri, auth.UserName));
            request.AddQueryString("source",ApiKeyNames.Source);
            request.AddQueryString("cat", "watching");
            request.AddQueryString("auth", auth.AuthEncode);
            request.AddTimetamp();
            return request;
        }

        protected override List<BagumiData> ValidateResult(List<BagumiData> result)
        {
            return result ?? new List<BagumiData>(0);
        }
    }
}