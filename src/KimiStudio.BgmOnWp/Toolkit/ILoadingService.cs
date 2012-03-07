using System;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public interface ILoadingService
    {
        void Show();
        void Show(string message);
        void Hide();
    }
}