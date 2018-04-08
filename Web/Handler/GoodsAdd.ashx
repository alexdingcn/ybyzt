<%@ WebHandler Language="C#" Class="GoodsAdd" %>

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using DBUtility;
using Hi.Model;
using LitJson;
using NPOI.HSSF.Record;

public class GoodsAdd : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        context.Response.Write(JsonToList(context));

        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public string JsonToList(HttpContext context)
    {
        int row = 1;
        int addNum = 0;
        List<Hi.Model.BD_Goods> list = new List<BD_Goods>();

        if (context.Request["Json"] != null)
        {
            JsonData JInfo = JsonMapper.ToObject(context.Request["Json"].ToString());
            if (JInfo.Count > 0 || JInfo["GoodsList"].ToString() != "")
            {
                foreach (JsonData info in JInfo["GoodsList"])
                {
                    string GoodsName = info["GoodsName"].ToString();
                    string Price = info["Price"].ToString();
                    string Unit = info["Unit"].ToString();
                    string Remark = info["Remark"].ToString();

                    if ((GoodsName == "" || Price == "" || Unit == "请选择") &&
                        (GoodsName.Trim() != "" || Price.Trim() != "" || Unit.Trim() != "请选择" || Remark != ""))
                        return "{\"falg\":\"" + false + "\",\"row\":\"" + row + "\",\"msg\":\"商品名称、基本价格、单位不能为空\"}";

                    if (GoodsName.Trim() != "" && Price.Trim() != "" && Unit.Trim() != "")
                    {
                        Hi.Model.BD_Goods goods = new BD_Goods();
                        goods.CompID = Convert.ToInt16(context.Request["CompID"].ToString());
                        goods.GoodsName = GoodsName;
                        goods.GoodsCode = "";
                        goods.SalePrice = Decimal.Parse(Price);
                        goods.Unit = Unit;
                        goods.memo = Remark ;
                        goods.IsEnabled = 1;
                        goods.CreateDate = DateTime.Now;
                        goods.CreateUserID = Convert.ToInt16(context.Request["UserID"].ToString());
                        goods.modifyuser = Convert.ToInt16(context.Request["UserID"].ToString());
                        goods.ts = DateTime.Now;
                        goods.dr = 0;
                        list.Add(goods);
                    }
                    row++;
                }
            }

            SqlConnection con = new SqlConnection(SqlHelper.LocalSqlServer);
            con.Open();
            SqlTransaction sqlTrans = con.BeginTransaction();
            try
            {
                if (context.Request["Json"] != null)
                {
                    foreach (var model in list)
                    {
                        List<Hi.Model.BD_Goods> goodsList = new Hi.BLL.BD_Goods().GetList("", " GoodsName='" + model.GoodsName + "' and CompID=" + model.CompID, "", sqlTrans);
                        if (goodsList.Count > 0)
                            return "{\"falg\":\"" + false + "\",\"row\":\"" + addNum + "\",\"msg\":\"商品不允许重名\"}";
                        int count = new Hi.BLL.BD_Goods().Add(con, model, sqlTrans);
                        if (count == 0)
                        {
                            sqlTrans.Rollback();
                        }

                        Hi.Model.BD_GoodsInfo goodsInfo = new BD_GoodsInfo();
                        goodsInfo.SalePrice = model.SalePrice;
                        goodsInfo.TinkerPrice = model.SalePrice;
                        goodsInfo.GoodsID = count;
                        goodsInfo.CompID = model.CompID;
                        goodsInfo.IsEnabled = true;
                        goodsInfo.CreateDate = DateTime.Now;
                        goodsInfo.CreateUserID = model.CreateUserID;
                        goodsInfo.modifyuser = model.CreateUserID;
                        goodsInfo.ts = DateTime.Now;
                        goodsInfo.dr = 0;
                        int res = new Hi.BLL.BD_GoodsInfo().Add(goodsInfo);
                        if (res == 0)
                        {
                            sqlTrans.Rollback();
                        }
                        addNum++;
                    }
                    if (0 == addNum)
                        return "{\"falg\":\"" + false + "\",\"row\":\"0\",\"msg\":\"数据不能为空\"}";
                }
                sqlTrans.Commit();
            }
            catch
            {
                sqlTrans.Rollback();
            }
            finally
            {
                con.Close();
            }
        }
        return "{\"falg\":\"" + true + "\",\"row\":\"" + row + "\",\"msg\":\"成功新增" + addNum + "件商品\"}";
    }
}