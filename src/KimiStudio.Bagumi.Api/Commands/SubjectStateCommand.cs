using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.Bangumi.Api.Commands
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
            request.AddQueryString("source", ApiKeyNames.Source);
            request.AddQueryString("auth", auth.AuthEncode);
            request.AddRandQueryString();
            return request;
        }
    }
}