using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Order_Tree_Addr : System.Web.UI.Page
{
    private int DisId=0;

    List<Hi.Model.BD_DisAddr> Addrli = new List<Hi.Model.BD_DisAddr>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Id"] != null)
        {
            DisId = Convert.ToInt32(Request.QueryString["Id"]);
        }

        DisAddr(DisId);

        //string jss = "var zNodes =[ { id: 0, pId: 0, name: \"收货地址\", open: true ,isParent:true },";

        string jss = "var zNodes = [";
        int i = 0;
        //Dislist.FindAll(p => p.CompID == null)
        foreach (Hi.Model.BD_DisAddr Addr in Addrli)
        {
            i++;
            //jss += "{ id:" + Addr.ID + ", name: " + Addr.Address + "}";
            //string Address = Addr.Province + Addr.City + Addr.Area + Addr.Address;
            //string Address = Addr.City + Addr.Area + Addr.Address;
            //jss += "{ id: " + Addr.ID + ", pId: 0, name: \"" + Common.MySubstring(Address, 20, "...") + "\" },";
            string Address = Addr.Address;
            jss += "{ id: " + Addr.ID + ", pId: 0, name: \"" + Address + "\" },";
        }
        //jss += "]}]";
        jss += "]";

        ClientScript.RegisterStartupScript(this.GetType(), "ClientScript", "<script>" + jss + "</script>");
    }

    /// <summary>
    /// 代理商收货地址
    /// </summary>
    /// <param name="DisId">代理商Id</param>
    public void DisAddr(int DisId)
    {
        List<Hi.Model.BD_DisAddr> li = new Hi.BLL.BD_DisAddr().GetList("", " isnull(dr,0)=0 and DisId=" + DisId, "");

        if (li != null) {
            if (li.Count > 0)
            {
                foreach (Hi.Model.BD_DisAddr item in li)
                {
                    Addrli.Add(item);
                }
            }
        }
    }

}