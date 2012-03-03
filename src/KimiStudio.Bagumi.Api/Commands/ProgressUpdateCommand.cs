using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public class ProgressUpdateCommand : Command<DefaultResult>
    {
        private readonly ProgressUpdateInfo updateInfo;
        private readonly AuthUser auth;

        public ProgressUpdateCommand(ProgressUpdateInfo updateInfo, AuthUser auth)
        {
            this.updateInfo = updateInfo;
            this.auth = auth;
        }

        private const string Uri = @"http://api.bgm.tv/ep/{0}/status/{1}";

        public ProgressUpdateInfo UpdateInfo
        {
            get { return updateInfo; }
        }

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(string.Format(Uri, UpdateInfo.EpisodeId, UpdateInfo.Method));
            request.AddQueryString("source", ApiKeyNames.Source);
            request.AddBody("source", ApiKeyNames.Source);
            request.AddBody("auth", auth.AuthEncode);

            if (UpdateInfo.Episodes != null && UpdateInfo.Episodes.Count > 0)
            {
                var builder = new StringBuilder();
                foreach (var item in UpdateInfo.Episodes)
                {
                    builder.Append(item);
                    builder.Append("%2C");//,
                }
                builder.Length -= 3;
                request.AddBody("ep%5Fid", builder.ToString());
            }

            return request;
        }
    }
}
