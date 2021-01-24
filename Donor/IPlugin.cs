using System;
using System.Collections.Generic;
using System.Text;

namespace Gator
    {
    public interface IPlugin
        {
        void Display();
        string Name();
        //void SetVars(string email, string passw, Gator.Constants.SoftLicense licns);
        }
    }
