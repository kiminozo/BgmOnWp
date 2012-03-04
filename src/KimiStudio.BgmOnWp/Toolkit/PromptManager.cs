using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using KimiStudio.Controls;

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
        public void ShowPopup(object rootModel, object context = null, IDictionary<string, Func<object>> settings = null)
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
            messagePrompt.Content = view;
            ViewModelBinder.Bind(rootModel, messagePrompt, null);
            var activatable = rootModel as IActivate;
            if (activatable != null)
            {
                activatable.Activate();
            }
            
            var resultPrompt = rootModel as IPrompt;
            if (resultPrompt != null)
            {
                messagePrompt.SetBinding(PopupPrompt.IsOpenProperty, new Binding { Path = new PropertyPath("IsOpen") });
            }

            var deactivator = rootModel as IDeactivate;
            if (deactivator != null)
            {
                messagePrompt.Completed += (sender, args) => deactivator.Deactivate(true);
            }


            ViewLocator.LocateForModel(rootModel, messagePrompt, null);
            messagePrompt.Show();
        }

        private PopupPrompt CreateMessagePrompt(IEnumerable<KeyValuePair<string, Func<object>>> settings)
        {
            var messagePrompt = new PopupPrompt();
            messagePrompt.Overlay = new SolidColorBrush(Color.FromArgb(200, 100, 149, 237));
            ApplySettings(messagePrompt, settings);
            return messagePrompt;
        }

        #endregion

        #region Toast
        public void ShowToast(string message, string title = null, IDictionary<string, Func<object>> settings = null)
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

        private ToastPrompt CreateToastPrompt(IEnumerable<KeyValuePair<string, Func<object>>> settings)
        {
            var toastPrompt = new ToastPrompt { AllowDrop = false };
            ApplySettings(toastPrompt, settings);
            return toastPrompt;
        }

        #endregion

        private bool ApplySettings(object target, IEnumerable<KeyValuePair<string, Func<object>>> settings)
        {
            if (settings != null)
            {
                var type = target.GetType();

                foreach (var pair in settings)
                {
                    var propertyInfo = type.GetProperty(pair.Key);

                    if (propertyInfo != null && pair.Value != null)
                        propertyInfo.SetValue(target, pair.Value(), null);
                }

                return true;
            }
            return false;
        }
    }


}