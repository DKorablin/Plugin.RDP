using System;
using System.Data;
using System.IO;
using System.Linq;
using AlphaOmega.Bll;

namespace Plugin.RDP.Bll
{
	internal class SettingsBll : BllBase<SettingsDataSet,SettingsDataSet.TreeRow>
	{
		private readonly PluginWindows _plugin;

		/// <summary>Event raised when a client is created or modified</summary>
		public event EventHandler<TreeRowEventArgs> RdpClientUpdated;

		/// <summary>Event when the connection status to the client changes</summary>
		public event EventHandler<RdpStateEventArgs> RdpClientStateChange;

		public SettingsBll(PluginWindows plugin)
			: base(0)
		{
			this._plugin = plugin;

			using(Stream stream = this._plugin.HostWindows.Plugins.Settings(this._plugin).LoadAssemblyBlob(Constant.SettingsFileName))
				if(stream != null)
					base.DataSet.ReadXml(stream);

			foreach(SettingsDataSet.RdpClientRow clientRow in base.DataSet.RdpClient)
				clientRow.Validate();//Errors are not stored in the settings file
		}

		public override void Save()
		{
			using(MemoryStream stream = new MemoryStream())
			{
				base.DataSet.WriteXml(stream);
				this._plugin.HostWindows.Plugins.Settings(this._plugin).SaveAssemblyBlob(Constant.SettingsFileName, stream);
			}
			//base.Save();
		}

		/// <summary>Event triggered when a connection to a client has been established</summary>
		/// <param name="clientName">Client ID in the tree</param>
		public void OnClientConnect(Int32 treeId)
			=> this.OnClientChangeState(treeId, RdpStateEventArgs.StateType.Connect);

		/// <summary>Event triggered when a client disconnects from the server</summary>
		/// <param name="treeId">Client ID in the tree</param>
		public void OnClientDisconnect(Int32 treeId)
			=> this.OnClientChangeState(treeId, RdpStateEventArgs.StateType.Disconnect);

		/// <summary>Event for changing or sending a new status for a window</summary>
		/// <param name="treeId">Client ID in the tree</param>
		/// <param name="state">Requested operation to perform in the client window</param>
		public void OnClientChangeState(Int32 treeId, RdpStateEventArgs.StateType state)
			=> this.RdpClientStateChange?.Invoke(this, new RdpStateEventArgs(treeId, state));

		/// <summary>Event when a client is updated or added</summary>
		/// <param name="row">The client row that was updated</param>
		protected void OnClientUpdated(SettingsDataSet.TreeRow row)
			=> this.RdpClientUpdated?.Invoke(this, new TreeRowEventArgs(row));

		/// <summary>Get a tree node by ID</summary>
		/// <param name="treeId">Tree node ID</param>
		/// <returns>A row describing the tree node</returns>
		public SettingsDataSet.TreeRow GetTreeNode(Int32 treeId)
			=> base.DataSet.Tree.FirstOrDefault(p => p.TreeID == treeId);

		/// <summary>Get all child nodes relative to the parent</summary>
		/// <param name="parentTreeId">Parent node ID</param>
		/// <returns>Array of child nodes</returns>
		public SettingsDataSet.TreeRow[] GetTreeNodes(Int32? parentTreeId)
			=> base.DataSet.Tree.Where(p => p.ParentTreeIDI == parentTreeId).OrderBy(p=>p.OrderId).ToArray();

		/// <summary>Get RDP client connection settings by node ID</summary>
		/// <param name="treeId">Node ID for which to get connection settings</param>
		/// <returns>A row describing the RDP client settings</returns>
		public SettingsDataSet.RdpClientRow GetClientRow(Int32 treeId)
			=> base.DataSet.RdpClient.FirstOrDefault(p => p.TreeID == treeId);

		/// <summary>Get a list of computers that are already in the settings</summary>
		/// <returns>Array of computers</returns>
		public String[] GetServerList()
			=> base.DataSet.RdpClient.Select(p => p.Server).Distinct().ToArray();

		/// <summary>Move a node in the tree to another group</summary>
		/// <param name="treeId">The ID of the node to move</param>
		/// <param name="parentTreeId">The parent ID to move the node to</param>
		public void MoveNode(Int32 treeId, Int32? parentTreeId)
		{
			SettingsDataSet.TreeRow row = this.GetTreeNode(treeId);
			_ = row ?? throw new ArgumentNullException(nameof(treeId));

			if(parentTreeId != null)
			{
				SettingsDataSet.TreeRow parentRow = this.GetTreeNode(parentTreeId.Value)
					?? throw new ArgumentNullException(nameof(parentTreeId));

				if(parentRow.ElementType == ElementType.Client) throw new ArgumentException("Can't move to a client node");
			}

			if(row.ParentTreeIDI != parentTreeId)//He's already in this node
			{
				row.BeginEdit();
				row.ParentTreeIDI = parentTreeId;
				row.AcceptChanges();
			}
		}

		/// <summary>Modify a tree node</summary>
		/// <param name="treeId">The node ID, if needed, to modify. Or null if the node needs to be added.</param>
		/// <param name="parentTreeId">The parent node ID.</param>
		/// <param name="elementType">The type of element to add.</param>
		/// <param name="name">The name of the element to add.</param>
		/// <returns>A pointer to the added or modified code.</returns>
		public SettingsDataSet.TreeRow ModifyTreeNode(Int32? treeId, Int32? parentTreeId, ElementType elementType, String name)
		{
			if(String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			SettingsDataSet.TreeRow row = treeId == null
				? base.DataSet.Tree.NewTreeRow()
				: this.GetTreeNode(treeId.Value);

			_ = row ?? throw new ArgumentNullException(nameof(treeId));

			row.BeginEdit();
			row.ParentTreeIDI = parentTreeId;
			row.ElementType = elementType;
			row.Name = name.Trim();

			if(row.RowState == DataRowState.Detached)
				base.DataSet.Tree.AddTreeRow(row);
			else
				row.AcceptChanges();

			return row;
		}

		/// <summary>Change the name of a node in the tree</summary>
		/// <param name="row">Row to change</param>
		/// <param name="name">Node name</param>
		/// <returns>Node change successful</returns>
		public static Boolean ModifyTreeNodeName(SettingsDataSet.TreeRow row, String name)
		{
			_ = row ?? throw new ArgumentNullException(nameof(row));
			if(String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			Boolean result = name != row.Name;
			if(result)
			{
				row.BeginEdit();
				row.Name = name.Trim();
				row.AcceptChanges();
			}
			return result;
		}

		/// <summary>Create a new client with default settings</summary>
		/// <remarks>The client is invalid until the remote computer is specified</remarks>
		/// <param name="parentTreeId">The parent node ID or null to add client to the root</param>
		/// <param name="name">The client node name</param>
		/// <returns>The tree row of the created client</returns>
		public SettingsDataSet.TreeRow CreateClient(Int32? parentTreeId, String name)
		{
			SettingsDataSet.TreeRow treeRow = this.ModifyTreeNode(null, parentTreeId, ElementType.Client, name);

			SettingsDataSet.RdpClientRow clientRow = base.DataSet.RdpClient.NewRdpClientRow();
			clientRow.BeginEdit();
			clientRow.ResetToDefaults();
			clientRow.TreeID = treeRow.TreeID;
			clientRow.Server = String.Empty;
			clientRow.UserName = Environment.UserName;
			clientRow.DomainI = Environment.UserDomainName;
			base.DataSet.RdpClient.AddRdpClientRow(clientRow);

			clientRow.Validate();
			return treeRow;
		}

		/// <summary>Validate the changed client settings, save them and notify about the change</summary>
		/// <param name="treeRow">Tree row of the changed client or folder</param>
		public void CommitClient(SettingsDataSet.TreeRow treeRow)
		{
			_ = treeRow ?? throw new ArgumentNullException(nameof(treeRow));

			this.GetClientRow(treeRow.TreeID)?.Validate();
			this.Save();
			this.OnClientUpdated(treeRow);
		}

		/// <summary>Create a new client from the Remote Desktop Connection (.rdp) file</summary>
		/// <param name="parentTreeId">The parent node ID or null to add client to the root</param>
		/// <param name="filePath">Path to the .rdp file</param>
		/// <returns>The tree row of the imported client</returns>
		public SettingsDataSet.TreeRow ImportClient(Int32? parentTreeId, String filePath)
		{
			RdpFile file = RdpFile.Load(filePath);

			SettingsDataSet.RdpClientRow clientRow = base.DataSet.RdpClient.NewRdpClientRow();
			clientRow.BeginEdit();
			file.ToClient(clientRow);//Parse before adding the tree node so an invalid file leaves no orphan nodes

			String name = Path.GetFileNameWithoutExtension(filePath);
			SettingsDataSet.TreeRow treeRow = this.ModifyTreeNode(null, parentTreeId, ElementType.Client, String.IsNullOrWhiteSpace(name) ? clientRow.Server : name);

			clientRow.TreeID = treeRow.TreeID;
			base.DataSet.RdpClient.AddRdpClientRow(clientRow);
			clientRow.Validate();

			this.OnClientUpdated(treeRow);
			return treeRow;
		}

		/// <summary>Save the client settings to the Remote Desktop Connection (.rdp) file</summary>
		/// <param name="treeRow">Tree row of the client to export</param>
		/// <param name="filePath">Path to the .rdp file</param>
		/// <param name="includePassword">Store the password encrypted for the current Windows user</param>
		public void ExportClient(SettingsDataSet.TreeRow treeRow, String filePath, Boolean includePassword)
		{
			_ = treeRow ?? throw new ArgumentNullException(nameof(treeRow));

			SettingsDataSet.RdpClientRow clientRow = this.GetClientRow(treeRow.TreeID)
				?? throw new ArgumentException($"Row with client ID {treeRow.TreeID} not found");

			RdpFile.FromClient(treeRow, clientRow, includePassword).Save(filePath);
		}

		/// <summary>Delete a tree node and all its child nodes</summary>
		/// <param name="row">The tree row to delete</param>
		public void RemoveNode(SettingsDataSet.TreeRow row)
		{
			_ = row ?? throw new ArgumentNullException(nameof(row));

			switch(row.ElementType)
			{
				case ElementType.Tree:
					foreach(SettingsDataSet.TreeRow treeRow in this.GetTreeNodes(row.TreeID))
						this.RemoveNode(treeRow);
					base.DataSet.Tree.RemoveTreeRow(row);
					break;
				case ElementType.Client:
					this.RemoveClient(row);
					break;
				default:
					throw new NotImplementedException($"Element with type {row.ElementType} not implemented");
			}
		}

		/// <summary>Delete client node and client settings</summary>
		/// <param name="treeRow">Tree row to delete</param>
		private void RemoveClient(SettingsDataSet.TreeRow treeRow)
		{
			_ = treeRow ?? throw new ArgumentNullException(nameof(treeRow));

			if(treeRow.ElementType == ElementType.Client)
			{
				SettingsDataSet.RdpClientRow clientRow = this.GetClientRow(treeRow.TreeID)
					?? throw new ArgumentException($"Row with client ID {treeRow.TreeID} not found");

				base.DataSet.RdpClient.RemoveRdpClientRow(clientRow);

				base.DataSet.Tree.RemoveTreeRow(treeRow);
			}
		}
	}
}