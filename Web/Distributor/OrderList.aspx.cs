
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class Distributor_OrderList : DisPageBase
{
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    Hi.Model.BD_Distributor dis = null;
    public string oId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);

        if (!IsPostBack)
        {
            //if (!Common.HasRight(this.UserID, "2010"))
            //    this.btnAdd.Visible = false;

            //if (!string.IsNullOrEmpty(Request["Otype"]) && Request["Otype"].ToString() == "1")
            //{
            //    this.ddlOType.SelectedValue = "1";
            //}

            string strwhere = string.Empty;
            if (Request["t"] != null)
            {
                //获取当前时间
                DateTime date = DateTime.Now;
                //当月第一天
                DateTime day1 = new DateTime(date.Year, date.Month, 1);
                //获取当前时间加一天
                DateTime Sday = date.AddDays(1);
                //当天0点0分
                DateTime day0 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

                if (Request["t"] + "" == "1")
                {
                    strwhere += " and ReturnState=0 and OState in (1,2,4,5) and CreateDate>='" + day0 + "'";

                    this.txtArriveDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else if (Request["t"] + "" == "2")
                {
                    this.txtArriveDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                    this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    strwhere += " and ReturnState=0 and OState in (1,2,4,5) and CreateDate>='" + day1 + "'";
                }
            }
            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());
            ViewState["strwhere"] = strwhere;

            //user = LoginModel.IsLogined(this);
            //AddTypeBind();
        }
       
        //if (user != null)
        //{
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
        }
        //}
        if (Request.QueryString["type"] != null)
        {
            if (Request.QueryString["type"].ToString() == "present")
            {
                present(Request["ReceiptNo"].ToString());
            }
            if (Request.QueryString["type"].ToString() == "affirm")
            {
                affirm(Request["ReceiptNo"].ToString());
            }
            if (Request.QueryString["type"].ToString() == "Del")
            {
                Del(Request["oid"].ToString());
            }
        }

        if (!Common.HasRight(this.CompID, this.UserID, "2010", this.DisID))
            this.btnAdd.Visible = false;
        if (!Common.HasRight(this.CompID, this.UserID, "2011", this.DisID))
        {
            this.rptOrder.Visible = false;
            this.Pager.Visible = false;
        }
        //this.ddrComp.DataSource = 
        //this.ddrComp.DataTextField = "CompName";
        //this.ddrComp.DataValueField = "id";
        //this.ddrComp.DataBind();

    }

    //public void AddTypeBind() {
    //    ddlAddtype.Items.Add(new ListItem("全部", "-1"));
    //    foreach (Enums.AddType a in Enum.GetValues(typeof(Enums.AddType))) {
    //        ddlAddtype.Items.Add(new ListItem(a.ToString(), ((int)a).ToString()));
    //    }
    //}

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
        strwhere += "and Otype!=9 and isnull(dr,0)=0 and disid=" + this.DisID;  //and PayState in (0,2,7)
        if (this.ddrComp.Value != "")
            strwhere += " and CompID=" + this.ddrComp.Value; //add by hgh

        //if (ddlOType.SelectedValue != "-1")
        //{
        //    strwhere += " and otype=" + ddlOType.SelectedValue + "";
        ////}

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


        List<Hi.Model.DIS_Order> orders = new Hi.BLL.DIS_Order().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);
        this.rptOrder.DataSource = orders;
        this.rptOrder.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        oId = string.Join(",", orders.Select(T => T.ID));
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    public void btnSearch_Click(object sender, EventArgs e)
    {
        string strwhere = string.Empty;
        if (!string.IsNullOrEmpty(orderid.Value))
        {
            strwhere += " and receiptno like ('%" + Common.NoHTML(orderid.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (ddrOState.Value != "-2")
        {
            var ostate = ddrOState.Value;
            if (ostate.ToInt(0) == 0)
                strwhere += " and OState in (1,2,3,4) and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
            else if (ostate.ToInt(0) == 1)
                strwhere += " and OState=5 and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
            else
                strwhere += " and OState=6 and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";

        }
        if (ddrPayState.Value != "-1")
        {
            strwhere += " and PayState=" + Common.NoHTML(ddrPayState.Value.Trim());
        }
        if (txtArriveDate.Value.Trim() != "")
        {
            strwhere += "and CreateDate>='" + txtArriveDate.Value.Trim() + "'";
        }
        if (txtArriveDate1.Value.Trim() != "")
        {
            strwhere += "and CreateDate<'" + this.txtArriveDate1.Value.Trim().ToDateTime().AddDays(1) + "'";
        }
        //if (ddlAddtype.Items.Count > 0)
        //{
        //    if (ddlAddtype.SelectedValue != "-1")
        //    {
        //        strwhere += " and addtype='" + ddlAddtype.SelectedValue + "'";
        //    }
        //}
        if (this.txtTotalAmount1.Value != "")
        {
            strwhere += " and AuditAmount>=" + this.txtTotalAmount1.Value.Trim();
        }
        if (this.txtTotalAmount2.Value != "")
        {
            strwhere += " and AuditAmount<=" + this.txtTotalAmount2.Value.Trim();
        }

        //if (ddrReturnState.Value != "-1")
        //{
        //    strwhere += " and ReturnState=" + ddrReturnState.Value.Trim();
        //}
        ViewState["strwhere"] = strwhere;
        Bind();
    }

    /// <summary>
    /// 提交订单
    /// </summary>
    public void present(string ReceiptNo)
    {
        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(ReceiptNo);
        if (order != null)
        {
            if (order.IsAudit == 1)
            {
                order.OState = (int)Enums.OrderState.已审;
                order.AuditDate = DateTime.Now;
            }
            else
            {
                order.OState = (int)Enums.OrderState.待审核;
            }
            order.ts = DateTime.Now;
            order.modifyuser = this.UserID;
            if (new Hi.BLL.DIS_Order().Update(order))
            {
                Utils.AddSysBusinessLog(dis.CompID, "Order", orderid.ToString(), "提交订单", "");
                string str = "\"str\":true";
                str = "{" + str + "}";
                Response.Write(str);
                Response.End();
            }
        }
    }

    /// <summary>
    /// 确认到货
    /// </summary>
    public void affirm(string ReceiptNo)
    {
        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(ReceiptNo);
        if (order.OState == 1 || order.OState == 2 || order.OState == 4)
        {
            order.OState = (int)Enums.OrderState.已到货;
            order.ts = DateTime.Now;
            order.modifyuser = this.UserID;
            if (new Hi.BLL.DIS_Order().Update(order))
            {
                Hi.Model.DIS_OrderOut orderout = new Hi.BLL.DIS_OrderOut().GetOutModel(order.ID);
                if (orderout != null)
                {
                    orderout.SignDate = DateTime.Now;
                    orderout.IsSign = 1;
                    orderout.SignUserId = this.UserID;
                    orderout.SignUser = this.UserName;
                    orderout.ts = DateTime.Now;
                    orderout.modifyuser = this.UserID;
                    if (new Hi.BLL.DIS_OrderOut().Update(orderout))
                    {
                        Utils.AddSysBusinessLog(dis.CompID, "Order", order.ID.ToString(), "签收", "");
                        string str = "\"str\":true";
                        str = "{" + str + "}";
                        Response.Write(str);
                        Response.End();
                    }
                }
                else
                {
                    string str = "\"str\":fales";
                    str = "{" + str + "}";
                    Response.Write(str);
                    Response.End();
                }
            }
        }
        else
        {
            string str = "\"str\":fales";
            str = "{" + str + "}";
            Response.Write(str);
            Response.End();
        }
    }

    public void Del(string oid)
    {
        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(int.Parse(oid));
        order.OState = (int)Enums.OrderState.已作废;
        order.ts = DateTime.Now;
        order.modifyuser = this.UserID;
        if (new Hi.BLL.DIS_Order().Update(order))
        {
            Utils.AddSysBusinessLog(dis.CompID, "Order", order.ID.ToString(), "取消订单", "");
            string str = "\"str\":true";
            str = "{" + str + "}";
            Response.Write(str);
            Response.End();
        }
    }

    /// <summary>
    /// 根据不同的状态，显示不同的操作按钮
    /// </summary>
    /// <param name="state"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string Getmessage(object Ostate, int state, object ReturnState, int id)
    {
        string str = string.Empty;
        string strDB = string.Empty;

        if (Ostate.ToString() == "2" || Ostate.ToString() == "4" || (Ostate.ToString() == "5" && (ReturnState.ToString() == "0" || ReturnState.ToString() == "1")))
        {
            if (state == 0)
            {
                str = string.Format("<a href='javascript:void(0)' onclick=\'pay(\"{0}\")\'  class=\"a-red\">立即支付</a> ", Common.DesEncrypt(id.ToString(), Common.EncryptKey));

                //判断是否开启担保支付
                //if (ConfigurationManager.AppSettings["IsDBPay"].ToString() == "1")
                //    strDB = string.Format("<a href='javascript:void(0)' onclick=\'payDB(\"{0}\")\' >担保支付</a>", Common.DesEncrypt(id.ToString(), Common.EncryptKey));
            }
            else if (state == 1)
            {
                str = string.Format("<a href='javascript:void(0)' onclick=\'pay(\"{0}\")\' class=\"a-red\">立即支付</a>", Common.DesEncrypt(id.ToString(), Common.EncryptKey));


                //判断是否开启担保支付
                //if (ConfigurationManager.AppSettings["IsDBPay"].ToString() == "1")
                //    strDB = string.Format("<a href='javascript:void(0)' onclick=\'payDB(\"{0}\")\'>担保支付</a>", Common.DesEncrypt(id.ToString(), Common.EncryptKey));

            }
        }
        return str + strDB;
    }
}