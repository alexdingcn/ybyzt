using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hi.Model;
using Hi.BLL;
using System.Data;

public partial class error : System.Web.UI.Page
{
    public string strMsg = "页面已走丢";//提示信息
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object obj = Request.QueryString["errortype"];
            if (obj != null)
            {
                if (obj.ToString() == "1")
                {
                    strMsg = "页面来源不正确";
                }
                else if (obj.ToString() == "2")
                {
                    strMsg = "用户未登录";
                }
                else if (obj.ToString() == "3")
                {
                    strMsg = "页面提交含有危险字符";
                }
                else if (obj.ToString() == "4")
                {
                    strMsg = "不明的访问来源";
                }
                else if (obj.ToString() == "5")
                {
                    strMsg = "不明的访问来源";
                }
            }
            //绑定数据
            Bind();
        }
    }

    private void Bind()
    {
        List<Hi.Model.BD_Company> LComp = null;
        if (HttpRuntime.Cache.Get("hotShop_RPT") == null)
        {
            LComp = new Hi.BLL.BD_Company().GetList("top 6 CompName,ID,CompLogo,ShortName,ShopLogo", " dr=0 and AuditState=2  and FirstShow=1  and IsEnabled=1 ", " SortIndex desc,id ");
            HttpRuntime.Cache.Insert("hotShop_RPT", LComp, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        else
        {
            LComp = HttpRuntime.Cache["hotShop_RPT"] as List<Hi.Model.BD_Company>;
        }
        hotShop_RPT.DataSource = LComp;
        hotShop_RPT.DataBind();

        List<Hi.Model.BD_Goods> LGood = null;
        if (HttpRuntime.Cache.Get("hotGood_RPT") == null)
        {
            string SqlQuery = " isnull(Pic2,'')<>'' and BD_Goods.dr=0 and IsOffline=1 ";
            LGood = new Hi.BLL.BD_Goods().GetList(" top 5 * ", SqlQuery, " BD_Goods.ID ");
            HttpRuntime.Cache.Insert("hotGood_RPT", LGood, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        else
        {
            LGood = HttpRuntime.Cache["hotGood_RPT"] as List<Hi.Model.BD_Goods>;
        }
        hotGoods_RPT.DataSource = LGood;
        hotGoods_RPT.DataBind();
    }
}