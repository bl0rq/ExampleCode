using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BandSox.Utility.Extensions
{
	public static class DictionaryExtensions
	{
		public static T_Value SafeGet<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> oDictionary, T_Key sKey )
		{
			return oDictionary.SafeGet ( sKey, ( ) => default ( T_Value ), false );
		}

		public static T_Value SafeGet<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> oDictionary, T_Key sKey, Func<T_Value> fnGetDefault, bool bAddDefault )
		{
			if ( oDictionary == null ) return fnGetDefault ( );

			T_Value value;
			if ( oDictionary.TryGetValue ( sKey, out value ) )
				return value;
			else
			{
				T_Value oDefault = fnGetDefault ( );

				if ( bAddDefault )
					oDictionary[ sKey ] = oDefault;

				return oDefault;
			}
		}

		public static void Add<T_Key, T_Value> ( this Dictionary<T_Key, IList<T_Value>> ht, T_Key oKey, T_Value oValue )
		{
			IList<T_Value> aList = null;
			if ( !ht.ContainsKey ( oKey ) )
			{
				aList = new List<T_Value> ( );
				ht[ oKey ] = aList;

				aList.Add ( oValue );
			}
			else
			{
				aList = ht[ oKey ];
				if ( !aList.Contains ( oValue ) )
					aList.Add ( oValue );
			}
		}

		public static void AddRange<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key oKey, IEnumerable<T_Value> aValues )
		{
			foreach ( T_Value oValue in aValues )
			{
				ht.Add ( oKey, oValue );
			}
		}

		public static void Add<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key oKey, T_Value oValue )
		{
			List<T_Value> aList = null;
			if ( !ht.ContainsKey ( oKey ) )
			{
				aList = new List<T_Value> ( );
				ht[ oKey ] = aList;

				aList.Add ( oValue );
			}
			else
			{
				aList = ht[ oKey ];
				if ( !aList.Contains ( oValue ) )
					aList.Add ( oValue );
			}
		}

		public static void Remove<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key oKey, T_Value oValue )
		{
			List<T_Value> aList = null;
			if ( ht.ContainsKey ( oKey ) )
			{
				aList = ht[ oKey ];
				aList.Remove ( oValue );

				if ( aList.Count == 0 )
					ht.Remove ( oKey );
			}
		}

		public static void Add<T_Key, T_Value> ( this Dictionary<T_Key, System.Collections.IList> ht, T_Key oKey, T_Value oValue )
		{
			System.Collections.IList aList = null;
			if ( !ht.ContainsKey ( oKey ) )
			{
				aList = new List<T_Value> ( );
				ht[ oKey ] = aList;

				aList.Add ( oValue );
			}
			else
			{
				aList = ht[ oKey ];
				if ( !aList.Contains ( oValue ) )
					aList.Add ( oValue );
			}
		}

		public static void Remove<T_Key, T_Value> ( this Dictionary<T_Key, System.Collections.IList> ht, T_Key oKey, T_Value oValue )
		{
			System.Collections.IList aList = null;
			if ( ht.ContainsKey ( oKey ) )
			{
				aList = ht[ oKey ];
				aList.Remove ( oValue );

				if ( aList.Count == 0 )
					ht.Remove ( oKey );
			}
		}

		public static bool Contains<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key oKey, T_Value oValue ) where T_Value : class //, IEquatable<T_Value>
		{
			if ( ht.ContainsKey ( oKey ) )
				return ht[ oKey ].Where ( itm => itm == oValue ).Any ( );
			else
				return false;
		}
	}
}
