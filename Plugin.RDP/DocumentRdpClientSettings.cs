using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Plugin.RDP
{
	internal class DocumentRdpClientSettings : INotifyPropertyChanged
	{
		private Int32? _treeId;
		private Boolean _isConnected = true;

		public Int32? TreeId
		{
			get => this._treeId;
			set => this.SetField(ref this._treeId, value, nameof(this.TreeId));
		}

		public Boolean IsConnected
		{
			get => this._isConnected;
			set => this.SetField(ref this._isConnected, value, nameof(this.IsConnected));
		}

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private Boolean SetField<T>(ref T field, T value, String propertyName)
		{
			if(EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
		#endregion INotifyPropertyChanged
	}
}