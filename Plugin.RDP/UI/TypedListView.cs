using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Plugin.RDP.UI
{
	internal class TypedListView : ListView
	{
		public void CreateColumns(IList items)
		{
			base.Items.Clear();
			base.Columns.Clear();

			if(items?.Count > 0)
			{
				Type objectType = items[0].GetType();
				foreach(PropertyInfo property in objectType.GetProperties())
					base.Columns.Add(property.Name);
			}
		}

		public void FillList(IList items, Int32 imageIndex)
		{
			List<ListViewItem> items2Add = new List<ListViewItem>();
			String[] subItems = Array.ConvertAll(new String[base.Columns.Count], a => String.Empty);
			foreach(Object row in items)
			{
				ListViewItem item = new ListViewItem() { Tag = row, ImageIndex = imageIndex, StateImageIndex = imageIndex, };
				item.SubItems.AddRange(subItems);
				foreach(ColumnHeader column in base.Columns)
				{
					PropertyInfo property = row.GetType().GetProperty(column.Text);
					Object val = property.GetValue(row, null);

					item.SubItems[column.Index].Text = val == null ? String.Empty : val.ToString();
				}
				items2Add.Add(item);
			}
			if(base.InvokeRequired)
				base.Invoke((System.Windows.Forms.MethodInvoker)delegate { base.Items.AddRange(items2Add.ToArray()); });
			else
				base.Items.AddRange(items2Add.ToArray());
		}
	}
}