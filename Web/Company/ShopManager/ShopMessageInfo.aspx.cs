using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_ShopManager_ShopMessageInfo : System.Web.UI.Page
{
    Hi.BLL.BD_ShopMessage OrderBll = new Hi.BLL.BD_ShopMessage();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request.QueryString["ID"] != "")
        {
            Bind();
        }
    }
    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        Hi.Model.BD_ShopMessage model = OrderBll.GetModel(int.Parse(Request.QueryString["ID"].ToString()));

        this.lblname.InnerText = model.Name;
        this.lblphone.InnerText = model.Phone;
        this.lblremark.InnerText = model.Remark;
        this.lblread.InnerText = model.IsRead == 0 ? "未读" : "已读";
        this.lblDate.InnerText = model.CreateDate.ToString("yyyy-MM-dd");
        model.IsRead = 1;
        model.ID = int.Parse(Request.QueryString["ID"].ToString());
        bool bol = new Hi.BLL.BD_ShopMessage().Update(model);
    }
    //删除
    protected void btnDelate_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["ID"] != "")
            {
                Hi.Model.BD_ShopMessage model = OrderBll.GetModel(int.Parse(Request.QueryString["ID"].ToString()));
                model.dr = 1;
                model.ID = int.Parse(Request.QueryString["ID"].ToString());
                bool bol = new Hi.BLL.BD_ShopMessage().Update(model);
                if (bol)
                {
                    Response.Redirect("ShopMessage.aspx");
                }
            }
        }
        catch (Exception)
        {
            Response.Redirect("ShopMessage.aspx");
        }
    }
}