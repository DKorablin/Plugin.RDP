using System;
using System.Linq;
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
		partial class TreeRow
		{
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

		partial class RdpClientRow
		{
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
				get { return this.IsConnectPortNull() ? RDP.RdpClient.DefaultRDPPort : this.ConnectPort; }
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
				get { return this.IsColorDepthNull() ? RDP.RdpClient.DefaultColorDepth : this.ColorDepth; }
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