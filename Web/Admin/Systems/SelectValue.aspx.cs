using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_SelectValue : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<Hi.Model.SYS_SysName> SysNameList = new Hi.BLL.SYS_SysName().GetList("", " name='搜索热词' and  dr=0 ", "");
            if (SysNameList.Count > 0)
            {
                txt_SelectValue.Value = SysNameList[0].Value;
            }
        }
        
    }

    protected void btnSubMit_Click(object sender, EventArgs e)
    {   
        
        if (!string.IsNullOrWhiteSpace(txt_SelectValue.Value))
        {
            int count = txt_SelectValue.Value.Split(',').Length;
            if (txt_SelectValue.Value.Split(',').Length > 6)
            {
                JScript.AlertMsg(this, "最多设置6个热词");
            }
            else
            {
                List<Hi.Model.SYS_SysName> SysNameList = new Hi.BLL.SYS_SysName().GetList("", " name='搜索热词' and  dr=0 ", "");
                if (SysNameList.Count > 0)
                {
                    Hi.Model.SYS_SysName sysname = SysNameList[0];
                    sysname.Value = txt_SelectValue.Value;
                    sysname.ts = DateTime.Now;
                    sysname.modifyuser = this.UserID;
                    if (new Hi.BLL.SYS_SysName().Update(sysname))
                        JScript.AlertMsg(this, "设置成功");
                    else
                        JScript.AlertMsg(this, "失败");
                }
                else
                {
                    Hi.Model.SYS_SysName sysname = new Hi.Model.SYS_SysName();
                    sysname.CompID = 0;
                    sysname.Code = "0001";
                    sysname.Name = "搜索热词";
                    sysname.Value = txt_SelectValue.Value;
                    sysname.ts = DateTime.Now;
                    sysname.dr = 0;
                    sysname.modifyuser = this.UserID;
                    if (new Hi.BLL.SYS_SysName().Add(sysname) > 0)
                        JScript.AlertMsg(this, "设置成功");
                    else
                        JScript.AlertMsg(this, "失败");
                }
            }
           

        }
       ClientScript.RegisterStartupScript(this.GetType(), "MSG", "<script>window.parent.location.href=window.parent.location.href; </script>");


    }
}