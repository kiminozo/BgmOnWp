using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public static class PromptManagerExtentions
    {
        public static IPopupPromptSetup<T> PopupFor<T>(this IPromptManager promptManager)
        {
            return new PromptSetup<T>(promptManager);
        }

        private sealed class PromptSetup<T> : IPopupPromptSetup<T>
        {
            private readonly IPromptManager promptManager;
            private readonly T viewModel;
            private object context = null;
            private IDictionary<string, object> settings;

            public PromptSetup(IPromptManager promptManager)
            {
                this.promptManager = promptManager;
                viewModel = IoC.Get<T>();
            }

            public void Show()
            {
                promptManager.ShowPopup(viewModel, context, settings);
            }

            public IPopupPromptSetup<T> SetTitleBackground(string resourcesKey)
            {
                if (settings == null) settings = new Dictionary<string, object>();
                settings["TitleBackground"] = Application.Current.Resources[resourcesKey];
                return this;
            }

            public IPopupPromptSetup<T> SetEnableCancel(bool enable = true)
            {
                if (settings == null) settings = new Dictionary<string, object>();
                settings["IsCancelVisible"] = enable;
                return this;
            }

            public IPopupPromptSetup<T> Setup(Action<T> action)
            {
                action(viewModel);
                return this;
            }
        }


        public static void ToastInfo(this IPromptManager promptManager, string message, string title = null)
        {
            var setting = new Dictionary<string, object>
                              {
                                  {"Background", Application.Current.Resources["ToastInfoBackground"]}
                              };

            promptManager.ShowToast(message, title, setting);
        }

        public static void ToastWarn(this IPromptManager promptManager, string message, string title = null)
        {
            var setting = new Dictionary<string, object>
                              {
                                  {"Background", Application.Current.Resources["ToastErrorBackground"]}
                              };

            promptManager.ShowToast(message, title, setting);
        }


        public static void ToastError(this IPromptManager promptManager, Exception exception, string title = null)
        {

            var setting = new Dictionary<string, object>
                              {
                                  {"Background", Application.Current.Resources["ToastErrorBackground"]}
                              };
            promptManager.ShowToast(exception.Message, title, setting);
        }
    }

    public interface IPopupPromptSetup<T>
    {
        void Show();

        IPopupPromptSetup<T> SetTitleBackground(string resourcesKey);
        IPopupPromptSetup<T> SetEnableCancel(bool enable = true);
        IPopupPromptSetup<T> Setup(Action<T> action);
    }

}