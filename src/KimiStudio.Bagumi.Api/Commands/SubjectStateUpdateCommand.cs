using System.Globalization;
using System.Text;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
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


            request.AddBody("sysusername", auth.UserName);
            request.AddBody("source", ApiKeyNames.Source);
            request.AddBody("sysbuild", ApiKeyNames.Sysbuild);
            request.AddBody("auth", auth.AuthEncode);
            request.AddBody("sysuid", auth.Id);

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