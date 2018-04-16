using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

public partial class Company_Order_OrderZdtsAdd : CompPageBase
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

    //代理商Id
    public int DisId;
    //代理商地址Id
    public int AddrId;
    public string AddrName;

    public string page;

    Hi.BLL.DIS_Order OrderInfoBll = new Hi.BLL.DIS_Order();
    Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["action"] != null)
        {
            Action = Request["action"].ToString();
        }
        if (Action == "Addr")
        {
            //选择代理商地址
            DisId = Convert.ToInt32(Request["DisId"]);
            AddrId = Convert.ToInt32(Request["AddrId"]);
           
        }
        else if (Action == "GoodsInfo")
        {
            //修改商品信息 （价格，数量）
            int goodsinfoId = Request["goodsInfoId"].ToInt(0);  //商品信息Id
            decimal num = Request["num"].ToDecimal(0);  //商品数量
            string R =Common.NoHTML( Request["R"]);   //商品备注
            string Price =Common.NoHTML( Request["Price"]);  //商品价格

            string DisId =Common.NoHTML( Request["disId"]); //代理商
            string AddrId =Common.NoHTML( Request["AddrId"]);  //代理商收货地址

            
        }
        else if (Action == "AddFykm")
        {
            //新增费用科目
            string unit = Request["unit"];
            Response.Write(AddFykm(unit));
            Response.End();
        }

        if (!IsPostBack)
        {
            this.DisListID.CompID = this.CompID.ToString();
            this.hidCompId.Value = this.CompID.ToString();
           

            Common.BindUnit(this.rptUnit, "费用科目", this.CompID);//绑定单位下拉

            Bind();
        }
    }

    protected void Bind()
    {
       
        this.txtDisUser.InnerText = Common.GetUserName(this.UserID);
        this.hidDisUserId.Value = this.UserID.ToString();
        if (KeyID != 0)
        {
            if (!Common.PageCompOperable("Order", KeyID, CompID))
            {
                Response.Redirect("../../NoOperable.aspx");
                return;
            }

            Hi.Model.DIS_Order OrderInfoModel = OrderInfoBll.GetModel(KeyID);

            if (OrderInfoModel != null)
            {

                string js = "<script language=javascript>history.go(-1);</script>";
                //编辑订单时 判断订单状态
                if (OrderInfoModel.AddType == (int)Enums.AddType.企业补单 || OrderInfoModel.AddType == (int)Enums.AddType.App企业补单)
                {
                    //企业补单
                    if (OrderInfoModel.OState >= (int)Enums.OrderState.已审 && OrderInfoModel.PayState != (int)Enums.PayState.未支付)
                    {
                        HttpContext.Current.Response.Write(string.Format(js));
                    }
                }
                else
                {
                    //App下单 网页下单
                    if (OrderInfoModel.OState >= (int)Enums.OrderState.待审核)
                    {
                        HttpContext.Current.Response.Write(string.Format(js));
                    }
                }

                this.lblReceiptNo.InnerText = OrderInfoModel.ReceiptNo;

                DisId = OrderInfoModel.DisID;
                this.hidTotalAmount.Value = OrderInfoModel.TotalAmount.ToString("N");
                this.txtArriveDate.Value = OrderInfoModel.ArriveDate == DateTime.MinValue ? "" : OrderInfoModel.ArriveDate.ToString("yyyy-MM-dd");
                this.txtDisUser.InnerText = Common.GetUserName(OrderInfoModel.DisUserID);
                this.hidDisUserId.Value = OrderInfoModel.DisUserID.ToString();


                this.txtRemark.Value = OrderInfoModel.Remark;

            }
        }
        else
        {
            //清除商品数据
            SelectGoods.Clear();


        }

    }

    /// <summary>
    /// 生成订单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ReceiptNo = string.Empty;
        string Otype = string.Empty;
        string ArriveDate = string.Empty;
        decimal TotalAmount = 0;
        decimal OtherAmount = 0;
        string DisUser = string.Empty;
        string DisUserId = string.Empty;
        string LogRemark = string.Empty;  //日志备注

        string Remark = string.Empty;
        string str = string.Empty;

        if (this.DisListID.Disid == "")
        {
            str += " - 代理商不能为空。\\r\\n";
        }
        if (this.txtunit.Value == "")
            str += " - 费用名称不能为空。\\r\\n";
        if (this.txtOtherAmount.Value.ToDecimal(0) <= 0)
            str += " - 账单金额不能为0。\\r\\n";

        if (this.txtRemark.Value != "")
        {
            Remark =Common.NoHTML( this.txtRemark.Value.Trim().ToString());
            if (Remark.Length > 400)
            {
                str += " - 账单备注不能大于400个字符。\\r\\n";
            }
        }

        if (str != "")
        {
            JScript.AlertMsgOne(this, str, JScript.IconOption.错误, 2500);
            return;
        }

        try
        {
            Hi.Model.DIS_Order OrderInfoModel = null;
            DisId = Convert.ToInt32(this.DisListID.Disid);

            decimal Amount = Convert.ToDecimal(this.txtOtherAmount.Value);
            TotalAmount = Amount;

            if (this.txtArriveDate.Value.Trim() != "")
            {
                ArriveDate =Common.NoHTML( this.txtArriveDate.Value.Trim().ToString());
            }
            else
            {
                ArriveDate = DateTime.MinValue.ToString();
            }
            DisUser =Common.NoHTML( this.txtDisUser.InnerText.Trim().ToString());
            DisUserId =Common.NoHTML( this.hidDisUserId.Value.Trim().ToString());

            OrderInfoModel = OrderInfoBll.GetModel(KeyID);

            LogRemark += " 账单总价：" + Amount.ToString("N");



            #region 新增订单

            //新增订单
            String guid = Guid.NewGuid().ToString().Replace("-", "");

            ReceiptNo = SysCode.GetZD_NewCode("账单", 1);

            OrderInfoModel = new Hi.Model.DIS_Order();

            OrderInfoModel.GUID = guid;
            OrderInfoModel.CompID = this.CompID;
            OrderInfoModel.DisID = DisId;
            OrderInfoModel.ReceiptNo = ReceiptNo;


            int OState = 1;

            //无需审核
            OState = (int)Enums.OrderState.已审;
            OrderInfoModel.IsAudit = 1;

            OrderInfoModel.OState = OState;
            OrderInfoModel.Remark = Remark;
            OrderInfoModel.CreateUserID = this.UserID;
            OrderInfoModel.CreateDate = DateTime.Now;

            //总价
            OrderInfoModel.TotalAmount = TotalAmount;
            OrderInfoModel.AuditAmount = TotalAmount;
            OrderInfoModel.OtherAmount = OtherAmount;

            OrderInfoModel.ArriveDate = ArriveDate.ToDateTime();
            OrderInfoModel.DisUserID = DisUserId.ToInt(0);

            OrderInfoModel.Otype = (int)Enums.OType.推送账单;
            OrderInfoModel.AddType = (int)Enums.AddType.网页下单;

            OrderInfoModel.ts = DateTime.Now;
            //OrderInfoModel.vdef9 = "0";

            OrderInfoModel.vdef2 = this.txtunit.Value.Trim();//费用科目




            int OrderId = new Hi.BLL.DIS_Order().Add(OrderInfoModel);

            if (OrderId > 0)
            {
                Utils.AddSysBusinessLog(this.CompID, "Order", OrderId.ToString(), "账单新增", LogRemark);

                //代理商手机号
                string Phone = Common.GetDis(DisId, "Phone");
                string msg = "您的账单：" + ReceiptNo + "已经生成，" + this.txtunit.Value.Trim() + "：" + TotalAmount + "元，请尽快完成付款！[ " + Common.GetCompValue(CompID, "CompName") + " ]";

                //账单推送向代理商推送信息提示
                Common.GetPhone(Phone, msg);

                string Id = Common.DesEncrypt(OrderId.ToString(), Common.EncryptKey);

                ClientScript.RegisterStartupScript(this.GetType(), "add", "<script>window.location.href ='OrderZdtsInfo.aspx?KeyID=" + Id + "';</script>");

            }

            #endregion

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 添加费用科目
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected string AddFykm(string unit)
    {
        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            //新增数据字典
            Hi.Model.BD_DefDoc doc = new Hi.Model.BD_DefDoc();
            doc.CompID = this.CompID;
            doc.AtCode = "";
            doc.AtName = "费用科目";
            doc.ts = DateTime.Now;
            doc.modifyuser = UserID;
            doc.dr = 0;
            List<Hi.Model.BD_DefDoc> ll = new Hi.BLL.BD_DefDoc().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and atname='费用科目'", "");
            int defid = 0;
            if (ll.Count == 0)
            {
                defid = new Hi.BLL.BD_DefDoc().Add(doc, Tran);
            }
            else
            {
                defid = ll[0].ID;
            }
            if (defid != 0)
            {
                Hi.Model.BD_DefDoc_B doc2 = new Hi.Model.BD_DefDoc_B();
                doc2.CompID = this.CompID;
                doc2.DefID = defid;
                doc2.AtName = "费用科目";
                doc2.AtVal = unit; //txtunits.Value.Trim();
                doc2.ts = DateTime.Now;
                doc2.dr = 0;
                doc2.modifyuser = this.UserID;
                List<Hi.Model.BD_DefDoc_B> lll = new Hi.BLL.BD_DefDoc_B().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and atname='费用科目' and defid=" + defid + " and atval='" + unit + "'", "");
                if (lll.Count == 0)
                {
                    new Hi.BLL.BD_DefDoc_B().Add(doc2, Tran);
                }
                else
                {
                    return "[{\"AtVal\":\"ycz\"}]";
                }
            }
            Tran.Commit();
            List<Hi.Model.BD_DefDoc> l = new Hi.BLL.BD_DefDoc().GetList("", "AtName='费用科目' and compid=" + this.CompID + " and isnull(dr,0)=0", "");
            if (l.Count > 0)
            {
                List<Hi.Model.BD_DefDoc_B> llll = new Hi.BLL.BD_DefDoc_B().GetList("", "DefID=" + l[0].ID + "and ISNULL(dr,0)=0 and compid=" + this.CompID, "id desc");
                if (llll.Count > 0)
                {
                    DataTable dt = Common.FillDataTable(llll);
                    if (dt.Rows.Count != 0)
                    {
                        return ConvertJson.ToJson(dt);
                    }
                }
            }
            return "[{\"AtVal\":\"cc\"}]";
            // this.txtunit.Value = unit;
        }
        catch (Exception)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
            return "[{\"AtVal\":\"sb\"}]";
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
}