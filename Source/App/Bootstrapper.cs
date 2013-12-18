using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace App
{
    public class Bootstrapper
    {
        private BandSox.Utility.UI.Navigation.IService m_navigationService;

        public void Start ( Windows.UI.Xaml.Controls.Frame navigationFrame )
        {
            LoadServiceLocator ( navigationFrame );

            m_navigationService = BandSox.Utility.ServiceLocator.Instance.GetInstance<BandSox.Utility.UI.Navigation.IService> ( );
            m_navigationService.Navigated += Bootstrapper_Navigated;
            m_navigationService.Navigate ( new ViewModel.Start ( ) );
        }

        void Bootstrapper_Navigated ( )
        {
            m_navigationService.Navigated -= Bootstrapper_Navigated;

            GotoStart ( );
        }

        private async void GotoStart ( )
        {
            // this nonsene is due to the frame ignoring a second call to navigate that comes too quickly.  #fail
            await Task.Delay ( TimeSpan.FromMilliseconds ( 10 ) );

            m_navigationService.Navigate ( new ViewModel.Start ( ) );
        }

        private void LoadServiceLocator ( Windows.UI.Xaml.Controls.Frame navigationFrame )
        {
            Autofac.ContainerBuilder oBuilder = new Autofac.ContainerBuilder ( );

            var vf = new BandSox.Utility.UI.ViewFinder ( );
            oBuilder.Register<BandSox.Utility.UI.IViewFinder> (
                oContext => vf )
                .InstancePerLifetimeScope ( );

            var vm = new BandSox.Utility.UI.ViewMapper ( GetType ( ).GetTypeInfo ( ).Assembly, vf );
            oBuilder.Register<BandSox.Utility.UI.IViewMapper> (
                oContext => vm )
                .InstancePerLifetimeScope ( );

            oBuilder.Register<BandSox.Utility.UI.Navigation.IService> (
                oContext => new BandSox.Utility.UI.Navigation.Service ( navigationFrame, vm, _vm => _vm is ViewModel.Start && ( (ViewModel.Start)_vm ).Count == 0 ) )
                .InstancePerLifetimeScope ( );

            BandSox.Utility.ServiceLocator.Instance = new BandSox.Utility.ServiceLocator ( oBuilder.Build ( ) );
        }
    }
}
