using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.ObjectModel
{
	public class Singleton<T> where T : class, ICanBeASingleton, new ( )
	{
		protected Singleton ( ) { }

		internal static T m_instance;
		private static readonly object ms_sync = new object ( );

		public static T Instance
		{
			get
			{
				if ( m_instance == null )
				{
					lock ( ms_sync )
					{
						if ( m_instance == null )
							CreateInstance ( );
					}
				}

				return m_instance;
			}
		}

		private static void CreateInstance ( )
		{
			try
			{
				m_instance = new T ( );
			}
			catch ( Exception ex )
			{
				throw new Exception (
					string.Format (
						"Unable to create instance of type {0} : {1}",
							typeof ( T ).FullName,
							ex.InnerException != null
								? ex.InnerException.Message
								: ex.Message ),
					ex );
			}
		}
	}

	public interface ICanBeASingleton
	{
	}
}
