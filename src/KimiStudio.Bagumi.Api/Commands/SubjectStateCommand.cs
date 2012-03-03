using System.Globalization;
using System.Text;
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
            if (stateUpdateInfo.Tags != null && stateUpdateInfo.Tags.Length > 0)
            {
                var builder = new StringBuilder();
                foreach (var item in stateUpdateInfo.Tags)
                {
                    builder.Append(item);
                    builder.Append(' ');
                }
                builder.Length--;
                request.AddBody("tags", builder.ToString());
            }



            return request;

        }

        #endregion
    }
}