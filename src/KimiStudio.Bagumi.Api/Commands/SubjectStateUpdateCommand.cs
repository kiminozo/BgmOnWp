using System.Globalization;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.Bangumi.Api.Commands
{
    public sealed class SubjectStateUpdateCommand : Command<SubjectState>
    {
        private readonly SubjectStateUpdateInfo stateUpdateInfo;
        private readonly AuthUser auth;

        public SubjectStateUpdateCommand(SubjectStateUpdateInfo stateUpdateInfo, AuthUser auth)
        {
            this.stateUpdateInfo = stateUpdateInfo;
            this.auth = auth;
        }

        private const string Uri = @"http://api.bgm.tv/collection/{0}/update";

        #region Overrides of Command<DefaultResult>

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(string.Format(Uri, stateUpdateInfo.SubjectId));
            request.AddQueryString("source", ApiKeyNames.Source);
            
            request.AddBody("source", ApiKeyNames.Source);
            request.AddBody("auth", auth.AuthEncode);

            request.AddBody("status", stateUpdateInfo.Method);

            if (stateUpdateInfo.Comment != null) request.AddBody("comment", stateUpdateInfo.Comment);
            if (stateUpdateInfo.Rating != null)
                request.AddBody("rating", stateUpdateInfo.Rating.Value.ToString(CultureInfo.InvariantCulture));

            var tags = stateUpdateInfo.Tags.GetTags();
            if (!string.IsNullOrEmpty(tags))
            {
                request.AddBody("tags", tags);
            }
            return request;

        }

        #endregion
    }
}