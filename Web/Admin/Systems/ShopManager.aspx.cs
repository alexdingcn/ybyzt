using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;

public partial class Company_ShopManager_ShopManager : AdminPageBase
{
    public string BindShowJson = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //绑定推荐商品
            BindTopGoods();
            //绑定联系人
            PrincipalBind();
            //绑定新闻
            NewsBind();
            Hid_UserCompKey.Value = Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);
        }
        BindShowJson = "var Showoption = { \"btnBanner\": \".banner-edit\",\"btnRecommend\": \".adMenu-edit\",\"btnFiveImg\": \".adImg-edit\",\"btnContact\": \".contact-edit\" }; \n        var RequesType = { Query:0,Submit:1,Action:2  };";
    }



    public string GetBannerTopImg(string type, int TypeId)
    {

        List<Hi.Model.BD_ShopImageList> ShopList = ShopList = new Hi.BLL.BD_ShopImageList().GetList("GoodsUrl,ImageUrl", " dr=0 and Compid=" + KeyID + "", "id");
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

     /// <summary>
    /// //绑定推荐商品
    /// </summary>
    private void BindTopGoods()
    {
        StringBuilder html = new StringBuilder();
        List<Hi.Model.BD_ShopGoodsList> l = new Hi.BLL.BD_ShopGoodsList().GetList("Title,GoodsID,ShowName", "isnull(dr,0)=0 and compId=" + KeyID, "title ");
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


    public void PrincipalBind()
    {
        List<Hi.Model.BD_Company> ComList = new Hi.BLL.BD_Company().GetList("", " dr=0  and id=" + KeyID + "", "");
        if (ComList.Count > 0)
        {
            string[] PathArry = ComList[0].FirstBanerImg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string OuterHtml = "";
            int index = 0;
            foreach (string PathV in PathArry)
            {
                index++;
                OuterHtml += "<li id=\"Banner_" + index + "\" _src=\"url(" + ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + PathV + ")\" ><a href=\"javascript:;\"></a></li>";
            }
            BannerUl.InnerHtml = OuterHtml;
            lblPrincipal.InnerHtml = "<i>联系人：</i>" + ComList[0].Principal;
            lblPhone.InnerHtml = "<i>电　话：</i>" + ComList[0].Phone;
            lblAddress.InnerHtml = "<i>地　址：</i>" + ComList[0].Address;
        }
    }


    //绑定新闻
    public void NewsBind()
    {
        string whereStr = "isnull(dr,0)=0 and IsEnabled=1 and Compid=" + KeyID + " ";
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
                        Html += "<li><a " + (LNew[i].ShowType.IndexOf("2") >= 0 ? "style='color:red;'" : "") + " title='" + LNew[i].NewsTitle + "'  target=\"_blank\" href=\"/EShop/NewsInfo.aspx?ID=" + LNew[i].ID + "&comid=" + LNew[i].CompID + "&Type=3\">" + (LNew[i].NewsTitle.Length > 14 ? LNew[i].NewsTitle.Substring(0, 14) + "..." : LNew[i].NewsTitle) + "</a></li>";
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
}