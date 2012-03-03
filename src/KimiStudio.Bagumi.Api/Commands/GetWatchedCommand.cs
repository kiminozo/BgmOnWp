using System.Collections.Generic;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
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
            request.AddQueryString("sysbuild", ApiKeyNames.Sysbuild);
            request.AddQueryString("cat", "watching");
            request.AddQueryString("sysuid", auth.Id);
            request.AddQueryString("sysusername", auth.UserName);
            request.AddQueryString("auth", auth.AuthEncode);
            return request;
        }
    }
}