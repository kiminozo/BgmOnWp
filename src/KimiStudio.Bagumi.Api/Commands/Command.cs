using System;
using Newtonsoft.Json;

namespace KimiStudio.Bagumi.Api.Commands
{
    public abstract class Command<TResult> where TResult : class
    {
        public IAsyncResult BeginExecute(AsyncCallback asyncCallback, object state)
        {
            var data = CreateRequestData();
            return BeginSend(data, asyncCallback, state);
        }

        private IAsyncResult BeginSend(RequestData data, AsyncCallback asyncCallback, object state)
        {
            var requestDelegate = new HttpRequestDelegate();
            var callBackDelegate = AsyncCallbackDelegate.Create(asyncCallback, requestDelegate);
            var result = requestDelegate.BeginSend(data, callBackDelegate.CreateCallback(), state);
            return callBackDelegate.CreateResult(result);
        }

        protected abstract RequestData CreateRequestData();

        private string EndSend(IAsyncResult asyncResult)
        {
            IAsyncResult ownedAsyncResult;
            var requestDelegate = AsyncCallbackDelegate.GetDelegateObject<HttpRequestDelegate>(asyncResult,out ownedAsyncResult );
            return requestDelegate.EndSend(ownedAsyncResult);
        }

        public TResult EndExecute(IAsyncResult asyncResult)
        {
            var result = EndSend(asyncResult);
            return JsonConvert.DeserializeObject<TResult>(result);
        }

    }
}