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
    /// Interaction logic for logistics_info.xaml
    /// </summary>
    public partial class logistics_info : Window
    {
        public logistics_info(string id)
        {
            InitializeComponent();

            try
            {
                goodsid.Text = id;
                DataSet ds = DBO.getRecord(id);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string info = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        switch ((int)dr["action"])
                        {
                            case 0:
                                info += dr[0].ToString() + " 当前节点：" + (string)dr[1] + ", 包裹已入库\n";
                                break;
                            case 1:
                                info += dr[0].ToString() + " 包裹从" + (string)dr[1] + "发出，发往下一站: " + (string)dr[2] + "\n";
                                break;
                            case 2:
                                info += dr[0].ToString() + " 包裹已在" + (string)dr[1] + "签收\n";
                                break;
                            case 3:
                                info += dr[0].ToString() + "包裹已完成结算";
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
