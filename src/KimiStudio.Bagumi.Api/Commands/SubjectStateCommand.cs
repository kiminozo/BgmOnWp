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
            request.AddQueryString("sysuid", auth.Id);
            request.AddQueryString("source", ApiKeyNames.Source);
            request.AddQueryString("sysusername", auth.UserName);
            request.AddQueryString("sysbuild", ApiKeyNames.Sysbuild);
            request.AddQueryString("auth", auth.AuthEncode);
            return request;
        }
    }
}