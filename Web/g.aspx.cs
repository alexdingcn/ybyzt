using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class g : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                int goodsId = Convert.ToInt32(Request["id"]);
                Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(goodsId);
                if (model == null)
                {
                    this.DivShow.InnerHtml = "<p style=\"padding-top: 20px; line-height: 40px; padding-left: 20px\">暂无数据</p>";
                }
                if (model.Details == "")
                {
                    this.DivShow.InnerHtml = "<p style=\"padding-top: 20px; line-height: 40px; padding-left: 20px\">暂无数据</p>";
                }
                else
                {
                    this.DivShow.InnerHtml = model.Details.Replace("<pre>", "<p>").Replace("</pre>", "</p>");
                }
            }
            catch (Exception es)
            {
                this.DivShow.InnerHtml = "<p style=\"padding-top: 20px; line-height: 40px; padding-left: 20px\">" + es.Message + "</p>";
            }

        }
    }
}