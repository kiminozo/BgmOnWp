using System;

namespace KimiStudio.BgmOnWp.ModelMessages
{
    public abstract class Message
    {
        public bool Cancelled { get; set; }
        public string ErrorMessage { get; set; }
    }
}
