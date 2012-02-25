using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public interface IProgressService
    {
        void Show();
        void Show(string message);
        void Hide();
    }
}
