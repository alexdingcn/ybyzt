using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_Pay_PayOrderList : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;
            //this.txtCreateDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            //this.txtEndCreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewState["strwhere"] = Where();
            Bind();
        }
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        strwhere += "and CompID=" + CompID + " and Otype!=9  and OState in (2,3,4,5,7) and isnull(vdef1,0) in (0,1)  and isnull(dr,0)=0 and  (PayState in (0,1)  or  (PayState=2 and ID in (select OrderID from pay_prepayment where PreType=7)))"; //IsDel=1  订单已删除

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
        string type = e.CommandName;
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
                    JScript.AlertMsgOne(this, "订单处理中，不能删除！", JScript.IconOption.错误);
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
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" +Common.NoHTML( txtDisName.Value.Trim().Replace("'", "")) + "%')";
        }
        if (this.txtReceiptNo.Value != "")
        {
            strWhere += "and ReceiptNo like '%" +Common.NoHTML( this.txtReceiptNo.Value.Trim().Replace("'", "")) + "%'";
        }
        //if (this.ddrOState.Value != "-2")
        //{
        //    if (this.ddrOState.Value == "33")
        //    {
        //        strWhere += " and OState=5 and ReturnState!=0";
        //    }
        //    else if (this.ddrOState.Value == "55")
        //    {
        //        strWhere += " and OState=2 and ReturnState in(" + (int)Enums.ReturnState.申请退款 + "," + (int)Enums.ReturnState.退货退款 + ") and PayState in (" + (int)Enums.PayState.已结算 + "," + (int)Enums.PayState.已退款 + ")";
        //    }
        //    else
        //    {
        //        strWhere += " and OState=" + this.ddrOState.Value + "and ReturnState=" + (int)Enums.ReturnState.未退货;
        //    }
        //}
        //if (this.ddrPayState.Value != "-1")
        //{
        //    strWhere += " and PayState=" + this.ddrPayState.Value;
        //}
        //if (this.ddrAddType.Value != "-1")
        //{
        //    strWhere += " and AddType=" + this.ddrAddType.Value;
        //}
        //支付状态
        if (this.ddrPaytype.Value != "-1")
        {
            strWhere += " and PayState=" + this.ddrPaytype.Value;
        }
        //订单类型
        //if (this.ddrOtype.Value != "-1")
        //{
        //    strWhere += " and Otype=" + this.ddrOtype.Value;
        //}
        if (this.txtTotalAmount1.Value != "")
        {
            strWhere += " and TotalAmount>=" +Common.NoHTML( this.txtTotalAmount1.Value.Trim());
        }
        if (this.txtTotalAmount2.Value != "")
        {
            strWhere += " and TotalAmount<" +Common.NoHTML( this.txtTotalAmount2.Value.Trim());
        }
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and CreateDate>'" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += " and CreateDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }
        //订单来源
        //if (this.ddraddtypess.Value != "-1")
        //{
        //    strWhere += " and addtype =" + this.ddraddtypess.Value;
        //}

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

    /// <summary>
    /// 批量删除订单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVolumeDel_Click(object sender, EventArgs e)
    {
        string Id = CB_SelAll();

        string msg = string.Empty;
        if (Id != "")
        {
            if (OrderInfoBLL.OrderDel(Id, out msg))
            {
                Bind();
            }
            if (msg != "")
            {
                JScript.AlertMsgOne(this, "订单:" + msg.Substring(0, msg.Length - 1) + "正在处理中，不能删除！", JScript.IconOption.错误);
            }
        }
    }

    /// <summary>
    /// 根据不同的状态，显示不同的操作按钮
    /// </summary>
    /// <param name="Ostate"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string Getmessage(int Ostate,int id,int paystate)
    {
        string str = string.Empty;

        if (paystate != 2)
        {
            str = string.Format("<a href=\'javascript:void(0)\' onclick=\'pay({0},{1})\' class=\"tablelinkQx\">订单补录</a>", id, Ostate);

        }
        else
        {
            DataTable dt = new Hi.BLL.PAY_PrePayment().GetDate("ID", "pay_prepayment", "PreType=7 and OrderID="+id);
            if(dt.Rows.Count>0)
                id=Convert.ToInt32(dt.Rows[0][0]);
            str += string.Format("<a href=\'javascript:void(0)\' onclick=\'goPreInfo({0})\' class=\"tablelinkQx\">补录详情</a>", id);
        }
        //}
        return str;
    }
}