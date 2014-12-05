using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilis.Extensions;

namespace Utilis
{
	public static class Logger
	{
		public enum Types
		{
			Exception, Error, Warning, Debug, Performance,
			Information
		}

		public static void Log ( Types eType, string sMessage )
		{
			Messaging.Bus.Instance.Send ( new Messaging.StatusMessage ( eType.ToString ( ), null, sMessage, null ) );
		}

		public static void Log ( Types eType, string sMessage, string sDetail )
		{
			Messaging.Bus.Instance.Send ( new Messaging.StatusMessage ( eType.ToString ( ), null, sMessage, sDetail ) );
		}

		public static void Log ( Types eType, string sMessage, string sDetail, string sContext )
		{
			Messaging.Bus.Instance.Send ( new Messaging.StatusMessage ( eType.ToString ( ), sContext, sMessage, sDetail ) );
		}

		public static void Log ( Exception ex, string sContext )
		{
			Log ( ex, sContext, "Error in " + sContext + " : " + ex.Message );
		}

		public static void Log ( Exception ex, string sContext, string sUserMessage )
		{
			Messaging.Bus.Instance.Send (
				new Messaging.StatusMessage (
					Types.Exception.ToString ( ),
					sContext,
					sUserMessage,
					ex.ToString ( ) ) );
		}

		public static void Log ( TimeSpan ts, string sActionName, string sContext )
		{
			Messaging.Bus.Instance.Send (
				new Messaging.StatusMessage (
					Types.Performance.ToString ( ),
					sContext,
					sActionName + " took " + ts.ToPrettyString ( ),
					null ) );
		}

		public static void Log ( TimeSpan ts, string sActionName, string sContext, string sDetail )
		{
			Messaging.Bus.Instance.Send (
				new Messaging.StatusMessage (
					Types.Performance.ToString ( ),
					sContext,
					sActionName + " took " + ts.ToPrettyString ( ),
					sDetail ) );
		}

		public static void LogInfo ( string sMessage, string sContext )
		{
			Log ( Types.Information, sMessage, null, sContext );
		}

		public static void LogError ( string sMessage, string sDetail, string sContext )
		{
			Log ( Types.Error, sMessage, sDetail, sContext );
		}
	}
}
