using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plugin.RDP.Bll;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.RDP
{
	/// <summary>http://www.codeproject.com/KB/cs/RemoteDesktop_CSharpNET.aspx</summary>
	public class PluginWindows : IPlugin, IPluginSettings<PluginSettings>
	{
		private PluginSettings _settings;
		private Dictionary<String, DockState> _documentTypes;

		internal ITraceSource Trace { get; }

		internal IHostWindows HostWindows { get; }

		/// <summary>Settings for interaction from the host</summary>
		Object IPluginSettings.Settings => this.Settings;

		/// <summary>Settings for interaction from the plugin</summary>
		public PluginSettings Settings
		{
			get
			{
				if(this._settings == null)
				{
					this._settings = new PluginSettings(this);
					this.HostWindows.Plugins.Settings(this).LoadAssemblyParameters(this._settings);
				}
				return this._settings;
			}
		}

		internal IMenuItem RdpClientMenu { get; set; }

		private Dictionary<String, DockState> DocumentTypes
		{
			get
			{
				if(this._documentTypes == null)
					this._documentTypes = new Dictionary<String, DockState>()
					{
						{ typeof(DocumentRdpClient).ToString(), DockState.Document },
						{ typeof(PanelRdpClient).ToString(), DockState.DockLeft },
					};
				return this._documentTypes;
			}
		}

		public PluginWindows(IHostWindows hostWindows, ITraceSource trace)
		{
			this.HostWindows = hostWindows ?? throw new ArgumentNullException(nameof(hostWindows));
			this.Trace = trace ?? throw new ArgumentNullException(nameof(trace));
		}

		public IWindow GetPluginControl(String typeName, Object args)
			=> this.CreateWindow(typeName, false, args);

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			IHostWindows host = this.HostWindows;
			if(host == null)
				this.Trace.TraceEvent(TraceEventType.Error, 10, "Plugin {0} requires {1}", this, typeof(IHostWindows));
			else
			{
				IMenuItem menuTools = host.MainMenu.FindMenuItem("Tools");
				if(menuTools == null)
					this.Trace.TraceEvent(TraceEventType.Error, 10, "Menu item 'Tools' not found");
				else
				{
					this.RdpClientMenu = menuTools.Create("RDP Client");
					this.RdpClientMenu.Name = "Tools.RdpClient";
					this.RdpClientMenu.Click += (sender, e) => this.CreateWindow(typeof(PanelRdpClient).ToString(), true);
					menuTools.Items.Add(this.RdpClientMenu);
					return true;
				}
			}
			return false;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			if(this.RdpClientMenu != null)
				this.HostWindows.MainMenu.Items.Remove(this.RdpClientMenu);
			return true;
		}

		internal IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
			=> this.DocumentTypes.TryGetValue(typeName, out DockState state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;

		internal IWindow CreateWindow<T, A>(A args) where T : class, IPluginSettings<A> where A : class
		{
			String type = typeof(T).ToString();
			return this.DocumentTypes.TryGetValue(type, out DockState state)
				? this.HostWindows.Windows.CreateWindow(this, type, true, state, args)
				: null;
		}
	}
}