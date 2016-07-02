using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WPF.JoshSmith.Controls.Utilities;

namespace Webplayer.Modules.Structure.Helper
{
    //http://www.wpftutorial.net/draganddrop.html
    class DragDropHelper
    {
        private Point startPoint;
        private ListView _ls;

        public DragDropHelper(ListView ls)
        {
            _ls = ls;
            ls.AllowDrop = true;

            ls.PreviewMouseLeftButtonDown += Ls_PreviewMouseRightButtonDown;
            ls.PreviewMouseMove += Ls_MouseMove;
            ls.Drop += Ls_Drop;
            ls.DragEnter += Ls_DragEnter;
        }

        /// <summary>
        /// Returns the index of the ListViewItem underneath the
        /// drag cursor, or -1 if the cursor is not over an item.
        /// </summary>
        int IndexUnderDragCursor
        {
            get
            {
                int index = -1;
                for (int i = 0; i < this._ls.Items.Count; ++i)
                {
                    ListViewItem item = this.GetListViewItem(i);
                    if (this.IsMouseOver(item))
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
        }


        bool IsMouseOver(Visual target)
        {
            // We need to use MouseUtilities to figure out the cursor
            // coordinates because, during a drag-drop operation, the WPF
            // mechanisms for getting the coordinates behave strangely.

            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePos = MouseUtilities.GetMousePosition(target);
            return bounds.Contains(mousePos);
        }

        ListViewItem GetListViewItem(int index)
        {
            if (this._ls.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return this._ls.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }

        private void Ls_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Ls_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                ISongModel contact = e.Data.GetData("myFormat") as ISongModel;
                ListView listView = sender as ListView;
                var source = ((IList<ISongModel>)listView.ItemsSource);
                
                var location = IndexUnderDragCursor;
                if (location == -1)
                    return;

                source.Remove(contact);
               source.Insert(location, contact);
            }
        }

        private void Ls_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            var lm = Mouse.LeftButton;
            if (lm == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null)
                    return;

                // Find the data behind the ListViewItem
                ISongModel contact = (ISongModel)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", contact);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private void Ls_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        // Helper to search up the VisualTree
        private static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}
