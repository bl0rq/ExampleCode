using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
	public static class MiscExtensions
	{
		/// <summary>
		/// Disposes an object (if its not null) inside a try/catch w/ an empty catch.
		/// WARNING: surpresses the error!
		/// </summary>
		public static void DisposeWithCare ( this IDisposable oDisposable )
		{
			if ( oDisposable != null )
			{
				try
				{
					oDisposable.Dispose ( );
				}
				catch ( Exception e )
				{
					Logger.Log ( e, oDisposable.GetType ( ).Name + " Dispose" );
				}
			}
		}

		public static void DisposeItems ( this IEnumerable<IDisposable> aDisposables )
		{
			foreach ( IDisposable oDisposable in aDisposables )
			{
				oDisposable.DisposeWithCare ( );
			}
		}
	}

	public static class TimeExtensions
	{
		public static string ToPrettyString ( this TimeSpan ts )
		{
			if ( ts == TimeSpan.Zero )
				return "00:00";
			else if ( ts.TotalHours >= 1 )
				return string.Format (
					"{0}:{1}:{2}",
					Math.Floor ( ts.TotalHours ),
					Math.Floor ( ts.TotalMinutes - ( Math.Floor ( ts.TotalHours ) * 60 ) ).ToString ( "00" ),
					Math.Round ( ts.TotalSeconds - ( Math.Floor ( ts.TotalMinutes ) * 60 ), 1 ).ToString ( "00.#" ) );
			else if ( ts.TotalMinutes >= 1 )
				return string.Format (
					"{0}:{1}",
					Math.Floor ( ts.TotalMinutes ).ToString ( "00" ),
					Math.Round ( ts.TotalSeconds - ( Math.Floor ( ts.TotalMinutes ) * 60 ), 1 ).ToString ( "00.#" ) );
			else if ( ts.TotalSeconds >= 1 )
				return "00:" + Math.Round ( ts.TotalSeconds, 1 ).ToString ( "00.#" );
			else
				return Math.Round ( ts.TotalMilliseconds, 1 ) + " ms";
		}
	}

	public static class IOExtensions
	{
		public static void Write ( this System.IO.Stream oStream, byte[ ] aBytes )
		{
			if ( oStream == null )
				throw new ArgumentException ( "Stream cannot be null.", "oStream" );
			if ( aBytes == null )
				throw new ArgumentException ( "Byte array cannot be null.", "aBytes" );

			oStream.Write ( aBytes, 0, aBytes.Length );
		}
	}
}
