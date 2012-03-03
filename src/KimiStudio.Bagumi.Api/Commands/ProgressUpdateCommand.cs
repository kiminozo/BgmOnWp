using System;
using System.Collections.Generic;
using System.Globalization;
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

            request.AddBody("sysusername", auth.UserName);
            request.AddBody("source", ApiKeyNames.Source);
            request.AddBody("sysbuild", ApiKeyNames.Sysbuild);
            request.AddBody("auth", auth.AuthEncode);
            request.AddBody("sysuid", auth.Id);

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

    //public class ProgressUpdateEndCommand : Command<DefaultResult>
    //{
    //    private readonly int subjectId;
    //    private readonly int sort;
    //    private readonly AuthUser auth;

    //    public ProgressUpdateEndCommand(int subjectId, int sort, AuthUser auth)
    //    {
    //        this.subjectId = subjectId;
    //        this.sort = sort;
    //        this.auth = auth;
    //    }

    //    private const string Uri = @"http://api.bgm.tv/subject/{0}/update/watched_eps";


    //    protected override RequestData CreateRequestData()
    //    {
    //        var request = new RequestData(string.Format(Uri, subjectId));
    //        request.AddParameter("source", ApiKeyNames.Source);

    //        request.AddBody("");
    //        request.AddBody("watchedeps", sort.ToString(CultureInfo.InvariantCulture));
    //        request.AddBody("sysuid", auth.Id);
    //        request.AddBody("sysusername", auth.UserName);
    //        request.AddBody("sysbuild", ApiKeyNames.Sysbuild);
    //        request.AddBody("auth", auth.AuthEncode);

    //        return request;
    //    }
    //}
}
