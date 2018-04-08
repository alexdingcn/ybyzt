using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

public partial class Distributor_newOrder_logistview : System.Web.UI.Page
{
    //发货单ID
    public int KeyID = 0;
    //厂商ID
    public int CompID = 0;
    //代理商ID
    public int DisID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            databind();
        }
    }

    public void databind()
    {

        if (!string.IsNullOrEmpty(Request["KeyID"]))
            KeyID =  Common.DesDecrypt((Request["KeyID"] + ""), Common.EncryptKey).ToInt(0);

        StringBuilder str = new StringBuilder();
        List<Hi.Model.DIS_Logistics> logisticsl = new Hi.BLL.DIS_Logistics().GetList("", " OrderOutID=" + KeyID + " and isnull(dr,0)=0", "");

        if (logisticsl != null && logisticsl.Count > 0)
        {
            if (logisticsl[0].ComPName != "" && logisticsl[0].LogisticsNo != "")
            {
                string ApiKey = "4088ed72ed034b61b4b5adf05870aeba";
                string typeCom = logisticsl[0].ComPName;
                typeCom = Information.TypeCom(typeCom);
                string nu = logisticsl[0].LogisticsNo;
                string apiurl = "http://www.aikuaidi.cn/rest/?key=" + ApiKey + "&order=" + nu + "&id=" + typeCom + "&ord=asc&show=json";
                WebRequest request = WebRequest.Create(@apiurl);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding encode = Encoding.UTF8;
                StreamReader reader = new StreamReader(stream, encode);
                string detail = reader.ReadToEnd();
                Logistics logistics = JsonConvert.DeserializeObject<Logistics>(detail);
                if (logistics.errCode == "0")
                {
                    List<Information> information = logistics.data;
                    logisticsl[0].Context = JsonConvert.SerializeObject(information);
                    new Hi.BLL.DIS_Logistics().Update(logisticsl[0]);
                }
                else
                {
                    logisticsl[0].Context = "";
                    new Hi.BLL.DIS_Logistics().Update(logisticsl[0]);
                }

                if (!string.IsNullOrEmpty(logisticsl[0].Context))
                {
                    //DataTable dt = JsonToDataTable(logisticsl[0].Context);

                    JavaScriptSerializer Serializer = new JavaScriptSerializer();
                    List<Information> objs = Serializer.Deserialize<List<Information>>(logisticsl[0].Context);

                    if (objs != null && objs.Count > 0)
                    {
                        //排序
                        List<Information> obj = objs.OrderByDescending(a => a.time).ToList();

                        string ts = "";
                        foreach (Information item in obj)
                        {
                            if (ts != item.time.ToDateTime().ToString("yyyy-MM-dd"))
                            {
                                ts = item.time.ToDateTime().ToString("yyyy-MM-dd");
                                str.AppendFormat(" <li><i class=\"day\">{0}</i><i class=\"time\">{1}</i>{2}<i class=\"circle\"></i></li>", item.time.ToString().ToDateTime().ToString("yyyy-MM-dd"), item.time.ToString().ToDateTime().ToString("HH:mm:ss"), item.content == null ? "" : item.content);
                            }
                            else
                            {
                                str.AppendFormat(" <li><i class=\"time\">{0}</i>{1}<i class=\"circle\"></i></li>", item.time.ToString().ToDateTime().ToString("HH:mm:ss"), item.content == null ? "" : item.content);
                            }
                        }
                    }
                }
                else
                {
                    str.AppendFormat(" <li style=\"border-left:0px;\">{0}</li>", "还没有流物信息");
                }
            }
            else
            {
                if (logisticsl[0].CarUser == "" && logisticsl[0].CarNo == "" && logisticsl[0].Car == "")
                {
                    str.AppendFormat(" <li style=\"border-left:0px;\">{0}</li>", "还没有流物信息");
                }
                else
                {
                    str.AppendFormat("<li style=\"border-left:0px;\"><i class=\"time\">司机姓名</i>{0}&nbsp;</li>", logisticsl[0].CarUser);
                    str.AppendFormat("<li style=\"border-left:0px;\"><i class=\"time\">司机手机</i>{0}&nbsp;</li>", logisticsl[0].CarNo);
                    str.AppendFormat("<li style=\"border-left:0px;\"><i class=\"time\">车牌号</i>{0}&nbsp;</li>", logisticsl[0].Car);
                }
            }
        }
        else
        {
            str.AppendFormat(" <li style=\"border-left:0px;\">{0}</li>", "还没有流物信息");

        }
        this.logislist.InnerHtml = str.ToString();
    }
}