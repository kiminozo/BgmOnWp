using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using Caliburn.Micro;
using Coding4Fun.Phone.Controls;
using ToastPrompt = KimiStudio.Controls.ToastPrompt;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public sealed class PromptManager : IPromptManager
    {
        private readonly Dispatcher dispatcher;

        public PromptManager(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        #region ShowPopup
        public void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(() => ShowPopup(rootModel, context, settings));
                return;
            }
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
                                                   if (deactivator != null) deactivator.Deactivate(true);
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

        #endregion

        #region Toast
        public void ShowToast(string message, string title = null, IDictionary<string, object> settings = null)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(() => ShowToast(message, title, settings));
                return;
            }
            var toast = CreateToastPrompt(settings);
            toast.Message = message;
            toast.Title = title;
            toast.Show();
        }

        private ToastPrompt CreateToastPrompt(IEnumerable<KeyValuePair<string, object>> settings)
        {
            var toastPrompt = new ToastPrompt {AllowDrop = false};
            ApplySettings(toastPrompt, settings);
            return toastPrompt;
        }

        #endregion

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