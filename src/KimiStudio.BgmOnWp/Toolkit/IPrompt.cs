using System;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public interface IPrompt
    {
     //   void PromptResult(bool canceled);

        Action HideAction { get; set; }
    }
}