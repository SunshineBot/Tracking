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
using System.Data;
namespace Tracking
{
    /// <summary>
    /// Interaction logic for Window_person.xaml
    /// </summary>
    public partial class Window_person : Window
    {
        public Window_person()
        {
            InitializeComponent();
            initSite();
            initType();
        }

        public void initSite()
        {
            DataSet ds = DBO.getNode();
            sendsite.DisplayMemberPath="name";
            sendsite.ItemsSource=ds.Tables[0].AsDataView();
            receivesite.DisplayMemberPath = "name";
            receivesite.ItemsSource = ds.Tables[0].AsDataView();
            sendsite.SelectedIndex = 0;
            receivesite.SelectedIndex = 1;
        }

        public void initType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name", Type.GetType("System.String"));
            dt.Columns.Add("id", Type.GetType("System.String"));
            DataRow dr = dt.NewRow();
            dr["name"] = "食品";
            dr["id"] = "1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "衣服";
            dr["id"] = "2";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["name"] = "日用品";
            dr["id"] = "3";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "电子产品";
            dr["id"] = "4";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "书籍";
            dr["id"] = "5";
            dt.Rows.Add(dr);

            type.DisplayMemberPath = "name";
            type.ItemsSource = dt.AsDataView();
            type.SelectedIndex = 0;
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sendername", sendername.Text);
            dic.Add("senderaddr", senderaddr.Text);
            dic.Add("senderphone", senderphone.Text);
            dic.Add("receivername", receivername.Text);
            dic.Add("receiveraddr", receiveraddr.Text);
            dic.Add("receiverphone", receiverphone.Text);
            dic.Add("sendsite", sendsite.SelectedIndex);
            dic.Add("receivesite", receivesite.SelectedIndex);
            dic.Add("price", "100");
            dic.Add("type", type.SelectedIndex);
            string id=DBO.newGoods(dic);
            DBO.newRecord(id, (int)dic["sendsite"],(int)dic["receivesite"], 0);
            MessageBox.Show("请记住运单编号："+id, "发货成功");
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataSet ds = DBO.getRecord(goodsid.Text);
                DataTable dt = ds.Tables[0];
                if(dt.Rows.Count>0)
                {
                    string info = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        switch ((int)dr["action"])
                        {
                            case 0:
                                info += dr[0].ToString() + " 当前节点：" + (string)dr[1] + ", 您的包裹已入库\n";
                                break;
                            case 1:
                                info += dr[0].ToString() + " 您的包裹从" + (string)dr[1] + "发出，发往下一站: " + (string)dr[2] + "\n";
                                break;
                            case 2:
                                info += dr[0].ToString() + " 您的包裹已在" + (string)dr[1] + "签收\n";
                                break;
                            case 3:
                                info += dr[0].ToString() + "您的包裹已完成结算";
                                break;
                        }
                    }
                    info_textblock.Text = info;
                }
                else
                {
                    MessageBox.Show("暂时没有该包裹的记录，请核对物流编号", "系统提示");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }
}
