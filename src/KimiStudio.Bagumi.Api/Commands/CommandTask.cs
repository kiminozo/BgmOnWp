using System;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace KimiStudio.Bagumi.Api.Commands
{
    public static class CommandTaskFactory
    {
        public static CommandTask<TResult> Create<TResult>(ICommand<TResult> command) where TResult : class
        {
            return new CommandTask<TResult>(command);
        }
    }

    public sealed class CommandTask<TResult> where TResult : class
    {
        private readonly ICommand<TResult> command;
        private Action<TResult> callBackAction;
        private Action<Exception> exceptionAction;
        private SynchronizationContext synchronization;

        public CommandTask(ICommand<TResult> command)
        {
            this.command = command;
        }

        public void Start()
        {
            synchronization = SynchronizationContext.Current;
            command.BeginExecute(AsyncCallBack, null);
        }

        public void Result(Action<TResult> action)
        {
            callBackAction = action;
        }

        public void Exception(Action<Exception> action)
        {
            exceptionAction = action;
        }

        private void AsyncCallBack(IAsyncResult asyncResult)
        {
            try
            {
                var result = command.EndExecute(asyncResult);
                DoResult(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                DoException(e);
            }
        }

        private void DoResult(TResult result)
        {
            if (callBackAction == null) return;
            if (synchronization == null)
            {
                callBackAction(result);
            }
            else
            {
                synchronization.Post(d => callBackAction(result), null);
            }
        }

        private void DoException(Exception e)
        {
            if (exceptionAction == null) return;
            if (synchronization == null)
            {
                exceptionAction(e);
            }
            else
            {
                synchronization.Post(d => exceptionAction(e), null);
            }
        }
    }
}
