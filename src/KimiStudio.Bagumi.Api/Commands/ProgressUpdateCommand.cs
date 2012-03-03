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
        //POST /ep/1807/status/watched?source=onAir
        private readonly int episodeId;
        private readonly IList<int> episodes;
        private readonly string method;
        private readonly AuthUser auth;

        public static ProgressUpdateCommand Watched(int episodeId, AuthUser auth)
        {
            return new ProgressUpdateCommand(episodeId, "watched", auth);
        }

        public static ProgressUpdateCommand Watched(int episodeId, IList<int> episodes, AuthUser auth)
        {
            return new ProgressUpdateCommand(episodeId, "watched", auth, episodes);
        }

        public static ProgressUpdateCommand Queue(int episodeId, AuthUser auth)
        {
            return new ProgressUpdateCommand(episodeId, "queue", auth);
        }

        public static ProgressUpdateCommand Drop(int episodeId, AuthUser auth)
        {
            return new ProgressUpdateCommand(episodeId, "drop", auth);
        }

        public static ProgressUpdateCommand Remove(int episodeId, AuthUser auth)
        {
            return new ProgressUpdateCommand(episodeId, "remove", auth);
        }

        private ProgressUpdateCommand(int episodeId, string method, AuthUser auth, IList<int> episodes = null)
        {
            this.episodeId = episodeId;
            this.method = method;
            this.auth = auth;
            this.episodes = episodes;
        }

        private const string Uri = @"http://api.bgm.tv/ep/{0}/status/{1}";

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(string.Format(Uri, episodeId, method));
            request.AddQueryString("source", ApiKeyNames.Source);

            request.AddBody("sysusername", auth.UserName);
            request.AddBody("source", ApiKeyNames.Source);
            request.AddBody("sysbuild", ApiKeyNames.Sysbuild);
            request.AddBody("auth", auth.AuthEncode);
            request.AddBody("sysuid", auth.Id);

            if (episodes != null && episodes.Count > 0)
            {
                var builder = new StringBuilder();
                foreach (var item in episodes)
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
