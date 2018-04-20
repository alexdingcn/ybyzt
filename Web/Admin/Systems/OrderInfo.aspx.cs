using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;


public partial class Admin_Systems_OrderInfo : AdminPageBase
{
    //是否启用商品库存，默认启用库存
    public int IsInve = 0;
    //代理商ID
    public int DisID = 0;
    //订单下单数量保留小数位数
    public string Digits = "0";
    //是否启用返利
    public int isbate = 1;
    public int CompID = 0;
    //订单状态
    public int OState = 0;
    public int IsOutState = 1;
    //订单支付状态
    public int payState = 0;
    public int top = 1;

    public string TotalAmount = string.Empty;//订单总价
    public string PayedAmount = string.Empty;//已付金额
    public string paymoney = string.Empty;//未付款金额

    public string CreateDate = string.Empty;
    public string AuditDate = string.Empty;
    public string sendde = string.Empty;
    public string signde = string.Empty;
    public string fulfil = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            KeyID = Common.DesDecrypt(Request["KeyID"], Common.EncryptKey).ToInt(0);
            databind();
            IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompID).ToInt(0);

            hidCompID.Value = CompID.ToString();
            hidUserType.Value = 4.ToString();
            hidPicpath.Value = Common.GetPicBaseUrl(CompID.ToString());

            if (!string.IsNullOrEmpty(Request["top"]))
                top = Request["top"].ToInt(0);

            //if (top == 0)
            //    navigation2.Attributes.Add("href", "orderBuy.aspx");
            //else if (top == 2)
            //    navigation2.Attributes.Add("href", "../Order/OrderAuditList.aspx");
            //else if (top == 3)
            //    navigation2.Attributes.Add("href", "../Order/OrderShipList.aspx");
            //else if (top == 4)//线下付款成功，刷新页面
            //    ClientScript.RegisterStartupScript(this.GetType(), "payshowTop", "<script>TopShow();</script>");
        }
    }

    /// <summary>
    /// 订单信息
    /// </summary>
    public void databind()
    {
        if (KeyID != 0)
        {
            hidOrderID.Value = Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);
            string where = "and isnull(o.dr,0)=0 and o.otype<>9  and o.ID= " + KeyID;
            DataTable dt = new Hi.BLL.DIS_Order().GetList("", where);

            if (dt != null && dt.Rows.Count > 0)
            {
                CompID = dt.Rows[0]["CompID"].ToString().ToInt(0);
                DisID = dt.Rows[0]["DisID"].ToString().ToInt(0);
                hidDisID.Value = DisID.ToString();

                //判断改该条数据代理商是否有操作权限
                if (!Common.PageDisOperable("Order", KeyID, DisID))
                {
                    Response.Redirect("../NoOperable.aspx");
                    return;
                }

                //////再次购买
                ////buyagain.Visible = false;
                //////订单修改
                ////modifyorder.Visible = false;
                //订单作废
                //ordervoid.Visible = false;
                //////订单审核
                ////orderaudit.Visible = false;

                Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", dt.Rows[0]["CompID"].ToString().ToInt(0));
                hidDigits.Value = Digits;

                isbate = OrderInfoType.rdoOrderAudit("订单支付返利是否启用", CompID).ToInt(0);
                if (isbate == 0)
                {
                    trbate.Visible = false;
                    rebate.Visible = false;
                }

                OState = dt.Rows[0]["OState"].ToString().ToInt(0);
                if (OState == (int)Enums.OrderState.已作废)
                {
                    orderDelete.Visible = true;
                }
                else
                {
                    orderDelete.Visible = false;
                }
                IsOutState = dt.Rows[0]["IsOutState"].ToString().ToInt(0);
                payState = dt.Rows[0]["PayState"].ToString().ToInt(0);
                hidIsOutstate.Value = dt.Rows[0]["IsOutState"].ToString();
                hidOstate.Value = OState.ToString();
                hidpaystate.Value = payState.ToString();

                #region 订单操作按钮

                //switch (OState)
                //{
                //    case (int)Enums.OrderState.未提交:
                //        //modifyorder.Visible = true;
                //        //buyagain.Visible = true;
                //        //orderaudit.Visible = true;
                //        //ordervoid.Visible = true;
                //        deliver.Attributes.Add("style", "display: none;");
                //        break;
                //    case (int)Enums.OrderState.待审核:
                //        //modifyorder.Visible = true;
                //        //buyagain.Visible = true;
                //        //orderaudit.Visible = true;
                //        //ordervoid.Visible = true;
                //        deliver.Attributes.Add("style", "display: none;");
                //        break;
                //    case (int)Enums.OrderState.已审:
                //        //订单审核前，可以修改订单。
                //        buyagain.Visible = true;
                //        if (payState == 0)
                //        {
                //            //订单支付前订单可以作废，支付后不能作废订单
                //            ordervoid.Visible = true;
                //        }
                //        break;
                //    case (int)Enums.OrderState.退货处理:
                //        buyagain.Visible = true;
                //        fulfil = signde;
                //        break;
                //    case (int)Enums.OrderState.已发货:
                //        modifyorder.Visible = false;
                //        buyagain.Visible = true;
                //        break;
                //    case (int)Enums.OrderState.已到货:
                //        modifyorder.Visible = false;
                //        buyagain.Visible = true;
                //        fulfil = signde;
                //        break;
                //    case (int)Enums.OrderState.已作废:
                //        modifyorder.Visible = false;
                //        buyagain.Visible = true;
                //        break;
                //    case (int)Enums.OrderState.已退货:
                //        buyagain.Visible = true;
                //        fulfil = signde;
                //        break;
                //    default:
                //        break;
                //}

                #endregion

                // 订单编号
                lblReceiptNo.InnerText = dt.Rows[0]["ReceiptNo"].ToString();
                lblDisName.InnerText = Common.GetDisValue(dt.Rows[0]["DisID"].ToString().ToInt(0), "DisName").ToString();
                lblCreateDate.InnerText = dt.Rows[0]["CreateDate"].ToString().ToDateTime().ToString("yyyy-MM-dd");
                lblOstate.InnerText = OrderType.GetOState(dt.Rows[0]["OState"].ToString(), dt.Rows[0]["IsOutState"].ToString());

                //订单流程
                lblTotalAmount.InnerText = dt.Rows[0]["TotalAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["TotalAmount"].ToString().ToDecimal().ToString("N");
                lblProAmount.InnerText = dt.Rows[0]["ProAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["ProAmount"].ToString().ToDecimal().ToString("N");

                lblbateAmount.InnerText = dt.Rows[0]["bateAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["bateAmount"].ToString().ToDecimal().ToString("N");
                lblbate.InnerText = dt.Rows[0]["bateAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["bateAmount"].ToString().ToDecimal().ToString("0.00");

                lblPostFee.InnerText = dt.Rows[0]["PostFee"].ToString() == "" ? "0.00" : dt.Rows[0]["PostFee"].ToString().ToDecimal().ToString("0.00");
                lblAuditAmount.InnerText = dt.Rows[0]["AuditAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["AuditAmount"].ToString().ToDecimal().ToString("N");

                //下单信息
                lblArriveDate.InnerText = dt.Rows[0]["ArriveDate"].ToString() == "" ? "" : dt.Rows[0]["ArriveDate"].ToString().ToDateTime().ToString("yyyy-MM-dd");
                lblGiveMode.InnerText = dt.Rows[0]["GiveMode"].ToString();
                iRemark.InnerText = dt.Rows[0]["Remark"].ToString();

                //收货地址
                lblPrincipal.InnerText = dt.Rows[0]["Principal"].ToString();
                lblPhone.InnerText = dt.Rows[0]["Phone"].ToString();
                lblAddress.InnerText = dt.Rows[0]["Address"].ToString();
                hidAddrID.Value = dt.Rows[0]["AddrID"].ToString();

                //开票信息
                if (dt.Rows[0]["IsOBill"].ToString() == "1")
                {
                    hidDisAccID.Value = dt.Rows[0]["DisAccID"].ToString();
                    hidval.Value = dt.Rows[0]["IsOBill"].ToString();
                    string Billing = string.Empty;
                    Billing += "发票抬头：<label id=\"lblRise\" runat=\"server\">" + dt.Rows[0]["Rise"].ToString() + "</label>";
                    Billing += "，发票内容：<label id=\"lblContent\" runat=\"server\">" + dt.Rows[0]["Content"].ToString() + "</label>";
                    if (!string.IsNullOrEmpty(dt.Rows[0]["OBank"].ToString()))
                    {
                        Billing += "，开户银行：<label id=\"lblOBank\" runat=\"server\">" + dt.Rows[0]["OBank"].ToString() + "</label>";
                        Billing += "，开户账户：<label id=\"lblOAccount\" runat=\"server\">" + dt.Rows[0]["OAccount"].ToString() + "</label>";
                        Billing += "，纳税人登记号：<label id=\"lblTRNumber\" runat=\"server\">" + dt.Rows[0]["TRNumber"].ToString() + "</label>";
                    }
                    iInvoice.InnerHtml = Billing;

                    //lblRise.InnerText = dt.Rows[0]["Rise"].ToString();
                    //lblContent.InnerText = dt.Rows[0]["Content"].ToString();
                    //lblOBank.InnerText = dt.Rows[0]["OBank"].ToString();
                    //lblOAccount.InnerText = dt.Rows[0]["OAccount"].ToString();
                    //lblTRNumber.InnerText = dt.Rows[0]["TRNumber"].ToString();
                }
                else
                    this.iInvoice.InnerHtml = "不开发票";

                //发票信息
                lblBillNo.InnerText = dt.Rows[0]["BillNo"].ToString();
                lblIsBill.InnerText = dt.Rows[0]["IsBill"].ToString() == "1" ? "是" : "否";
                lblIsBill.Attributes.Add("tip", dt.Rows[0]["IsBill"].ToString());
                this.hidisBill.Value = dt.Rows[0]["IsBill"].ToString();


                #region  订单支付信息

                TotalAmount = Convert.ToDecimal(dt.Rows[0]["AuditAmount"]).ToString("0.00");
                PayedAmount = Convert.ToDecimal(dt.Rows[0]["PayedAmount"]).ToString("0.00");

                //订单已支付不显示支付按钮(未审核、未支付完成)
                if (Convert.ToInt32(dt.Rows[0]["OState"]) == 1 || (Convert.ToDecimal(dt.Rows[0]["AuditAmount"]) - Convert.ToDecimal(dt.Rows[0]["PayedAmount"]) == 0))
                    btn_pay.Visible = false;
                paymoney = (Convert.ToDecimal(dt.Rows[0]["AuditAmount"]) - Convert.ToDecimal(dt.Rows[0]["PayedAmount"])).ToString("0.00");

                //加密keyId
                desKeyID.Value = Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);


                //绑定支付流水信息
                DataTable paytable = new Hi.BLL.PAY_PrePayment().GetPayedItem(KeyID);
                rptmessage.DataSource = paytable;
                rptmessage.DataBind();


                //初始值加载
                txtArriveDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                DataTable dtpayment = new Hi.BLL.PAY_PrePayment().GetDate(" top 1   CreateDate, payName ,paycode ,paybank", "pay_payment", " DisID=" + DisID + " and vdef3=5 order by  CreateDate desc");
                foreach (DataRow dr in dtpayment.Rows)
                {
                    bankname.Value = Convert.ToString(dr["payName"]);
                    bankcode.Value = Convert.ToString(dr["paycode"]);
                    bank.Value = Convert.ToString(dr["paybank"]);
                }


                #endregion
                //附件
                if (dt.Rows[0]["Atta"].ToString() != "")
                {
                    StringBuilder li = new StringBuilder();
                    string[] atta = dt.Rows[0]["Atta"].ToString().Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    if (atta.Length > 0)
                    {
                        foreach (var item in atta)
                        {
                            string[] att = item.Split(new string[] { "^^" }, StringSplitOptions.RemoveEmptyEntries);
                            if (att.Length > 1)
                                li.AppendFormat("<li> <a href=\"{2}\" target=\"_blank\" class=\"name\">{0}（大小：{4}KB）</a><a href=\"javascript:;\"  class=\"bule del\" tip=\"{3}\" orderid=\"{1}\">删除</a><a href=\"{2}\" target=\"_blank\" class=\"bule\">下载</a></li>", att[0] + att[1].Substring(att[1].LastIndexOf(".")), KeyID, Common.GetWebConfigKey("ImgViewPath") + "OrderFJ/" + item, item, OrderType.GetSize(item));

                        }
                    }
                    ulAtta.InnerHtml = li.ToString();
                    this.hrOrderFj.Value = dt.Rows[0]["Atta"].ToString();
                }

                #region 订单明细

                //订单所有商品明细
                DataTable l = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " IsNUll(o.dr,0)=0 and o.OrderId=" + KeyID);
                if (l != null && l.Rows.Count > 0)
                {
                    rptOrderD.DataSource = l;
                    rptOrderD.DataBind();

                    //待发货商品
                    DataTable lno = SelectGoodsInfo.SreeenDataTable(l, "isnull(IsOut,0)=0"); //l.Select(string.Format("IsOut={0}", 0));
                    if (lno != null && lno.Rows.Count > 0)
                    {
                        txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                        rptIsout.DataSource = lno;
                        rptIsout.DataBind();
                    }
                    else
                        deliver.Attributes.Add("style", "display: none;");

                    //已发货商品
                    List<Hi.Model.DIS_OrderOutDetail> loud = new Hi.BLL.DIS_OrderOutDetail().GetList("", " isnull(dr,0)=0 and OrderID=" + KeyID, "");
                    //发货单 作废-- and o.IsAudit<>3 
                    DataTable lo = new Hi.BLL.DIS_OrderOut().GetList("", " isnull(o.dr,0)=0 and o.OrderID=" + KeyID + " Order by o.IsAudit");

                    if (loud != null && loud.Count > 0)
                    {
                        //outbind(lo, l, loud);

                        outGoods.InnerHtml = SelectGoodsInfo.outbind(lo, l, loud, Digits, 1);
                    }

                    #region 流程时间控件

                    //订单流程时间显示
                    //订单提交时间
                    CreateDate = dt.Rows[0]["CreateDate"].ToString() != "" ? dt.Rows[0]["CreateDate"].ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm") : "";

                    //订单审核时间
                    AuditDate = dt.Rows[0]["AuditDate"].ToString() != "" ? dt.Rows[0]["AuditDate"].ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm") : "";

                    if (lo != null && lo.Rows.Count > 0)
                    {
                        if (((int)Enums.OrderState.已发货 <= OState || OState == (int)Enums.OrderState.退货处理) && (IsOutState == 3 || IsOutState == 4))
                        {
                            //订单发货时间
                            DataRow[] sendlo = lo.Select("IsAudit<>3");
                            DataRow[] dv = sendlo.OrderByDescending(x => x["id"]).ToArray();
                            if (dv.Length > 0)
                                sendde = dv[0]["SendDate"].ToString() != "" ? dv[0]["SendDate"].ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm") : "";
                        }
                        if ((int)Enums.OrderState.已到货 <= OState || OState == (int)Enums.OrderState.退货处理)
                        {
                            //订单签收时间
                            DataRow[] signlo = lo.Select("IsAudit<>3 and IsSign=1");
                            DataRow[] dr = signlo.OrderByDescending(x => x["id"]).ToArray();
                            if (dr.Length > 0)
                                signde = dr[0]["SignDate"].ToString() != "" ? dr[0]["SignDate"].ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm") : "";
                        }
                    }
                    #endregion
                }
                #endregion



            }
            else
            {
                Response.Redirect("../NoOperable.aspx");
                return;
            }
        }
    }



    /// <summary>
    /// 判断是否是线下支付类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string Pretype(string type, string paymentid)
    {
        string str = string.Empty;

        if (type == "线下支付")
        {
            if (new Hi.BLL.PAY_Payment().GetModel(Convert.ToInt32(paymentid)).IsAudit == 1)
                str = string.Format("<a href='javascript:void(0)' onclick='TovoidPay({0})'> 作废 </a>", paymentid);

        }

        return str;

    }



    /// <summary>
    /// 商品备注
    /// </summary>
    ///  <param name="ID">订单明细ID</param>
    /// <param name="Remark">商品备注</param>
    /// <returns></returns>
    public string goodsRemark(string ID, string Remark)
    {
        string str = "";

        int ostate = hidOstate.Value.ToInt(0);

        if (Remark != "")
        {
            if (ostate < 3)
            {
                str = "<a href=\"javascript:;\" tip=\"" + ID + "\" class=\"addRemark\">编辑</a>";
                str += "<div class=\"subRemark\">" + Common.MySubstring(Remark, 6, "...") + "</div><div class=\"cur\">" + Remark + "</div>";
            }
            else
            {
                str = "<div class=\"subRemark\">" + Common.MySubstring(Remark, 6, "...") + "</div><div class=\"cur\">" + Remark + "</div>";
            }
        }
        else
        {
            if (ostate < 3)
            {
                str = "<a href=\"javascript:;\" tip=\"" + ID + "\" class=\"addRemark\">添加</a><div class=\"subRemark\"></div>";
            }
        }

        return str;
    }

}