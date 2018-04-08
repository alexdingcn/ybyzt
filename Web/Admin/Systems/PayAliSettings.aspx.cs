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

public partial class Company_SysManager_PayAliSettings : AdminPageBase
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

            if (Sysl[0].ali_isno == "1")
                this.chisno.Checked = true;

            this.seller_email.Value = Convert.ToString(Sysl[0].ali_seller_email);
            this.partner.Value = Convert.ToString(Sysl[0].ali_partner);
            this.PayKey.Value = Convert.ToString(Sysl[0].ali_key);
            this.alirsa.Value = Convert.ToString(Sysl[0].ali_RSAkey);
           

        }
        else //默认值显示
        {
            this.seller_email.Value = "";
            this.partner.Value = "";
            this.PayKey.Value = "";
          
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
            string seller_email = Convert.ToString(Request["seller_email"]);
            string partner = Convert.ToString(Request["partner"]);
            string PayKey = Convert.ToString(Request["PayKey"]);
            string alirsa = Convert.ToString(Request["alirsa"]);

            bool fal = false;

            //查询该企业的设置
            List<Hi.Model.Pay_PayWxandAli> Sysl = new Hi.BLL.Pay_PayWxandAli().GetList("", " CompID=" + CompId, "");

            //判断企业的是否有设置
            if (Sysl.Count > 0)
            {
                Hi.Model.Pay_PayWxandAli paywxandaliModel = Sysl[0];
                paywxandaliModel.ali_isno = chkisno;
                paywxandaliModel.ali_seller_email = seller_email;
                paywxandaliModel.ali_partner = partner;
                paywxandaliModel.ali_key = PayKey;
                paywxandaliModel.ali_RSAkey = alirsa;
                fal=  new Hi.BLL.Pay_PayWxandAli().Update(paywxandaliModel);
                if (fal)
                {
                    string strmessage = string.Format("支付宝企业账户：{0};合作者身(Partner ID)：{1};安全校验码（Key）：{2};RSA加密(RSA秘钥)：{3};是否启用：{4}", seller_email, partner, PayKey, alirsa,chisno);
                    Utils.AddSysBusinessLog(CompId, "paymentbank", paywxandaliModel.ID.ToString(), "支付宝收款账户修改", strmessage, this.UserID.ToString());
                }
            }
            else
            {
                Hi.Model.Pay_PayWxandAli paywxandaliModel =new Hi.Model.Pay_PayWxandAli();
                paywxandaliModel.ali_isno = chkisno;
                paywxandaliModel.ali_seller_email = seller_email;
                paywxandaliModel.ali_partner = partner;
                paywxandaliModel.ali_key = PayKey;
                paywxandaliModel.ali_RSAkey = alirsa;
                paywxandaliModel.CompID = CompId;
               
               int num = new Hi.BLL.Pay_PayWxandAli().Add(paywxandaliModel);
               if (num > 0)
               {
                   fal = true;
                   string strmessage = string.Format("支付宝企业账户：{0};合作者身(Partner ID)：{1};安全校验码（Key）：{2};RSA加密(RSA秘钥)：{3};是否启用：{4}", seller_email, partner, PayKey, alirsa,chisno);
                    Utils.AddSysBusinessLog(CompId, "paymentbank", paywxandaliModel.ID.ToString(), "支付宝收款账户新增", strmessage, this.UserID.ToString());
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