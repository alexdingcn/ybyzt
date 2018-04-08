using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Company_newOrder_optDis : System.Web.UI.Page
{
    //厂商ID
    public int CompID = 1028;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["compid"]))
                CompID = (Request["compid"] + "").ToInt(0);

            this.hidCompID.Value = CompID.ToString();
            this.menu2.InnerHtml = grtdistype(CompID);
        }
    }

    public string grtdistype(int CompID)
    {
        StringBuilder sb = new StringBuilder();

        List<Hi.Model.BD_DisType> tl = new Hi.BLL.BD_DisType().GetList("", " CompID=" + CompID + " and isnull(IsEnabled,0)=0 and isnull(dr,0)=0", "");

        if (tl != null && tl.Count > 0)
        {
            //代理商分类一级
            List<Hi.Model.BD_DisType> tl1 = tl.FindAll(p => p.ParentId == 0);

            if (tl1 != null && tl1.Count > 0)
            {
                sb.Append("<div class=\"sorts\">");

                sb.AppendFormat("<div class=\"sorts1\"><i class=\"arrow2\"></i><a href=\"javascript:;\" class=\"a1\" tip=\"\">{0}</a></div>", "全部");
                sb.Append("</div>");

                foreach (var item in tl1)
                {
                    sb.Append("<div class=\"sorts\">");
                    sb.AppendFormat("<div class=\"sorts1\"><i class=\"arrow2\"></i><a href=\"javascript:;\" class=\"a1\" tip=\"{1}\">{0}</a></div>", item.TypeName, item.ID);

                    //代理商分类二级
                    List<Hi.Model.BD_DisType> tl2 = tl.FindAll(p => p.ParentId == item.ID);
                    if (tl2 != null && tl2.Count > 0)
                    {
                        sb.Append("<ul class=\"sorts2\" tipdis=\"no\" style=\"display:none;\">");
                        foreach (var item2 in tl2)
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<i class=\"arrow3\"></i><a href=\"javascript:;\" class=\"a1\"  tip=\"{1}\">{0}</a>", item2.TypeName, item2.ID);
                            //代理商分类三级
                            List<Hi.Model.BD_DisType> tl3 = tl.FindAll(p => p.ParentId == item2.ID);

                            if (tl3 != null && tl3.Count > 0)
                            {
                                sb.Append("<ul class=\"sorts3\" tipdis=\"no\" style=\"display:none;\">");
                                foreach (var item3 in tl3)
                                {
                                    sb.AppendFormat("<li><a href=\"javascript:;\" tip=\"{1}\">{0}</a></li>", item3.TypeName, item3.ID);
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