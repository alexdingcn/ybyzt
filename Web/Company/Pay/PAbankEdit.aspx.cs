using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Company_Pay_PAbankEdit : CompPageBase
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

    public string page;

    //收款帐号ID
    private int paid;

    public int Paid
    {
        get { return paid; }
        set { paid = value; }
    }

    Hi.BLL.BD_Distributor BDdbutorbll = new Hi.BLL.BD_Distributor();
    public int userid = 0;
    public string username = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Paid = Convert.ToInt32(Request.QueryString["paid"]);
        if (Request.QueryString["action"] != null)
        {
            Action = Request.QueryString["action"].ToString();
        }

        if (Action == "Addr")
        {
            DisId = Convert.ToInt32(Request["DisId"]);
            AddrId = Convert.ToInt32(Request["AddrId"]);
        }
        userid = this.UserID;
        username = this.UserName;
        this.hid_username.Value = username;
        this.hid_userid.Value = userid.ToString();
        if (!IsPostBack)
        {
            if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
            {
                atitle.InnerText = "我要开通";
                btitle.InnerText = "绑定收款帐号";
                btnSave.Text = "下一步";
            }
            else
            {
                btnSave.Text = "确定";
            }
            this.txtcompId.Value = this.CompID.ToString();


            DataTable dt = ViewState["Distributor"] as DataTable;
            this.gvDtl.DataSource = dt;
            this.gvDtl.DataBind();
            Bind();
        }
    }

    protected void Bind()
    {
        string str = string.Empty;
        if (KeyID > 0)
        {
            //修改时控制账户类型是否可用
            ddltype.Disabled = false;

            Hi.Model.PAY_PaymentBank bankModel = new Hi.BLL.PAY_PaymentBank().GetModel(KeyID);

            this.txtDisUser.Value = bankModel.AccountName;//账户名称
            this.txtbankcode.Value = bankModel.bankcode;
            this.txtbankAddress.Value = bankModel.bankAddress;
            this.txtphone.InnerText = bankModel.vdef4 == "" ? new Hi.BLL.PAY_PrePayment().GetPhoneBYCompID(this.CompID).ToString() : bankModel.vdef4;
            this.hid_tel.Value = bankModel.vdef4 == "" ? new Hi.BLL.PAY_PrePayment().GetPhoneBYCompID(this.CompID).ToString() : bankModel.vdef4;

            //this.txtbankPrivate.Value = bankModel.bankPrivate;
            // this.txtbankCity.Value = bankModel.bankCity;

            this.hidProvince.Value = bankModel.bankPrivate;
            this.hidCity.Value = bankModel.bankCity;
            this.hidArea.Value = bankModel.vdef1;



            this.chkIsno.Checked = bankModel.Isno == 1 ? true : false;

            if (bankModel.Isno == 1)
            {
                this.btnDis.Attributes.Add("style", "display:none");
                div_grid.Attributes.Add("style", "display:none");
            }
            this.txtRemark.Value = bankModel.Remark;

            //现有银行列表
            string bankID_str = "102,103,104,105,301,100,303,305,306,302,310,309,401,403,307,308";
            string BankID = Convert.ToString(bankModel.BankID);

            bool index = bankID_str.Contains(BankID);
            if (index)
                this.ddlbank.Value = Convert.ToString(bankModel.BankID);
            else
            {
                this.ddlbank.Items.Insert(0, new ListItem(new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(BankID), BankID));
                this.ddlbank.SelectedIndex = 0;
            }

            this.ddltype.Value = Convert.ToString(bankModel.type);
            if (bankModel.type == 11)
            {

                this.tbdis.Visible = true;
                this.SltPesontype.Value = bankModel.vdef2;
                this.txtpesoncode.Value = bankModel.vdef3;
            }
            else
            {
                this.tbdis.Visible = false;
            }
            //this.ddlbank.InnerText=


            DataTable dtdis = new Hi.BLL.PAY_PaymentAccountdtl().GetDisBYpbID(KeyID);


            List<Hi.Model.BD_Distributor> dblist = new List<Hi.Model.BD_Distributor>();
            foreach (DataRow dr in dtdis.Rows)
            {
                Hi.Model.BD_Distributor dbmodel = new Hi.Model.BD_Distributor();
                dbmodel.ID = Convert.ToInt32(dr["DisID"]);
                dbmodel.DisName = dr["DisName"].ToString();
                dbmodel.DisCode = dr["DisCode"].ToString();
                dbmodel.DisLevel = dr["DisLevel"].ToString();
                dbmodel.Address = dr["Address"].ToString();
                dbmodel.Principal = dr["Principal"].ToString();
                dbmodel.AreaID = Convert.ToInt32(dr["AreaID"]);
                dblist.Add(dbmodel);
                //if (string.IsNullOrEmpty(str))
                //{
                //    str =dr["DisID"] + ",";
                //}
                //else
                //{
                //    str += dr["DisID"] + ",";
                //}
                //this.txtGoodsCodes.Value = new Hi.BLL.PAY_PrePayment().GetDisIDBYCompID(this.CompID) + ",";// str;


            }

            this.gvDtl.DataSource = dtdis;
            this.gvDtl.DataBind();

            AddMaterial(dblist);
        }
        else
        {
            int compid = CompID;
            this.txtphone.InnerText = new Hi.BLL.PAY_PrePayment().GetPhoneBYCompID(compid).ToString();
            this.hid_tel.Value = new Hi.BLL.PAY_PrePayment().GetPhoneBYCompID(compid).ToString();
            //设置默认值
            ddltype.Value = "11";
            SltPesontype.Value = "0";

            int num = new Hi.BLL.PAY_PrePayment().GetBankBYCompID(compid);
            if (num >= 1)
            {
                this.chkIsno.Checked = false;

                //this.btnDis.Attributes.Add("style", "display:block;");
                //div_grid.Attributes.Add("style", "display:block;");
                this.gvDtl.Visible = true;
                this.btnDis.Visible = true;
                
            }
            else
            {
                //this.btnDis.Attributes.Add("style", "display:none;");
                //div_grid.Attributes.Add("style", "display:none;");
                this.gvDtl.Visible = false;
                this.btnDis.Visible = false;
            }





        }
        str = new Hi.BLL.PAY_PrePayment().GetDisIDBYCompID(CompID);
        if (str != "")
        {
            this.txtGoodsCodes.Value = new Hi.BLL.PAY_PrePayment().GetDisIDBYCompID(CompID) + ",";// str;
        }
    }


    
    /// <summary>
    /// 银行卡绑定、关联代理商
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //声明变量
        string AccountName = string.Empty;
        string bankcode = string.Empty;
        string bankAddress = string.Empty;
        string bankPrivate = string.Empty;
        string bankCity = string.Empty;
        int Isno = 0;
        string remark = string.Empty;
        string county = string.Empty;//开会所在区县
        int BankId = 0;//银行Id
        int type = 0;//账户类型

        //收集数据
        AccountName = this.txtDisUser.Value.Trim();//账户名称
        bankcode = this.txtbankcode.Value.Replace(" ", "");
        bankAddress = this.txtbankAddress.Value.Trim();
        // bankPrivate = this.txtbankPrivate.Value;
        // bankCity = this.txtbankCity.Value;
        bankPrivate = this.hidProvince.Value.Trim();
        bankCity = this.hidCity.Value.Trim();
        county = this.hidArea.Value.Trim();
        Isno = Convert.ToInt32(this.chkIsno.Checked == true ? 1 : 0);

        int is_no = Convert.ToInt32(this.hid_isno.Value);

        if (is_no == 1)
        {
            //Isno = 1;
            int success = new Hi.BLL.PAY_PrePayment().UpisnoBYCompID(CompID);
        }
        //else
        //{
        //    Isno = 0;
        //}
        remark = this.txtRemark.Value.Trim();
        BankId = Convert.ToInt32(this.ddlbank.Value);
        type = Convert.ToInt32(this.ddltype.Value);

        //实例化对象实体
        Hi.Model.PAY_PaymentBank pbModel = new Hi.Model.PAY_PaymentBank();

        // Paid = Convert.ToInt32(Request.QueryString["paid"]);
        pbModel.paymentAccountID = Paid;
        pbModel.BankID = BankId;
        pbModel.AccountName = AccountName;
        pbModel.bankcode = bankcode;
        pbModel.bankAddress = bankAddress;
        pbModel.bankPrivate = bankPrivate;
        pbModel.bankCity = bankCity;
        pbModel.Isno = Isno;
        pbModel.Remark = remark;
        pbModel.CreateDate = DateTime.Now;
        pbModel.CreateUser = UserID;
        pbModel.dr = 0;
        pbModel.modifyuser = UserID;
        pbModel.ts = DateTime.Now;
        pbModel.vdef1 = county;
        pbModel.Start = 1;

        pbModel.vdef4 = this.txtphone.InnerText;
        pbModel.vdef5 = this.txtphpcode.Value;
        pbModel.CompID = CompID;
        pbModel.type = type;
        pbModel.vdef2 = this.SltPesontype.Value;
        pbModel.vdef3 = this.txtpesoncode.Value;
        if (KeyID > 0)
        {
            pbModel.ID = KeyID;
            bool result = new Hi.BLL.PAY_PaymentBank().Update(pbModel);

            if (result)
            {
                result = new Hi.BLL.PAY_PaymentAccountdtl().DeldtlBYpbID(KeyID);

                if (result)
                {
                    if (this.chkIsno.Checked != true)
                    {
                        #region 保存关联代理商信息


                        Hi.BLL.PAY_PaymentAccountdtl padtlbll = new Hi.BLL.PAY_PaymentAccountdtl();
                        List<Hi.Model.PAY_PaymentAccountdtl> padtlList = new List<Hi.Model.PAY_PaymentAccountdtl>();
                        DataTable dt = ViewState["Distributor"] as DataTable;
                        foreach (DataRow dr in dt.Rows)
                        {
                            string lblDisId = Convert.ToString(dr["ID"]);//代理商ID


                            Hi.Model.PAY_PaymentAccountdtl padtl = new Hi.Model.PAY_PaymentAccountdtl();
                            //    Label lblDisId = (Label)gvRow.FindControl("lblGoodsId");//代理商ID
                            //    Label lblDisName = (Label)gvRow.FindControl("lblDisName");
                            //    Label lblDisCode = (Label)gvRow.FindControl("lblDisCode");
                            //    Label lblAreaID = (Label)gvRow.FindControl("lblAreaID");
                            //    Label lblDisLevel = (Label)gvRow.FindControl("lblDisLevel");
                            //    Label lblAddress = (Label)gvRow.FindControl("lblAddress");
                            //    Label lblPrincipal = (Label)gvRow.FindControl("lblPrincipal");

                            padtl.DisID = Convert.ToInt32(lblDisId);//代理商ID
                            padtl.PBID = KeyID;//银行表ID
                            padtl.CreateDate = DateTime.Now;
                            padtl.CreateUser = UserID;
                            padtl.ts = DateTime.Now;
                            padtl.dr = 0;
                            padtl.modifyuser = UserID;
                            padtl.Start = 0;

                            padtlList.Add(padtl);//插入到list集合中
                        }

                        padtlbll.Add(padtlList);
                        #endregion
                    }

                    //判断代理商是否有默认账户
                    int num = new Hi.BLL.PAY_PrePayment().GetBankBYCompID(CompID);

                    if (Request["nextstep"] + "" == "1")
                    {
                        if (num == 0)
                        {
                            JScript.AlertMethod(this, "一个企业，最好有一个，默认第一收款账户，请检查！", JScript.IconOption.错误, "function (){ location.href='" + ("PayBankAuditInfo.aspx?nextstep=1&paid=" + Paid + "&KeyID=" + KeyID) + "'; }");
                        }
                        else
                            Response.Redirect("PayBankAuditInfo.aspx?nextstep=1&paid=" + Paid + "&KeyID=" + KeyID);
                    }
                    else
                    {
                        if (num == 0)
                        {
                            JScript.AlertMethod(this, "一个企业，最好有一个，默认第一收款账户，请检查！", JScript.IconOption.错误, "function (){ location.href='" + ("PayBankAuditInfo.aspx?paid=" + Paid + "&KeyID=" + KeyID) + "'; }");
                        }
                        else
                            Response.Redirect("PayBankAuditInfo.aspx?paid=" + Paid + "&KeyID=" + KeyID);
                        //JScript.AlertMsg(this, "操作成功！", "PayBankAuditInfo.aspx?paid=" + Paid + "&KeyID=" + KeyID);
                    }
                }
            }



        }
        else
        {

            int succes = new Hi.BLL.PAY_PaymentBank().Add(pbModel);
            if (succes > 0)//银行账户保存成功后，进行绑定代理商操作
            {
                if (this.chkIsno.Checked != true)
                {
                    #region 保存关联代理商信息


                    Hi.BLL.PAY_PaymentAccountdtl padtlbll = new Hi.BLL.PAY_PaymentAccountdtl();
                    List<Hi.Model.PAY_PaymentAccountdtl> padtlList = new List<Hi.Model.PAY_PaymentAccountdtl>();
                    DataTable dt = ViewState["Distributor"] as DataTable;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string lblDisId = Convert.ToString(dr["ID"]);//代理商ID
                        //Label lblDisName = (Label)gvRow.FindControl("lblDisName");
                        //Label lblDisCode = (Label)gvRow.FindControl("lblDisCode");
                        //Label lblAreaID = (Label)gvRow.FindControl("lblAreaID");
                        //Label lblDisLevel = (Label)gvRow.FindControl("lblDisLevel");
                        //Label lblAddress = (Label)gvRow.FindControl("lblAddress");
                        //Label lblPrincipal = (Label)gvRow.FindControl("lblPrincipal");

                        Hi.Model.PAY_PaymentAccountdtl padtl = new Hi.Model.PAY_PaymentAccountdtl();
                        padtl.DisID = Convert.ToInt32(lblDisId);//代理商ID
                        padtl.PBID = succes;//银行表ID
                        padtl.CreateDate = DateTime.Now;
                        padtl.CreateUser = UserID;
                        padtl.ts = DateTime.Now;
                        padtl.dr = 0;
                        padtl.modifyuser = UserID;
                        padtl.Start = 0;

                        padtlList.Add(padtl);//插入到list集合中
                    }

                    padtlbll.Add(padtlList);

                    #endregion
                }
                // JScript.AlertMsg(this, "操作成功！", "PAbankInfo.aspx?paid=" + Paid + "&KeyID=" + succes);
                // ClientScript.RegisterStartupScript(this.GetType(), "add", "<script>activeName(); window.location.href ='PAbankInfo.aspx?paid=" + Paid + "&KeyID=" + succes+"';</script>");
                //判断代理商是否有默认账户
                int num = new Hi.BLL.PAY_PrePayment().GetBankBYCompID(CompID);

                if (Request["nextstep"] + "" == "1")
                {
                    if (num == 0)
                    {
                        JScript.AlertMethod(this, "一个企业，最好有一个，默认第一收款账户，请检查！", JScript.IconOption.错误, "function (){ location.href='" + ("PayBankAuditInfo.aspx?nextstep=1&paid=" + Paid + "&KeyID=" + succes) + "'; }");
                    }
                    else
                        Response.Redirect("PayBankAuditInfo.aspx?nextstep=1&paid=" + Paid + "&KeyID=" + succes);
                }
                else
                {
                    if (num == 0)
                    {
                        JScript.AlertMethod(this, "一个企业，最好有一个，默认第一收款账户，请检查！", JScript.IconOption.错误, "function (){ location.href='" + ("PayBankAuditInfo.aspx?paid=" + Paid + "&KeyID=" + succes) + "'; }");
                    }
                    else
                        Response.Redirect("PayBankAuditInfo.aspx?paid=" + Paid + "&KeyID=" + succes);
                }
            }
        }

    }
    /// <summary>
    /// 选择代理商返回
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

        //判断证件类型层的显示
        if (this.ddltype.Value == "12")
            this.tbdis.Visible = false;
        else
            this.tbdis.Visible = true;

        //判断关联代理商按钮是否可用
        if (this.chkIsno.Checked)
        {
            //this.btnDis.Attributes.Add("style", "display:none;");
            //div_grid.Attributes.Add("style", "display:none;");
            this.gvDtl.Visible = false;
            this.btnDis.Visible = false;
        }
        else
        {
            //this.btnDis.Attributes.Add("style", "display:block;");
            //div_grid.Attributes.Add("style", "display:block;");
            this.gvDtl.Visible = true;
            this.btnDis.Visible = true;
        }

    }

    /// <summary>
    /// 选择银行返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSelectBankReturn_ServerClick(object sender, EventArgs e)
    {

        string bankname = this.txtbandname.Value;
        string bankcode = this.txtbankcodes.Value;

        ListItem list = ddlbank.Items.FindByValue(bankcode);
        if (list == null)
        {
            this.ddlbank.Items.Insert(0, new ListItem(bankname, bankcode));
            this.ddlbank.SelectedIndex = 0;
        }
        else
        {
            int index = ddlbank.Items.IndexOf(list);
            this.ddlbank.SelectedIndex = index;
        }

        //判断证件类型层的显示
        if (this.ddltype.Value == "12")
            this.tbdis.Visible = false;
        else
            this.tbdis.Visible = true;

        //判断关联代理商按钮是否可用
        if (this.chkIsno.Checked)
        {
            this.btnDis.Attributes.Add("style", "display:none;");
            div_grid.Attributes.Add("style", "display:none;");
        }
        else
        {
            this.btnDis.Attributes.Add("style", "display:block;");
            div_grid.Attributes.Add("style", "display:block;");
        }

    }

    /// <summary>
    /// 返回选择的代理商
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnDelGoods_Click(object sender, EventArgs e)
    {
        //DisId = this.txtDisID1.Disid.Trim().ToInt(0);

        string Id = this.hiddelgoodsid.Value;

        if (Id == "")
        {
            JScript.AlertMsgOne(this, "没有代理商信息!", JScript.IconOption.错误, 2500);
            return;
        }
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
        else
        {
            gvDtl.DataSource = null;
            gvDtl.DataBind();
        }
    }



}