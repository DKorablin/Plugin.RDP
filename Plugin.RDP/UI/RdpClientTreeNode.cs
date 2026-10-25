using System;
using System.Drawing;
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
			/// <summary>Disconnected client with alternative icon</summary>
			Client2Disconnected = 3,
			/// <summary>Connected client with alternative icon</summary>
			Client2Connected = 4,
		}

		private Boolean _isConnected;

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
		public ElementType ElementType { get; }

		/// <summary>Create a tree node instance with a server link</summary>
		/// <param name="row">Server link for testing</param>
		public RdpClientTreeNode(SettingsDataSet.TreeRow row)
			: this(row.Name, row.ElementType)
		{
			this.Tag = row;
			this.UpdateState();
		}

		public RdpClientTreeNode(String text, ElementType type)
			: base(text)
		{
			switch(type)
			{
			case ElementType.Client:
			case ElementType.Tree:
				this.ElementType = type;
				break;
			default:
				throw new NotImplementedException($"Element with type \"{type}\" not implemented");
			}
			this.UpdateImage();
		}

		/// <summary>This is a folder</summary>
		public Boolean IsFolderNode => this.ElementType == ElementType.Tree;

		/// <summary>The client settings are invalid and the client can't connect</summary>
		public Boolean IsInvalid
		{
			get
			{
				SettingsDataSet.RdpClientRow clientRow = this.IsFolderNode || this.Tag == null
					? null
					: this.Tag.RdpClientRow;
				return clientRow != null && clientRow.HasErrors;
			}
		}

		/// <summary>Show the client as disabled with validation errors in the tooltip when settings are invalid</summary>
		public void UpdateState()
		{
			this.UpdateImage();
			if(this.IsInvalid)
			{
				this.ForeColor = SystemColors.GrayText;
				this.ToolTipText = this.Tag.RdpClientRow.Error;
			} else
			{
				this.ForeColor = Color.Empty;
				this.ToolTipText = String.Empty;
			}
		}

		/// <summary>The client is connected to the server</summary>
		public Boolean IsConnected
		{
			get => this._isConnected;
			set
			{
				if(this.IsFolderNode)
					throw new InvalidOperationException();

				this._isConnected = value;
				this.UpdateImage();
			}
		}

		/// <summary>Show the folder image or the client image selected by the user in the client settings</summary>
		private void UpdateImage()
		{
			TreeImageList image;
			if(this.IsFolderNode)
				image = TreeImageList.Folder;
			else
			{
				Int16 iconId = this.Tag?.RdpClientRow?.IconIdI ?? 0;
				switch(iconId)
				{
				case 1:
					image = this._isConnected ? TreeImageList.Client2Connected : TreeImageList.Client2Disconnected;
					break;
				default:
					image = this._isConnected ? TreeImageList.ClientConnected : TreeImageList.ClientDisconnected;
					break;
				}
			}

			if(this.ImageIndex != image)
				this.ImageIndex = this.SelectedImageIndex = image;
		}
	}
}