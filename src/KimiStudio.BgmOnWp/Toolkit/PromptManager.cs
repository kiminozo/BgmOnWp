using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using KimiStudio.Controls;
using Action = System.Action;

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
            var popupPrompt = CreateMessagePrompt(settings);
            var view = ViewLocator.LocateForModel(rootModel, popupPrompt, context);

            var displayName = rootModel as IHaveDisplayName;
            if (displayName != null)
            {
                popupPrompt.Title = displayName.DisplayName;
            }
            popupPrompt.Content = view;
            ViewModelBinder.Bind(rootModel, popupPrompt, null);
            var activatable = rootModel as IActivate;
            if (activatable != null)
            {
                activatable.Activate();
            }

            var resultPrompt = rootModel as IPrompt;
            if (resultPrompt != null)
            {
                popupPrompt.Completed += (sender, args) => resultPrompt.PromptResult(popupPrompt.Result != true);
            }

            var deactivator = rootModel as IDeactivate;
            if (deactivator != null)
            {
                popupPrompt.Completed += (sender, args) => deactivator.Deactivate(true);
            }

            ViewLocator.LocateForModel(rootModel, popupPrompt, null);
            popupPrompt.Show();
        }

        private PopupPrompt CreateMessagePrompt(IEnumerable<KeyValuePair<string, object>> settings)
        {
            var messagePrompt = new PopupPrompt();
            messagePrompt.Overlay = new SolidColorBrush(Color.FromArgb(200, 100, 149, 237));
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
            var toastPrompt = new ToastPrompt { AllowDrop = false };
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

                    if (propertyInfo != null && pair.Value != null)
                        propertyInfo.SetValue(target, pair.Value, null);
                }

                return true;
            }
            return false;
        }
    }


}