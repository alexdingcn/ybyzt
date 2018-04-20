<%@ WebHandler Language="C#" Class="GetPrice" %>

using System;
using System.Web;

public class GetPrice : loginInfoMation, IHttpHandler
{
    public int page = 1;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        object action = context.Request["action"];
        page = Convert.ToInt32(context.Request["page"]);
        if (action != null)
        {
            if (action.ToString() == "getData")
            {
                new loginInfoMation().LoadData();
                string compId = context.Request["compId"];
                context.Response.Write(BindData(compId));
                context.Response.End();
            }
            if (action.ToString() == "getDisType")
            {
                string compId = context.Request["compId"];//厂商id
                string parentId = context.Request["parentId"];//父级id
                context.Response.Write(BindDisType(compId, parentId));//绑定代理商分类
                context.Response.End();
            }
            if (action.ToString() == "getDisArea")
            {
                string compId = context.Request["compId"];//厂商id
                string parentId = context.Request["parentId"];//父级id
                context.Response.Write(BindDisArea(compId, parentId));//绑定代理商分类
                context.Response.End();
            }
            if (action.ToString() == "selectDis")//查询具体代理商
            {
                string compId = context.Request["compId"];//厂商id
                string dis = context.Request["objDis"];//代理商id
                context.Response.Write(SelectDis(compId, dis));
                context.Response.End();
            }
            if (action.ToString() == "selectgoods")//查询下拉商品
            {
                string compId = context.Request["compId"];//厂商id
                string goodsInfoid = context.Request["goodsInfoId"];//商品id
                context.Response.Write(SelectGoods(compId, goodsInfoid));
                context.Response.End();
            }
            if (action.ToString() == "IsChkGoods")//检查是否有调过价格
            {
                string compId = context.Request["compId"];//厂商id
                context.Response.Write(IsChkGoods(compId));
                context.Response.End();
            }


            if (action.ToString() == "bindgoods")//查询绑定商品
            {
                string compId = context.Request["compId"];//厂商id
                string parentidlist = context.Request["parentidlist"];//过滤已存在的商品id
                string type = context.Request["type"];//区分等级、区域
                context.Response.Write(BindGoods(compId, parentidlist, type));
                context.Response.End();
            }
            if (action.ToString() == "bindgoodss")//查询绑定商品
            {
                string compId = context.Request["compId"];//厂商id
                string disid = context.Request["disid"];//代理商id
                context.Response.Write(BindGoods2(compId, disid));
                context.Response.End();
            }


            if (action.ToString() == "inertDisPrice")
            {//插入调价表
                new loginInfoMation().LoadData();
                string compId = context.Request["compId"];//厂商id
                string parentidlist = context.Request["parentidlist"];
                string type = context.Request["type"];//区分等级、区域
                string json = context.Request["json"];//代理商id
                context.Response.Write(InsertGoods(compId, parentidlist, type, json));
                context.Response.End();
            }
            if (action.ToString() == "inertGoodsPrice")
            {//插入代理商调价表
                new loginInfoMation().LoadData();
                string compId = context.Request["compId"];//厂商id
                string disId = context.Request["disId"];//代理商id
                string json = context.Request["json"];//商品数据
                context.Response.Write(InsertGoodsPrice(compId, disId, json));
                context.Response.End();
            }
            if (action.ToString() == "IsEnabledDisPrice")
            {//禁用\启用
                new loginInfoMation().LoadData();
                string compId = context.Request["compId"];//厂商id
                string parentidlist = context.Request["parentidlist"];
                string type = context.Request["type"];//区分等级、区域
                string type2 = context.Request["type2"];//1 启用 0启用
                context.Response.Write(IsEnableDisPrice(compId, parentidlist, type, type2));
                context.Response.End();
            }
            if (action.ToString() == "IsEnabledGoodsPrice")
            {//禁用\启用
                new loginInfoMation().LoadData();
                string compId = context.Request["compId"];//厂商id
                string disid = context.Request["disid"];//代理商id
                string type2 = context.Request["type2"];//1 启用 0启用
                context.Response.Write(IsEnableGoodsPrice(compId, disid, type2));
                context.Response.End();
            }
        }
    }
    /// <summary>
    /// 检查是否调过价格
    /// </summary>
    /// <returns></returns>
    public string IsChkGoods(string compid)
    {
        System.Collections.Generic.List<Hi.Model.BD_GoodsPrice> l = new Hi.BLL.BD_GoodsPrice().GetList("top 1 *", "isnull(dr,0)=0 and compid=" + compid + " and goodsname is not null", "id desc");
        if (l.Count > 0)
        {
            return l[0].DisID.ToString();
        }
        return "";
    }
    /// <summary>
    /// 启用、禁用代理商调价
    /// </summary>
    /// <returns></returns>
    public string IsEnableGoodsPrice(string compid, string disid, string type)
    {
        System.Data.SqlClient.SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            System.Collections.Generic.List<Hi.Model.BD_GoodsPrice> l = new Hi.BLL.BD_GoodsPrice().GetList("", "isnull(dr,0)=0 and compid=" + compid + " and     disid=" + disid + " and goodsname is not null", "", Tran);
            System.Collections.Generic.List<Hi.Model.BD_GoodsPrice> ll = new System.Collections.Generic.List<Hi.Model.BD_GoodsPrice>();
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_GoodsPrice item in l)
                {
                    Hi.Model.BD_GoodsPrice model = new Hi.BLL.BD_GoodsPrice().GetModel(item.ID, Tran);
                    model.IsEnabled = (type == "1" ? true : false);
                    model.modifyuser = UserID;
                    model.ts = DateTime.Now;
                    ll.Add(model);
                }
                new Hi.BLL.BD_GoodsPrice().Update(ll, Tran);
                Tran.Commit();
                return "cg";
            }
            else
            {
                Tran.Rollback();
                return "";
            }
        }
        catch (Exception)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
            return "";
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
    /// <summary>
    /// 禁用、启用
    /// </summary>
    /// <returns></returns>
    public string IsEnableDisPrice(string compid, string parentidlist, string type, string type2)
    {
        if (type2 == "1")
        {
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> disl = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0  and type=" + (type == "0" ? "2" : "1") + " and isenabled=1 and compid=" + compid, "");
            if (disl.Count > 0)
            {
                return "sb";
            }
        }
        System.Data.SqlClient.SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            int disType = 0;
            string where = string.Empty;
            if (type == "0")
            {
                disType = 1;
            }
            else
            {
                disType = 2;
            }
            string[] str = parentidlist.Split(',');
            if (str.Length == 3)
            {
                where = string.Format("One={0} and Two={1} and Three={2}", str[0], str[1], str[2]);
            }
            else if (str.Length == 2)
            {
                where = string.Format("One={0} and Two={1} and Three=0", str[0], str[1]);
            }
            else if (str.Length == 1)
            {
                where = string.Format("One={0}  and Two=0 and Three=0", str[0]);
            }
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> l = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and " + where + " and type=" + disType + " and compid=" + compid, "", Tran);
            if (l.Count > 0)
            {
                Hi.Model.BD_DisPrice model = new Hi.BLL.BD_DisPrice().GetModel(Convert.ToInt32(l[0].ID), Tran);
                model.IsEnabled = (type2 == "1" ? true : false);
                model.modifyuser = UserID;
                model.ts = DateTime.Now;
                new Hi.BLL.BD_DisPrice().Update(model, Tran);//禁用启用主表
                System.Collections.Generic.List<Hi.Model.BD_DisPriceInfo> ll = new Hi.BLL.BD_DisPriceInfo().GetList("", "isnull(dr,0)=0 and disPriceid=" + l[0].ID + " and compid=" + compid, "");
                if (ll.Count > 0)
                {
                    System.Collections.Generic.List<Hi.Model.BD_DisPriceInfo> lll = new System.Collections.Generic.List<Hi.Model.BD_DisPriceInfo>();
                    foreach (Hi.Model.BD_DisPriceInfo item in ll)
                    {
                        Hi.Model.BD_DisPriceInfo model2 = new Hi.BLL.BD_DisPriceInfo().GetModel(item.ID, Tran);
                        model2.IsEnabled = (type2 == "1" ? true : false);
                        model2.modifyuser = UserID;
                        model2.ts = DateTime.Now;
                        lll.Add(model2);
                    }
                    new Hi.BLL.BD_DisPriceInfo().Update(lll, Tran);//后禁用启用附表
                    Tran.Commit();
                    return "cg";
                }
                else
                {
                    Tran.Rollback();
                    return "";
                }
            }
            else
            {
                Tran.Rollback();
                return "";
            }

        }
        catch (Exception)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
            return "";
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
    /// <summary>
    /// 检查是否同时存在代理商分类和区域调价
    /// </summary>
    /// <returns></returns>
    public string IsChkDisPrice(string compid, string type)
    {
        System.Collections.Generic.List<Hi.Model.BD_DisPrice> l = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and compid=" + compid + " and type=" + (type == "0" ? 2 : 1), "");
        if (l.Count > 0)
        {
            return "ycz";
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// 检查是否存在同上下级数据
    /// </summary>
    /// <returns></returns>
    public string IsChkDisPrice(string compid, string parentidlist, string type)
    {
        string where = string.Empty;
        int num = parentidlist.Split(',').Length;
        if (num == 1)
        {
            where = string.Format(" one={0} and two!={1} and three={2}", parentidlist.Split(',')[0], 0, 0);
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> disll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and" + where + " and type=" + (type == "0" ? 1 : 2) + " and compid=" + compid, "");
            if (disll.Count > 0)
            { //判断是否存在上下级的数据
                return "ycz";
            }
            where = string.Format(" one={0} and two!={1} and three!={2}", parentidlist.Split(',')[0], 0, 0);
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> dislll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and" + where + " and type=" + (type == "0" ? 1 : 2) + " and compid=" + compid, "");
            if (dislll.Count > 0)
            { //判断是否存在上下级的数据
                return "ycz";
            }
        }
        else if (num == 2)
        {
            where = string.Format(" one={0} and two={1} and three={2}", parentidlist.Split(',')[0], 0, 0);
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> disll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and" + where + " and type=" + (type == "0" ? 1 : 2) + " and compid=" + compid, "");
            if (disll.Count > 0)
            { //判断是否存在上下级的数据
                return "ycz";
            }
            where = string.Format(" one={0} and two={1} and three!={2}", parentidlist.Split(',')[0], parentidlist.Split(',')[1], 0);
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> dislll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and" + where + " and type=" + (type == "0" ? 1 : 2) + " and compid=" + compid, "");
            if (dislll.Count > 0)
            { //判断是否存在上下级的数据
                return "ycz";
            }
        }
        else if (num == 3)
        {
            where = string.Format(" one={0} and two={1} and three={2}", parentidlist.Split(',')[0], 0, 0);
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> disll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and" + where + " and type=" + (type == "0" ? 1 : 2) + " and compid=" + compid, "");
            if (disll.Count > 0)
            { //判断是否存在上下级的数据
                return "ycz";
            }
            where = string.Format(" one={0} and two={1} and three={2}", parentidlist.Split(',')[0], parentidlist.Split(',')[1], 0);
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> dislll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and" + where + " and type=" + (type == "0" ? 1 : 2) + " and compid=" + compid, "");
            if (dislll.Count > 0)
            { //判断是否存在上下级的数据
                return "ycz";
            }
        }
        return "";
    }
    /// <summary>
    /// 插入代理商调价表
    /// </summary>
    /// <returns></returns>
    public string InsertGoodsPrice(string compid, string disId, string json)
    {
        System.Data.SqlClient.SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            System.Collections.Generic.List<Hi.Model.BD_GoodsPrice> l = new Hi.BLL.BD_GoodsPrice().GetList("", "isnull(dr,0)=0 and compid=" + compid + " and disid=" + disId + " and goodsname is not null", "", Tran);
            if (l.Count > 0)
            {
                new Hi.BLL.BD_GoodsPrice().Delete(Convert.ToInt32(compid), Convert.ToInt32(disId), Tran);
                if (json == "[]")
                {
                    Tran.Commit();
                    return "cg";
                }
            }
            Newtonsoft.Json.Linq.JArray llll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            System.Collections.Generic.List<Hi.Model.BD_GoodsPrice> lll = new System.Collections.Generic.List<Hi.Model.BD_GoodsPrice>();
            if (llll.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject ja in llll)
                {
                    Hi.Model.BD_GoodsPrice model2 = new Hi.Model.BD_GoodsPrice();
                    model2.CompID = Convert.ToInt32(compid);
                    model2.DisID = Convert.ToInt32(disId);
                    model2.GoodsInfoID = Convert.ToInt32(ja["goodsinfoid"].ToString());
                    model2.GoodsName = ja["name"].ToString();
                    model2.BarCode = ja["code"] != null ? ja["code"].ToString() : "";
                    model2.InfoValue = ja["info"] != null ? ja["info"].ToString() : "";
                    model2.Unit = ja["unit"].ToString();
                    model2.TinkerPrice = Convert.ToDecimal(ja["price"].ToString());
                    model2.IsEnabled = true;// l.Count > 0 ? l[0].IsEnabled : false;
                    model2.CreateDate = DateTime.Now;
                    model2.CreateUserID = UserID;
                    model2.modifyuser = UserID;
                    model2.ts = DateTime.Now;
                    lll.Add(model2);
                }
                new Hi.BLL.BD_GoodsPrice().Add(lll, Tran);
                Tran.Commit();
                return "cg";
            }
            else
            {
                Tran.Rollback();
                return "";
            }
        }
        catch (Exception)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
            return "";
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
    /// <summary>
    /// 插入调价表
    /// </summary>
    /// <returns></returns>
    public string InsertGoods(string compid, string parentidlist, string type, string json)
    {
        if (IsChkDisPrice(compid, parentidlist, type) != "")
        {
            return "ycz";
        }
        if (IsChkDisPrice(compid, type) != "")
        {
            return "yczl";
        }
        System.Data.SqlClient.SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            int disPriceId = 0;
            int disType = 0;
            string where = string.Empty;
            if (type == "0")
            {
                disType = 1;
            }
            else
            {
                disType = 2;
            }
            string[] str = parentidlist.Split(',');
            if (str.Length == 3)
            {
                where = string.Format("One={0} and Two={1} and Three={2}", str[0], str[1], str[2]);
            }
            else if (str.Length == 2)
            {
                where = string.Format("One={0} and Two={1} and Three=0", str[0], str[1]);
            }
            else if (str.Length == 1)
            {
                where = string.Format("One={0}  and Two=0 and Three=0", str[0]);
            }
            System.Collections.Generic.List<Hi.Model.BD_DisPrice> l = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and " + where + " and type=" + disType + " and compid=" + compid, "", Tran);
            if (l.Count > 0)
            {
                //已存在先删除
                disPriceId = l[0].ID;
                new Hi.BLL.BD_DisPrice().Delete(disPriceId, Tran);
                new Hi.BLL.BD_DisPriceInfo().Delete(disPriceId, Tran);
                if (json == "[]")
                {
                    Tran.Commit();
                    return "cg";
                }
            }
            //else
            //{
            string strlistId = string.Empty;//接受递归的查询条件
            Hi.Model.BD_DisPrice model = new Hi.Model.BD_DisPrice();
            model.CompID = Convert.ToInt32(compid);
            //model.Title = "";
            //model.State = 0;
            //model.DisIDs = "";
            //model.DisNames = "";
            model.Type = disType;
            if (str.Length == 3)
            {
                model.One = Convert.ToInt32(str[0]);
                model.Two = Convert.ToInt32(str[1]);
                model.Three = Convert.ToInt32(str[2]);
                strlistId = str[2];
            }
            else if (str.Length == 2)
            {
                model.One = Convert.ToInt32(str[0]);
                model.Two = Convert.ToInt32(str[1]);
                if (disType == 1)
                {
                    strlistId = Common.DisTypeId(Convert.ToInt32(str[1]), Convert.ToInt32(compid));
                }
                else if (disType == 2)
                {
                    strlistId = Common.DisAreaId(Convert.ToInt32(str[1]), Convert.ToInt32(compid));
                }
            }
            else if (str.Length == 1)
            {
                model.One = Convert.ToInt32(str[0]);
                if (disType == 1)
                {
                    strlistId = Common.DisTypeId(Convert.ToInt32(str[0]), Convert.ToInt32(compid));
                }
                else if (disType == 2)
                {
                    strlistId = Common.DisAreaId(Convert.ToInt32(str[0]), Convert.ToInt32(compid));
                }
            }
            string where2 = string.Empty;
            if (disType == 1)
            {
                where2 = " and distypeid in(" + strlistId + ")";
            }
            else if (disType == 2)
            {
                where2 = " and areaid in(" + strlistId + ")";
            }
            string disids = string.Empty;
            System.Collections.Generic.List<Hi.Model.BD_Distributor> ll = new Hi.BLL.BD_Distributor().GetList("", "isnull(dr,0)=0 and compId=" + compid + where2, "");
            if (ll.Count > 0)
            {
                foreach (Hi.Model.BD_Distributor item in ll)
                {
                    disids += item.ID + ",";
                }
            }
            if (disids != "")
            {
                disids = "," + disids;
            }
            model.DisIDs = disids;
            model.IsEnabled = true; // l.Count > 0 ? l[0].IsEnabled : false;
            model.CreateDate = DateTime.Now;
            model.CreateUserID = UserID;
            model.modifyuser = UserID;
            model.ts = DateTime.Now;
            disPriceId = new Hi.BLL.BD_DisPrice().Add(model, Tran);
            //}
            Newtonsoft.Json.Linq.JArray llll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            System.Collections.Generic.List<Hi.Model.BD_DisPriceInfo> lll = new System.Collections.Generic.List<Hi.Model.BD_DisPriceInfo>();
            if (llll.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject ja in llll)
                {
                    Hi.Model.BD_DisPriceInfo model2 = new Hi.Model.BD_DisPriceInfo();
                    model2.CompID = Convert.ToInt32(compid);
                    model2.DisPriceID = disPriceId;
                    model2.GoodsInfoID = Convert.ToInt32(ja["goodsinfoid"].ToString());
                    model2.GoodsName = ja["name"].ToString();
                    model2.BarCode = ja["code"] != null ? ja["code"].ToString() : "";
                    model2.InfoValue = ja["info"] != null ? ja["info"].ToString() : "";
                    model2.Unit = ja["unit"].ToString();
                    model2.TinkerPrice = Convert.ToDecimal(ja["price"].ToString());
                    model2.IsEnabled = true;// l.Count > 0 ? l[0].IsEnabled : false;
                    model2.CreateDate = DateTime.Now;
                    model2.CreateUserID = UserID;
                    model2.modifyuser = UserID;
                    model2.ts = DateTime.Now;
                    lll.Add(model2);
                }
                new Hi.BLL.BD_DisPriceInfo().Add(lll, Tran);
                Tran.Commit();
                return "cg";
            }
            else
            {
                Tran.Rollback();
                return "";
            }
        }
        catch (Exception)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
            return "";
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
    /// <summary>
    /// 查询代理商已存在的商品
    /// </summary>
    /// <returns></returns>
    public string BindGoods2(string compid, string disid)
    {
        //System.Collections.Generic.List<Hi.Model.BD_GoodsPrice> l = new Hi.BLL.BD_GoodsPrice().GetList("", "isnull(dr,0)=0 and compid=" + compid + " and disid=" + disid + " and goodsname is not null", "");

        string sql = string.Format(@" select p.ID, p.DisID, p.CompID, p.GoodsInfoID, p.GoodsName, p.BarCode, p.InfoValue, p.Unit, p.TinkerPrice, p.IsEnabled,i.SalePrice,g.Pic from BD_GoodsPrice as p,BD_GoodsInfo as i,BD_Goods as g
  where g.id=i.GoodsID and i.ID=p.GoodsInfoID and isnull(p.dr,0)=0 and p.compid=" + compid + " and p.disid=" + disid + " and p.goodsname is not null");
        System.Data.DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];
        return ConvertJson.ToJson(dt).ToString();
        //return ConvertJson.ToJson(Common.FillDataTable(l)).ToString();
    }
    /// <summary>
    /// 查询已存在的商品
    /// </summary>
    /// <returns></returns>
    public string BindGoods(string compid, string parentidlist, string type)
    {
        int disType = 0;
        string where = string.Empty;
        if (type == "getDisType")
        {
            disType = 1;
        }
        else
        {
            disType = 2;
        }
        string[] str = parentidlist.Split(',');
        if (str.Length == 3)
        {
            where = string.Format("One={0} and Two={1} and Three={2}", str[0], str[1], str[2]);
        }
        else if (str.Length == 2)
        {
            where = string.Format("One={0} and Two={1} and Three=0", str[0], str[1]);
        }
        else if (str.Length == 1)
        {
            where = string.Format("One={0}  and Two=0 and Three=0", str[0]);
        }
        string sql = string.Format(@"select goodsinfoid, g.GoodsName, i.BarCode ,infoValue as ValueInfo,g.Unit,i.TinkerPrice,a.isenabled,
 i.SalePrice,g.Pic from BD_DisPrice as a,BD_DisPriceInfo as b,BD_GoodsInfo as i,BD_Goods as g  
 where g.id=i.GoodsID and i.ID=b.GoodsInfoID and a.ID=b.DisPriceID and ISNULL(a.dr,0)=0 and " + where + " and type={0} and ISNULL(b.dr,0)=0 and a.CompID={1} and b.CompID={1}", disType, compid);
        System.Data.DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];
        return ConvertJson.ToJson(dt).ToString();
    }
    /// <summary>
    /// 查询goodsInfo
    /// </summary>
    /// <returns></returns>
    public string SelectGoods(string compid, string goodsinfoId)
    {
        System.Data.DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, SelectGoodsInfo.sql(compid, " and b.ID In(" + goodsinfoId + ")")).Tables[0];
        return ConvertJson.ToJson(dt).ToString();
    }
    /// <summary>
    /// 绑定代理商分类
    /// </summary>
    /// <returns></returns>
    public string BindDisType(string compId, string parentId)
    {
        System.Collections.Generic.List<Hi.Model.BD_DisType> l = new Hi.BLL.BD_DisType().GetList("", "isnull(dr,0)=0 and compId=" + compId + " and parentId=" + parentId, "");
        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(l).ToString();
    }
    /// <summary>
    /// 绑定代理商区域
    /// </summary>
    /// <returns></returns>
    public string BindDisArea(string compId, string parentId)
    {
        System.Collections.Generic.List<Hi.Model.BD_DisArea> l = new Hi.BLL.BD_DisArea().GetList("", "isnull(dr,0)=0 and companyID=" + compId + " and parentId=" + parentId, "");
        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(l).ToString();
    }
    /// <summary>
    /// 查询代理商
    /// </summary>
    /// <param name="compId"></param>
    /// <param name="dis"></param>
    /// <returns></returns>
    public string SelectDis(string compId, string dis)
    {
        string where = string.Empty;
        string top = string.Empty;
        if (dis == "")
        {
            where = "";
            top = " top 10 *";
        }
        else
        {
            string[] str = dis.Split(',');
            if (str[0] != "")
            {
                where += " and (disname like '%" + str[0] + "%' or discode like '%" + str[0] + "%')";
            }

            if (str[1] != "")
            {
                string typeid = Common.DisTypeId(Convert.ToInt32(str[1]), Convert.ToInt32(compId));//商品分类递归
                where += " and distypeid in(" + typeid + ")";
            }
        }
        System.Collections.Generic.List<Hi.Model.BD_Distributor> l = new Hi.BLL.BD_Distributor().GetList(top, "isnull(dr,0)=0 and compId=" + compId + where, "");
        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(l).ToString();
    }
    /// <summary>
    /// 推荐商品
    /// </summary>
    public string BindData(string compId)
    {
        string sql = "select a.id,goodsname,pic,a.compId from bd_goods as a, bd_goodsInfo as b where a.id=b.goodsId and isnull(a.dr,0)=0 and isnull(b.dr,0)=0  and a.isoffline=1 and b.isoffline=1 and isrecommended=2 and a.compid=" + compId + " and b.compId=" + compId + " group by a.id,goodsname,pic,a.compId order by a.id";
        string sql2 = "select distinct  a.id,goodsname,pic,a.compId from bd_goods as a, bd_goodsInfo as b where a.id=b.goodsId and isnull(a.dr,0)=0 and isnull(b.dr,0)=0  and a.isoffline=1 and b.isoffline=1  and isrecommended=2 and a.compid=" + compId + " and b.compId=" + compId + " order by a.id";
        MyPagination mypag = new MyPagination();
        string data = mypag.GetJson2(page, 4, sql, sql2);
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