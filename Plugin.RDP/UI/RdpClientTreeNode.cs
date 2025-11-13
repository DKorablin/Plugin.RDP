using System;
using Plugin.RDP.Bll;
using AlphaOmega.Windows.Forms;

namespace Plugin.RDP.UI
{
	internal class RdpClientTreeNode : TreeNode2<SettingsDataSet.TreeRow>
	{
		/// <summary>Tree node images</summary>
		public enum TreeImageList
		{
			/// <summary>Folder</summary>
			Folder = 0,
			/// <summary>Disconnected client</summary>
			ClientDisconnected = 1,
			/// <summary>Connected client</summary>
			ClientConnected = 2,
		}

		/// <summary>Parent test in a tree</summary>
		public new RdpClientTreeNode Parent => (RdpClientTreeNode)base.Parent;

		public new RdpClientTreeNode PrevNode => (RdpClientTreeNode)base.PrevNode;

		/// <summary>Test status image index</summary>
		public new TreeImageList SelectedImageIndex
		{
			get => (TreeImageList)base.SelectedImageIndex;
			set => base.SelectedImageIndex = (Int32)value;
		}

		/// <summary>Test status image index</summary>
		public new TreeImageList ImageIndex
		{
			get => (TreeImageList)base.ImageIndex;
			set => base.ImageIndex = (Int32)value;
		}

		/// <summary>The element type</summary>
		public ElementType ElementType
			=> this.ImageIndex == TreeImageList.Folder
				? ElementType.Tree
				: ElementType.Client;

		/// <summary>Create a tree node instance with a server link</summary>
		/// <param name="row">Server link for testing</param>
		public RdpClientTreeNode(SettingsDataSet.TreeRow row)
			: this(row.Name, row.ElementType)
			=> this.Tag = row;

		public RdpClientTreeNode(String text, ElementType type)
			: base(text)
		{
			switch(type)
			{
			case ElementType.Client:
				this.ImageIndex = this.SelectedImageIndex = TreeImageList.ClientDisconnected;
				break;
			case ElementType.Tree:
				this.ImageIndex = this.SelectedImageIndex = TreeImageList.Folder;
				break;
			default:
				throw new NotImplementedException($"Element with type \"{type}\" not implemented");
			}
		}

		/// <summary>This is a folder</summary>
		public Boolean IsFolderNode => this.ImageIndex == TreeImageList.Folder;

		/// <summary>The client is connected to the server</summary>
		public Boolean IsConnected
		{
			get => this.ImageIndex == TreeImageList.ClientConnected;
			set
			{
				if(this.IsFolderNode)
					throw new InvalidOperationException();

				this.ImageIndex = this.SelectedImageIndex = value
					? TreeImageList.ClientConnected
					: TreeImageList.ClientDisconnected;
			}
		}
	}
}