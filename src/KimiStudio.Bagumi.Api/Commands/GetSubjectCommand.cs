using System.Collections.Generic;
using System.Globalization;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public sealed class GetSubjectCommand : Command<Subject>
    {
        private readonly string subjectId;
        private readonly AuthUser auth;
        private const string Uri = @"http://api.bgm.tv/subject/";

        public GetSubjectCommand(int subjectId, AuthUser auth)
        {
            this.auth = auth;
            this.subjectId = subjectId.ToString(CultureInfo.InvariantCulture);
        }

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(Uri + subjectId);
            request.AddQueryString("responseGroup", "large");
            request.AddQueryString("source", ApiKeyNames.Source);
            request.AddQueryString("sysbuild", ApiKeyNames.Sysbuild);
            request.AddQueryString("sysuid", auth.Id);
            request.AddQueryString("sysusername", auth.UserName);
            request.AddQueryString("auth", auth.AuthEncode);
            return request;
        }
    }
}