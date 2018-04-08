using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using LitJson;

public partial class Company_SysManager_PayWxSettings : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string action = Request["action"] + "";
            if (action.Equals("sett"))
            {                
                Settings();
               
            }
            bind();
        }
    }

    /// <summary>
    /// 绑定信息
    /// </summary>
    public void bind()
    {
        //查询该企业的设置
        List<Hi.Model.Pay_PayWxandAli> Sysl = new Hi.BLL.Pay_PayWxandAli().GetList("", " CompID=" + KeyID, "");
        if (Sysl.Count > 0)
        {

            if (Sysl[0].wx_Isno == "1")
                this.chisno.Checked = true;

            this.appid.Value = Convert.ToString(Sysl[0].wx_appid);
            this.appsecret.Value = Convert.ToString(Sysl[0].wx_appsechet);
            this.mchid.Value = Convert.ToString(Sysl[0].wx_mchid);
            this.key.Value = Convert.ToString(Sysl[0].wx_key);

        }
        else //默认值显示
        {
            this.appid.Value ="";
            this.appsecret.Value = "";
            this.mchid.Value ="";
            this.key.Value ="";
        }

        hid_CompID.Value = Convert.ToString(KeyID);
       
   }

    /// <summary>
    /// 保存系统设置信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Settings()
    {
        string bind = string.Empty;
        string  strSql = string.Empty;

        SqlTransaction TranSaction = null;
        SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
        Connection.Open();
        TranSaction = Connection.BeginTransaction();

        try
        {
            //企业ID
            int CompId = Convert.ToInt32(Request["hid_compID"]);

            string chkisno = Convert.ToString(Request["chkisno"]);
            string appid = Convert.ToString(Request["appid"]);
            string appsecret = Convert.ToString(Request["appsecret"]);
            string mchid = Convert.ToString(Request["mchid"]);
            string key = Convert.ToString(Request["key"]);

            bool fal = false;

            //查询该企业的设置
            List<Hi.Model.Pay_PayWxandAli> Sysl = new Hi.BLL.Pay_PayWxandAli().GetList("", " CompID=" + CompId, "");

            //判断企业的是否有设置
            if (Sysl.Count > 0)
            {
                Hi.Model.Pay_PayWxandAli paywxandaliModel = Sysl[0];
                paywxandaliModel.wx_Isno = chkisno;      
                paywxandaliModel.wx_appid = appid;
                paywxandaliModel.wx_appsechet = appsecret;
                paywxandaliModel.wx_mchid = mchid;
                paywxandaliModel.wx_key = key;

                fal=  new Hi.BLL.Pay_PayWxandAli().Update(paywxandaliModel);
                if (fal)
                {
                    string strmessage = string.Format("ApppID(应用ID)：{0};AppSecrect(应用秘钥)：{1};Mchid(商户号)：{2};APPKey(API秘钥)：{3};是否启用：{4}", appid, appsecret, mchid, key,chkisno);
                    Utils.AddSysBusinessLog(CompId, "paymentbank", paywxandaliModel.ID.ToString(), "微信收款帐号修改", strmessage, this.UserID.ToString());
                }
            }
            else
            {
                Hi.Model.Pay_PayWxandAli paywxandaliModel =new Hi.Model.Pay_PayWxandAli();
                paywxandaliModel.wx_Isno = chkisno;
                paywxandaliModel.wx_appid = appid;
                paywxandaliModel.wx_appsechet = appsecret;
                paywxandaliModel.wx_mchid = mchid;
                paywxandaliModel.wx_key = key;
                paywxandaliModel.CompID = CompId;
               int num = new Hi.BLL.Pay_PayWxandAli().Add(paywxandaliModel);
               if (num > 0)
                {
                    fal = true;
                    string strmessage = string.Format("ApppID(应用ID)：{0};AppSecrect(应用秘钥)：{1};Mchid(商户号)：{2};APPKey(API秘钥)：{3};是否启用：{4}", appid, appsecret, mchid, key,chkisno);
                    Utils.AddSysBusinessLog(CompId, "paymentbank", paywxandaliModel.ID.ToString(), "微信收款帐号新增", strmessage, this.UserID.ToString());
                }
            }


            if (fal)
            {
                TranSaction.Commit();
                bind = "{\"ds\":\"0\",\"prompt\":\"提交成功！\"}";
            }
            else
            {
                TranSaction.Rollback();
                bind = "{\"ds\":\"1\",\"prompt\":\"提交失败！\"}";
            }
        }
        catch (Exception ex)
        {
            TranSaction.Rollback();
            bind = "{\"ds\":\"1\",\"prompt\":\"提交失败！\"}";
        }
        finally
        {
            Connection.Dispose();

            Response.Write(bind);
            Response.End();
        }
   
    }
}