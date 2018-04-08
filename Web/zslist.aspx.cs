using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using System.Text;
using DBUtility;

public partial class zslist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string action = (Request["action"] + "").Trim();

            if (action == "getCategoryResource") {
                Response.Write(getCategoryResource((Request["cateId"] + "").Trim()));
                Response.End();
            }
            else if (action == "getGoodsResource")
            {
                Response.Write(GetGoodsResource());
                Response.End();
            }
            else if (action == "applyCooperation")
            {
                string ycCompID = Request["ycCompID"] + "";
                Response.Write(applyCooperation(ycCompID));
                Response.End();
            }

            loadData();
        }
    }


    //查询数据
    public void loadData()
    {
        string builder = string.Empty;

        LoginModel user = Session["UserModel"] as LoginModel;
        if (user != null)
        {
            builder = @" and yc.id in ( select fca.CMID from YZT_FCArea fca left join BD_Distributor dis on (fca.Province+fca.City+fca.Area=dis.Province+dis.City+dis.Area or fca.Province+fca.City=dis.Province+dis.City or fca.Province=dis.Province) and dis.IsEnabled=1 where 1=1 and dis.ID=" + user.DisID + " and fca.CompID=" + user.CompID + " union select fcd.CMID from YZT_FCDis fcd where fcd.DisID=" + user.DisID + "and fcd.CompID=" + user.CompID + " union select ID from YZT_CMerchants where type=1 and dr=0 and isnull(IsEnabled,0)=1)";
        }
        else
        {
            builder = @" and yc.Type=1 and isnull(yc.IsEnabled,0)=1";
        }

        //查询企业数据
        List<Hi.Model.BD_Company> comps = new Hi.BLL.BD_Company().GetList("id,CompName", " dr=0 and IsEnabled=1 and AuditState=2 and exists(select 1 from YZT_CMerchants  yc  where yc.InvalidDate >=getdate() and yc.CompID = BD_Company.id " + builder + ")", " AuditState");
        Rp_Production.DataSource = comps;
        Rp_Production.DataBind();

        string gtypeids = string.Empty;
        string sql = "select g.CategoryID,gt.* from  YZT_CMerchants yc  left join BD_GoodsInfo info   on info.ID=yc.GoodsID left join BD_Goods g on g.ID=info.GoodsID left join SYS_GType gt on gt.ID=g.CategoryID where isnuLL(gt.dr,0)=0 and isnuLL(gt.IsEnabled,0)=1 " + builder;

        DataTable dt = SqlHelper.GetTable(SqlHelper.LocalSqlServer, sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                if (item["deep"].ToString() == "1")
                {
                    gtypeids += gtypeids != "" ? "," + item["ID"].ToString() : item["ID"].ToString();
                }
                else
                {
                    string cid = getCategoryDet(item["ParentId"].ToString());
                    gtypeids += gtypeids != "" ? "," + cid : cid;
                }
            }
        }

        //查询商品一级分类数据
        List<Hi.Model.SYS_GType> gtypes = new Hi.BLL.SYS_GType().GetList("id,TypeName", " dr=0 and IsEnabled=1 and deep=1 and id in(" + gtypeids + ")", "createdate");

        Rp_Category1.DataSource = gtypes;
        Rp_Category1.DataBind();
    }

    //查询下级分类数据
    public string getCategoryResource(string cateId)
    {
        string data = string.Empty;

        string builder = string.Empty;

        LoginModel user = Session["UserModel"] as LoginModel;
        if (user != null)
        {
            builder = @" and yc.id in ( select fca.CMID from YZT_FCArea fca left join BD_Distributor dis on (fca.Province+fca.City+fca.Area=dis.Province+dis.City+dis.Area or fca.Province+fca.City=dis.Province+dis.City or fca.Province=dis.Province) and dis.IsEnabled=1 where 1=1 and dis.ID=" + user.DisID + " and fca.CompID=" + user.CompID + " union select fcd.CMID from YZT_FCDis fcd where fcd.DisID=" + user.DisID + "and fcd.CompID=" + user.CompID + " union select ID from YZT_CMerchants where type=1 and dr=0 and isnull(IsEnabled,0)=1)";
        }
        else
        {
            builder = @" and yc.Type=1 and isnull(yc.IsEnabled,0)=1";
        }

        string gtypeids = string.Empty;
        string sql = "select g.CategoryID,gt.* from  YZT_CMerchants yc  left join BD_GoodsInfo info   on info.ID=yc.GoodsID left join BD_Goods g on g.ID=info.GoodsID left join SYS_GType gt on gt.ID=g.CategoryID where isnuLL(gt.dr,0)=0 and isnuLL(gt.IsEnabled,0)=1 and ParentId=" + cateId + builder;

        //查询商品一级分类数据
        //List<Hi.Model.SYS_GType> gtypes = new Hi.BLL.SYS_GType().GetList("id,TypeName", " dr=0 and IsEnabled=1 and ParentId=" + cateId + "", "createdate");

        List<Hi.Model.SYS_GType> gtypes = new List<Hi.Model.SYS_GType>();
        DataTable dt = SqlHelper.GetTable(SqlHelper.LocalSqlServer, sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                Hi.Model.SYS_GType gt = new Hi.Model.SYS_GType();
                gt.ID = item["ID"].ToString().ToInt(0);
                gt.ParentId = item["ParentId"].ToString().ToInt(0);
                gt.FullCode = item["FullCode"].ToString();
                gt.TypeName = item["TypeName"].ToString();
                gtypes.Add(gt);
            }
        }

        data = new JavaScriptSerializer().Serialize(gtypes);
        return data;
    }

    string cid = string.Empty;
    /// <summary>
    /// 递归得到第一级分类ID
    /// </summary>
    /// <param name="cateId"></param>
    /// <returns></returns>
    public string getCategoryDet(string cateId)
    {
        List<Hi.Model.SYS_GType> gtypes = new Hi.BLL.SYS_GType().GetList("", "dr=0 and IsEnabled=1 and ID=" + cateId, "");
        if (gtypes != null && gtypes.Count > 0)
        {
            if (gtypes[0].Deep != 1)
            {
                cid = string.Empty;
                getCategoryDet(gtypes[0].ParentId.ToString());
            }
            else
            {
                cid = gtypes[0].ID.ToString();
            }
        }
        return cid;
    }

    //查询商品数据
    public string GetGoodsResource()
    {
        string data = string.Empty;
        int pSzie = (Request["pagesize"] + "").ToInt(1);
        int pageIndex = (Request["page"] + "").ToInt(1);
        int DataCount = 0;
        DataTable goods = BLL.Common.GetListByAjaxPage(pSzie, pageIndex, out DataCount, "YZT_CMerchants yc join BD_GoodsInfo info on yc.goodsID=info.ID join BD_Goods bg on info.GoodsID=bg.ID", "yc.id ycId,bg.ID,bg.GoodsName,bg.Pic,yc.ForceDate,yc.CompID ycCompID,bg.CompID,bg.ShowName,bg.Title,bg.Details,CONVERT(varchar(100), yc.CreateDate, 23) CreateDate2,yc.CreateDate,yc.Remark", getWhere(), "yc.CreateDate desc, yc.ForceDate desc,bg.ID");

        if (goods != null && goods.Rows.Count > 0)
        {
            foreach (DataRow item in goods.Rows)
            {
                item["Pic"] = SelectGoodsInfo.GetGoodsPic(item["Pic"].ToString());

                DateTime dt = Convert.ToDateTime(item["CreateDate"]);
                TimeSpan dtnum = DateTime.Now.Subtract(dt);
                item["ShowName"] = dtnum.Days.ToString();
            }
        }

        MyPaginationPage page = new MyPaginationPage();
        data = page.CreateJsonToTable(DataCount, pSzie, goods);
        return data;
    }


    public string getWhere()
    {
        StringBuilder builder = new StringBuilder(" bg.dr=0 and yc.dr=0 and bg.IsEnabled=1 and yc.IsEnabled=1 and (ISNULL(yc.ForceDate,0)=0 or yc.ForceDate <= getdate() ) and (ISNULL(yc.InvalidDate,0)=0 or yc.InvalidDate>=getdate()) ");

        LoginModel user = Session["UserModel"] as LoginModel;
        if (user != null)
        {
            builder.Append(" and yc.id in ( select fca.CMID from YZT_FCArea fca left join BD_Distributor dis on (fca.Province+fca.City+fca.Area=dis.Province+dis.City+dis.Area or fca.Province+fca.City=dis.Province+dis.City or fca.Province=dis.Province) and dis.IsEnabled=1 where 1=1 and dis.ID=" + user.DisID + " and fca.CompID=" + user.CompID + " union select fcd.CMID from YZT_FCDis fcd where fcd.DisID=" + user.DisID + "and fcd.CompID=" + user.CompID + " union select ID from YZT_CMerchants where type=1 and dr=0 and isnull(IsEnabled,0)=1)");
        }
        else
        {
            builder.Append(" and yc.Type=1 and isnull(yc.IsEnabled,0)=1");
        }

        int compId = (Request["compId"] + "").Trim().ToInt(0);
        int cate1 = (Request["cate1"] + "").Trim().ToInt(0);
        int cate2 = (Request["cate2"] + "").Trim().ToInt(0);
        int cate3 = (Request["cate3"] + "").Trim().ToInt(0);
        if (compId > 0)
        {
            builder.Append(" and bg.Compid=" + compId + "");
        }

        if (cate3 > 0)
        {
            builder.Append(" and bg.CategoryID=" + cate3 + "");
        }
        else if (cate2 > 0)
        {
            builder.Append(@" and bg.CategoryID in(
           select id from  SYS_GType where FullCode like ''+ (select FullCode from SYS_GType  where ID=" + cate2 + @") +'%'
                )");
        }
        else if (cate1 > 0)
        {
            builder.Append(@" and bg.CategoryID in(
           select id from  SYS_GType where FullCode like ''+ (select FullCode from SYS_GType  where ID=" + cate1 + @") +'%'
                )");
        }

        return builder.ToString();

    }

    public string applyCooperation(string ycCompID)
    {
        Information.ResultMsg msg = new Information.ResultMsg();

        if (HttpContext.Current.Session["UserModel"] is LoginModel)
        {
            LoginModel uModel = HttpContext.Current.Session["UserModel"] as LoginModel;

            if (uModel.CompID == ycCompID.ToInt(0))
            {
                msg.Msg = "无法申请成为自己的代理商";
            }
            else
            {
                msg.Result = true;
            }

            //if (uModel.Ctype == 2 || uModel.Ctype == 0)
            //{
            //    msg.Result = true;
            //}
            //else
            //{
            //    msg.Msg = "当前用户不是代理商，无法申请合作";
            //}
        }
        else
        {
            msg.Code = "Login";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }


}