using System;

namespace Utilis.Messaging
{
	public class StatusMessage : BaseDataMessage<StatusMessage.Status>
	{
		public class Status
		{
			public string Type { get; set; }
			public string Context { get; set; }
			public string Message { get; set; }
			public string TechnicalDetail { get; set; }
			public DateTimeOffset Timestamp { get; set; }
		}

		public StatusMessage ( string sType, string sContext, string sMessage, string sTechnicalDetail )
		{
			Data = new Status ( )
				{
					Type = sType,
					Context = sContext,
					Message = sMessage, 
					TechnicalDetail = sTechnicalDetail,
					Timestamp = DateTimeOffset.Now
				};
		}
	}
}