using System;

namespace Plugin.RDP
{
	public enum AuthenticationLevel : UInt32
	{
		/// <summary>No authentication of the server.</summary>
		NoAuthentication = 0,
		/// <summary>Server authentication is required and must complete successfully for the connection to proceed.</summary>
		Disconnect = 1,
		/// <summary>Attempt authentication of the server. If authentication fails, the user will be prompted with the option to cancel the connection or to proceed without server authentication.</summary>
		Prompt = 2,
	}
}