using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using DBUtility;
using System.Web.Script.Serialization;

public partial class Company_GoodsNew_DisPriceList : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_txtTypename\").css(\"width\", \"120px\");</script>");
        if (!IsPostBack)
        {
            this.hidCompId.Value = this.CompID.ToString();//厂商id
            this.divGoodsName.InnerText = disBing(CompID.ToString());//下拉商品的动态加载隐藏
        }
    }
    /// <summary>
    ///下拉商品
    /// </summary>
    /// <returns></returns>
    public string disBing(string compid)
    {
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, SelectGoodsInfo.sql(compid, "")).Tables[0];
        return ConvertJson.ToJson(ds).ToString();
    }
}