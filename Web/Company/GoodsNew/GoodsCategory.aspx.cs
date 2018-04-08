using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using Hi.Model;
using System.Text;

public partial class Company_SysManager_GoodsCategory : CompPageBase
{
    public string HiddenUpValue = string.Empty;
    public string HiddenDownValue = string.Empty;
    public int Count = 1;
    public bool Isopen = true;
    public List<Hi.Model.BD_GoodsCategory> categoryList = new List<BD_GoodsCategory>();
    public List<Hi.Model.BD_GoodsCategory> List50 = new List<BD_GoodsCategory>();

    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["type"];
        if (obj != null)
        {
            if (obj.ToString() == "1")
            {
                this.isShow.Visible = false;
                isright.Style.Add("margin-top", "0px");
                isright.Style.Add("margin-left", "0px");
                isright.Style.Add("width", "auto");
                top1.Visible = false;
            }
            else
            {
                this.isShow.Visible = true;
                top1.Visible = true;
                this.isright.Attributes.Add("style", "margin-left:130px;margin-top:60px;");
            }
        }
        if (Request.Form["Action"] != null)
        {
            if (Request.Form["Action"] == "Del")
            {
                Response.Write(DelType(Request.Form["Id"]));
                Response.End();
            }
            if (Request.Form["Action"] == "Sort")
            {
                Response.Write(SortUp(Request.Form["Id"]));
                Response.End();
            }
            if (Request.Form["Action"] == "SortDown")
            {
                Response.Write(SortDown(Request.Form["Id"]));
                Response.End();
            }
            if (Request.Form["Action"] == "Msg")
            {
                Response.Write(GetMsgFor(Request.Form["Id"]));
                Response.End();
            }
            if (Request["Action"] == "Gtype")
            {
                Response.Write(Gtype(Request["Gid"]));
                Response.End();
            }

        }

        bind();
        if (!IsPostBack)
        {
            categoryList = new Hi.BLL.BD_GoodsCategory().GetList("ParentId,ID,GoodsTypeID,CategoryName,SortIndex,Deep", "CompID=" + this.CompID + " and isnull(dr,0)=0 and IsEnabled = 1", "");
            if (categoryList != null && categoryList.Count > 30)
                Isopen = false;
            DataBinds();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void DataBinds()
    {
        List<Hi.Model.BD_GoodsCategory> LType = new List<BD_GoodsCategory>();
        OpenList(0);
        LType = List50;

        this.rptDisTypeList.DataSource = LType;
        this.rptDisTypeList.DataBind();

        if (HiddenUpValue.Length >= 2)
            this.HiddenUp.Value = HiddenUpValue.Substring(0, HiddenUpValue.Length - 1);

        if (HiddenUpValue.Length >= 2)
            this.HiddenDown.Value = HiddenDownValue.Substring(0, HiddenDownValue.Length - 1);
    }

    private void OpenList(int parentID)
    {
        List<Hi.Model.BD_GoodsCategory> list = categoryList.Where(T => T.ParentId == parentID).OrderBy(T => T.SortIndex.ToInt(0)).ToList();
        if (list != null && list.Count > 0)
        {
            HiddenUpValue += list[0].ID + ",";
            HiddenDownValue += list[list.Count - 1].ID + ",";

            foreach (var item in list)
            {
                List50.Add(item);
                OpenList(item.ID);
            }
        }
    }

    /// <summary>
    /// 新增子类
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string Typename =Common.NoHTML( txtTypeName.Value.Trim());
        string Parentid =Common.NoHTML( hideTypeId.Value.Trim());
        string Typecode = string.Empty;
        string ParentIds = "0";//父元素ID   用于判断是否存在重复子元素
        Hi.Model.BD_GoodsCategory DisType = new Hi.Model.BD_GoodsCategory();

        if (Parentid != "0" && Parentid != "")
        {
            Hi.Model.BD_GoodsCategory one = new Hi.BLL.BD_GoodsCategory().GetModel(Convert.ToInt32(Parentid));
            if (one != null)
            {
                Typecode = one.GoodsTypeID.ToString(); //商品大类
                DisType.Code = one.Code + "-" + NewCategoryCode((one.Deep + 1).ToString());
                DisType.Deep = one.Deep + 1;
                DisType.ParCode = one.Code;
                ParentIds = one.ID.ToString();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('添加失败！', IconOption.哭脸, 2000);</script>");
                return;
            }

            if (DisType.Deep == 4)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('最多只能三级！', IconOption.哭脸, 2000);</script>");
                return;
            }
        }
        else//一级大类
        {
            Typecode = hid_txtGtype.Value; ;//商品大类
            DisType.Code = NewCategoryCode("1");
            DisType.Deep = 1;
            DisType.ParCode = "";
            ParentIds = "0";
        }

        int Result = 0;
        if (string.IsNullOrEmpty(Typecode) || Typecode == "0")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('商品大类不能为空！', IconOption.哭脸, 2000);</script>");
            return;
        }
        else if(Parentid == "0")
        {
            Hi.Model.SYS_GType gtype = new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(Typecode));
            if (gtype.Deep != 3)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('请选择三级商品大类！', IconOption.哭脸, 2000);</script>");
                return;
            }
        }
        if (string.IsNullOrEmpty(Typename))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('分类名称不能为空！', IconOption.哭脸, 2000);</script>");
            return;
        }
        if (IsExistsType("CategoryName", Typename, ParentIds))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('分类名称已存在！', IconOption.哭脸, 2000);</script>");
            return;
        }
        if (string.IsNullOrEmpty(Parentid) || !int.TryParse(Parentid, out Result))
        {
            DisType.ParentId = 0;
        }
        else
        {
            DisType.ParentId = Result;
        }
        DisType.CompID = CompID;
        DisType.CategoryName = Typename;
        DisType.GoodsTypeID = Convert.ToInt32(Typecode);
        DisType.SortIndex = NewCateId().ToString();
        DisType.CreateDate = DateTime.Now;
        DisType.CreateUserID = 0;
        DisType.IsEnabled = 1;
        DisType.ts = DateTime.Now;
        DisType.modifyuser = 0;

        SqlTransaction trans = SqlHelper.CreateStoreTranSaction();
        try
        {
            int countID = 0;
            if ((countID = new Hi.BLL.BD_GoodsCategory().Add(DisType, trans)) > 0)
            {
                List<Hi.Model.BD_Goods> gList = new Hi.BLL.BD_Goods().GetList("", " CategoryID='" + Parentid + "'", "");
                if (gList != null && gList.Count > 0)
                {
                    foreach (var bdGoodse in gList)
                    {
                        bdGoodse.CategoryID = countID;
                        new Hi.BLL.BD_Goods().Update(bdGoodse, trans);
                    }
                }
                trans.Commit();

                Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('新增成功！', IconOption.笑脸, 2000);location.href='GoodsCategory.aspx?type=" + Request["type"].ToString() + "';</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('添加失败！', IconOption.哭脸, 2000);</script>");
            }
        }
        catch (Exception ex)
        {
          
            if (trans != null)
            {
                if (trans.Connection != null)
                {
                    trans.Rollback();
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('"+ ex .Message+ "', IconOption.哭脸, 2000);</script>");
            return;
        }
        finally
        {
            SqlHelper.ConnectionClose();
            if (trans != null)
            {
                if (trans.Connection != null)
                {
                    trans.Connection.Close();
                }
            }
        }
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string Typename =Common.NoHTML( txtTypeNames.Value.Trim());
        string id =Common.NoHTML( hideTypeIds.Value.Trim());
        string sortid = Common.NoHTML( txtSortIndexs.Value.Trim());
        //string typecode = txtTypecodes.Value.Trim();
        int Result = 0;
        if (string.IsNullOrEmpty(Typename))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('分类名称不能为空！', IconOption.哭脸, 2000);</script>");
            return;
        }
        Hi.Model.BD_GoodsCategory model = new Hi.BLL.BD_GoodsCategory().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            if (IsExistsType("CategoryName", Typename, id, model.ParentId.ToString()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('分类名称已存在！', IconOption.哭脸, 2000);</script>");
                return;
            }
            if (model.Deep == 1)
            {
                if (this.hid_txtGtype2.Value == "0")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('请选择大类！', IconOption.哭脸, 2000);</script>");
                    return;
                }
            }
        }
        if (int.TryParse(id, out Result))
        {
            //Hi.Model.BD_GoodsCategory DisType = categoryList.Find(p=>p.ID ==Result);
            Hi.Model.BD_GoodsCategory DisType = new Hi.BLL.BD_GoodsCategory().GetModel(Result);
            if (DisType != null)
            {
                DisType.CategoryName = Typename;
                DisType.SortIndex = sortid;
                //DisType.TypeCode = typecode;
                DisType.GoodsTypeID = Convert.ToInt32(this.hid_txtGtype2.Value);
                DisType.ts = DateTime.Now;
                DisType.modifyuser = 0;
                if (new Hi.BLL.BD_GoodsCategory().Update(DisType))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                        "<script>layerCommon.msg('编辑成功！', IconOption.笑脸, 2000);location.href=location.href;</script>");
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('编辑失败！', IconOption.哭脸, 2000);</script>");
                return;
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.alert('分类异常！', IconOption.错误, 2000);</script>");
            return;
        }

    }

    /// <summary>
    /// 删除分类
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string DelType(string id)
    {
        string json = string.Empty;
        if (!string.IsNullOrEmpty(id))
        {
            categoryList = new Hi.BLL.BD_GoodsCategory().GetList("", " ParentId = '" + id + "' and CompID=" + this.CompID + " and isnull(dr,0)=0 ", "");
            List<Hi.Model.BD_GoodsCategory> List = categoryList != null && categoryList.Count > 0 ? categoryList.Where(p => p.ParentId.ToString() == id).ToList() : null;
            if (List != null && List.Count > 0)
            {
                return "{\"result\":false,\"code\":\"此类别下还有子级类别，请先删除子级类别!\"}";
            }
            List<Hi.Model.BD_Goods> Dis = new Hi.BLL.BD_Goods().GetList(null, " CompID=" + CompID + " and isnull(dr,0)=0  and  CategoryID=" + id, null);
            if (Dis != null && Dis.Count > 0)
            {
                return "{\"result\":false,\"code\":\"此分类已被使用，不允许删除!\"}";
            }

            Hi.Model.BD_GoodsCategory type = new Hi.BLL.BD_GoodsCategory().GetModel(id.ToInt(0));
            if (type != null && type.dr == 0)
            {
                type.dr = 1;
                type.ts = DateTime.Now;
                type.modifyuser = UserID;
                if (new Hi.BLL.BD_GoodsCategory().Update(type))
                {
                    categoryList = new Hi.BLL.BD_GoodsCategory().GetList("",
                        "CompID=" + this.CompID + " and IsEnabled = 1 and isnull(dr,0)=0 ", "");

                    return "{\"result\":true,\"code\":\"操作成功\"}";
                }
            }
            else
            {
                return "{\"result\":false,\"code\":\"类别已删除!\"}";
            }
        }
        return json;
    }

    /// <summary>
    /// 上移分类
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string SortUp(string id)
    {
        categoryList = new Hi.BLL.BD_GoodsCategory().GetList("", "CompID=" + this.CompID + " and isnull(dr,0)=0 ", "");
        if (categoryList == null || categoryList.Count == 0)
            return "{\"result\":false,\"code\":\"操作失败\"}";
        string json = string.Empty;
        if (!string.IsNullOrEmpty(id))
        {
            Hi.Model.BD_GoodsCategory type = categoryList.Find(p => p.ID.ToString() == id);
            if (type == null) return "";
            string sort = type.SortIndex;
            List<Hi.Model.BD_GoodsCategory> LType = categoryList.Where(p => p.ParentId == type.ParentId).OrderBy(p => p.SortIndex).ToList();
            if (LType == null || LType.Count < 2 || LType[0] == type) return json;
            int count = 0;
            for (int i = 0; i < LType.Count; i++)
            {
                if (LType[i].ID == type.ID)
                    count = i - 1;
            }
            type.SortIndex = LType[count].SortIndex == "" ? NewCateId().ToString() : LType[count].SortIndex;
            type.ts = DateTime.Now;
            type.modifyuser = UserID;
            LType[count].SortIndex = sort;
            LType[count].SortIndex = LType[count].SortIndex;
            LType[count].ts = DateTime.Now;
            if (new Hi.BLL.BD_GoodsCategory().Update(type) && new Hi.BLL.BD_GoodsCategory().Update(LType[count]))
            {
                return "{\"result\":true,\"code\":\"操作成功\"}";
            }
        }
        return json;
    }

    /// <summary>
    /// 下移
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string SortDown(string id)
    {
        categoryList = new Hi.BLL.BD_GoodsCategory().GetList("", "CompID=" + this.CompID + " and isnull(dr,0)=0 ", "");
        if (categoryList == null || categoryList.Count == 0)
            return "{\"result\":false,\"code\":\"操作失败\"}";
        string json = string.Empty;
        if (!string.IsNullOrEmpty(id))
        {
            //老数据没有排序，就添加一个
            Hi.Model.BD_GoodsCategory type = categoryList.Find(p => p.ID.ToString() == id);
            if (type == null) return "";
            string sort = type.SortIndex;
            List<Hi.Model.BD_GoodsCategory> LType = categoryList.Where(p => p.ParentId == type.ParentId).OrderBy(p => p.SortIndex).ToList();
            if (LType == null || LType.Count < 2 || LType[LType.Count - 1] == type) return json;
            int count = 0;
            for (int i = 0; i < LType.Count; i++)
            {
                if (LType[i].ID == type.ID)
                    count = i + 1;
            }
            type.SortIndex = LType[count].SortIndex == "" ? NewCateId().ToString() : LType[count].SortIndex;
            type.ts = DateTime.Now;
            type.modifyuser = UserID;
            LType[count].SortIndex = sort;
            LType[count].SortIndex = LType[count].SortIndex;
            LType[count].ts = DateTime.Now;
            if (new Hi.BLL.BD_GoodsCategory().Update(type) && new Hi.BLL.BD_GoodsCategory().Update(LType[count]))
            {
                return "{\"result\":true,\"code\":\"操作成功\"}";
            }
        }
        return json;
    }

    /// <summary>
    /// 判断属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsExistsType(string name, string value, string ParentId)
    {
        bool bfg = false;
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            List<Hi.Model.BD_GoodsCategory> List = new Hi.BLL.BD_GoodsCategory().GetList("", name + "='" + value + "' and CompID=" + CompID + " and isnull(dr,0)=0 and ParentId=" + ParentId, "");
            if (List != null && List.Count > 0)
            {
                bfg = true;
            }
        }
        return bfg;
    }

    /// <summary>
    /// 判断属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsExistsType(string name, string value, string id, string ParentId)
    {
        bool bfg = false;
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            List<Hi.Model.BD_GoodsCategory> List = new Hi.BLL.BD_GoodsCategory().GetList("ID", name + "='" + value + "' and id<>'" + id + "' and CompID=" + CompID + " and isnull(dr,0)=0  and ParentId=" + ParentId, "");
            if (List != null && List.Count > 0)
            {
                bfg = true;
            }
        }
        return bfg;
    }

    protected string Simage(object obj)
    {
        string image = "../images/menu_plus.gif";
        if (obj != null)
        {
            List<Hi.Model.BD_GoodsCategory> l = categoryList.Where(p => p.ParentId.ToString() == obj.ToString()).ToList();
            if (l != null && l.Count > 0)
            {
                image = "../images/menu_plus.gif";
            }
            else
            {
                image = "../images/menu_minus.gif";
            }
            return image;
        }
        return image;
    }

    protected string GetDeep(object obj)
    {
        string res = "1";
        if (obj != null)
        {
            Hi.Model.BD_GoodsCategory l = new Hi.BLL.BD_GoodsCategory().GetModel(Convert.ToInt32(obj));
            if (l != null)
            {
                res = l.Deep.ToString();
            }
        }
        return res;
    }

    protected string IsOpen(object obj)
    {
        string image = "0";
        if (obj != null)
        {
            List<Hi.Model.BD_GoodsCategory> l = categoryList.Where(p => p.ParentId.ToString() == obj.ToString()).ToList();
            if (l != null && l.Count > 0)
            {
                image = "1";
            }
            else
            {
                image = "0";
            }
            return image;
        }
        return image;
    }

    /// <summary>
    /// 最新 的排序号
    /// </summary>
    /// <returns></returns>
    public static int NewCateId()
    {
        List<int> intList = new List<int>();
        List<Hi.Model.BD_GoodsCategory> goodsCategoryList = new Hi.BLL.BD_GoodsCategory().GetAllList();
        if (goodsCategoryList != null && goodsCategoryList.Count > 0)
        {
            intList.AddRange(goodsCategoryList.Select(item => Convert.ToInt32(string.IsNullOrEmpty(item.SortIndex) ? "0" : item.SortIndex)));
            return intList.Max() != 0 ? intList.Max() + 1 : 1000;
        }
        else
        {
            return 1000;
        }
    }

    /// <summary>
    /// 获取最大Code
    /// </summary>
    /// <param name="deep"></param>
    /// <returns></returns>
    public string NewCategoryCode(string deep)
    {
        string NewCode = string.Empty;
        List<Hi.Model.BD_GoodsCategory> oneList = new Hi.BLL.BD_GoodsCategory().GetList("", "deep = '" + deep + "'", "ID desc");
        if (oneList != null && oneList.Count > 0)
        {
            if (deep == "1")
                NewCode = oneList[0].Code.Substring(0, 4);
            else if (deep == "2")
            {
                NewCode = oneList[0].Code.Substring(5, 4);
            }
            else
            {
                NewCode = oneList[0].Code.Substring(10, 4);
            }
        }
        else
        {
            NewCode = "999";
        }
        return (Convert.ToInt32(NewCode) + 1).ToString();
    }

    public string GetMsgFor(string id)
    {
        if (id != "")
        {
            List<Hi.Model.BD_Goods> gList = new Hi.BLL.BD_Goods().GetList("", " CategoryID='" + id.ToString() + "'", "");
            return gList != null && gList.Count > 0 ? "1" : "0";
        }
        return "0";
    }

    /// <summary>
    /// 绑定  代理商分类 区域 
    /// </summary>
    public string GoodsType = string.Empty;//代理商分类数据源
    public void bind()
    {

        int IndID = new Hi.BLL.BD_Company().GetModel(this.CompID).IndID;
        //绑定分类
        StringBuilder sbdis = new StringBuilder();
        List<Hi.Model.SYS_GType> godtype1 = new Hi.BLL.SYS_GType().GetList("TypeCode,ID,TypeName", "isnull(dr,0)=0 and  ParentId=0 ", "");
        if (godtype1.Count > 0)
        {
            sbdis.Append("[");
            int count = 0;
            foreach (var item in godtype1)
            {
                count++;
                sbdis.Append("{code:'" + item.ID + "',value: '" + item.ID + "',label: '" + item.TypeName + "'");
                List<Hi.Model.SYS_GType> godtype2 = new Hi.BLL.SYS_GType().GetList("TypeCode,ID,TypeName", "isnull(dr,0)=0  and ParentId=" + item.ID, "");
                if (godtype2.Count > 0)
                {
                    sbdis.Append(",children: [");
                    int count2 = 0;
                    foreach (var item2 in godtype2)
                    {
                        count2++;
                        sbdis.Append("{code:'" + item2.ID + "',value: '" + item2.ID + "',label: '" + item2.TypeName + "'");
                        List<Hi.Model.SYS_GType> godtype3 = new Hi.BLL.SYS_GType().GetList("TypeCode,ID,TypeName", "isnull(dr,0)=0  and ParentId=" + item2.ID, "");
                        if (godtype3.Count > 0)
                        {
                            sbdis.Append(",children: [");
                            int count3 = 0;
                            foreach (var item3 in godtype3)
                            {
                                count3++;
                                if (count3 == godtype3.Count)
                                    sbdis.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.TypeName + "'}");
                                else
                                    sbdis.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.TypeName + "'},");

                            }
                            sbdis.Append("]");

                        }

                        if (count2 == godtype2.Count)
                            sbdis.Append("}");
                        else
                            sbdis.Append("},");
                    }
                    sbdis.Append("]");

                }
                if (count == godtype1.Count)
                    sbdis.Append("}");
                else
                    sbdis.Append("},");
            }
            sbdis.Append("]");
            GoodsType = sbdis.ToString();
        }

    }

    /// <summary>
    /// 获取行业分类三级分类名称
    /// </summary>
    /// <param name="ParentId"></param>
    /// <returns></returns>
    public string GetGtypeName(string ParentId)
    {
        Hi.Model.SYS_GType gtype3 = new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(ParentId));
        if (gtype3 != null)
        {
            Hi.Model.SYS_GType gtype2 = new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(gtype3.ParentId));
            if(gtype2==null)
             return "（请重新选择系统大类）";
            Hi.Model.SYS_GType gtype1 = new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(gtype2.ParentId));
            if(gtype1==null)
                return "（请重新选择系统大类）";
            return "（" + gtype1.TypeName + ">" + gtype2.TypeName + ">" + gtype3.TypeName+"）";
        }
        else
            return "（请重新选择系统大类）";
    }


    /// <summary>
    /// 获取商品分类
    /// </summary>
    /// <param name="Gid"></param>
    /// <returns></returns>
    public string Gtype(string Gid)
    {
        Hi.Model.SYS_GType gtype = new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(Gid));
        if (gtype != null)
        {
            return "{\"TypeName\":\"" + gtype.TypeName + "\",\"ID\":\"" + gtype.ID + "\"}";
        }
        else
            return "{\"TypeName\":\"请选择\",\"ID\":\"0\"}";
    }

}