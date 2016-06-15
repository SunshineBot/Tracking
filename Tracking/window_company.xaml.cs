using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tracking.Model;
namespace Tracking
{
    /// <summary>
    /// window_company.xaml 的交互逻辑
    /// </summary>
    
    public partial class window_company : Window, zsmTreeViewCallback
    {

        private ZsmTreeView[][] treeList; 
        private MainWindow mainWindow;
        private RunTimer runTimer;

        private List<TreeModel> sitList;
        private List<TreeModel> sitCheckedList;
        private List<TreeModel> sendingList;
        private List<TreeModel> arrivedList;
        private List<TreeModel> arrivedCheckedList;
        private List<TreeModel> settledList;

        public window_company(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            treeList = new ZsmTreeView[8][]{
                new ZsmTreeView[3]{
                    checkTree_bj_situation.setCallback(this), 
                    checkTree_bj_sending.setCallback(this), 
                    checkTree_bj_arrived.setCallback(this)
                },
                new ZsmTreeView[3]{
                    checkTree_sh_situation.setCallback(this), 
                    checkTree_sh_sending.setCallback(this), 
                    checkTree_sh_arrived.setCallback(this)
                },
                new ZsmTreeView[3]{
                    checkTree_tj_situation.setCallback(this), 
                    checkTree_tj_sending.setCallback(this), 
                    checkTree_tj_arrived.setCallback(this), 
                },
                new ZsmTreeView[3]{
                    checkTree_gd_situation.setCallback(this), 
                    checkTree_gd_sending.setCallback(this), 
                    checkTree_gd_arrived.setCallback(this), 
                },
                new ZsmTreeView[3]{
                    checkTree_sd_situation.setCallback(this), 
                    checkTree_sd_sending.setCallback(this), 
                    checkTree_sd_arrived.setCallback(this)
                },
                new ZsmTreeView[3]{
                    checkTree_sc_situation.setCallback(this), 
                    checkTree_sc_sending.setCallback(this), 
                    checkTree_sc_arrived.setCallback(this)
                },
                new ZsmTreeView[3]{
                    checkTree_hb_situation.setCallback(this), 
                    checkTree_hb_sending.setCallback(this), 
                    checkTree_hb_arrived.setCallback(this)
                },
                new ZsmTreeView[3]{
                    checkTree_hlj_situation.setCallback(this), 
                    checkTree_hlj_sending.setCallback(this), 
                    checkTree_hlj_arrived.setCallback(this)
                }
            };
            sitList = new List<TreeModel>();
            sitCheckedList = new List<TreeModel>();
            sendingList = new List<TreeModel>();
            arrivedList = new List<TreeModel>();
            arrivedCheckedList = new List<TreeModel>();
            settledList = new List<TreeModel>();
            refreshTables();
            runTimer = new RunTimer(treeList);
            runTimer.setSynchronizationContext(SynchronizationContext.Current);
            runTimer.context = this;
            runTimer.runningList = sendingList;
        }

        private void window_unload(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
        }

        private void refreshTables()
        {
            //ZsmTreeView tree;
            List<List<TreeModel>> tList = new List<List<TreeModel>>();
            tList.Add(sitList);
            tList.Add(sendingList);
            tList.Add(arrivedList);
            tList.Add(settledList);
            for (int i = 0; i < RES.LOC_MAX; i++)
            {

                IList<String> dateList;
                List<TreeModel> content = null;
                for (int j = 0; j < RES.TABLE_MAX; j++)
                {
                    dateList = getDateList(i, j);
                    content = new List<TreeModel>(dateList.Count() + 1);
                    if (j == 2)
                    {
                        TreeModel settledItem = new TreeModel("已结算");
                        settledItem.type = TreeModel.DATE;
                        List<String> settledDateList = getDateList(i, j + 1);
                        foreach (String dateStr in settledDateList)
                        {
                            IList<String> billList = getBillListByDate(i, j + 1, dateStr);
                            foreach (String billStr in billList)
                            {
                                TreeModel t = new TreeModel(billStr);
                                t.type = TreeModel.BILL;
                                t.src = Int32.Parse(billStr.Substring(0, 2)); ;
                                //t.current = i;
                                t.dst = Int32.Parse(billStr.Substring(2, 2));
                                t.Parent = settledItem;
                                settledItem.Children.Add(t);
                                tList.ElementAt<List<TreeModel>>(j + 1).Add(t);
                            }
                        }
                        content.Add(settledItem);
                    }
                    foreach (String dateStr in dateList)
                    {
                        TreeModel tree = new TreeModel(dateStr);
                        tree.type = TreeModel.DATE;
                        IList<String> billList = getBillListByDate(i, j, dateStr);
                        foreach (String billStr in billList)
                        {
                            TreeModel t = new TreeModel(billStr);
                            t.type = TreeModel.BILL;
                            t.src = Int32.Parse(billStr.Substring(0, 2));
                            //t.src = 0;
                            //t.current = i;
                            t.dst = Int32.Parse(billStr.Substring(2, 2));
                            //t.dst = 3;
                            t.Parent = tree;
                            tree.Children.Add(t);
                            tList.ElementAt<List<TreeModel>>(j).Add(t);
                        }
                        content.Add(tree);
                    }
                    treeList[i][j].ItemsSourceData = content;
                    
                }
            }
        }

        private List<String> getDateList(int Location, int action)
        {
            List<String> list = new List<String>();
            DataSet dataSet = DBO.getDate(Location, action);
            foreach (DataRow row in dataSet.Tables["result"].Rows)
            {
                list.Add(row["date"].ToString());
            }
            return list;
        }

        private List<String> getBillListByDate(int Location, int action, String date)
        {
            List<String> list = new List<String>();
            DataSet dataSet = DBO.getRecord(date, action, Location);
            foreach (DataRow row in dataSet.Tables["result"].Rows)
            {
                list.Add(row["cargo_id"].ToString());
            }
            return list;
        }

        public Boolean checkIt(TreeModel data)
        {
            try
            {
                if (sitList.Contains(data))
                {
                    sitList.Remove(data);
                    sitCheckedList.Add(data);
                }
                if (arrivedList.Contains(data))
                {
                    arrivedList.Remove(data);
                    arrivedCheckedList.Add(data);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean uncheckIt(TreeModel data)
        {
            try
            {
                if (sitCheckedList.Contains(data))
                {
                    sitCheckedList.Remove(data);
                    sitList.Add(data);
                }
                if (arrivedCheckedList.Contains(data))
                {
                    arrivedCheckedList.Remove(data);
                    arrivedList.Add(data);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void label_send_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //label_send.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\send-click.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.send_click.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_send.Background = new ImageBrush(source);
        }

        private void label_send_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // todo : 暂停timer
            runTimer.Enabled = false;
            //label_send.Background = new ImageBrush(new BitmapImage(
                //new Uri("res\\button\\send-simple.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.send_simple.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_send.Background = new ImageBrush(source);

            Distance[] Nodes = new Distance[RES.LOC_MAX];
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i] = new Distance(i);
            }

            foreach (TreeModel t in sitCheckedList)
            {
                //写入数据库
                //DBO.newRecord(t.Name, t.src, t.src, 2);     //此处存疑，该表项表示发货但是仍未到达第一站
                //不需要写入数据库，实际发送操作有timer完成，这里只修改状态。
                //修改逻辑表，从勾选状态进入发送状态
                sendingList.Add(t);
            }
            //修改界面表，将之从仓库表移至运送中表
            //移除同样属于发送操作，交由timer完成。
            //修改UI表结构
            List<TreeModel> bills = new List<TreeModel>();
            foreach (ZsmTreeView[] trees in treeList)
            {
                ZsmTreeView tree = trees[0];
                tree.tvZsmTree.BeginInit();
                for (int i = 0; i < tree.ItemsSourceData.Count(); i++)
                {
                    IList<TreeModel> childrenList = tree.ItemsSourceData.ElementAt<TreeModel>(i).Children;
                    foreach (TreeModel t in sitCheckedList)
                    {
                        if (childrenList.Contains(t))
                        {
                            childrenList.Remove(t);
                            int dateIndex = trees[1].ItemsSourceData.Count();
                            while (dateIndex-- > 0)
                            {
                                TreeModel date2 = trees[1].ItemsSourceData.ElementAt<TreeModel>(dateIndex);
                                if (t.Parent.Name.Equals(date2.Name))
                                    break;
                            }
                            if (dateIndex == -1)
                            {
                                TreeModel newDate = new TreeModel(t.Parent.Name);
                                newDate.type = TreeModel.DATE;
                                newDate.Children = new List<TreeModel>();
                                newDate.Children.Add(t);
                                trees[1].ItemsSourceData.Add(newDate);
                            }
                            else
                            {
                                trees[1].ItemsSourceData.ElementAt<TreeModel>(dateIndex).Children.Add(t);
                            }
                        }
                    }
                }
                tree.tvZsmTree.EndInit();
            }
            //取消所有勾选
            foreach (ZsmTreeView[] trees in treeList)
            {
                trees[0].menuUnSelectAll_Click(null, null);
            }
            //从逻辑表中移除他们
            sitCheckedList.RemoveRange(0, arrivedCheckedList.Count());
            // todo : 继续timer
            runTimer.Enabled = true;
        }

        private void label_settle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //label_settle.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\settle-click.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.settle_click.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_settle.Background = new ImageBrush(source);
        }

        private void label_settle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //label_settle.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\settle-simple.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.settle_simple.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_settle.Background = new ImageBrush(source);

            //修改逻辑表项
            foreach (TreeModel t in arrivedCheckedList)
            {
                //写入数据库
                DBO.newRecord(t.Name, t.dst, t.dst, RES.SETTLED);
                //修改逻辑表，从勾选状态进入已结算状态
                //arrivedCheckedList.Remove(t);
                settledList.Add(t);
            }
            //修改UI表结构
            List<TreeModel> bills = new List<TreeModel>();
            foreach (ZsmTreeView[] trees in treeList)
            {
                ZsmTreeView tree = trees[2];
                for (int i = 1; i < tree.ItemsSourceData.Count(); i++)
                {
                    IList<TreeModel> childrenList = tree.ItemsSourceData.ElementAt<TreeModel>(i).Children;
                    foreach(TreeModel t in arrivedCheckedList){
                        if (childrenList.Contains(t))
                        {
                            childrenList.Remove(t);
                            tree.ItemsSourceData.ElementAt<TreeModel>(0).Children.Add(t);
                        }
                    }
                }
                tree.tvZsmTree.BeginInit();
                tree.tvZsmTree.EndInit();
            }
            //取消所有勾选
            //修改UI表项
            foreach (ZsmTreeView[] trees in treeList)
            {
                trees[2].menuUnSelectAll_Click(null, null);
            }
            arrivedCheckedList.RemoveRange(0, arrivedCheckedList.Count());
        }

        public void arrive(TreeModel data)
        {

        }

        private void label_back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //label_back.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\back-click.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.back_click.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_back.Background = new ImageBrush(source);
        }

        private void label_back_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //label_back.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\back-simple.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.back_simple.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_back.Background = new ImageBrush(source);
            this.Close();
        }

        public void startUpdateUI(object o)
        {
            ((ZsmTreeView)o).tvZsmTree.BeginInit();
        }

        public void endUpdateUI(object o)
        {
            ((ZsmTreeView)o).tvZsmTree.EndInit();
        }

        public void moveToArrivedCheckedList(TreeModel t)
        {
            arrivedList.Remove(t);
            arrivedCheckedList.Add(t);
        }

        private void label_send_MouseEnter(object sender, MouseEventArgs e)
        {
            IntPtr bitmap = global::Tracking.Properties.Resources.send_hover.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_send.Background = new ImageBrush(source);
        }


        private void label_send_MouseLeave(object sender, MouseEventArgs e)
        {
            IntPtr bitmap = global::Tracking.Properties.Resources.send_simple.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_send.Background = new ImageBrush(source);
        }

        private void label_settle_MouseEnter(object sender, MouseEventArgs e)
        {
            IntPtr bitmap = global::Tracking.Properties.Resources.settle_hover.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_settle.Background = new ImageBrush(source);
        }

        private void label_settle_MouseLeave(object sender, MouseEventArgs e)
        {
            IntPtr bitmap = global::Tracking.Properties.Resources.settle_simple.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_settle.Background = new ImageBrush(source);
        }

        private void label_back_MouseEnter(object sender, MouseEventArgs e)
        {
            IntPtr bitmap = global::Tracking.Properties.Resources.back_hover.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_back.Background = new ImageBrush(source);
        }

        private void label_back_MouseLeave(object sender, MouseEventArgs e)
        {
            IntPtr bitmap = global::Tracking.Properties.Resources.back_simple.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            label_back.Background = new ImageBrush(source);
        }

    }
}
