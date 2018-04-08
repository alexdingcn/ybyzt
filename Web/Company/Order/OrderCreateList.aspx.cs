using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

/**
 * 订单状态查询：
 *     申请退货：订单状态已到货（OState=5），订单退货状态为申请退货（ReturnState=2）；
 *     退货处理：订单状态已到货（OState=3），订单退货状态为申请退货（ReturnState!=2）；
 *     已退货：订单状态已到货（OState=7），订单退货状态为申请退货（ReturnState!=2）；
 * **/
public partial class Company_Order_OrderCreateList : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public string oId = string.Empty;
    public int PageSize = 12;

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
            if (!Common.HasRight(this.CompID,this.UserID, "1010")) 
                this.btnAdd.Visible = false;
            //不排除管理员的权限验证
            if (!Common.HasRightAll(this.CompID, this.UserID, "1030"))
                this.DelAll.Visible = false;
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
        string IDlist = string.Empty;//销售经理下属 员工ID集合
        
        if (DisSalesManID != 0)
        {
            if (Common.GetDisSalesManType(DisSalesManID.ToString(), out IDlist))
            {
                //销售经理
                strwhere = "and DisID in(select ID from BD_Distributor where smid in(" + IDlist + "))";
            }
            else
            {
                strwhere = "and DisID in(select ID from BD_Distributor where smid = " + DisSalesManID + ")";
            }
        }

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        #region
        if (Request.QueryString["type"] + "" == "2")
        {
            //待收款订单
            strwhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in(0,1) ) or( Otype<>1 and OState in (2,4,5) and PayState in(0,1))) and OState<>6   and CompID='" + this.CompID + "' and ReturnState=0 and Otype!=9 and isnull(dr,0)=0"; //IsDel=1  订单已删除
            this.ddrPayState.Value = "0";
            ddrOState.Attributes["disabled"] = "false";
        }
        else if (Request.QueryString["type"] + "" == "1")
        {
            //待审核
            strwhere += " and OState=1 and Otype<>9 and isnull(dr,0)=0 and CompID='" + this.CompID + "'";
            ddrOState.Value = "0";
            ddrOState.Attributes["disabled"] = "false";
        }
        else if (Request.QueryString["type"] + "" == "3")
        {
            //待发货
            strwhere += " and (OState=" + (int)Enums.OrderState.已审 + " or (OState=" + (int)Enums.OrderState.已发货 + " and IsOutState in (0,1,2))) and Otype<>9 and isnull(dr,0)=0 and CompID='" + this.CompID + "'";
            ddrOState.Value = "0";
            ddrOState.Attributes["disabled"] = "false";
        }
        else
        {
            strwhere += "and CompID=" + this.CompID + " and OState>=" + (int)Enums.OrderState.待审核 + "and Otype!=9 and isnull(dr,0)=0"; //IsDel=1  订单已删除
        }
        #endregion

        if (this.txtPager.Value.Trim().ToString() != "" && this.txtPager.Value.Trim().ToString() != "0")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                PageSize = Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }

        List<Hi.Model.DIS_Order> l = OrderInfoBLL.GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        foreach (var item in l)
        {
            oId += item.ID + ",";
        }
        oId += "0";

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
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" +Common.NoHTML( txtDisName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (this.txtReceiptNo.Value != "")
        {
            strWhere += "and ReceiptNo like '%" +Common.NoHTML( this.txtReceiptNo.Value.Trim().ToString().Replace("'", "''")) + "%'";
        }
        if (this.ddrOState.Value != "-2")
        {
            //if (this.ddrOState.Value == "33")
            //{
            //    strWhere += " and OState=5 and ReturnState=" + (int)Enums.ReturnState.申请退货;
            //}
            //else if (this.ddrOState.Value == "55")
            //{
            //    strWhere += " and OState=2 and ReturnState in(" + (int)Enums.ReturnState.申请退款 + "," + (int)Enums.ReturnState.退货退款 + ") and PayState in (" + (int)Enums.PayState.已结算 + "," + (int)Enums.PayState.已退款 + ")";
            //}
            //else if (this.ddrOState.Value == "71")
            //{
            //    strWhere += " and ReturnState=" + (int)Enums.ReturnState.拒绝退货;
            //}
            //else if (this.ddrOState.Value == "3" || this.ddrOState.Value == "7")
            //{
            //    strWhere += " and OState=" + this.ddrOState.Value + "and ReturnState not in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
            //}
            //else
            //{
               // strWhere += " and OState=" + this.ddrOState.Value + "and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
            //}
            var ostate = ddrOState.Value;
            if (ostate.ToInt(0) == 0)
                strWhere += " and OState in (1,2,3,4) and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
            else if (ostate.ToInt(0) == 1)
                strWhere += " and OState=5 and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
            else
                strWhere += " and OState=6 and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
        }
        if (this.ddrPayState.Value != "-1")
        {
            //if (this.ddrPayState.Value.Trim().ToString() == "0")
            //{
            //    if (this.ddrOState.Value == "-2")
            //    {
            //        //未选中订单状态  待收款订单查询条件
            //        strWhere += " and (OState not in (-1,0,1)  and PayState=0 ) and ReturnState in (0,1)";
            //    }
            //    else
            //    {
            //        //选中订单状态
            //        strWhere += " and PayState=" + this.ddrPayState.Value;
            //    }
            //}
            //else if (this.ddrPayState.Value.Trim().ToString() == "1") {
            //    strWhere += "and ReturnState in (0,1) and PayState=" + this.ddrPayState.Value;
            //}
            //else
            //{
            //    strWhere += " and PayState=" + this.ddrPayState.Value;
            //}
            strWhere += " and PayState=" + this.ddrPayState.Value;
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
            strWhere += " and AuditAmount<=" +Common.NoHTML( this.txtTotalAmount2.Value.Trim());
        }
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and CreateDate>='" +this.txtCreateDate.Value.Trim().ToDateTime() + "'";
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

    /// <summary>
    /// 批量删除订单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnVolumeDel_Click(object sender, EventArgs e)
    //{
    //    string Id = CB_SelAll();

    //    string msg = string.Empty;
    //    if (Id != "")
    //    {
    //        if (OrderInfoBLL.OrderDel(Id, out msg))
    //        {
    //            Bind();
    //        }
    //        if (msg != "")
    //        {
    //            JScript.AlertMsg(this, "订单:" + msg.Substring(0, msg.Length - 1) + "正在处理中，不能删除！");
    //        }
    //    }
    //}

    /// <summary>
    /// 审核退回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRAudit_Click(object sender, EventArgs e)
    {
        Response.Redirect("OrderCreateList.aspx");
    }

    /// <summary>
    /// 菜单操作
    /// </summary>
    /// <param name="Oid"></param>
    /// <param name="PageIndex"></param>
    /// <returns></returns>
    public string OrderMeau(string Oid)
    {
        string Str_Meau = "";
        Oid = Server.UrlDecode(Oid);
        string id = Common.DesDecrypt(Oid, Common.EncryptKey);

        if (id == "")
        {
            return "";
        }

        Hi.Model.DIS_Order OrderModel = OrderInfoBLL.GetModel(id.ToString().ToInt(0));

        if (OrderModel != null)
        {
            if (this.Erptype == 0)
            {
                //非U8、U9等用户  可以对订单进行操作
                if (OrderModel.OState == (int)Enums.OrderState.待审核)
                {
                    //if (Common.HasRight(this.CompID,this.UserID, "1012"))
                        //Str_Meau += "<a class='tablelinkQx' style='cursor: pointer;' onclick='return RegionAudit(\"" + Server.UrlEncode(Oid) + "\")'>审核</a>";
                    Str_Meau += "<a class='tablelinkQx' style='cursor: pointer;' href='../newOrder/orderdetail.aspx?top=1&KeyID=" + Server.UrlEncode(Oid) + "'>详情</a>";
                }
                //else if (OrderModel.OState == (int)Enums.OrderState.已发货){ }
                else if (OrderModel.OState == (int)Enums.OrderState.已审 && (OrderModel.PayState == (int)Enums.PayState.已支付 || OrderModel.PayState == (int)Enums.PayState.部分支付 || OrderModel.Otype == (int)Enums.OType.赊销订单) && OrderModel.ReturnState == (int)Enums.ReturnState.未退货)
                {
                    //if (Common.HasRight(this.CompID,this.UserID, "1013"))
                        //Str_Meau += "<a class='tablelinkQx' style='cursor: pointer;' onclick='return ship(\"" + Server.UrlEncode(Oid) + "\"," + OrderInfoType.JuOrder(id.ToInt(0)) + ")'>发货</a>";
                    Str_Meau += "<a class='tablelinkQx' style='cursor: pointer;' href='../newOrder/orderdetail.aspx?top=1&KeyID=" + Server.UrlEncode(Oid) + "'>详情</a>";
                }
                else if ((OrderModel.OState == (int)Enums.OrderState.已审) && (OrderModel.Otype != (int)Enums.OType.赊销订单 || OrderModel.PayState == (int)Enums.PayState.未支付) && OrderModel.ReturnState == (int)Enums.ReturnState.未退货)
                {
                    Str_Meau += "<a class='tablelinkQx' style='cursor: pointer;' href='../newOrder/orderdetail.aspx?top=1&KeyID=" + Server.UrlEncode(Oid) + "'>详情</a>";
                }
                else
                {
                    Str_Meau += "<a class='tablelinkQx' style='cursor: pointer;' href='../newOrder/orderdetail.aspx?top=1&KeyID=" + Server.UrlEncode(Oid) + "'>详情</a>";
                }
            }
            else
            {
                //U8、U9等用户  不能对订单进行操作
                Str_Meau += "<a class='tablelinkQx' style='cursor: pointer;' href='../newOrder/orderdetail.aspx?top=1&KeyID=" + Server.UrlEncode(Oid) + "'>详情</a>";
            }
        }

        return Str_Meau;
    }
}