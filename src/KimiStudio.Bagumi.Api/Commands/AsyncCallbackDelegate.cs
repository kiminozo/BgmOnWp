using System;

namespace KimiStudio.Bangumi.Api.Commands
{
    /// <summary>
    /// 异步回调代理
    /// </summary>
    internal sealed class AsyncCallbackDelegate
    {
        private static readonly AsyncCallbackDelegate Empty = new AsyncCallbackDelegate(null, null);

        /// <summary>
        /// 构造 异步回调代理
        /// </summary>
        /// <param name="callback">需要代理的异步回调</param>
        /// <param name="delegateObject"></param>
        public static AsyncCallbackDelegate Create<T>(AsyncCallback callback, T delegateObject) where T : class
        {
            if (callback == null) return Empty;

            //用AsyncCallbackDelegate包装AsyncCallback
            return new AsyncCallbackDelegate(callback, delegateObject);
        }

        public static T GetDelegateObject<T>(IAsyncResult asyncResult, out IAsyncResult ownedAsyncResult) where T : class
        {
            var asyncResultDelegate = asyncResult as AsyncResultDelegate;//还原AsyncResultDelegate

            if (asyncResultDelegate == null)
            {
                ownedAsyncResult = null;
                return null;
            }
            ownedAsyncResult = asyncResultDelegate.AsyncResult;
            return (T)asyncResultDelegate.Delegate;//获得原始对象
        }

        public AsyncCallback CreateCallback()
        {
            if (this == Empty) return null;
            return AsyncCallBack;
        }

        public IAsyncResult CreateResult(IAsyncResult asyncResult)
        {
            return new AsyncResultDelegate(asyncResult, Delegate);
        }

        /// <summary>
        /// 需要代理的异步回调
        /// </summary>
        private readonly AsyncCallback asyncCallback;

        /// <summary>
        /// 原始调用对象
        /// </summary>
        public object Delegate { get; set; }


        private AsyncCallbackDelegate(AsyncCallback asyncCallback, object delegateObject)
        {
            this.asyncCallback = asyncCallback;
            this.Delegate = delegateObject;
        }

        /// <summary>
        /// 包装后的异步回调
        /// </summary>
        /// <param name="asyncResult"></param>
        private void AsyncCallBack(IAsyncResult asyncResult)
        {
            //用AsyncResultDelegate包装asyncResult
            asyncCallback(CreateResult(asyncResult));//调用原始异步回调
        }

        /// <summary>
        /// 异步状态代理
        /// </summary>
        private sealed class AsyncResultDelegate : IAsyncResult
        {
            /// <summary>
            /// 原始异步状态
            /// </summary>
            private readonly IAsyncResult asyncResult;


            /// <summary>
            /// 原始调用对象
            /// </summary>
            public object Delegate { get; set; }

            public AsyncResultDelegate(IAsyncResult asyncResult, object delegateObject)
            {
                this.asyncResult = asyncResult;
                Delegate = delegateObject;
            }

            #region 装饰模式包装

            public object AsyncState
            {
                get { return AsyncResult.AsyncState; }
            }

            public System.Threading.WaitHandle AsyncWaitHandle
            {
                get { return AsyncResult.AsyncWaitHandle; }
            }

            public bool CompletedSynchronously
            {
                get { return AsyncResult.CompletedSynchronously; }
            }

            public bool IsCompleted
            {
                get { return AsyncResult.IsCompleted; }
            }

            public IAsyncResult AsyncResult
            {
                get { return asyncResult; }
            }
            #endregion
        }
    }
}