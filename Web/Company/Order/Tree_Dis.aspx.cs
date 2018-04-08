using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Order_Tree_Dis : System.Web.UI.Page
{
    public List<Hi.Model.BD_Distributor> Dislist = new List<Hi.Model.BD_Distributor>();

    public int Id = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Id"] != null)
        {
            Id = Convert.ToInt32(Request.QueryString["Id"]);
        }

        GetDis(Id);//查询代理商
        //string jss = "var zNodes = [{ id: 0, pId: 0, name: \"代理商\", open: true ,isParent:true },";
        string jss = "var zNodes = [";
        int i = 0;

        //Dislist.FindAll(p => p.CompID == Id)
        foreach (Hi.Model.BD_Distributor dis in Dislist)
        {
            i++;
            jss += "{ id: " + dis.ID + ", pId: 0, name: \"" + dis.DisName + "\" },";
        }
        jss += "]";
        //jss += "]}]";

        ClientScript.RegisterStartupScript(this.GetType(), "ClientScript", "<script>" + jss + "</script>");
    }

    /// <summary>
    /// 查询代理商
    /// </summary>
    public void GetDis(int Id)
    {
        List<Hi.Model.BD_Distributor> li = new Hi.BLL.BD_Distributor().GetList("", " CompID=" + Id + "and isnull(AuditState,0)=2 and isnull(IsEnabled,0)=1 and isnull(dr,0)=0", "Id desc");
        if (li != null)
        {
            if (li.Count > 0)
            {
                foreach (Hi.Model.BD_Distributor item in li)
                {
                    Dislist.Add(item);
                }
            }
        }
    }
}