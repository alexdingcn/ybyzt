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
using DBUtility;

/// <summary>
///MyPagination 的摘要说明
/// </summary>
public class MyPagination
{
    public MyPagination()
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
    public string GetJson(int pageIndex, int pageSize, string sql)
    {
        Pagger pagger = new Pagger(sql);
        int count = pagger.getDataCount();
        DataTable dt = pagger.getData(pageSize, (pageIndex - 1) * pageSize);
        return CreateJson(count, pageSize, dt);
    }
    /// <summary>
    /// 返回前台分页数据
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页数据</param>
    /// <param name="sql">查询sql</param>
    /// <returns></returns>
    public string GetJson2(int pageIndex, int pageSize, string sql, string sql2)
    {
        Pagger pagger = new Pagger(sql);
        int count = pagger.GetDataCount(sql2);
        DataTable dt = pagger.getData(pageSize, (pageIndex - 1) * pageSize);
        return CreateJson(count, pageSize, dt);
    }
    /// <summary>
    /// 返回前台分页数据
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页数据</param>
    /// <param name="sql">查询sql</param>
    /// <returns></returns>
    public DataTable GetDt(int pageIndex, int pageSize, string sql, string sql2)
    {
        Pagger pagger = new Pagger(sql);
        int count = pagger.GetDataCount(sql2);
        DataTable dt = pagger.getDataSql(pageSize, (pageIndex - 1) * pageSize);
        return dt;
    }

    /// <summary>
    /// 返回前台分页数据
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页数据</param>
    /// <param name="sql">查询sql</param>
    /// <returns></returns>
    public DataTable GetDt(int pageIndex, int pageSize, string sql, string sql2,out int count)
    {
        Pagger pagger = new Pagger(sql);
        count = pagger.GetDataCount(sql2);
        DataTable dt = pagger.getDataSql(pageSize, (pageIndex - 1) * pageSize);
        return dt;
    }

    /// <summary>
    /// 返回前台分页数据
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页数据</param>
    /// <param name="sql">查询sql</param>
    /// <returns></returns>
    public string GetJson3(int pageIndex, int pageSize, string sql, string sql2, int CompId, int DisID)
    {
        Pagger pagger = new Pagger(sql);
        int count = pagger.GetDataCount(sql2);
        DataTable dt = pagger.getData(pageSize, (pageIndex - 1) * pageSize);

        List<int> infoidl = new List<int>();

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                int id = item["ID"].ToString().ToInt(0);//BD_goodsInfo表的ID
                infoidl.Add(id);
            }
        }

        //获取商品价格
        List<BLL.gDprice> l = BLL.Common.GetPrice(CompId, DisID, infoidl);

        if (l != null && l.Count > 0)
        {
            foreach (var item in l)
            {
                DataRow[] dr = dt.Select(" ID=" + item.goodsInfoId);
                if (dr.Length > 0)
                {
                    //获取的价格大于促销价时、取促销价 
                    dr[0]["pr"] = item.FinalPrice;
                    dr[0]["typeTinkerPrice"] = item.typePrice;
                    dr[0]["disTinkerPrice"] = item.disPrice;
                    dr[0]["disProPr"] = item.bpPrice;
                }
            }
        }

        if (!dt.Columns.Contains("StockNum"))
        {
            DataColumn col = new DataColumn("StockNum", typeof(int));
            dt.Columns.Add(col);
            col.DefaultValue = 0;
            //dt.Columns["StockNum"].DataType = Type.GetType("System.Int");
        }
        //获取
        if (infoidl != null && infoidl.Count > 0)
        {
            var infoID = string.Join(",", infoidl);
            List<Hi.Model.DIS_GoodsStock> stocklist = new Hi.BLL.DIS_GoodsStock().GetList("", "Goodsinfo in (" + infoID + ")", "");
            if (stocklist != null && stocklist.Count > 0)
            {
                foreach (var item in stocklist)
                {
                    DataRow[] dr = dt.Select(" ID=" + item.GoodsInfo);
                    if (dr.Length > 0)
                    {
                        dr[0]["StockNum"] = item.StockTotalNum == 0 ? 0 : item.StockTotalNum;
                    }
                }
            }
        }

        return CreateJson(count, pageSize, dt);
    }
    /// <summary>
    /// 返回前台分页数据
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页数据</param>
    /// <param name="sql">查询sql</param>
    /// <returns></returns>
    public string GetJson4(int pageIndex, int pageSize, string sql, string sql2, int CompId, int DisID, string where)
    {
        Pagger pagger = new Pagger(sql);
        int count = pagger.GetDataCount(sql2);
        DataTable dt = pagger.getData(pageSize, (pageIndex - 1) * pageSize);

        List<int> infoidl = new List<int>();

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                int id = item["ID"].ToString().ToInt(0);//BD_goodsInfo表的ID
                infoidl.Add(id);
            }
        }

        //获取商品价格
        List<BLL.gDprice> l = BLL.Common.GetPrice(CompId, DisID, infoidl);

        if (l != null && l.Count > 0)
        {
            foreach (var item in l)
            {
                DataRow[] dr = dt.Select(" ID=" + item.goodsInfoId);
                if (dr.Length > 0)
                {
                    //获取的价格大于促销价时、取促销价 
                    dr[0]["pr"] = item.FinalPrice;
                    dr[0]["typeTinkerPrice"] = item.typePrice;
                    dr[0]["disTinkerPrice"] = item.disPrice;
                    dr[0]["disProPr"] = item.bpPrice;
                }
            }
        }

        return CreateJson(count, pageSize, dt);
    }
    /// <summary>
    /// 返回前台分页数据
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页数据</param>
    /// <param name="sql">查询sql</param>
    /// <returns></returns>
    public string GetJson(int pageIndex, int pageSize, string sql, string dbConn)
    {
        Pagger pagger = new Pagger(sql, dbConn);
        int count = pagger.getDataCount();
        DataTable dt = pagger.getData(pageSize, (pageIndex - 1) * pageSize);
        return CreateJson(count, pageSize, dt);
    }
    /// <summary>
    /// 得到 前台分页的json格式
    /// </summary>
    /// <param name="count">数据的总条数</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="dt">数据集</param>
    /// <returns></returns>
    public string CreateJson(int count, int pageSize, DataTable dt)
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
}
