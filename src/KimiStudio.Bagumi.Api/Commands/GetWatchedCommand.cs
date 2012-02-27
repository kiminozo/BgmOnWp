using System.Collections.Generic;
using KimiStudio.Bagumi.Api.Models;
using KimiStudio.BgmOnWp.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public sealed class GetWatchedCommand : Command<List<BagumiData>>
    {
        private readonly AuthUser auth;

        public GetWatchedCommand(AuthUser auth)
        {
            this.auth = auth;
        }

        private const string Uri = @"http://api.bgm.tv/user/kiminozo/collection";

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(Uri);
            request.AddParameter("source", "OnAir");
            request.AddParameter("sysbuild", "201107272200");
            request.AddParameter("cat", "watching");
            request.AddParameter("sysuid", auth.Id);
            request.AddParameter("sysusername", auth.UserName);
            request.AddParameter("auth", auth.Auth);
            return request;
        }
    }
}