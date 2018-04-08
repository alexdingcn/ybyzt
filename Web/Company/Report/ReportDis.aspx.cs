using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinancingReferences;
using System.Configuration;

public partial class Company_Report_ReportDis : System.Web.UI.Page
{
    public string url = string.Empty;
    public string higth = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region bo报表地址
        string str = string.Empty;//保存token字符串
        string username = string.Empty;//保存当前登陆人名称
        username = Common.UserName();//获取当前登陆人
        IPubnetwk wk = new IPubnetwk();//java接口实例

        try
        {
            //判断session是否为空
            if (Session["token"] != null && Session["bo_username"] != null)
            {
                if (username.Equals(Convert.ToString(Session["bo_username"])))
                    str = Session["token"].ToString();
                else
                {
                    Session.Add("bo_username", username);
                    str = wk.BOLogin("{\"poUsername\":\"administrator\",\"poPassword\":\"Password01\",\"cms\":\"120.55.125.131:6400\",\"authentication\":\"secEnterprise\"}");
                    Session.Add("token", str);
                }

            }
            else
            {
                Session.Add("bo_username", username);

                str = wk.BOLogin("{\"poUsername\":\"administrator\",\"poPassword\":\"Password01\",\"cms\":\"120.55.125.131:6400\",\"authentication\":\"secEnterprise\"}");
                Session.Add("token", str);
            }
            string Bopath = ConfigurationManager.AppSettings["BoPath"];//&sReportMode=sales
            url = Bopath + "/BOE/OpenDocument/1511031559/OpenDocument/opendoc/openDocument.jsp?sIDType=AaVtArVx7HxAi7xmF4GEEGA&iDocID=5764&lsSyear=2014&lsSmonth=8&sType=wid&sRefresh=Y&token=" + str;


        }
        catch
        {

        }
        #endregion
    }
}