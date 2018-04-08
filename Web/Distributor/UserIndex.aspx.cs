
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Configuration;

public partial class Distributor_UserIndex : DisPageBase
{
    Hi.Model.SYS_Users user = null;
    public int dayOrderCount = 0;    //当天订单数量
    public string DaySum = string.Empty;     //当天订购额
    public string DayPaymentSum = string.Empty;   //当天付款额
    public int payCount = 0;  //待支付订单数
    public int payzdCount = 0; //待支付账单数
    public int ReceiveCount = 0; //待收货订单数
    public int salesorder = 0;   //赊销订单
    public int message = 0;    //问题回复
    public string price = string.Empty;            //企业钱包金额
    public string disType = string.Empty;      //代理商分类
    public string disAreaID = string.Empty;    //代理商区域
    public int orderCount = 0;    //本月订单数量
    public string MonthSum = string.Empty;  //本月订购额
    public string PaymentSum = string.Empty; //本月付款额
    public string PayableSum = string.Empty;  //本月应款额
    public string Cx_Sum = string.Empty;//促销商品
    public string Order_html = string.Empty;//待支付订单
    public string goods_Sum = "0";  //快过期商品
    public double sum = 0.00D;

    public Hi.Model.BD_Distributor dis = null;
    public List<Hi.Model.DIS_Order> orderl = null;
    public List<Hi.Model.DIS_Order> orderll = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!(Session["UserModel"] as LoginModel).IsExistRole)
        {
            Response.Redirect("~/Distributor/UserEdit.aspx", true);
        }

        //获取当前时间
        DateTime date = DateTime.Now;
        //当天0点0分
        DateTime day0 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        //获取当前时间加一天
        DateTime Sday = day0.AddDays(1);

        //当月第一天
        DateTime day1 = new DateTime(date.Year, date.Month, 1);
        //本月  最后一天  多加一天
        DateTime mothday = day1.AddMonths(1);

        user = new Hi.BLL.SYS_Users().GetModel(this.UserID);

        orderl = new Hi.BLL.DIS_Order().GetList("", " isnull(dr,0)=0 and Otype<>9 and OState<>6  and CompID=" + this.CompID + " and DisID=" + this.DisID, " CreateDate desc");
        if (orderl != null)
        {
            #region   
            
            //当天订单数
            dayOrderCount = orderl.FindAll(
                p => (p.OState == 2 || p.OState == 3 || p.OState == 4 || p.OState == 5 || p.OState == 7) && p.CreateDate >= day0 && p.CreateDate < Sday).Count;

            //本月订单数
            orderCount = orderl.FindAll(
                p => (p.OState == 2 || p.OState == 3 || p.OState == 4 || p.OState == 5 || p.OState == 7) && p.CreateDate >= day1 && p.CreateDate < mothday).Count;

            #endregion
            orderll = orderl.FindAll(p => (p.PayState == 0 || p.PayState == 1));

        }
        List<Hi.Model.DIS_Suggest> suggest = new Hi.BLL.DIS_Suggest().GetList("", " isnull(dr,0)=0 and Stype=0 and isanswer=1 and DisUserID=" + this.UserID, "");
        message = suggest.Count;
        if (message == 0)
        {
            //imessage.Attributes.Add("style", "display:none");
        }
        price = Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(this.DisID, this.CompID)).ToString("0.00");
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (dis != null)//add by hgh  出现为null值
        {
            //if ( dis.CreditType == 0)
            //{
            //lisalesorder.Attributes.Add("style", "background-color:#fbfbfb");
            //}

            Hi.Model.BD_DisType distypemodel = new Hi.BLL.BD_DisType().GetModel(dis.DisTypeID);
            if (distypemodel != null)
            {
                disType = distypemodel.TypeName;
            }
            Hi.Model.BD_DisArea disaddrmodel = new Hi.BLL.BD_DisArea().GetModel(dis.AreaID);
            if (disaddrmodel != null)
            {
                disAreaID = disaddrmodel.AreaName;
            }
        }
        #region 促销商品

        string Cx_sql = string.Format(@"SELECT count(1) num from BD_Goods
where ID in (SELECT GoodsID from BD_PromotionDetail where ProID in
(SELECT ID from BD_Promotion where compID={0} and ProStartTime<=getdate() 
and ProStartTime<=GETDATE() and DATEADD(D,1,ProEndTime)>= getDate() and IsEnabled=1	))and ID not in (	
select GoodsID from BD_GoodsAreas where	compID={0} and DisID={1} and isnull(dr,0)=0)
and compid= {0} and ISNULL(dr,0)=0 and IsEnabled = 1 and IsOffLine=1 ", this.CompID, this.DisID);
        DataTable Cx_Dt = SqlHelper.Query(SqlHelper.LocalSqlServer, Cx_sql).Tables[0];
        if (Cx_Dt != null)
        {
            if (Cx_Dt.Rows.Count > 0)
            {
                decimal sum_CX = Convert.ToDecimal(Cx_Dt.Rows[0]["num"]);
                Cx_Sum = sum_CX.ToString();
            }
        }

        #endregion

        #region  快过期商品 goods_Sum
        //当前时间
        DateTime now = DateTime.Now;
        //快到期时间
        DateTime today = now.AddDays(30);

        string goods_sql = string.Format(@"select * from YZT_GoodsStock s where s.validDate<='" + today + "'" + " and DisID=" + DisID + " and dr=0 ");

        DataTable goods_Dt = SqlHelper.Query(SqlHelper.LocalSqlServer, goods_sql).Tables[0];
        if (goods_Dt != null && goods_Dt.Rows.Count > 0)
        {
            goods_Sum = goods_Dt.Rows.Count.ToString();
        }
        #endregion

        #region 当天订购额
        //add by hgh  //包含已退订单金额、退单处理
        string daysql = "SELECT SUM(AuditAmount) as AuditAmount FROM [dbo].[DIS_Order] where ostate in (2,3,4,5,7) and DisID=" +
                          this.DisID + " and CompID=" + this.CompID + " and CreateDate>='" + day0 + "' and CreateDate<'" + Sday + "'";
        DataTable dayDt = SqlHelper.Query(SqlHelper.LocalSqlServer, daysql).Tables[0];
        if (dayDt != null)
        {
            if (dayDt.Rows.Count > 0)
            {
                decimal sumAmount = dayDt.Rows[0]["AuditAmount"].ToString() == ""
                    ? sum.ToString().ToDecimal()
                    : Convert.ToDecimal(dayDt.Rows[0]["AuditAmount"]);
                DaySum = (sumAmount).ToString("N");
            }
        }
        #endregion

        #region 本月订购额
        string monthsql = "SELECT SUM(AuditAmount) as AuditAmount FROM DIS_Order where OState in (2,3,4,5,7) and isnull(dr,0)=0 and Otype<>9 and DisID=" + this.DisID + " and CompID=" + this.CompID + " and CreateDate>='" + day1 + "' and CreateDate<'" + mothday + "'";
        DataTable monthDt = SqlHelper.Query(SqlHelper.LocalSqlServer, monthsql).Tables[0];
        if (monthDt != null)
        {
            if (monthDt.Rows.Count > 0)
            {
                decimal sumAmount = monthDt.Rows[0]["AuditAmount"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(monthDt.Rows[0]["AuditAmount"]);
                MonthSum = (sumAmount).ToString("N");
            }
        }
        #endregion

        #region 当天付款额
        //付款额    add by hgh  CompCollection_view 状态   and status!=3
        string daypaggersql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where DisID=" + this.DisID +
                           "and CompID=" + this.CompID + " and status!=3 and Date>='" + day0 + "' and Date<'" + Sday + "' AND vedf9=1 ";

        DataTable daypaggerdt = SqlHelper.Query(SqlHelper.LocalSqlServer, daypaggersql).Tables[0];
        if (daypaggerdt != null)
        {
            if (daypaggerdt.Rows.Count > 0)
            {
                decimal Price = daypaggerdt.Rows[0]["Price"].ToString() == ""
                    ? sum.ToString().ToDecimal()
                    : Convert.ToDecimal(daypaggerdt.Rows[0]["Price"]);
                DayPaymentSum = (Price).ToString("N");
            }
        }

        #endregion

        #region 本月付款额
        //本月付款额    add by hgh  CompCollection_view 状态   and status!=3
        string paggersql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID not in (select ID from Dis_Order where ISNULL(dr,0)=0 and Otype=9) and DisID=" + this.DisID + "  and status!=3 and CompID=" + this.CompID + " and Date>='" + day1 + "' and Date<'" + mothday + "' AND vedf9=1 ";

        DataTable paggerdt = SqlHelper.Query(SqlHelper.LocalSqlServer, paggersql).Tables[0];
        if (paggerdt != null)
        {
            if (paggerdt.Rows.Count > 0)
            {
                decimal Price = paggerdt.Rows[0]["Price"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(paggerdt.Rows[0]["Price"]);
                PaymentSum = (Price).ToString("N");
            }
        }
        #endregion

        #region 本月应付额
        //本月应付额

        decimal AuditAmount = 0;
        decimal payAmount = 0;
        decimal payyzf = 0;

        //赊销订单  未支付的
        //string ArrearageSql = "SELECT SUM(AuditAmount) as AuditAmount FROM [dbo].[ArrearageRpt_view] where DisID=" + user.DisID + "and CompID=" + user.CompID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday + "'";

        //DataTable ArrearageDt = SqlHelper.Query(SqlHelper.LocalSqlServer, ArrearageSql).Tables[0];
        //if (ArrearageDt != null)
        //{
        //    if (ArrearageDt.Rows.Count > 0)
        //    {
        //        AuditAmount = ArrearageDt.Rows[0]["AuditAmount"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(ArrearageDt.Rows[0]["AuditAmount"]);
        //    }
        //}

        //未支付订单金额
        //string paysql = "  select SUM(AuditAmount) as AuditAmount from DIS_Order where (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and OState=2   and PayState in (0,1) )) and OState<>6 and ReturnState=0 and isnull(dr,0)=0 and Otype!=9 and CompID=" + this.CompID + " and DisID=" + this.DisID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday + "'";

        //DataTable payDt = SqlHelper.Query(SqlHelper.LocalSqlServer, paysql).Tables[0];
        //if (payDt != null && payDt.Rows.Count > 0)
        //{
        //    payAmount = payDt.Rows[0]["AuditAmount"].ToString() == "" ? 0 : payDt.Rows[0]["AuditAmount"].ToString().ToDecimal(0);
        //}
        //paysql = "  select SUM(PayedAmount) as PayedAmount from DIS_Order where (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and OState=2   and PayState in (0,1) )) and OState<>6 and ReturnState=0 and isnull(dr,0)=0 and Otype!=9 and CompID=" + this.CompID + " and DisID=" + this.DisID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday + "'";
        //payDt = SqlHelper.Query(SqlHelper.LocalSqlServer, paysql).Tables[0];
        //if (payDt != null && payDt.Rows.Count > 0)
        //{
        //    payyzf = payDt.Rows[0]["PayedAmount"].ToString() == "" ? 0 : payDt.Rows[0]["PayedAmount"].ToString().ToDecimal(0);
        //}
        //PayableSum = (payAmount - payyzf + AuditAmount).ToString("N");


        //edit by hgh   正常合同 总额-已付
        string paysql = "  select SUM(AuditAmount)-SUM(PayedAmount) as AuditAmount from DIS_Order where OState in(2,3,5) and isnull(dr,0)=0 and Otype!=9 and CompID=" + this.CompID + " and DisID=" + this.DisID + " and CreateDate>='" + day1 + "' and CreateDate<'" + Sday + "'";
        DataTable payDt = SqlHelper.Query(SqlHelper.LocalSqlServer, paysql).Tables[0];
        if (payDt != null && payDt.Rows.Count > 0)
        {
            payAmount = payDt.Rows[0]["AuditAmount"].ToString() == "" ? 0 : payDt.Rows[0]["AuditAmount"].ToString().ToDecimal(0);
        }
        PayableSum = payAmount.ToString("N");
        #endregion

        #region 商家公告

        List<Hi.Model.BD_CompNews> LNew = new Hi.BLL.BD_CompNews().GetList("top 3 *", "isnull(dr,0)=0 and IsEnabled=1 and Compid=" + this.CompID + " ", " istop desc,createdate desc");
        string Html = "";
        if (LNew.Count > 0)
        {
            for (int i = 0; i < LNew.Count; i++)
            {
                if (LNew[i].ShowType == null)
                    LNew[i].ShowType = "";

                if (i <=4)
                {
                    string type = LNew[i].ShowType == "1" ? "top" : LNew[i].ShowType == "2" ? "red" : LNew[i].ShowType == "1,2" ? "top red" : "";
                    Html += "<li class='" + type + "'><a title='" + LNew[i].NewsTitle + "' href=\"CompNewInfo.aspx?KeyID=" + Common.DesEncrypt(LNew[i].ID.ToString(), Common.EncryptKey) + "&Type=3\">【" + Common.GetCPNewStateName(LNew[i].NewsType) + "】" + (LNew[i].NewsTitle.Length > 16 ? LNew[i].NewsTitle.Substring(0, 16) + "..." : LNew[i].NewsTitle) + "</a>" + IsEnd(LNew[i].PmID) + (LNew[i].ShowType.IndexOf("1") >= 0 ? "<i class='newIcon'></i>" : "") + "<i class='date1'>" + LNew[i].CreateDate.ToString("yyyy-MM-dd") + "</i></li>";
                }
            }
            ULNewList.InnerHtml = Html;
        }
        else
        {
            //ULNewList.InnerHtml = "<li style='text-align:center'><span>暂无公告</span></li>";
            ULNewList.InnerHtml ="<div class='nomh-box'><i class='nomh-i'></i>暂无公告</div>";
        }
        #endregion

        if (!IsPostBack)
        {
            if (IsDisAdmin(this.UserID))
            {
                if (user.UserPwd == Util.md5("123456"))
                {
                    DisImport.Attributes.Add("style", "display:block");
                    zzc.Attributes.Add("style", "display:block");
                    return;
                }

            }
        }
    }

    public string IsEnd(int PMID)
    {
        string str = "";
        if (PMID > 0)
        {
            Hi.Model.BD_Promotion ProM = new Hi.BLL.BD_Promotion().GetModel(PMID);
            if (ProM != null)
            {
                if (ProM.ProEndTime.Date < DateTime.Now.Date.Date)
                {
                    str = "<i class='grayTxt' title='活动已结束' style='margin-left:5px;color: #aaaaaa;font-weight:bold;'>已结束</i>";
                }
            }
        }
        return str;
    }

    //习惯支付密码
    protected void Btn_Update(object sender, EventArgs e)
    {
        if (user != null)
        {
            if (Util.md5(txtpwd1.Value) == user.UserPwd)
            {

                if (Util.md5(txtpwd2.Value) == user.UserPwd)
                {
                    spanpwd.InnerText = "新密码不能与原密码相同";
                    return;
                }
                spanpwd.Attributes.CssStyle.Value = "display:none;";
                user.UserPwd = Util.md5(txtpwd2.Value);
                user.ts = DateTime.Now;
                user.modifyuser = user.ID;

                user.IsFirst = 1;//3->1
                new Hi.BLL.SYS_Users().Update(user);
                ClientScript.RegisterStartupScript(GetType(), "", "<script>layerCommon.alert('恭喜您，您的登录密码已经修改成功！', IconOption.笑脸);</script>");
                DisImport.Attributes.Add("style", "display:none");
                zzc.Attributes.Add("style", "display:none");
                return;

            }
            else
            {
                spanpwd.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
                spanpwd.InnerText = "原密码错误";
                return;
            }
        }
    }

    //判断是否为企业管理员登录
    public bool IsDisAdmin(int ID)
    {
        string sql = "select id from Sys_Compuser where isnull(dr,0)=0 and utype=5 and userid=" + ID;
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            return true;
        else
            return false;
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

        //if (Ostate.ToString() == "2" || Ostate.ToString() == "4" || (Ostate.ToString() == "5" && (ReturnState.ToString() == "0" || ReturnState.ToString() == "1")))
        //{
        //    if (state == 0)
        //    {
        //        str = string.Format("<a href='javascript:void(0)' onclick=\'pay(\"{0}\")\'  class=\"a-red\">立即支付</a> ", Common.DesEncrypt(id.ToString(), Common.EncryptKey));

        //        //判断是否开启担保支付
        //        //if (ConfigurationManager.AppSettings["IsDBPay"].ToString() == "1")
        //        //    strDB = string.Format("<a href='javascript:void(0)' onclick=\'payDB(\"{0}\")\' >担保支付</a>", Common.DesEncrypt(id.ToString(), Common.EncryptKey));
        //    }
        //    else if (state == 1)
        //    {
        //        str = string.Format("<a href='javascript:void(0)' onclick=\'pay(\"{0}\")\' class=\"a-red\">立即支付</a>", Common.DesEncrypt(id.ToString(), Common.EncryptKey));


        //        //判断是否开启担保支付
        //        //if (ConfigurationManager.AppSettings["IsDBPay"].ToString() == "1")
        //        //    strDB = string.Format("<a href='javascript:void(0)' onclick=\'payDB(\"{0}\")\'>担保支付</a>", Common.DesEncrypt(id.ToString(), Common.EncryptKey));

        //    }
        //}
        return str + strDB;
    }
}