using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Company_GoodsNew_selectgoods : System.Web.UI.Page
{
    public string page;
    public string action;
    //代理商ID
    public int DisID = 0;
    //企业ID
    public int CompId = 0;
    //订单下单数量是否取整
    public string Digits = "0";
    public string goodsInfoId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["CompId"] != null)
                CompId = Request["CompId"].ToString().ToInt(0);
            if (Request["index"] != null)
                this.hidIndex.Value = Request["index"].ToString();
            if (Request["goodsInfoId"] != null)
                this.hidgoodsInfoId.Value = Request["goodsInfoId"].ToString();
            if (Request["goodsInfoIdList"] != null)
                this.hidgoodsInfoIdList.Value = Request["goodsInfoIdList"].ToString();


            Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", CompId);
            //IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompId).ToInt(0);
            //tdIsInve.Visible = IsInve == 0;
            hidsDigits.Value = Digits;
            //hidIsInve.Value = IsInve.ToString();
            hidCompId.Value = CompId.ToString();
            hidDisId.Value = DisID.ToString();
            hidImgViewPath.Value = Common.GetPicBaseUrl(CompId.ToString());
            //databind();

            menu2.InnerHtml = GoodsCategory(CompId);
        }
    }

    /// <summary>
    /// 绑定商品分类
    /// </summary>
    /// <param name="CompId"></param>
    /// <returns></returns>
    public string GoodsCategory(int CompId)
    {
        StringBuilder sb = new StringBuilder();

        List<Hi.Model.BD_GoodsCategory> cl = new Hi.BLL.BD_GoodsCategory().GetList("", "CompID=" + CompId + " and  Isnull(dr,0)=0 and Isnull(IsEnabled,0)=1", "");

        if (cl != null && cl.Count > 0)
        {
            //商品分类一级
            List<Hi.Model.BD_GoodsCategory> cl1 = cl.FindAll(p => p.Deep == 1 && p.ParentId == 0);
            if (cl1 != null && cl1.Count > 0)
            {
                sb.Append("<div class=\"sorts\">");

                sb.AppendFormat("<div class=\"sorts1\"><i class=\"arrow2\"></i><a href=\"javascript:;\" class=\"a1\" tip=\"\">{0}</a></div>", "全部");
                sb.Append("</div>");
                foreach (var item in cl1)
                {
                    sb.Append("<div class=\"sorts\">");

                    sb.AppendFormat("<div class=\"sorts1\"><i class=\"arrow2\"></i><a href=\"javascript:;\" class=\"a1\" tip=\"{1}\">{0}</a></div>", item.CategoryName, item.ID);

                    //商品分类二级
                    List<Hi.Model.BD_GoodsCategory> cl2 = cl.FindAll(p => p.Deep == 2 && p.ParentId == item.ID);
                    if (cl2 != null && cl2.Count > 0)
                    {
                        sb.Append("<ul class=\"sorts2\" tipdis=\"no\" style=\"display:none;\">");
                        foreach (var item2 in cl2)
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<i class=\"arrow3\"></i><a href=\"javascript:;\" tip=\"{1}\">{0}</a>", item2.CategoryName, item2.ID);
                            //商品分类三级
                            List<Hi.Model.BD_GoodsCategory> cl3 = cl.FindAll(p => p.Deep == 3 && p.ParentId == item2.ID);

                            if (cl3 != null && cl3.Count > 0)
                            {
                                sb.Append("<ul class=\"sorts3\" tipdis=\"no\" style=\"display:none;\">");
                                foreach (var item3 in cl3)
                                {
                                    sb.AppendFormat("<li tip=\"{1}\"><a href=\"javascript:;\" tip=\"{1}\">{0}</a></li>", item3.CategoryName, item3.ID);
                                }
                                sb.Append("</ul>");
                            }
                            sb.Append("</li>");
                        }
                        sb.Append("</ul>");
                    }
                    sb.Append("</div>");
                }
            }
        }
        return sb.ToString();
    }
}