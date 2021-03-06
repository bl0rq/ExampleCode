﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModel
{
    public class Start : Utilis.UI.ViewModel.Base
    {
        public Start ( )
        {
            Next = new Utilis.UI.NavigationCommand<Start> ( ( ) => new Start ( ) { Count = m_count + 1 } );
        }

        private int m_count = 0;
        public int Count
        {
            get { return m_count; }
            set
            {
                if ( m_count != value )
                {
                    m_count = value;

                    OnPropertyChanged ( ( ) => Count );
                }
            }
        }

        private System.Windows.Input.ICommand m_next;
        public System.Windows.Input.ICommand Next
        {
            get { return m_next; }
            set
            {
                if ( m_next != value )
                {
                    m_next = value;

                    OnPropertyChanged ( ( ) => Next );
                }
            }
        }
    }
}
