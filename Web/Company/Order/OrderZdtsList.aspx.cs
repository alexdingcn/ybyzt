using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/**
 * 订单状态查询：
 *     申请退货：订单状态已到货（OState=5），订单退货状态为申请退货（ReturnState=2）；
 *     退货处理：订单状态已到货（OState=3），订单退货状态为申请退货（ReturnState!=2）；
 *     已退货：订单状态已到货（OState=7），订单退货状态为申请退货（ReturnState!=2）；
 * **/
public partial class Company_Order_OrderZdtsList : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id

    protected void Page_Load(object sender, EventArgs e)
    {
        //page = Request["page"] + "";
        //this.Pager.CurrentPageIndex = page.ToInt(0);

        if (!IsPostBack)
        {
            this.hidCompId.Value = this.CompID.ToString();
            this.txtPager.Value = Common.PageSize;
            //this.txtCreateDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            //this.txtEndCreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

            Bind();
        }
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        int pageCount=0;
        int Counts=0;
        string strwhere = string.Empty;

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        if (Request.QueryString["type"] + "" == "2")
        {
            //待收款订单
            strwhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in(0,1) ) or( Otype<>1 and OState in (2,4,5) and PayState in(0,1))) and OState<>6   and CompID='" + this.CompID + "' and ReturnState=0 and isnull(dr,0)=0"; //IsDel=1  订单已删除
            this.ddrPayState.Value = "0";
        }
        else
        {
            strwhere += "and CompID=" + this.CompID + " and OState>=" + (int)Enums.OrderState.待审核 + " and otype=9  and isnull(dr,0)=0"; //IsDel=1  订单已删除
        }

        if (this.txtPager.Value.Trim().ToString() != "" && this.txtPager.Value.Trim().ToString() != "0")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }

        List<Hi.Model.DIS_Order> l = OrderInfoBLL.GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = l;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        ViewState["strwhere"] = Where();
        Bind();
    }

    protected void rptOrder_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string type=e.CommandName;
        Id = Convert.ToInt32(e.CommandArgument);

        if (type == "del")
        {
            Hi.Model.DIS_Order OrderInfoModel = OrderInfoBLL.GetModel(Id);

             if (OrderInfoModel != null)
             {
                 if (OrderInfoModel.OState == (int)Enums.OrderState.退回 || OrderInfoModel.OState == (int)Enums.OrderState.未提交)
                 {
                     OrderInfoModel.dr = 1;
                     bool falg = OrderInfoBLL.OrderDel(OrderInfoModel);
                     if (falg)
                     {
                         //JScript.ShowAlert(this, "");
                         Bind();
                     }
                 }
                 else
                 {
                     JScript.AlertMsgOne(this, "订单处理中，不能删除！", JScript.IconOption.错误, 2500);
                 }
             }
        }
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }

    public string Where()
    {
        string strWhere = string.Empty;

        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" +Common.NoHTML( txtDisName.Value.Trim().Replace("'","") )+ "%')";
        }
        if (this.txtReceiptNo.Value != "")
        {
            strWhere += "and ReceiptNo like '%" +Common.NoHTML( this.txtReceiptNo.Value.Trim().Replace("'", "")) + "%'";
        }
        
        if (this.ddrPayState.Value != "-1")
        {
            if (this.ddrPayState.Value.Trim().ToString() == "0")
            {
                //if (this.ddrOState.Value == "-2")
                //{
                //    //未选中订单状态  待收款订单查询条件
                //    strWhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState=0 ) or( Otype<>1 and OState=2 and PayState=0 )) and ReturnState in (0,1) and OState<>6";
                //}
                //else
                //{
                    //选中订单状态
                    strWhere += " and PayState=" + this.ddrPayState.Value;
                //}
            }
            else
            {
                strWhere += " and PayState=" + this.ddrPayState.Value;
            }

        }
        //if (this.ddrAddType.Value != "-1")
        //{
        //    strWhere += " and AddType=" + this.ddrAddType.Value;
        //}
        //if (this.ddrOtype.Value != "-1")
        //{
        //    strWhere += " and Otype=" + this.ddrOtype.Value;
        //}
        if (this.txtTotalAmount1.Value != "")
        {
            strWhere += " and AuditAmount>=" +Common.NoHTML( this.txtTotalAmount1.Value.Trim());
        }
        if (this.txtTotalAmount2.Value != "")
        {
            strWhere += " and AuditAmount<" +Common.NoHTML( this.txtTotalAmount2.Value.Trim());
        }
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and CreateDate>'" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and CreateDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }

    /// <summary>
    /// 获取选中的订单
    /// </summary>
    /// <returns></returns>
    public string CB_SelAll()
    {
        string strId = string.Empty;

        foreach (RepeaterItem item in rptOrder.Items)
        {
            CheckBox cb = item.FindControl("CB_SelItem") as CheckBox;
            if (cb != null && cb.Checked == true)
            {
                HiddenField fld = item.FindControl("Hd_Id") as HiddenField;

                if (fld != null)
                {
                    int id = Convert.ToInt32(fld.Value);
                    strId += id + ",";
                }
            }
        }
        if (strId != "")
        {
            strId = strId.Substring(0, strId.Length - 1);
        }
        return strId;
    }
}