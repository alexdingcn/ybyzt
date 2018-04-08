using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WindowLogin : System.Web.UI.Page
{
    public string WriteHTML = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string GetAction = Request["Action"] + "";
            if (Request["ShowRegis"] == "show")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Key", "<script>$('.A_PhoneRegis,.A_defaultRegis').show(); </script>");
            }
            if (GetAction == "AcccountSwich")
            {
                defaultlogin.Visible = false;
                Phonelogin.Visible = false;
                AccountCheck Acheck = Session["UserModel"] as AccountCheck;
                if (Acheck != null)
                {
                    LoginBox2.Attributes["class"] = "";
                    if (Acheck.IsAccountCheck)
                    {
                        if (!Acheck.IsPhoneLogin)
                        {
                            LoginHTML(Acheck.UsersID,1);
                        }
                        else
                        {
                            List<Hi.Model.SYS_Users> ListUsers = Acheck.ListUser;
                            string Userid = string.Join(",", ListUsers.Select(T => T.ID));
                            LoginHTML(Userid, 0);//0手机登录
                        }
                    }
                    else
                    {
                        Response.Write("请通过用户验证后，再进行帐号切换。");
                        Response.End();
                    }
                }
                else
                {
                    Response.Write("请通过用户验证后，再进行帐号切换。");
                    Response.End();
                }
            }
            else
            {
                Phonelogin.Attributes.Add("style", "margin-top:30px;display: none;");
                defaultlogin.Attributes.Add("style", "margin-top:30px;");
            }
        }
    }
    public void LoginHTML(string Userid, int type)
    {
        if (!string.IsNullOrWhiteSpace(Userid))
        {
            WriteHTML = "  <div class=\"pageLogin\" id=\"AccountSwitch\" >	<div class=\"role-cur\"> ";
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("id,compid,Disid,Ctype,Utype", " isnull(dr,0)=0 and IsAudit=2 and IsEnabled=1 and userid  in(" + Userid + ")", "");
            if (ListCompUser.Count > 0)
            {
                string Compid = string.Join(",", ListCompUser.Where(T => T.CType == 1 && (T.UType == 3 || T.UType == 4)).ToList().Select(T => T.CompID));
                if (Compid != "")
                {
                    List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("ID,CompName", " isnull(dr,0)=0 and AuditState=2 and IsEnabled=1 and ID in(" + Compid + ")", "createdate");
                    if (ListComp.Count > 0)
                    {
                        WriteHTML += (ListComp.Count > 0 ? "<div class=\"title\"><i class=\"hx-i\"></i>厂商</div> <div class=\"list\">" : "");
                        string CompHTML = "";
                        foreach (Hi.Model.BD_Company model in ListComp)
                        {
                            CompHTML += "<a href=\"javascript:;\" type=" + type + "  title='" + model.CompName + "' tip='" + Common.DesEncrypt(ListCompUser.Where(T => T.CType == 1 && T.CompID == model.ID).ToList()[0].ID.ToString(), Common.EncryptKey) + "'>" + model.CompName + "</a>";
                        }
                        WriteHTML += CompHTML;
                        WriteHTML += (ListComp.Count > 0 ? "</div>" : "");
                    }
                }
                ListCompUser = ListCompUser.Where(T => T.CType == 2 && (T.UType == 1 || T.UType == 5)).ToList();
                string DisID = string.Join(",", ListCompUser.Select(T => T.DisID));
                Compid = string.Join(",", ListCompUser.ToList().Select(T => T.CompID));
                if (DisID != "" && Compid != "")
                {
                    List<Hi.Model.BD_Distributor> ListDis = new Hi.BLL.BD_Distributor().GetList("ID,DisName,CompID", " isnull(dr,0)=0 and AuditState=2 and IsEnabled=1 and ID in(" + DisID + ")", "createdate");
                    List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("ID,CompName", " isnull(dr,0)=0  and ID in(" + Compid + ")", "createdate");
                    if (ListDis.Count > 0 && ListComp.Count > 0)
                    {
                        WriteHTML += (ListDis.Count > 0 ? "<div class=\"title\"><i class=\"jx-i\"></i>代理商</div> <div class=\"list\">" : "");
                        string CompHTML = "";
                        foreach (Hi.Model.BD_Distributor model in ListDis)
                        {
                            string Name = "";
                            if (ListComp.Where(T => T.ID == model.CompID).ToList().Count > 0)
                            {
                                Name = ListComp.Where(T => T.ID == model.CompID).ToList()[0].CompName;
                            }
                            CompHTML += "<a href=\"javascript:;\" title='" + (Name) + "' type=" + type + "  tip='" + Common.DesEncrypt(ListCompUser.Where(T => T.CType == 2 && T.DisID == model.ID).ToList()[0].ID.ToString(), Common.EncryptKey) + "'>" + (Name) + "</a>";
                        }
                        WriteHTML += CompHTML;
                        WriteHTML += (ListDis.Count > 0 ? "</div>" : "");
                    }
                }
                //Compid = string.Join(",", ListCompUser.Where(T => T.CType == 2 && (T.UType == 1 || T.UType == 5)).ToList().Select(T => T.CompID));
                //if (Compid != "")
                //{
                //    List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("ID,CompName", " isnull(dr,0)=0 and AuditState=2 and IsEnabled=1 and ID in(" + Compid + ")", "createdate");
                //    WriteHTML += (ListComp.Count > 0 ? "<div class=\"title\"><i class=\"hx-i\"></i>代理商</div> <div class=\"list\">" : "");
                //    string CompHTML = "";
                //    foreach (Hi.Model.BD_Company model in ListComp)
                //    {
                //        if (string.IsNullOrEmpty(CompHTML))
                //        {
                //            CompHTML += "<a href=\"javascript:;\" title='" + model.CompName + "'  tip='" + Common.DesEncrypt(ListCompUser.Where(T => T.CType == 2 && T.CompID == model.ID).ToList()[0].ID.ToString(), Common.EncryptKey) + "'>" + model.CompName + "</a>";
                //        }
                //        else
                //        {
                //            CompHTML += "<i>|</i><a href=\"javascript:;\" title='" + model.CompName + "' tip='" + Common.DesEncrypt(ListCompUser.Where(T => T.CType == 2 && T.CompID == model.ID).ToList()[0].ID.ToString(), Common.EncryptKey) + "' >" + model.CompName + "</a>";
                //        }
                //    }
                //    WriteHTML += CompHTML;
                //    WriteHTML += (ListComp.Count > 0 ? "</div>" : "");
                //}


            }
            else
            {
                WriteHTML += "用户没有绑定任何企业。";
            }
            WriteHTML += "	</div><div class=\"bg\"></div></div>";
        }
    }
}