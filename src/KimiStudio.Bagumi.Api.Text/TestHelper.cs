using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Text
{
    public static class TestHelper
    {
        public static AuthUser Login()
        {
            var command = new LoginCommand("piova", "piova@live.com");
            return command.Execute();
        }

        public static TResult Execute<TResult>(this Command<TResult> command) where TResult : class
        {
            var resetEvent = new ManualResetEvent(false);
            ////var command = new CalendarCommand(auth);
            //Exception err = null;
            //TResult result = null;
            IAsyncResult async = null;
            command.BeginExecute(x =>
                                     {
                                         async = x;
                                         resetEvent.Set();
                                         //try
                                         //{
                                         //    result = command.EndExecute(x);
                                         //}
                                         //catch (Exception e)
                                         //{
                                         //    err = e;
                                         //}
                                         //finally
                                         //{
                                         //    resetEvent.Set();
                                         //}
                                     }, null);
            resetEvent.WaitOne();
            resetEvent.Dispose();
            return command.EndExecute(async);
        }
    }
}
