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
            request.AddParameter("source",ApiKeyNames.Source);
            request.AddParameter("sysbuild", ApiKeyNames.Sysbuild);
            request.AddParameter("cat", "watching");
            request.AddParameter("sysuid", auth.Id);
            request.AddParameter("sysusername", auth.UserName);
            request.AddParameter("auth", auth.AuthEncode);
            return request;
        }
    }
}