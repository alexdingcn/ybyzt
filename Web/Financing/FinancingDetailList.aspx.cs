using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Financing_FinancingDetailList : DisPageBase
{
    public string page = "1";//默认初始页
   // Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["IsFinancing"] != "1")
        {
            Response.Redirect("../NoOperable.aspx", true);
            Response.End();
        }
        //user = LoginModel.IsLogined(this);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
        }
    }

    protected void A_Seek(object sender, EventArgs e)
    {
        string str = "";
        if (txtFinancingNo.Value != "")
        {
            str += " and CONVERT(varchar(12) , ts, 112 ) + case when LEN(ID)>6 then convert(varchar(20),ID) else right(cast('000000'+rtrim(convert(varchar(20),ID)) as varchar(20)),6) end like '%" + txtFinancingNo.Value.Replace("'", "''") + "%'";
        }
        if (ddrType.Value != "-1")
        {
            str += " and type=" + ddrType.Value;
        }
        ViewState["strwhere"] = str;
        Bind();
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    public string GetStr(string str)
    {
        if (str.Length == 0)
        {
            str = "无";
        }
        else if (str.Length > 5)
        {
            str = str.Substring(0, 4) + "...";
        }
        return str;
    }

    public string getIDLen(string ID)
    {
        string newID = ID;
        for (int i = 0; i < 6; i++)
        {
            if (newID.Length == 6 || newID.Length > 6)
            {
                break;
            }
            else
            {
                newID = "0" + newID;
            }
        }
        return newID;
    }

    public string GetOrderNo(int OrderID)
    {
        Hi.Model.DIS_Order OModel = new Hi.BLL.DIS_Order().GetModel(OrderID);
        if (OModel == null)
        {
            return "";
        }
        else
        {

            return "<a href=\"javascript:void(0);\" onclick='Goinfo(\""+Common.DesEncrypt(OModel.ID.ToString(),Common.EncryptKey)+"\")'>"+OModel.ReceiptNo+"</a>";
        }
        
    }

    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        strwhere += " and (State = 1 or (State = 3 and Type=4)) and isnull(dr,0)=0 and disid=" + this.DisID;  //and PayState in (0,2,7)

        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length < 4)
            {
                Pager.PageSize = int.Parse(this.txtPager.Value.Trim());
            }
            else
            {
                this.txtPager.Value = "100";
                Pager.PageSize = 100;
            }
        }


        List<Hi.Model.PAY_Financing> Financing = new Hi.BLL.PAY_Financing().GetList(Pager.PageSize, Pager.CurrentPageIndex, "ts", true, strwhere, out pageCount, out Counts);
        this.repfinan.DataSource = Financing;
        this.repfinan.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
}