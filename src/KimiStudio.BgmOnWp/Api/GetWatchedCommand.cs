using System;
using System.Collections.Generic;
using System.Globalization;
using KimiStudio.BgmOnWp.Models;
using Newtonsoft.Json;

namespace KimiStudio.BgmOnWp.Api
{
    public sealed class GetWatchedCommand:ApiCommand
    {
        private readonly Action<IList<BagumiData>> action;
        private const string Uri = @"http://api.bgm.tv/user/kiminozo/collection";
        #region Overrides of ApiCommand

        public GetWatchedCommand(Action<IList<BagumiData>> action)
        {
            this.action = action;
        }

        public override void Execute()
        {
            var authData = BagumiService.Auth;
            var request = new RequestData(Uri);
            request.AddParameter("source", "OnAir");
            request.AddParameter("sysbuild", "201107272200");
            request.AddParameter("cat", "watching");
            request.AddParameter("sysuid", authData.Id);
            request.AddParameter("sysusername", authData.UserName);
            request.AddParameter("auth", authData.Auth);

            var executor = new GetExecutor(CallBackPost);
            executor.Execute(request);
        }

        #endregion

        private void CallBackPost(string result)
        {
            IList<BagumiData> list = JsonConvert.DeserializeObject<List<BagumiData>>(result);
            action(list);
        }
    }

    public sealed class GetSubjectCommand : ApiCommand
    {
        private readonly string subjectId;
        private readonly Action<Subject> action;
        private const string Uri = @"http://api.bgm.tv/subject/";
        #region Overrides of ApiCommand

        public GetSubjectCommand(int subjectId, Action<Subject> action)
        {
            this.subjectId = subjectId.ToString(CultureInfo.InvariantCulture);
            this.action = action;
        }

        public override void Execute()
        {
            var authData = BagumiService.Auth;
            var request = new RequestData(Uri + subjectId);
            request.AddParameter("responseGroup", "large");
            request.AddParameter("source", "OnAir");
            request.AddParameter("sysbuild", "201107272200");
            request.AddParameter("sysuid", authData.Id);
            request.AddParameter("sysusername", authData.UserName);
            request.AddParameter("auth", authData.Auth);

            var executor = new GetExecutor(CallBackPost);
            executor.Execute(request);
        }

        #endregion

        private void CallBackPost(string result)
        {
            var subject = JsonConvert.DeserializeObject<Subject>(result);
            action(subject);
        }
    }
}