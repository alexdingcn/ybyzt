using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DBUtility;

public partial class Company_ImportGoods2 : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.Session["GoodsTable"] != null)
            {
                DataTable dt = HttpContext.Current.Session["GoodsTable"] as DataTable;
                rptGoods.DataSource = dt;
                rptGoods.DataBind();
            }
            else
            {
                JScript.AlertMethod(this, "请先导入", JScript.IconOption.错误, "function(){location.href='ImportGoods.aspx'}");
            }
        }
    }
    /// <summary>
    /// 确定导入
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["GoodsTable"] != null)
        {
            SqlTransaction Tran = null;
            SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
            Connection.Open();
            Tran = Connection.BeginTransaction();
            try
            {
                DataTable dt = HttpContext.Current.Session["GoodsTable"] as DataTable;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["chkstr"].ToString() == "数据正确！")
                    {
                        Hi.Model.BD_Goods model = new Hi.Model.BD_Goods();
                        model.CompID = this.CompID;
                        model.GoodsName = dt.Rows[i]["goodsname"].ToString();
                        model.GoodsCode = "";// dt.Rows[i]["barcode"].ToString(); ;
                        model.CategoryID = Convert.ToInt32(dt.Rows[i]["categoryid"].ToString());
                        model.Unit = CheckGoodsUnit(dt.Rows[i]["unit"].ToString(), "计量单位", Tran);
                        model.SalePrice = Convert.ToDecimal(dt.Rows[i]["price"].ToString());
                        //if (dt.Rows[i]["isoffline"] == null)
                        //{
                        //    model.IsOffline = 1;
                        //}
                        //else
                        //{
                        model.IsOffline = dt.Rows[i]["isoffline"].ToString() == "是" || dt.Rows[i]["isoffline"].ToString() == "" ? 1 : 0;
                        // }
                        model.IsSale = 0;
                        //if (dt.Rows[i]["isrecommended"] == null)
                        //{
                        //    model.IsRecommended = 1;
                        //}
                        //else
                        //{
                        model.IsRecommended = dt.Rows[i]["isrecommended"].ToString() == "是" || dt.Rows[i]["isrecommended"].ToString() == "" ? 1 : 0;
                        // }
                        model.IsIndex = 0;
                        model.Title = dt.Rows[i]["title"].ToString();
                        model.CreateUserID = this.UserID;
                        model.CreateDate = DateTime.Now;
                        model.IsEnabled = 1;
                        model.modifyuser = this.UserID;
                        model.memo = dt.Rows[i]["details"].ToString();
                        model.ts = DateTime.Now;

                        int goodsId = new Hi.BLL.BD_Goods().Add(model, Tran);
                        string[] spec = (string[])dt.Rows[i]["spec"];

                        if (spec.Length == 0)
                        {
                            Hi.Model.BD_GoodsInfo model4 = new Hi.Model.BD_GoodsInfo();
                            model4.CompID = this.CompID;
                            if (dt.Rows[i]["barcode"].ToString() == "")
                            {
                                model4.BarCode = GoodsCode(Tran);

                            }
                            else
                            {
                                model4.BarCode = dt.Rows[i]["barcode"].ToString();
                            }
                            model4.GoodsID = goodsId;
                            if (dt.Rows[i]["inventory"].ToString() != "")
                            {
                                model4.Inventory = Convert.ToDecimal(dt.Rows[i]["inventory"].ToString());
                            }

                            model4.IsOffline = 1;
                            model4.ValueInfo = "";
                            model4.SalePrice = Convert.ToDecimal(dt.Rows[i]["price"].ToString());
                            model4.TinkerPrice = Convert.ToDecimal(dt.Rows[i]["price"].ToString());
                            model4.IsEnabled = true;
                            model4.CreateDate = DateTime.Now;
                            model4.CreateUserID = this.UserID;
                            model4.ts = DateTime.Now;
                            model4.modifyuser = this.UserID;
                            int goodsInfoId = new Hi.BLL.BD_GoodsInfo().Add(model4, Tran);

                            model.ID = goodsId;
                            model.ViewInfoID = goodsInfoId;
                            model.ts = DateTime.Now;
                            model.modifyuser = this.UserID;
                            new Hi.BLL.BD_Goods().Update(model, Tran);
                        }
                        else
                        {
                            List<List<string>> valueList = new List<List<string>>();
                            for (int v = 0; v < spec.Length; v++)
                            {
                                //添加商品属性数据
                                Hi.Model.BD_GoodsAttrs attr = new Hi.Model.BD_GoodsAttrs();
                                attr.GoodsID = goodsId;
                                attr.CompID = CompID;
                                attr.dr = 0;
                                attr.ts = DateTime.Now;
                                attr.modifyuser = UserID;
                                attr.AttrsName = spec[v].Trim();
                                int attrid = new Hi.BLL.BD_GoodsAttrs().Add(attr, Tran);

                                List<string> valueList2 = new List<string>();
                                string[] values = dt.Rows[i]["value" + (v + 1) + ""].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string value in values)
                                {
                                    valueList2.Add(value);

                                    //添加商品属性值明细数据
                                    Hi.Model.BD_GoodsAttrsInfo attrinfo = new Hi.Model.BD_GoodsAttrsInfo();
                                    attrinfo.AttrsID = attrid;
                                    attrinfo.GoodsID = goodsId;
                                    attrinfo.CompID = CompID;
                                    attrinfo.ts = DateTime.Now;
                                    attrinfo.modifyuser = UserID;
                                    attrinfo.AttrsInfoName = value.Trim();
                                    new Hi.BLL.BD_GoodsAttrsInfo().Add(attrinfo, Tran);
                                }
                                valueList.Add(valueList2);

                            }

                            string[] valueInfos = makeValueInfo(valueList, spec);  //生成商品ValueInfo
                            int firstInfoId = 0;
                            foreach (string valueinfo in valueInfos)
                            {
                                Hi.Model.BD_GoodsInfo goodsinfo = new Hi.Model.BD_GoodsInfo();
                                goodsinfo.IsOffline = 1;
                                goodsinfo.ValueInfo = "";
                                goodsinfo.SalePrice = dt.Rows[i]["price"].ToString().ToDecimal(0);
                                goodsinfo.TinkerPrice = dt.Rows[i]["price"].ToString().ToDecimal(0);
                                goodsinfo.IsEnabled = true;
                                goodsinfo.CreateDate = DateTime.Now;
                                goodsinfo.CreateUserID = this.UserID;
                                goodsinfo.ts = DateTime.Now;
                                goodsinfo.modifyuser = this.UserID;
                                goodsinfo.GoodsID = goodsId;
                                if (dt.Rows[i]["inventory"].ToString() != "")
                                {
                                    goodsinfo.Inventory = Convert.ToDecimal(dt.Rows[i]["inventory"].ToString());
                                }
                                goodsinfo.BarCode = GoodsCode(Tran);
                                goodsinfo.CompID = this.CompID;
                                goodsinfo.ValueInfo = valueinfo;

                                string[] infos= valueinfo.Split(new string[] { "；" }, StringSplitOptions.RemoveEmptyEntries);
                                for (int index = 0; index < infos.Length; index++)
                                {
                                    goodsinfo.GetType().GetProperty("Value" + (index + 1) + "").SetValue(goodsinfo, infos[index].Split(new char[] { ':' })[1], null);
                                }

                                int goodsInfoId = new Hi.BLL.BD_GoodsInfo().Add(goodsinfo, Tran);
                                if (firstInfoId == 0)
                                {
                                    firstInfoId = goodsInfoId;
                                }

                            }

                            model.ID = goodsId;
                            model.ViewInfoID = firstInfoId;
                            model.ts = DateTime.Now;
                            model.modifyuser = this.UserID;
                            new Hi.BLL.BD_Goods().Update(model, Tran);
                        }
                    }
                }
                Tran.Commit();
                Response.Redirect("ImportGoods3.aspx", false);
                //ClientScript.RegisterStartupScript(this.GetType(), "Add", "<script>addlis(" + count + "," + count2 + ",'" + str + "');</script>");
            }
            catch (Exception ex)
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                    {
                        Tran.Rollback();
                    }
                }
                HttpContext.Current.Session["GoodsTable"] = null;
                JScript.AlertMethod(this, "系统错误，导入商品失败！", JScript.IconOption.错误, "function(){location.href='ImportGoods.aspx'}");
            }
        }
        else
        {
            JScript.AlertMethod(this, "Excel没有数据，请重新导入", JScript.IconOption.错误, "function(){location.href='ImportGoods.aspx'}");
        }
    }

    /// <summary>
    /// 生成商品ValueInfo
    /// </summary>
    /// <param name="valueList"></param>
    /// <param name="sepc"></param>
    /// <returns></returns>
    public string[] makeValueInfo(List<List<string>> valueList, string[] sepc)
    {
        List<string> valueInfos = new List<string>();
        if (valueList.Count > 0 && sepc.Length > 0)
        {
            List<string> valueList1 = valueList[0]; //以第一条规格值为索引 递归创建商品的属性规格
            valueList.RemoveAt(0);
            foreach (string value in valueList1)
            {
                string info = sepc[0] + ":" + value + "；";
                createInfo(valueList, sepc, info, 0, ref valueInfos);
            }

        }
        return valueInfos.ToArray();
    }

    /// <summary>
    /// 递归调用规格属性
    /// </summary>
    /// <param name="valueList"></param>
    /// <param name="sepc"></param>
    /// <param name="valueinfo"></param>
    /// <param name="index"></param>
    public void createInfo(List<List<string>> valueList, string[] sepc, string valueinfo,int index,ref List<string> valueInfos)
    {
        //判断规格属性值是否大于等于当前索引
        if (valueList.Count >= index + 1)
        {
            List<string> newVList = valueList[index];
            foreach (string value in newVList)
            {
                string newinfo= valueinfo+ sepc[index + 1] + ":" + value + "；";
                createInfo(valueList, sepc, newinfo, index + 1,ref valueInfos);
            }
        }
        else
        {
            valueInfos.Add(valueinfo);
        }
    }



    /// <summary>
    /// 计量单位验证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <param name="Tran"></param>
    /// <returns></returns>
    public string CheckGoodsUnit(string value, string str, SqlTransaction Tran)
    {
        List<Hi.Model.BD_DefDoc_B> l = new Hi.BLL.BD_DefDoc_B().GetList("", " AtVal = '" + value.Trim() + "' and AtName='计量单位' and compid=" + this.CompID + " and isnull(dr,0)=0", "", Tran);
        if (l.Count == 0)
        {
            List<Hi.Model.BD_DefDoc> list = new Hi.BLL.BD_DefDoc().GetList("", " AtName='计量单位' and compid=" + this.CompID + " and isnull(dr,0)=0", "", Tran);
            int defid = 0;
            if (list.Count == 0)
            {
                Hi.Model.BD_DefDoc doc = new Hi.Model.BD_DefDoc();
                doc.CompID = this.CompID;
                doc.AtCode = "";
                doc.AtName = "计量单位";
                doc.ts = DateTime.Now;
                doc.modifyuser = this.UserID;
                doc.dr = 0;
                defid = new Hi.BLL.BD_DefDoc().Add(doc, Tran);
            }
            else
            {
                defid = list[0].ID;
            }
            Hi.Model.BD_DefDoc_B item = new Hi.Model.BD_DefDoc_B();
            item.AtVal = value;
            item.AtName = "计量单位";
            item.CompID = this.CompID;
            item.DefID = defid;
            item.ts = DateTime.Now;
            item.dr = 0;
            item.modifyuser = this.UserID;
            int count = new Hi.BLL.BD_DefDoc_B().Add(item, Tran);
            //if (count == 0)
            //{
            //    JScript.AlertMethod(this, "Excel没有数据，请重新导入", JScript.IconOption.错误, "function(){location.href='ImportGoods.aspx'}");
            //    throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "录入数据库失败，请修改后重新导入。");
            //}
        }
        return value;
    }
    /// <summary>
    /// 获取商品编码并更新数据库最新单据值
    /// </summary>
    /// <returns></returns>
    public string GoodsCode(SqlTransaction Tran)
    {
        List<Hi.Model.SYS_SysCode> l = new Hi.BLL.SYS_SysCode().GetList("top 1 *", "isnull(dr,0)=0 and compId=" + this.CompID + " and codeName='P" + this.CompID + "'", " codevalue desc", Tran);
        if (l.Count > 0)
        {
            l[0].CodeValue = (l[0].CodeValue.ToInt(0) + 1).ToString();
            l[0].ts = DateTime.Now;
            l[0].modifyuser = this.UserID;
            new Hi.BLL.SYS_SysCode().Update(l[0], Tran);
            return l[0].CodeName + l[0].CodeValue.PadLeft(6, '0');
        }
        else
        {

            Hi.Model.SYS_SysCode model5 = new Hi.Model.SYS_SysCode();
            model5.CompID = this.CompID;
            model5.CodeName = "P" + this.CompID;
            model5.CodeValue = "1";
            model5.ts = DateTime.Now;
            model5.dr = 0;
            model5.modifyuser = this.UserID;
            new Hi.BLL.SYS_SysCode().Add(model5, Tran);
            return "P" + this.CompID + "1".PadLeft(6, '0');
        }
    }

}