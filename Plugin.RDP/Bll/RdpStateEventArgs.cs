using System;

namespace Plugin.RDP.Bll
{
	/// <summary>Client connection or disconnection arguments</summary>
	internal class RdpStateEventArgs : EventArgs
	{
		public enum StateType
		{
			Connect,
			Disconnect,
			Focus,
		}

		/// <summary>Client ID</summary>
		public Int32 TreeId { get; private set; }

		/// <summary>The client needs to be connected, disconnected, or focused</summary>
		public StateType State { get; private set; }

		public RdpStateEventArgs(Int32 treeId, StateType state)
		{
			this.TreeId = treeId;
			this.State = state;
		}
	}
}