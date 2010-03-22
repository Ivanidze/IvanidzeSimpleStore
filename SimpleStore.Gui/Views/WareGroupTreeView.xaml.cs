using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleStore.Gui.Views
{
    /// <summary>
    /// Interaction logic for WareGroupTreeView.xaml
    /// </summary>
    public partial class WareGroupTreeView : UserControl
    {
        public WareGroupTreeView()
        {
            InitializeComponent();
        }
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            treeViewItem.IsSelected = true;
        }

        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }
        void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
                
        }
    }
}
