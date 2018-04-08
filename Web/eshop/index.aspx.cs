using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;
using System.Configuration;

public partial class EShop_index : LoginPageBase
{
    public string shopname = "";//店铺名称
    public string page = "1";
    public bool IsComp = false;
    public bool kfmoney = false;//是否开放价格
    public List<Hi.Model.BD_ShopImageList> ShopList;
    public string ShowJs = "";
    bool IsVersion = true;
    //public string qqtrueOrfalse = "";//店主是否维护QQ
    //public string qq = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (!IsVersion)
            //{
            //    Header.IE_warningShow = true;
            //    return;
            //}
            if (!string.IsNullOrWhiteSpace(Request["ScroTop"]))
            {
                HidScrollTop.Value = Request["ScroTop"];
            }
            //绑定商品数据
            BindData();
            //绑定商品分类
            BindGoodsCategory();
            //绑定新闻
            NewsBind();
            //绑定联系人
            PrincipalBind();
            //绑定推荐商品
            BindTopGoods();

            //操作日志统计开始
            Utils.WritePageLog(Request, "企业店铺：" + shopname);
            //操作日志统计结束
        }
    }

    protected override void OnInit(EventArgs e)
    {
        string BrowerName = Request.Browser.Browser;
        int BrowserVersion = Request.Browser.MajorVersion;
        //if (BrowerName == "IE" && BrowserVersion < 8)
        //{
        //    IsVersion = false;
        //    return;
        //}
        Header.NoGetUserList = true;
        List<Hi.Model.BD_Company> ComList = Header.ComList;
        //qqtrueOrfalse = string.IsNullOrWhiteSpace(ComList[0].QQ) ? "none" : "block";
        //qq = ComList[0].QQ;
        ViewState["Compid"] = ComList[0].ID;
        shopname = ComList[0].CompName;
        mKeyword.Content=!string.IsNullOrWhiteSpace(ComList[0].BrandInfo)? ComList[0].BrandInfo:string.IsNullOrWhiteSpace(ComList[0].ManageInfo) ? ComList[0].CompName : ComList[0].ManageInfo;
        if (!string.IsNullOrEmpty(ComList[0].FirstBanerImg))
        {
            Top_Banner.Visible = true;
            string[] PathArry = ComList[0].FirstBanerImg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string OuterHtml = "";
            int index = 0;
            foreach (string PathV in PathArry)
            {
                index++;
                OuterHtml += "<li id=\"Banner_" + index + "\" _src=\"url(" + (ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/") + "/" + PathV + ")\" ><a href=\"javascript:;\"></a></li>";
            }
            BannerUl.InnerHtml = OuterHtml;
        }
        ShopList = new Hi.BLL.BD_ShopImageList().GetList("GoodsUrl,ImageUrl", " dr=0 and Compid=" + ComList[0].ID + "", "id");
        if (ShopList.Count > 0)
        {
            if (ShopList.Where(T => T.ImageUrl != "").ToList().Count > 0)
            {
                Top_Advertisement.Visible = true;
            }
        }
        if (Session["UserModel"] is LoginModel)
        {
            LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;
            if (logUser.CompID == Request["Comid"].ToInt(0))
            {
                IsComp = true;
            }
            else
            {
                LiCollect.Visible = false;
            }
            if (logUser.TypeID == 1 || logUser.TypeID == 5)
            {
                ViewState["DisID"] = logUser.DisID;
                Href_NewMore.Attributes["href"] = "/Distributor/CompNewList.html";
                if (logUser.CompID == Request["Comid"].ToInt(0))
                {
                    ShowJs = " GoodsCoomon.GoodsAddCollect(" + true.ToString().ToLower() + "," + true.ToString().ToLower() + "," + ComList[0].ID + ")";
                }
                else
                {
                    ShowJs = " GoodsCoomon.GoodsAddCollect(" + false.ToString().ToLower() + "," + true.ToString().ToLower() + "," + ComList[0].ID + ")";
                }
            }
            else
            {
                Href_NewMore.Attributes["href"] = "/Company/SysManager/NewsList.aspx";
                ShowJs = " GoodsCoomon.GoodsAddCollect(" + IsComp.ToString().ToLower() + "," + true.ToString().ToLower() + "," + ComList[0].ID + ")";
                ViewState["DisID"] = "-1";
            }
        }
        else
        {
            LiCollect.Visible = false;
            ShowJs = " GoodsCoomon.GoodsAddCollect(" + false.ToString().ToLower() + "," + false.ToString().ToLower() + "," + ComList[0].ID + ")";
            ViewState["DisID"] = "-1";
            Href_NewMore.Attributes["href"] = "javascript:layerCommon.msg('请登录！',IconOption.哭脸);";
        }
    }

    protected void PagerList_PageChanged(object sender, EventArgs e)
    {
        page = Pager_List.CurrentPageIndex.ToString();
        BindData();
    }

    protected void Category_SearchClick(object sender, EventArgs e)
    {
        BindData();
    }

    public string GetBannerTopImg(string type, int TypeId)
    {
        if (ShopList.Count == 0)
        {
            return "";
        }
        string str = string.Empty;
        switch (type)
        {
            case "Img": str = ShopList.Count >= TypeId ? (ShopList[TypeId - 1].ImageUrl == "" ? "/images/Goods400x400.jpg" : ConfigurationManager.AppSettings["ImgViewPath"] + "CompFiveImg/" + ShopList[TypeId - 1].ImageUrl) : "/images/Goods400x400.jpg"; break;
            case "GoodsUrl": str = ShopList.Count >= TypeId ? (ShopList[TypeId - 1].GoodsUrl.ToString() == "" ? "#" : ShopList[TypeId - 1].GoodsUrl) : "javascript:;"; break;
        }
        return str;
    }


    //绑定商品数据
    public void BindData()
    {
        string sysNameWhere = string.Format(" CompID={0} and Name='是否店铺开放价格'", Request["Comid"].ToInt(0));
        List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", sysNameWhere, "");
        if (Sysl.Count>0)
        {
            kfmoney = Sysl[0].Value == "1" ? true : false;
        }

        string Sqlwhere = SearchWhere();
        int pageCount = 0;
        int Counts = 0;
        string JoinTableStr = "  BD_Goods left join (select  ROW_NUMBER() over(PARTITION BY GoodsID order by bp.createdate desc) rowid,  GoodsID id,bp.id  Proid,bp.Type,bp.ProType,bp.Discount,bpd.GoodsPrice from BD_PromotionDetail bpd  join BD_Promotion Bp on  bp.ID=bpd.ProID and bp.IsEnabled=1 and bp.dr=0 and Bp.CompID=" + ViewState["Compid"] + " and '" + DateTime.Now + "' between ProStartTime and  dateadd(D,1,ProEndTime))b   on BD_Goods.id=b.id and b.rowid=1  left join BD_DisCollect Bdc on BD_Goods.id=Bdc.GoodsID and Bdc.DisID=" + ViewState["DisID"] + "  and Bdc.dr=0 ";
        DataTable ListGoods = new Hi.BLL.BD_Goods().GetList(Pager_List.PageSize, Pager_List.CurrentPageIndex, " b.Type desc,Bdc.id desc ,Ispic desc, BD_Goods.isindex desc ,BD_Goods.CreateDate ", true, " (case when isnull(BD_Goods.pic2,'')='' then 0 else 1 end) Ispic,BD_Goods.CreateDate, BD_Goods.id,BD_Goods.GoodsName,b.Type ,BD_Goods.isindex,b.Proid,BD_Goods.Pic2,Bdc.id  BdcID,dbo.GetPMInfoMation(b.type,b.ProType,b.Discount,b.GoodsPrice) ProInfoMation,IsLS,LSPrice,SalePrice", JoinTableStr, Sqlwhere, out pageCount, out Counts, " ,(case when isnull(BD_Goods.pic2,'')='' then 0 else 1 end) Ispic ,b.Type ,Bdc.id , BD_Goods.CreateDate,BD_Goods.isindex ");

        Rpt_GoodsBig.DataSource = ListGoods;
        Rpt_GoodsBig.DataBind();
        Pager_List.RecordCount = Counts;
        Pager_List.TextBeforePageIndexBox = "<i class='tf2'>共" + Pager_List.PageCount + "页</i> <span class='tf2'>到第:</span>";
        page = Pager_List.CurrentPageIndex.ToString();
        if (Pager_List.CurrentPageIndex == 1)
        {
            PagePrev.Style.Add("background-color", "rgb(234,234,234) ");
            PagePrev.Attributes.Add("disbled", "");
        }
        else
        {
            PagePrev.Style.Remove("background-color");
            PagePrev.Attributes.Remove("disbled");
        }
        if (Pager_List.CurrentPageIndex == Pager_List.PageCount)
        {
            PageNext.Style.Add("background-color", "rgb(234,234,234) ");
            PageNext.Attributes.Add("disbled", "");
        }
        else
        {
            PageNext.Style.Remove("background-color");
            PageNext.Attributes.Remove("disbled");
        }
    }

    public void BindGoodsCategory()
    {
        List<Hi.Model.BD_GoodsCategory> ClassList = new Hi.BLL.BD_GoodsCategory().GetList(" id,CategoryName ", " dr=0 and IsEnabled=1 and Parentid=0 and Compid='" + ViewState["Compid"] + "' ", " createdate ");
        string.Join(",",ClassList.Select(p=>p.ID));

        if (ClassList.Count > 0)
        {
            Hi.Model.BD_GoodsCategory DefaultClass = new Hi.Model.BD_GoodsCategory();
            DefaultClass.CategoryName = "全 部";
            DefaultClass.ID = 0;
            DefaultClass.CompID = ClassList[0].CompID;
            DefaultClass.ParentId = 0;
            DefaultClass.IsEnabled = 1;
            ClassList.Insert(0, DefaultClass);
        }
        Rpt_GoodsClass.DataSource = ClassList;
        Rpt_GoodsClass.DataBind();
    }

    //查询条件
    public string SearchWhere()
    {
        string SqlQuery = " and BD_Goods.Compid='" + ViewState["Compid"] + "' and BD_Goods.dr=0 and IsOffline=1 and bd_goods.id not in(select GoodsID from bd_goodsareas where DisID=" + ViewState["DisID"] + " and dr=0 ) and BD_Goods.IsRecommended>0 ";
        if (!string.IsNullOrEmpty(Request["GoodsName"]))
        {
            SqlQuery += " and  BD_Goods.GoodsName like '%" + Request["GoodsName"].Trim() + "%' ";
        }
        if (CK_Pro.Checked && CK_Collect.Checked)
        {
            SqlQuery += " and ( (b.type=1 or b.type=0) or  Bdc.id!='' )";
        }
        else
        {
            if (CK_Pro.Checked)
            {
                SqlQuery += " and  (b.type=1 or b.type=0) ";
            }
            if (CK_Collect.Checked)
            {
                SqlQuery += " and Bdc.id!='' ";
            }
        }
        if (!string.IsNullOrWhiteSpace(HidCategoryCheckId.Value))
        {
            int Id;
            string[] CategoryArry = HidCategoryCheckId.Value.Trim().Split(new char[] { ',' });
            HidCategoryCheckId.Value = "";
            foreach (string ID in CategoryArry)
            {
                if (int.TryParse(ID, out Id))
                {
                    HidCategoryCheckId.Value += HidCategoryCheckId.Value == "" ? Id.ToString() : "," + Id;
                }
            }
            if (!string.IsNullOrWhiteSpace(HidCategoryCheckId.Value))
            {
                if (CategoryArry.Length == 1)
                {
                    SqlQuery += @" and BD_Goods.CategoryID in(
           select id from  BD_GoodsCategory where code like ''+ (select code from BD_GoodsCategory  where ID=" + HidCategoryCheckId.Value + @") +'%'
                )";
                }
                else if (CategoryArry.Length > 1)
                {
                    SqlQuery += @" and BD_Goods.CategoryID in(" + HidCategoryCheckId.Value + ")";
                }
            }
        }
        return SqlQuery;
    }

    protected void btnProMotion_Click(object sender, EventArgs e)
    {
        if (CK_Pro.Checked)
        {
            CK_Pro.Checked = false;
        }
        else
        {
            CK_Pro.Checked = true;
        }
        BindData();
    }

    protected void btnCollect_Click(object sender, EventArgs e)
    {
        if (CK_Collect.Checked)
        {
            CK_Collect.Checked = false;
        }
        else
        {
            CK_Collect.Checked = true;
        }
        BindData();
    }

    protected void PagePrev_Click(object sender, EventArgs e)
    {
        if (Pager_List.CurrentPageIndex > 1)
        {
            Pager_List.CurrentPageIndex -= 1;
        }
    }

    protected void PageNext_Click(object sender, EventArgs e)
    {
        if (Pager_List.CurrentPageIndex < Pager_List.PageCount)
        {
            Pager_List.CurrentPageIndex += 1;
        }
    }

    //绑定新闻
    public void NewsBind()
    {
        if (ShopList.Count == 0)
        {
            return;
        }
        string whereStr = "isnull(dr,0)=0 and IsEnabled=1 and Compid=" + Request["Comid"] + " ";
        if (HttpContext.Current.Session["UserModel"] == null)
        {
            //未登录
            whereStr += " and NewsType in (1,2,3,5)";
        }

        #region 新闻公告
        List<Hi.Model.BD_CompNews> LNew = new Hi.BLL.BD_CompNews().GetList("NewsTitle,isnull(ShowType,0) as ShowType,ID,CompID", whereStr, " istop desc,createdate desc");

        string Html = "";
        if (LNew.Count > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                if (LNew.Count > i)
                {
                    LNew[i].ShowType = LNew[i].ShowType == null ? "" : LNew[i].ShowType;
                    if (LNew[i] != null)
                        Html += "<li><a " + (LNew[i].ShowType.IndexOf("2") >= 0 ? "style='color:red;'" : "") + " title='" + LNew[i].NewsTitle + "'  target=\"_blank\" href=\"/eshop/NewsInfo_" + LNew[i].ID + "_" + LNew[i].CompID + "_3.html\" >" + (LNew[i].NewsTitle.Length > 14 ? LNew[i].NewsTitle.Substring(0, 14) + "..." : LNew[i].NewsTitle) + "</a></li>";
                }
                else
                {
                    Html += "<li>&nbsp;</li>";
                }
            }
            NewsList.InnerHtml = Html;
        }
        else
        {
            NewsList.InnerHtml = "<li>暂无公告</li><li></li><li></li><li></li>";
        }
        #endregion
    }


    //绑定联系人
    public void PrincipalBind()
    {
        LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;

        if (logUser != null)
        {
            List<Hi.Model.BD_Company> ComList = Header.ComList;
            if (ComList.Count > 0)
            {
                lblPrincipal.InnerHtml = "<i>联系人：</i>" + ComList[0].Principal;
                lblPhone.InnerHtml = "<i>电　话：</i>" + ComList[0].Phone;
                lblAddress.InnerHtml = "<i>地　址：</i>" + ComList[0].Address;
                lbllogin.Visible = false;
                //qqtrueOrfalse = string.IsNullOrWhiteSpace(ComList[0].QQ) ? "none" : "block";
                //qq = ComList[0].QQ;
            }
        }
        else
        {
            lblPrincipal.InnerHtml = "<i>联系人：</i>" +"***";
            lblPhone.InnerHtml = "<i>电　话：</i>" + "***";
            lblAddress.InnerHtml = "<i>地　址：</i>" + "***";
            lbllogin.InnerHtml = "<i  style=\" color:Red;\">请先登录</i>";
        }
    }


    /// <summary>
    /// //绑定推荐商品
    /// </summary>
    private void BindTopGoods()
    {
        if (ShopList.Count == 0)
        {
            return ;
        }
        StringBuilder html = new StringBuilder();
        List<Hi.Model.BD_ShopGoodsList> l = new Hi.BLL.BD_ShopGoodsList().GetList("Title,GoodsID,ShowName", "isnull(dr,0)=0 and compId=" + ViewState["Compid"], "title");
        if (l.Count > 0)
        {
            string title = "";
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].Title != title)
                {
                    title = l[i].Title;
                    if (i != 0)
                    {
                        html.Append("</ul><div class=\"line\"></div>");
                    }
                    html.Append("<div class=\"bt\" style=\"cursor: pointer;\">" + l[i].Title + "</div><ul class=\"list\">");
                    html.Append("<li><a target=\"blank\" href=\"/e" + l[i].GoodsID + "_" + ViewState["Compid"] + ".html\">" + l[i].ShowName + "</a></li>");
                }
                else
                {
                    html.Append("<li><a target=\"blank\" href=\"/e" + l[i].GoodsID + "_" + ViewState["Compid"] + ".html\">" + l[i].ShowName + "</a></li>");
                }
            }
        }

        this.lblHtml.Text = html.ToString();
    }


    public  string GetPicURL()
    {
        return Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/";
    }


}