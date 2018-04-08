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

public partial class Company_SysManager_PayMentSettings : AdminPageBase
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
        List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + KeyID, "");
        if (Sysl.Count > 0)
        {
            // Hi.Model.Pay_PaymentSettings=Sysl[0];
            //手续费收取
            int zfsxf = Sysl[0].pay_sxfsq;
            if (zfsxf == 0)
                this.rdo_pt.Checked = true;
            else if (zfsxf == 1)
                this.rdo_jxs.Checked = true;
            else
                this.rdo_hxqy.Checked = true;

            //支付方式
            int zffs = Sysl[0].pay_zffs;
            if (zffs == 0)
                this.che_xs.Checked = true;
            else if (zffs == 1)
                this.che_xx.Checked = true;
            else
            {
                this.che_xs.Checked = true;
                this.che_xx.Checked = true;
            }

            //手续费比例
            this.txt_kjzfbl.Value = Convert.ToString(Sysl[0].pay_kjzfbl);
            //this.txt_kjzfstart.Value = Convert.ToString(Sysl[0].pay_kjzfstart);
            //this.txt_kjzfend.Value = Convert.ToString(Sysl[0].pay_kjzfend);

            //this.txt_ylzfbl.Value = Convert.ToString(Sysl[0].pay_ylzfbl);
            //this.txt_ylzfstart.Value = Convert.ToString(Sysl[0].pay_ylzfstart);
            //this.txt_ylzfend.Value = Convert.ToString(Sysl[0].pay_ylzfend);

            this.txt_b2cwyzfbl.Value = Convert.ToString(Sysl[0].pay_b2cwyzfbl);
            //this.txt_b2cwyzfstart.Value = Convert.ToString(Sysl[0].vdef1);

            this.txt_b2bwyzf.Value = Convert.ToString(Sysl[0].pay_b2bwyzf);

            //免手续费支付次数
            //this.txt_mfcs.Value = Convert.ToString(Sysl[0].Pay_mfcs);

        }
        else //默认值显示
        {
            //手续费收取
            this.rdo_pt.Checked = true;

            //支付方式
            this.che_xs.Checked = true;
            this.che_xx.Checked = true;

            //手续费比例
            this.txt_kjzfbl.Value = "5";
            //this.txt_kjzfstart.Value = "2";
            //this.txt_kjzfend.Value = "40";

            //this.txt_ylzfbl.Value = "3";
            //this.txt_ylzfstart.Value = "10";
            //this.txt_ylzfend.Value = "50";

            this.txt_b2cwyzfbl.Value = "5";

            this.txt_b2bwyzf.Value = "10";

            //免手续费支付次数
           // this.txt_mfcs.Value ="0";
        }

        hid_CompID.Value = Convert.ToString(KeyID);
       

        

        //ClientScript.RegisterStartupScript(this.GetType(), "bind", "<script>binddata('" + Newtonsoft.Json.JsonConvert.SerializeObject(Sysl) + "')</script>");
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

            //手续费收取
            int pay_sxfsq = Convert.ToInt32(Request["pay_sxfsq"]);           

            //支付方式
            int pay_zffs = Convert.ToInt32(Request["pay_zffs"]);          

            //手续费比例
            decimal pay_kjzfbl = Convert.ToDecimal(Request["pay_kjzfbl"] + "" == "" ? "0" : Request["pay_kjzfbl"]);
            decimal pay_kjzfstart = Convert.ToDecimal(Request["pay_kjzfstart"] + "" == "" ? "0" : Request["pay_kjzfstart"]);
            decimal pay_kjzfend = Convert.ToDecimal(Request["pay_kjzfend"] + "" == "" ? "0" : Request["pay_kjzfend"]);

            decimal pay_ylzfbl =0;// Convert.ToDecimal(Request["pay_ylzfbl"] + "" == "" ? "0" : Request["pay_ylzfbl"]);
            decimal pay_ylzfstart =0;// Convert.ToDecimal(Request["pay_ylzfstart"] + "" == "" ? "0" : Request["pay_ylzfstart"]);
            decimal pay_ylzfend = 0;// Convert.ToDecimal(Request["pay_ylzfend"] + "" == "" ? "0" : Request["pay_ylzfend"]);

            decimal pay_b2cwyzfbl = Convert.ToDecimal(Request["pay_b2cwyzfbl"] + "" == "" ? "0" : Request["pay_b2cwyzfbl"]);
            decimal pay_b2cwyzfstart=Convert.ToDecimal(Request["pay_b2cwyzfstart"]+""==""?"0":Request["pay_b2cwyzfstart"]);

            decimal pay_b2bwyzf = Convert.ToDecimal(Request["pay_b2bwyzf"] + "" == "" ? "0" : Request["pay_b2bwyzf"]);

            //免手续费支付次数
            decimal Pay_mfcs = Convert.ToDecimal(Request["Pay_mfcs"] + "" == "" ? "0" : Request["Pay_mfcs"]); 

         


            //查询该企业的设置
            List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + CompId, "");

            //判断企业的是否有设置
            if (Sysl.Count > 0)
            {
                strSql = string.Format(@"UPDATE [Pay_PaymentSettings]
                   SET [pay_sxfsq] = {0}
                      ,[pay_zffs] = {1}
                      ,[pay_kjzfbl] = {2}
                      ,[pay_kjzfstart] = {3}
                      ,[pay_kjzfend] ={4}
                      ,[pay_ylzfbl] = {5}
                      ,[pay_ylzfstart] ={6}
                      ,[pay_ylzfend] = {7}
                      ,[pay_b2cwyzfbl] = {8}
                      ,[pay_b2bwyzf] = {9}
                      ,[Pay_mfcs] = {10}       
                      ,[modifyuser] = {11}
                      ,vdef1={13}
                 WHERE [CompID] = {12}", pay_sxfsq, pay_zffs, pay_kjzfbl, pay_kjzfstart, pay_kjzfend
                        , pay_ylzfbl, pay_ylzfstart, pay_ylzfend, pay_b2cwyzfbl, pay_b2bwyzf, Pay_mfcs, UserID, CompId, pay_b2cwyzfstart);

            }
            else
            {
                strSql = string.Format(@"INSERT INTO [Pay_PaymentSettings]
                               ([CompID]
                               ,[pay_sxfsq]
                               ,[pay_zffs]
                               ,[pay_kjzfbl]
                               ,[pay_kjzfstart]
                               ,[pay_kjzfend]
                               ,[pay_ylzfbl]
                               ,[pay_ylzfstart]
                               ,[pay_ylzfend]
                               ,[pay_b2cwyzfbl]
                               ,[pay_b2bwyzf]
                               ,[Pay_mfcs]
                               ,[createUser]
                               ,[createDate]
                               ,[Start]
                               ,[remark]          
                               ,[ts]
                               ,[dr]
                               ,[modifyuser]
                               ,vdef1)
                         VALUES
                               ({0}
                               ,{1}
                               ,{2}
                               ,{3}
                               ,{4}
                               ,{5}
                               ,{6}
                               ,{7}
                               ,{8}
                               ,{9}
                               ,{10}
                               ,{11}
                               ,{12}
                               ,'{13}'
                               ,{14}
                               ,'{15}'         
                               ,'{16}'
                               ,{17}
                               ,{18}
                               ,{19})", CompId, pay_sxfsq, pay_zffs, pay_kjzfbl, pay_kjzfstart, pay_kjzfend, pay_ylzfbl
                            , pay_ylzfstart, pay_ylzfend, pay_b2cwyzfbl, pay_b2bwyzf, Pay_mfcs, UserID, DateTime.Now, 0
                            , "", DateTime.Now, 0, UserID, pay_b2cwyzfstart);
            }

            SqlCommand cmd = new SqlCommand(strSql.ToString(), Connection, TranSaction);
            cmd.CommandType = CommandType.Text;
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
            if (rowsAffected > 0)
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