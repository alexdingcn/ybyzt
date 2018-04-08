using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Collections;
using DBUtility;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.UI.HtmlControls;

public partial class Company_Goods_GoodsEdit : CompPageBase
{
    public string sAttr = "";
    public string sAttr1 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/html";
        object obj = Request["action"];
        if (obj != null)
        {
            string action = obj.ToString();
            if (action == "Sku")
            {//sku信息
                string[] list = Request["list[]"] == null ? null : Request["list[]"].Split(',');
                string[] list1 = Request["list1[]"] == null ? null : Request["list1[]"].Split(',');
                string[] list2 = Request["list2[]"] == null ? null : Request["list2[]"].Split(',');
                string guigeName =Common.NoHTML( Request["guigeName"]);
                string goodsid =Common.NoHTML( Request["goodsid"]);
                Response.Write(GetGoodsSku(list, list1, list2, guigeName, goodsid));
                Response.End();
            }
            else if (action == "Del_Doc")
            {
                Response.Write(DelDoc(Request["key"]));
                Response.End();
            }
            else if (action == "Get_Doc")
            {
                Response.Write(GetDoc());
                Response.End();
            }
        }
        if (!IsPostBack)
        {
           
            bind();
            GetGoodsLabels(KeyID);//商品标签
            Common.BindUnit(this.rptUnit, "计量单位", this.CompID);//绑定单位下拉
            ZiDingYi(KeyID);// 自定义字段
            Common.BindTemplate(this.ddlTemplate, this.CompID);//绑定规格模板
            if (KeyID != 0)
            {
                Bind();
            }
            else
            {
                this.txtCode.Value = GoodsCode();
                this.divSku.Style["display"] = "none";
                this.divProduct.Style["display"] = "none";
            }

            if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
            {
                atitle.InnerText = "我要开通";
                btitle.InnerText = "新增商品";

            }
        }

        DataBindLink();

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
            JScript.AlertMsgOne(this, "附件不存在！", JScript.IconOption.错误);
        }
    }

    /// <summary>
    /// 附件绑定
    /// </summary>
    public void DataBindLink()
    {
        if (KeyID != 0) {
            Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(KeyID);

            if (model != null && model.CompID == this.CompID)
            {
                if (!string.IsNullOrWhiteSpace(model.registeredCertificate)) { 
                    LinkButton linkFile = new LinkButton();
                    linkFile.Click += new EventHandler(Download_Click);
                    if (model.registeredCertificate.LastIndexOf("_") != -1)
                    {
                        string text = model.registeredCertificate.Substring(0, model.registeredCertificate.LastIndexOf("_")) + Path.GetExtension(model.registeredCertificate);
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
                        string text = model.registeredCertificate.Substring(0, model.registeredCertificate.LastIndexOf("-")) + Path.GetExtension(model.registeredCertificate);
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
                    linkFile.Attributes.Add("fileName", model.registeredCertificate);
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Controls.Add(linkFile);
                    HtmlImage img = new HtmlImage();
                    img.Src = "../../images/icon_del.png";
                    img.Attributes.Add("title", "删除附件");
                        img.Attributes.Add("onclick", "AnnexDelete2()");
                        div.Controls.Add(img);
                        UpFileText2.Controls.Add(div);
                    this.HidFfileName2.Value = model.registeredCertificate;
                    if (model.validity != null)
                    this.validDate2.Value = model.validity.ToString("yyyy-MM-dd");
                }
            }

        }
    }





    //编辑赋值
    public void Bind()
    {
        // this.lblProduct.Visible = false;//编辑状态不显示多规格单选框
        // this.divProduct.Visible = false;//编辑状态不显示模板下拉
        try
        {
            // Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(KeyID);
            // if (model2 != null)
            // {
            Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(KeyID);
            if (model != null&&model.CompID==this.CompID)
            {
                #region 规格属性绑定
                StringBuilder html2 = new StringBuilder();
                List<Hi.Model.BD_GoodsAttrs> goodsattrlist = new Hi.BLL.BD_GoodsAttrs().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsid=" + model.ID, "");
                if (goodsattrlist.Count > 0)
                {
                    if (goodsattrlist.Count >= 3)
                    {
                        //this.addSpec.Style["display"] = "none";
                    }
                    this.chkProduct.Checked = true;//勾选多规格
                    this.chkProduct.Disabled = true;//多规格不能修改
                    this.ddlTemplate.Enabled = false;//模板不能修改
                    foreach (Hi.Model.BD_GoodsAttrs item in goodsattrlist)
                    {
                        string html3 = string.Empty;//临时储存
                        string stratts = string.Empty;//临时储存规格值
                        //规格开始
                        html2.Append("<div class=\"mulSpecItem\"><div class=\"mulSpecProperty\" style=\"width:150px;height: auto;min-height:35px\"><input type=\"text\" value=\"" + item.AttrsName + "\" maxlength=\"4\"  placeholder=\"规格名称(4字内) \" class=\"ui-input-dashed mulSpecName box2\" name=\"mulSpecName\" style=\"height:auto;min-height:35px\"/></div><a class=\"delMulSpec\" style=\"display:none\"></a> <div class=\"mulSpecValues\" style=\"width:680px;\" >        ");
                        List<Hi.Model.BD_GoodsAttrsInfo> goodsattrinfolist = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsid=" + model.ID + " and attrsid=" + item.ID, "");
                        if (goodsattrinfolist.Count > 0)
                        {
                            foreach (Hi.Model.BD_GoodsAttrsInfo item2 in goodsattrinfolist)
                            {
                                stratts += item2.AttrsInfoName + "@@";
                                //规格值
                                html3 += "<i class=\"o-t item\" data-value=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "<i  tabindex=\"-1\" class=\"remove close\"></i></i>";
                            }
                        }
                        if (stratts != "")
                        {
                            stratts = stratts.Substring(0, stratts.Length - 2);
                        }
                        html2.Append("<input type=\"text\" style=\"display: none;\" class=\"mulSpecInp selectized\" name=\"selectized\" tabindex=\"-1\" value=\"" + stratts + "\" maxlength=\"15\"/><div class=\"selectize-control mulSpecInp multi plugin-remove_button\" ><div class=\"selectize-input items not-full box1 fl\" style=\"width:720px;height: auto;min-height:30px\" placeholder=\"使用键盘“回车键”确认并添加多个规格值\">");
                        html2.Append(html3);
                        //规格结束
                        html2.Append("<input type=\"text\" autocomplete=\"off\" tabindex=\"\" style=\"width: 4px; float:left;\" maxlength=\"15\"/><i class=\"del-i del-i-a\"></i></div><div class=\"selectize-dropdown multi mulSpecInp plugin-remove_button\" style=\"display: none;\"><div class=\"selectize-dropdown-content\"></div> </div> </div></div> <div class=\"cb\"></div> </div>");
                    }
                }
                else
                {
                    this.chkProduct.Checked = false;//取消勾选多规格
                    this.chkProduct.Disabled = false;//多规格不能修改
                    this.ddlTemplate.Enabled = true;//模板不能修改
                    this.divSku.Style["display"] = "none";
                    this.divProduct.Style["display"] = "none";
                }


                #endregion
                this.txtGoodsName.Value = model.GoodsName;//商品名称
                this.hid_txt_product_class.Value = model.CategoryID.ToString();//商品分类
                this.hid_txt_product_class.Attributes.Add("code", model.CategoryID.ToString());
                Hi.Model.SYS_GType type = new Hi.BLL.SYS_GType().GetModel(model.CategoryID);
                if (type != null)
                {
                    hid_product_CateGoryName.Value = type.TypeName;
                    this.txt_product_class.Value= type.TypeName;
                }
                this.txtunit.Value = model.Unit;
                this.txtunittex.Value = model.Unit;
                this.ImgList.InnerHtml = GetImgList(model.ID.ToString()).ToString();//图片相册
                var num = new Hi.BLL.BD_ImageList().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsId=" + model.ID, "");
                if (num.Count >= 10)
                {
                    this.AddImg.Visible = false;
                }
                else
                {
                    this.AddImg.Visible = true;
                }
                this.txtPrice.Value =  decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(model.SalePrice.ToString()).ToString())).ToString("0.00");//价格.ToString("#,##0.00")
                // this.chkRecommend.Checked = model.IsRecommended == 2 ? true : false;
                //this.chkshow.Checked = model.IsRecommended == 1 ? true : false;
                if (model.IsRecommended == 1 || model.IsRecommended == 2)
                {
                    this.chkshow.Checked = true;
                }
                else
                {
                    this.chkshow.Checked = false;
                }
                if (model.IsRecommended == 2)
                {
                    this.chkRecommend.Checked = true;
                    this.chkRecommendno.Checked = false;
                }
                else
                {
                    this.chkRecommendno.Checked = true;
                    this.chkRecommend.Checked = false;
                }
                if (model.IsRecommended == 0)
                {
                    this.isrecommend.Style.Add("display", "none");
                }
                if (model.IsLS == 1)
                {
                    this.chkisprice.Checked = true;
                    //this.txtlsprice.Style.Add("display", "");
                    //this.splsprice.Style.Add("display", "");
                    //this.lbllsprice.ColSpan = 0;
                    this.txtisPrice.Value = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(model.LSPrice.ToString()).ToString())).ToString("0.00");
                }
                else
                {
                    this.chkisprice.Checked = false;
                    //this.txtlsprice.Style.Add("display", "none");
                    //this.splsprice.Style.Add("display", "none");
                    //this.lbllsprice.ColSpan = 3;
                }
                this.hrImgPath.Value = model.Pic;//图片
                this.txtTitle.Value = model.Title;//标题
                this.txtIndex.Value = model.IsIndex == 0 ? "" : model.IsIndex.ToString();//排序号
                this.txtHideInfo1.Value = model.HideInfo1;//隐私1
                this.txtHideInfo2.Value = model.HideInfo2;//隐私2
                this.editor1.Text = model.memo != "" ? model.memo : model.Details;//描述
                ViewState["createdate"] = model.CreateDate;//创建时间
                ViewState["createuser"] = model.CreateUserID;//创建人
                ViewState["categoryid"] = model.CategoryID;//类别id
                string cateName = string.Empty;//商品分类
                string cateId = string.Empty;//商品分类ID
                Hi.Model.BD_GoodsCategory model3 = new Hi.BLL.BD_GoodsCategory().GetModel(Convert.ToInt32(model.CategoryID));
                if (model3 != null)
                {
                    //txtCategory.treeName = model3.CategoryName;
                    //txtCategory.treeId = model3.ID.ToString();
                }
                StringBuilder html = new StringBuilder();

                string checkeds = string.Empty;

                html.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width:1050px;margin:0 auto;\"><thead><tr><th class=\"key\"></th>");

                List<Hi.Model.BD_GoodsInfo> goodsinfolist = new Hi.BLL.BD_GoodsInfo().GetList("", " dr<>1 and compid=" + this.CompID + " and goodsid=" + model.ID, "");
                if (goodsinfolist.Count > 0)
                {
                    int z = 0;//goodsinfo 数量
                    foreach (Hi.Model.BD_GoodsInfo item in goodsinfolist)
                    {
                        z++;
                        string goodsInfo = item.ValueInfo;
                        ArrayList listName = new ArrayList();
                        ArrayList listValue = new ArrayList();
                        if (!Util.IsEmpty(goodsInfo))
                        {
                            string[] list = goodsInfo.Split('；');

                            for (int i = 0; i < list.Length; i++)
                            {
                                if (!Util.IsEmpty(list[i]))
                                {
                                    listName.Add(list[i].Split(':')[0]);
                                    listValue.Add(list[i].Split(':')[1]);
                                }
                            }
                        }
                        //else
                        //{
                        //    this.trProduct.Visible = false;//没有规格属性时 不显示多规格tr
                        //}
                        string style = string.Empty;
                        if ("1" == "1")
                        {
                            style = "style='display:none'";
                        }
                        if (z == 1)//第一次时加载th
                        {
                            if (listName.Count == 0)
                            {
                                html.Append("<th></th>");
                            }
                            for (int x = 0; x < listName.Count; x++)
                            {
                                html.Append("<th>" + listName[x] + "</th>");
                            }
                            html.Append("<th class=\"tle2 t3\">商品编码</th><th class=\"t3\">销售价格(元)</th><th class=\"t3 kc\"  " + style + "><i class=\"red\">*</i>库存</th><th class=\"t5\" " + style + ">批次号</th><th class=\"t5\" " + style + ">有效期</th><th  class=\"t5\">是否上架</th><th  class=\"t5\">操作</th></tr></thead><tbody>");
                        }
                        string inputType = "";//当商品处于被删除是的状态
                        string delete = "";//当商品处于被删除 时 用来控制操作按钮
                        string hf = "none";//当商品处于被删除 时 用来控制操作按钮
                        if (item.dr == 2)
                        {
                            inputType = "disabled='disabled'";
                            delete = "none";
                            hf = "";
                            html.Append("<tr class=\"disabled\"><td class=\"key odd trOp\">" + z + "</td>");
                        }else
                        html.Append("<tr><td class=\"key odd trOp\">" + z + "</td>");
                        if (listValue.Count == 0)
                        {
                            html.Append("<td class=\"tc mulspec1Value trOp\"><input "+ inputType + " type=\"text\" style=\"border: 0px;width: 100px; text-align: center;\" class=\"dataBox\" readonly=\"readonly\" name=\"value1\"></td>");
                        }
                        for (int y = 0; y < listValue.Count; y++)
                        {
                            html.Append("<td class=\"tc mulspec1Value trOp\"><input " + inputType + "  type=\"text\" style=\"border: 0px;width: 100px; text-align: center;\" class=\"dataBox\" readonly=\"readonly\" name=\"value" + Convert.ToInt32(y + 1) + "\" value=\"" + listValue[y] + "\"></td>");
                        }
                        if (item.IsOffline == 1)
                        {
                            checkeds = "checked=\"checked\"";
                        }
                        else {
                            checkeds = "";
                        }
                        string Validdate = item.Validdate == DateTime.MinValue ? "" : item.Validdate.ToString("yyyy-MM-dd");
                        html.Append("<td class=\"trOp\"><input " + inputType + "  type=\"text\" style=\"width:120px;\" class=\"dataBox txtCode\" id=\"Text2\" name=\"txtCode\" value=\"" + item.BarCode + "\"  maxlength=\"15\"></td><td class=\"trOp\"><input " + inputType + "  type=\"text\" onkeyup=\"KeyInt2(this);\" style=\"width:100px;\" class=\"dataBox txtPrices\" id=\"Text1\" maxlength=\"10\" name=\"txtPrices\" value=\"" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(item.TinkerPrice.ToString()).ToString())).ToString("0.00") + "\"></td><td class=\"trOp\" " + style + "><input " + inputType + "  type=\"text\" style=\"width:100px;\" class=\"dataBox txtInventory\" onkeyup=\"KeyInt2(this);\" id=\"Text2\" name=\"txtInventory\" value=\"" + item.Inventory + "\" maxlength=\"11\"></td><td class=\"trOp\" " + style + "><input name=\"txtBatchNO\" type=\"text\"  style=\"width: 100px;\" class=\"dataBox txtBatchNO\" maxlength=\"11\" value=\"" + item.Batchno + "\"/></td><td class=\"trOp\"><input name=\"txtvalidDate\" onclick=\"WdatePicker()\" readonly=\"readonly\" type=\"text\" style=\"width: 100px;\" class=\"dataBox txtvalidDate\" maxlength=\"11\" value=\"" + Validdate + "\"/></td><td class=\"trOp\" " + style + "><div class=\"tc\"><input " + inputType + "  type=\"checkbox\"  name=\"isOffline\" value=\"" + item.IsOffline + "\"  id=\"checks-" + z + "\" class=\"r-check\" " + checkeds + "/><label " + inputType + "  for=\"checks-" + z + "\"></label></div> <input type=\"hidden\" value=\"" + item.IsOffline + "\" name=\"hidIsOffline\" /><input type=\"hidden\" value=\"" + item.ID + "\" name=\"hidId\" class=\"deleteIDlist\"/></label></td><td class=\"trOp\"><a href=\"javascript:;\" class=\"theme-color delete " + delete + "\"><i class=\"del-i\"></i></a><a href=\"javascript:;\" class=\"theme-color restore " + hf + "\"><i class=\"pre-i\"></i></a></td></tr>");
                    }
                }
                html.Append("</tbody></table>");
                ClientScript.RegisterStartupScript(this.GetType(), "msgss", "<script>$(function(){ $(\".productListBox\").html(\"" + html.ToString().Replace("\"", "'") + "\");$(\".mulSpecList\").html(\"" + html2.ToString().Replace("\"", "'") + "\")})</script>");
            }
            // }
        }
        catch (Exception ex)
        {

            JScript.AlertMethod(this, "出错了", JScript.IconOption.错误, "function(){location.href='GoodsInfoList.aspx';}");
            return;
        }
    }
    /// <summary>
    /// 自定义字段
    /// </summary>
    private void ZiDingYi(int keyId)
    {
        string value = string.Empty;
        string valueList = string.Empty;
        // Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(keyId);
        // if (model != null)
        // {
        Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(keyId);
        if (model2 != null)
        {
            valueList = model2.Value1 + "," + model2.Value2 + "," + model2.Value3 + "," + model2.Value4 + "," + model2.Value5;
        }
        //}
        List<Hi.Model.SYS_SysName> l = new Hi.BLL.SYS_SysName().GetList("", "isnull(dr,0)=0 and compId=" + this.CompID + " and name='商品自定义字段'", "");//自定义字段
        if (l.Count > 0)
        {
            StringBuilder htmlstr = new StringBuilder();
            string strname = l[0].Value;
            if (strname != "")
            {
                int count = strname.Split(',').Length;
                for (int i = 0; i < count; i++)
                {
                    if (!Util.IsEmpty(valueList))
                    {
                        value = "  value=\"" + valueList.Split(',')[i] + "\"";
                    }
                    htmlstr.Append("<li class=\"lb fl none gd\"><i class=\"name\">" + strname.Split(',')[i] + "</i><input type=\"text\" class=\"box1 txtValue\" maxlength=\"100\" style=\"margin-left:4px; \" id=\"txtValue\" name=\"txtValue\" " + value + "></li>");
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "msgs", "<script>$(function(){ $(\"#tb\").before(\"" + htmlstr.ToString().Replace("\"", "'") + "\")})</script>");
        }
    }
    /// <summary>
    /// 得到sku信息
    /// </summary>
    /// <param name="list"></param>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <returns></returns>
    public string GetGoodsSku(string[] list, string[] list1, string[] list2, string guigeName, string goodsid)
    {
        List<string[]> al = new List<string[]>();
        if (list != null)
        {
            al.Add(list);
        }
        if (list1 != null)
        {
            al.Add(list1);
        }
        if (list2 != null)
        {
            al.Add(list2);
        }
        string[] guigelist = { };
        if (guigeName != "")
        {
            guigelist = guigeName.Split(',');//规格名称
        }
        StringBuilder html = new StringBuilder();
        string[] mmm;
        mmm = Common.bianli(al);

        if (mmm != null)
        {
            html.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width:1050px;margin:0 auto;\">");
            if (mmm.Length >= 3)
            {
                //this.addSpec.Style["display"] = "none";
            }
            for (int i = 0; i < mmm.Length; i++)
            {
                string style = string.Empty;
                if (true)
                {
                    style = "style='display:none'";
                }
                if (i == 0)
                {
                    html.Append("<thead><tr><th class=\"key\"></th>");
                    for (int x = 0; x < mmm[i].Split(',').Length; x++)//规格的个数guigelist.Length
                    {
                        if (guigelist.Length != 0)
                        {
                            html.Append("<th>" + guigelist[x].Trim() + "</th>");
                        }
                        else
                        {
                            html.Append("<th></th>");
                        }
                    }
                    html.Append("<th class=\"tle2 t3\">商品编码</th><th class=\"t3\">销售价格(元)</th><th class=\"t3 kc\"  " + style + "><i class=\"red\">*</i>库存</th><th class=\"t5\" " + style + ">批次号</th><th class=\"t5\" " + style + ">有效期</th><th  class=\"t5\">是否上架</th><th  class=\"t5\">操作</th></tr></thead>");
                    html.Append("<tbody>");
                }
                //else
                //{
                string str = string.Empty;//规格值接受 用于查询价格
                for (int x = 0; x < mmm[i].Split(',').Length; x++)//规格的个数guigelist.Length
                {
                    if (guigelist.Length != 0)
                    {
                        str += guigelist[x] + ":" + mmm[i].Split(',')[x] + "；";
                    }
                }
                // Hi.Model.BD_GoodsInfo goodsinfo = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(infoid));
                List<Hi.Model.BD_GoodsInfo> infolist = new Hi.BLL.BD_GoodsInfo().GetList("", " dr<>1 and compid=" + this.CompID + " and valueinfo='" + str + "' and goodsid=" + goodsid, "");

                string inputType = "";//当商品处于被删除是的状态
                string delete = "";//当商品处于被删除 时 用来控制操作按钮
                string hf = "none";//当商品处于被删除 时 用来控制操作按钮
                if (infolist.Count > 0)
                {
                    if (infolist[0].dr == 2)
                    {
                        inputType = "disabled='disabled'";
                        delete = "none";
                        hf = "";
                        html.Append("<tr class=\"disabled\"><td class=\"key odd\">" + Convert.ToInt32(Convert.ToInt32(i) + Convert.ToInt32(1)) + "</td>");

                    }
                    else
                        html.Append("<tr><td class=\"key odd\">" + Convert.ToInt32(Convert.ToInt32(i) + Convert.ToInt32(1)) + "</td>");

                }
                else
                    html.Append("<tr><td class=\"key odd\">" + Convert.ToInt32(Convert.ToInt32(i) + Convert.ToInt32(1)) + "</td>");
                for (int z = 0; z < mmm[i].Split(',').Length; z++)//规格值
                {
                    if (mmm[i].Split(',')[z] != "")
                    {
                        html.Append("<td class=\"tc mulspec1Value trOp\"><input "+ inputType + " type=\"text\" name=\"value" + Convert.ToInt32(Convert.ToInt32(z) + Convert.ToInt32(1)) + "\"  readonly=\"readonly\" class=\"dataBox\"    style=\"border: 0px; text-align: center;width: 100px;\" value=\"" + mmm[i].Split(',')[z] + "\" /></td>");
                    }
                    else
                    {
                        html.Append("<td class=\"tc mulspec1Value trOp\"><input " + inputType + " type=\"text\" name=\"value" + Convert.ToInt32(Convert.ToInt32(z) + Convert.ToInt32(1)) + "\"  readonly=\"readonly\" class=\"dataBox\"    style=\"border: 0px; text-align: center;width: 100px;\" /></td>");
                    }
                }
                string Validdate = (infolist.Count > 0) ? infolist[0].Validdate == DateTime.MinValue ? "" : infolist[0].Validdate.ToString("yyyy-MM-dd"):"";
                html.Append("<td class=\"trOp\"><input " + inputType + " type=\"text\" style=\"width:120px;\" class=\"dataBox txtCode\" id=\"Text2\" name=\"txtCode\" value=\"" + (infolist.Count > 0 ? infolist[0].BarCode : GoodsCode((i + 1).ToString())) + "\"  maxlength=\"15\"></td><td class=\"trOp\"><input " + inputType + " type=\"text\" onkeyup=\"KeyInt2(this);\" style=\"width:100px;\" class=\"dataBox txtPrices\" id=\"Text1\" name=\"txtPrices\" maxlength=\"10\" value=\"" + (infolist.Count > 0 ? decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(infolist[0].TinkerPrice.ToString()).ToString())).ToString("0.00") : "") + "\"/></td><td class=\"trOp\"  " + style + "><input " + inputType + " type=\"text\" style=\"width:100px;\" class=\"dataBox txtInventory\" onkeyup=\"KeyInt2(this);\" id=\"Text2\" name=\"txtInventory\" maxlength=\"11\" value=\"" + (infolist.Count > 0 ? infolist[0].Inventory.ToString("0.00") : "") + "\" ></td><td class=\"trOp\" " + style + "><input name=\"txtBatchNO\" type=\"text\"  style=\"width: 100px;\" class=\"dataBox txtBatchNO\" maxlength=\"11\" value=\"" + (infolist.Count > 0 ? infolist[0].Batchno : "") + "\"/></td><td class=\"trOp\" " + style + "><input name=\"txtvalidDate\" onclick=\"WdatePicker()\" readonly=\"readonly\" type=\"text\" style=\"width: 100px;\" class=\"dataBox txtvalidDate\" maxlength=\"11\" value=\"" + Validdate + "\"/></td><td class=\"trOp\"><div class=\"tc\"><input " + inputType + " type=\"checkbox\"  name=\"isOffline\" value=\"" + (infolist.Count > 0 ? infolist[0].IsOffline : 1) + "\" checked=\"checked\" id=\"checks-" + i + "\" class=\"r-check\" /><label for=\"checks-" + i + "\"></label></div> <input type=\"hidden\" value=\"" + (infolist.Count > 0 ? infolist[0].IsOffline : 1) + "\" name=\"hidIsOffline\" /> <input value=\"" + (infolist.Count > 0 ? infolist[0].ID : 1) + "\" name=\"hidId\" class=\"deleteIDlist\" type=\"hidden\"/></td><td class=\"trOp\"><a href=\"javascript:;\" class=\"theme-color delete " + delete + "\"><i class=\"del-i\"></i></a><a href=\"javascript:;\" class=\"theme-color restore " + hf + "\"><i class=\"pre-i\"></i></a></td></tr>");
                // }
            }
            html.Append("</tbody></table>");
        }
        return html.ToString();
    }
    /// <summary>
    /// 继承Json序列化类  指定序列化字段   add by kb
    /// </summary>
    public class CusTomPropsContractResolver : DefaultContractResolver
    {
        //[JsonProperty(PropertyName = "")]
        string[] props = null;

        bool retain;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段  false：表示props是要排除的字段</param>
        public CusTomPropsContractResolver(string[] props, bool retain = true)
        {
            //指定要序列化属性的清单
            this.props = props;

            this.retain = retain;
        }

        protected override IList<JsonProperty> CreateProperties(Type type,

        Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
            //只保留清单有列出的属性
            return list.Where(p =>
            {
                if (retain)
                {
                    return props.Contains(p.PropertyName);
                }
                else
                {
                    return !props.Contains(p.PropertyName);
                }
            }).ToList();
        }
    }
    /// <summary>
    /// 显示计量单位
    /// </summary>
    private string GetDoc()
    {
        List<Hi.Model.BD_DefDoc_B> ListDef = new Hi.BLL.BD_DefDoc_B().GetList("AtVal,ID", " dr=0 and Compid=" + CompID + " and AtName='计量单位' ", "");
        JsonSerializerSettings jsetting = new JsonSerializerSettings();
        jsetting.ContractResolver = new CusTomPropsContractResolver(new string[] { "AtVal", "ID" });
        return JsonConvert.SerializeObject(ListDef, Formatting.Indented, jsetting);
    }
    public string DelDoc(string Key)
    {
        Common.ResultMessage Msg = new Common.ResultMessage();
        List<Hi.Model.BD_DefDoc_B> ListDef = new Hi.BLL.BD_DefDoc_B().GetList("", " Id=" + Key + " and AtName='计量单位' ", "");
        if (ListDef.Count > 0)
        {
            ListDef[0].modifyuser = UserID;
            ListDef[0].ts = DateTime.Now;
            ListDef[0].dr = 1;
            new Hi.BLL.BD_DefDoc_B().Update(ListDef[0]);
            Msg.result = true;
            Msg.code = "删除成功";
        }
        else
        {
            Msg.code = "未查找到数据";
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }
    /// <summary>
    /// 商品标签
    /// </summary>
    /// <returns></returns>
    public void GetGoodsLabels(int keyId)
    {
        //Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(keyId);
        //if (model != null)
        //{
        //    keyId = model.GoodsID;
        //}
        string[] list = { };
        StringBuilder str = new StringBuilder();
        List<Hi.Model.SYS_SysName> l = new Hi.BLL.SYS_SysName().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and name='商品标签管理'", "");
        if (l.Count > 0)
        {
            foreach (Hi.Model.SYS_SysName item in l)
            {
                string labelname = item.Value;
                if (labelname.Trim() != "")
                {
                    list = labelname.Split(',');
                }
            }
            List<Hi.Model.BD_GoodsLabels> ll = new Hi.BLL.BD_GoodsLabels().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsId=" + keyId, "");
            for (int i = 0; i < list.Length; i++)
            {
                if (keyId != 0)
                {
                    if (ll.Count > 0)
                    {
                        int z = 0;//不相同的情况
                        foreach (Hi.Model.BD_GoodsLabels item in ll)
                        {
                            if (list[i] == item.LabelName)
                            {
                                str.Append("<i class=\"t productLabelItem cur\"><input type=\"checkbox\" name=\"labelcheckbox\" value=\"" + list[i] + "\" style=\"display: none\" checked=\"checked\" />" + list[i] + "</i></ItemTemplate>");
                                break;
                            }
                            z++;
                        }
                        if (z == ll.Count)
                        {
                            str.Append("<i class=\"t productLabelItem\"><input type=\"checkbox\" name=\"labelcheckbox\" value=\"" + list[i] + "\" style=\"display: none\"  />" + list[i] + "</i></ItemTemplate>");
                        }
                    }
                    else
                    {
                        str.Append("<i class=\"t productLabelItem\"><input type=\"checkbox\" name=\"labelcheckbox\" value=\"" + list[i] + "\" style=\"display: none\"  />" + list[i] + "</i></ItemTemplate>");
                    }
                }
                else
                {
                    str.Append("<i class=\"t productLabelItem\"><input type=\"checkbox\" name=\"labelcheckbox\" value=\"" + list[i] + "\" style=\"display: none\"  />" + list[i] + "</i></ItemTemplate>");
                }
            }

        }
        this.DivLabel.InnerHtml = str.ToString();
    }
    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        try
        {
            string goodsName = this.txtGoodsName.Value.Trim();//商品名称
            string goodsCategoryID = Common.NoHTML(hid_txt_product_class.Value.Trim());// this.hid_product_class.Value.Trim();//商品类别ID
            string unit = Common.NoHTML(this.txtunittex.Value.Trim());// this.ddlUnit.SelectedItem.Text;//单位
            string price = this.txtPrice.Value.Trim() == "" ? "0" : Common.NoHTML(this.txtPrice.Value.Trim());//价格
            string pic = Common.NoHTML(this.hrImgPath.Value.Trim().Trim());//图片
            string pic2 = "X" + Common.NoHTML(this.hrImgPath.Value.Trim());//图片2
            string pic3 = "D" + Common.NoHTML(this.hrImgPath.Value.Trim());//图片3
            string[] imglist = { };
            if (Request["hidImg"] != null && Request["hidImg"] != "")
            {
                imglist = Request["hidImg"].Split(',');//图片相册
            }
            string title = Common.NoHTML(this.txtTitle.Value.Trim());//标题
            string index = Common.NoHTML(this.txtIndex.Value.Trim()) == "" ? "0" : txtIndex.Value.Trim();//排序
            string hideInfo1 = Common.NoHTML(this.txtHideInfo1.Value.Trim());//隐私1
            string hideInfo2 = Common.NoHTML(this.txtHideInfo2.Value.Trim());//隐私2
            string details = editor1.Text.Trim();//描述
            int recommend = 0;//不推荐不显示
            if (this.chkshow.Checked)
            {
                if (this.chkRecommend.Checked)
                {
                    recommend = 2;//推荐
                }
                else
                {
                    recommend = 1;//显示
                }
            }
            //this.chkRecommend.Checked == true ? 2 : 1;//是否推荐
            int IsAttribute = this.chkProduct.Checked == true ? 1 : 0;//是否启用规格属性
            int templateId = this.ddlTemplate.SelectedValue == "" ? 0 : Convert.ToInt32(this.ddlTemplate.SelectedValue.Trim());
            object value =Common.NoHTML( Request["txtValue"]);//自定义字段
            object prices = Request["txtPrices"].ToString() == "" ? "0" : Common.NoHTML(Request["txtPrices"]);//销售价格
            object selectized = Common.NoHTML(Request["selectized"]);//属性值
            object mulSpecName = Common.NoHTML(Request["mulSpecName"]);//规格
            object value2 = Common.NoHTML(Request["value2"]);//规格2 sku
            object value1 = Common.NoHTML(Request["value1"]);//规格1 sku
            object value3 = Common.NoHTML(Request["value3"]);//规格3 sku
            object offline = Common.NoHTML(Request["hidIsOffline"]);//上下架
            object gsinfoid = Common.NoHTML(Request["OdlIDList"]);//goodsinfoid !=0 属于修改 =0 添加
            object labelname = Common.NoHTML(Request["labelcheckbox"]);//商品标签
            object goodsInfoCode = Common.NoHTML(Request["txtCode"]);//商品sku编码
            object inventory = Common.NoHTML(Request["txtInventory"]);//商品库存
            //by update szj 新增批次号、有效期
            object batchNO = Common.NoHTML(Request["txtBatchNO"]);//批次号
            object validDate = Common.NoHTML(Request["txtvalidDate"]);//有效期

            string deleteIDlist = Request["deleteIDlist"];//手动删除的商品ID

            string registeredCertificate = Common.NoHTML(Request["HidFfileName2"].ToString().Trim());//商品注册资格证
            DateTime validity = this.validDate2.Value.Trim() != "" ? Convert.ToDateTime(this.validDate2.Value.Trim()) : DateTime.MinValue;//商品注册资格证有效期


            //对勾选了恢复的商品 做恢复处理
            List<Hi.Model.BD_GoodsInfo> ls = new Hi.BLL.BD_GoodsInfo().GetList("", "dr=2 and compid=" + this.CompID + " and goodsid=" + KeyID + " and id  in(" + gsinfoid.ToString() + ")", "");
            if (ls.Count > 0)
            {

                foreach (Hi.Model.BD_GoodsInfo item in ls)
                {
                    Hi.Model.BD_GoodsInfo model4 = new Hi.BLL.BD_GoodsInfo().GetModel(item.ID);
                    model4.dr = 0;
                    model4.ts = DateTime.Now;
                    model4.modifyuser = this.UserID;
                    new Hi.BLL.BD_GoodsInfo().Update(model4);
                }

            }




            Hi.Model.BD_Goods model = null;
            //  return;
            int goodsId = KeyID;//goodsId
            if (KeyID != 0)
            {
                // Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(KeyID);
                // if (model2 != null)
                // {
                model = new Hi.BLL.BD_Goods().GetModel(KeyID);
                //   goodsId = model2.GoodsID;
                // }
            }
            else
            {
                model = new Hi.Model.BD_Goods();
                model.IsAttribute = IsAttribute;
                model.TemplateId = templateId;
            }
            model.registeredCertificate = registeredCertificate;
            model.validity = validity;
            model.CompID = this.CompID;
            model.GoodsName = goodsName;
            model.GoodsCode = "";
            model.CategoryID = Convert.ToInt32(goodsCategoryID);
            model.Unit = unit;
            model.SalePrice = Convert.ToDecimal(price);
            model.IsIndex = Convert.ToInt32(index);
            model.IsRecommended = recommend;
            if (this.chkisprice.Checked)
            {
                model.IsLS = 1;
                model.LSPrice = Convert.ToDecimal(this.txtisPrice.Value.Trim());
            }
            else
            {
                model.IsLS = 0;
            }
            if (offline != null)
            {
                //if (KeyID != 0)
                //{
                //    List<Hi.Model.BD_GoodsInfo> ll = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and   goodsId=" + KeyID + " and isnull(IsOffline,1)=1", "");// and id<>" + KeyID, "");
                //    if (ll.Count > 0)
                //    {
                //        model.IsOffline = 1;
                //    }
                //    else
                //    {
                //        model.IsOffline = 0;
                //    }
                //}
                //else
                //{
                if (offline.ToString().IndexOf("1") == -1)
                {
                    model.IsOffline = 0;
                }
                else
                {
                    model.IsOffline = 1;
                }
                //}
            }
            else
            {
                model.IsOffline = 1;
            }
            model.IsSale = 0;
            model.Pic = pic;
            model.Pic2 = pic2;
            model.Pic3 = pic3;
            model.Title = title;
            model.HideInfo1 = hideInfo1;
            model.HideInfo2 = hideInfo2;
            model.Details = details;

            model.IsEnabled = 1;
            model.CreateDate = DateTime.Now;
            model.CreateUserID = this.UserID;
            model.ts = DateTime.Now;
            model.modifyuser = this.UserID;
            if (value != null)
            {
                if (!Util.IsEmpty(value.ToString()))
                {
                    string[] valuelist = value.ToString().Split(',');
                    for (int i = 0; i < valuelist.Length; i++)
                    {
                        switch (i)
                        {
                            case 0: model.Value1 = valuelist[i];
                                break;
                            case 1: model.Value2 = valuelist[i];
                                break;
                            case 2: model.Value3 = valuelist[i];
                                break;
                            case 3: model.Value4 = valuelist[i];
                                break;
                            case 4: model.Value5 = valuelist[i];
                                break;
                        }
                    }
                }
            }

            if (KeyID != 0)
            {
                model.CreateUserID = Convert.ToInt32(ViewState["createuser"]);
                model.CreateDate = Convert.ToDateTime(ViewState["createdate"]);
                bool bol = new Hi.BLL.BD_Goods().Update(model, Tran);
            }
            else
            {
                goodsId = new Hi.BLL.BD_Goods().Add(model, Tran);
            }
            ////////////BD_GoodsLabels开始插入//////////////////////
            if (labelname != null)
            {
                if (!Util.IsEmpty(labelname.ToString()))
                {
                    string[] labellist = labelname.ToString().Split(',');
                    if (KeyID != 0)
                    {
                        new Hi.BLL.BD_GoodsLabels().Delete(goodsId, this.CompID, Tran);
                    }
                    for (int i = 0; i < labellist.Length; i++)
                    {
                        if (labellist[i] != "")
                        {
                            Hi.Model.BD_GoodsLabels modellabels = new Hi.Model.BD_GoodsLabels();
                            modellabels.CompID = this.CompID;
                            modellabels.GoodsID = goodsId;
                            modellabels.ts = DateTime.Now;
                            modellabels.LabelName = labellist[i].ToString();
                            modellabels.modifyuser = this.UserID;
                            new Hi.BLL.BD_GoodsLabels().Add(modellabels, Tran);
                        }
                    }
                }
            }
            ////////////BD_ImageList开始插入//////////////////////
            if (KeyID != 0)
            {
                new Hi.BLL.BD_ImageList().Delete(goodsId, this.CompID, Tran);
            }
            for (int i = 0; i < imglist.Length; i++)
            {
                if (imglist[i] != "")
                {
                    Hi.Model.BD_ImageList modelImg = new Hi.Model.BD_ImageList();
                    modelImg.CompID = this.CompID;
                    modelImg.GoodsID = goodsId;
                    modelImg.Pic = imglist[i];
                    modelImg.Pic2 = "X" + imglist[i];
                    modelImg.Pic3 = "D" + imglist[i];
                    modelImg.IsIndex = 0;
                    modelImg.CreateDate = DateTime.Now;
                    modelImg.CreateUserID = this.UserID;
                    modelImg.ts = DateTime.Now;
                    modelImg.modifyuser = this.UserID;
                    new Hi.BLL.BD_ImageList().Add(modelImg, Tran);
                }
            }
            ////////////BD_GoodsInfo开始插入//////////////////////
            //if (KeyID != 0)
            //{
            //    Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(KeyID);
            //    if (model2 != null)
            //    {
            //        model2.modifyuser = this.UserID;
            //        model2.ts = DateTime.Now;
            //        model2.CompID = this.CompID;
            //        if (goodsInfoCode != null)
            //        {
            //            if (!Util.IsEmpty(goodsInfoCode.ToString()))
            //            {
            //                if (goodsInfoCode.ToString().Split(',').Length == 1)
            //                {
            //                    model2.BarCode = goodsInfoCode.ToString().Split(',')[0];
            //                }
            //            }
            //        }
            //        model2.IsOffline = Convert.ToInt32(offline.ToString().Split(',')[0]);
            //        model2.SalePrice = Convert.ToDecimal(price);
            //        model2.TinkerPrice = Convert.ToDecimal(prices.ToString().Split(',')[0]);
            //        model2.Inventory = Convert.ToDecimal(inventory.ToString().Split(',')[0]);
            //        new Hi.BLL.BD_GoodsInfo().Update(model2, Tran);
            //    }
            //}
            //else
            //{

            if (prices != null)
            {
                if (!Util.IsEmpty(prices.ToString()))
                {
                    string[] pricelist = prices.ToString().Split(',');
                    //去此之外的 全部删除
                  
                        List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", " dr<>1  and compid=" + this.CompID + " and goodsid=" + KeyID + " and id not in(" + gsinfoid.ToString() + ")", "");
                        if (l.Count > 0)
                        {

                            foreach (Hi.Model.BD_GoodsInfo item in l)
                            {
                                Hi.Model.BD_GoodsInfo model3 = new Hi.BLL.BD_GoodsInfo().GetModel(item.ID);
                                model3.dr = 1;
                                model3.ts = DateTime.Now;
                                model3.modifyuser = this.UserID;
                                new Hi.BLL.BD_GoodsInfo().Update(model3, Tran);
                            }

                        }

                    //对手动删除的商品 做可恢复标记
                    if (!string.IsNullOrWhiteSpace(deleteIDlist))
                    {
                        List<Hi.Model.BD_GoodsInfo> DeleteList = new Hi.BLL.BD_GoodsInfo().GetList("", " compid=" + this.CompID + " and goodsid=" + KeyID + " and id  in(" + deleteIDlist + ")", "");
                        if (DeleteList.Count > 0)
                        {

                            foreach (Hi.Model.BD_GoodsInfo item in DeleteList)
                            {
                                Hi.Model.BD_GoodsInfo model3 = new Hi.BLL.BD_GoodsInfo().GetModel(item.ID);
                                model3.dr = 2;
                                model3.ts = DateTime.Now;
                                model3.modifyuser = this.UserID;
                                new Hi.BLL.BD_GoodsInfo().Update(model3, Tran);
                            }

                        }
                    }

                    //然后再修改现有的goodsinfo
                    for (int i = 0; i < pricelist.Length; i++)
                    {
                        Hi.Model.BD_GoodsInfo model2 = null;
                        if (!Util.IsEmpty(pricelist[i]))
                        {
                            if (KeyID != 0)
                            {
                                if (gsinfoid.ToString().Split(',')[i].ToString() != "1"&& gsinfoid.ToString().Split(',')[i].ToString() != "0")
                                {
                                    model2 = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(gsinfoid.ToString().Split(',')[i]));
                                }
                                else
                                {
                                    model2 = new Hi.Model.BD_GoodsInfo();
                                    model2.CreateDate = DateTime.Now;
                                    model2.CreateUserID = this.UserID;
                                }
                            }
                            else
                            {
                                model2 = new Hi.Model.BD_GoodsInfo();
                                model2.CreateDate = DateTime.Now;
                                model2.CreateUserID = this.UserID;
                            }
                            model2.CompID = this.CompID;
                            model2.GoodsID = goodsId;
                            if (goodsInfoCode != null)
                            {
                                if (!Util.IsEmpty(goodsInfoCode.ToString()))
                                {
                                    if (goodsInfoCode.ToString().Split(',').Length == pricelist.Length)
                                    {
                                        if (goodsInfoCode.ToString().Split(',')[i].Length == 11)
                                        {
                                            string code = goodsInfoCode.ToString().Split(',')[i].Substring(5);
                                            if (code.Length == 6)
                                            {
                                                int xx = 0;
                                                try
                                                {
                                                    xx = Convert.ToInt32(code);
                                                }
                                                catch (Exception)
                                                {
                                                    xx = 0;
                                                }
                                                if (xx != 0)
                                                {
                                                    ViewState["code"] = xx.ToString();
                                                }
                                            }
                                        }
                                        model2.BarCode = goodsInfoCode.ToString().Split(',')[i];
                                    }
                                }
                            }
                            if (value1 != null)
                            {
                                if (!Util.IsEmpty(value1.ToString()))
                                {
                                    model2.Value1 = value1.ToString().Split(',')[i];
                                }
                            }
                            if (value2 != null)
                            {
                                if (!Util.IsEmpty(value2.ToString()))
                                {
                                    model2.Value2 = value2.ToString().Split(',')[i];
                                }
                            }
                            if (value3 != null)
                            {
                                if (!Util.IsEmpty(value3.ToString()))
                                {
                                    model2.Value3 = value3.ToString().Split(',')[i];
                                }
                            }
                            string str = string.Empty;
                            if (mulSpecName != null)
                            {
                                if (!Util.IsEmpty(mulSpecName.ToString()))
                                {
                                    for (int x = 0; x < mulSpecName.ToString().Split(',').Length; x++)
                                    {
                                        if (!Util.IsEmpty(mulSpecName.ToString().Split(',')[x]))
                                        {
                                            if (value1 != null && x == 0)
                                            {
                                                if (!Util.IsEmpty(value1.ToString()))
                                                {
                                                    str += mulSpecName.ToString().Split(',')[x] + ":" + value1.ToString().Split(',')[i] + "；";
                                                }
                                            }
                                            if (value2 != null && x == 1)
                                            {
                                                if (!Util.IsEmpty(value2.ToString()))
                                                {
                                                    str += mulSpecName.ToString().Split(',')[x] + ":" + value2.ToString().Split(',')[i] + "；";
                                                }
                                            }
                                            if (value3 != null && x == 2)
                                            {
                                                if (!Util.IsEmpty(value3.ToString()))
                                                {
                                                    str += mulSpecName.ToString().Split(',')[x] + ":" + value3.ToString().Split(',')[i] + "；";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            model2.IsOffline = Convert.ToInt32(offline.ToString().Split(',')[i]);
                            model2.ValueInfo = str;
                            model2.SalePrice = Convert.ToDecimal(price);
                            model2.TinkerPrice = Convert.ToDecimal(prices.ToString().Split(',')[i]);
                            if (inventory.ToString().Split(',')[i] != "")
                            {
                                model2.Inventory = Convert.ToDecimal(inventory.ToString().Split(',')[i]);
                            }
                            if (inventory.ToString().Split(',')[i] != "")
                            {
                                model2.Batchno = batchNO.ToString().Split(',')[i];
                            }
                            if (inventory.ToString().Split(',')[i] != "")
                            {
                                model2.Validdate = validDate.ToString().Split(',')[i].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(validDate.ToString().Split(',')[i]);
                            }

                            model2.IsEnabled = true;

                            model2.ts = DateTime.Now;
                            model2.modifyuser = this.UserID;
                            if (KeyID != 0)
                            {
                                if (gsinfoid.ToString().Split(',')[i].ToString() != "0"&& gsinfoid.ToString().Split(',')[i].ToString() != "1")
                                {
                                    new Hi.BLL.BD_GoodsInfo().Update(model2, Tran);

                                }
                                else
                                {
                                    new Hi.BLL.BD_GoodsInfo().Add(model2, Tran);
                                }
                            }
                            else
                            {
                                new Hi.BLL.BD_GoodsInfo().Add(model2, Tran);
                            }
                        }
                    }
                }
            }
            //}
            ////////////BD_GoodsAttrs开始插入//////////////////////
            if (KeyID != 0)
            {
                List<Hi.Model.BD_GoodsAttrs> l = new Hi.BLL.BD_GoodsAttrs().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsid=" + KeyID, "");
                if (l.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsAttrs item in l)
                    {
                        Hi.Model.BD_GoodsAttrs attrmodel = new Hi.BLL.BD_GoodsAttrs().GetModel(item.ID);
                        attrmodel.dr = 1;
                        attrmodel.ts = DateTime.Now;
                        attrmodel.modifyuser = this.UserID;
                        new Hi.BLL.BD_GoodsAttrs().Update(attrmodel, Tran);
                        List<Hi.Model.BD_GoodsAttrsInfo> ll = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsid=" + KeyID + " and attrsid=" + item.ID, "");
                        if (ll.Count > 0)
                        {
                            foreach (Hi.Model.BD_GoodsAttrsInfo item2 in ll)
                            {
                                Hi.Model.BD_GoodsAttrsInfo attrinfomodel = new Hi.BLL.BD_GoodsAttrsInfo().GetModel(item2.ID);
                                attrinfomodel.dr = 1;
                                attrinfomodel.ts = DateTime.Now;
                                attrinfomodel.modifyuser = this.UserID;
                                new Hi.BLL.BD_GoodsAttrsInfo().Update(attrinfomodel, Tran);
                            }
                        }
                    }
                }
            }
            if (mulSpecName != null && selectized != null)
            {
                if (selectized.ToString() != "@@@@")
                {
                    if (!Util.IsEmpty(mulSpecName.ToString()))
                    {
                        for (int i = 0; i < mulSpecName.ToString().Split(',').Length; i++)
                        {
                            if (!Util.IsEmpty(mulSpecName.ToString().Split(',')[i]))
                            {
                                Hi.Model.BD_GoodsAttrs model3 = new Hi.Model.BD_GoodsAttrs();
                                model3.CompID = this.CompID;
                                model3.GoodsID = goodsId;
                                model3.AttrsName = mulSpecName.ToString().Split(',')[i].ToString();
                                model3.ts = DateTime.Now;
                                model3.modifyuser = this.UserID;
                                int attrsId = new Hi.BLL.BD_GoodsAttrs().Add(model3, Tran);

                                if (!Util.IsEmpty(selectized.ToString()))
                                {
                                    string[] selectizedlsit = selectized.ToString().Split(',');
                                    //for (int x = 0; x < selectizedlsit.Length; x++)
                                    //{
                                    if (!Util.IsEmpty(selectizedlsit[i]))
                                    {
                                        for (int y = 0; y < selectizedlsit[i].Replace("@@", ",").Split(',').Length; y++)
                                        {
                                            if (!Util.IsEmpty(selectizedlsit[i].Replace("@@", ",").Split(',')[y]))
                                            {
                                                Hi.Model.BD_GoodsAttrsInfo model4 = new Hi.Model.BD_GoodsAttrsInfo();
                                                model4.AttrsID = attrsId;
                                                model4.CompID = this.CompID;
                                                model4.GoodsID = goodsId;
                                                model4.modifyuser = this.UserID;
                                                model4.ts = DateTime.Now;
                                                model4.AttrsInfoName = selectizedlsit[i].Replace("@@", ",").Split(',')[y];
                                                int attrsInfoId = new Hi.BLL.BD_GoodsAttrsInfo().Add(model4, Tran);
                                            }
                                        }
                                    }
                                    // }
                                }
                            }
                        }
                    }
                }
            }


            ////////////////////////sys_syscode表插入//////////////////////
            //if (KeyID == 0)
            //{
            if (ViewState["code"] != null)
            {
                List<Hi.Model.SYS_SysCode> l = new Hi.BLL.SYS_SysCode().GetList("", "isnull(dr,0)=0 and compId=" + this.CompID + " and codeName='P" + this.CompID + "'", "", Tran);
                if (l.Count > 0)
                {

                    Hi.Model.SYS_SysCode model6 = new Hi.BLL.SYS_SysCode().GetModel(l[0].ID);
                    model6.CodeValue = ViewState["code"].ToString();
                    model6.ts = DateTime.Now;
                    model6.modifyuser = this.UserID;
                    new Hi.BLL.SYS_SysCode().Update(model6, Tran);
                }
                else
                {
                    Hi.Model.SYS_SysCode model5 = new Hi.Model.SYS_SysCode();
                    model5.CompID = this.CompID;
                    model5.CodeName = "P" + this.CompID;
                    model5.CodeValue = ViewState["code"].ToString();
                    model5.ts = DateTime.Now;
                    model5.dr = 0;
                    model5.modifyuser = this.UserID;
                    new Hi.BLL.SYS_SysCode().Add(model5, Tran);
                }
            }
            //}
            Tran.Commit();
            sAttr = "";
            Common com = new Common();
            model.ViewInfos = com.GoodsType(goodsId.ToString(), this.CompID.ToString());
            model.ID = goodsId;
            string strWhere = " GoodsID=" + goodsId + " and isnull(ValueInfo,'')='" + com.sAttr + "' and CompID=" + this.CompID + " and IsEnabled=1 and isnull(dr,0)=0";
            List<Hi.Model.BD_GoodsInfo> llll = new Hi.BLL.BD_GoodsInfo().GetList("", strWhere, "");
            if (llll != null && llll.Count > 0)
                model.ViewInfoID = llll[0].ID;

            new Hi.BLL.BD_Goods().Update(model);
            List<Hi.Model.BD_GoodsInfo> lllll = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compId=" + this.CompID + " and goodsId=" + goodsId, "");
            if (Request["nextstep"] + "" == "1")
            {
                Response.Redirect("GoodsInfo.aspx?nextstep=1&goodsId=" + Common.DesEncrypt(goodsId.ToString(), Common.EncryptKey) + "&goodsInfoId=" + lllll[0].ID, false);
            }
            else
            {
                Response.Redirect("GoodsInfo.aspx?goodsId=" + Common.DesEncrypt(goodsId.ToString(), Common.EncryptKey) + "&goodsInfoId=" + lllll[0].ID, false);
            }
        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMethod(this, (KeyID != 0 ? "商品编辑失败" : "商品新增失败"), JScript.IconOption.错误, "function(){location.href='goodsEdit.aspx?nextstep=" + Request["nextstep"].Trim() + "';}");
            return;
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }

    }
    /// <summary>
    /// 图片相册
    /// </summary>
    /// <returns></returns>
    public string GetImgList(string keyId)
    {
        int count = 0;
        StringBuilder html = new StringBuilder();
        List<Hi.Model.BD_ImageList> ll = new Hi.BLL.BD_ImageList().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsId=" + keyId, "");
        if (ll.Count > 0)
        {
            foreach (Hi.Model.BD_ImageList item in ll)
            {
                count++;
                html.Append("<div><p  draggable=\"true\"  style=\"margin:0 5px 5px 0; float: left;cursor: move;\" class=\"p" + count + "\"><img src=\"" + Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + item.Pic + "\" id=\"img" + count + "\" width=\"150\" height=\"150\" class=\"imgWrap\"  alt=\"图片\" /></p><a href=\"JavaScript:;\" class=\"delImg\" tip=\"" + item.Pic + "\" style=\"color:red; cursor: pointer; float: left; margin: 120px 0 0 -90px;display:none;\">删除</a><input type=\"hidden\" name=\"hidImg\" value=\"" + item.Pic + "\" id=\"hidImg" + count + "\" /></div>");
            }
        }
        return html.ToString();
    }
    /// <summary>
    /// 获取商品编码
    /// </summary>
    /// <returns></returns>
    public string GoodsCode(string i = "")
    {
        if (i == "")
        {
            List<Hi.Model.SYS_SysCode> l = new Hi.BLL.SYS_SysCode().GetList("", "isnull(dr,0)=0 and compId=" + this.CompID + "and codeName='P" + this.CompID + "'", "");
            if (l.Count > 0)
            {
                return l[0].CodeName + (Convert.ToInt32(l[0].CodeValue) + 1).ToString().PadLeft(6, '0');
            }
            return "P" + this.CompID + "000001";
        }
        else
        {
            List<Hi.Model.SYS_SysCode> l = new Hi.BLL.SYS_SysCode().GetList("", "isnull(dr,0)=0 and compId=" + this.CompID + " and codeName='P" + this.CompID + "'", "");
            if (l.Count > 0)
            {
                return l[0].CodeName + (Convert.ToInt32(l[0].CodeValue) + Convert.ToInt32(i)).ToString().PadLeft(6, '0');
            }
            return "P" + this.CompID + i.ToString().PadLeft(6, '0');
        }
    }

    /// <summary>
    /// 绑定  代理商分类 区域 
    /// </summary>
    public string GoodsType = string.Empty;//代理商分类数据源
    public void bind()
    {
        //绑定分类
        StringBuilder sbdis = new StringBuilder();
        List<Hi.Model.SYS_GType> gType1 = new Hi.BLL.SYS_GType().GetList("FullCode,ID,TypeName", "isnull(dr,0)=0 and  ParentId=0 and IsEnabled = 1", " id");
        if (gType1.Count > 0)
        {
            sbdis.Append("[");
            int count = 0;
            foreach (var item in gType1)
            {
                count++;

                string name1 = item.TypeName;
                if (item.TypeName.Length > 10)
                    name1 = item.TypeName.ToString().Substring(0, 10);
                sbdis.Append("{code:'" + item.ID + "',value: '" + item.ID + "',label: '" + name1 + "'");
                List<Hi.Model.SYS_GType> gType2 = new Hi.BLL.SYS_GType().GetList("FullCode,ID,TypeName", "isnull(dr,0)=0 and IsEnabled = 1  and ParentId=" + item.ID, "");
                if (gType2.Count > 0)
                {
                    sbdis.Append(",children: [");
                    int count2 = 0;
                    foreach (var item2 in gType2)
                    {
                        count2++;

                        string name2 = item2.TypeName;
                        if (item2.TypeName.Length > 10)
                            name2 = item2.TypeName.ToString().Substring(0, 10);
                        sbdis.Append("{code:'" + item2.ID + "',value: '" + item2.ID + "',label: '" + name2 + "'");
                        List<Hi.Model.SYS_GType> gType3 = new Hi.BLL.SYS_GType().GetList("FullCode,ID,TypeName", "isnull(dr,0)=0 and IsEnabled = 1  and ParentId=" + item2.ID, "");
                        if (gType3.Count > 0)
                        {
                            sbdis.Append(",children: [");
                            int count3 = 0;
                            foreach (var item3 in gType3)
                            {
                                count3++;

                                string name3 = item3.TypeName;
                                if (item3.TypeName.Length > 10)
                                    name3 = item3.TypeName.ToString().Substring(0, 10);
                                if (count3 == gType3.Count)
                                    sbdis.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + name3 + "'}");
                                else
                                    sbdis.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + name3 + "'},");

                            }
                            sbdis.Append("]");

                        }

                        if (count2 == gType2.Count)
                            sbdis.Append("}");
                        else
                            sbdis.Append("},");
                    }
                    sbdis.Append("]");

                }
                if (count == gType1.Count)
                    sbdis.Append("}");
                else
                    sbdis.Append("},");
            }
            sbdis.Append("]");
            GoodsType = sbdis.ToString();
        }

    }

}