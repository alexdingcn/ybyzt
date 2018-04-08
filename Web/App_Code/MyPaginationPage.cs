using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;

/// <summary>
///MyPaginationPage 的摘要说明
/// </summary>
public class MyPaginationPage
{
    public MyPaginationPage()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 返回前台分页数据
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页数据</param>
    /// <param name="sql">查询sql</param>
    /// <returns></returns>
    public string GetJsonTable(int pageSize,int PageCount,DataTable dt)
    {
        return CreateJsonToTable(PageCount, pageSize, dt);
    }


    /// <summary>
    /// 返回前台分页数据
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页数据</param>
    /// <param name="sql">查询sql</param>
    /// <returns></returns>
    public string GetJsonList<T>(int pageSize, int PageCount, List<T> ListModel)
    {
        return CreateJsonToList<T>(PageCount, pageSize, ListModel);
    }

    /// <summary>
    /// 得到 前台分页的json格式
    /// </summary>
    /// <param name="count">数据的总条数</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="dt">数据集</param>
    /// <returns></returns>
    public string CreateJsonToTable(int count, int pageSize, DataTable dt)
    {
        int totalPage = 0;
        //计算总页数
        if (count == 0)
        {   //如果没有查询到数据 totalPage设为1
            totalPage = 1;
        }
        else if (count % pageSize == 0)
        {
            totalPage = count / pageSize;
        }
        else
        {
            totalPage = count / pageSize + 1;
        }
        string head = string.Format("{0}\"pageCount\":\"{1}\",\"totalCount\":\"{2}\",\"rows\":", "{", totalPage, count);
        string json = "";
        if (dt.Rows.Count == 0)
        {
            json = "[]";
        }
        else
        {
            json = ConvertJson.ToJson(dt);
        }
        string write = head + json + "}";
        return write;
    }

    /// <summary>
    /// 得到 前台分页的json格式
    /// </summary>
    /// <param name="count">数据的总条数</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="dt">数据集</param>
    /// <returns></returns>
    public string CreateJsonToList<T>(int count, int pageSize, List<T> ListModel)
    {
        int totalPage = 0;
        //计算总页数
        if (count == 0)
        {   //如果没有查询到数据 totalPage设为1
            totalPage = 1;
        }
        else if (count % pageSize == 0)
        {
            totalPage = count / pageSize;
        }
        else
        {
            totalPage = count / pageSize + 1;
        }
        string head = string.Format("{0}\"pageCount\":\"{1}\",\"totalCount\":\"{2}\",\"rows\":", "{", totalPage, count);
        string json = "";
        if (ListModel.Count == 0)
        {
            json = "[]";
        }
        else
        {
            json = "[" + new JavaScriptSerializer().Serialize(ListModel) + "]";
        }
        string write = head + json + "}";
        return write;
    }
}
