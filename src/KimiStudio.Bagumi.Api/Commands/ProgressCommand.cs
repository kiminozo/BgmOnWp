using System.Globalization;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public sealed class ProgressCommand : Command<Progress>
    {
        private readonly string subjectId;
        private readonly AuthUser auth;

        public ProgressCommand(int subjectId, AuthUser auth)
        {
            this.subjectId = subjectId.ToString(CultureInfo.InvariantCulture);
            this.auth = auth;
        }

        private const string Uri = @"http://api.bgm.tv/user/{0}/progress";

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(string.Format(Uri, auth.UserName));
            request.AddQueryString("sysuid", auth.Id);
            request.AddQueryString("source",ApiKeyNames.Source);
            request.AddQueryString("subject%5Fid", subjectId);
            request.AddQueryString("sysusername", auth.UserName);
            request.AddQueryString("sysbuild", ApiKeyNames.Sysbuild);
            request.AddQueryString("auth", auth.AuthEncode);
            return request;
        }
    }
}