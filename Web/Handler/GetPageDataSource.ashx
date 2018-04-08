<%@ WebHandler Language="C#" Class="GetPageDataSource" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using DBUtility;
using System.Web.SessionState;

public class GetPageDataSource : IHttpHandler, IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string PageAction = context.Request["PageAction"];
        string ReturnMsg = "";
        switch (PageAction)
        {
            case "GetGoods": ReturnMsg = GetGoods(context); break;
            case "GetGoods1": ReturnMsg = GetGoods1(context); break;
            case "GetGoods2": ReturnMsg = GetGoods2(context); break;
            case "GetGoodsInfo": ReturnMsg = GetGoodsInfo(context); break;
            case "GetGoodsInfo1": ReturnMsg = GetGoodsInfo1(context); break;
            case "optdis": ReturnMsg = optdis(context); break;
        }
        context.Response.Write(ReturnMsg);
        context.Response.End();
    }
    public string GetGoods1(HttpContext context)
    {
        string rowid = string.Empty;//是否需要查询多规格属性
        int PageSize = context.Request["pagesize"].ToInt(1);
        int CurtIndex = context.Request["page"].ToInt(1);
        int CompId = context.Request["CompId"].ToInt(0);
        int DisID = context.Request["DisId"].ToInt(0);
        string goodsId = context.Request["goodsId"] + "";
        string shouc = context.Request["shouc"] + "";//是否收藏商品
        StringBuilder strwhere = new StringBuilder();
        if (!string.IsNullOrEmpty(shouc))
        {
            strwhere.AppendFormat(" and coll.GoodsID is not null ");
        }
        if (!string.IsNullOrEmpty(goodsId))
        {
            strwhere.AppendFormat(" and info.GoodsID ={0}", goodsId);
        }
        else
        {
            rowid = "where rowid=1";
        }
        //商品是否启用库存
        //  int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompId).ToInt(0);
        // if (IsInve == 0)
        //      strwhere.AppendFormat("and info.Inventory>0");

        //商品名称、编码搜索
        if (!string.IsNullOrWhiteSpace(context.Request["txtGoods"]) && context.Request["txtGoods"] != "商品名称搜索")
            strwhere.AppendFormat("and (g.GoodsName like '%{0}%' Or info.BarCode like '%{0}%' )", context.Request["txtGoods"]);

        //促销
        string str = "";
        if (!string.IsNullOrWhiteSpace(context.Request["goodspro"]))
        {
            if (str != "")
                str += "," + context.Request["goodspro"];
            else
                str += context.Request["goodspro"];
        }
        //if (!string.IsNullOrWhiteSpace(context.Request["pro"]))
        //{
        //    if (str != "")
        //        str += "," + context.Request["pro"];
        //    else
        //        str += context.Request["pro"];
        //}
        if (str != "")
        {
            strwhere.AppendFormat("and isnull(ProID,0)<>0 and pro.Type in ({0})", str);
        }
        string sql = SelectGoodsInfo.Returnsql2(CompId.ToString(), DisID.ToString(), strwhere.ToString(), rowid,"1");
        if (rowid == "")
        {
            return ConvertJson.ToJson(SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0]);
        }
        else
        {
            MyPagination mypag = new MyPagination();
            string data = mypag.GetJson4(CurtIndex, PageSize, sql, sql, CompId, DisID, strwhere.ToString());
            return data;
        }
    }
    public string GetGoods(HttpContext context)
    {
        int PageSize = context.Request["pagesize"].ToInt(1);
        int CurtIndex = context.Request["page"].ToInt(1);
        int CompId = context.Request["CompId"].ToInt(0);
        int DisID = context.Request["DisId"].ToInt(0);
        string Utype = context.Request["Utype"] + "";   //1 、代理商   2、厂商
        string goodsInfoIdList = context.Request["goodsInfoIdList"] + "";

        StringBuilder strwhere = new StringBuilder();

        //商品是否启用库存
        int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompId).ToInt(0);
        if (IsInve == 0)
        {
            if (context.Request["wwwtype"] != null)
            {
                if (context.Request["wwwtype"].ToString() == "1")
                {
                    strwhere.AppendFormat("and info.Inventory>=0");
                }
                else
                {
                    strwhere.AppendFormat("and info.Inventory>0");
                }
            }
        }
        if (!string.IsNullOrEmpty(goodsInfoIdList))
            strwhere.AppendFormat(" and info.ID not in({0})", goodsInfoIdList);

        //商品名称、编码搜索
        if (!string.IsNullOrWhiteSpace(context.Request["txtGoods"]) && context.Request["txtGoods"] != "商品名称搜索")
            strwhere.AppendFormat("and (g.GoodsName like '%{0}%' Or info.BarCode like '%{0}%' )", context.Request["txtGoods"]);

        //商品分类搜索
        if (!string.IsNullOrEmpty(context.Request["cat"]))
        {
            var cat = (context.Request["cat"]).Trim();
            strwhere.AppendFormat("and g.CategoryID in ({0})", " select id from  SYS_GType where FullCode like ''+ (select FullCode from SYS_GType  where ID=" + cat + ") +'%'");
        }

        //促销
        string str = "";
        if (!string.IsNullOrWhiteSpace(context.Request["goodspro"]))
        {
            if (str != "")
                str += "," + context.Request["goodspro"];
            else
                str += context.Request["goodspro"];
        }
        if (!string.IsNullOrWhiteSpace(context.Request["pro"]))
        {
            if (str != "")
                str += "," + context.Request["pro"];
            else
                str += context.Request["pro"];
        }
        if (str != "")
            strwhere.AppendFormat("and isnull(ProID,0)<>0 and pro.Type in ({0})", str);

        string sql = SelectGoodsInfo.Returnsql(CompId.ToString(), DisID.ToString(), strwhere.ToString(), Utype);

        #region
        ////查询商品信息
        //Pagger pagger = new Pagger(sql);
        //Counts = pagger.GetDataCount(sql);
        //DataTable dt = pagger.getData(PageSize, CurtIndex - 1);

        //MyPaginationPage mypag = new MyPaginationPage();
        //return mypag.GetJsonTable(PageSize, Counts, dt);
        #endregion

        MyPagination mypag = new MyPagination();
        string data = mypag.GetJson3(CurtIndex, PageSize, sql, sql, CompId, DisID);
        return data;
    }
    public string GetGoods2(HttpContext context)
    {
        int PageSize = context.Request["pagesize"].ToInt(1);
        int CurtIndex = context.Request["page"].ToInt(1);
        int CompId = context.Request["CompId"].ToInt(0);
        string goodsInfoIdList = context.Request["goodsInfoIdList"] + "";

        StringBuilder strwhere = new StringBuilder();

        if (!string.IsNullOrEmpty(goodsInfoIdList))
            strwhere.AppendFormat(" and b.ID not in({0})", goodsInfoIdList);

        //商品名称、编码搜索
        if (!string.IsNullOrWhiteSpace(context.Request["txtGoods"]) && context.Request["txtGoods"] != "商品名称搜索")
            strwhere.AppendFormat("and (a.GoodsName like '%{0}%' Or b.BarCode like '%{0}%' )", context.Request["txtGoods"]);

        //商品分类搜索
        if (!string.IsNullOrEmpty(context.Request["cat"]))
        {
            var cat = context.Request["cat"];
            strwhere.AppendFormat("and a.CategoryID in ({0})", Common.CategoryId(cat.ToInt(0), CompId));
        }

        string sql = string.Format(@"select a.GoodsName,b.BarCode,a.Unit,b.ValueInfo,b.TinkerPrice,a.Pic,b.ID
 from BD_Goods as a ,BD_GoodsInfo as b
where a.ID=b.GoodsID and ISNULL(a.dr,0)=0 and ISNULL(b.dr,0)=0
and a.CompID={0} and b.CompID={0} and b.IsOffline=1 {1} order by b.id desc", CompId, strwhere);
        MyPagination mypag = new MyPagination();
        string data = mypag.GetJson2(CurtIndex, PageSize, sql, sql);
        return data;
    }
    public string GetGoodsInfo(HttpContext context)
    {
        string goodsInfoId = context.Request["goodsInfoId"];
        int CompId = context.Request["CompId"].ToInt(0);
        int DisID = context.Request["DisId"].ToInt(0);
        string Utype = context.Request["Utype"] + "";   //1 、代理商   2、厂商
        
        StringBuilder strwhere = new StringBuilder();

        //商品是否启用库存
        int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompId).ToInt(0);
        if (IsInve == 0)
        {
            strwhere.AppendFormat("and info.Inventory>0");
        }
        strwhere.AppendFormat("and info.ID in(" + goodsInfoId + ")");
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, SelectGoodsInfo.Returnsql(CompId.ToString(), DisID.ToString(), strwhere.ToString(), Utype)).Tables[0];

        System.Collections.Generic.List<int> infoidl = new System.Collections.Generic.List<int>();

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                int id = item["ID"].ToString().ToInt(0);//BD_goodsInfo表的ID
                infoidl.Add(id);
            }
        }

        //获取商品价格
        System.Collections.Generic.List<BLL.gDprice> l = BLL.Common.GetPrice(Convert.ToInt32(CompId), Convert.ToInt32(DisID), infoidl);

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


        return ConvertJson.ToJson2(dt);
    }
    public string GetGoodsInfo1(HttpContext context)
    {
        string valueinfo = context.Request["valueinfo"];
        int CompId = context.Request["CompId"].ToInt(0);
        int DisID = context.Request["DisId"].ToInt(0);
        int goodsId = context.Request["goodsId"].ToInt(0);
        StringBuilder strwhere = new StringBuilder();

        //商品是否启用库存
        int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompId).ToInt(0);
        if (IsInve == 0)
        {
            strwhere.AppendFormat("and info.Inventory>0");
        }
        strwhere.AppendFormat("and info.ValueInfo ='" + valueinfo + "'");
        strwhere.AppendFormat(" and info.GoodsID=" + goodsId);
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, SelectGoodsInfo.Returnsql2(CompId.ToString(), DisID.ToString(), strwhere.ToString(),"","1")).Tables[0];

        System.Collections.Generic.List<int> infoidl = new System.Collections.Generic.List<int>();

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                int id = item["ID"].ToString().ToInt(0);//BD_goodsInfo表的ID
                infoidl.Add(id);
            }
        }

        //获取商品价格
        System.Collections.Generic.List<BLL.gDprice> l = BLL.Common.GetPrice(Convert.ToInt32(CompId), Convert.ToInt32(DisID), infoidl);

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
        return ConvertJson.ToJson(dt); //dt.Rows[0]["pr"].ToString();
    }
    /// <summary>
    ///查询厂商下的代理商
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string optdis(HttpContext context)
    {
        int PageSize = context.Request["pagesize"].ToInt(1);
        int CurtIndex = context.Request["page"].ToInt(1);
        int CompId = context.Request["CompId"].ToInt(0);

        StringBuilder strwhere = new StringBuilder();

        string sql = @"select dis.ID,dis.DisName,cu.AreaID,cu.DisTypeID,a.ID aID,a.AreaName,t.ID tID,t.TypeName from SYS_CompUser cu left join BD_Distributor dis on cu.DisID=dis.ID left join BD_DisArea a on cu.AreaID=a.ID and isnull(a.dr,0)=0 left join  BD_DisType t on cu.DisTypeID=t.id and isnull(t.dr,0)=0 where cu.CType=2 and UType=5 and  cu.IsAudit=2 and cu.IsEnabled=1 and isnull(dis.dr,0)=0 and isnull(cu.dr,0)=0 ";

        if (CompId != 0)
            strwhere.AppendFormat(" and cu.CompID={0} and isnull(dis.dr,0)=0", CompId);

        if (!string.IsNullOrEmpty(context.Request["disnc"]))
        {
            var disnc =Common.NoHTML( context.Request["disnc"]);
            strwhere.AppendFormat(" and (dis.DisName like '%" + disnc + "%' or dis.DisCode like '%" + disnc + "%')");
        }
        if (!string.IsNullOrEmpty(context.Request["distype"]))
        {
            var typeId = context.Request["distype"].ToInt(0);
            strwhere.AppendFormat(" and cu.DisTypeID in ({0})", Common.DisTypeId(typeId, CompId));
        }

        if (!string.IsNullOrEmpty(strwhere.ToString()))
            sql +=  strwhere;

        sql += " order by dis.ID desc";

        MyPagination mypag = new MyPagination();
        string data = mypag.GetJson2(CurtIndex, PageSize, sql, sql);
        return data;
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}