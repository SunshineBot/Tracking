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
        private static string connection="uid='root';pwd='12345';database='tracing';server='127.0.0.1'";
        
        public static DataSet execute(string sql)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
            DataSet result = new DataSet();
            adapter.Fill(result, "result");
            return result;
        }

        public static DataSet getRecord(int goods_id)
        {
            string sql = string.Format("select * from record where cargo_id = {0} order by send_id desc", goods_id);
            return DBO.execute(sql);
        }

        public static void newRecord(int goods_id, int currentnode, int nextnode, int action)
        {
            string time = DateTime.Now.ToString();
            object[] args = { goods_id, time, currentnode, nextnode, action, goods_id };
            string sql = string.Format("insert into record (cargo_id,time,currentnode,nextnode,action,send_id) values ({0},'{1}',{2},{3},{4}，(select count(*) from record where cargo_id={5}))", args);
            DBO.execute(sql);
        }

        public static DataSet getDate(int currentnode)
        {
            string sql = string.Format("select a.time from record as a where a.currentnode= {0} and not exisit (selete * from record as b where a.cargo_id = b.cargo_id and b.send_id>a.send_id)", currentnode);
            return DBO.execute(sql);
        }

        public static DataSet getRecord(string time, int action, int cuttentnode)
        {
            string sql = string.Format("select * from record as a where a.currentnode= {0} and a.action={1} and time='{2}' and not exisit (selete * from record as b where a.cargo_id = b.cargo_id and b.send_id>a.send_id)");
            return DBO.execute(sql);
        }
    }
}
