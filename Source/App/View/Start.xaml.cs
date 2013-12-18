using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace App.View
{
    public abstract class StartBase : BandSox.Utility.UI.BasePage<ViewModel.Start>
    {
        public StartBase()
        {
            if ( Windows.ApplicationModel.DesignMode.DesignModeEnabled )
            {
                ViewModel = new ViewModel.Start ( ) { Count = 42 };
                DataContext = ViewModel;
            }
        }
    }

    public sealed partial class Start
    {
        public Start ( )
        {
            this.InitializeComponent ( );
        }
    }
}
