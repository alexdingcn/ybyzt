using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Data;

public partial class Company_SysManager_ComLogisticsAdd : CompPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"] + "";
        if (action.Equals("Complog"))
        {
            string json = Request["json"] + "";
            complog(json);
        }
        if (action.Equals("ComplogDel"))
        {
            string json = Request["json"] + "";
            ComlogDel(json);
        }

        bind();
    }

    public void bind()
    {
        List<Hi.Model.BD_ComLogistics> cl = new Hi.BLL.BD_ComLogistics().GetList("", "dr=0 and CompID=" + this.CompID, "");

        if (cl != null && cl.Count > 0)
        {
            var json = "{";
            //string str = string.Empty;
            for (int i = 0; i <= cl.Count - 1; i++)
            {
                //str += "<li title=\"" + cl[i].LogisticsName + "\" class=\"Comli\">" + cl[i].LogisticsName + "</li>";
                json += ",\"" + i + "\":\"" + cl[i].LogisticsName + "\"";
            }
            //this.ulComLog.InnerHtml = str;
            json += "}";
            json = json.Replace("{,\"0\"", "{\"0\"");

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>bindComLog('" + json + "')</script>");

        }
    }

    /// <summary>
    /// 新增保存
    /// </summary>
    /// <param name="json"></param>
    protected void complog(string json)
    {
        JsonData JInfo = JsonMapper.ToObject(json);

        string bind = "{\"ds\":\"1\",\"prompt\":\"提交失败\"}";
        StringBuilder strSql = new StringBuilder(); 

        if (JInfo.Count < 0)
        {
            Response.Write(bind);
            Response.End();
        }
        List<Hi.Model.BD_ComLogistics> cl = new Hi.BLL.BD_ComLogistics().GetList("", "dr=0 and CompID=" + this.CompID, "");

        SqlTransaction TranSaction = null;
        SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
        Connection.Open();
        TranSaction = Connection.BeginTransaction();

        try
        {
            for (int i = 0; i <= JInfo.Count - 1; i++)
            {
                //系统设置的标题
                string name = JInfo["" + i + ""].ToString();

                List<Hi.Model.BD_ComLogistics> sl = cl.FindAll(p => p.LogisticsName == name);
                if (sl != null && sl.Count > 0)
                {

                }
                else
                {
                    strSql.AppendFormat("INSERT INTO BD_ComLogistics ([CompID],[LogisticsName],[LogisticsCode],[CreateDate],[CreateUserID],[dr],[modifyuser],[ts]) VALUES({0},'{1}',null,'{2}',{3},0,{4},'{5}');", CompID, name, DateTime.Now, UserID, UserID, DateTime.Now);
                }
            }

            if (!strSql.ToString().Equals(""))
            {
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
            else
            {
                //没有新增的
                bind = "{\"ds\":\"0\",\"prompt\":\"提交成功！\"}";
            }
        }
        catch (Exception)
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

    /// <summary>
    /// 删除物流信息
    /// </summary>
    /// <param name="json"></param>
    private void ComlogDel(string json)
    {
        string bind = "{\"ds\":\"1\",\"prompt\":\"提交失败\"}";
        StringBuilder strSql = new StringBuilder();

        try
        {
            List<Hi.Model.BD_ComLogistics> cl = new Hi.BLL.BD_ComLogistics().GetList("", "CompID=" + this.CompID + " and LogisticsName='" + json + "' and dr=0", "");

            if (cl != null && cl.Count > 0)
            {
                foreach (Hi.Model.BD_ComLogistics item in cl)
                {
                    item.dr = 1;
                    new Hi.BLL.BD_ComLogistics().Update(item);
                }

                bind = "{\"ds\":\"0\",\"prompt\":\"删除成功！\"}";
            }
            else
            {
                bind = "{\"ds\":\"3\",\"prompt\":\"没有删除的数据！\"}";
            }

        }
        catch (Exception)
        {
            bind = "{\"ds\":\"1\",\"prompt\":\"提交失败！\"}";
        }
        finally
        {
            Response.Write(bind);
            Response.End();
        }
    }
}