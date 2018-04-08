<%@ WebHandler Language="C#" Class="GetDataSource" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using LitJson;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class GetDataSource : loginInfoMation, IHttpHandler {

    string compid = "";
    public void ProcessRequest(HttpContext context)
    {
        string Requst_REFERER = context.Request.ServerVariables["HTTP_REFERER"];
        if (string.IsNullOrWhiteSpace(Requst_REFERER))
        {
            context.Response.Write("禁止地址栏访问处理程序");
            context.Response.End();
        }
        context.Response.ContentType = "text/plain";
        string action = context.Request["action"];
        string value = context.Request["Value"];
        compid = context.Request["CompKey"];
        string ReturnMsg = "";
        switch (action)
        {
            case "GetCompNameS": ReturnMsg=GetCompNames(context, value); break;
            case "GetGoodsClass": ReturnMsg = GetGoodsClass(context); break;
            case "GetGoodsAttribute": ReturnMsg = GetGoodsAttribute(context); break;
            case "GetGoodsInfoIdPrice": ReturnMsg = GetGoodsInfoIdPrice(context); break;
            case "GetGoodsHot": ReturnMsg = GetGoodsHot(context); break;
            case "GetCompBanner": ReturnMsg = GetCompBanner(context); break;
            case "GetRecommend": ReturnMsg = GetRecommend(context); break;
            case "GetFiveImg": ReturnMsg = GetFiveImg(context); break;
            case "GetCompContact": ReturnMsg = GetCompContact(context); break;
        }
        context.Response.Write(ReturnMsg);
        context.Response.End();
    }

    /// <summary>
    /// 继承Json序列化类  指定序列化字段   add by kb
    /// </summary>
    public class CusTomPropsContractResolver : DefaultContractResolver
    {
        //[JsonProperty(PropertyName = "")]
        string[] props = null;

        bool retain;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段  false：表示props是要排除的字段</param>
        public CusTomPropsContractResolver(string[] props, bool retain = true)
        {
            //指定要序列化属性的清单
            this.props = props;

            this.retain = retain;
        }

        protected override IList<JsonProperty> CreateProperties(Type type,

        Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
            //只保留清单有列出的属性
            return list.Where(p =>
            {
                if (retain)
                {
                    return props.Contains(p.PropertyName);
                }
                else
                {
                    return !props.Contains(p.PropertyName);
                }
            }).ToList();
        }
    }

    public string GetCompContact(HttpContext context)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        new loginInfoMation().LoadData();
        if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
        {
            if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                Ctype = 1;
            }
            if (Ctype == 1)
            {
                List<Hi.Model.BD_Company> ComList = new Hi.BLL.BD_Company().GetList("Principal,Phone,Address", " id='" + CompID + "' and dr=0 and AuditState=2 ", "");
                if (ComList.Count > 0)
                {
                    msg.Result = true;
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.ContractResolver = new CusTomPropsContractResolver(new string[] { "Principal", "Phone", "Address" });
                    msg.Code = JsonConvert.SerializeObject(ComList, Formatting.Indented, jsetting);
                }
                else
                {
                    msg.Msg = "企业异常无法获取联系人数据";
                }
            }
            else
            {
                msg.Msg = "当前用户非企业用户，无法获取数据";
            }
        }
        else
        {
            msg.Msg = "用户未登陆";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }


    public string GetCompBanner(HttpContext context)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        new loginInfoMation().LoadData();
        if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
        {
            if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                Ctype = 1;
            }
            if (Ctype == 1)
            {
                List<Hi.Model.BD_Company> ComList = new Hi.BLL.BD_Company().GetList("FirstBanerImg", " id='" + CompID + "' and dr=0 and AuditState=2 ", "");
                if (ComList.Count > 0)
                {
                    msg.Result = true;
                    msg.Code = ComList[0].FirstBanerImg;
                }
                else
                {
                    msg.Msg = "企业异常无法获取Banner图数据";
                }
            }
            else
            {
                msg.Msg = "当前用户非企业用户，无法获取Banner图数据";
            }
        }
        else
        {
            msg.Msg = "用户未登陆";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string GetFiveImg(HttpContext context)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        new loginInfoMation().LoadData();
        if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
        {
            if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                Ctype = 1;
            }
            if (Ctype == 1)
            {
                List<Hi.Model.BD_ShopImageList> ImgList = new Hi.BLL.BD_ShopImageList().GetList("ID,ImageUrl,GoodsUrl,GoodsID,ImageName", "isnull(dr,0)=0 and compId=" + CompID + " ", "");
                msg.Result = true;
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.ContractResolver = new CusTomPropsContractResolver(new string[] { "ID", "ImageUrl", "GoodsUrl", "GoodsID", "ImageName" });
                msg.Code = JsonConvert.SerializeObject(ImgList, Formatting.Indented, jsetting);
            }
            else
            {
                msg.Msg = "当前用户非企业用户，无法获取主推商品数据";
            }
        }
        else
        {
            msg.Msg = "用户未登陆";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }


    public string GetRecommend(HttpContext context)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        try
        {
            new loginInfoMation().LoadData();
            if (UserModel != null ||  context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
                {
                    CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                    Ctype = 1;
                }
                if (Ctype == 1)
                {
                    List<Hi.Model.BD_ShopGoodsList> ShopList = new Hi.BLL.BD_ShopGoodsList().GetList("ID,Title,ShowName,GoodsID,(select top 1 GoodsName from BD_goods where id=BD_ShopGoodsList.GoodsID) GoodsName", "isnull(dr,0)=0 and compId=" + CompID, "title");
                    msg.Result = true;
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.ContractResolver = new CusTomPropsContractResolver(new string[] { "ID", "Title", "ShowName", "GoodsID", "GoodsName" });
                    msg.Code = JsonConvert.SerializeObject(ShopList, Formatting.Indented, jsetting);
                }
                else
                {
                    msg.Msg = "当前用户非企业用户,无法获取店铺推荐数据";
                }
            }
            else
            {
                msg.Msg = "用户未登陆";
            }
        }
        catch (Exception ex)
        {
            msg.Msg = "服务器异常，数据加载失败";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }



    public string GetCompNames(HttpContext context, string value)
    {
        if (value != "")
        {
            //只做模糊搜索
            System.Collections.Generic.List<Hi.Model.BD_Company> DisM = new Hi.BLL.BD_Company().GetList("CompName", " isnull(dr,0)=0  and AuditState=2  and FirstShow=1 and IsEnabled=1 " + (value == "" ? "" : " and CompName like '%" + value + "%'") + "", "");
            if (DisM.Count > 0)
            {
                string json = "[";
                foreach (Hi.Model.BD_Company model in DisM)
                {
                    json += "{\"name\":\"" + model.CompName + "\"},";
                }
                if (json[json.Length - 1] == ',')
                {
                    json = json.Substring(0, json.Length - 1);
                }
                json += "]";
                return json;
            }
        }
        return "";
    }


    /// <summary>
    /// 获取商品分类信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string GetGoodsClass(HttpContext context)
    {
        Information.ResultMsg<Hi.Model.BD_GoodsCategory> msg = new Information.ResultMsg<Hi.Model.BD_GoodsCategory>();
        string CategoryId = context.Request["CategoryId"];
        string Compid = context.Request["Compid"];
        try
        {
            if (!string.IsNullOrWhiteSpace(CategoryId) && !string.IsNullOrWhiteSpace(Compid))
            {
                List<Hi.Model.BD_GoodsCategory> list = new Hi.BLL.BD_GoodsCategory().GetList(" id,CategoryName", " ParentID='" + CategoryId + "' and dr=0 and IsEnabled=1 and Compid='" + Compid + "' ", " createdate ");
                msg.Result = true;
                msg.ListSource = list;
                msg.Msg = "Success";
            }
            else
            {
                msg.Msg = "参数异常，请重试。";
                msg.Error = true;
            }
        }
        catch (Exception ex)
        {
            msg.Msg = "获取分类失败，服务器异常";
            msg.Error = true;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string GetGoodsAttribute(HttpContext context)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        string GoodsArryId = context.Request["GoodIdArrr"];
        string Compid = context.Request["Compid"];
        try
        {
            List<Information.GoodsAttribute> Arrt =  Information.GetGoodsAttribute(Compid.ToInt(0), GoodsArryId);
            if (Arrt != null)
            {
                string[] GoodsArry = GoodsArryId.Split(new char[] { ',' });
                SortedDictionary<string, List<Information.GoodsAttribute>> sorted = new SortedDictionary<string, List<Information.GoodsAttribute>>();
                foreach (string Goosid in GoodsArry)
                {
                    if (!sorted.ContainsKey(Goosid))
                        sorted.Add(Goosid, Arrt.Where(T => T.GoodsID == Goosid.ToInt(0)).ToList<Information.GoodsAttribute>());
                }
                msg.Result = true;
                msg.Code = new JavaScriptSerializer().Serialize(sorted);
                msg.Msg = "获取商品规格属性成功";
            }
            msg.Result = true;

        }
        catch (Exception ex)
        {
            msg.Msg = "获取商品规格属性失败。";
            msg.Code = "获取商品规格属性失败，服务器异常（" + ex.Message + "）";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }


    public string GetGoodsInfoIdPrice(HttpContext context)
    {
        DateTime time = DateTime.Now;
        Information.ResultMsg msg = new Information.ResultMsg();
        new loginInfoMation().LoadData();
        string Compid = context.Request["Compid"];
        string GoodsInfoArry = context.Request["GoodsInfoArry"];
        string IsAll = context.Request["IsAll"];
        if (UserModel != null)
        {
            if (Compid.ToInt(0) == CompID)
            {
                if (!string.IsNullOrEmpty(GoodsInfoArry))
                {
                    try
                    {
                        string QeuryWhere = string.Empty;
                        JsonData Json = JsonMapper.ToObject(GoodsInfoArry);
                        if (Json["GoodsInfoArry"].Count > 0)
                        {
                            foreach (JsonData Jdata in Json["GoodsInfoArry"])
                            {
                                if (string.IsNullOrEmpty(QeuryWhere))
                                {
                                    QeuryWhere += "(goodsid=" + Jdata["GoodsID"] + " " + (!string.IsNullOrWhiteSpace(Jdata["AttrValue"].ToString()) ? "and valueinfo='" + Jdata["AttrValue"] + "'" : "") + ")";
                                    continue;
                                }
                                QeuryWhere += " or (goodsid=" + Jdata["GoodsID"] + " " + (!string.IsNullOrWhiteSpace(Jdata["AttrValue"].ToString()) ? "and valueinfo='" + Jdata["AttrValue"] + "'" : "") + ")";
                            }
                        }
                        List<Information.GoodInfoIdPrice> Arrt = new List<Information.GoodInfoIdPrice>();
                        Arrt = Information.GetGoodsInfoPrice(QeuryWhere, CompID, DisID);
                        if (Arrt.Count > 0)
                        {
                            SortedDictionary<string, List<Information.GoodInfoIdPrice>> sorted = new SortedDictionary<string, List<Information.GoodInfoIdPrice>>();
                            foreach (Information.GoodInfoIdPrice GoodsInfo in Arrt)
                            {
                                if (!sorted.ContainsKey(GoodsInfo.GoodsID.ToString()))
                                    sorted.Add(GoodsInfo.GoodsID.ToString(), Arrt.Where(T => T.GoodsID == GoodsInfo.GoodsID).ToList());
                            }
                            msg.Result = true;
                            msg.Code = new JavaScriptSerializer().Serialize(sorted);
                            msg.Msg = "获取商品价格成功";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(GoodsInfoArry) && IsAll != "1")
                            {
                                msg.Error = true;
                                msg.Msg = "该属性商品已下架。";
                            }
                            else
                            {
                                msg.Result = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        msg.Msg = "获取商品价格失败。";
                        msg.Error = true;
                        msg.Code = "获取商品价格失败，服务器异常。";
                    }
                }
            }
            else
            {
                msg.Msg = "当前用户不是该企业代理商，无法获取价格";
            }
        }
        else
        {
            msg.Msg = "用户未登陆，无法获取价格";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string GetGoodsHot(HttpContext context)
    {
        string Msg = "";
        int pageCount = 0;
        int Counts = 0;
        int Compid = context.Request["Comid"].ToInt(0);
        if (Compid > 0)
        {
            int CurrentPageIndex = context.Request["PageIndex"].ToInt(1);
            new loginInfoMation().LoadData();
            string Sqlwhere = "and BD_Goods.Compid='" + Compid + "' and IsOffline=1 and BD_Goods.dr=0 and (IsRecommended=2  ) and bd_goods.id not in(select GoodsID from bd_goodsareas where DisID=" + DisID + " and dr=0) ";
            string JoinTableStr = "  BD_Goods left join (select ROW_NUMBER() over(PARTITION BY GoodsID order by bp.createdate desc) rowid,  GoodsID id,bp.id  Proid,bp.Type,bp.ProType,bp.Discount,bpd.GoodsPrice from BD_PromotionDetail bpd  join BD_Promotion Bp on  bp.ID=bpd.ProID and bp.IsEnabled=1 and bp.dr=0 and Bp.CompID=" + Compid + " and '" + DateTime.Now + "' between ProStartTime and  dateadd(D,1,ProEndTime))b   on BD_Goods.id=b.id and b.rowid=1  left join BD_DisCollect Bdc on BD_Goods.id=Bdc.GoodsID and Bdc.DisID=" + DisID + "  and Bdc.dr=0 ";
            DataTable ListGoods = new Hi.BLL.BD_Goods().GetList(5, CurrentPageIndex, " BD_Goods.IsRecommended desc, b.Type desc,BD_Goods.pic2 desc, BD_Goods.CreateDate  ", true, " BD_Goods.id,BD_Goods.GoodsName,b.Type ,BD_Goods.CreateDate,b.Proid,BD_Goods.Pic2,Bdc.id  BdcID,IsRecommended,dbo.GetPMInfoMation(b.type,b.ProType,b.Discount,b.GoodsPrice) ProInfoMation", JoinTableStr, Sqlwhere, out pageCount, out Counts);
            Msg = DataTableToJson("Source", ListGoods, CurrentPageIndex, pageCount);
        }
        return Msg;
    }

    /// <summary>
    /// //将Datatable转换为json格式数据并给table命名
    /// </summary>
    /// <param name="jsonName"></param>
    /// <param name="dt">table名称</param>
    /// <returns></returns>
    public static string DataTableToJson(string jsonName, DataTable dt,int PageIndex,int PageCount)
    {
        StringBuilder Json = new StringBuilder();
        Json.Append("{\"" + jsonName + "\":[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString().ToLower().Replace("\\", "\\\\") + "\":\"" + dt.Rows[i][j].ToString().Replace("\\", "\\\\") + "\"");
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("],\"PageIndex\":" + PageIndex + ",\"PageCount\":" + PageCount + "}");
        return Json.ToString();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}