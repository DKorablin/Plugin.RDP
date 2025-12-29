using System;

namespace Plugin.RDP.Bll
{
	internal class TreeRowEventArgs : EventArgs
	{
		public SettingsDataSet.TreeRow Row { get; }

		public TreeRowEventArgs(SettingsDataSet.TreeRow row)
		{
			this.Row = row;
		}
	}
}