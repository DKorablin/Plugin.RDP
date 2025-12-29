using System;
using System.Windows.Forms;
using AlphaOmega.Windows.Forms;
using Plugin.RDP.Bll;

namespace Plugin.RDP.UI
{
	internal class RdpClientTreeView : DraggableTreeView<RdpClientTreeNode, SettingsDataSet.TreeRow>
	{
		public PluginWindows Plugin { get; set; }

		public new RdpClientTreeNode SelectedNode
		{
			get => (RdpClientTreeNode)base.SelectedNode;
			set => base.SelectedNode = value;
		}

		protected override Boolean IsFolderNode(RdpClientTreeNode treeNode)
			=> treeNode.IsFolderNode;

		protected override void OnDragDrop(DragEventArgs args)
		{
			base.OnDragDrop(args);

			if(base.IsDataPresent(args) && base.HasMoved)
			{
				RdpClientTreeNode movingNode = base.GetDragDropNode(args);

				SettingsDataSet.TreeRow movingRow = movingNode.Tag;
				SettingsDataSet.TreeRow toRow = movingNode.Parent == null ? null : movingNode.Parent.Tag;

				Int32? parentTreeId = toRow == null ? (Int32?)null : toRow.TreeID;
				this.Plugin.Settings.XmlSettings.MoveNode(movingRow.TreeID, parentTreeId);

				Int32 orderId = 0;
				TreeNodeCollection nodes = movingNode.Parent == null
					? base.Nodes
					: movingNode.Parent.Nodes;

				foreach(RdpClientTreeNode node in nodes)
					node.Tag.OrderId = orderId++;

				this.Plugin.Settings.XmlSettings.Save();
			}
		}

		public RdpClientTreeNode FindNode(Int32 treeId)
		{
			foreach(RdpClientTreeNode node in base.EnumerateNodes(null))
				if(node.Tag.TreeID == treeId)
					return node;
			return null;
		}
	}
}