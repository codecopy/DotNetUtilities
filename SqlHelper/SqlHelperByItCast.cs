using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DotNet.Utilities.SqlHelper
{
    /// <summary>
    /// 根据传智播客写的sqlhelper
    /// </summary>
    public static class SqlHelperByItCast
    {
        //连接字符串
        private static readonly string Strcon = ConfigurationManager.ConnectionStrings["mssqlserver"].ConnectionString;

        // 配置文件
        //<configuration>
        //  <connectionStrings>
        //    <add connectionString = "Data Source=DESKTOP-B0QVA84;Catalog=MyfirstOne;Integrated Security=True" name="mssqlserver" />
        //  </connectionStrings>
        //</configuration>

        /// <summary>
        /// 增删改返回单条无内容ExcuteNonQuery方法
        /// </summary>
        /// <param name="sql">Sql执行语句</param>
        /// <param name="pms">传入变量</param>
        /// <returns></returns>
        public static int ExcuteNonQuery(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(Strcon))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 查询返回单条数据内容ExcuteScalar方法
        /// </summary>
        /// <param name="sql">Sql执行语句</param>
        /// <param name="pms">传入参数</param>
        /// <returns></returns>
		public static object ExcuteScalar(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(Strcon))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 查询Reader方法
        /// </summary>
        /// <param name="sql">Sql执行语句</param>
        /// <param name="pms">传入参数</param>
        /// <returns></returns>
        public static SqlDataReader Reader(string sql, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(Strcon);
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                try
                {
                    con.Open();
                    return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                catch (Exception)
                {
                    con.Close();
                    con.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// DataTable方法
        /// </summary>
        /// <param name="sql">Sql执行语句</param>
        /// <param name="pms">可变Sql参数</param>
        /// <returns></returns>
        public static DataTable Adapter(string sql, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, Strcon))
            {
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
