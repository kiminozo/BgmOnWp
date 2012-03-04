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
            private IDictionary<string, Func<object>> settings;

            public PromptSetup(IPromptManager promptManager)
            {
                this.promptManager = promptManager;
                this.viewModel = IoC.Get<T>();
            }

            public void Show()
            {
                promptManager.ShowPopup(viewModel, context, settings);
            }

            public IPopupPromptSetup<T> EnableCancel
            {
                get
                {
                    if (settings == null) settings = new Dictionary<string, Func<object>>();
                    settings["IsCancelVisible"] = () => true;
                    return this;
                }
            }

            public IPopupPromptSetup<T> Setup(Action<T> action)
            {
                action(viewModel);
                return this;
            }
        }


        public static void ToastInfo(this IPromptManager promptManager, string message, string title = null)
        {
            var setting = new Dictionary<string, Func<object>>()
                              {
                                  {"Background", () => Application.Current.Resources["ToastInfoBackground"]}
                              };

            promptManager.ShowToast(message, title, setting);
        }

        public static void ToastError(this IPromptManager promptManager, Exception exception, string title = null)
        {

            var setting = new Dictionary<string, Func<object>>()
                              {
                                  {"Background", () => Application.Current.Resources["ToastErrorBackground"]}
                              };
            promptManager.ShowToast(exception.Message, title, setting);
        }
    }

    public interface IPopupPromptSetup<T>
    {
        void Show();

        IPopupPromptSetup<T> EnableCancel { get; }
        IPopupPromptSetup<T> Setup(Action<T> action);
    }

}