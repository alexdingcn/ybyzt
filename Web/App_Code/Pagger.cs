using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using DBUtility;

public class Pagger
{

    private string _sql = string.Empty;
    private string _sqlorder = string.Empty;
    private string _sqlcount = string.Empty;
    /// <summary>
    /// 查询语句
    /// </summary>
    public string Sql
    {
        set
        {
            if (IsNullOrEmpty(value))
                throw new Exception("查询语句不能为空。");
            if (value.ToLower().IndexOf("select") != 0)
                throw new Exception("查询语句必须以SELECT开头。");
            //找到最后一个ORDER BY子句
            int pos = value.ToLower().LastIndexOf("order by");
            if (pos < 0)
                throw new Exception("查询语句没有指定ORDER BY子句。");
            pos += 8;
            //判断ORDER BY是否处在一个子查询中
            if (getSubstringCount(value, "(", pos) < getSubstringCount(value, ")", pos))
                throw new Exception("查询语句没有指定ORDER BY子句。");
            //提取ORDER BY子句的内容
            _sqlorder = value.Substring(pos).Trim();
            //生成统计语句
            string countStr = string.Empty;
            countStr += @"(?<=^select\s)[^()]*"; //匹配查询字段开始
            countStr += @"(";                       //匹配子查询语句开始，开始查找括号
            countStr += @"\(((?<sub1>\()|(?<-sub1>\))|[^()])*(?(sub1)(?!))\)";          //匹配一个闭合的括号
            countStr += @"([^()]*\(((?<sub2>\()|(?<-sub2>\))|[^()])*(?(sub2)(?!))\))*"; //如果在闭合括号之后还有括号，则继续匹配
            countStr += @")?";                      //匹配子查询语句结束
            countStr += @"[^()]*(?=\sfrom)";    //匹配查询字符结束
            _sqlcount = Regex.Replace(value, countStr, "count(*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            _sqlcount = Regex.Replace(_sqlcount, @" order by .+?$", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);
            _sql = value;
        }
        get
        {
            return _sql;
        }
    }
    public Pagger(string strsql)
    {
        Sql = strsql;
    }
    public Pagger(string strsql, string ConnectionString)
    {
        Sql = strsql;
        // Dbhelp = new NewClassesAdmin.LIB.DbHelperSQL(ConnectionString);
    }
    public DataTable getData(int maximumRows, int startRowIndex)
    {
        string strsql = "SELECT * FROM (SELECT TOP 100 PERCENT ROW_NUMBER() OVER (ORDER BY " + _sqlorder + ") Row," + _sql.Substring(7).Replace(" order by " + _sqlorder, string.Empty) + ") TMP WHERE Row>" + startRowIndex + " AND Row<=" + (maximumRows + startRowIndex) + " order by Row";
        return SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
        //return SqlHelper.ExecuteDataReaderToTable(strsql, 1000);
    }

    public DataTable getDataSql(int maximumRows, int startRowIndex)
    {
        string strsql = "SELECT * FROM (SELECT TOP 100 PERCENT ROW_NUMBER() OVER (ORDER BY " + _sqlorder + ") 序号," + _sql.Substring(7).Replace(" order by " + _sqlorder, string.Empty) + ") TMP WHERE 序号>" + startRowIndex + " AND 序号<=" + (maximumRows + startRowIndex) + " order by 序号";
        return SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
        //return SqlHelper.ExecuteDataReaderToTable(strsql, 1000);
    }

    public int getDataCount()
    {
        //_sqlcount = "select count(*) from(" + _sqlcount + ")aa";
        if (SqlHelper.GetSingle(SqlHelper.LocalSqlServer, _sqlcount) == null)
        {
            return 0;
        }
        else
        {
            return (int)SqlHelper.GetSingle(SqlHelper.LocalSqlServer, _sqlcount);
        }
    }
    public int GetDataCount(string SQLValue)
    {
        string SQL = SQLValue.Remove(SQLValue.ToLower().LastIndexOf("order by"));
        return (int)SqlHelper.GetSingle(SqlHelper.LocalSqlServer, "select COUNT(*) from (" + SQL + ") temp");
    }
    /// <summary>
    /// 判断字符串是否为空
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool IsNullOrEmpty(string str)
    {
        if (string.IsNullOrEmpty(str))
            return true;
        return str.Trim() == string.Empty;
    }
    public bool IsNullOrEmpty(object obj)
    {
        if (obj != null)
            return IsNullOrEmpty(obj.ToString());
        return true;
    }
    public bool IsNullOrEmpty(Uri uri)
    {
        if (uri != null)
            return IsNullOrEmpty(uri.ToString());
        return true;
    }
    /// <summary>
    /// 判断子字符串出现的次数
    /// </summary>
    /// <param name="strSource"></param>
    /// <param name="strSub"></param>
    /// <returns></returns>
    public int getSubstringCount(string strSource, string strSub)
    {
        return getSubstringCount(strSource, strSub, 0);
    }
    public int getSubstringCount(string strSource, string strSub, int startIndex)
    {
        //判断起始位置是否越界
        if (IsNullOrEmpty(strSource) || IsNullOrEmpty(strSub))
            return 0;
        if (startIndex >= strSource.Length)
            return 0;
        //得到子字符串出现的次数
        strSource = strSource.Substring(startIndex);
        return (strSource.Length - strSource.Replace(strSub, string.Empty).Length) / strSub.Length;
    }
}

