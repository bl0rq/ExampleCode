using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.UI.Navigation
{
    public class BackCommand : System.Windows.Input.ICommand
    {
        public BackCommand ( )
        {
            if ( !Windows.ApplicationModel.DesignMode.DesignModeEnabled && ServiceLocator.Instance != null )
                ServiceLocator.Instance.GetInstance<Navigation.IService> ( ).Navigated += NavigationCommand_Navigated;
        }

        void NavigationCommand_Navigated ( )
        {
            DoCanExecuteChanged ( );
        }

        public bool CanExecute ( object parameter )
        {
            if ( !Windows.ApplicationModel.DesignMode.DesignModeEnabled && ServiceLocator.Instance != null )
            {
                var navigationService = ServiceLocator.Instance.GetInstance<Navigation.IService> ( );
                return navigationService.CanGoBack();
            }
            else
                return true;
        }

        public void Execute ( object parameter )
        {
            if ( !Windows.ApplicationModel.DesignMode.DesignModeEnabled && ServiceLocator.Instance != null ) 
                ServiceLocator.Instance.GetInstance<Navigation.IService> ( ).GoBack ( );
        }

        public event EventHandler CanExecuteChanged;
        private void DoCanExecuteChanged ( )
        {
            EventHandler act = CanExecuteChanged;
            if ( act != null )
                act ( this, EventArgs.Empty );
        }
    }
}