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
    
    public partial class window_company : Window
    {

        private ZsmTreeView[][] treeList; 
        private MainWindow mainWindow;
        public window_company(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            treeList = new ZsmTreeView[8][]{
                new ZsmTreeView[3]{checkTree_bj_situation, checkTree_bj_sending, checkTree_bj_arrived},
                new ZsmTreeView[3]{checkTree_sh_situation, checkTree_sh_sending, checkTree_sh_arrived},
                new ZsmTreeView[3]{checkTree_tj_situation, checkTree_tj_sending, checkTree_tj_arrived},
                new ZsmTreeView[3]{checkTree_gd_situation, checkTree_gd_sending, checkTree_gd_arrived},
                new ZsmTreeView[3]{checkTree_sd_situation, checkTree_sd_sending, checkTree_sd_arrived},
                new ZsmTreeView[3]{checkTree_sc_situation, checkTree_sc_sending, checkTree_sc_arrived},
                new ZsmTreeView[3]{checkTree_hb_situation, checkTree_hb_sending, checkTree_hb_arrived},
                new ZsmTreeView[3]{checkTree_hlj_situation, checkTree_hlj_sending, checkTree_hlj_arrived}
            };
            refreshTables();
        }

        private void window_unload(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
        }

        private void refreshTables()
        {
            //ZsmTreeView tree;
            IList<String> dateList;
            IList<TreeModel> content;
            for (int i = 0; i < RES.LOC_MAX; i++)
            {
                for (int j = 0; j < RES.TABLE_MAX; j++)
                {
                    dateList = getDateList(i, j);
                    content = new List<TreeModel>(dateList.Count());
                    foreach (String dataStr in dateList)
                    {
                        TreeModel tree = new TreeModel(dataStr);
                        tree.type = TreeModel.DATE;
                        IList<String> billList = getBillListByDate(i, j, dataStr);
                        foreach (String billStr in billList)
                        {
                            TreeModel t = new TreeModel(billStr);
                            t.type = TreeModel.BILL;
                            tree.Children.Add(t);
                        }
                        content.Add(tree);
                    }
                    treeList[i][j].ItemsSourceData = content;
                }
            }
        }

        private IList<String> getDateList(int Location, int table)
        {
            IList<String> list = new List<String>();
            // todo : fill this method.
            list.Add("2016-05-18");
            list.Add("2016-05-17");
            list.Add("2016-05-06");
            list.Add("2016-04-18");
            return list;
        }

        private IList<String> getBillListByDate(int Location, int table, String date)
        {
            IList<String> list = new List<String>();
            // todo : fill this method.
            list.Add("010302112313");
            list.Add("010302112543");
            list.Add("010302112344");
            list.Add("010302112322");
            return list;
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
