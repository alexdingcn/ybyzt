using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Company_GoodsNew_GoodsAreaEdit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["action"];
        if (obj != null)
        {
            string action = obj.ToString();
            if (action == "dislist")
            {
                string id = Request["id"];//区域id
                Response.Write(GetDisList(id));
                Response.End();
            }
            if (action == "goodslist")
            {
                string id = Request["id"];//分类id
                string name = Request["name"];//分类id
                Response.Write(GetGoodsList(id, name));
                Response.End();
            }
        }
    }
    /// <summary>
    /// 根据区域id得到代理商
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetDisList(string areaId)
    {
        string aredIdList = Common.DisAreaId(Convert.ToInt32(areaId), this.CompID);//递归得到区域id
        List<Hi.Model.BD_Distributor> ll = new Hi.BLL.BD_Distributor().GetList("ID,DisName", "isnull(dr,0)=0 and compId=" + this.CompID + " and areaId in(" + aredIdList + ")", "");
        if (ll.Count > 0)
        {
            DataTable dt2 = Common.FillDataTable<Hi.Model.BD_Distributor>(ll);
            if (dt2.Rows.Count != 0)
            {
                return ConvertJson.ToJson(dt2);
            }
        }
        return "[{}]";
    }
    /// <summary>
    /// 商品列表
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetGoodsList(string id, string name)
    {
        string where = string.Empty;
        name = name.Replace("'", "''");//商品名称
        if (name != "")
        {
            where += " and goodsname like '%" + name + "%'";
        }
        string cateID = Common.CategoryId(Convert.ToInt32(id), this.CompID);//商品分类递归
        List<Hi.Model.BD_Goods> l = new Hi.BLL.BD_Goods().GetList("", "isnull(dr,0)=0 and isenabled=1  and compid=" + this.CompID + " and categoryID in(" + cateID + ")" + where, "");
        if (l.Count > 0)
        {
            DataTable dt2 = Common.FillDataTable<Hi.Model.BD_Goods>(l);
            if (dt2.Rows.Count != 0)
            {
                return ConvertJson.ToJson(dt2);
            }
        }
        return "[{}]";
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            object disId = Request["disId"];//代理商id
            object goodsId = Request["goodsId"];//商品Id
            if (disId != null && goodsId != null)
            {
                for (int i = 0; i < disId.ToString().Split(',').Length; i++)
                {
                    for (int z = 0; z < goodsId.ToString().Split(',').Length; z++)
                    {
                        List<Hi.Model.BD_GoodsAreas> l = new Hi.BLL.BD_GoodsAreas().GetList("", "isnull(dr,0)=0 and compId=" + this.CompID + " and disId=" + disId.ToString().Split(',')[i] + " and goodsId=" + goodsId.ToString().Split(',')[z], "", Tran);
                        if (l.Count == 0)
                        {
                            Hi.Model.BD_GoodsAreas model = new Hi.Model.BD_GoodsAreas();
                            Hi.Model.BD_Distributor model2 = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(disId.ToString().Split(',')[i]), Tran);
                            Hi.Model.BD_Goods model3 = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(goodsId.ToString().Split(',')[z]), Tran);
                            model.CompID = this.CompID;
                            model.areaID = model2.AreaID;
                            model.GoodsID = Convert.ToInt32(goodsId.ToString().Split(',')[z]);
                            model.CategoryID = model3.CategoryID;
                            model.ts = DateTime.Now;
                            model.dr = 0;
                            model.modifyuser = this.UserID;
                            model.DisID = Convert.ToInt32(disId.ToString().Split(',')[i]);
                            new Hi.BLL.BD_GoodsAreas().Add(model, Tran);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            Tran.Commit();
            Response.Redirect("GoodsAreaList.aspx");
        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMethod(this, "保存失败了", JScript.IconOption.错误, "function(){location.href='GoodsAreaList.aspx';}");
            return;
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
}