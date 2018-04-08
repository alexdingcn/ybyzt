using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

public partial class Company_Pay_PayCreateInfo : CompPageBase
{

    Hi.BLL.PAY_PrePayment PAbll = new Hi.BLL.PAY_PrePayment();
    public static bool Auditstatr = false;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
        DataFileBind();
    }


    /// <summary>
    /// 绑定附件信息
    /// </summary>
    public void DataFileBind() 
    {
        Hi.Model.PAY_PrePayment Pre = new Hi.BLL.PAY_PrePayment().GetModel(KeyID);
        if (Pre != null)
        {
            if (!string.IsNullOrEmpty(Pre.vdef5))
            {
            //    string[] files = Pre.vdef5.Split(new char[] { ',' });
            //    foreach (string file in files)
            //    {
            //        if (!string.IsNullOrEmpty(file))
            //        {
            //            LinkButton linkFile = new LinkButton();
            //            linkFile.Click += new EventHandler(Download_Click);
            //            if (file.LastIndexOf("_") != -1)
            //            {
            //                string text = file.Substring(0, file.LastIndexOf("_")) + Path.GetExtension(file);
            //                if (text.Length < 15)
            //                    linkFile.Text = text;
            //                else
            //                {
            //                    linkFile.Text = text.Substring(0, 15) + "...";
            //                }
            //                linkFile.Attributes.Add("title", text);
            //            }
            //            else
            //            {
            //                string text = file.Substring(0, file.LastIndexOf("-")) + Path.GetExtension(file);
            //                if (text.Length < 15)
            //                    linkFile.Text = text;
            //                else
            //                {
            //                    linkFile.Text = text.Substring(0, 15) + "...";
            //                }
            //                linkFile.Attributes.Add("title", text);
            //            }
            //            linkFile.Style.Add("margin-right", "5px");
            //            linkFile.Style.Add("text-decoration", "underline");
            //            linkFile.Attributes.Add("fileName", file);
            //            HtmlGenericControl div = new HtmlGenericControl("div");
            //            div.Controls.Add(linkFile);
            //            //HtmlImage img = new HtmlImage();
            //            //img.Src = "../../images/icon_del.png";
            //            //img.Attributes.Add("title", "删除附件");
            //            //img.Attributes.Add("onclick", "AnnexDel(this,'Dis'," + KeyID + ",'" + file + "')");
            //            //div.Controls.Add(img);
            //            DFile.Controls.Add(div);
            //        }
            //    }


                //附件
                if (Pre.vdef5 != "")
                {
                    StringBuilder li = new StringBuilder();
                    string[] atta = Pre.vdef5.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    if (atta.Length > 0)
                    {
                        foreach (var item in atta)
                        {
                            string[] att = item.Split(new string[] { "^^" }, StringSplitOptions.RemoveEmptyEntries);
                            if (att.Length > 1)
                               // li.AppendFormat("<li> <a href=\"{2}\" target=\"_blank\" class=\"name\">{0}（大小：{4}KB）</a><a href=\"javascript:;\"  class=\"bule del\" tip=\"{3}\" orderid=\"{1}\">删除</a><a href=\"{2}\" target=\"_blank\" class=\"bule\">下载</a></li>", att[0] + att[1].Substring(att[1].LastIndexOf(".")), KeyID, Common.GetWebConfigKey("ImgViewPath") + "OrderFJ/" + item, item, OrderType.GetSize(item));
                                li.AppendFormat("<li> <a href=\"{2}\" target=\"_blank\"  style=\"text-decoration:underline;margin-left:10px;\" class=\"name\">{0}（大小：{4}KB）</a></li>", att[0] + att[1].Substring(att[1].LastIndexOf(".")), KeyID, Common.GetWebConfigKey("ImgViewPath") + "OrderFJ/" + item, item, OrderType.GetSize(item));

                        }
                    }
                    ulAtta.InnerHtml = li.ToString();
                    //this.hrOrderFj.Value = dt.Rows[0]["Atta"].ToString();
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

    /// <summary>
    /// 绑定事件
    /// </summary>
    public void Bind()
    {
        if(KeyID>0)
        {

            Hi.Model.PAY_PrePayment Ppmodel = PAbll.GetModel(KeyID);
            this.lbldis.InnerText = Common.GetDis(Ppmodel.DisID, "DisName");
            this.lblcreatetime.InnerText = Convert.ToDateTime(Ppmodel.CreatDate).ToString("yyyy-MM-dd");
            this.lblpay_type.InnerText = Ppmodel.vdef6;
            this.lblcreateuser.InnerText =Common.GetUserName(Ppmodel.CrateUser);
            this.lblprice.InnerText = Convert.ToDecimal(Ppmodel.price).ToString("0.00");
            this.lblpaytype.InnerText = Ppmodel.vdef3;
            this.lblRemark.InnerText = Ppmodel.vdef1;
            //this.Audit.Visible= Ppmodel.AuditState == 2 ? false : true;

            this.Audit.Visible = false;
            this.Log.Visible = false;
            
        }

        
    }


    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAudit_Click(object sender, EventArgs e)
    {

        Hi.Model.PAY_PrePayment PAmodel =this.PAbll.GetModel(KeyID);

        if (PAmodel != null)
        {
            if (PAmodel.AuditState !=Convert.ToInt32(Enums.PrePayState.已审))
            {                
                PAmodel.AuditState = 2;
                PAmodel.IsEnabled = 1;
                PAmodel.ID = KeyID;
                if ( PAbll.Update(PAmodel))
                {
                    ////sum代理商全部补录，冲正金额
                    //decimal sums = new Hi.BLL.PAY_PrePayment().sums(PAmodel.DisID,PAmodel.CompID);

                    ////修改代理商的企业钱包金额
                    ////调用model,对属性进行赋值
                    //Hi.Model.BD_Distributor dismodel = new Hi.BLL.BD_Distributor().GetModel(PAmodel.DisID);
                    //dismodel.DisAccount = sums;
                    //dismodel.ID = PAmodel.DisID;
                    ////调用修改方法    
                    //Hi.BLL.BD_Distributor disupdate = new Hi.BLL.BD_Distributor();
                    //bool disup = disupdate.Update(dismodel);                  
                   
                    //if (disup)
                    //{

                    Utils.AddSysBusinessLog(this.CompID, "PrePayment", KeyID.ToString(), "预收款补录审核", "");
                    JScript.AlertMsgOne(this, "操作成功！", JScript.IconOption.笑脸);
                    Bind();

                   // }

                }
            }
            else
            {
                JScript.AlertMsgOne(this, "数据状态不正确,不能进行审核！", JScript.IconOption.错误);
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "数据不存在！", JScript.IconOption.错误);
        }

    }
   

    /// <summary>
    /// 退回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturn_Click(object sender, EventArgs e)
    {       
            //JScript.ShowAlert(this, "数据不存在!");
        
    }

  
}