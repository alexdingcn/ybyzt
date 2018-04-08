using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public partial class Company_newOrder_logistadd : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(Request["CompID"] + ""))
                CompID = (Request["CompID"] + "").ToInt(0);

            logisticsbind();

            if (!string.IsNullOrEmpty(Request["str"]))
            {
                var str = Request["str"];

                string[] s = str.Split(':');

                if (s.Length > 0)
                {
                    this.txtLogistics.Value = s[0];
                    this.txtLogisticsNo.Value = s[1];
                    this.txtCarUser.Value = s[2];
                    this.txtCarNo.Value = s[3];
                    this.txtCar.Value = s[4];
                }
            }
            else if (!string.IsNullOrEmpty(Request["KeyID"]))
            {
                KeyID = Common.DesDecrypt((Request["KeyID"] + ""), Common.EncryptKey).ToInt(0);

                List<Hi.Model.DIS_Logistics> logist = new Hi.BLL.DIS_Logistics().GetList("*", "  Isnull(dr,0)=0 and OrderOutID=" + KeyID, "");
                if (logist != null && logist.Count > 0)
                {
                    this.txtLogistics.Value = logist[0].ComPName;
                    this.txtLogisticsNo.Value = logist[0].LogisticsNo;
                    this.txtCarUser.Value = logist[0].CarUser;
                    this.txtCarNo.Value = logist[0].CarNo;
                    this.txtCar.Value = logist[0].Car;
                }

            }
        }
    }

    public void logisticsbind()
    {
        List<Hi.Model.BD_ComLogistics> cl = new Hi.BLL.BD_ComLogistics().GetList("", "dr=0 and CompID=" + CompID, "");

        if (cl != null && cl.Count > 0)
        {
            this.rptlogista.DataSource = cl;
            this.rptlogista.DataBind();
        }
    }
 
    [WebMethod]
    public static string Edit(string KeyID,string logistics, string logisticsNo,string carUser,string carNo,string car)
    {
        logistics = Common.NoHTML(logistics);
        logisticsNo = Common.NoHTML(logisticsNo);
        carUser = Common.NoHTML(carUser);
        carNo = Common.NoHTML(carNo);
        car = Common.NoHTML(car);
        Common.ResultMessage Msg = new Common.ResultMessage();
        Hi.BLL.DIS_Logistics OrderBllLogistics = new Hi.BLL.DIS_Logistics();
        KeyID = Common.DesDecrypt(KeyID, Common.EncryptKey);

        List<Hi.Model.DIS_Logistics> logist = new Hi.BLL.DIS_Logistics().GetList("*", "  Isnull(dr,0)=0 and OrderOutID=" + KeyID, "");
      
        //Hi.Model.DIS_Logistics OrderModelLogistics = logist;
        if (KeyID!=null)
        {
            if (logist != null)
            {
                logist[0].ComPName = logistics;
                logist[0].LogisticsNo = logisticsNo;


                if (logistics != "" && logisticsNo != "")
                {
                    string ApiKey = "4088ed72ed034b61b4b5adf05870aeba";
                    string typeCom = logistics;
                    typeCom = Information.TypeCom(typeCom);
                    string nu = logisticsNo;
                    string apiurl = "http://www.aikuaidi.cn/rest/?key=" + ApiKey + "&order=" + nu + "&id=" + typeCom + "&ord=asc&show=json";
                    WebRequest request = WebRequest.Create(@apiurl);
                    WebResponse response = request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(stream, encode);
                    string detail = reader.ReadToEnd();
                    Logistics logis = JsonConvert.DeserializeObject<Logistics>(detail);
                    if (logis.errCode == "0")
                    {
                        List<Information> information = logis.data;
                        logist[0].Context = JsonConvert.SerializeObject(information);
                    }
                    else
                    {
                        logist[0].Context = "";
                    }
                }

                logist[0].CarUser = carUser;
                logist[0].CarNo = carNo;
                logist[0].Car = car;
                logist[0].ts = DateTime.Now;
                if (OrderBllLogistics.Update(logist[0]))
                {
                    Msg.result = true;
                }
            }
        }
        else
        {
            Msg.code = "未查找到数据";
        }
        return new JavaScriptSerializer().Serialize(Msg);

    }
}