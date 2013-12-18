using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BandSox.Utility.Extensions;

namespace BandSox.Utility.ObjectModel
{
	[System.Runtime.Serialization.DataContract]
	public abstract class BaseNotifyPropertyChanged : System.ComponentModel.INotifyPropertyChanged
	{
		private readonly object m_oPropertiesListSync = new object ( );

		public event Action<BaseNotifyPropertyChanged, IEnumerable<Pair<string, TunneledPropertyChangedEventArgs>>> PropertiesChanged;
		protected void DoPropertiesChanged ( BaseNotifyPropertyChanged sender, IEnumerable<Pair<string, TunneledPropertyChangedEventArgs>> aEventArgs )
		{
			if ( !m_bPropertiesChangedDisabled )
			{
				Action<BaseNotifyPropertyChanged, IEnumerable<Pair<string, TunneledPropertyChangedEventArgs>>> act = PropertiesChanged;
				if ( act != null )
					act ( sender, aEventArgs );
			}
		}

		#region INotifyPropertyChanged Members
		private bool m_bPropertiesChangedDisabled = false;
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		protected void DoPropertyChanged ( System.ComponentModel.PropertyChangedEventArgs e )
		{
			System.ComponentModel.PropertyChangedEventHandler oHandler = PropertyChanged;
			if ( oHandler != null )
				oHandler ( this, e );
		}

		protected virtual void OnPropertyChanged ( System.ComponentModel.PropertyChangedEventArgs e )
		{


		}

		protected void OnPropertyChanged<T> ( System.Linq.Expressions.Expression<Func<T>> exp )
		{
			OnPropertyChanged ( exp.GetName ( ) );
		}

		protected void OnPropertyChanged ( string sName )
		{
			TunneledPropertyChangedEventArgs e = new TunneledPropertyChangedEventArgs ( sName, this, null );

			OnPropertyChanged ( e );

			if ( !m_bDisableOnChange )
			{
				DoPropertyChanged ( e );

				DoPropertiesChanged ( this, new Pair<string, TunneledPropertyChangedEventArgs>[ ] { new Pair<string, TunneledPropertyChangedEventArgs> ( e.PropertyName, e ) } );
			}
			else if ( m_bTrackChanges )
			{
				lock ( m_oPropertiesListSync )
					if ( !m_aPropertiesChanged.Any ( item => item.A == sName ) )
						m_aPropertiesChanged.Add ( new Pair<string, TunneledPropertyChangedEventArgs> ( sName, e ) );

				m_bNeedsChangeFired = true;
			}
		}

		protected void OnPropertyChanged ( string sName, System.ComponentModel.PropertyChangedEventArgs oInner )
		{
			TunneledPropertyChangedEventArgs e = new TunneledPropertyChangedEventArgs ( sName, this, oInner );

			OnPropertyChanged ( e );

			TunneledPropertyChangedEventArgs oInner_tunned = oInner as TunneledPropertyChangedEventArgs;
			if ( oInner != null && oInner_tunned == null )
				oInner_tunned = new TunneledPropertyChangedEventArgs ( oInner.PropertyName, null, null );

			if ( !m_bDisableOnChange )
			{
				DoPropertyChanged ( e );

				DoPropertiesChanged ( this, new Pair<string, TunneledPropertyChangedEventArgs>[ ] { new Pair<string, TunneledPropertyChangedEventArgs> ( e.PropertyName, e ) } );
			}
			else if ( m_bTrackChanges )
			{
				lock ( m_oPropertiesListSync )
					if ( !m_aPropertiesChanged.Any ( item => item.A == sName && item.B == oInner ) )
						m_aPropertiesChanged.Add ( new Pair<string, TunneledPropertyChangedEventArgs> ( sName, e ) );

				m_bNeedsChangeFired = true;
			}
		}
		#endregion

		private int m_nDisabledCount = 0;
		protected bool m_bDisableOnChange = false;
		protected bool m_bNeedsChangeFired = true;
		protected bool m_bTrackChanges = true;
		protected List<Pair<string, TunneledPropertyChangedEventArgs>> m_aPropertiesChanged = new List<Pair<string, TunneledPropertyChangedEventArgs>> ( );

		public void DisableOnChange ( )
		{
			DisableOnChange ( true );
		}

		public void DisableOnChange ( bool bTrackChanges )
		{
			m_bTrackChanges = bTrackChanges;

			m_bDisableOnChange = true;
			m_nDisabledCount++;
		}

		public void EnableOnChange ( )
		{
			m_nDisabledCount--;
			if ( m_nDisabledCount == 0 )
			{
				m_bDisableOnChange = false;
				if ( m_bNeedsChangeFired )
				{
					Pair<string, TunneledPropertyChangedEventArgs>[ ] aPropertiesChanged;
					lock ( m_oPropertiesListSync )
					{
						aPropertiesChanged = m_aPropertiesChanged.ToArray ( );
						m_aPropertiesChanged.Clear ( );
					}

					m_bPropertiesChangedDisabled = true;
					foreach ( Pair<string, TunneledPropertyChangedEventArgs> oProperty in aPropertiesChanged )
					{
						OnPropertyChanged ( oProperty.A, oProperty.B );
					}
					m_bPropertiesChangedDisabled = false;

					DoPropertiesChanged ( this, aPropertiesChanged );
				}
			}
		}

		protected void HandleChanged<T> (
			System.Collections.ObjectModel.ObservableCollection<T> arr,
			System.Collections.Specialized.NotifyCollectionChangedEventArgs e,
			Action<T> fnAdd,
			Action<T> fnRemove,
			System.ComponentModel.PropertyChangedEventHandler fnChanged ) where T : System.ComponentModel.INotifyPropertyChanged
		{
			if ( e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add )
			{
				foreach ( T itm in e.NewItems )
				{
					fnAdd ( itm );
					if ( fnChanged != null )
						itm.PropertyChanged += fnChanged;
				}
			}
			else if ( e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove )
			{
				foreach ( T itm in e.OldItems )
				{
					if ( fnChanged != null )
						itm.PropertyChanged -= fnChanged;
					fnRemove ( itm );
				}
			}
		}
	}


	public class TunneledPropertyChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
	{
		private static Pair<object, string>[ ] EmptyList = new Pair<object, string>[ ] { };

		public System.ComponentModel.PropertyChangedEventArgs Inner { get; set; }

		public Type CurrentType { get; set; }

		public object Sender { get; set; }

		public TunneledPropertyChangedEventArgs ( string sPropertyName, object oSender, System.ComponentModel.PropertyChangedEventArgs oInner )
			: base ( sPropertyName )
		{
			Sender = oSender;
			CurrentType = oSender.GetType ( );
			Inner = oInner;
		}

		public bool ContainsProperty ( string sProperty )
		{
			if ( string.IsNullOrEmpty ( sProperty ) )
				return false;

			return GetAllPropertiesChanged ( ).Any ( item => item.B == sProperty );
		}

		public virtual IEnumerable<Pair<object, string>> GetAllPropertiesChanged ( )
		{
			return new Pair<object, string>[ ] { new Pair<object, string> ( Sender, PropertyName ) }.Concat ( GetAllInnerPropertiesChanged ( ) );
		}

		protected IEnumerable<Pair<object, string>> GetAllInnerPropertiesChanged ( )
		{
			if ( Inner != null )
			{
				TunneledPropertyChangedEventArgs oInner = Inner as TunneledPropertyChangedEventArgs;
				if ( oInner != null )
					return oInner.GetAllPropertiesChanged ( );
				else
					return new Pair<object, string>[ ] { new Pair<object, string> ( null, Inner.PropertyName ) };
			}
			return EmptyList;
		}
	}

	public class TunneledMultiPropertyChangedEventArgs : TunneledPropertyChangedEventArgs
	{
		public IEnumerable<string> PropertyNames { get; set; }

		public TunneledMultiPropertyChangedEventArgs ( IEnumerable<string> aPropertyNames, object oSender, System.ComponentModel.PropertyChangedEventArgs oInner )
			: base ( aPropertyNames.First ( ), oSender, oInner )
		{
			PropertyNames = aPropertyNames;
		}

		public override IEnumerable<Pair<object, string>> GetAllPropertiesChanged ( )
		{
			return PropertyNames
				.Select ( sPropertyName => new Pair<object, string> ( Sender, sPropertyName ) )
				.Concat ( GetAllInnerPropertiesChanged ( ) );
		}
	}
}
