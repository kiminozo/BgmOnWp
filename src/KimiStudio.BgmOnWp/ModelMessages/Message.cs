using System;
using System.Collections.Generic;
using KimiStudio.BgmOnWp.Models;

namespace KimiStudio.BgmOnWp.ModelMessages
{
    public abstract class Message
    {
        public bool Cancelled { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SubjectMessage : Message
    {
        public Subject Subject { get; set; }
    }

    public class WatchedsMessage : Message
    {
        public IEnumerable<BagumiData> Watcheds { get; set; }
    }
}
