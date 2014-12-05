using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
    public static class DictionaryExtensions
    {
        public static T_Value SafeGet<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> oDictionary, T_Key sKey )
        {
            return oDictionary.SafeGet ( sKey, ( ) => default ( T_Value ), false );
        }

        //public static T_Value SafeGet<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> oDictionary, T_Key sKey, System.Threading.ReaderWriterLockSlim rwlLock )
        //{
        //	T_Value oReturn = default ( T_Value );

        //	Runner.RunReadLocked (
        //		( ) =>
        //		{
        //			oReturn = oDictionary.SafeGet ( sKey, ( ) => default ( T_Value ), false );
        //		}, rwlLock );

        //	return oReturn;
        //}

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
                    oDictionary [ sKey ] = oDefault;

                return oDefault;
            }
        }

        //public static string SafeGetValue ( this System.Web.HttpCookieCollection htCookies, string sKey )
        //{
        //    System.Web.HttpCookie oCookie = htCookies[ sKey ];
        //    if ( oCookie != null )
        //        return oCookie.Value;
        //    else
        //        return null;
        //}

        //public static T_Value GetOrCreate<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> ht, T_Key oKey, Func<T_Key, T_Value> fnCreate, System.Threading.ReaderWriterLockSlim rwlLock ) where T_Value : class
        //{
        //	T_Value value = null;
        //	Runner.RunReadLocked ( ( ) =>
        //	{
        //		value = ht.SafeGet ( oKey );
        //	}, rwlLock );

        //	if ( value == null )
        //	{
        //		Runner.RunWriteLocked ( ( ) =>
        //		{
        //			value =
        //				ht.SafeGet ( oKey )
        //				?? fnCreate ( oKey );
        //		}, rwlLock );
        //	}

        //	return value;
        //}

        public static void Add<T_Key, T_Value> ( this Dictionary<T_Key, IList<T_Value>> ht, T_Key key, T_Value value )
        {
            IList<T_Value> list = null;
            if ( !ht.ContainsKey ( key ) )
            {
                list = new List<T_Value> ( );
                ht [ key ] = list;

                list.Add ( value );
            }
            else
            {
                list = ht [ key ];
                if ( !list.Contains ( value ) )
                    list.Add ( value );
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
                ht [ oKey ] = aList;

                aList.Add ( oValue );
            }
            else
            {
                aList = ht [ oKey ];
                if ( !aList.Contains ( oValue ) )
                    aList.Add ( oValue );
            }
        }

        public static void Remove<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key oKey, T_Value oValue )
        {
            List<T_Value> aList = null;
            if ( ht.ContainsKey ( oKey ) )
            {
                aList = ht [ oKey ];
                aList.Remove ( oValue );

                if ( aList.Count == 0 )
                    ht.Remove ( oKey );
            }
        }

        public static void Add<T_Key, T_Value> ( this Dictionary<T_Key, System.Collections.IList> ht, T_Key key, T_Value value )
        {
            System.Collections.IList aList = null;
            if ( !ht.ContainsKey ( key ) )
            {
                aList = new List<T_Value> ( );
                ht [ key ] = aList;

                aList.Add ( value );
            }
            else
            {
                aList = ht [ key ];
                if ( !aList.Contains ( value ) )
                    aList.Add ( value );
            }
        }

        public static void Remove<T_Key, T_Value> ( this Dictionary<T_Key, System.Collections.IList> ht, T_Key key, T_Value value )
        {
            System.Collections.IList aList = null;
            if ( ht.ContainsKey ( key ) )
            {
                aList = ht [ key ];
                aList.Remove ( value );

                if ( aList.Count == 0 )
                    ht.Remove ( key );
            }
        }

        public static bool Contains<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key key, T_Value value ) where T_Value : class //, IEquatable<T_Value>
        {
            if ( ht.ContainsKey ( key ) )
                return ht [ key ].Where ( itm => itm == value ).Any ( );
            else
                return false;
        }
    }
}
