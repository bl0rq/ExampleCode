using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace App
{
    public static class ServiceLocator
    {
        public static void Start ( Windows.UI.Xaml.Controls.Frame navigationFrame )
        {
            Autofac.ContainerBuilder builder = new Autofac.ContainerBuilder ( );

            var vf = new Utilis.UI.ViewFinder ( );
            builder.Register<Utilis.UI.IViewFinder> (
                oContext => vf )
                .InstancePerLifetimeScope ( );

            var vm = new Utilis.UI.ViewMapper ( typeof ( App ).GetTypeInfo ( ).Assembly, vf );
            builder.Register<Utilis.UI.IViewMapper> (
                oContext => vm )
                .InstancePerLifetimeScope ( );

            builder.Register<Utilis.UI.Navigation.IService> (
                oContext => new Utilis.UI.Navigation.Service ( navigationFrame, vm, _vm => _vm is ViewModel.Start && ( (ViewModel.Start)_vm ).Count == 0 ) )
                .InstancePerLifetimeScope ( );

            Utilis.ServiceLocator.Instance = new Utilis.ServiceLocator ( builder.Build ( ) );
        }
    }
}
