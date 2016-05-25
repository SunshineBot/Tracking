using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                        //List<String> settledDateList = getDateList(i, j + 1);
                        foreach (String dateStr in getDateList(i, j + 1))
                        {
                            //IList<String> billList = getBillListByDate(i, j + 1, dateStr);
                            foreach (String billStr in getBillListByDate(i, j + 1, dateStr))
                            {
                                TreeModel t = new TreeModel(billStr);
                                t.type = TreeModel.BILL;
                                t.src = Int32.Parse(billStr.Substring(0, 2)); ;
                                //t.current = i;
                                t.dst = Int32.Parse(billStr.Substring(2, 2));
                                settledItem.Children.Add(t);
                                tList.ElementAt<List<TreeModel>>(j).Add(t);
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
                            t.src = Int32.Parse(billStr.Substring(0, 2)); ;
                            //t.current = i;
                            t.dst = Int32.Parse(billStr.Substring(2, 2));
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
            // todo : fill this method.
            list.Add("2016-05-18");
            //list.Add("2016-05-17");
            //list.Add("2016-05-06");
            //list.Add("2016-04-18");
            return list;
        }

        private List<String> getBillListByDate(int Location, int table, String date)
        {
            List<String> list = new List<String>();
            // todo : fill this method.
            list.Add(String.Format("{0:D2}0202112313", Location));
            list.Add(String.Format("{0:D2}0202112543", Location));
            list.Add(String.Format("{0:D2}0202112344", Location));
            list.Add(String.Format("{0:D2}0302112322", Location));
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
            label_send.Background = new ImageBrush(new BitmapImage(
                new Uri("res\\button\\send-click.png", System.UriKind.Relative)));
        }

        private void label_send_MouseUp(object sender, MouseButtonEventArgs e)
        {
            label_send.Background = new ImageBrush(new BitmapImage(
                new Uri("res\\button\\send-hover.png", System.UriKind.Relative)));
            Distance[] Nodes = new Distance[RES.LOC_MAX];
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i] = new Distance(i);
            }

            foreach (TreeModel t in sitCheckedList)
            {
                //写入数据库
                DBO.newRecord(t.Name, t.src, t.src, 2);     //此处存疑，该表项表示发货但是仍未到达第一站
                //不需要写入数据库，实际发送操作有timer完成，这里只修改状态。
                //修改逻辑表，从勾选状态进入发送状态
                // todo : 暂停timer
                runTimer.Enabled = false;
                sendingList.Add(t);
                // todo : 继续timer
                runTimer.Enabled = true;
            }
            
            
            //修改界面表，将之从仓库表移至运送中表
            //移除同样属于发送操作，交由timer完成。

            //取消所有勾选
            foreach (ZsmTreeView[] trees in treeList)
            {
                trees[0].menuUnSelectAll_Click(null, null);
            }
            //从逻辑表中移除他们
            sitCheckedList.RemoveRange(0, arrivedCheckedList.Count());
        }

        private void label_settle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            label_settle.Background = new ImageBrush(new BitmapImage(
                new Uri("res\\button\\settle-click.png", System.UriKind.Relative)));
        }

        private void label_settle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            label_settle.Background = new ImageBrush(new BitmapImage(
                new Uri("res\\button\\settle-hover.png", System.UriKind.Relative)));
            
            //修改逻辑表项
            foreach (TreeModel t in arrivedCheckedList)
            {
                //写入数据库
                //DBO.newRecord(t.Name, t.current, t.current, 4);
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
            label_back.Background = new ImageBrush(new BitmapImage(
                new Uri("res\\button\\back-click.png", System.UriKind.Relative)));
        }

        private void label_back_MouseUp(object sender, MouseButtonEventArgs e)
        {
            label_back.Background = new ImageBrush(new BitmapImage(
                new Uri("res\\button\\back-hover.png", System.UriKind.Relative)));
            this.Close();
        }

        
    }
}
