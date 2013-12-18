using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BandSox.Utility.UI.Navigation
{
    public class NavigationException : Exception
    {
        public NavigationException ( string s )
            : base ( s )
        {
        }
    }
}