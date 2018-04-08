

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;

public partial class Distributor_DeliveryList : DisPageBase
{
    public string userphone = "";
    public string Principal="";
    public string Phone = "";
    public string ID = "";
    public string phones = "";
    public Hi.Model.BD_Distributor dis = new Hi.Model.BD_Distributor();
    public string coutn = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet ds = new Hi.BLL.BD_DisAddr().GetModel(this.DisID.ToString());
        coutn = (ds.Tables[0].Rows.Count + 1).ToString();
        rptdelivery.DataSource = ds;
        rptdelivery.DataBind();
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if(dis!=null){
            Principal = dis.Principal;
            Phone = dis.Phone;
            ID = dis.ID.ToString();
        }     
        string phone= new Hi.BLL.SYS_Users().GetModel(this.UserID).Phone;
        phones = AESHelper.Encrypt_php(phone);
        userphone = phone.Substring(0,3)+"*****"+ phone.Substring(phone.Length-4);
        if (!string.IsNullOrEmpty(Request["type"]) && Request["type"].ToString() == "add")
        {
            Add_Addre();
        }
    }

    public void Add_Addre()
    {
        int disid = int.Parse(Request["disid"].ToString());
        dis = new Hi.BLL.BD_Distributor().GetModel(disid);
        if (dis != null)
        {
            dis.Address = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string str = js.Serialize(dis);
            Response.Clear();
            Response.Write(str);
            Response.End();
        }
    }

    protected void A_Defat(object sender, EventArgs e)
    {
        HtmlAnchor adefat = sender as HtmlAnchor;
        string defatid = adefat.Attributes["defatid"];
        if (!string.IsNullOrEmpty(defatid))
        {
            Hi.Model.BD_DisAddr disaddr = new Hi.BLL.BD_DisAddr().GetModel(int.Parse(defatid));
            if (disaddr != null)
            {
                if (new Hi.BLL.BD_DisAddr().UpdateS(this.DisID.ToString()))
                {
                    disaddr.IsDefault = 1;
                    disaddr.ts = DateTime.Now;
                    disaddr.modifyuser = this.UserID;
                    if (new Hi.BLL.BD_DisAddr().Update(disaddr))
                    {
                        JScript.AlertMethod(this, "设置成功", JScript.IconOption.正确, "function (){ location.replace('" + ("DeliveryList.aspx") + "'); }");
                    }
                }
            }
        }
    }

    protected void A_DLT(object sender, EventArgs e)
    {
        HtmlAnchor adefat = sender as HtmlAnchor;
        string dltID = adefat.Attributes["deleteid"];
        if (!string.IsNullOrEmpty(dltID))
        {
            Hi.Model.BD_DisAddr disaddr = new Hi.BLL.BD_DisAddr().GetModel(int.Parse(dltID));
            if (disaddr != null)
            {
                disaddr.dr = 1;
                disaddr.ts = DateTime.Now;
                disaddr.modifyuser = this.UserID;
                if (new Hi.BLL.BD_DisAddr().Update(disaddr))
                {
                    JScript.AlertMethod(this, "删除成功", JScript.IconOption.正确, "function (){ location.replace('" + ("DeliveryList.aspx") + "'); }");
                }
            }
        }
    }
}