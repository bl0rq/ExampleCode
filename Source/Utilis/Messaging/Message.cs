﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Messaging
{
	public class TrivialAttribute : Attribute
	{
	}

	public abstract class BaseAsyncMessage : IMessage
	{
		public bool IsAsync
		{
			get { return true; }
		}
	}

	public abstract class BaseSyncMessage : IMessage
	{
		public bool IsAsync
		{
			get { return false; }
		}
	}

	public class AppStartedMessage : BaseAsyncMessage
	{
	}

	public class AppShutdownMessage : BaseSyncMessage
	{
	}

	public class AppKillRequestedMessage : BaseAsyncMessage
	{

	}

	public class UserInfoMessage : BaseAsyncMessage
	{
		public string Message { get; private set; }

		public UserInfoMessage ( string sMessage )
		{
			Message = sMessage;
		}
	}

	public class StartupCompleteMessage : BaseSyncMessage
	{

	}

	public class FirstWindowLoaded : BaseSyncMessage
	{
	}
}
