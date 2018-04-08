using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

public class DBHelper
{
    SqlConnection conn;

    static string str = ConfigurationManager.AppSettings["ConnectionString"].ToString();

    public static bool IsOpen()
    {

        bool IsOpen = false;
        SqlConnection _SqlConnection = new SqlConnection(str);
        try
        {
            _SqlConnection.Open();
            IsOpen = true;
        }
        catch (Exception)
        {

            IsOpen = false;

        }
        finally
        {
            _SqlConnection.Close();
        }

        return IsOpen;
    }
    /// <summary>
    /// 执行sql语句，返回受影响的行数
    /// </summary>
    /// <param name="sql">sql语句或存储过程</param>
    /// <returns></returns>
    public int GetExecuteNonQuery(string sql)
    {
        conn = new SqlConnection(str);
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        int count = cmd.ExecuteNonQuery();
        conn.Close();
        return count;
    }

    /// <summary>
    /// 执行sql语句，返回受影响的行数
    /// </summary>
    /// <param name="sql">sql语句或存储过程</param>
    /// <param name="arr">所带的参数</param>
    /// <returns></returns>
    public int GetExecuteNonQuery(string sql, SqlParameter[] arr)
    {
        conn = new SqlConnection(str);
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddRange(arr);
        int count = cmd.ExecuteNonQuery();
        conn.Close();
        return count;
    }

    /// <summary>
    /// 执行sql语句，返回结果集中的第一行第一列
    /// </summary>
    /// <param name="sql">sql语句或存储过程</param>
    /// <returns></returns>
    public int GetExecuteScalar(string sql)
    {
        conn = new SqlConnection(str);
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        conn.Close();
        return count;

    }

    /// <summary>
    /// 执行sql语句，返回结果集中的第一行第一列
    /// </summary>
    /// <param name="sql">sql语句或存储过程</param>
    /// <param name="arr">所带的参数</param>
    /// <returns></returns>
    public int GetExecuteScalar(string sql, SqlParameter[] arr)
    {
        conn = new SqlConnection(str);
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddRange(arr);
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        conn.Close();
        return count;

    }

    /// <summary>
    /// 执行sql语句，返回一个DataReader的对象
    /// </summary>
    /// <param name="safeSql">sql语句或存储过程</param>
    /// <returns></returns>
    public SqlDataReader GetReader(string safeSql)
    {
        conn = new SqlConnection(str);
        conn.Open();
        SqlCommand cmd = new SqlCommand(safeSql, conn);
        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        conn.Close();
        return reader;

    }

    /// <summary>
    /// 执行sql语句，返回一个DataReader的对象
    /// </summary>
    /// <param name="Sql">sql语句或存储过程</param>
    /// <param name="values">所带的参数</param>
    /// <returns></returns>
    public SqlDataReader GetReader(string Sql, params SqlParameter[] values)
    {
        conn = new SqlConnection(str);
        conn.Open();
        SqlCommand cmd = new SqlCommand(Sql, conn);
        cmd.Parameters.AddRange(values);
        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        return reader;

    }

    /// <summary>
    /// 执行sql语句，返回一个表的集合
    /// </summary>
    /// <param name="safeSql">sql语句或存储过程</param>
    /// <returns></returns>
    public DataTable GetDataSet(string safeSql)
    {
        conn = new SqlConnection(str);
        conn.Open();
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand(safeSql, conn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        conn.Close();
        return ds.Tables[0];

    }
    public string ReturnStringScalar(string sql)
    {
        conn = new SqlConnection(str);
        SqlCommand cmd = new SqlCommand(sql, conn);
        conn.Open();
        try
        {
            string result = cmd.ExecuteScalar().ToString();
            return result;
        }
        catch (Exception)
        {
            return "0";
        }
        finally
        {
            conn.Close();
        }
    }
    /// <summary>
    /// 执行sql语句，返回一个表的集合
    /// </summary>
    /// <param name="Sql">sql语句或存储过程</param>
    /// <param name="values">所带的参数</param>
    /// <returns></returns>
    public DataTable GetDataSet(string Sql, params SqlParameter[] values)
    {
        conn = new SqlConnection(str);
        conn.Open();
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand(Sql, conn);
        cmd.Parameters.AddRange(values);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        conn.Close();
        return ds.Tables[0];

    }
    /// <summary>
    /// 执行sql语句，返回数据的条数
    /// </summary>
    /// <param name="safeSql"></param>
    /// <returns></returns>
    public int GetCount(string safeSql)
    {
        conn = new SqlConnection(str);
        conn.Open();
        SqlCommand cmd = new SqlCommand(safeSql, conn);
        try
        {
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }
        catch (Exception)
        {
            return 0;
        }
        finally
        {
            conn.Close();
        }
    }
    public int GetCount(string sql, params SqlParameter[] values)
    {
        conn = new SqlConnection(str);
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddRange(values);
        int result = Convert.ToInt32(cmd.ExecuteScalar());
        conn.Close();
        return result;
    }

}
