using System;
using System.Runtime.InteropServices;

namespace Plugin.RDP
{
	internal static class Native
	{
		public enum ConnectStateClass
		{
			Active,
			Connected,
			ConnectQuery,
			Shadow,
			Disconnected,
			Idle,
			Listen,
			Reset,
			Down,
			Init
		}
		public enum InfoClass
		{
			InitialProgram,
			ApplicationName,
			WorkingDirectory,
			OEMId,
			SessionId,
			UserName,
			WinStationName,
			DomainName,
			ConnectState,
			ClientBuildNumber,
			ClientName,
			ClientDirectory,
			ClientProductId,
			ClientHardwareId,
			ClientAddress,
			ClientDisplay,
			ClientProtocolType,
			IdleTime,
			LogonTime,
			IncomingBytes,
			OutgoingBytes,
			IncomingFrames,
			OutgoingFrames
		}

		public enum ShutdownMode
		{
			/// <summary>Shuts down and then restarts the system on the RD Session Host server. This is equivalent to calling ExitWindowsEx with EWX_REBOOT. The calling process must have the SE_SHUTDOWN_NAME privilege enabled.</summary>
			Reboot = 4,
			/// <summary>Shuts down the system on the RD Session Host server and, on computers that support software control of AC power, turns off the power. This is equivalent to calling ExitWindowsEx with EWX_SHUTDOWN and EWX_POWEROFF. The calling process must have the SE_SHUTDOWN_NAME privilege enabled.</summary>
			PowerOff = 8
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class SessionInfo
		{
			/// <summary>Session identifier of the session.</summary>
			public Int32 SessionId;
			/// <summary>Pointer to a null-terminated string that contains the WinStation name of this session. The WinStation name is a name that Windows associates with the session, for example, "services", "console", or "RDP-Tcp#0".</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public String WinStationName;
			/// <summary>A value from the ConnectStateClass enumeration type that indicates the session's current connection state.</summary>
			public Native.ConnectStateClass State;
		}

		/// <summary>Opens a handle to the specified Remote Desktop Session Host (RD Session Host) server.</summary>
		/// <param name="serverName">Pointer to a null-terminated string specifying the NetBIOS name of the RD Session Host server.</param>
		/// <remarks>
		/// When you have finished using the handle returned by <see cref="OpenServer"/>, release it by calling the <see cref="CloseServer"/> function.
		/// You do not need to open a handle for operations performed on the RD Session Host server on which your application is running. Use the constant WTS_CURRENT_SERVER_HANDLE instead.
		/// </remarks>
		/// <returns>
		/// If the function succeeds, the return value is a handle to the specified server.
		/// If the function fails, it returns a handle that is not valid. You can test the validity of the handle by using it in another function call.
		/// </returns>
		[DllImport("wtsapi32.dll", EntryPoint = "WTSOpenServer", SetLastError = true)]
		public static extern IntPtr OpenServer(String serverName);

		/// <summary>Closes an open handle to a Remote Desktop Session Host (RD Session Host) server.</summary>
		/// <remarks>
		/// Call the WTSCloseServer function as part of your program's clean-up routine to close all the server handles opened by calls to the <see cref="OpenServer"/> or <see cref="M:WTSOpenServerEx"/> function.
		/// After the handle has been closed, it cannot be used with any other WTS APIs.
		/// </remarks>
		/// <param name="hServer">
		/// A handle to an RD Session Host server opened by a call to the <see cref="OpenServer"/> or <see cref="M:WTSOpenServerEx"/> function.
		/// Do not pass WTS_CURRENT_SERVER_HANDLE for this parameter.
		/// </param>
		[DllImport("wtsapi32.dll", EntryPoint = "WTSCloseServer")]
		public static extern void CloseServer(IntPtr hServer);

		/// <summary>Retrieves a list of sessions on a specified Remote Desktop Session Host (RD Session Host) server.</summary>
		/// <param name="hServer">A handle to an RD Session Host server. Specify a handle opened by the <see cref="OpenServer"/> or <see cref="M:WTSOpenServerEx"/> function, or specify WTS_CURRENT_SERVER_HANDLE to indicate the RD Session Host server on which your application is running.</param>
		/// <param name="reserved">This parameter is reserved. It must be zero.</param>
		/// <param name="version">The version of the enumeration request. This parameter must be 1.</param>
		/// <param name="pSessionInfo">
		/// A pointer to a variable that receives a pointer to an array of WTS_SESSION_INFO structures. Each structure in the array contains information about a session on the specified RD Session Host server. To free the returned buffer, call the WTSFreeMemory function.
		/// To enumerate a session, you must have Query Information permission. For more information, see Remote Desktop Services Permissions. To modify permissions on a session, use the Remote Desktop Services Configuration administrative tool.
		/// To enumerate sessions running on a virtual machine hosted on a RD Virtualization Host server, you must be a member of the Administrators group on the RD Virtualization Host server.
		/// </param>
		/// <param name="count">A pointer to the variable that receives the number of WTS_SESSION_INFO structures returned in the ppSessionInfo buffer.</param>
		/// <returns>
		/// If the function succeeds, the return value is a nonzero value.
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("wtsapi32.dll", EntryPoint = "WTSEnumerateSessionsW",SetLastError=true)]
		public static extern Boolean EnumerateSessions(IntPtr hServer, Int32 reserved, Int32 version, out IntPtr pSessionInfo, out Int32 count);

		/// <summary>Frees memory allocated by a Remote Desktop Services function.</summary>
		/// <remarks>Several Remote Desktop Services functions allocate buffers to return information. Use the WTSFreeMemory function to free these buffers.</remarks>
		/// <param name="pMemory">Pointer to the memory to free.</param>
		[DllImport("wtsapi32.dll", EntryPoint = "WTSFreeMemory")]
		public static extern void FreeMemory(IntPtr pMemory);

		/// <summary>Retrieves session information for the specified session on the specified Remote Desktop Session Host (RD Session Host) server. It can be used to query session information on local and remote RD Session Host servers.</summary>
		/// <param name="hServer">A handle to an RD Session Host server. Specify a handle opened by the WTSOpenServer function, or specify WTS_CURRENT_SERVER_HANDLE to indicate the RD Session Host server on which your application is running.</param>
		/// <param name="sessionId">
		/// A Remote Desktop Services session identifier. To indicate the session in which the calling application is running (or the current session) specify WTS_CURRENT_SESSION. Only specify WTS_CURRENT_SESSION when obtaining session information on the local server. If WTS_CURRENT_SESSION is specified when querying session information on a remote server, the returned session information will be inconsistent. Do not use the returned data.
		/// You can use the WTSEnumerateSessions function to retrieve the identifiers of all sessions on a specified RD Session Host server.
		/// To query information for another user's session, you must have Query Information permission. For more information, see Remote Desktop Services Permissions. To modify permissions on a session, use the Remote Desktop Services Configuration administrative tool.
		/// </param>
		/// <param name="infoClass">A value of the WTS_INFO_CLASS enumeration that indicates the type of session information to retrieve in a call to the WTSQuerySessionInformation function.</param>
		/// <param name="pBuffer">A pointer to a variable that receives a pointer to the requested information. The format and contents of the data depend on the information class specified in the WTSInfoClass parameter. To free the returned buffer, call the WTSFreeMemory function.</param>
		/// <param name="bytesReturned">A pointer to a variable that receives the size, in bytes, of the data returned in ppBuffer.</param>
		/// <returns>
		/// If the function succeeds, the return value is a nonzero value.
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("wtsapi32.dll", EntryPoint = "WTSQuerySessionInformationW", SetLastError = true)]
		public static extern Boolean QuerySessionInformation(IntPtr hServer, Int32 sessionId, Native.InfoClass infoClass, out IntPtr pBuffer, out Int32 bytesReturned);

		/// <summary>Disconnects the logged-on user from the specified Remote Desktop Services session without closing the session. If the user subsequently logs on to the same Remote Desktop Session Host (RD Session Host) server, the user is reconnected to the same session.</summary>
		/// <param name="hServer">A handle to an RD Session Host server. Specify a handle opened by the WTSOpenServer or WTSOpenServerEx function, or specify WTS_CURRENT_SERVER_HANDLE to indicate the RD Session Host server on which your application is running.</param>
		/// <param name="sessionId">
		/// A Remote Desktop Services session identifier. To indicate the current session, specify WTS_CURRENT_SESSION. To retrieve the identifiers of all sessions on a specified RD Session Host server, use the WTSEnumerateSessions function.
		/// To be able to disconnect another user's session, you need to have the Disconnect permission. For more information, see Remote Desktop Services Permissions. To modify permissions on a session, use the Remote Desktop Services Configuration administrative tool.
		/// To disconnect sessions running on a virtual machine hosted on a RD Virtualization Host server, you must be a member of the Administrators group on the RD Virtualization Host server.
		/// </param>
		/// <param name="wait">Indicates whether the operation is synchronous. Specify TRUE to wait for the operation to complete, or FALSE to return immediately.</param>
		/// <returns>
		/// If the function succeeds, the return value is a nonzero value.
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("wtsapi32.dll", EntryPoint = "WTSDisconnectSession", SetLastError = true)]
		public static extern Boolean DisconnectSession(IntPtr hServer, Int32 sessionId, Boolean wait);

		/// <summary>Logs off a specified Remote Desktop Services session.</summary>
		/// <param name="hServer">
		/// A handle to an RD Session Host server. Specify a handle opened by the WTSOpenServer or WTSOpenServerEx function, or specify WTS_CURRENT_SERVER_HANDLE to indicate the RD Session Host server on which your application is running.
		/// </param>
		/// <param name="sessionId">
		/// A Remote Desktop Services session identifier. To indicate the current session, specify WTS_CURRENT_SESSION. You can use the WTSEnumerateSessions function to retrieve the identifiers of all sessions on a specified RD Session Host server.
		/// To be able to log off another user's session, you need to have the Reset permission. For more information, see Remote Desktop Services Permissions. To modify permissions on a session, use the Remote Desktop Services Configuration administrative tool.
		/// To log off sessions running on a virtual machine hosted on a RD Virtualization Host server, you must be a member of the Administrators group on the RD Virtualization Host server.
		/// </param>
		/// <param name="wait">
		/// Indicates whether the operation is synchronous.
		/// If bWait is TRUE, the function returns when the session is logged off.
		/// If bWait is FALSE, the function returns immediately. To verify that the session has been logged off, specify the session identifier in a call to the WTSQuerySessionInformation function. WTSQuerySessionInformation returns zero if the session is logged off.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a nonzero value.
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("wtsapi32.dll", EntryPoint = "WTSLogoffSession",SetLastError = true)]
		public static extern Boolean LogOffSession(IntPtr hServer, Int32 sessionId, Boolean wait);

		/// <summary>
		/// Shuts down (and optionally restarts) the specified Remote Desktop Session Host (RD Session Host) server.
		/// To shut down or restart the system, the calling process must have the SE_SHUTDOWN_NAME privilege enabled. For more information about security privileges, see Privileges and Authorization Constants.
		/// </summary>
		/// <param name="hServer">Handle to an RD Session Host server. Specify a handle opened by the WTSOpenServer function, or specify WTS_CURRENT_SERVER_HANDLE to indicate the RD Session Host server on which your application is running.</param>
		/// <param name="mode">Indicates the type of shutdown. This parameter can be one of the following values. <see cref="T:ShutdownMode"/></param>
		/// <returns></returns>
		[DllImport("wtsapi32.dll", EntryPoint = "WTSShutdownSystem",SetLastError = true)]
		public static extern Boolean ShutdownSystem(IntPtr hServer, Int32 shutdownFlag);
	}
}