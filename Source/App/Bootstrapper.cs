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
        private Utilis.UI.Navigation.IService m_navigationService;

        public void Start ( Windows.UI.Xaml.Controls.Frame navigationFrame )
        {
            ServiceLocator.Start ( navigationFrame );

            // Other startup/init stuff here

            m_navigationService = Utilis.ServiceLocator.Instance.GetInstance<Utilis.UI.Navigation.IService> ( );
            m_navigationService.Navigate ( new ViewModel.Start ( ) );
        }
    }
}
