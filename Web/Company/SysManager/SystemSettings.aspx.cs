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

public partial class Company_SysManager_SystemSettings : CompPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string action = Request["action"] + "";
            if (action.Equals("sett"))
            {
                string json = Request["json"] + "";
                Settings(json);
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
        List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", " CompID=" + CompID, "");
        List<Hi.Model.SYS_SysName> sl = null;

        #region 代理商设置
        //代理商加盟是否需要审核
        sl = Sysl.FindAll(p => p.Name == "代理商加盟是否需要审核");
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "1")
                this.rdodisNo.Checked = true;
            else
                this.rdodisOk.Checked = true;

            sl = null;
        }
        else
            this.rdodisOk.Checked = true;
        #endregion

        #region 代理商支付设置
        //代理商加盟是否需要审核
        sl = Sysl.FindAll(p => p.Name == "支付方式");
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "1")
                this.payDBNo.Checked = true;
            else
                this.payDBOk.Checked = true;

            sl = null;
        }
        else
            this.payDBOk.Checked = true;
        #endregion

        #region 订单配置
        //订单自动签收
        sl = Sysl.FindAll(p => p.Name == "订单自动签收");
        if (sl != null && sl.Count > 0)
        {
            this.txtSinceSign.Value = sl[0].Value;
            sl = null;
        }
        else
            this.txtSinceSign.Value = "15";

        //超时未付款自动作废订单
        sl = Sysl.FindAll(p => p.Name == "超时未付款自动作废订单");
        if (sl != null && sl.Count > 0)
        {
            this.txtSinceOff.Value = sl[0].Value;
            sl = null;
        }
        else
            this.txtSinceOff.Value = "30";

        //代客下单是否需要审核
        sl = Sysl.FindAll(p => p.Name == "代客下单是否需要审核");
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "1")
                this.rdoOrderAddOK.Checked = true;
            else
                this.rdoOrderAddNO.Checked = true;

            sl = null;
        }
        else
            this.rdoOrderAddNO.Checked = true;

        //订单下单数量是否取整
        sl = Sysl.FindAll(p => p.Name == "订单下单数量是否取整");
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "0")
                this.rdoDigits0.Checked = true;
            else
                this.rdoDigits2.Checked = true;
            sl = null;
        }
        else
            this.rdoDigits0.Checked = true;

        sl = Sysl.FindAll(p => p.Name == "订单支付返利是否启用");
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "0")
                this.Rebate1.Checked = true;
            else
                this.Rebate2.Checked = true;
            sl = null;
        }
        else
            this.Rebate1.Checked = true;

        #endregion

        #region 商品配置
        //商品是否启用库存
        sl = Sysl.FindAll(p => p.Name == "商品是否启用库存");
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "0")
                this.rdoStockOk.Checked = true;
            else
                this.rdoStockNo.Checked = true;

            sl = null;
        }
        else
            this.rdoStockOk.Checked = true;

        //商品分类选择是否折叠
        sl = Sysl.FindAll(p => p.Name == "商品分类选择是否折叠");
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "1")
                this.rdoFoldingOk.Checked = true;
            else
                this.rdoFoldingNo.Checked = true;
            sl = null;
        }
        else
            this.rdoFoldingOk.Checked = true;

        sl = Sysl.FindAll(p => p.Name == "商品标签管理");
        if (sl != null && sl.Count > 0)
        {
            string val = sl[0].Value;
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>GoodsL('" + val + "')</script>");
            sl = null;
        }
        else
        {
            this.txtGoodsLable1.Value = "新品上架";
            this.txtGoodsLable2.Value = "热卖推荐";
            this.txtGoodsLable3.Value = "清仓优惠";
        }

        sl = Sysl.FindAll(p => p.Name == "商品自定义字段");
        if (sl != null && sl.Count > 0)
        {
            string val = sl[0].Value;
            ClientScript.RegisterStartupScript(this.GetType(), "GoodsC", "<script>GoodsC('" + val + "')</script>");
            sl = null;
        }
        //是否店铺开放价格
        sl = Sysl.FindAll(p => p.Name == "是否店铺开放价格");
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "0")
                this.kfmoney0.Checked = true;
            else
                this.kfmoney1.Checked = true;

            sl = null;
        }
        else
            this.kfmoney0.Checked = true;

        #endregion

        #region 查询企业物流信息

        List<Hi.Model.BD_ComLogistics> cl = new Hi.BLL.BD_ComLogistics().GetList("", "dr=0 and CompID=" + this.CompID, "");

        if (cl != null && cl.Count > 0)
        {
            string str = string.Empty;
            foreach (Hi.Model.BD_ComLogistics item in cl)
            {
                str += "<i style=\"margin-left:10px;\" title=\"" + item.LogisticsName + "\">" + item.LogisticsName + "</i>";
            }

            this.divlogistics.InnerHtml = str;
        }

        #endregion

        #region 计息设置

        #endregion

        #region 订单完成节点设置
        sl = Sysl.FindAll(p => p.Name == "订单完成节点设置");
        string scriptstr = string.Empty;
        if (sl != null && sl.Count > 0)
        {
            if (sl[0].Value.ToString() == "0")
            {
                scriptstr = "$(\".quan4\").text(\"√\")";
            }
            else if (sl[0].Value.ToString() == "1")
            {
                scriptstr = "$(\".quan4\").text(\"\")";
            }
            else if (sl[0].Value.ToString() == "2")
            {
                scriptstr = "$(\".ddlist li\").eq(2).find(\".quan4\").text(\"√\");$(\".ddlist li\").eq(3).find(\".quan4\").text(\"\");$(\".ddlist li\").eq(4).find(\".quan4\").text(\"\");";
            }
            else if (sl[0].Value.ToString() == "3")
            {
                scriptstr = "$(\".ddlist li\").eq(2).find(\".quan4\").text(\"√\");$(\".ddlist li\").eq(3).find(\".quan4\").text(\"√\");$(\".ddlist li\").eq(4).find(\".quan4\").text(\"\");";
            }
            sl = null;
        }
        else
        {
            scriptstr = "$(\".quan4\").text(\"√\")";
        }
        #endregion
        if (scriptstr != "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ddjdsz", "<script>$(function(){" + scriptstr + "})</script>");
        }
    }

    /// <summary>
    /// 保存系统设置信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Settings(string str)
    {
        JsonData JInfo = JsonMapper.ToObject(str);

        string bind = "{\"ds\":\"1\",\"prompt\":\"提交失败\"}";

        if (JInfo.Count < 0)
        {
            Response.Write(bind);
            Response.End();
        }

        StringBuilder strSql = new StringBuilder();

        SqlTransaction TranSaction = null;
        SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
        Connection.Open();
        TranSaction = Connection.BeginTransaction();

        try
        {
            //查询该企业的设置
            List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", " CompID=" + CompID, "");
            int d = 0;

            //判断企业的是否有设置
            if (Sysl != null && Sysl.Count > 0)
            {
                for (int i = 0; i <= JInfo.Count - 1; i++)
                {
                    JsonData j = JInfo[i];
                    //系统设置的标题
                    string key =Common.NoHTML( j["key"].ToString());
                    //系统设置的值
                    string val =Common.NoHTML( j["val"].ToString());


                    //if (key == "商品是否启用库存")
                    //{
                    //    List<Hi.Model.DIS_Order> l = new Hi.BLL.DIS_Order().GetList("", " compID=" + CompID + " and OState in(1,2) and isnull(dr,0)=0", "");
                    //    if (l != null && l.Count > 0)
                    //    {
                    //        d = 1;
                    //        continue;
                    //    }
                    //}

                    List<Hi.Model.SYS_SysName> sl = Sysl.FindAll(p => p.Name == key);
                    if (sl != null && sl.Count > 0)
                        strSql.AppendFormat("update SYS_SysName set Value='{0}',ts='{1}' where Name='{2}' and CompID={3};", val, DateTime.Now.AddDays(-1), key, CompID);
                    else
                        strSql.AppendFormat("INSERT INTO [SYS_SysName]([CompID],[Code],[Name],[Value],[ts],[dr],[modifyuser])VALUES({0},'','{1}','{2}','{3}',0,{4});", CompID, key, val, DateTime.Now.AddDays(-1), UserID);
                }
            }
            else
            {
                for (int i = 0; i <= JInfo.Count - 1; i++)
                {
                    JsonData j = JInfo[i];

                    //系统设置的标题
                    string key =Common.NoHTML( j["key"].ToString());
                    //系统设置的值
                    string val =Common.NoHTML( j["val"].ToString());

                    strSql.AppendFormat("INSERT INTO [SYS_SysName]([CompID],[Code],[Name],[Value],[ts],[dr],[modifyuser])VALUES({0},'','{1}','{2}','{3}',0,{4});", CompID, key, val, DateTime.Now.AddDays(-1), UserID);
                }
            }

            SqlCommand cmd = new SqlCommand(strSql.ToString(), Connection, TranSaction);
            cmd.CommandType = CommandType.Text;
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
            if (rowsAffected > 0)
            {
                TranSaction.Commit();

                //if (d == 1)
                //    bind = "{\"ds\":\"0\",\"prompt\":\"订单没有处理完成，修改商品启用库存失败！\"}";
                //else
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