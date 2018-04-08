using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using DBUtility;

public partial class Distributor_newOrder_orderBuy : DisPageBase
{
    public int DisId;
    public string ProID = "0";
    public string ProPrice = "";
    public string ProIDD = "0";
    public string ProType = "";
    public int IsInve = 0;
    public int Fanli = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            this.hidType2.Value = Request.QueryString["type"] + "";
            this.hidDisID.Value = this.DisID.ToString();
            this.hidKeyId.Value = KeyID.ToString();
           
            StringBuilder strwhere = new StringBuilder();
            Fanli = OrderInfoType.rdoOrderAudit("订单支付返利是否启用", CompID).ToInt(0);
            this.hidFanl.Value = Fanli.ToString();
            LoginModel logUser = Session["UserModel"] as LoginModel;
            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());
            this.hidCompId.Value = this.ddrComp.Value.ToString();

            decimal CreditAmount = 0;
            if (BLL.Common.GetCredit(this.hidCompId.Value.ToString().ToInt(0), logUser.DisID, out CreditAmount))
            {
                decimal GetSumAmount = OrderInfoType.GetSumAmount(logUser.DisID.ToString(), this.hidCompId.Value.ToString(), KeyID);
                if (GetSumAmount >= CreditAmount)
                {
                    this.Msg.InnerHtml = "您的授信额度(" + CreditAmount + ")已用完或超出！无法下单";
                    this.Btn.InnerHtml = "<a href=\"javascript:; \" class=\"gray-btn2\">取消</a>";
                }
                else
                {
                    this.Msg.InnerHtml = "";
                }
            }
            else
            {
                this.Msg.InnerHtml = "";
            }
            // isbate = OrderInfoType.rdoOrderAudit("订单支付返利是否启用", CompID).ToInt(0);
            //if (Fanli == 0)
            //{
            //    this.trbate.Visible = false;
            //    this.rebate.Visible = false;
            //}
            //商品是否启用库存
            IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", this.CompID).ToInt(0);
            if (IsInve == 0)
            {
                strwhere.AppendFormat("and info.Inventory>0");
            }
            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, SelectGoodsInfo.Returnsql(CompID.ToString(), DisID.ToString(), strwhere.ToString(),"1")).Tables[0];

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
            List<BLL.gDprice> l = BLL.Common.GetPrice(CompID, DisID, infoidl);

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
            this.divGoodsName.InnerText = ConvertJson.ToJson2(dt);
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
            else
            {
                if ((Request.QueryString["type"] + "") == "1")
                {
                    this.hidType.Value = Request.QueryString["type"] + "";
                    ShopCart();
                }
            }

           
        }
    }
    /// <summary>
    /// 购物车商品
    /// </summary>
    public void ShopCart()
    {
        DataTable dt = new Hi.BLL.DIS_ShopCart().GetGoodsCart(" sc.[CompID]=" + this.CompID + " and sc.[DisID]=" + this.DisID + "and sc.dr=0", "sc.[CreateDate] desc ");

        if (dt != null && dt.Rows.Count > 0)
        {
            decimal TotalAmount = 0;
            string html = string.Empty;
            string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID); //小数位数
            string str6 = IsInve == 0 ? "" : "display:none";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int goodsinfoid = Convert.ToInt32(dt.Rows[i]["GoodsinfoID"].ToString());
                Hi.Model.BD_GoodsInfo model3 = new Hi.BLL.BD_GoodsInfo().GetModel(goodsinfoid);
                Hi.Model.BD_Goods model4 = new Hi.BLL.BD_Goods().GetModel(model3.GoodsID);
                string pic = dt.Rows[i]["pic"].ToString();// new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(model2.GoodsID)).Pic;
                string inventory = string.Empty;
                BLL.Common.GetInevntory(this.CompID, goodsinfoid, 0, Convert.ToDecimal(dt.Rows[i]["GoodsNum"]), out inventory);// dt.Rows[i]["Inventory"].ToString();
                string str7 = string.Empty;//是否促销 
                if (dt.Rows[i]["ProID"].ToString() != "0")
                {
                    str7 = SelectGoodsInfo.protitle(dt.Rows[i]["ProID"].ToString(), dt.Rows[i]["Unit"].ToString());
                }
                string strremark = string.Empty;
                decimal zxprice = BLL.Common.GetGoodsPrice(this.CompID, this.DisID, goodsinfoid);//最新价格
                decimal num = decimal.Parse(string.Format("{0:N4}", Convert.ToDecimal(dt.Rows[i]["GoodsNum"]).ToString("#,####" + Digits)));
                TotalAmount += zxprice * num;
                html += "<tr tip=\"" + goodsinfoid + "\" trindex=\"" + i + "\" trindex2=\"" + i + "\" id=\"\"><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"sPic\"><a class=\"opt-i2\"></a><span><a href=\"javascript:;\"><img src=\"" + (pic != "" ? Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + pic : "../../images/pic.jpg") + "\" width=\"60\" height=\"60\"></a></span><a href=\"javascript:;\" class=\"code\">商品编码：" + dt.Rows[i]["barCode"].ToString() + str7 + "</a><a href=\"javascript:;\" class=\"name\">" + GetGoodsName(model4.GoodsName, model3.ValueInfo, "1") + "<i>" + GetGoodsName(model4.GoodsName, model3.ValueInfo, "2") + "</i></a></div></td><td><div class=\"tc\">" + model4.Unit + "</div></td><td><div class=\"tc divprice" + i + "\" tip=\"" + decimal.Parse(string.Format("{0:N2}", zxprice)).ToString("0.00") + "\">￥" + decimal.Parse(string.Format("{0:N2}", zxprice)).ToString("0.00") + "</div><input type=\"hidden\" class=\"hidPrice\" value=\"" + zxprice + "\" /></td><td style=\"" + str6 + "\"><div class=\"tc\"><input type=\"hidden\" id=\"hidInventory_" + i + "\" value=\"" + decimal.Parse(string.Format("{0:N2}", inventory)).ToString(Digits) + "\" />" + decimal.Parse(string.Format("{0:N2}", inventory)).ToString(Digits) + "</div></td><td><div class=\"sl divnum\" tip=\"" + dt.Rows[i]["GoodsinfoID"].ToString() + "\" tip2=\"" + i + "\"><a href=\"javascript:void(0);\"  class=\"minus\">-</a><input type=\"text\" class=\"box txtGoodsNum txtGoodsNum" + i + "\" onfocus=\"InputFocus(this)\" onkeyup='KeyInt2(this)' maxlength=\"9\"  value=\"" + num + "\"><a href=\"javascript:void(0);\"  class=\"add\">+</a></div></td><td><div class=\"tc lblTotal_" + i + "\">￥" + decimal.Parse(string.Format("{0:N2}", zxprice * num)).ToString("0.00") + "</div></td><td><div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + i + "\">添加</a>" + strremark + "</div></td></tr>";
            }
            //查询购物车商品数量、总价
            // DataTable dtp = new Hi.BLL.DIS_ShopCart().SumCartNum(this.CompID.ToString(), this.DisID.ToString());
            // if (dtp != null && dtp.Rows.Count > 0)
            //{
            //TotalAmount = (dtp.Rows[0]["SumAmount"].ToString().ToDecimal(0));
            //订单总价
            this.lblTotalAmount.InnerText = decimal.Parse(string.Format("{0:N2}", TotalAmount.ToString())).ToString("0.00");//商品总额
            //订单促销
            decimal ProAmount = Common.GetProPrice(TotalAmount, out ProID, out ProIDD, out ProType, this.CompID);
            this.lblCux.InnerText = decimal.Parse(string.Format("{0:N2}", ProAmount.ToString())).ToString("0.00");
            //合计
            string str8 = (TotalAmount - ProAmount).ToString("N");
            ClientScript.RegisterStartupScript(this.GetType(), "adder", "<script>$(function(){disId=" + this.DisID + "; $(\".tabLine table tbody tr:last\").before(\"" + html.Replace("\"", "'") + "\");$(\"#lblYFPrice\").text(\"" + str8 + "\")})</script>");
            // }
        }
        else
        {
            Response.Redirect("/Distributor/Shop.aspx");
        }
    }

    /// <summary>
    /// 修改是绑定的数据
    /// </summary>
    public void Bind()
    {
        if (!Common.PageCompOperable("Order", KeyID, CompID))
        {
            Response.Redirect("../../NoOperable.aspx");
            return;
        }
        Hi.Model.DIS_Order OrderInfoModel = new Hi.BLL.DIS_Order().GetModel(KeyID);
        if (OrderInfoModel != null)
        {
            //修改订单时,不能修改厂商
            this.ddrComp.Disabled = true;
            this.ddrComp.Value = OrderInfoModel.CompID.ToString();

            hidts.Value = OrderInfoModel.ts.ToString();
            //this.divGoodsName.InnerText = disBing(CompID.ToString(), OrderInfoModel.DisID.ToString());
            Hi.Model.BD_Distributor model = new Hi.BLL.BD_Distributor().GetModel(OrderInfoModel.DisID);
            // this.txtDisName.Value = model.DisName;
            //this.txtDisName.Disabled = true;
            this.hidDisID.Value = OrderInfoModel.DisID.ToString();//代理商ID
             if (Request["type"] + "" != "2" || Fanli == 1)
            {
            this.txtRebate.Value = OrderInfoModel.bateAmount.ToString();//返利金额
             }
            this.txtDate.Value = OrderInfoModel.ArriveDate.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : OrderInfoModel.ArriveDate.ToString("yyyy-MM-dd");//交货日期
            this.lblPsType.InnerText = OrderInfoModel.GiveMode;//配送方式
            this.lblPsType2.InnerText = OrderInfoModel.GiveMode == "送货" ? "自提" : "送货";
            this.hidPsType.Value = OrderInfoModel.GiveMode;
            this.OrderNote.Value = OrderInfoModel.Remark;//订单备注
            this.hidAdder.Value = OrderInfoModel.Address;//地址
            this.hidAddName.Value = OrderInfoModel.Principal;//联系人
            this.hidAddPhone.Value = OrderInfoModel.Phone;//联系人电话
            this.hrAdder.Value = OrderInfoModel.AddrID.ToString();//收货地址id
            this.hrOrderFj.Value = OrderInfoModel.Atta;//附件文件
            string str10 = decimal.Parse(string.Format("{0:N2}", OrderInfoModel.PostFee.ToString())).ToString("0.00");//运费
            this.lblPostFee.InnerText = str10;
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
            string str = "收货人：" + OrderInfoModel.Principal + "，联系电话：" + OrderInfoModel.Phone + "，收货地址：" + OrderInfoModel.Address;//收货信息

            string price = string.Empty;
            if (Request.QueryString["type"] + "" == "2")
            {
                price = OrderType.GetRebate(0, OrderInfoModel.DisID);
            }
            else
            {
                price = OrderType.GetRebate(KeyID, OrderInfoModel.DisID);
            }
            string str4 = "可用返利￥ <label id=\"lblRebate\">" + string.Format("{0:N2}", price.ToString()) + "</label><i class=\"sus-i seebate\"></i>";//可用返利金额
            List<Hi.Model.DIS_OrderDetail> ll = new Hi.BLL.DIS_OrderDetail().GetList("", "isnull(dr,0)=0 and orderId=" + KeyID, "");
            if (ll.Count > 0)
            {
                string html = string.Empty;
                int index = 0;
                string str8 = "0.00";
                foreach (Hi.Model.DIS_OrderDetail obj in ll)
                {
                    Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(obj.GoodsinfoID);
                    string kc = string.Empty;
                    decimal newprice = obj.AuditAmount;//判断修改和再次购买时的价格 显示的
                    decimal newprice2 = obj.GoodsPrice;//判断修改和再次购买时的价格 隐藏的
                    str8 = decimal.Parse(string.Format("{0:N2}", obj.sumAmount)).ToString("0.00");

                    if (Request.QueryString["type"] + "" == "2")
                    {
                        kc = model2.Inventory.ToString();
                       // newprice2 = newprice = BLL.Common.GetGoodsPrice(this.CompID, OrderInfoModel.DisID, obj.GoodsinfoID);
                       // str8 = decimal.Parse(string.Format("{0:N2}", newprice * obj.GoodsNum)).ToString("0.00");
                    }
                    else
                    {
                        kc = (model2.Inventory + obj.GoodsNum + obj.ProNum.ToDecimal(0)).ToString();
                    }
                    string pic = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(model2.GoodsID)).Pic;
                    string str6 = IsInve == 0 ? "" : "display:none";
                    string str7 = string.Empty;//是否促销 
                    str7 = SelectGoodsInfo.protitle(obj.ProID, obj.Protype, obj.Unit);// ConvertJson.IsCx(dt.Rows[0]["proTypes"].ToString(), dt.Rows[0]["proType"].ToString(), dt.Rows[0]["proGoodsPrice"].ToString(), dt.Rows[0]["proDiscount"].ToString(), dt.Rows[0]["unit"].ToString()); //SelectGoodsInfo.protitle(obj.ProID, obj.Protype, obj.Unit);
                    string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID); //小数位数
                    decimal num = decimal.Parse(string.Format("{0:N4}", (obj.GoodsNum).ToString("#,####" + Digits)));
                    string str9 = obj.Remark;
                    if (obj.Remark != null && obj.Remark.Length > 6)
                    {
                        str9 = obj.Remark.Substring(0, 6) + "...";
                    }
                    string strremark = string.Empty;
                    if (obj.Remark != "")
                    {
                        strremark = "<div class=\"divremark" + index + "\">" + str9 + "</div><div class=\"cur\">" + obj.Remark + "</div>";
                    }
                   // decimal zxprice = BLL.Common.GetGoodsPrice(this.CompID, OrderInfoModel.DisID, obj.GoodsinfoID);
                    html += "<tr trindex=\"" + index + "\" trindex2=\"" + index + "\" id=\"" + obj.ID + "\" tip=\"" + obj.GoodsinfoID + "\"><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"sPic\"><a class=\"opt-i2\"></a><span><a href=\"javascript:;\"><img src=\"" + (pic != "" ? Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + pic : "../../images/pic.jpg") + "\" width=\"60\" height=\"60\"></a></span><a href=\"javascript:;\" class=\"code\">商品编码：" + obj.GoodsCode + str7 + "</a><a href=\"javascript:;\" class=\"name\">" + GetGoodsName(obj.GoodsName, obj.GoodsInfos, "1") + "<i>" + GetGoodsName(obj.GoodsName, obj.GoodsInfos, "2") + "</i></a></div></td><td><div class=\"tc\">" + obj.Unit + "</div></td><td><div class=\"tc divprice" + index + "\" tip=\"" + decimal.Parse(string.Format("{0:N2}", newprice)).ToString("0.00") + "\">￥" + decimal.Parse(string.Format("{0:N2}", newprice)).ToString("0.00") + "</div><input type=\"hidden\" class=\"hidPrice\" value=\"" + decimal.Parse(string.Format("{0:N2}", newprice2)).ToString("0.00") + "\" /></td><td style=\"" + str6 + "\"><div class=\"tc\"><input type=\"hidden\" id=\"hidInventory_" + index + "\" value=\"" + decimal.Parse(string.Format("{0:N2}", kc)).ToString(Digits) + "\" />" + decimal.Parse(string.Format("{0:N2}", kc)).ToString(Digits) + "</div></td><td><div class=\"sl divnum\" tip=\"" + obj.GoodsinfoID + "\" tip2=\"" + index + "\"><a href=\"javascript:;\"  class=\"minus\">-</a><input type=\"text\" class=\"box txtGoodsNum txtGoodsNum" + index + "\" onfocus=\"InputFocus(this)\" onkeyup='KeyInt2(this)' maxlength=\"9\"  value=\"" + num + "\"><a href=\"javascript:;\"  class=\"add\">+</a></div></td><td><div class=\"tc lblTotal_" + index + "\">￥" + str8 + "</div></td><td><div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">" + (obj.Remark == "" ? "添加" : "编辑") + "</a>" + strremark + "</div></td></tr>";
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
    /// <summary>
    /// 清除DataTable数据
    /// </summary>
    public void ClearShop()
    {
            //清空购物车
            string str = " CompID=" + this.CompID + " and DisID=" + this.DisID;
            new Hi.BLL.DIS_ShopCart().CartEmpty(str);

            //清除全部Session
            Session["GoodsInfo"] = null;
            Session.Remove("GoodsInfo");
    }
}