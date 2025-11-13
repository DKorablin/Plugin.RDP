using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Plugin.RDP.RDP
{
	internal class RemoteSessions
	{
		public class RemoteSessionInfo
		{
			public String ClientName { get; set; }
			public String DomainName { get; set; }
			public Int32 SessionId { get; set; }
			public Native.ConnectStateClass State { get; set; }
			public String UserName { get; set; }
		}

		private IntPtr _hServer;
		private readonly String _server;

		public Boolean IsConnected => this._hServer != (IntPtr)0;

		public RemoteSessions(String server)
		{
			this._server = server;
			this._hServer = IntPtr.Zero;
		}

		public void OpenServer()
		{
			this._hServer = Native.OpenServer(this._server);
			if(this._hServer == IntPtr.Zero)
				throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		public void CloseServer()
		{
			if(this._hServer != IntPtr.Zero)
			{
				Native.CloseServer(this._hServer);
				this._hServer = IntPtr.Zero;
			}
		}

		public RemoteSessions.RemoteSessionInfo[] QuerySessions()
		{
			if(this._hServer == IntPtr.Zero)
				throw new InvalidOperationException("QuerySessions called before OpenServer succeeded");

			if(!Native.EnumerateSessions(this._hServer, 0, 1, out IntPtr intPtr, out Int32 num))
				throw new Win32Exception(Marshal.GetLastWin32Error());

			List<RemoteSessionInfo> list = new List<RemoteSessionInfo>();
			Native.SessionInfo sessionInfo = new Native.SessionInfo();
			try
			{
				IntPtr intPtr2 = intPtr;
				for(Int32 i = 0; i < num; i++)
				{
					Marshal.PtrToStructure(intPtr2, sessionInfo);
					intPtr2 = (IntPtr)((Int64)intPtr2 + (Int64)Marshal.SizeOf(sessionInfo));
					Native.QuerySessionInformation(this._hServer, sessionInfo.SessionId, Native.InfoClass.UserName, out IntPtr pBuffer, out _);
					String text = Marshal.PtrToStringAuto(pBuffer);
					if(text.Length != 0)
					{
						Native.QuerySessionInformation(this._hServer, sessionInfo.SessionId, Native.InfoClass.DomainName, out pBuffer, out _);
						String domainName = Marshal.PtrToStringAuto(pBuffer);
						Native.QuerySessionInformation(this._hServer, sessionInfo.SessionId, Native.InfoClass.ClientName, out pBuffer, out _);
						String clientName = Marshal.PtrToStringAuto(pBuffer);
						list.Add(new RemoteSessionInfo
						{
							ClientName = clientName,
							DomainName = domainName,
							SessionId = sessionInfo.SessionId,
							UserName = text,
							State = sessionInfo.State
						});
					}
				}
			} finally
			{
				Native.FreeMemory(intPtr);
			}
			return list.ToArray();
		}

		public void DisconnectSession(Int32 sessionId)
		{
			if(!Native.DisconnectSession(this._hServer,sessionId,true))
				throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		public void LogOffSession(Int32 sessionId)
		{
			if(!Native.LogOffSession(this._hServer, sessionId, true))
				throw new Win32Exception(Marshal.GetLastWin32Error());
		}
	}
}