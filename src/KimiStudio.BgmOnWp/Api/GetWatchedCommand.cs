using System;
using System.Collections.Generic;
using KimiStudio.BgmOnWp.ModelMessages;
using KimiStudio.BgmOnWp.Models;
using Newtonsoft.Json;

namespace KimiStudio.BgmOnWp.Api
{
    public sealed class GetWatchedCommand : ApiCommand<WatchedsMessage>
    {
        private const string Uri = @"http://api.bgm.tv/user/kiminozo/collection";
        #region Overrides of ApiCommand

        public override void Execute(Action<WatchedsMessage> callbackHandler)
        {
            var authData = BagumiService.Auth;
            var request = new RequestData(Uri);
            request.AddParameter("source", "OnAir");
            request.AddParameter("sysbuild", "201107272200");
            request.AddParameter("cat", "watching");
            request.AddParameter("sysuid", authData.Id);
            request.AddParameter("sysusername", authData.UserName);
            request.AddParameter("auth", authData.Auth);

            var executor = new HttpGetExecutor();
            executor.ExecuteCompleted += args => callbackHandler(ToMessage(args));
            executor.ExecuteAsync(request);
        }

        #endregion

        private WatchedsMessage ToMessage(ExecuteCompletedEventArgs args)
        {
            if (args.Cancelled) return new WatchedsMessage { Cancelled = true, ErrorMessage = args.Error.ToString() };
            return new WatchedsMessage
                       {
                           Watcheds = JsonConvert.DeserializeObject<List<BagumiData>>(args.Result),
                       };
        }
    }
}