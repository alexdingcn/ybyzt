using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hi.BLL;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Text;

public partial class Company_Contract_AreaMvg : CompPageBase
{
    //订单、订单明细ID
    public int KeyID = 0;
    public string DisArea = string.Empty;//代理商区域数据源

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["TrId"]))
                this.hidTrId.Value = Request["TrId"].ToString();
            if (!string.IsNullOrEmpty(Request["indexs"]))
                this.hidIndex.Value = Request["indexs"].ToString();
            if (!string.IsNullOrEmpty(Request["AreaID"]))
            {
                this.hid_txtDisArea.Value = Request["AreaID"].ToString();
                this.hid_txtDisArea.Attributes.Add("code", Request["AreaID"].ToString());
                this.txtDisArea.Value = Common.GetDisAreaNameById(Request["AreaID"].ToString().ToInt(0));
            }
            bind();
        }

    }


    public void bind()
    {
        

        //绑定区域
        StringBuilder sbare = new StringBuilder();
        List<Hi.Model.BD_DisArea> are = new Hi.BLL.BD_DisArea().GetList("top 12 * ", "isnull(dr,0)=0 and  ParentId=0  and CompanyID=" + this.CompID, " SortIndex");
        if (are.Count > 0)
        {
            sbare.Append("[");
            int num = 0;
            foreach (var model in are)
            {
                num++;
                sbare.Append("{code:'" + model.ID + "',value: '" + model.ID + "',label: '" + model.AreaName + "'");
                List<Hi.Model.BD_DisArea> aret1 = new Hi.BLL.BD_DisArea().GetList("Areacode,ID,AreaName", "isnull(dr,0)=0  and ParentId=" + model.ID, "");
                if (aret1.Count > 0)
                {
                    sbare.Append(",children: [");
                    int num2 = 0;
                    foreach (var model2 in aret1)
                    {
                        num2++;
                        sbare.Append("{code:'" + model2.ID + "',value: '" + model2.ID + "',label: '" + model2.AreaName + "'");
                        List<Hi.Model.BD_DisArea> are3 = new Hi.BLL.BD_DisArea().GetList("Areacode,ID,AreaName", "isnull(dr,0)=0  and ParentId=" + model2.ID, "");
                        if (are3.Count > 0)
                        {
                            sbare.Append(",children: [");
                            int num3 = 0;
                            foreach (var item3 in are3)
                            {
                                num3++;
                                if (num3 == are3.Count)
                                    sbare.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.AreaName + "'}");
                                else
                                    sbare.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.AreaName + "'},");

                            }
                            sbare.Append("]");

                        }

                        if (num2 == aret1.Count)
                            sbare.Append("}");
                        else
                            sbare.Append("},");
                    }
                    sbare.Append("]");

                }
                if (num == are.Count)
                    sbare.Append("}");
                else
                    sbare.Append("},");
            }
            sbare.Append("]");
            DisArea = sbare.ToString();
        }


        

    }
}