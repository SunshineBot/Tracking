using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
namespace Tracking
{
    class DBO
    {
        private static string connection="uid='root';pwd='root';database='tracing';server='127.0.0.1'";
        
        public static DataSet execute(string sql)
        {
            DataSet result = new DataSet();
            result.Tables.Add(new DataTable("result"));
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                adapter.Fill(result, "result");
            }
            catch (Exception ee)
            {
                MessageBoxResult messageResult = System.Windows.MessageBox.Show("数据库访问出现错误，可能的原因是账户名或者密码不对，或者不存在该数据库。\n错误代码如下：\n" + ee.ToString() +"\n\n点击\"确定\"继续运行，点击\"取消\"立即关闭程序。", 
                    "警告", 
                    System.Windows.MessageBoxButton.OKCancel, 
                    System.Windows.MessageBoxImage.Warning);
                if (messageResult == MessageBoxResult.Cancel)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            return result;
        }

        public static DataSet getRecord(string goods_id)
        {
            string sql = string.Format("select time,a.name,b.name,action from record,node as a,node as b where cargo_id = '{0}' and currentnode= a.id and nextnode=b.id order by send_id ", goods_id);
            return DBO.execute(sql);
        }

        public static void newRecord(string goods_id, int currentnode, int nextnode, int action)
        {
            object[] args = { goods_id, currentnode, nextnode, action, goods_id };
            string sql = string.Format("insert into record (cargo_id,time,currentnode,nextnode,action,send_id) values ('{0}',now(),{1},{2},{3},(select count(*) from record as b where cargo_id='{4}'))", args);
            DBO.execute(sql);
        }

        public static DataSet getDate(int currentnode, int action)
        {
            string sql = string.Format("select distinct DATE_FORMAT(a.time,'%Y-%m-%d') as date from record as a where a.action = {1} and a.currentnode= {0} and not exists (select * from record as b where a.cargo_id = b.cargo_id and b.send_id>a.send_id)", currentnode,action);
            return DBO.execute(sql);
        }

        public static DataSet getRecord(string time, int action, int cuttentnode)
        {
            string sql = string.Format("select * from record as a where a.currentnode= {0} and a.action={1} and DATE_FORMAT(time,'%Y-%m-%d') like'{2}%' and not exists (select * from record as b where a.cargo_id = b.cargo_id and b.send_id>a.send_id)",cuttentnode,action,time);
            return DBO.execute(sql);
        }

        public static string newGoods(Dictionary<string,object> args)
        {
            try
            {
                string sql = string.Format("insert into goods(id,sender,senderaddr,receiver,receiverphone,senderphone,time,price,receiveraddr,sendnode,receivenode,type) values (concat(lpad({7},2,'0'),lpad({8},2,'0'),lpad({10},2,'0'),lpad((select count(*) from goods as b where b.type = '{10}'),6,'0')),'{0}','{1}','{2}','{3}','{4}',now(),'{5}','{6}',{7},{8},{9})",
                    new object[] { (string)args["sendername"],(string)args["senderaddr"] ,(string)args["receivername"],(string)args["receiverphone"],(string)args["senderphone"],(string)args["price"], (string)args["receiveraddr"],args["sendsite"],args["receivesite"],args["type"],args["type"]});
               execute(sql);
               sql = "select id from goods order by time desc limit 1";
               return (string)execute(sql).Tables[0].Rows[0][0];
            }
            catch(Exception )
            {
                return null;
            }
        }

        public static DataSet getNode()
        {
            string sql = "select id, name from node";
            return execute(sql);
        }

    }
}
