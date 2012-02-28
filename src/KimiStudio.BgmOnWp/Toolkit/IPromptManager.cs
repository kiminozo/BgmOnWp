using System.Collections.Generic;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public interface IPromptManager
    {
        void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null);
    }
}