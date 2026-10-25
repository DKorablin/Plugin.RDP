using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Plugin.RDP.RDP;
using System.ComponentModel;

namespace Plugin.RDP.Bll
{
	/// <summary>Тип элемента</summary>
	public enum ElementType : byte
	{
		/// <summary>Элемент дерева</summary>
		Tree = 0,
		/// <summary>RDP клиент</summary>
		Client = 1,
	}
	partial class SettingsDataSet
	{
		/// <summary>Check whether the column value is really changed</summary>
		/// <remarks>Empty string and <see cref="DBNull"/> are the same value, because bound text controls can't keep null</remarks>
		/// <param name="previous">The column value before the change</param>
		/// <param name="current">The new column value</param>
		/// <returns>The value is different</returns>
		private static Boolean IsValueChanged(Object previous, Object current)
		{
			Boolean IsEmpty(Object value) => value == null || value == DBNull.Value || (value is String str && str.Length == 0);

			return IsEmpty(previous)
				? !IsEmpty(current)
				: !previous.Equals(current);
		}

		/// <summary>Get the name of the row property to notify about the column change</summary>
		/// <remarks>Typed wrapper with the "I" suffix is preferred, because the editor is bound to the wrappers</remarks>
		/// <param name="row">The changed row</param>
		/// <param name="column">The changed column</param>
		/// <returns>The row property name</returns>
		private static String GetPropertyName(DataRow row, DataColumn column)
		{
			String wrapperName = column.ColumnName + "I";
			return row.GetType().GetProperty(wrapperName, BindingFlags.Instance | BindingFlags.Public) == null
				? column.ColumnName
				: wrapperName;
		}

		partial class TreeDataTable
		{
			private Object _previousValue;

			protected override void OnColumnChanging(DataColumnChangeEventArgs e)
			{
				this._previousValue = e.Row.RowState == DataRowState.Detached ? null : e.Row[e.Column];
				base.OnColumnChanging(e);
			}

			protected override void OnColumnChanged(DataColumnChangeEventArgs e)
			{
				base.OnColumnChanged(e);
				if(e.Row.RowState != DataRowState.Detached && IsValueChanged(this._previousValue, e.ProposedValue))
					((TreeRow)e.Row).OnPropertyChanged(GetPropertyName(e.Row, e.Column));
			}
		}

		partial class RdpClientDataTable
		{
			private Object _previousValue;

			protected override void OnColumnChanging(DataColumnChangeEventArgs e)
			{
				this._previousValue = e.Row.RowState == DataRowState.Detached ? null : e.Row[e.Column];
				base.OnColumnChanging(e);
			}

			protected override void OnColumnChanged(DataColumnChangeEventArgs e)
			{
				base.OnColumnChanged(e);
				if(e.Row.RowState != DataRowState.Detached && IsValueChanged(this._previousValue, e.ProposedValue))
					((RdpClientRow)e.Row).OnPropertyChanged(GetPropertyName(e.Row, e.Column));
			}
		}

		partial class TreeRow : INotifyPropertyChanged
		{
			/// <summary>The column value is changed</summary>
			public event PropertyChangedEventHandler PropertyChanged;

			internal void OnPropertyChanged(String propertyName)
				=> this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

			/// <summary>Тип элемента в узле дерева</summary>
			public ElementType ElementType
			{
				get => (ElementType)this.ElementTypeID;
				set => this.ElementTypeID = (Byte)value;
			}

			/// <summary>Родительский идентификатор узла в дереве</summary>
			public Int32? ParentTreeIDI
			{
				get => this.IsParentTreeIDNull() ? (Int32?)null : this.ParentTreeID;
				set
				{
					if(value == null)
						this.SetParentTreeIDNull();
					else
						this.ParentTreeID = value.Value;
				}
			}

			/// <summary>Получить информацию о настройках клиента</summary>
			public RdpClientRow RdpClientRow
			{
				get
				{
					switch(this.ElementType)
					{
						case ElementType.Client:
							return ((SettingsDataSet)this.Table.DataSet).RdpClient.First(p => p.TreeID == this.TreeID);
						case Bll.ElementType.Tree:
							return null;
						default:
							throw new NotImplementedException(String.Format("Type: {0} not implemented", this.ElementType));
					}
				}
			}
		}

		partial class RdpClientRow : INotifyPropertyChanged, IDataErrorInfo
		{
			/// <summary>The column value is changed</summary>
			public event PropertyChangedEventHandler PropertyChanged;

			/// <summary>All validation errors of the client settings</summary>
			public String Error
				=> String.Join(Environment.NewLine, this.GetColumnsInError().Select(c => this.GetColumnError(c)).ToArray());

			String IDataErrorInfo.this[String columnName]
			{
				get
				{
					DataColumn column = this.Table.Columns[columnName];
					if(column == null && columnName.EndsWith("I"))
						column = this.Table.Columns[columnName.Substring(0, columnName.Length - 1)];
					return column == null ? String.Empty : this.GetColumnError(column);
				}
			}

			internal void OnPropertyChanged(String propertyName)
				=> this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

			/// <summary>Check required settings and store errors in the row</summary>
			/// <returns>Client settings are valid and the client can connect</returns>
			public Boolean Validate()
			{
				this.ClearErrors();
				RdpClientDataTable table = (RdpClientDataTable)this.Table;

				if(String.IsNullOrWhiteSpace(this.Server))
					this.SetColumnError(table.ServerColumn, "You need to specify remote computer to connect");
				if(String.IsNullOrWhiteSpace(this.UserName))
					this.SetColumnError(table.UserNameColumn, "You need to specify login");
				if(this.ConnectionTypeI == Plugin.RDP.ConnectionType.VirtualMachineConsoleConnect && String.IsNullOrWhiteSpace(this.VirtualMachineId))
					this.SetColumnError(table.VirtualMachineIdColumn, "Virtual machine ID is required");

				return !this.HasErrors;
			}

			/// <summary>Set default values from <see cref="DefaultValueAttribute"/> of the typed properties</summary>
			public void ResetToDefaults()
			{
				foreach(PropertyInfo property in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
					if(property.CanWrite && property.GetCustomAttribute<DefaultValueAttribute>(false) is DefaultValueAttribute attr)
						property.SetValue(this, attr.Value, null);
			}

			public String PasswordI
			{
				get => this.IsPasswordNull() ? null : this.Password;
				set
				{
					if(String.IsNullOrEmpty(value))
						this.SetPasswordNull();
					else
						this.Password = value;
				}
			}

			public String DomainI
			{
				get => this.IsDomainNull() ? null : this.Domain;
				set
				{
					if(String.IsNullOrEmpty(value))
						this.SetDomainNull();
					else
						this.Domain = value;
				}
			}

			[DefaultValue((Int16)0)]
			public Int16 IconIdI
			{
				get => this.IsIconIdNull() ? (Int16)0 : this.IconId;
				set
				{
					if(value < 1)
						this.SetIconIdNull();
					else
						this.IconId = value;
				}
			}

			[DefaultValue(RDP.RdpClient.DefaultRDPPort)]
			public Int32 ConnectPortI
			{
				get => this.IsConnectPortNull() ? RDP.RdpClient.DefaultRDPPort : this.ConnectPort;
				set
				{
					if(value == 0 || value == RDP.RdpClient.DefaultRDPPort)
						this.SetConnectPortNull();
					else
						this.ConnectPort = value;
				}
			}

			[DefaultValue(Plugin.RDP.RDP.RdpClient.DefaultColorDepth)]
			public Int32 ColorDepthI
			{
				get => this.IsColorDepthNull() ? RDP.RdpClient.DefaultColorDepth : this.ColorDepth;
				set
				{
					if(value == 0 || value == RDP.RdpClient.DefaultColorDepth)
						this.SetColorDepthNull();
					else
						this.ColorDepth = value;
				}
			}

			[DefaultValue(Plugin.RDP.ConnectionType.Normal)]
			public ConnectionType ConnectionTypeI
			{
				get => (ConnectionType)this.ConnectionType;
				set => this.ConnectionType = (SByte)value;
			}

			[DefaultValue(Plugin.RDP.TS_PERF.DISABLE_NOTHING)]
			public TS_PERF PerformanceFlagsI
			{
				get => (TS_PERF)this.PerformanceFlags;
				set => this.PerformanceFlags = (Int32)value;
			}

			[DefaultValue(Plugin.RDP.RedirectFlags.Clipboard)]
			public RedirectFlags RedirectI
			{
				get => (RedirectFlags)this.Redirect;
				set => this.Redirect = (Int32)value;
			}

			[DefaultValue(Plugin.RDP.RDP.RdpClient.AudioRedirectionMode.Redirect)]
			public RdpClient.AudioRedirectionMode RedirectAudioI
			{
				get => (RdpClient.AudioRedirectionMode)this.RedirectAudio;
				set => this.RedirectAudio = (UInt32)value;
			}

			[DefaultValue(Plugin.RDP.RDP.RdpClient.AudioRedirectionQuality.Dynamic)]
			public RdpClient.AudioRedirectionQuality RedirectAudioQualityI
			{
				get => (RdpClient.AudioRedirectionQuality)this.RedirectAudioQuality;
				set => this.RedirectAudioQuality = (UInt32)value;
			}

			public String RunApplicationPathI
			{
				get { return this.IsRunApplicationPathNull() ? String.Empty : this.RunApplicationPath; }
				set
				{
					if(String.IsNullOrEmpty(value))
						this.SetRunApplicationPathNull();
					else
						this.RunApplicationPath = value;
				}
			}

			public String RunApplicationWorkingDirI
			{
				get { return this.IsRunApplicationWorkingDirNull() ? String.Empty : this.RunApplicationWorkingDir; }
				set
				{
					if(String.IsNullOrEmpty(value))
						this.SetRunApplicationWorkingDirNull();
					else
						this.RunApplicationWorkingDir = value;
				}
			}

			[DefaultValue(Plugin.RDP.AuthenticationLevel.NoAuthentication)]
			public AuthenticationLevel AuthenticationLevelI
			{
				get => (AuthenticationLevel)this.AuthenticationLevel;
				set => this.AuthenticationLevel = (UInt32)value;
			}

			[DefaultValue(RDP.RdpClient.KeyboardHookMode.FullScreen)]
			public RdpClient.KeyboardHookMode KeyboardHookI
			{
				get => (RdpClient.KeyboardHookMode)this.KeyboardHook;
				set => this.KeyboardHook = (Int32)value;
			}

			public DesktopSizeParser DesktopSizeI
			{
				get => this.IsDesktopSizeNull()
					? new DesktopSizeParser()
					: new DesktopSizeParser(this.DesktopSize);
				set
				{
					if(value == null || value.SameAsClient)
						this.SetDesktopSizeNull();
					else
						this.DesktopSize = value.ToString();
				}
			}

			[DefaultValue(RDP.RdpClient.GatewayLogonMethod.Any)]
			public RdpClient.GatewayLogonMethod GatewayLogonMethodI
			{
				get => (RdpClient.GatewayLogonMethod)this.GatewayLogonMethod;
				set => this.GatewayLogonMethod = (UInt32)value;
			}
		}
	}
}