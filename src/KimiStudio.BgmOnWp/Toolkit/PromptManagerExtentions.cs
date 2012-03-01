using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public static class PromptManagerExtentions
    {
        public static IPromptSetup<T> PopupFor<T>(this IPromptManager promptManager)
        {
            return new PromptSetup<T>(promptManager);
        }


        private sealed class PromptSetup<T> : IPromptSetup<T>
        {
            private readonly IPromptManager promptManager;
            private readonly T viewModel;
            private object context = null;
            private IDictionary<string, object> settings;

            public PromptSetup(IPromptManager promptManager)
            {
                this.promptManager = promptManager;
                this.viewModel = IoC.Get<T>();
            }

            public void Show()
            {
                promptManager.ShowPopup(viewModel, context, settings);
            }

            public IPromptSetup<T> Setup(Action<T> action)
            {
                action(viewModel);
                return this;
            }
        }
    }

    public interface IPromptSetup<T>
    {
        void Show();

        IPromptSetup<T> Setup(Action<T> action);
    }
}