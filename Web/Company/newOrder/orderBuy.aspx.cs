using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using DBUtility;

public partial class Company_newOrder_orderBuy : CompPageBase
{
    public int DisId;//代理商id
    //public string ProID = "0";
    //public string ProPrice = "";
    //public string ProIDD = "0";
    //public string ProType = "";
    public int IsInve = 0;//是否启用库存
    public int Fanli = 0;//是否启用返利
    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["action"];
        if (obj != null)
        {
            if (obj.ToString() == "dislist")
            {
                string compid = Request["compId"] + "";
                string disid = Request["disId"] + "";
                Response.Write(disBing(compid, disid));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            DataTable dt = Common.BindDisList(this.CompID.ToString());//代理商绑定
            if (dt != null)
            {
                this.rptDisList.DataSource = dt;
                this.rptDisList.DataBind();
            }
            this.hidType2.Value = Request.QueryString["type"] + "";//2 再次购买
            this.hidKeyId.Value = KeyID.ToString();
            this.hidCompId.Value = this.CompID.ToString();
            Fanli = OrderInfoType.rdoOrderAudit("订单支付返利是否启用", CompID).ToInt(0);
            this.hidFanl.Value = Fanli.ToString();//是否启用返利
            IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompID).ToInt(0);//是否启用库存
            //代理商列表
            List<Hi.Model.BD_Distributor> list = new Hi.BLL.BD_Distributor().GetList("ID,DisName", "isnull(dr,0)=0 and compid=" + this.CompID, "");
            this.divDisList.InnerText = ConvertJson.ToJson(Common.FillDataTable(list));
            if (KeyID != 0)
            {
                if (Request.QueryString["type"] + "" != "2")
                {
                    Hi.Model.DIS_Order model = new Hi.BLL.DIS_Order().GetModel(KeyID);
                    //判断订单是否可以修改  企业下单订单已审未支付，代理商下单企业不能修改
                    if (model.OState > (int)Enums.OrderState.待审核)
                    {
                        JScript.AlertAndRedirect("订单已被其他人修改，请刷新后再重新操作！", "orderdetail.aspx?top=1&KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
                        return;
                    }
                }
                else
                {
                    this.hidType.Value = Request.QueryString["type"] + "";
                }
                Bind();
            }
        }
    }
    /// <summary>
    /// 代理商选择商品
    /// </summary>
    /// <returns></returns>
    public string disBing(string compid, string disid)
    {
        
        StringBuilder strwhere = new StringBuilder();
        //商品是否启用库存
        IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", compid.ToInt(0)).ToInt(0);
        if (IsInve == 0)
        {
            strwhere.AppendFormat("and info.Inventory>0");
        }
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, SelectGoodsInfo.Returnsql(compid, disid, strwhere.ToString(),"2")).Tables[0];

        List<int> infoidl = new List<int>();

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                int id = item["ID"].ToString().ToInt(0);//BD_goodsInfo表的ID
                infoidl.Add(id);
            }
        }

        //获取商品价格
        List<BLL.gDprice> l = BLL.Common.GetPrice(Convert.ToInt32(compid), Convert.ToInt32(disid), infoidl);

        if (l != null && l.Count > 0)
        {
            foreach (var item in l)
            {
                DataRow[] dr = dt.Select(" ID=" + item.goodsInfoId);
                if (dr.Length > 0)
                {
                    //获取的价格大于促销价时、取促销价 
                    dr[0]["pr"] = item.FinalPrice;
                    dr[0]["typeTinkerPrice"] = item.typePrice;
                    dr[0]["disTinkerPrice"] = item.disPrice;
                    dr[0]["disProPr"] = item.bpPrice;
                }
            }
        }


        return ConvertJson.ToJson2(dt);
    }
    /// <summary>
    /// 修改是绑定的数据
    /// </summary>
    public void Bind()
    {
        //判断是否有权限
        if (!Common.PageCompOperable("Order", KeyID, CompID))
        {
            Response.Redirect("../../NoOperable.aspx");
            return;
        }
        Hi.Model.DIS_Order OrderInfoModel = new Hi.BLL.DIS_Order().GetModel(KeyID);
        if (OrderInfoModel != null)
        {
            hidts.Value = OrderInfoModel.ts.ToString();//时间戳
            this.divGoodsName.InnerText = disBing(CompID.ToString(), OrderInfoModel.DisID.ToString());//筛选商品
            Hi.Model.BD_Distributor model = new Hi.BLL.BD_Distributor().GetModel(OrderInfoModel.DisID);
            this.txtDisName.Value = model.DisName;//代理商名称
            this.txtDisName.Disabled = true;//只读
            this.hidDisID.Value = OrderInfoModel.DisID.ToString();//代理商ID
            if (Request["type"] + "" != "2" || Fanli == 1)
            {
                this.txtRebate.Value = OrderInfoModel.bateAmount.ToString();//返利金额
            }
            this.txtDate.Value = OrderInfoModel.ArriveDate.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : OrderInfoModel.ArriveDate.ToString("yyyy-MM-dd");//交货日期
            this.lblPsType.InnerText = OrderInfoModel.GiveMode;//配送方式
            this.lblPsType2.InnerText = OrderInfoModel.GiveMode == "送货" ? "自提" : "送货";
            this.hidPsType.Value = OrderInfoModel.GiveMode;//配送方式
            this.OrderNote.Value = OrderInfoModel.Remark;//订单备注
            this.hidAdder.Value = OrderInfoModel.Address;//地址
            this.hidAddName.Value = OrderInfoModel.Principal;//联系人
            this.hidAddPhone.Value = OrderInfoModel.Phone;//联系人电话
            this.hrAdder.Value = OrderInfoModel.AddrID.ToString();//收货地址id
            this.hrOrderFj.Value = OrderInfoModel.Atta;//附件文件
            string str10 = decimal.Parse(string.Format("{0:N2}", OrderInfoModel.PostFee.ToString())).ToString("0.00");//运费
            this.lblPostFee.InnerText = str10;//运费
            this.hidPostFree.Value = str10;//运费
            //附件
            string str3 = string.Empty;//附件信息
            if (OrderInfoModel.Atta.ToString() != "")
            {
                StringBuilder li = new StringBuilder();
                string[] atta = OrderInfoModel.Atta.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                if (atta.Length > 0)
                {
                    foreach (var item in atta)
                    {
                        string[] att = item.Split(new string[] { "^^" }, StringSplitOptions.RemoveEmptyEntries);

                        li.AppendFormat("<li> <a href=\"javascript:;\" class=\"name\">{0}（大小：{4}KB）</a><a href=\"javascript:;\"  class=\"bule del\" tip=\"{3}\" orderid=\"{1}\">删除</a><a href=\"{2}\" target=\"_blank\" class=\"bule\">下载</a></li>", att[0] + att[1].Substring(att[1].LastIndexOf(".")), KeyID, Common.GetWebConfigKey("ImgViewPath") + "OrderFJ/" + item, item, OrderType.GetSize(item));

                    }
                }
                str3 = li.ToString();
            }
            //收货信息
            string str = "收货人：" + OrderInfoModel.Principal + "，联系电话：" + OrderInfoModel.Phone + "，收货地址：" + OrderInfoModel.Address;
            string price = string.Empty;//返利总金额
            if (Request.QueryString["type"] + "" == "2")
            {
                //再次购买时获取返利总金额
                price = OrderType.GetRebate(0, OrderInfoModel.DisID);
            }
            else
            {
                //其他获取返利总金额
                price = OrderType.GetRebate(KeyID, OrderInfoModel.DisID);
            }
            //可用返利金额
            string str4 = "可用返利￥ <label id=\"lblRebate\">" + string.Format("{0:N2}", price.ToString()) + "</label><i class=\"sus-i seebate\"></i>";
            List<Hi.Model.DIS_OrderDetail> ll = new Hi.BLL.DIS_OrderDetail().GetList("", "isnull(dr,0)=0 and orderId=" + KeyID, "");
            if (ll.Count > 0)
            {
                string html = string.Empty;//绑定的商品数据
                int index = 0;//索引
                string str8 = "0.00";//商品小计
                foreach (Hi.Model.DIS_OrderDetail obj in ll)
                {
                    Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(obj.GoodsinfoID);
                    string kc = string.Empty;
                    decimal newprice = obj.AuditAmount;//判断修改和再次购买时的价格 显示的
                    decimal newprice2 = obj.GoodsPrice;//判断修改和再次购买时的价格 隐藏的
                    str8 = decimal.Parse(string.Format("{0:N2}", obj.sumAmount)).ToString("0.00");//判断修改和再次购买时的小计价格
                    // decimal str88 = 0;//判断修改和再次购买时的小计价格 隐藏的
                    if (Request.QueryString["type"] + "" == "2")
                    {
                        //再次 购买时获取库存
                        kc = model2.Inventory.ToString();
                       // newprice2 = newprice = BLL.Common.GetGoodsPrice(this.CompID, OrderInfoModel.DisID, obj.GoodsinfoID);
                      //  str8 = decimal.Parse(string.Format("{0:N2}", newprice * obj.GoodsNum)).ToString("0.00");
                    }
                    else
                    {
                        //其他获取库存
                        kc = (model2.Inventory + obj.GoodsNum + obj.ProNum.ToDecimal(0)).ToString();
                    }
                    string pic = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(model2.GoodsID)).Pic;//图片
                    string str6 = IsInve == 0 ? "" : "display:none";//是否显示库存
                    string str7 = string.Empty;//是否促销 
                    str7 = SelectGoodsInfo.protitle(obj.ProID, obj.Protype, obj.Unit);// ConvertJson.IsCx(dt.Rows[0]["proTypes"].ToString(), dt.Rows[0]["proType"].ToString(), dt.Rows[0]["proGoodsPrice"].ToString(), dt.Rows[0]["proDiscount"].ToString(), dt.Rows[0]["unit"].ToString()); //SelectGoodsInfo.protitle(obj.ProID, obj.Protype, obj.Unit);
                    string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID); //小数位数
                    decimal num = decimal.Parse(string.Format("{0:N4}", (obj.GoodsNum).ToString("#,####" + Digits)));//购买数量
                    string str9 = obj.Remark;//goodsInfoid备注
                    if (obj.Remark != null && obj.Remark.Length > 6)
                    {
                        str9 = obj.Remark.Substring(0, 6) + "...";
                    }
                    string strremark = string.Empty;//鼠标移上去时显示的备注
                    if (obj.Remark != "")
                    {
                        strremark = "<div class=\"divremark" + index + "\">" + str9 + "</div><div class=\"cur\">" + obj.Remark + "</div>";
                    }
                    // decimal zxprice = BLL.Common.GetGoodsPrice(this.CompID, OrderInfoModel.DisID, obj.GoodsinfoID);
                    html += "<tr trindex=\"" + index + "\" trindex2=\"" + index + "\" id=\"" + obj.ID + "\" tip=\"" + obj.GoodsinfoID + "\"><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"sPic\"><a class=\"opt-i2\"></a><span><a href=\"javascript:;\"><img src=\"" + (pic != "" ? Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + pic : "../../images/pic.jpg") + "\" width=\"60\" height=\"60\"></a></span><a href=\"javascript:;\" class=\"code\">商品编码：" + obj.GoodsCode + str7 + "</a><a href=\"javascript:;\" class=\"name\">" + GetGoodsName(obj.GoodsName, obj.GoodsInfos, "1") + "<i>" + GetGoodsName(obj.GoodsName, obj.GoodsInfos, "2") + "</i></a></div></td><td><div class=\"tc\">" + obj.Unit + "</div></td><td><input type=\"text\" tip2=\"" + index + "\" class=\"boxs divprice" + index + "\"   value=\"" + decimal.Parse(string.Format("{0:N2}", newprice)).ToString("0.00") + "\" maxlength=\"9\" ><input type=\"hidden\" class=\"hidPrice\" value=\"" + decimal.Parse(string.Format("{0:N2}", newprice2)).ToString("0.00") + "\" /></td><td style=\"" + str6 + "\"><div class=\"tc\"><input type=\"hidden\" id=\"hidInventory_" + index + "\" value=\"" + decimal.Parse(string.Format("{0:N2}", kc)).ToString(Digits) + "\" />" + decimal.Parse(string.Format("{0:N2}", kc)).ToString(Digits) + "</div></td><td><div class=\"sl divnum\" tip=\"" + obj.GoodsinfoID + "\" tip2=\"" + index + "\"><a href=\"javascript:void(0);\"  class=\"minus\">-</a><input type=\"text\" class=\"box txtGoodsNum txtGoodsNum" + index + "\" onfocus=\"InputFocus(this)\" onkeyup='KeyInt2(this)' maxlength=\"9\"  value=\"" + num + "\"><a href=\"javascript:void(0);\"  class=\"add\">+</a></div></td><td><div class=\"tc lblTotal_" + index + "\">￥" + str8 + "</div></td><td><div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">" + (obj.Remark == "" ? "添加" : "编辑") + "</a>" + strremark + "</div></td></tr>";
                    index++;
                }
                this.lblTotalAmount.InnerText = decimal.Parse(string.Format("{0:N2}", OrderInfoModel.TotalAmount)).ToString("0.00");//商品总额
                string str2 = string.Empty;//开票信息
                List<Hi.Model.DIS_OrderExt> l = new Hi.BLL.DIS_OrderExt().GetList("", "orderID=" + KeyID, "");
                if (l.Count > 0)
                {
                    foreach (Hi.Model.DIS_OrderExt item in l)
                    {
                        this.hrOrderInv.Value = item.DisAccID;//开票Id
                        this.hidLookUp.Value = item.Rise;//发票抬头
                        this.hidBank.Value = item.OBank;//开会银行
                        this.hidContext.Value = item.Content;//发票内容
                        this.hidAccount.Value = item.OAccount;//开户帐号
                        this.hidRegNo.Value = item.TRNumber;//纳税人登记号
                        if (item.IsOBill == 0)
                        {
                            str2 = "不开票";
                            this.hidVal.Value = "0";
                        }
                        else if (item.IsOBill == 1 && item.OAccount == "" && item.TRNumber == "" && item.OBank == "")
                        {
                            str2 = "发票抬头：" + item.Rise + "，发票内容：" + item.Content;
                            this.hidVal.Value = "1";
                        }
                        else
                        {
                            str2 = "发票抬头：" + item.Rise + "，发票内容：" + item.Content + "，开户银行：" + item.OBank + "，开户账户：" + item.OAccount + "，纳税人登记号：" + item.TRNumber;
                            this.hidVal.Value = "2";
                        }

                        this.lblCux.InnerText = decimal.Parse(string.Format("{0:N2}", item.ProAmount.ToString())).ToString("0.00");//订单促销

                    }
                }
                if (Convert.ToDecimal(str8) < 0)
                {
                    str8 = "0.00";
                }
                if (Request["type"] + "" != "2" || Fanli == 1)
                {
                    this.lblFanl.InnerText = decimal.Parse(string.Format("{0:N2}", OrderInfoModel.bateAmount.ToString())).ToString("0.00");//返利金额
                }
                ClientScript.RegisterStartupScript(this.GetType(), "adder", "<script>$(function(){$(\".site .site-if\").text(\"" + str + "\");$(\".invoice .in-if\").text(\"" + str2 + "\");$(\".attach .list\").html(\"" + str3.Replace("\"", "'") + "\");$(\".edit-ok .txt\").html(\"" + str4.Replace("\"", "'") + "\");  $(\".tabLine table tbody\").html(\"" + html.Replace("\"", "'") + "\"); $(\"#lblYFPrice\").text(\"" + decimal.Parse(string.Format("{0:N2}", (Request["type"] + "" == "2" && Fanli == 0 ? OrderInfoModel.AuditAmount + OrderInfoModel.bateAmount : OrderInfoModel.AuditAmount))).ToString("0.00") + "\"); $(\".jxs-box .opt-i\").hide();})</script>");
            }

        }
    }
    //截取字符串
    //商品名称，属性值，是否需要截取
    public string GetGoodsName(string goodsName, string valueInfo, string type)
    {
        string str = string.Empty;
        string str2 = string.Empty;
        if (valueInfo != "")
        {
            str2 = valueInfo.ToString().Substring(0, valueInfo.Length - 1).ToString().Replace('；', ',');
        }
        else
        {
            str2 = valueInfo;
        }
        str = goodsName + "&nbsp;" + str2;
        if (type == "1")
        {
            if (str.Length > 30)
            {
                str = str.Substring(0, 30) + "...";
            }
        }
        return str;
    }
}