using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using DBUtility;
using System.IO;
using LitJson;

/// <summary>
///GetSource 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class GetSource : System.Web.Services.WebService {

    public GetSource () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public void HelloWorld(string GetType)
    //{
    //    string json = "";
    //    StreamReader Sreader = ImportDisProD.VisitWebService("http://localhost:84/MoveService/GetSource.asmx/GetDataSource?Key=" + Common.DesEncrypt("1qaz@WSX", "HaiYuSE9SFOT") + "&GetType=" + GetType + "");
    //    if (Sreader != null)
    //    {
    //        json = Sreader.ReadToEnd();
    //        JsonData Jdata = JsonMapper.ToObject(json);
    //        JsonData data = Jdata["table"];
    //        foreach (JsonData jdata in data)
    //        {

    //        }
    //    }
    //    Context.Response.Write(json);
    //    Context.Response.End();
    //}

    [WebMethod]
    public void GetDataSource(string Key, string GetType)
    {
        string Source = string.Empty;
        if (Common.DesDecrypt(Key, "HaiYuSE9SFOT") == "1qaz@WSX")
        {
            string Sql = "";
            if (GetType == "Comp")
            {
                Sql = "select CompName,convert(varchar(10),CreateDate,120) CreateDate from BD_Company where dr=0  order by CreateDate desc";
                DataSet Ds = SqlHelper.Query(SqlHelper.LocalSqlServer, Sql);
                Source = DataTableToJson("table", Ds.Tables[0]);
            }
            else if (GetType == "Dis")
            {
                Sql = "select DisName,convert(varchar(10),CreateDate,120) CreateDate from BD_Distributor where dr=0   order by CreateDate desc";
                DataSet Ds = SqlHelper.Query(SqlHelper.LocalSqlServer, Sql);
                Source = DataTableToJson("table", Ds.Tables[0]);
            }
            else if (GetType == "Pay")
            {
                Sql = "select top 100 PayPrice,convert(varchar(10),PayDate,120) PayDate,Type from PAY_Payment where dr=0 and IsAudit=1  order by PayDate desc";
                DataSet Ds = SqlHelper.Query(SqlHelper.LocalSqlServer, Sql);
                Source = DataTableToJson("table", Ds.Tables[0]);
            }
            else if (GetType == "Order")
            {
                Sql = "select top 50 ReceiptNo,TotalAmount,convert(varchar(10),CreateDate,120) CreateDate,OState,PayState,ReturnState,IsAudit,AddType,Otype from DIS_Order where dr=0  order by CreateDate desc";
                DataSet Ds = SqlHelper.Query(SqlHelper.LocalSqlServer, Sql);
                Source = DataTableToJson("table", Ds.Tables[0]);
            }
        }
        Context.Response.Write(Source);
        Context.Response.End();
    }

    /// <summary>
    /// //将Datatable转换为json格式数据并给table命名
    /// </summary>
    /// <param name="jsonName"></param>
    /// <param name="dt">table名称</param>
    /// <returns></returns>
    public static string DataTableToJson(string jsonName, DataTable dt)
    {
        StringBuilder Json = new StringBuilder();
        Json.Append("{\"" + jsonName + "\":[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString().ToLower() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]}");
        return Json.ToString();
    }
    
}
