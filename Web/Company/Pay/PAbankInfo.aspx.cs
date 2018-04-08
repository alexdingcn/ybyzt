using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Company_Pay_PAbankInfo : CompPageBase
{
    
    /// <summary>
    /// 命令
    /// </summary>
    private string action;

    public string Action
    {
        get { return action; }
        set { action = value; }
    }

    //关联银行卡表ID
    private string paid;
    public string Paid
    {
        get { return paid; }
        set { paid = value; }
    }

    //代理商Id
    public int DisId;
    //代理商地址Id
    public int AddrId;
   
    public string page;

   

    Hi.BLL.BD_Distributor BDdbutorbll = new Hi.BLL.BD_Distributor();

    protected void Page_Load(object sender, EventArgs e)
    {
       // Paid = Request.QueryString["paid"].ToString();

        if (Request.QueryString["action"] != null)
        {
            Action = Request.QueryString["action"].ToString();
        }
        if (Action == "Addr")
        {
            DisId = Convert.ToInt32(Request["DisId"]);
            AddrId = Convert.ToInt32(Request["AddrId"]);
            BindAddr(DisId, AddrId);
        }

        if (!IsPostBack)
        {
            DataTable dt = ViewState["Distributor"] as DataTable;            
            this.gvDtl.DataSource = dt;
            this.gvDtl.DataBind();
            Bind();
        }
    }

    protected void Bind()
    {
       if(KeyID>0)
       {
           Hi.Model.PAY_PaymentBank bankModel = new Hi.BLL.PAY_PaymentBank().GetModel(KeyID);

           this.lblDisUser.InnerText=bankModel.AccountName;//账户名称
           this.lblbankcode.InnerText = bankModel.bankcode;
           this.lblbankAddress.InnerText = bankModel.bankAddress;
           this.lblprivateCity.InnerText = bankModel.bankPrivate + "/" + bankModel.bankCity + "/" + bankModel.vdef1;           
           this.lblisno.InnerText = bankModel.Isno==1?"是":"否";
           this.lblremake.InnerText = bankModel.Remark;
           this.lblType.InnerText = GetType(bankModel.type);
           this.lblddlbank.InnerText = new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(bankModel.BankID.ToString());
           this.lblstart.InnerText = bankModel.Start == 1 ? "已复核" : "未复核";
           if(bankModel.Start==1)
           {
               this.Edit.Attributes.Add("style", "display:none;");
           }

           //根据收款银行ID 获取关联的代理商
           DataTable dtdis = new Hi.BLL.PAY_PaymentAccountdtl().GetDisBYpbID(KeyID);
           this.gvDtl.DataSource = dtdis;
           this.gvDtl.DataBind();
       }
        
    }

    /// <summary>
    /// 绑定订单明细
    /// </summary>
    public void BindOrderDetail(int DisId,int CompId)
    {
       
        
    }

    /// <summary>
    /// 绑定代理商地址
    /// </summary>
    public void BindAddr(int DisId, int AddrId)
    {
        
    }

   
    protected void gvDtl_DataBound(object sender, EventArgs e)
    {
        
    }

    
    /// <summary>
    /// 选择材料返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 
    protected void btnSelectMaterialsReturn_ServerClick(object sender, EventArgs e)
    {
        string str = this.txtMaterialCodes.Value;
        string strWhere = string.Empty;

        string GoodsId = this.txtGoodsCodes.Value;
        GoodsId += str;
        this.txtGoodsCodes.Value = GoodsId;

        if (str != "")
        {
            strWhere += " Id in (" + str.Substring(0, str.Length - 1) + ")";
        }

        List<Hi.Model.BD_Distributor> dbutorList = BDdbutorbll.GetList("", strWhere, "Id desc");
        AddMaterial(dbutorList);
    }

    /// <summary>
    /// 返回新增商品
    /// </summary>
    /// <param name="l"></param>
    private void AddMaterial(List<Hi.Model.BD_Distributor> dbutorList)
    {
        if (dbutorList.Count > 0)
        {
            DataTable dt = null;

            foreach (var item in dbutorList)
            {
                if (ViewState["Distributor"] == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("ID", typeof(string));
                    dt.Columns.Add("DisName", typeof(string));
                    dt.Columns.Add("DisCode", typeof(string));
                    dt.Columns.Add("DisLevel", typeof(string));
                    dt.Columns.Add("Address", typeof(string));
                    dt.Columns.Add("Principal", typeof(string));
                    dt.Columns.Add("AreaID", typeof(int));//区域

                    DataRow dr1 = dt.NewRow();
                    dr1["Id"] = item.ID; 
                    Hi.Model.BD_Distributor dbutorModel = BDdbutorbll.GetModel(item.ID);
                    if (dbutorModel != null)
                    {
                        dr1["DisName"] = dbutorModel.DisName;
                        dr1["DisCode"] = dbutorModel.DisCode;
                        dr1["DisLevel"] = dbutorModel.DisLevel;
                        dr1["Address"] = dbutorModel.Address;
                        dr1["Principal"] = dbutorModel.Principal;
                        dr1["AreaID"] = dbutorModel.AreaID;
                    }
                    
                    dt.Rows.Add(dr1);
                }
                else
                {
                    dt = ViewState["Distributor"] as DataTable;
                    DataRow dr2 = dt.NewRow();
                    dr2["Id"] = item.ID;                   
                    Hi.Model.BD_Distributor dbutorModel = BDdbutorbll.GetModel(item.ID);
                    if (dbutorModel != null)
                    {
                        dr2["DisName"] = dbutorModel.DisName;
                        dr2["DisCode"] = dbutorModel.DisCode;
                        dr2["DisLevel"] = dbutorModel.DisLevel;
                        dr2["Address"] = dbutorModel.Address;
                        dr2["Principal"] = dbutorModel.Principal;
                        dr2["AreaID"] = dbutorModel.AreaID;
                    }
                    dt.Rows.Add(dr2);
                }
                ViewState["Distributor"] = dt;
            }

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dt.DefaultView.Sort = "ID desc"; //dt排序
                }
            }

            this.gvDtl.DataSource = dt;
            this.gvDtl.DataBind();
        }
    }

   

    /// <summary>
    /// 删除商品
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void gvDtl_RowCommand(object source, GridViewCommandEventArgs e)
    {
        string type = e.CommandName;
        if (type == "del")
        {
            string Id = e.CommandArgument.ToString();
            DataTable dt = ViewState["Distributor"] as DataTable;

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Select(string.Format("Id='{0}'", Id))[0];

                dt.Rows.Remove(dr);
                string code = this.txtGoodsCodes.Value.ToString().Replace(Id + ",", "");  //删除商品Id
                this.txtGoodsCodes.Value = code;

                ViewState["Distributor"] = dt;

                
                DataTable dt1 = ViewState["Distributor"] as DataTable;

                gvDtl.DataSource = dt1;
                gvDtl.DataBind();
            }
        }
    }

    protected void gvDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    break;
                case DataControlRowType.Separator:
                    break;
                case DataControlRowType.Footer:
                   // ClientScript.RegisterStartupScript(this.GetType(), "", "<script>onQtyChanged()</script>");
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


    }
    /// <summary>
    /// 获取收款类型
    /// </summary>
    /// <param name="type">收款类型Id</param>
    /// <returns></returns>
    public string GetType(int type)
    {
        string str = string.Empty;
        switch (type)
        {
            case 11:
                str = "个人账户";
                break;
            case 12:
                str = "企业账户";
                break;
            case 20:
                str = "支付账户";
                break;

        }
        return str;
    }
   

}