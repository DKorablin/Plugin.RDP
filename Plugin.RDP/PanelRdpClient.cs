using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Plugin.RDP.Bll;
using Plugin.RDP.UI;
using SAL.Windows;
using System.Diagnostics;
using Plugin.RDP.Properties;

namespace Plugin.RDP
{
	internal partial class PanelRdpClient : UserControl
	{
		private PluginWindows Plugin => (PluginWindows)this.Window.Plugin;
		private IWindow Window => (IWindow)base.Parent;

		public PanelRdpClient()
		{
			InitializeComponent();
			gridSearch.TreeView = tvList;
		}

		protected override void OnCreateControl()
		{
			tvList.Plugin = this.Plugin;
			this.Window.Caption = "RDP Client";
			this.Window.SetDockAreas(DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.Float);
			this.Window.SetTabPicture(Resources.iconRDP);
			this.Window.Closed += new EventHandler(Window_Closed);

			this.Plugin.Settings.XmlSettings.RdpClientStateChange += this.XmlSettings_RdpClientConnected;
			this.Plugin.Settings.XmlSettings.RdpClientUpdated += this.XmlSettings_RdpClientUpdated;

			if(!bgList.IsBusy)
				bgList.RunWorkerAsync();

			base.OnCreateControl();
		}

		private void XmlSettings_RdpClientUpdated(Object sender, TreeRowEventArgs e)
		{
			RdpClientTreeNode node = tvList.FindNode(e.Row.TreeID);
			if(node == null)
			{
				node = new RdpClientTreeNode(e.Row);
				if(e.Row.ParentTreeIDI == null)
					tvList.Nodes.Add(node);
				else
				{
					RdpClientTreeNode parentNode = tvList.FindNode(e.Row.ParentTreeID);
					parentNode.Nodes.Add(node);
					parentNode.Expand();
				}
			} else
				node.Text = e.Row.Name;
			node.EnsureVisible();
		}

		private void XmlSettings_RdpClientConnected(Object sender, RdpStateEventArgs e)
		{
			RdpClientTreeNode node = tvList.FindNode(e.TreeId)
				?? throw new ArgumentException($"Node {e.TreeId:n0} not found", "e.TreeId");

			node.IsConnected = e.State==RdpStateEventArgs.StateType.Connect;
		}

		private void Window_Closed(Object sender, EventArgs e)
		{
			this.Plugin.Settings.XmlSettings.RdpClientStateChange -= this.XmlSettings_RdpClientConnected;
			this.Plugin.Settings.XmlSettings.RdpClientUpdated -= this.XmlSettings_RdpClientUpdated;
		}

		private void bgList_DoWork(Object sender, System.ComponentModel.DoWorkEventArgs e)
			=> e.Result = this.FillList(null, null);

		private void bgList_RunWorkerCompleted(Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			if(!e.Cancelled)
			{
				if(e.Error != null)
					this.Plugin.Trace.TraceData(TraceEventType.Error, 10, e.Error);
				else
					tvList.Nodes.AddRange((RdpClientTreeNode[])e.Result);
			}
		}

		private RdpClientTreeNode[] FillList(RdpClientTreeNode parentNode, Int32? parentTreeId)
		{
			List<RdpClientTreeNode> result = new List<RdpClientTreeNode>();
			foreach(SettingsDataSet.TreeRow row in this.Plugin.Settings.XmlSettings.GetTreeNodes(parentTreeId))
			{
				RdpClientTreeNode node = new RdpClientTreeNode(row);
				if(parentNode == null)
					result.Add(node);
				else
				{
					parentNode.Nodes.Add(node);
					parentNode.Expand();
				}

				switch(row.ElementType)
				{
				case ElementType.Tree:
					this.FillList(node, row.TreeID);
					break;
				case ElementType.Client:
					foreach(IWindow wnd in this.Plugin.HostWindows.Windows)
					{
						DocumentRdpClient doc = wnd.Control as DocumentRdpClient;
						if(doc != null && doc.Settings.TreeId == row.TreeID)
						{
							node.IsConnected = doc.Settings.IsConnected;
							break;
						}
					}
					break;
				}
			}
			return result.ToArray();
		}

		private void tsbnAdd_Click(Object sender, EventArgs e)
			=> this.tsbnAdd_DropDownItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiAddClient));

		private void tsbnAdd_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem == tsmiAddClient || e.ClickedItem == tsmiNodeAddClient)
			{
				SettingsDataSet.TreeRow parentRow = tvList.SelectedNode != null && tvList.SelectedNode.ElementType == ElementType.Tree
					? tvList.SelectedNode.Tag
					: null;

				this.Plugin.InvokeCreateClientDlg(parentRow);
			} else if(e.ClickedItem == tsmiAddNode || e.ClickedItem == tsmiNodeAddNode)
			{
				RdpClientTreeNode node = new RdpClientTreeNode(null, ElementType.Tree);
				if(tvList.SelectedNode != null && tvList.SelectedNode.ElementType == ElementType.Tree)
				{
					tvList.SelectedNode.Nodes.Add(node);
					tvList.SelectedNode.Expand();
				} else
					tvList.Nodes.Add(node);
				node.BeginEdit();
			} else
				throw new NotImplementedException(String.Format("Element {0} not implemented", e.ClickedItem));
		}

		private void tsbnDelete_Click(Object sender, EventArgs e)
		{
			SettingsDataSet.TreeRow row = tvList.SelectedNode.Tag;
			String message;
			switch(row.ElementType)
			{
			case ElementType.Client:
				message = $"Are you sure you want to remove client node {row.Name}?";
				break;
			case ElementType.Tree:
				message = String.Format("Are you sure you want to remove node {0} and all children{1}?", row.Name, tvList.SelectedNode.Nodes.Count == 0 ? String.Empty : "s");
				break;
			default:
				throw new NotImplementedException();
			}
			if(MessageBox.Show(message, this.Window.Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{//Removing a node from settings
				this.Plugin.Settings.XmlSettings.RemoveNode(row);
				this.Plugin.Settings.XmlSettings.Save();
				tvList.SelectedNode.Remove();
			}
		}

		private void tvList_AfterLabelEdit(Object sender, NodeLabelEditEventArgs e)
		{
			RdpClientTreeNode node = (RdpClientTreeNode)e.Node;
			if(e.Label == null)//Cancel editing
			{
				if(node.Tag == null)//Deleting a node if the user canceled creation
					node.Remove();
				return;
			}

			if(node.PrevNode != null)
			{
				RdpClientTreeNode itNode = node;
				do
				{
					itNode = itNode.PrevNode;
					if(itNode.Text.Equals(e.Label, StringComparison.InvariantCultureIgnoreCase))
					{
						MessageBox.Show("There is already node with the same name in this branch", e.Label, MessageBoxButtons.OK, MessageBoxIcon.Stop);
						e.CancelEdit = true;
						if(node.Tag != null)
							node.Remove();
						return;
					}
				} while(itNode.PrevNode != null);
			}

			if(node.Tag != null)//Modifying a previously created node
				this.Plugin.Settings.XmlSettings.ModifyTreeNodeName(node.Tag, e.Label);
			else
			{//Adding a new node
				SettingsDataSet.TreeRow parentRow = node.Parent == null ? null : node.Parent.Tag;
				switch(node.ElementType)
				{
				case ElementType.Client://Adding a new client
					throw new NotImplementedException();
				case ElementType.Tree://Adding a new node to the tree
					{
						SettingsDataSet.TreeRow newRow = this.Plugin.Settings.XmlSettings.ModifyTreeNode(null, parentRow == null ? (Int32?)null : parentRow.TreeID, ElementType.Tree, e.Label);
						node.Tag = newRow;
						node.Text = e.Label;
					}
					break;
				default:
					throw new NotImplementedException($"Element with type {node.ElementType} not implemented");
				}
			}

			this.Plugin.Settings.XmlSettings.Save();
		}

		private void tvList_KeyDown(Object sender, KeyEventArgs e)
		{
			switch(e.KeyData)
			{
			case Keys.Delete:
			case Keys.Back:
				if(tvList.SelectedNode != null)
				{
					this.tsbnDelete_Click(this, e);
					e.Handled = true;
				}
				break;
			case Keys.N|Keys.Control:
					this.tsbnAdd_DropDownItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiAddClient));
				e.Handled = true;
				break;
			case Keys.Return:
				if(tvList.SelectedNode != null && !tvList.SelectedNode.IsFolderNode)
				{
					if(tvList.SelectedNode.IsConnected)
						this.cmsClient_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiClientFocus));
					else
						this.cmsClient_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiClientConnect));
					e.Handled = true;
				}
				break;
			case Keys.Return | Keys.Alt:
				if(tvList.SelectedNode != null && !tvList.SelectedNode.IsFolderNode)
				{
					this.cmsClient_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiClientProperties));
					e.Handled = true;
				}
				break;
			case Keys.F2:
				if(tvList.SelectedNode != null)
				{
					e.Handled = true;
					tvList.SelectedNode.BeginEdit();
				}
				break;
			}
		}

		private void tvList_MouseClick(Object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				TreeViewHitTestInfo info = tvList.HitTest(e.Location);
				if(info.Node != null)
				{
					RdpClientTreeNode node = (RdpClientTreeNode)info.Node;
					tvList.SelectedNode = node;
					switch(node.ElementType)
					{
					case ElementType.Client:
						cmsClient.Show(tvList, e.Location);
						break;
					case ElementType.Tree:
						cmsNode.Show(tvList, e.Location);
						break;
					default:
						throw new NotImplementedException($"RMB action for element {node.ElementType} not implemented");
					}
				}
			}
		}

		private void tvList_MouseDoubleClick(Object sender, MouseEventArgs e)
		{
			TreeViewHitTestInfo info = tvList.HitTest(e.Location);
			RdpClientTreeNode node = (RdpClientTreeNode)info.Node;
			if(node != null && !node.IsFolderNode)
			{
				if(node.IsConnected)
					this.cmsClient_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiClientFocus));
				else
					this.cmsClient_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiClientConnect));
			}
		}

		private void tvList_AfterSelect(Object sender, TreeViewEventArgs e)
		{
			tsbnDelete.Enabled = e.Node != null;
			if(e.Node != null)
			{
				RdpClientTreeNode node = (RdpClientTreeNode)e.Node;
				this.Plugin.OnRowSelected(node.Tag);
			}
		}

		private void cmsClient_Opening(Object sender, System.ComponentModel.CancelEventArgs e)
		{
			RdpClientTreeNode node = tvList.SelectedNode;
			if(!node.IsFolderNode)
			{
				tsmiClientConnect.Visible = !node.IsConnected;
				tsmiClientDisconnect.Visible = node.IsConnected;
				tsmiClientFocus.Visible = node.IsConnected;
				tsmiClientLogOff.Visible = node.IsConnected;
			} else
				e.Cancel = true;
		}

		private void cmsClient_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			cmsClient.Visible = false;
			SettingsDataSet.TreeRow row = tvList.SelectedNode.Tag;

			if(e.ClickedItem == tsmiClientFocus)
			{
				this.Plugin.Settings.XmlSettings.OnClientChangeState(row.TreeID, RdpStateEventArgs.StateType.Focus);
			} else if(e.ClickedItem == tsmiClientConnect)
			{//Connecting a client to a server
				if(row.ElementType == ElementType.Client)
				{
					_ = this.Plugin.CreateWindow(typeof(DocumentRdpClient).ToString(),
						true,
						new DocumentRdpClientSettings() { TreeId = row.TreeID });
				} else
					throw new InvalidOperationException();
			} else if(e.ClickedItem == tsmiClientDisconnect)
			{//Disconnecting the client from the server
				if(MessageBox.Show($"Are you sure you want to disconnect from client {row.Name}?", this.Window.Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					this.Plugin.Settings.XmlSettings.OnClientChangeState(row.TreeID, RdpStateEventArgs.StateType.Disconnect);
			} else if(e.ClickedItem == tsmiClientListSessions)
			{//Displaying a list of sessions to the server
				using(RemoteSessionsDlg dlg = new RemoteSessionsDlg(this.Plugin, row.RdpClientRow.Server))
					dlg.ShowDialog();
			} else if(e.ClickedItem == tsmiClientLogOff)
			{//Client exits the server
				if(MessageBox.Show(String.Format("Are you sure you want to logOff from client {0}?", row.Name), this.Window.Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					SettingsDataSet.RdpClientRow clientRow = this.Plugin.Settings.XmlSettings.GetClientRow(row.TreeID);
					if(RemoteSessionsDlg.LogOffUser(clientRow.Server, clientRow.UserName))
						this.Plugin.Settings.XmlSettings.OnClientChangeState(row.TreeID, RdpStateEventArgs.StateType.Disconnect);
				}
			} else if(e.ClickedItem == tsmiClientProperties)
			{//Displaying server connection properties
				if(row.ElementType == ElementType.Client)
					this.Plugin.InvokeModifyClientDlg(row);
				else
					throw new InvalidOperationException();
			} else
				throw new NotImplementedException(e.ClickedItem.ToString());
		}

		private void cmsNode_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			cmsNode.Visible = false;
			if(e.ClickedItem == tsmiNodeAddClient)
				this.tsbnAdd_DropDownItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiAddClient));
			else if(e.ClickedItem == tsmiNodeAddNode)
				this.tsbnAdd_DropDownItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiNodeAddNode));
			else
				throw new NotImplementedException($"Action for node {e.ClickedItem} not implemented");
		}
	}
}