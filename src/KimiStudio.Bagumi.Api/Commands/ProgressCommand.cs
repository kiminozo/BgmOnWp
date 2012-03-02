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
            request.AddParameter("sysuid", auth.Id);
            request.AddParameter("source",ApiKeyNames.Source);
            request.AddParameter("subject%5Fid", subjectId);
            request.AddParameter("sysusername", auth.UserName);
            request.AddParameter("sysbuild", ApiKeyNames.Sysbuild);
            request.AddParameter("auth", auth.AuthEncode);
            return request;
        }
    }
}