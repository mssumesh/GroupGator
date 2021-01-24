using System;
using System.Collections.Generic;
using System.Text;

namespace Gator
    {
    public interface IPlugin
        {
        void Display();
        string Name();
        string Version();
        }
    }
