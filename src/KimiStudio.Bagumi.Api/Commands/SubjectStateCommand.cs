using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public sealed class SubjectStateCommand : Command<SubjectState>
    {
        private readonly AuthUser auth;
        private readonly int subjectId;

        public SubjectStateCommand(int subjectId, AuthUser auth)
        {
            this.auth = auth;
            this.subjectId = subjectId;
        }

        private const string Uri = @"http://api.bgm.tv/collection/{0}";

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(string.Format(Uri, subjectId));
            request.AddParameter("sysuid", auth.Id);
            request.AddParameter("source", ApiKeyNames.Source);
            request.AddParameter("sysusername", auth.UserName);
            request.AddParameter("sysbuild", ApiKeyNames.Sysbuild);
            request.AddParameter("auth", auth.AuthEncode);
            return request;
        }
    }
}