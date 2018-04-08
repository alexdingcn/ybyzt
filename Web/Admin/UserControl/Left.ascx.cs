using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserControl_Left : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Hi.Model.SYS_AdminUser model = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser;
        if (model != null)
        {
            if (model.UserType == 1)
            {
                //系统管理
                this.xtgl.Visible = true;      
                this.xtyhjqx.Visible = true;
                //this.ssgjz.Visible = true;
                this.xwfb.Visible = true;      
                this.hyflgl.Visible = true;
                //this.spflgl.Visible = true;
                this.kfly.Visible = true;
                //this.ptskzh.Visible = true;
                this.dlrz.Visible = true;
                this.ywrz.Visible = true;
                this.yyda.Visible = true;
                //this.gmfwyh.Visible = true;

                //机构管理
                this.jggl.Visible = true;
                this.jgwh.Visible = true;
                this.jgyhwh.Visible = true;
                this.jgywywh.Visible = true;

                //厂商管理
                this.hxqygl.Visible = true;
                this.hxqyxz.Visible = true;
                this.hxqysh.Visible = true;
                this.hxqywh.Visible = true;
                this.qyyhwh.Visible = true;
                this.hxqyzx.Visible = false;
                this.hxqyzxsh.Visible = true;

                //代理商管理
                this.jxsgl.Visible = true;
                this.jxscx.Visible = true;
                //this.jxsglycx.Visible = true;

                //商品查询
                this.spgls.Visible = true;
                this.spcx.Visible = true;

                //订单查询
                this.ddcxs.Visible = true;
                this.ddcx.Visible = true;

                //报表查询
                this.bbcxs.Visible = true;
                this.xsddcx.Visible = true;
                this.jxsxssj.Visible = true;
                this.spxssj.Visible = true;
                this.xssjyfbd.Visible = true;
                this.yszk.Visible = true;
                this.zd.Visible = true;
                this.dpdjlcx.Visible = true;
                this.ipdjlcx.Visible = true;
            }
            else
            {
                string sql = "select rf.* from SYS_RoleSysFun rf join SYS_AdminUser u on u.RoleID=rf.RoleID where u.ID=" + model.ID;
                DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    switch (dr["FunCode"].ToString())
                    {
                        //系统管理
                        case "3010":
                            this.xtgl.Visible = true;
                            this.xtyhjqx.Visible = true;
                            break;
                        case "3011":
                            this.xtgl.Visible = true;
                            this.xwfb.Visible = true;
                            break;
                        case "3012":
                            this.xtgl.Visible = true;
                            this.hyflgl.Visible = true;
                            break;
                        case "3013":
                            this.xtgl.Visible = true;
                            //this.spflgl.Visible = true;
                            break;
                        case "3015":
                            this.xtgl.Visible = true;
                            //this.ptskzh.Visible = true;
                            break;
                        case "3016":
                            this.xtgl.Visible = true;
                            this.dlrz.Visible = true;
                            break;
                        case "3017":
                            this.xtgl.Visible = true;
                            this.ywrz.Visible = true;
                            break;
                        case "3018":
                            this.xtgl.Visible = true;
                            this.yyda.Visible = true;
                            break;

                        //机构管理
                        case "3110":
                            this.jggl.Visible = true;
                            this.jgwh.Visible = true;
                            break;
                        case "3111":
                            this.jggl.Visible = true;
                            this.jgyhwh.Visible = true;
                            break;
                        case "3112":
                            this.jggl.Visible = true;
                            this.jgywywh.Visible = true;
                            break;

                        //厂商管理
                        case "3210":
                            this.hxqygl.Visible = true;
                            this.hxqyxz.Visible = true;
                            break;
                        case "3211":
                            this.hxqygl.Visible = true;
                            this.hxqysh.Visible = true;
                            break;
                        case "3212":
                            this.hxqygl.Visible = true;
                            this.hxqywh.Visible = true;
                            break;
                        case "3213":
                            this.hxqygl.Visible = true;
                            this.qyyhwh.Visible = true;
                            break;
                        case "3214":
                            this.hxqygl.Visible = true;
                            this.hxqyzx.Visible = true;
                            break;
                        case "3215":
                            this.hxqygl.Visible = true;
                            this.hxqyzxsh.Visible = true;
                            break;

                        //代理商管理
                        case "3310":
                            this.jxsgl.Visible = true;
                            this.jxscx.Visible = true;
                            break;
                        case "3311":
                            this.jxsgl.Visible = true;
                            //this.jxsglycx.Visible = true;
                            break;

                        //商品查询
                        case "3410":
                            this.spgls.Visible = true;
                            this.spcx.Visible = true;
                            break;

                        //订单查询
                        case "3511":
                            this.ddcxs.Visible = true;
                            this.ddcx.Visible = true;
                            break;

                        //报表查询
                        case "3611":
                            this.bbcxs.Visible = true;
                            this.xsddcx.Visible = true;
                            break;
                        case "3612":
                            this.bbcxs.Visible = true;
                            this.jxsxssj.Visible = true;
                            break;
                        case "3613":
                            this.bbcxs.Visible = true;
                            this.spxssj.Visible = true;
                            break;
                        case "3614":
                            this.bbcxs.Visible = true;
                            this.xssjyfbd.Visible = true;
                            break;
                        case "3615":
                            this.bbcxs.Visible = true;
                            this.yszk.Visible = true;
                            break;
                        case "3616":
                            this.bbcxs.Visible = true;
                            this.zd.Visible = true;
                            break;
                        case "3617":
                            this.bbcxs.Visible = true;
                            this.dpdjlcx.Visible = true;
                            break;
                        case "3618":
                            this.bbcxs.Visible = true;
                            this.ipdjlcx.Visible = true;
                            break;
                    }
                }

            }
        }
        else
        {
            Response.Write("用户不存在！");
            Response.End();
        }
    }
}