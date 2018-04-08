using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using DBUtility;

public partial class Company_GoodsNew_GoodsList : CompPageBase
{
    public string page = "1";//默认初始页
    int TitleIndex = 2;
    bool Eroor = false;
    string TitleError = string.Empty;
    string categoryId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if ((Request["isoffline"] + "") != "")
            {
                this.ddlState.SelectedValue = Request["isoffline"].ToString();
            }
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Bind();

            if (!Common.HasRight(this.CompID, this.UserID, "1211"))
            {
                this.btnnpoi.Visible = false;
                this.btnliAdd.Visible = false;
                ViewState["IsPower"] = false;
            }
            else
            {
                ViewState["IsPower"] = true;
            }
            //商品删除上下架
            if (!Common.HasRight(this.CompID, this.UserID, "1212"))
            {
                this.xj.Visible = false;
                this.sj.Visible = false;
                this.btnliDel.Visible = false;
            }
        }
    }
    /// <summary>
    /// 商品列表绑定
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = "  and ComPid=" + this.CompID + " and isnull(dr,0)=0";

        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }
        else
        {
            if (this.ddlState.SelectedValue != "")
            {
                strWhere += " and isNUll(IsOffLine,0)=" + this.ddlState.SelectedValue;
            }
        }
        //每页显示的数据设置
        if (this.txtPageSize.Value.ToString() != "")
        {
            if (this.txtPageSize.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.BD_Goods> l = new Hi.BLL.BD_Goods().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", true, strWhere, out pageCount, out Counts);
        List<int> CateIds = l.Select(T => T.CategoryID).ToList();
        CateIds = CateIds.Distinct().ToList();
        List<Hi.Model.SYS_GType> CateList = new List<Hi.Model.SYS_GType>();
        if (CateIds.Count > 0)
        {
            CateList = new Hi.BLL.SYS_GType().GetList("TypeName,ID", "dr=0 and ID in(" + string.Join(",", CateIds) + ")", "");
        } 
        ViewState["CateList"] = CateList;
        this.rptGoods.DataSource = l;
        this.rptGoods.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();

    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string Where()
    {
        string strWhere = string.Empty;
        //赋值
        string goodsName =Common.NoHTML( this.txtGoodsName.Value.Trim().Replace("'", "''"));//商品名称
        string isEnabled =Common.NoHTML( this.ddlState.SelectedItem.Value.Trim());//状态
        string idlist = string.Empty;
        string idlist2 = string.Empty;
        string hideID =Common.NoHTML( this.txtCategory.treeId.Trim());
       // string recommendId = this.ddlRecommend.SelectedItem.Value.Trim();//是否推荐
        if (this.txtCategory.treeId.Trim() != "")
        {
            string goodsidlist = string.Empty;
            if (!Util.IsEmpty(hideID) && !Util.IsEmpty(txtCategory.treeName))
            {
                string cateID = Common.CategoryId(Convert.ToInt32(hideID), this.CompID);//商品分类递归
                strWhere += " and categoryID in (" + cateID + ") ";
            }
        }
        if (!Util.IsEmpty(goodsName))
        {
            strWhere += " and goodsname like '%" + goodsName.Replace("'", "''") + "%'";
        }
        if (isEnabled != "")
        {
            strWhere += " and  isnull(IsOffline,0)=" + isEnabled;
        }
        //if (recommendId != "")
        //{
        //    strWhere += " and isrecommended=" + recommendId;
        //}
        return strWhere;
    }
    /// <summary>
    /// 获取分类名称
    /// </summary>
    /// <returns></returns>
    public string GoodsCategory(string id)
    {
        if (!Util.IsEmpty(id))
        {
            if (ViewState["CateList"] is List<Hi.Model.SYS_GType>)
            {
                List<Hi.Model.SYS_GType> CateList = ViewState["CateList"] as List<Hi.Model.SYS_GType>;
                List<Hi.Model.SYS_GType> FindList = CateList.Where(T => T.ID == id.ToInt(0)).ToList();
                if (FindList.Count > 0)
                {
                    return FindList[0].TypeName;
                }
                return "";
            }
        }
        else
        {

            return "";
        }
        return "";
    }
    /// <summary>
    /// 获取图片路径
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetPicURL(object Pic)
    {
        if (Pic != null)
        {
            if (!string.IsNullOrEmpty(Pic.ToString()))
            {
                if (Pic.ToString().Trim() != "X")
                {
                    return Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + Pic;
                }
                else
                {
                    return "../../images/havenopicsmallest.gif";
                }
            }
        }
        return "../../images/havenopicsmallest.gif";

    }
    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 搜索按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOffline_Click(object sender, EventArgs e)
    {
        SqlTransaction Tran = null;
        try
        {
            SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
            Connection.Open();
            Tran = Connection.BeginTransaction();
            List<int> CkIds = new List<int>();
            foreach (RepeaterItem row in this.rptGoods.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                    if (fld != null)
                    {
                        CkIds.Add(fld.Value.ToInt(0));
                    }
                }
            }
            List<Hi.Model.BD_Goods> GoodsList = new Hi.BLL.BD_Goods().GetList("", "ID in(" + string.Join(",", CkIds) + ")", "");
            foreach (Hi.Model.BD_Goods goods in GoodsList)
            {
                goods.ts = DateTime.Now;
                goods.modifyuser = this.UserID;
                goods.IsOffline = 1;
                new Hi.BLL.BD_Goods().Update(goods, Tran);
            }
            List<Hi.Model.BD_GoodsInfo> GoodsinfoList = new Hi.BLL.BD_GoodsInfo().GetList("", "dr=0 and GoodsID in(" + string.Join(",", CkIds) + ")", "");
            foreach (Hi.Model.BD_GoodsInfo info in GoodsinfoList)
            {
                info.ts = DateTime.Now;
                info.modifyuser = this.UserID;
                info.IsOffline = 1;
                new Hi.BLL.BD_GoodsInfo().Update(info, Tran);
            }
            Tran.Commit();
        }
        catch (Exception)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
        Bind();
    }
    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOffline2_Click(object sender, EventArgs e)
    {
        SqlTransaction Tran = null;
        try
        {
            SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
            Connection.Open();
            Tran = Connection.BeginTransaction();
            List<int> CkIds = new List<int>();
            foreach (RepeaterItem row in this.rptGoods.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                    if (fld != null)
                    {
                        CkIds.Add(fld.Value.ToInt(0));
                    }
                }
            }
            string CkIdslist = string.Join(",", CkIds);
            if (CkIdslist == null)
                CkIdslist = "''";
            List<Hi.Model.BD_Goods> GoodsList = new Hi.BLL.BD_Goods().GetList("", "ID in(" + CkIdslist + ")", "");
            foreach (Hi.Model.BD_Goods goods in GoodsList)
            {
                goods.ts = DateTime.Now;
                goods.modifyuser = this.UserID;
                goods.IsOffline = 0;
                new Hi.BLL.BD_Goods().Update(goods, Tran);
            }
            List<Hi.Model.BD_GoodsInfo> GoodsinfoList = new Hi.BLL.BD_GoodsInfo().GetList("", " dr=0 and GoodsID in(" + CkIdslist + ")", "");
            foreach (Hi.Model.BD_GoodsInfo info in GoodsinfoList)
            {
                info.ts = DateTime.Now;
                info.modifyuser = this.UserID;
                info.IsOffline = 0;
                new Hi.BLL.BD_GoodsInfo().Update(info, Tran);
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
        Bind();
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDel_Click(object sender, EventArgs e)
    {
        DataTable dt = Common.GoodsIsExist2(this.CompID.ToString());
        if (dt.Rows.Count != 0)
        {
            string strmsg = string.Empty;
            foreach (RepeaterItem row in this.rptGoods.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_GoodsName") as HiddenField;
                    HiddenField fld2 = row.FindControl("HF_Id") as HiddenField;
                    if (fld != null)
                    {
                        string name = fld.Value;
                        int id = fld2.Value.ToInt(0);
                        DataRow[] rows = dt.Select(" GoodsID ='" + id + "'");
                        if (rows.Length > 0)
                        {
                            strmsg += "商品名：" + name + "<br>";
                        }

                    }
                }
            }
            if (!Util.IsEmpty(strmsg))
            {
                JScript.AlertMethod(this, strmsg + "已有订单存在商品，不能删除", JScript.IconOption.错误);
            }
            else
            {
                DelGoods();
            }
        }
        else
        {
            DelGoods();
        }
        Bind();
    }
    /// <summary>
    /// 删除GOods以及goodsinfo表
    /// </summary>
    /// <param name="Tran"></param>
    private void DelGoods()
    {
        SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        try
        {
            foreach (RepeaterItem row in this.rptGoods.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                    if (fld != null)
                    {
                        int goodsId = Convert.ToInt32(fld.Value);
                        // int goodsinfoId = Convert.ToInt32(fld.Value);

                        Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(goodsId, Tran);
                        if (model != null)
                        {
                            model.ts = DateTime.Now;
                            model.modifyuser = this.UserID;
                            model.dr = 1;
                            model.CompID = this.CompID;
                            bool bol = new Hi.BLL.BD_Goods().Update(model, Tran);
                            List<Hi.Model.BD_GoodsInfo> ll = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and   goodsId=" + goodsId, "", Tran);
                            if (ll.Count > 0)
                            {
                                foreach (Hi.Model.BD_GoodsInfo item in ll)
                                {
                                    Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(item.ID, Tran);
                                    if (model2 != null)
                                    {
                                        model2.ts = DateTime.Now;
                                        model2.modifyuser = this.UserID;
                                        model2.dr = 1;
                                        model2.CompID = this.CompID;
                                        new Hi.BLL.BD_GoodsInfo().Update(model2, Tran);
                                    }
                                }
                            }



                            List<Hi.Model.BD_GoodsAttrs> lll = new Hi.BLL.BD_GoodsAttrs().GetList("", "isnull(dr,0)=0 and goodsid=" + goodsId + " and compid=" + this.CompID, "", Tran);
                            if (lll.Count > 0)
                            {
                                foreach (Hi.Model.BD_GoodsAttrs item in lll)
                                {
                                    Hi.Model.BD_GoodsAttrs attsmodel = new Hi.BLL.BD_GoodsAttrs().GetModel(item.ID);
                                    if (attsmodel != null)
                                    {
                                        attsmodel.ts = DateTime.Now;
                                        attsmodel.modifyuser = this.UserID;
                                        attsmodel.dr = 1;
                                        attsmodel.CompID = this.CompID;
                                        new Hi.BLL.BD_GoodsAttrs().Update(attsmodel, Tran);
                                    }
                                    List<Hi.Model.BD_GoodsAttrsInfo> llll = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", "isnull(dr,0)=0 and goodsid=" + goodsId + " and compid=" + this.CompID + " and attrsid=" + item.ID, "", Tran);
                                    if (llll.Count > 0)
                                    {
                                        foreach (Hi.Model.BD_GoodsAttrsInfo item2 in llll)
                                        {
                                            Hi.Model.BD_GoodsAttrsInfo attsinfomodel = new Hi.BLL.BD_GoodsAttrsInfo().GetModel(item2.ID);
                                            if (attsmodel != null)
                                            {
                                                attsinfomodel.ts = DateTime.Now;
                                                attsinfomodel.modifyuser = this.UserID;
                                                attsinfomodel.dr = 1;
                                                attsinfomodel.CompID = this.CompID;
                                                new Hi.BLL.BD_GoodsAttrsInfo().Update(attsinfomodel, Tran);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            Tran.Commit();
        }
        catch (Exception)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
    /// <summary>
    /// 获取单位
    /// </summary>
    /// <param name="goodsId"></param>
    /// <returns></returns>
    public string GetUnti(string goodsId)
    {
        Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(goodsId));
        if (model != null)
        {
            return model.Unit;
        }
        else
        {
            return "";
        }
    }
    #region 商品表格导入
    public void btnAddList_Click(object sender, EventArgs e)
    {
        string path = "";
        int count = 0;
        int count2 = 0;
        int index = 0;
        SqlTransaction Tran = null;
        try
        {
            if (FileUpload1.HasFile == false)//HasFile用来检查FileUpload是否有指定文件
            {
                JScript.AlertMethod(this, "请您选择Excel文件", JScript.IconOption.错误);
                return;//当无文件时,返回
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
            if (IsXls != ".xls" && IsXls != ".xlsx")
            {
                JScript.AlertMethod(this, "只可以选择Excel文件", JScript.IconOption.错误);
                return;//当选择的不是Excel文件时,返回
            }
            if (!Directory.Exists(Server.MapPath("TemplateFile")))
            {
                Directory.CreateDirectory(Server.MapPath("TemplateFile"));
            }
            string filename = FileUpload1.FileName;
            string name = filename.Replace(IsXls, "");
            path = Server.MapPath("TemplateFile/") + name + "-" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + IsXls;
            FileUpload1.SaveAs(path);
            System.Data.DataTable dt = Common.ExcelToDataTable(path, TitleIndex);
            if (dt == null)
            {
                throw new Exception("Excel表中无数据");
            }
            if (dt.Rows.Count == 0)
            {
                throw new Exception("Excel表中无数据");
            }

            string goodsName = string.Empty;
            string goodsPrice = string.Empty;
            string goodsUnit = string.Empty;
            string goodsRemark = string.Empty;
            string goodsKuc = string.Empty;
            string goodsBarCode = string.Empty;
            string goodsCate = string.Empty;
            string goodsCate2 = string.Empty;
            string goodsCate3 = string.Empty;
            DataRow[] rows = dt.Select();
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            Eroor = false;
            TitleError = string.Empty;
            int goodsId = 0;
            foreach (DataRow row in rows)
            {
                List<string[]> al = new List<string[]>();
                string goodsAttrValue = string.Empty;
                string goodsAttr = string.Empty;
                try
                {
                    if (row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "" && row["商品名称*（必填项，不能重复，不超过30个汉字）"].ToString().Trim() == "" && row["计量单位*（必填项，例如：件、个）"].ToString().Trim() == "" && row["价格*（必填项，最多两位小数）"].ToString().Trim() == "")
                    {
                        continue;
                    }
                    index++;
                    if (row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "示例：食品/面包" || row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "示例：食品/牛奶/伊利" || row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "示例：食品/牛奶/伊利")
                    {
                        continue;
                    }
                    string str = string.Empty;//几级商品分类
                    if (Util.IsEmpty(row["分类*（必填项，“/”号分割分类）"].ToString().Trim()))
                    {
                        CheckVal(row["分类*（必填项，“/”号分割分类）"].ToString().Trim(), "分类", index);
                    }
                    else
                    {
                        goodsCate = CheckGoodsCate(CheckVal(row["分类*（必填项，“/”号分割分类）"].ToString().Trim(), "分类", index), "", index, Tran);
                        categoryId = goodsCate.Split('@')[1];
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException)
                    {
                        Eroor = true;
                        TitleError += ex.Message;
                        continue;
                    }
                    else
                    {
                        throw new Exception("商品Excel模版格式错误，请重新下载模版填入数据后导入。");
                    }
                }
                goodsName = GoodsObjExists(CheckVal(row["商品名称*（必填项，不能重复，不超过30个汉字）"].ToString().Trim(), "商品名称", index), "商品名称", index, Tran);
                goodsBarCode = GoodsObjExists(CheckVal(row["商品编码（非必填项，允许为空。如果填了，不能重复，不超过15个字符）"].ToString().Trim(), "商品编码", index), "商品编码", index, Tran);
                //if (goodsName == "ycz" || goodsBarCode == "ycz")
                //{ //已存在的数据，则跳过
                //    count2++;
                //    continue;
                //}
                goodsUnit = CheckGoodsUnit(CheckVal(row["计量单位*（必填项，例如：件、个）"].ToString().Trim(), "计量单位", index), "计量单位", index, Tran);
                goodsPrice = CheckPrice(CheckVal(row["价格*（必填项，最多两位小数）"].ToString().Trim(), "价格", index), "价格", index);
                goodsKuc = CheckVal(row["库存（可设置是否启用）"].ToString().Trim(), "商品库存", index);
                goodsRemark = row["卖点/关键词（可不填，所填内容用来简单描述商品卖点信息）"].ToString().Trim();
                Hi.Model.BD_Goods model = new Hi.Model.BD_Goods();
                model.CompID = this.CompID;
                model.GoodsName = goodsName;
                model.GoodsCode = goodsBarCode;
                model.CategoryID = Convert.ToInt32(categoryId);
                model.Unit = goodsUnit;
                model.SalePrice = Convert.ToDecimal(goodsPrice);
                model.IsOffline = 1;
                model.IsSale = 0;
                model.IsRecommended = 1;
                model.IsIndex = 0;
                model.Title = goodsRemark;
                model.CreateUserID = this.UserID;
                model.CreateDate = DateTime.Now;
                model.IsEnabled = 1;
                model.modifyuser = this.UserID;
                model.ts = DateTime.Now;

                goodsId = new Hi.BLL.BD_Goods().Add(model, Tran);

                Hi.Model.BD_GoodsInfo model4 = new Hi.Model.BD_GoodsInfo();
                model4.CompID = this.CompID;
                model4.BarCode = goodsBarCode;
                model4.GoodsID = goodsId;
                if (!Util.IsEmpty(goodsKuc))
                {
                    model4.Inventory = Convert.ToDecimal(goodsKuc);
                }
                model4.IsOffline = 1;
                model4.ValueInfo = "";
                model4.SalePrice = Convert.ToDecimal(goodsPrice);
                model4.TinkerPrice = Convert.ToDecimal(goodsPrice);
                model4.IsEnabled = true;
                model4.CreateDate = DateTime.Now;
                model4.CreateUserID = this.UserID;
                model4.ts = DateTime.Now;
                model4.modifyuser = this.UserID;
                int goodsInfoId = new Hi.BLL.BD_GoodsInfo().Add(model4, Tran);

                Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(goodsId, Tran);
                goodsModel.ViewInfoID = goodsInfoId;
                goodsModel.ts = DateTime.Now;
                goodsModel.modifyuser = this.UserID;
                new Hi.BLL.BD_Goods().Update(goodsModel, Tran);
                //}
                count++;
            }
            if (!Eroor)
            {
                Tran.Commit();
                string str = string.Empty;
                if (count2 != 0)
                {
                    str = ",剩余" + count2 + "条为重复数据";
                }
                ClientScript.RegisterStartupScript(this.GetType(), "Add", "<script>addlis(" + count + "," + count2 + ",'" + str + "');</script>");
            }
            else
            {
                Tran.Rollback();
                JScript.AlertMethod(this, TitleError, JScript.IconOption.错误, "function(){ addList(); }");
            }
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
            JScript.AlertMethod(this, ex.Message, JScript.IconOption.错误, "function(){ addList(); }");
        }
        finally
        {
            if (!Util.IsEmpty(path))
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

    }
    /// <summary>
    /// 非空判断
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string CheckVal(string value, string str, int index)
    {
        int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompID).ToInt(0);//是否启用库存
        if (str == "商品库存")
        {
            if (IsInve == 0)
            {
                if (Util.IsEmpty(value))
                {
                    Eroor = true;
                    throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已启用不能为空！请修改后重新导入。<br/>");
                }
            }
        }
        else if (str != "商品编码")
        {
            if (Util.IsEmpty(value))
            {
                Eroor = true;
                throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不能为空！请修改后重新导入。<br/>");
            }
        }

        return value;
    }
    /// <summary>
    /// 价格验证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string CheckPrice(string value, string str, int index)
    {
        if (!Regex.IsMatch(value, "^(([0-9]+)|([0-9]+\\.[0-9]{1,5}))$"))
        {
            Eroor = true;
            throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "输入不正确，请修改后重新导入。");
        }
        return value;
    }
    /// <summary>
    /// 计量单位验证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <param name="Tran"></param>
    /// <returns></returns>
    public string CheckGoodsUnit(string value, string str, int index, SqlTransaction Tran)
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
            if (count == 0)
            {
                Eroor = true;
                throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "录入数据库失败，请修改后重新导入。");
            }
        }
        return value;
    }
    /// <summary>
    /// 验证商品大类下是否包含这个商品一级分类
    /// </summary>
    /// <returns></returns>
    public string CheckGoodsCate(string value, string str, int index, SqlTransaction Tran)
    {
        string catestr = string.Empty;
        string[] catelist = value.Split('/');//根据/分隔商品分类
        for (int i = 0; i < catelist.Length; i++)
        {
            if (i == 0)
            {//一级
                List<Hi.Model.SYS_GType> l = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=0   and TypeName='" + catelist[i] + "'", "", Tran);
                if (l.Count == 0)
                {
                    Eroor = true;
                    throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。没有【" + catelist[i] + "】这个一级分类！请修改后重新导入。<br/>");
                }
                else
                {
                    catestr = catelist[i] + "@" + l[0].ID;
                    if (catelist.Length == 1)//只有一级分类情况
                    {
                        List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=" + l[0].ID + " ", "", Tran);
                        if (ll.Count > 0)
                        {
                            Eroor = true;
                            throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。【" + catelist[i] + "】一级分类下还有二级分类，必须选择最小的分类！请修改后重新导入。<br/>");
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else if (i == 1)
            {//二级
                string goodsCateId = string.Empty;
                if (catestr.IndexOf("@") != -1)
                {
                    goodsCateId = catestr.Split('@')[1];
                    catestr = catestr.Split('@')[0];
                }
                else
                {
                    List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=0   and TypeName='" + catestr + "' ", "", Tran);
                    if (ll.Count > 0)
                    {
                        goodsCateId = ll[0].ID.ToString();
                    }
                }
                List<Hi.Model.SYS_GType> l = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1  and TypeName='" + catelist[i] + "'  and parentid=" + goodsCateId, "", Tran);
                if (l.Count == 0)
                {
                    Eroor = true;
                    throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。【" + catestr + "】一级分类下没有【" + catelist[i] + "】这个二级分类！请修改后重新导入。<br/>");
                }
                else
                {
                    catestr = catelist[i] + "@" + l[0].ID;
                    if (catelist.Length == 2)//只有二级分类情况
                    {
                        List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=" + l[0].ID + "", "", Tran);
                        if (ll.Count > 0)
                        {
                            Eroor = true;
                            throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。【" + catelist[i] + "】二级分类下还有三级分类，必须选择最小的分类！请修改后重新导入。<br/>");
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else if (i == 2)
            { //三级
                string goodsCateId = string.Empty;
                if (catestr.IndexOf("@") != -1)
                {
                    goodsCateId = catestr.Split('@')[1];
                    catestr = catestr.Split('@')[0];
                }
                else
                {
                    List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid!=0   and TypeName='" + catestr + "'", "", Tran);
                    if (ll.Count > 0)
                    {
                        goodsCateId = ll[0].ID.ToString();
                    }
                }

                List<Hi.Model.SYS_GType> l = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=" + goodsCateId + " and TypeName='" + catelist[i] + "' ", "", Tran);
                if (l.Count == 0)
                {
                    Eroor = true;
                    throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。【" + catestr + "】二级分类下没有【" + catelist[i] + "】这个三级分类！请修改后重新导入。<br/>");
                }
                else
                {
                    catestr = catelist[i] + "@" + l[0].ID;
                    if (catelist.Length == 3)//只有二级分类情况
                    {
                        break;
                    }
                }
            }
        }
        return catestr;
    }
    /// <summary>
    /// 验证规格属性是否存在一、二、三级分类
    /// </summary>
    /// <returns></returns>
    public string CheckGoodsAttr(string goodsTypeId, string goodsCate3, string value, string str, int index, SqlTransaction Tran)
    {
        string goodsCateId = string.Empty;
        if (goodsCate3.IndexOf("@") != -1)
        {
            categoryId = goodsCate3.Split('@')[1];
            goodsCateId = goodsCate3.Split('@')[1];
            goodsCate3 = goodsCate3.Split('@')[0];
        }
        else
        {
            List<Hi.Model.BD_GoodsCategory> ll = new Hi.BLL.BD_GoodsCategory().GetList("", "isnull(dr,0)=0 and isenabled=1 and goodstypeid=" + goodsTypeId + "   and categoryname='" + goodsCate3 + "' and compid=" + this.CompID, "", Tran);
            if (ll.Count > 0)
            {
                goodsCateId = ll[0].ID.ToString();
            }
        }
        string[] valueList = value.Split('，');//规格属性以逗号分隔
        bool bol = false;//如果有不存在的规格属性，则跳出for
        string strValue = string.Empty;//保存属性
        string listId = string.Empty;//attrId
        for (int i = 0; i < valueList.Length; i++)
        {
            if (!Util.IsEmpty(valueList[i]))
            {
                List<Hi.Model.BD_Attribute> l = new Hi.BLL.BD_Attribute().GetList("", "isnull(dr,0)=0 and isenabled=1 and compId=" + this.CompID + " and attributename='" + valueList[i] + "'", "", Tran);
                if (l.Count == 0)
                {
                    strValue = valueList[i];
                    bol = true;
                    break;
                }
                else
                {
                    foreach (Hi.Model.BD_Attribute item in l)
                    {
                        listId += item.ID + ",";
                        // strValue += item.AttributeName + ",";
                    }

                }
            }
        }
        if (bol)
        {
            Eroor = true;
            throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "不存在" + strValue + "，请修改后重新导入。");
        }
        else
        {
            if (!Util.IsEmpty(listId))
            {
                listId = listId.Substring(0, listId.Length - 1);
                strValue = value.Replace('，', ',');
            }
            List<Hi.Model.BD_CategoryAttribute> lll = new Hi.BLL.BD_CategoryAttribute().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + "  and attributeid in(" + listId + ") and categoryId=" + goodsCateId, "", Tran);
            if (lll.Count == 0)
            {
                Eroor = true;
                throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "不存在" + strValue + "，请修改后重新导入。");
            }
            else
            {
                string liststr = string.Empty;
                foreach (Hi.Model.BD_CategoryAttribute item in lll)
                {
                    liststr += item.ID + ",";
                }
                if (!Util.IsEmpty(liststr))
                {
                    liststr = liststr.Substring(0, liststr.Length - 1);
                }
                return listId + "@" + liststr;
            }
        }
        return listId;
    }
    /// <summary>
    /// 验证属性值是否存在
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public string CheckGoodsAttrValue(string attrlist, string value, string str, int index, SqlTransaction Tran, List<string[]> al)
    {
        string attrId = string.Empty;
        string strList = string.Empty;
        if (attrlist.IndexOf("@") != -1)
        {
            attrId = attrlist.Split('@')[1];
            attrlist = attrlist.Split('@')[0];
        }
        string[] valueList = value.Split('；');//属性值组数组
        string[] valueList2 = { };//属性值数组
        string strValue = string.Empty;//属性值拼接
        bool bol = false;
        bool bol2 = false;
        for (int i = 0; i < valueList.Length; i++)
        {
            string strList2 = string.Empty;
            valueList2 = valueList[i].Split('，');
            for (int z = 0; z < valueList2.Length; z++)
            {
                // strValue += "'" + valueList2[z] + "',";
                List<Hi.Model.BD_AttributeValues> l = new Hi.BLL.BD_AttributeValues().GetList("", "isnull(dr,0)=0 and isenabled=1 and compid=" + this.CompID + " and attributeid in(" + attrId + ") and attrvalue='" + valueList2[z] + "'", "");
                if (l.Count == 0)
                {
                    strValue = valueList2[z];
                    bol = true;
                    break;
                }
                else
                {
                    strList2 += l[0].ID + ",";
                    strList += l[0].ID + ",";
                }
            }
            if (bol)
            {
                bol2 = true;
                break;
            }
            if (!Util.IsEmpty(strList2))
            {
                strList2 = strList2.Substring(0, strList2.Length - 1);
            }
            al.Add(strList2.Split(','));
        }
        if (bol2)
        {
            Eroor = true;
            throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "不存在" + strValue + "，请修改后重新导入。");
        }
        if (!Util.IsEmpty(strList))
        {
            strList = strList.Substring(0, strList.Length - 1);
        }
        return strList;
    }
    /// <summary>
    /// 长度验证
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <param name="Tran"></param>
    /// <returns></returns>
    public string GoodsObjExists(string value, string str, int index, SqlTransaction Tran)
    {
        if (str == "商品名称")
        {
            if (value.Trim().Length > 60)
            {
                Eroor = true;
                throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + "</i> &nbsp;&nbsp;的数据有误。" + str + "过长，请修改后重新导入。<br/>");
            }
            else
            {
                string count = Yanz(value, str, Tran);
                if (count == "1")
                {
                    Eroor = true;
                    throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + "</i> &nbsp;&nbsp;的数据有误。" + value +str +"已存在，请修改后重新导入。<br/>");
                }
                else if (count == "2")
                {
                    Eroor = true;
                    throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不存在这个列，请修改后重新导入。<br/>");
                }
                else {
                    return value;
                }
            }
        }
        else if (str == "商品编码")
        {
            if (!Util.IsEmpty(value))
            {
                if (value.Trim().Length > 15)
                {
                    Eroor = true;
                    throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + "</i> &nbsp;&nbsp;的数据有误。" + str + "过长，请修改后重新导入。<br/>");
                }
                else
                {
                    string count = Yanz(value, str, Tran);
                    if (count == "1")
                    {
                        Eroor = true;
                        throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + "</i> &nbsp;&nbsp;的数据有误。" + value + str + "已存在，请修改后重新导入。<br/>");
                    }
                    else if (count == "2")
                    {
                        Eroor = true;
                        throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 3) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不存在这个列，请修改后重新导入。<br/>");
                    }
                    else
                    {
                        return value;
                    }
                }
            }
        }
        return "";
    }
    /// <summary>
    /// 验证是否重复
    /// </summary>
    /// <param name="goodsname"></param>
    /// <returns></returns>
    public string Yanz(string goodsname, string str, SqlTransaction Tran)
    {
        if (str == "商品名称")
        {
            List<Hi.Model.BD_Goods> count = new Hi.BLL.BD_Goods().GetList("", "goodsname='" + goodsname.Trim() + "' and isnull(dr,0)=0  and compid=" + this.CompID, "", Tran);
            if (count.Count > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        else if (str == "商品编码")
        {
            List<Hi.Model.BD_GoodsInfo> count = new Hi.BLL.BD_GoodsInfo().GetList("", "barcode='" + goodsname.Trim() + "' and isnull(dr,0)=0  and compid=" + this.CompID, "", Tran);
            if (count.Count > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        return "2";
    }

    #endregion
}