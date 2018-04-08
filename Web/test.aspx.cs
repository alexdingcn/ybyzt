using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class test : System.Web.UI.Page
{
    public string ss = "";
    public string sa = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetBusines bu = new GetBusines();
            //ss = bu.GetBus("海遇（上海）软件股份公司", "110108197109070074", "913100007831300384", "张卫");

            //sa = bu.GetBus1("海遇（上海）软件股份公司", "110108197109070074", "913100007831300384", "张卫");
            //ss = bu.GetResponseData(ggs);
        }
        catch (Exception)
        {

            throw;
        }
    }
}