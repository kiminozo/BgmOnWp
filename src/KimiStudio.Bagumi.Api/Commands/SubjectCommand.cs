﻿using System;
using System.Collections.Generic;
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
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public class SubjectCommand : Command
    {
        private readonly int subjectId;
        private readonly AuthUser auth;

        private class StateObj
        {
            public GetSubjectCommand GetSubjectCommand;
            public SubjectStateCommand SubjectStateCommand;
            public ProgressCommand ProgressCommand;
            public Countdown Countdown;
            public AsyncCallback Callback;
            public object State;
            public Exception Exception;

            public readonly SubjectResult Result = new SubjectResult();
        }

        private class StateAsyncResult : IAsyncResult
        {
            public StateObj StateObj { get; set; }

            public object AsyncState { get; set; }

            public WaitHandle AsyncWaitHandle
            {
                get { return null; }
            }

            public bool CompletedSynchronously
            {
                get { return true; }
            }

            public bool IsCompleted
            {
                get { return true; }
            }
        }

        public SubjectCommand(int subjectId, AuthUser auth)
        {
            this.subjectId = subjectId;
            this.auth = auth;
            ;
        }

        private void WaitCallBack(object state)
        {
            var stateObj = state as StateObj;
            if (stateObj == null) return;
            stateObj.Countdown.Wait();
            if (stateObj.Callback == null) return;
            stateObj.Callback(new StateAsyncResult {StateObj = stateObj, AsyncState = stateObj.State});
        }

        public void BeginExecute(AsyncCallback asyncCallback, object state)
        {
            var stateObj = new StateObj
                               {
                                   GetSubjectCommand = new GetSubjectCommand(subjectId, auth),
                                   SubjectStateCommand = new SubjectStateCommand(subjectId, auth),
                                   ProgressCommand = new ProgressCommand(subjectId, auth),
                                   Countdown = new Countdown(3),
                                   Callback = asyncCallback,
                                   State = state
                               };
            ThreadPool.QueueUserWorkItem(WaitCallBack, stateObj);
            //var data = CreateRequestData();
            //BeginSend(data, asyncCallback, state);
            stateObj.GetSubjectCommand.BeginExecute(GetSubjectCallBack, stateObj);
            stateObj.SubjectStateCommand.BeginExecute(SubjectStateCallBack, stateObj);
            stateObj.ProgressCommand.BeginExecute(ProgressCallBack, stateObj);
        }

        private void GetSubjectCallBack(IAsyncResult asyncResult)
        {
            var stateObj = (StateObj)asyncResult.AsyncState;

            try
            {
                stateObj.Result.Subject = stateObj.GetSubjectCommand.EndExecute(asyncResult);
                stateObj.Countdown.Signal();
            }
            catch (Exception e)
            {
                stateObj.Exception = e;
                stateObj.Countdown.Free();
            }

        }

        private void SubjectStateCallBack(IAsyncResult asyncResult)
        {
            var stateObj = (StateObj)asyncResult.AsyncState;

            try
            {
                stateObj.Result.SubjectState = stateObj.SubjectStateCommand.EndExecute(asyncResult);
                stateObj.Countdown.Signal();
            }
            catch (Exception e)
            {
                stateObj.Exception = e;
                stateObj.Countdown.Free();
            }
        }

        private void ProgressCallBack(IAsyncResult asyncResult)
        {
            var stateObj = (StateObj)asyncResult.AsyncState;

            try
            {
                stateObj.Result.Progress = stateObj.ProgressCommand.EndExecute(asyncResult);
                stateObj.Countdown.Signal();
            }
            catch (Exception e)
            {
                stateObj.Exception = e;
                stateObj.Countdown.Free();
            }
        }


        public SubjectResult EndExecute(IAsyncResult asyncResult)
        {
            var stateAsyncResult = asyncResult as StateAsyncResult;
            if (stateAsyncResult == null) return null;
            return stateAsyncResult.StateObj.Result;
        }
    }

    public class SubjectResult
    {
        public Subject Subject;
        public SubjectState SubjectState;
        public Progress Progress;
    }
}
