using CalculateVessels.Core.Interfaces;
using CalculateVessels.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Elements
{
    public partial class CalculatedElementsControl : UserControl
    {
        public CalculatedElementsControl()
        {
            InitializeComponent();
        }

        public void AddElement(ICalculatedElement element)
        {
            var item = new ListViewItem(element.ToString())
            {
                Tag = element
            };

            elementsListView.Items.Add(item);
        }

        public void LoadElements(IEnumerable<ICalculatedElement> elements)
        {
            elementsListView.Items.Clear();

            foreach (var element in elements)
            {
                AddElement(element);
            }
        }

        public void UpdateElement(ICalculatedElement element, int index)
        {
            var item = new ListViewItem(element.ToString())
            {
                Tag = element
            };

            elementsListView.Items[index] = item;
        }

        public void RemoveElement()
        {

        }

        public IEnumerable<ICalculatedElement> GetElements()
        {
            var elements = elementsListView.Items
            .Cast<ListViewItem>()
            .Select(i => (ICalculatedElement)i.Tag)
            .ToArray();

            return elements;
        }

        private void ElementsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (sender is not ListView lv) return;

            if (e.IsSelected)
            {
                deleteButton.Enabled = true;
                editButton.Enabled = true;
                if (e.ItemIndex < lv.Items.Count - 1)
                {
                    downButton.Enabled = true;
                }
                if (e.ItemIndex > 0)
                {
                    upButton.Enabled = true;
                }
            }
            else
            {
                editButton.Enabled = false;
                deleteButton.Enabled = false;
                upButton.Enabled = false;
                downButton.Enabled = false;
            }
        }

        private static void MoveSelectedItemListView(ListView lv, int idx, bool moveUp)
        {
            if (lv.Items.Count <= 1) return;

            int offset = 0;
            if (idx >= 0)
            {
                offset = moveUp ? -1 : 1;
            }

            if (offset == 0) return;

            lv.BeginUpdate();

            var selectItem = idx + offset;
            for (var i = 0; i < lv.Items[idx].SubItems.Count; i++)
            {
                (lv.Items[selectItem].SubItems[i].Text, lv.Items[idx].SubItems[i].Text) =
                    (lv.Items[idx].SubItems[i].Text, lv.Items[selectItem].SubItems[i].Text);
            }

            (lv.Items[selectItem].Tag, lv.Items[idx].Tag) =
                (lv.Items[idx].Tag, lv.Items[selectItem].Tag);

            lv.Focus();
            lv.Items[selectItem].Selected = true;
            lv.EnsureVisible(selectItem);

            lv.EndUpdate();
        }

        private void MoveItem(bool moveUp)
        {
            if (elementsListView.SelectedItems.Count == 0)
            {
                return;
            }

            if ((Parent is not MainForm mf) || mf.IsAnyWindowOpen())
            {
                MessageBox.Show("Закройте все окна с расчетами для изменения порядка рассчитанных элементов.");
                return;
            }

            var idx = elementsListView.SelectedItems[0].Index;

            MoveSelectedItemListView(elementsListView, idx, moveUp);
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            MoveItem(true);
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            MoveItem(false);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var idx = elementsListView.SelectedItems[0].Index;

            elementsListView.Items.RemoveAt(idx);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (elementsListView.SelectedItems.Count == 0)
            {
                return;
            }

            var idx = elementsListView.SelectedItems[0].Index;

            if (Parent is not MainForm mf) return;

            if (elementsListView.SelectedItems[0].Tag is not ICalculatedElement element) return;

            mf.CheckAndCreateFormForEditElement(element, idx);
        }
    }
}
