using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace App
{
    sealed partial class App : Application
    {
        private Bootstrapper m_bootstrapper;
        private Frame m_rootFrame;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched ( LaunchActivatedEventArgs args )
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            if ( args.PreviousExecutionState == ApplicationExecutionState.Running )
            {
                Window.Current.Activate ( );
                return;
            }

            if ( args.PreviousExecutionState == ApplicationExecutionState.Terminated )
            {
                //TODO: Load state from previously suspended application
            }

            // Create a Frame to act navigation context and navigate to the first page
            m_rootFrame = new Frame ( );
            if ( !m_rootFrame.Navigate ( typeof ( View.Splash ) ) )
            {
                throw new Exception ( "Failed to create initial page" );
            }

            m_bootstrapper = new Bootstrapper ( );
            m_bootstrapper.Start ( m_rootFrame );

            Window.Current.Content = m_rootFrame;
            Window.Current.Activate ( );
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
