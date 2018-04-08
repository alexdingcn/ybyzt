

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Distributor_neworder_PayItemInfo : DisPageBase
{
    //Hi.Model.SYS_Users user = null;
    //int KeyID = 0;
    int PreType = -1;
    int orderid = -1;

    public int paymentid = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);

        if (!IsPostBack)
        {
            //if (Request["KeyID"] != null)
            //{
            //    string Id = Common.DesDecrypt(Request["KeyID"].ToString(), Common.EncryptKey);
            //    KeyID = Id.ToInt(0);
            //}
            //支付类型
            if (Request["PreType"] != null)
            
                PreType =Convert.ToInt32(Common.DesDecrypt(Request["PreType"].ToString(), Common.EncryptKey));
            //订单Id
            if (Request["orderid"] != null)
            
                 orderid = Convert.ToInt32(Common.DesDecrypt(Request["orderid"].ToString(), Common.EncryptKey));

            //修改支付流程新增详情页面
            if (Request["Paymnetid"] != null)
                paymentid = Convert.ToInt32(Common.DesDecrypt(Request["Paymnetid"].ToString(), Common.EncryptKey));

            Bind();
          
        }
        //附件绑定
        DataFileBind();
      
    }

    private void Bind()
    {
       
            Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);

            string sql = string.Empty;
            if (paymentid > 0) 
            {
                sql = "SELECT * FROM [dbo].[CompCollection_view] where PreType=" + PreType + " and paymentid=" + paymentid;
            }
           
            DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

            if (ds != null && ds.Rows.Count > 0 && orderModel != null)
            {
                this.lblReceiptNo.InnerText = orderModel.ReceiptNo.ToString();


                this.lblOState.InnerText = OrderInfoType.OState(orderModel.ID);
                this.lblPayState.InnerText = OrderInfoType.PayState(orderModel.PayState);



                this.lblTotalPrice.InnerText = orderModel.AuditAmount.ToString("N");
                this.lblPayedPrice.InnerText = ds.Rows[0]["Price"].ToString().ToDecimal(0).ToString("N");
                this.lblPayAuomet.InnerText = orderModel.PayedAmount.ToString("N");
                this.lblArriveDate.InnerText = Convert.ToDateTime(ds.Rows[0]["Date"]).ToString("yyyy-MM-dd");
                this.lblDisUser.InnerText = Common.GetUserName(orderModel.DisUserID);
                this.lblCreateDate.InnerText = Convert.ToDateTime(orderModel.CreateDate).ToString("yyyy-MM-dd");

                this.lblPaySource.InnerText = ds.Rows[0]["Source"].ToString();

                ViewState["paymentID"] = ds.Rows[0]["paymentID"].ToString().ToInt(0);
               

            }

          

        
    }





    /// <summary>
    /// 绑定附件信息
    /// </summary>
    public void DataFileBind()
    {
        Hi.Model.PAY_Payment Pre = new Hi.BLL.PAY_Payment().GetModel(Convert.ToString(ViewState["paymentID"]).ToInt(0));
        if (Pre != null)
        {
            if (!string.IsNullOrEmpty(Pre.attach))
            {
                string[] files = Pre.attach.Split(new string[] { "&&" },StringSplitOptions.RemoveEmptyEntries);
                foreach (string file in files)
                {
                    if (!string.IsNullOrEmpty(file))
                    {
                        LinkButton linkFile = new LinkButton();
                        linkFile.Click += new EventHandler(Download_Click);
                        if (file.LastIndexOf("_") != -1)
                        {
                            string text = file.Substring(0, file.LastIndexOf("_")) + Path.GetExtension(file);
                            if (text.Length < 6)
                                linkFile.Text = text;
                            else
                            {
                                linkFile.Text = text.Substring(0,6) + "...";
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
                        linkFile.Style.Add("margin-right", "8px");
                        linkFile.Style.Add("text-decoration", "underline");
                        linkFile.Attributes.Add("fileName", file);
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Controls.Add(linkFile);
                        //HtmlImage img = new HtmlImage();
                        //img.Src = "../../images/icon_del.png";
                        //img.Attributes.Add("title", "删除附件");
                        //img.Attributes.Add("onclick", "AnnexDel(this,'Dis'," + KeyID + ",'" + file + "')");
                        //div.Controls.Add(img);
                        DFile.Controls.Add(div);
                    }
                }
            }
        }
    }
    //附件下载相关事件
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


