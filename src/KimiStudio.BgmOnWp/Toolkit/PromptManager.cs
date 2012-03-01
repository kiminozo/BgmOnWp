using System;
using System.Collections.Generic;
using System.Diagnostics;
using Caliburn.Micro;
using Coding4Fun.Phone.Controls;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public sealed class PromptManager : IPromptManager
    {
        public void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {

            var messagePrompt = CreateMessagePrompt(settings);
            var view = ViewLocator.LocateForModel(rootModel, messagePrompt, context);

            var displayName = rootModel as IHaveDisplayName;
            if (displayName != null)
            {
                messagePrompt.Title = displayName.DisplayName;
            }
            messagePrompt.Body = view;
            ViewModelBinder.Bind(rootModel, messagePrompt, null);
            var activatable = rootModel as IActivate;
            if (activatable != null)
            {
                activatable.Activate();
            }
            var deactivator = rootModel as IDeactivate;
            var resultPrompt = rootModel as IPrompt;
            if (deactivator != null || resultPrompt != null)
            {
                messagePrompt.Completed += (sender, args) =>
                                               {
                                                   if (deactivator != null)deactivator.Deactivate(true);
                                                   if (resultPrompt != null)
                                                       resultPrompt.PromptResult(args.PopUpResult != PopUpResult.Ok);
                                               };
            }
            ViewLocator.LocateForModel(rootModel, messagePrompt, null);
            messagePrompt.Show();
        }

        private MessagePrompt CreateMessagePrompt(IEnumerable<KeyValuePair<string, object>> settings)
        {
            var messagePrompt = new MessagePrompt();
            ApplySettings(messagePrompt, settings);
            return messagePrompt;
        }

        private bool ApplySettings(object target, IEnumerable<KeyValuePair<string, object>> settings)
        {
            if (settings != null)
            {
                var type = target.GetType();

                foreach (var pair in settings)
                {
                    var propertyInfo = type.GetProperty(pair.Key);

                    if (propertyInfo != null)
                        propertyInfo.SetValue(target, pair.Value, null);
                }

                return true;
            }
            return false;
        }
    }

    
}