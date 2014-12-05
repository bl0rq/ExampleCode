using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Utilis.Extensions;

namespace Utilis
{
	public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator, Messaging.IListener<Messaging.AppShutdownMessage>
	{
		void RegisterInstance<T> ( T o );
		void RemoveInstance<T> ( );
	}

	public class ServiceLocator : Microsoft.Practices.ServiceLocation.ServiceLocatorImplBase, IServiceLocator
	{
		public static IServiceLocator Instance
		{
			get;
			set;
		}


		private readonly Autofac.IComponentContext m_oContext;
		private IDisposable m_oBusToken;

		private readonly Dictionary<Type, object> m_htAdditionalTypes = new Dictionary<Type, object> ( );

		public ServiceLocator ( Autofac.IComponentContext oContext )
		{
			if ( oContext == null )
				throw new ArgumentNullException ( "oContext" );

			m_oContext = oContext;

			m_oBusToken = Messaging.Bus.Instance.ListenFor ( this );
		}

		protected override object DoGetInstance ( Type serviceType, string key )
		{
			return
				m_htAdditionalTypes.SafeGet ( serviceType )
				?? ( key != null
					? m_oContext.Resolve ( serviceType, new [] { new NamedParameter ( "key", key ) } )
					: m_oContext.SafeResolve ( serviceType ) );
		}

		protected override IEnumerable<object> DoGetAllInstances ( Type serviceType )
		{
			var enumerableType = typeof ( IEnumerable<> ).MakeGenericType ( serviceType );

			object instance = m_oContext.Resolve ( enumerableType );
			return ( (System.Collections.IEnumerable)instance ).Cast<object> ( );
		}

		public void Receive ( Messaging.AppShutdownMessage oMessage )
		{
			m_oBusToken.DisposeWithCare ( );
			m_oBusToken = null;

			m_oContext.ComponentRegistry.DisposeWithCare ( );
		}

		public void RegisterInstance<T> ( T o )
		{
			lock ( m_htAdditionalTypes )
			{
				m_htAdditionalTypes [ typeof ( T ) ] = o;
			}
		}

		public void RemoveInstance<T> ( )
		{
			lock ( m_htAdditionalTypes )
			{
				m_htAdditionalTypes.Remove ( typeof ( T ) );
			}
		}
	}

	public static class ComponentContextExtensions
	{
		public static object SafeResolve ( this Autofac.IComponentContext context, Type serviceType )
		{
			object o;
			if ( context.TryResolve ( serviceType, out o ) )
				return o;
			else
				return null;
		}
	}
}
