using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
namespace Tracking
{
    class DBO
    {
        private static string connection="uid='root';pwd='root';database='tracing';server='127.0.0.1'";
        
        public static DataSet execute(string sql)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
            DataSet result = new DataSet();
            adapter.Fill(result, "result");
            return result;
        }

        public static DataSet getRecord(string goods_id)
        {
            string sql = string.Format("select time,a.name,b.name,action from record,node as a,node as b where cargo_id = {0} and currentnode= a.id and nextnode=b.id order by send_id desc", goods_id);
            return DBO.execute(sql);
        }

        public static void newRecord(string goods_id, int currentnode, int nextnode, int action)
        {
            object[] args = { goods_id, currentnode, nextnode, action, goods_id };
            string sql = string.Format("insert into record (cargo_id,time,currentnode,nextnode,action,send_id) values ({0},now(),{1},{2},{3},(select count(*) from record as b where cargo_id={4}))", args);
            DBO.execute(sql);
        }

        public static DataSet getDate(int currentnode)
        {
            string sql = string.Format("select DATE_FORMAT(a.time,'%Y-%m-%d') from record as a where a.currentnode= {0} and not exisit (selete * from record as b where a.cargo_id = b.cargo_id and b.send_id>a.send_id)", currentnode);
            return DBO.execute(sql);
        }

        public static DataSet getRecord(string time, int action, int cuttentnode)
        {
            string sql = string.Format("select * from record as a where a.currentnode= {0} and a.action={1} and time like'{2}%' and not exisit (selete * from record as b where a.cargo_id = b.cargo_id and b.send_id>a.send_id)");
            return DBO.execute(sql);
        }

        public static string newGoods(Dictionary<string,object> args)
        {
            try
            {
                string sql = string.Format("insert into goods(id,sender,senderaddr,receiver,receiverphone,senderphone,time,price,receiveraddr,sendnode,receivenode) values ((select count(*) from goods as b),'{0}','{1}','{2}','{3}','{4}',now(),'{5}','{6}',{7},{8})",
                    new object[] { (string)args["sendername"],(string)args["senderaddr"] ,(string)args["receivername"],(string)args["receiverphone"],(string)args["senderphone"],(string)args["price"], (string)args["receiveraddr"],args["sendsite"],args["receivesite"]});
               execute(sql);
            }
            catch(Exception )
            {
                throw new Exception();
            }
            return ""+execute("select last_insert_id() from goods").Tables[0].Rows[0][0];
        }

        public static DataSet getNode()
        {
            string sql = "select id, name from node";
            return execute(sql);
        }
    }
}
