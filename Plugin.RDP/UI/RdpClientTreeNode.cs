using System;
using Plugin.RDP.Bll;
using AlphaOmega.Windows.Forms;

namespace Plugin.RDP.UI
{
	internal class RdpClientTreeNode : TreeNode2<SettingsDataSet.TreeRow>
	{
		/// <summary>Изображения узлов дерева</summary>
		public enum TreeImageList
		{
			/// <summary>Папка</summary>
			Folder = 0,
			/// <summary>Отключённый клиент</summary>
			ClientDisconnected = 1,
			/// <summary>Подключённый клиент</summary>
			ClientConnected = 2,
		}

		/// <summary>Родительский тест в дереве</summary>
		public new RdpClientTreeNode Parent => (RdpClientTreeNode)base.Parent;

		public new RdpClientTreeNode PrevNode => (RdpClientTreeNode)base.PrevNode;

		/// <summary>Индекс изображения статуса теста</summary>
		public new TreeImageList SelectedImageIndex
		{
			get => (TreeImageList)base.SelectedImageIndex;
			set => base.SelectedImageIndex = (Int32)value;
		}

		/// <summary>Индекс изображения статуса теста</summary>
		public new TreeImageList ImageIndex
		{
			get => (TreeImageList)base.ImageIndex;
			set => base.ImageIndex = (Int32)value;
		}

		/// <summary>Тип элемента</summary>
		public ElementType ElementType
			=> this.ImageIndex == TreeImageList.Folder
				? ElementType.Tree
				: ElementType.Client;

		/// <summary>Создать экзепляр узла дерева с ссылкой на сервер</summary>
		/// <param name="text">Ссылка на сервер для теста</param>
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

		/// <summary>Это папка</summary>
		public Boolean IsFolderNode => this.ImageIndex == TreeImageList.Folder;

		/// <summary>Клиент подключён к серверу</summary>
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