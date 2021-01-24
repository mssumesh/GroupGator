using System;
using System.Collections.Generic;
using System.Text;

namespace AdminPanel
    {
    public interface IPlugin
        {
        string GetName();
        string GetVersion();
        void Show();
        void Move(System.Drawing.Point topleft);
        void Close();
        }
    }
