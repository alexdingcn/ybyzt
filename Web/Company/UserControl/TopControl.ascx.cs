using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Configuration;

public partial class Company_UserControl_TopControl : System.Web.UI.UserControl
{
    public string CompName = string.Empty;
    public string UserName = string.Empty;
    public string compid = string.Empty;
    public string swUhtml = string.Empty;//账户切换
    public bool bol = false;
    public string logo = string.Empty;
    public bool IsHiddenLeft { get; set; }
    public string ShowID
    {
        get { return Showid.Value.Trim(); }
        set { Showid.Value = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["lefttype"] != null)
        {
            //Request["lefttype"]：1 不显示top 、 2 不显示left 3 top、left都不显示
            string lefttype = Request["lefttype"] + "";
            if (lefttype == "1")
            {
                //不显示top
                this.topBar.Visible = false;
            }
            else if (lefttype == "2")
            {
                //不显示left
                this.left1.Visible = false;
                IsHiddenLeft = true;
            }
            else if (lefttype == "3")
            {
                //top、left 都不显示 
                this.topBar.Visible = false;
                this.left1.Visible = false;
                IsHiddenLeft = true;
            }
        }

        if (HttpContext.Current.Session["UserModel"] is LoginModel)
        {
            LoginModel loguser = HttpContext.Current.Session["UserModel"] as LoginModel;

            if (loguser != null)
            {
                Hi.Model.BD_Company comModel=new Hi.BLL.BD_Company().GetModel(loguser.CompID);
                string lg = comModel != null ? comModel.CompLogo : "";
                logo = lg == "" ? "../Config/image/logo8.jpg" : System.Configuration.ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + lg;
                CompName = loguser.CompName;//  Common.GetCompValue(loguser.CompID, "CompName").ToString();
                //获取用户名
                //Hi.Model.SYS_Users sysuser = new Hi.BLL.SYS_Users().GetModel(loguser.UserID);
                UserName = loguser.TrueName == "" ? loguser.UserName : loguser.TrueName;
                compid = loguser.CompID.ToString();
                if (loguser.Url != null)
                {
                    //罗汉
                    if (loguser.Url.IndexOf("lhhome") != -1)
                    {
                        bol = true;
                    }
                    if (loguser.Url.IndexOf("lh") != -1)
                    {
                        bol = true;
                    }
                    //酒隆仓
                    if (loguser.Url.IndexOf("jlc") != -1)
                    {
                        bol = true;
                    }
                }
            }
            this.left1.ShowID = Showid.Value;
        }
        if (!IsPostBack)
        {
            // databind();
        }
        if (!IsHiddenLeft)
        {
            left1.Visible = true;
        }


    }

}