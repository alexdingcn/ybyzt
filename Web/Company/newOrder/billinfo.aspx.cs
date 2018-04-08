using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_newOrder_billinfo : System.Web.UI.Page
{
    //订单ID
    public int KeyID = 0;
    //代理商ID
    public int DisID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //当订单ID为0时、新增订单，否则修改订单发票信息
            if (!string.IsNullOrEmpty(Request["KeyID"]))
            KeyID = (Request["KeyID"] + "").ToInt(0);

            txtbill.Value =Common.NoHTML( Request["BillNo"]) + "";

            int IsBill = (Request["IsBill"] + "").ToInt(0);

            this.checkbox_4_1.Checked = IsBill == 0 ? false : true;

        }
    }
    [WebMethod]
    public static string Edit(string KeyID, string bill, string IsBill)
    {
       
        Common.ResultMessage Msg = new Common.ResultMessage();
        KeyID = Common.DesDecrypt(KeyID, Common.EncryptKey);
        List<Hi.Model.DIS_OrderExt> oextl = new Hi.BLL.DIS_OrderExt().GetList("","OrderID="+KeyID,"");
        if (oextl != null && oextl.Count>0)
        {

            //if (oextl[0].IsBill == 0)
            //{
                oextl[0].BillNo = bill;
                oextl[0].IsBill = IsBill.ToInt(0);
                if (new Hi.BLL.DIS_OrderExt().Update(oextl[0]))
                {
                    Msg.result = true;
                }
            //}
            //else
            //{
            //    Msg.code = "发票已开完";
            //}
        }
        else
        {
            Msg.code = "未查找到数据";
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }
}