using System;
using KimiStudio.BgmOnWp.ModelMessages;

namespace KimiStudio.BgmOnWp.Api
{
    public abstract class ApiCommand<TMessage> where TMessage : Message
    {
        public abstract void Execute(Action<TMessage> callbackHandler);
    }
}