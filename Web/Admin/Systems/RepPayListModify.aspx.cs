using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Data;
using DBUtility;

public partial class Admin_Systems_RepPayListModify : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string guid = Convert.ToString(Request.QueryString["guid"]);
            string type = Convert.ToString(Request.QueryString["type"]);
            if (type.Equals("1"))
            {
                this.btnAdd.Visible = false;
            }
            Databind(guid);
            ViewState["guid"] = guid;
        }
        DataBindLink();
    }

    /// <summary>
    /// 初次加载
    /// </summary>
    /// <param name="guid"></param>
    public void Databind(string guid)
    {


        try
        {
            if (!string.IsNullOrEmpty(guid))
            {
                string sql = @"select bd_distributor.DisName '付款方',PAY_Payment.vdef5 '手续费',  PAY_Payment.PayPrice '付款金额' ,PAY_Payment.guid '交易流水号',
                             dis_order.ReceiptNo '订单编号',
                        case
                         when Channel=1 then '快捷支付'  
                          when Channel=2 then '银联支付'  
                        when Channel=3 then '网银支付' 
                          when Channel=4 then 'B2B网银支付' end  as '付款方式',PayDate '支付时间',  
                           AccountName '收款方',bankcode '收款方帐号',PAY_PayLog.CreateDate '清算时间'
,case when PAY_PayLog.MarkName='40' then '成功' when PAY_PayLog.MarkName='9999' then '已处理'  else '失败' end as '清算状态'
                            from PAY_Payment
                        join  BD_Distributor  on bd_distributor.id=pay_payment.disid 
                        join dis_order on dis_order.id=pay_payment.orderid 
                        join PAY_PayLog on PAY_PayLog.number=pay_payment.guid and Start=2000
                        where PAY_Payment.isaudit=1 and PAY_Payment.guid ='" + guid + "' order by PayDate desc";

                Pagger pagger = new Pagger(sql);

                string qszt = string.Empty;
                DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                foreach (DataRow dr in ds.Rows)
                {
                    lblzflsh.InnerText = Convert.ToString(dr["交易流水号"]);
                    this.lblzfsj.InnerText = Convert.ToString(dr["支付时间"]);
                    this.lblzflx.InnerText = Convert.ToString(dr["付款方式"]);
                    this.lbldydh.InnerText = Convert.ToString(dr["订单编号"]);
                    this.lblfkf.InnerText = Convert.ToString(dr["付款方"]);
                    this.lblskf.InnerText = Convert.ToString(dr["收款方"]);
                    this.lblfkyh.InnerText = "";// Convert.ToString(dr["支付时间"]);
                    this.lblskyh.InnerText = "";// Convert.ToString(dr["支付时间"]);
                    this.lblfkzh.InnerText = "";// Convert.ToString(dr["支付时间"]);
                    this.lblskzh.InnerText = Convert.ToString(dr["收款方帐号"]);
                    this.lblzfje.InnerText = Convert.ToString(dr["付款金额"]);
                    this.lblsxf.InnerText = Convert.ToString(dr["手续费"]);
                    qszt = Convert.ToString(dr["清算状态"]);
                    this.lblqsjg.InnerText = qszt;
                    this.lblqssj.InnerText = Convert.ToString(dr["清算时间"]);
                }


                List<Hi.Model.PAY_PaymentLog> list = new Hi.BLL.PAY_PaymentLog().GetList("", " OrgCode='" + guid + "'", " id desc");
                if (list.Count > 0)
                {
                    txtArriveDate.Value = Convert.ToString(list[0].PayTime);
                    txtArriveDate.Disabled = true;
                    txtqssbyy.Value = Convert.ToString(list[0].Message);
                    txtsm.Value = Convert.ToString(list[0].PayCode);
                    ddrcljg.Value = Convert.ToString(list[0].PayorgCode);

                    ddrcljg.Value = Convert.ToString(list[0].PayorgCode);

                    if (!string.IsNullOrEmpty(list[0].Remark))
                    {
                        editdiv.Attributes.Add("style", " display:none;");
                        DFile.Visible = true;
                    }
                    else
                    {
                        DFile.Visible = false;

                    }
                    ddrcljg.Disabled = true;
                    if (qszt != "已处理" || qszt != "成功")
                    {
                        editdiv.Attributes.Add("style", " display:none;");
                        
                        DFileinfo.Visible = true;
                    }
                    else
                    {
                        editdiv.Attributes.Add("style", " display:block;");
                        DFileinfo.Visible = true;
                    }


                }
                else
                {
                    txtArriveDate.Value = DateTime.Now.ToString();
                }

            }



        }
        catch (Exception ex)
        {

        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Hi.Model.PAY_PaymentLog paymentlog = new Hi.Model.PAY_PaymentLog();
            paymentlog.OrgCode = Common.NoHTML(Convert.ToString(ViewState["guid"]));
            paymentlog.PayCode = Common.NoHTML(txtsm.Value);
            paymentlog.Message = Common.NoHTML(txtqssbyy.Value);
            paymentlog.PayorgCode = Common.NoHTML(ddrcljg.Value);
            paymentlog.PayTime = Convert.ToDateTime(txtArriveDate.Value);
            paymentlog.Remark = Common.NoHTML(this.HidFfileName.Value);
            int num = new Hi.BLL.PAY_PaymentLog().Add(paymentlog);
            if (num > 0)
            {
                if (ddrcljg.Value == "1")
                {
                    string upsql = "update pay_paylog set  markname='9999' where number='" + Common.NoHTML(Convert.ToString(ViewState["guid"])) + "'";
                    SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, upsql);
                }
                JScript.AlertMsg(this, "处理成功", "RepPayList.aspx");
            }
            else
                JScript.AlertMsg(this, "处理失败", "RepPayList.aspx");
        }
        catch (Exception ex)
        {
            throw new Exception();
        }

    }


    public void DataBindLink()
    {
        List<Hi.Model.PAY_PaymentLog> list = new Hi.BLL.PAY_PaymentLog().GetList("", " OrgCode='" + ViewState["guid"] + "'", " id desc");

        if (list.Count > 0)
        {
            if (!string.IsNullOrEmpty(list[0].Remark))
            {
                string[] files = list[0].Remark.Split(new char[] { ',' });
                foreach (string file in files)
                {
                    if (!string.IsNullOrEmpty(file))
                    {
                        LinkButton linkFile = new LinkButton();
                        linkFile.Click += new EventHandler(Download_Click);
                        if (file.LastIndexOf("_") != -1)
                        {
                            string text = file.Substring(0, file.LastIndexOf("_")) + Path.GetExtension(file);
                            if (text.Length < 15)
                                linkFile.Text = text;
                            else
                            {
                                linkFile.Text = text.Substring(0, 15) + "...";
                            }
                            linkFile.Attributes.Add("title", text);
                        }
                        else
                        {
                            string text = file.Substring(0, file.LastIndexOf("-")) + Path.GetExtension(file);
                            if (text.Length < 15)
                                linkFile.Text = text;
                            else
                            {
                                linkFile.Text = text.Substring(0, 15) + "...";
                            }
                            linkFile.Attributes.Add("title", text);
                        }
                        linkFile.Style.Add("margin-right", "5px");
                        linkFile.Style.Add("text-decoration", "underline");
                        linkFile.Attributes.Add("fileName", file);
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Controls.Add(linkFile);
                        //HtmlImage img = new HtmlImage();
                        //img.Src = "../../images/icon_del.png";
                        //img.Attributes.Add("title", "删除附件");
                        //img.Attributes.Add("onclick", "AnnexDel(this,'Dis'," + KeyID + ",'" + file + "')");
                        //div.Controls.Add(img);
                        DFileinfo.Controls.Add(div);
                    }
                }
            }
        }
    }
    public void Download_Click(object sender, EventArgs e)
    {
        LinkButton bt = sender as LinkButton;
        string fileName = bt.Attributes["fileName"];
        string filePath = Server.MapPath("../../UploadFile/") + fileName;
        if (File.Exists(filePath))
        {
            FileInfo file = new FileInfo(filePath);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name.Substring(0, file.Name.LastIndexOf("_")) + Path.GetExtension(file.Name))); //解决中文文件名乱码    
            Response.AddHeader("Content-length", file.Length.ToString());
            Response.ContentType = "appliction/octet-stream";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();

        }
        else
        {
            JScript.AlertMsgOne(this, "附件不存在！", JScript.IconOption.哭脸);
        }
    }

}