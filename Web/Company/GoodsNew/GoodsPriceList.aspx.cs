using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Company_Goods_GoodsPriceList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            this.lblGoodsPrice.Text = @"<table class='tablelist'>
                <thead>
                    <tr>
                        <th class='t8'>序号
                        </th>
                        <th>
                            商品名称
                        </th>
                        <th class='t6'>
                            商品规格属性
                        </th>
                        <th class='t2'>
                            基础价格(元)
                        </th>
                        <th class='t2'>
                            销售价格(元)
                        </th>
                        <th class='t2'>
                            本次调整价格(元)
                        </th>
                        <th  class='t2'>
                            操作
                        </th>
                    </tr>
                </thead></table>";
            this.hidCompId.Value = this.CompID.ToString();
        }
        object action = Request["action"];
        if (action != null)
        {
            //显示选择的商品
            if (action.ToString() == "selectPrice")
            {
                string disid = Request["disId"];
                Response.Write(Bind(disid));
                Response.End();
            }
            //删除商品
            if (action.ToString() == "delGoods")
            {
                string id = Request["id"];
                Response.Write(DelGoods(id));
                Response.End();
            }
        }
    }
    /// <summary>
    /// 删除商品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string DelGoods(string id)
    {
        List<Hi.Model.BD_GoodsInfo> llll = new List<Hi.Model.BD_GoodsInfo>();
        object lll = Session["GoodsPrice"];
        if (lll != null)
        {
            List<Hi.Model.BD_GoodsInfo> ll = lll as List<Hi.Model.BD_GoodsInfo>;
            foreach (Hi.Model.BD_GoodsInfo item in ll)
            {
                if (item.ID.ToString() == id)
                {
                    continue;
                }
                llll.Add(item);
            }
        }
        if (llll.Count == 0)
        {
            Session["GoodsPrice"] = null;
        }
        else
        {
            Session["GoodsPrice"] = llll;
        }
        return "cg";
    }
    /// <summary>
    /// 商品列表绑定
    /// </summary>
    public string Bind(string disId)
    {
        object obj = Session["GoodsPrice"];
        string html = string.Empty;
        if (obj != null)
        {
            string str = string.Empty;
            string strPrice = string.Empty;
            List<Hi.Model.BD_GoodsInfo> lll = obj as List<Hi.Model.BD_GoodsInfo>;
            html += @"<table class='tablelist'>
                <thead>
                    <tr> <th  style='display:none'>
                        <input type='checkbox' name='checkbox' id='CB_SelAll'  />
                    </th>
                    <th class='t8'>序号
                        </th>
                        <th>
                            商品上架名称
                        </th>
                        <th class='t6'>
                            商品规格属性
                        </th>
                        <th class='t5'>
                            基础价格(元)
                        </th>
                         <th class='t5'>
                            销售价格(元)
                        </th>
                        <th class='t5'>
                             本次调整价格(元)
                        </th>
                        <th  class='t8'>
                            操作
                        </th>

                    </tr>
                </thead> <tbody>";
            int zz = 0;
            foreach (Hi.Model.BD_GoodsInfo item2 in lll)
            {
                zz++;
                html += string.Format(@"<tr class='tr{3}'>
                             <td style='display:none'>
                                <div class='tc'><input type='checkbox' name='checkbox' id='CB_SelAll'  checked='checked'/>
                                <input id='HF_Price{3}' type='hidden' Value='{3}'/></div>
                            </td>
                                <td><div class='tc'>{5}</div></td>
                                <td>
                                  <div class='tcle'>{0}</div>
                                </td>
                                <td>
                                   <div class='tcle'>{1}</div>
                                </td>
                                <td>
                                  <div class='tc'> {2}</div>
                                </td>
                                <td>
                                   <div class='tc'> {4}</div>
                                </td>
                                <td>
                                <div class='tc'> <input type='text' class='textBox txtPrice{3}' id='txtPrice{3}' name='txtPrice'  value='{6}' onkeyup='KeyInt2(this)'/></div>
                                </td>
                                <td>
                                    <div class='tc'> <img alt='删除' title='删除' style='cursor: pointer;' onclick='return Delete({3})'
                                        id='img1' src='../images/t03.png' /></div>
                                </td>
                            </tr>", GoodsName(item2.ID.ToString()), GoodsAttr(item2.ID.ToString()), decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(item2.SalePrice.ToString()).ToString())).ToString("#,##0.00"), item2.ID, GoodsTinkerPrice(item2.ID, disId, item2.SalePrice.ToString(), 1), zz, GoodsTinkerPrice(item2.ID, disId, item2.SalePrice.ToString(), 2));
            }
            html += " </tbody></table>";
        }
        return html;
    }
    /// <summary>
    /// 最新价格
    /// </summary>
    /// <returns></returns>
    public string GoodsTinkerPrice(int id, string disId, string saleprice, int type)
    {
        string price = string.Empty;
        if (type == 1)
        {
            price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(saleprice.ToString()).ToString())).ToString("#,##0.00");
        }
        else
        {
            price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(saleprice.ToString()).ToString())).ToString("0.00");
        }
        if (disId.Split(',').Length == 1)
        {
            //List<Hi.Model.BD_DisPrice> ll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and isenabled=1 and compid=" + CompID + " and disids   like '%" + disId + "%'", " id desc");
            //if (ll.Count > 0)
            //{
            //    string disIdlist = string.Empty;
            //    foreach (Hi.Model.BD_DisPrice item in ll)
            //    {
            //        disIdlist += item.ID + ",";
            //    }
            List<Hi.Model.BD_GoodsPrice> l = new Hi.BLL.BD_GoodsPrice().GetList("", "isnull(dr,0)=0 and goodsinfoid =" + id + " and compid=" + this.CompID + " and disId=" + disId + " and isenabled=1", "id desc");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_GoodsPrice item in l)
                {
                    if (type == 1)
                    {
                        price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(item.TinkerPrice.ToString()).ToString())).ToString("#,##0.00");

                    }
                    else
                    {
                        price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(item.TinkerPrice.ToString()).ToString())).ToString("0.00");

                    }
                }
            }
            else
            {
                Hi.Model.BD_GoodsInfo lll = new Hi.BLL.BD_GoodsInfo().GetModel(id);
                if (lll != null)
                {
                    if (type == 1)
                    {
                        price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(lll.TinkerPrice.ToString()).ToString())).ToString("#,##0.00");
                    }
                    else
                    {
                        price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(lll.TinkerPrice.ToString()).ToString())).ToString("0.00");

                    }
                }
            }
            // }
        }
        return price;
    }
    /// <summary>
    /// 得到商品名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GoodsName(string id)
    {
        string name = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(model.GoodsID);
            if (model2 != null)
            {
                name = model2.GoodsName;
            }
        }
        return name;
    }
    /// <summary>
    /// 得到商品基础价格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public decimal GoodsSalePrice(string id)
    {
        decimal price = 0;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            //Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(model.GoodsID);
            //if (model2 != null)
            //{
            //    price = model2.SalePrice;
            //}
            price = model.SalePrice;
        }
        return price;
    }
    /// <summary>
    /// 得到属性
    /// </summary>
    /// <returns></returns>
    public string GoodsAttr(string id)
    {
        string str = string.Empty;
        string str2 = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            str = model.ValueInfo;
            if (!Util.IsEmpty(str))
            {
                string[] lsit = { };
                string[] lsit2 = { };
                lsit = str.Replace(';', '；').Split('；');
                for (int i = 0; i < lsit.Length; i++)
                {
                    if (lsit[i] != "")
                    {
                        lsit2 = lsit[i].Split(':');
                        str2 += lsit2[0] + "：" + "<label style='color:#0080b8'>" + lsit2[1] + "</label>" + "；";
                    }
                }
            }
            else
            {
                str2 = Common.GetGoodsMemo(Convert.ToInt32(model.GoodsID));
            }
        }
        return str2;
    }
    /// <summary>
    /// 确定并生效按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnInsert_Click(object sender, EventArgs e)
    {

        SqlTransaction Tran = null;
        object obj = Session["GoodsPrice"];
        if (obj != null)
        {
            try
            {
                Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                string title =Common.NoHTML( this.txtDisTitle.Value.Trim());//调价标题
                string disNameList =Common.NoHTML( this.txtDisID1.Name.ToString());//选择的代理商名称列表
                string disIdList =Common.NoHTML( this.txtDisID1.Disid.ToString());//选择的代理商Id列表
                string remark =Common.NoHTML( this.txtRemark.Value.Trim());//备注
                if (Util.IsEmpty(title))
                {
                    JScript.AlertMethod(this.Page, "调价标题不能为空", JScript.IconOption.错误);
                    return;
                }
                if (Util.IsEmpty(disNameList))
                {
                    JScript.AlertMethod(this.Page, "请选择代理商", JScript.IconOption.错误);
                    return;
                }
                Hi.Model.BD_DisPrice model2 = new Hi.Model.BD_DisPrice();
                //model2.Title = title;
                model2.CompID = this.CompID;
                model2.DisIDs = disIdList;
                //model2.DisNames = disNameList;
                //model2.Remark = remark;
                //model2.State = 0;
                model2.IsEnabled = true;
                model2.CreateUserID = this.UserID;
                model2.CreateDate = DateTime.Now;
                model2.dr = 0;
                model2.ts = DateTime.Now;
                model2.modifyuser = this.UserID;
                int disPriceId = new Hi.BLL.BD_DisPrice().Add(model2, Tran);
                int z = 0;
                List<Hi.Model.BD_GoodsPrice> llll = new List<Hi.Model.BD_GoodsPrice>();//修改
                List<Hi.Model.BD_GoodsPrice> llll2 = new List<Hi.Model.BD_GoodsPrice>();//新增
                List<Hi.Model.BD_DisPriceInfo> llll3 = new List<Hi.Model.BD_DisPriceInfo>();//新增
                // string disIdList = this.hidDisId.Value.Trim();//隐藏的选择代理商列表
                List<Hi.Model.BD_GoodsInfo> ll = obj as List<Hi.Model.BD_GoodsInfo>;
                foreach (Hi.Model.BD_GoodsInfo item in ll)
                {
                    List<Hi.Model.BD_GoodsPrice> lll = new Hi.BLL.BD_GoodsPrice().GetList("", "isnull(dr,0)=0 and isenabled=1 and disid in(" + disIdList + ") and compid=" + this.CompID + " and  goodsinfoid=" + item.ID, "", Tran);
                    if (lll.Count > 0)
                    {
                        foreach (Hi.Model.BD_GoodsPrice item2 in lll)
                        {
                            item2.IsEnabled = false;
                            item2.modifyuser = this.UserID;
                            item2.ts = DateTime.Now;
                            item2.CompID = this.CompID;
                            llll.Add(item2);
                            //new Hi.BLL.BD_GoodsPrice().Update(llll);
                        }
                    }
                    for (int i = 0; i < disIdList.Split(',').Length; i++)
                    {
                        Hi.Model.BD_GoodsPrice model = new Hi.Model.BD_GoodsPrice();
                        Hi.Model.BD_DisPriceInfo model3 = new Hi.Model.BD_DisPriceInfo();

                        //model.DisPriceID = 0;
                        model.DisID = Convert.ToInt32(disIdList.Split(',')[i]);
                        model.CompID = this.CompID;
                        model.GoodsInfoID = item.ID;
                        if (Request["txtPrice"] != null)
                        {
                            model.TinkerPrice = Convert.ToDecimal(Request["txtPrice"].Split(',')[z]);
                        }
                        model.IsEnabled = true;
                        model.CreateUserID = this.UserID;
                        model.CreateDate = DateTime.Now;
                        model.ts = DateTime.Now;
                        model.modifyuser = this.UserID;
                        llll2.Add(model);

                        model3.DisPriceID = disPriceId;
                        //model3.DisID = Convert.ToInt32(disIdList.Split(',')[i]);
                        model3.CompID = this.CompID;
                        model3.GoodsInfoID = item.ID;
                        if (Request["txtPrice"] != null)
                        {
                            model3.TinkerPrice = Convert.ToDecimal(Request["txtPrice"].Split(',')[z]);
                        }
                        model3.IsEnabled = true;
                        model3.CreateUserID = this.UserID;
                        model3.CreateDate = DateTime.Now;
                        model3.ts = DateTime.Now;
                        model3.modifyuser = this.UserID;
                        llll3.Add(model3);
                    }
                    z++;
                }
                new Hi.BLL.BD_GoodsPrice().Update(llll, Tran);
                new Hi.BLL.BD_GoodsPrice().Add(llll2, Tran);
                new Hi.BLL.BD_DisPriceInfo().Add(llll3, Tran);
                Tran.Commit();
                Response.Redirect("DispriceList.aspx");
                //  JScript.AlertMsg(this, "价格调整成功",);
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
                JScript.AlertMethod(this, "价格调整失败", JScript.IconOption.错误, "function(){location.href='DispriceList.aspx';}");
                return;
            }
        }
        else
        {
            JScript.AlertMethod(this.Page, "选择的商品数据有误", JScript.IconOption.错误);
            return;
        }

    }
}