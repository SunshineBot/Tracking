//***************************************************
//
// 文件名（FileName）  ： ZsmTreeView.xaml.cs
//
// 作者（Author）      ： zsm
//
// 创建时间（CreateAt）:  2013-03-15 16:52:40
//
// 描述（Description） ： 带CheckBox的TreeView控件的交互逻辑代码
//
//***************************************************
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
using Tracking.Model;

namespace Tracking
{

    public interface zsmTreeViewCallback
    {
        Boolean checkIt(TreeModel data);
        Boolean uncheckIt(TreeModel data);
    }

    /// <summary>
    /// ZsmTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class ZsmTreeView : UserControl
    {
        #region 私有变量属性
        private zsmTreeViewCallback callback;

        /// <summary>
        /// 控件数据
        /// </summary>
        private IList<Model.TreeModel> _itemsSourceData;
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public ZsmTreeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 控件数据
        /// </summary>
        public IList<Model.TreeModel> ItemsSourceData
        {
            get { return _itemsSourceData; }
            set
            {
                _itemsSourceData = value;
                tvZsmTree.ItemsSource = _itemsSourceData;
            }
        }

        /// <summary>
        /// 设置对应Id的项为选中状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SetCheckedById(string id, IList<Model.TreeModel> treeList)
        {
            foreach (var tree in treeList)
            {
                if (tree.Id.Equals(id))
                {
                    tree.IsChecked = true;
                    return 1;
                }
                if (SetCheckedById(id, tree.Children) == 1)
                {
                    return 1;
                }
            }
            return 0;
        }
        /// <summary>
        /// 设置对应Id的项为选中状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SetCheckedById(string id)
        {
            foreach (var tree in ItemsSourceData)
            {
                if (tree.Id.Equals(id))
                {
                    tree.IsChecked = true;
                    return 1;
                }
                if (SetCheckedById(id, tree.Children) == 1)
                {
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <returns></returns>
        public IList<Model.TreeModel> CheckedItemsIgnoreRelation()
        {

            return GetCheckedItemsIgnoreRelation(_itemsSourceData);
        }

        /// <summary>
        /// 私有方法，忽略层次关系的情况下，获取选中项
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IList<Model.TreeModel> GetCheckedItemsIgnoreRelation(IList<Model.TreeModel> list)
        {
            IList<Model.TreeModel> treeList = new List<Model.TreeModel>();
            foreach (var tree in list)
            {
                if (tree.IsChecked)
                {
                    treeList.Add(tree);
                }
                foreach (var child in GetCheckedItemsIgnoreRelation(tree.Children))
                {
                    treeList.Add(child);
                }
            }
            return treeList;
        }

        /// <summary>
        /// 选中所有子项菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelectAllChild_Click(object sender, RoutedEventArgs e)
        {
            if (tvZsmTree.SelectedItem != null)
            {
                Model.TreeModel tree = (Model.TreeModel)tvZsmTree.SelectedItem;
                tree.IsChecked = true;
                tree.SetChildrenChecked(true);
            }
        }

        /// <summary>
        /// 全部展开菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExpandAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Model.TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsExpanded = true;
                tree.SetChildrenExpanded(true);
            }
        }

        /// <summary>
        /// 全部折叠菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUnExpandAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Model.TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsExpanded = false;
                tree.SetChildrenExpanded(false);
            }
        }

        /// <summary>
        /// 全部选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Model.TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsChecked = true;
                tree.SetChildrenChecked(true);
            }
        }

        /// <summary>
        /// 全部取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuUnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Model.TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsChecked = false;
                tree.SetChildrenChecked(false);
            }
        }

        /// <summary>
        /// 鼠标右键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }
        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }

        private void treeItem_checked(object sender, RoutedEventArgs e)
        {
            //int i = 0;
            //CheckBox checkBox = (CheckBox)sender;
            //StackPanel panel = (StackPanel)checkBox.Content;
            TreeModel data = (TreeModel)((StackPanel)((CheckBox)sender).Content).DataContext;
            callback.checkIt(data);
        }

        private void treeItem_unchecked(object sender, RoutedEventArgs e)
        {
            TreeModel data = (TreeModel)((StackPanel)((CheckBox)sender).Content).DataContext;
            callback.uncheckIt(data);
        }

        public ZsmTreeView setCallback(zsmTreeViewCallback callback)
        {
            this.callback = callback;
            return this;
        }

        private void queryIt_Click(object sender, RoutedEventArgs e)
        {
            TreeModel t = (TreeModel)tvZsmTree.SelectedItem;
            if (t.type == TreeModel.BILL)
            {
                //todo : call it by t.name
                new logistics_info(t.Name).Show();
            }
            else
            {
                MessageBox.Show("请选择一个有效的订单！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}