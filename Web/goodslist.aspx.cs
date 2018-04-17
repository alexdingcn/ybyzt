using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Text;

public partial class goodslist : LoginPageBase
{
    public string GetTxtcontent = string.Empty;//一级搜索文本框的值
   
    public string types = "1";//CategoryID  商品种类
    public string page = "1";//分页页码数
    public string where = "";//内部条件
    public string SqlQuery = string.Empty;//完整的要查询的SQL
    public string GoodsName = "";//二级搜索名称
    public string GoodsNameWeb = "请输入商品名称";//二级搜索名称前台显示
    public string AddName = "";//所在地区查询
    public string AddNameWeb = "请选择";//所在地区查询前台显示
    public string CategoryIDUrl = "0";//地址栏ID
    public string CategoryID = "0";//一级分类ID
    public string CategoryID2 = "0";//二级分类ID
    public string CategoryID3 = "0";//三级分类ID
    public string CategoryID3List = "";//三级分类ID集合
    string fldSort = " CreateDate desc ";
    public string keywords = "B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统";
    public string description = "医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["CategoryID"]))
            CategoryIDUrl = Request.QueryString["CategoryID"];
        else
            CategoryIDUrl = GtypeHid.Value;

        if (!string.IsNullOrEmpty(Request.QueryString["GoodsName"]))
            GetTxtcontent = Common.NoHTML(Request.QueryString["GoodsName"]);
        if (!IsPostBack)
        {
           DataBindClass();
            A_Type.Value = "0";//查询类别（0,全部 1,销量 2,最新 3,促销）
            if (!string.IsNullOrEmpty(GetTxtcontent))
            {
                HttpCookie SelectGoods = Request.Cookies["SelectGoods"];
                if (SelectGoods != null)
                {
                    SelectGoods.HttpOnly = true;
                    string str = HttpUtility.UrlDecode(SelectGoods.Value, Encoding.GetEncoding("UTF-8"));
                  
                        str= str.Replace(GetTxtcontent + "&", "");
                   
                    str += GetTxtcontent + "&";
                        SelectGoods.Value = HttpUtility.UrlEncode(str, Encoding.GetEncoding("UTF-8"));
                        SelectGoods.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(SelectGoods);
                    
                }
                else
                {

                     SelectGoods = new HttpCookie("SelectGoods");
                    SelectGoods.HttpOnly = true;
                    string str = ""; str += GetTxtcontent + "&";
                    SelectGoods.Value = HttpUtility.UrlEncode(str, Encoding.GetEncoding("UTF-8"));
                    SelectGoods.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(SelectGoods);
                }

                Hi.Model.SYS_Select select = new Hi.Model.SYS_Select();
                select.SelectNamestring = GetTxtcontent;
                select.Type = 2;
                LoginModel logUser = Session["UserModel"] as LoginModel;
                if (logUser != null)
                    select.UserID = logUser.UserID;
                else
                    select.UserID = 0;
                select.CreateDate = DateTime.Now; ;
                select.ts = DateTime.Now;
                select.dr = 0;
                select.modifyuser = select.UserID;
                new Hi.BLL.SYS_Select().Add(select);
               
            }
            else
            { GetTxtcontent = "请输入商品名称"; }
            BindData();
           

            //操作日志统计开始
            Utils.WritePageLog(Request, "优质货源");
            //操作日志统计结束

            //  A_Nes.Attributes.Add("class", "hover");
        }
        this.TopNav1.getSelectType = "商品<i class=\"down-i\"></i><div class=\"cur\"><i>店铺</i></div>";
        this.TopNav1.getSelectValue = GetTxtcontent;
    }

    /// <summary>
    /// 分页 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PagerList_PageChanged(object sender, EventArgs e)
    {
        GoodsName = Common.NoHTML(Request["Select_Text"]);
        AddName = Common.NoHTML(AddName_.Value);
        page = Pager_List.CurrentPageIndex.ToString();
        BindData();
    }

    /// <summary>
    /// 获取要查询的SQL
    /// </summary>
    /// <returns></returns>
    public void Where()
    {

        where = "  and isnull(Pic2,'') not in('','X') and BD_Goods.dr=0 and IsOffline=1 ";
        if (!string.IsNullOrWhiteSpace(AddName))//地区搜索条件
        {
            AddNameWeb = Common.NoHTML(AddName);
            GoodsNameWeb = Common.NoHTML(GoodsName);
            where += " and bc.Address like '%" + Common.NoHTML(AddName) + "%' ";
        }
        if (GoodsName != "请输入商品名称"&&!string.IsNullOrWhiteSpace(GoodsName))//名称搜索条件
        {
            GoodsNameWeb = Common.NoHTML(GoodsName);
            where += " and BD_Goods.GoodsName like '%" + Common.NoHTML(GoodsName) + "%' ";
        }
        if (!string.IsNullOrEmpty(Request["GoodsName"]))//顶部名称搜索
        {
            string[] namelist = Request["GoodsName"].Split(' ');
            TitleDiv.InnerHtml = InnerHtmlWhere();
            foreach (var item in namelist)
            {
             where += "  and  BD_Goods.GoodsName like '%" + item + "%' ";
            }
           
        }
        if (CategoryIDUrl!="0" && CategoryID3List!="")//行业种类搜索条件
        {
            DataBindClass();
            where += string.Format(" and gc.ID in({0})", CategoryID3List);

        }
        if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
        {
            if (Request.QueryString["Type"] == "1")
            {
                where += "  and BD_Goods.IsRecommended>=1 '";
            }
        }
    }
    /// <summary>
    /// 拼接完整的分页查询语句
    /// </summary>
    /// <param name="page">当前页码数</param>
    /// <param name="pagecount">一页显示的行数</param>
    /// <param name="fldSort">排序 字段</param>
    /// <param name="tablename">表名称（可以是子查询表）</param>
    /// <param name="fldName">查询的表字段</param>
    /// <returns></returns>
    public string Sql(int page, int pagecount, string fldSort, string tablename)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("select * from (select ROW_NUMBER() over( order by  {0}) rwo,* from ", fldSort);
        sb.AppendFormat(" {0} )b", tablename);
        sb.AppendFormat(" where b.rwo between ({0}-1)*{1}+1 and {2}*{3}", page, pagecount, page, pagecount);
        return sb.ToString();
    }
    /// <summary>
    /// 绑定   
    /// </summary>
    /// <param name="type">商品状态 1销量 2最新 3 促销 0是全部</param>
    public void BindData()
    {
        Where();
        int Counts = 0;//数据源的数据行数

        string SqlQueryCount = string.Empty;
        //A_Type.Value(1,销量   2，最新   3，促销   0，综合排序)
        if (A_Type.Value == "0" || A_Type.Value == "2")
        {

            SqlQuery = @"(select gc.ID IndID, BD_Goods.ID,BD_Goods.GoodsName,BD_Goods.CompID,Pic2,BD_Goods.CreateDate,bc.CompName,bc.ManageInfo,bc.CompAddr,bc.Principal,bc.Phone,bc.Address
                from  BD_Goods join BD_Company bc 
				on BD_Goods.CompID=bc.ID and bc.dr=0 and bc.AuditState=2 and bc.IsEnabled=1 and FirstShow in(1,2)
                join SYS_GType gc on BD_Goods.CategoryID=gc.id
                where  1=1  " + where+")";
            SqlQueryCount = @"(select 1
                from  BD_Goods join BD_Company bc 
				on BD_Goods.CompID=bc.ID and bc.dr=0 and bc.AuditState=2 and bc.IsEnabled=1 and FirstShow in(1,2)
                join SYS_GType gc on BD_Goods.CategoryID=gc.id
                where  1=1  " + where + ")";
        }
        else if (A_Type.Value == "1")
        {
            SqlQuery = @" (select  gc.ID IndID,gb.num, BD_Goods.ID,BD_Goods.GoodsName,BD_Goods.CompID,Pic2,BD_Goods.CreateDate,bc.CompName,bc.ManageInfo,bc.CompAddr,bc.Principal,bc.Phone,bc.Address
                from  BD_Goods  join BD_Company bc 
				on BD_Goods.CompID=bc.ID and bc.dr=0 and bc.AuditState=2 and bc.IsEnabled=1 and FirstShow in(1,2)
                join SYS_GType gc on BD_Goods.CategoryID=gc.id
			    left join (select GoodsID, sum(GoodsNum) num
				from BD_GoodsInfo info join  DIS_OrderDetail od 
				on od.GoodsinfoID=info.ID where od.dr=0   group by GoodsID,od.dr) gb 
				on gb.GoodsID=BD_Goods.ID   where 1=1   
				  " + where + ")";
            SqlQueryCount = @" (select  1
                from  BD_Goods  join BD_Company bc 
				on BD_Goods.CompID=bc.ID and bc.dr=0 and bc.AuditState=2 and bc.IsEnabled=1 and FirstShow in(1,2)
                join SYS_GType gc on BD_Goods.CategoryID=gc.id
			    left join (select GoodsID, sum(GoodsNum) num
				from BD_GoodsInfo info join  DIS_OrderDetail od 
				on od.GoodsinfoID=info.ID where od.dr=0   group by GoodsID,od.dr) gb 
				on gb.GoodsID=BD_Goods.ID   where 1=1   
				  " + where + ")";
            fldSort = "num desc";
        }
        else if (A_Type.Value == "3")
        {
            SqlQuery = @" (select bp.CreateDate cdate,gc.ID IndID,BD_Goods.ID,BD_Goods.GoodsName,BD_Goods.CompID,Pic2,BD_Goods.CreateDate,
          bc.CompName,bc.ManageInfo,bc.CompAddr,bc.Principal,bc.Phone,bc.Address
          from  BD_Goods  join BD_Company bc 
	      on BD_Goods.CompID=bc.ID and bc.dr=0 and bc.AuditState=2 and bc.IsEnabled=1 and FirstShow in(1,2)
           join SYS_GType gc on BD_Goods.CategoryID=gc.id
          left join (select bp.createdate ,  bpd.goodsid ,ROW_NUMBER()
		   over(PARTITION BY bpd.goodsid order by bp.createdate desc) rowid  
           from  BD_PromotionDetail bpd   join BD_Promotion bp on bp.id=bpd.ProID where  
		   GETDATE() between bp.ProStartTime and bp.ProEndTime and bp.dr=0 and bp.IsEnabled=1  ) bp
		    on bp.GoodsID=bd_goods.id and rowid=1 where 1=1 " + where + " )";
            SqlQueryCount = @" (select 1
          from  BD_Goods  join BD_Company bc 
	      on BD_Goods.CompID=bc.ID and bc.dr=0 and bc.AuditState=2 and bc.IsEnabled=1 and FirstShow in(1,2)
           join BD_GoodsCategory gc on BD_Goods.CategoryID=gc.id
          left join (select bp.createdate ,  bpd.goodsid ,ROW_NUMBER()
		   over(PARTITION BY bpd.goodsid order by bp.createdate desc) rowid  
           from  BD_PromotionDetail bpd   join BD_Promotion bp on bp.id=bpd.ProID where  
		   GETDATE() between bp.ProStartTime and bp.ProEndTime and bp.dr=0 and bp.IsEnabled=1  ) bp
		    on bp.GoodsID=bd_goods.id and rowid=1 where 1=1 " + where + " )";
            fldSort = " cdate desc ";
        }
        DataTable ListGoods = null;
        string sql = Sql(Pager_List.CurrentPageIndex, Pager_List.PageSize, fldSort, SqlQuery + " s ");
        ListGoods = new Hi.BLL.BD_Goods().GoodsAttr(sql);//分页的数据源
        //查询所有条数
        if (CategoryID_type.Value != CategoryIDUrl || A_Type.Value == "2" || A_Type.Value == "1" || A_Type.Value == "0"|| A_Type.Value == "3")//如果状态未发生变化则不更新分页总数
        {
            CategoryID_type.Value = CategoryIDUrl;
            DataTable dt = new Hi.BLL.BD_Goods().GoodsAttr(SqlQueryCount);
            Counts = dt.Rows.Count;
        }
        Rpt_Goods.DataSource = ListGoods;
        Rpt_Goods.DataBind();
        Pager_List.RecordCount = Counts;
        //Pager_List.TextBeforePageIndexBox = "<i class='tf2'>共" + Pager_List.PageCount + "页</i> <span class='tf2'>到第:</span>";
        page = Pager_List.CurrentPageIndex.ToString();
        Count_goods.InnerHtml = Counts.ToString();
        Page_index.InnerHtml = Pager_List.CurrentPageIndex.ToString();
        Page_Count.InnerHtml = Pager_List.PageCount.ToString();

        //添加导航栏


    }
    /// <summary>
    /// 绑定前台导航
    /// </summary>
    public void TitleDivDataBind()
    {
        if (CategoryIDUrl!="0")
        {
            int categoryID = Convert.ToInt32(CategoryIDUrl);
            Hi.Model.SYS_GType Lindustry = new Hi.BLL.SYS_GType().GetModel(categoryID);
            TitleDiv.InnerHtml = "<i class='bt'>全部结果</i>>";
            TitleDiv.InnerHtml += "<i class='cdn'>" + Lindustry.TypeName + "<a href='javascript:;' class='close'></a></i>";
        }
    }
    public void DataBindClass()
    {
        CategoryID3List = "";
        Gtypediv2.Visible = false;
        Gtypediv3.Visible = false;

        List<Hi.Model.SYS_GType> GtypeModel = new Hi.BLL.SYS_GType().GetList("Deep,ID,ParentId,TypeCode,TypeName,FullCode", " dr='0' and IsEnabled=1 ", "ID asc");

        //绑定分类
        if (CategoryIDUrl!="0")//行业类别ID 不为空
        {
            Hi.Model.SYS_GType model = GtypeModel.Where(b => b.ID == Convert.ToInt32(CategoryIDUrl)).ToList()[0]; /*new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(Request.QueryString["CategoryID"]));*/
            if (model.Deep == 1)//一级分类
            {
                Gtypediv2.Visible = true;//展示二级分类
                GtypeName2.InnerHtml = "<i>" + model.TypeName + "</i>";
                CategoryID = model.ID.ToString();//保存一级分类ID
                List<Hi.Model.SYS_GType> Lindustry3 = GtypeModel.Where(b => b.Deep ==3 && b.FullCode.Contains(model.TypeCode+ "-")).ToList();
                foreach (var item in Lindustry3)
                {
                    CategoryID3List += item.ID + ",";
                }
                if (CategoryID3List != "" && CategoryID3List.Length > 0)
                    CategoryID3List = CategoryID3List.Substring(0, CategoryID3List.Length - 1);
            }
            else if (model.Deep == 2)//二级分类
            {
                Gtypediv2.Visible = true;//展示二级分类
                Gtypediv3.Visible = true;//展示三级分类
                CategoryID = model.ParentId.ToString();//保存一级分类ID
                CategoryID2 = model.ID.ToString();//保存二级分类ID
                Hi.Model.SYS_GType model2 = GtypeModel.Where(b => b.ID == Convert.ToInt32(CategoryID)).ToList()[0]; /*new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(CategoryID));*/
                GtypeName2.InnerHtml = "<i>" + model2.TypeName + "</i>";
                GtypeName3.InnerHtml = "<i>" + model.TypeName + "</i>";

                //绑定三级
                List<Hi.Model.SYS_GType> Lindustry3 = GtypeModel.Where(b => b.ParentId == Convert.ToInt32(CategoryID2)).ToList();
                //查询分类下所有三级ID
                foreach (var item in Lindustry3)
                {
                    CategoryID3List += item.ID + ",";
                }
                if (CategoryID3List.Length > 1)
                    CategoryID3List = CategoryID3List.Substring(0, CategoryID3List.Length - 1);
            }
            else if (model.Deep == 3)//三级分类
            {
                Gtypediv2.Visible = true;//展示二级分类
                Gtypediv3.Visible = true;//展示三级分类
                string TypeCode1 = model.FullCode.Split('-')[0];//保存一级分类ID
                List<Hi.Model.SYS_GType> gTypes1  = GtypeModel.Where(T => T.TypeCode == TypeCode1).ToList();
                if (gTypes1.Count > 0)
                {
                    CategoryID = gTypes1[0].ID.ToString();
                }
                CategoryID2 = model.ParentId.ToString();//保存二级分类ID
                CategoryID3 = model.ID.ToString();//保存二级分类ID
                Hi.Model.SYS_GType model2 = GtypeModel.Where(b => b.ID == Convert.ToInt32(CategoryID2)).ToList()[0]; /*new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(CategoryID2));*/
                Hi.Model.SYS_GType model1 = GtypeModel.Where(b => b.ID == Convert.ToInt32(CategoryID)).ToList()[0]; /*new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(CategoryID));*/
                GtypeName2.InnerHtml = "<i>" + model1.TypeName + "</i>";
                GtypeName3.InnerHtml = "<i>" + model2.TypeName + "</i>";
                CategoryID3List = model.ID.ToString();
            }
            List<Hi.Model.SYS_GType> Lindustry2 = GtypeModel.Where(b => b.ParentId == Convert.ToInt32(CategoryID)).ToList();
            this.Gtype2.DataSource = Lindustry2;
            this.Gtype2.DataBind();
            if (CategoryID2 != "0")
            {
                //绑定三级
                List<Hi.Model.SYS_GType> Lindustry3 =GtypeModel.Where(b => b.ParentId == Convert.ToInt32(CategoryID2)).ToList();
                this.Gtype3.DataSource = Lindustry3;
                this.Gtype3.DataBind();
            }
        }

        //绑定一级分类
        List<Hi.Model.SYS_GType> Lindustry = GtypeModel.Where(b => b.ParentId == Convert.ToInt32(0)).ToList();
        // new Hi.BLL.SYS_GType().GetList("ID,TypeName", " dr='0' and IsEnabled=1 and ParentId=0  ", "ID asc");
        this.RepList.DataSource = Lindustry;
        this.RepList.DataBind();
    }

    protected string ShowComImg(string ShopLogo, string CompLogo, string CompName, string ShortName)
    {
        string retunconct = "";
        int compnamelenght = CompName.Length;
        string shortname = compnamelenght > 10 ? CompName.ToString().Substring(0, 10) : CompName.ToString();
        if (!string.IsNullOrEmpty(ShopLogo))//新增logo
        {
            return "<img src='" + ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + ShopLogo + "' width=\"140\"  title='" + CompName + "' alt='" + CompName + "' />";
        }
        else if (!string.IsNullOrEmpty(CompLogo))
        {
            return "<img src='" + ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + CompLogo + "' width=\"140\"  title='" + CompName + "' alt='" + CompName + "' />";
        }
        else //空值，默认logo
        {
            retunconct = @"<div  style='margin:5px 0 0 5px; color:#047dc6;text-align:center; font-size:22px;padding:0px 5px ; vertical-align: middle;  display: table-cell;  width: 175px;'>"
                + ShortName + "</div>";
            return retunconct;
        }
    }

    /// <summary>
    /// 销量
    /// </summary>
    protected void A_ShopCount_ServerClick(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["CategoryID"]))
            CategoryIDUrl = Request.QueryString["CategoryID"];
        else
            CategoryIDUrl = GtypeHid.Value;
        Pager_List.CurrentPageIndex = 1;
        TitleDiv.InnerHtml = "<i class='bt'>全部结果</i>>";
        A_Type.Value = "1";
        BindData();
        A_cshop.Attributes.Remove("class");
        A_Nes.Attributes.Remove("class");
        A_ShopCount.Attributes.Add("class", "hover");
    }

    /// <summary>
    ///  最新 
    /// </summary>
    protected void A_Nes_ServerClick(object sender, EventArgs e)
    {
        Pager_List.CurrentPageIndex = 1;
        TitleDiv.InnerHtml = "<i class='bt'>全部结果</i>>";
        A_Type.Value = "2";
        fldSort = "CreateDate desc";
        BindData();
        A_cshop.Attributes.Remove("class");
        A_Nes.Attributes.Add("class", "hover");
        A_ShopCount.Attributes.Remove("class");
    }

    /// <summary>
    /// 促销
    /// </summary>
    protected void A_cshop_ServerClick(object sender, EventArgs e)
    {
        Pager_List.CurrentPageIndex = 1;
        TitleDiv.InnerHtml = "<i class='bt'>全部结果</i>>";
        A_Type.Value = "3";
        BindData();
        A_cshop.Attributes.Add("class", "hover");
        A_Nes.Attributes.Remove("class");
        A_ShopCount.Attributes.Remove("class");
    }

    /// <summary>
    ///绑定Title
    /// </summary>
    /// <returns></returns>
    public string GetTiTle()
    {
        if (!string.IsNullOrWhiteSpace(Request["Hot"]))
        {
            //优质货源
            keywords = "优质货源,供应货源,货源供应,货源批发市场,货源批发";
            description = "医站通-B2B电子商务平台,是为入驻b2b企业提供商品分销,批发及采购,帮助企业建立在线下单,在线支付,商品库存查询,物流信息查询等全面高效的订单管理流程.分销、批发就上医站通,40077-40088.";
            return "优质货源丨优质商品批发_优质商品供应_商品在线交易_医站通";
        }
        int CategoryIDs = Convert.ToInt32(CategoryIDUrl);
        switch (CategoryIDs)
        {
            case 0:
                return "医站通-B2B电子商务平台，分销、批发就上医站通";
            case 10:
                return "食品酒水-名酒茗茶、粮油调味、休闲食品、饮料冲印、生鲜-医站通";
            case 12:
                return "礼品园艺-工艺礼品、日用百货、办公文教、家纺-医站通";
            case 11:
                return "家居家装-家具、灯具、厨卫、家装材料-医站通";
            case 13:
                return "化工建材-橡塑制品、化工原料、钢材、冶金矿产-医站通";
            case 14:
                return "机械五金-机械、五金、工具、仪表-医站通";
            case 15:
                return "电工安防-照明、电工、个人防护、监控、消防-医站通";
            case 16:
                return "纺织皮革-面料、辅料、纱线、坯布、皮革-医站通";
            case 17:
                return "服饰配饰-服饰、鞋包、珠宝首饰、钟表眼镜-医站通";
            case 18:
                return "电器电子-智能设备、电子教育、集成电路-医站通";
            case 19:
                return "汽车用品-车载设备、维修保养、汽车装饰-医站通";
            case 20:
                return "个护化妆-美容护肤、个护健康、美发造型-医站通";
            case 21:
                return "母婴玩具-奶粉、童车童床、营养辅食、妈妈专区、玩具、母婴用品-医站通";
            case 22:
                return "运动户外-户外装备、健身训练、体育用品、保健器械、骑行、渔具-医站通";
            case 23:
                return "营养保健-传统滋补、急救卫生、营养健康、成人用品-医站通";
            default:
                return "优质货源-综合、销量、最新、促销-医站通";
        }
    }

    //二级搜索按钮
    protected void Select_ServerClick(object sender, EventArgs e)
    {
        Pager_List.CurrentPageIndex = 1;
        GoodsName = Request["Select_Text"];
        AddName = AddName_.Value;
        string InnerHtmls = InnerHtmlWhere();
        TitleDiv.InnerHtml = InnerHtmls;
        BindData();
    }

   /// <summary>
   /// 省市条件搜索
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void Unnamed_ServerClick(object sender, EventArgs e)
    {
        Pager_List.CurrentPageIndex = 1;
        AddName = AddName_.Value;
        GoodsName = Request["Select_Text"];
        string InnerHtmls = InnerHtmlWhere();
        TitleDiv.InnerHtml = InnerHtmls;
        BindData();
    }

    /// <summary>
    /// 头部条件导航栏 条件取消事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Titlt_close_ServerClick(object sender, EventArgs e)
    {
        GoodsName = GoodsName_.Value;
        AddName = AddName_.Value;
        //Request.QueryString["CategoryID"] = CategoryID_.Value;
        string InnerHtmls = "<i class='bt'>全部结果</i>>";
        if (!string.IsNullOrWhiteSpace(GoodsName))
        {
            InnerHtmls += "<i class='cdn'>" + GoodsName + "<a href='javascript:;' onclick='GoodName_title()' class='close'></a></i>";
        }
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            InnerHtmls += "<i class='cdn'>" + AddName + "<a href='javascript:;' onclick='Province_title()'  class='close'></a></i>";
        }
        Pager_List.CurrentPageIndex = 1;
        TitleDiv.InnerHtml = InnerHtmls;
        BindData();
    }

    //头部导航搜索条件
    public string InnerHtmlWhere()
    {
        string InnerHtmls = "<i class='bt'>全部结果</i>>";
        if (!string.IsNullOrWhiteSpace(GoodsName)&& GoodsName!="请输入商品名称")
        {
            InnerHtmls += "<i class='cdn'>" + GoodsName + "<a href='javascript:;' onclick='GoodName_title()' class='close'></a></i>";
        }
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            InnerHtmls += "<i class='cdn'>" + AddName + "<a href='javascript:;' onclick='Province_title()'  class='close'></a></i>";
        }
        if (!string.IsNullOrWhiteSpace(Request["GoodsName"]))
        {
            InnerHtmls += "<i class='cdn'>" + Request["GoodsName"] + "<a href='javascript:;' id='Top_Select' class='close'></a></i>";
        }
        return InnerHtmls;
    }

    //截取经营范围长度
    public string ManageInfoSubstring(string ManageInfo)
    {
        if (ManageInfo.Length < 31)
        {
            return ManageInfo;
        }
        else
        {
            return ManageInfo.Substring(0, 31) + "...";
        }
    }

    /// <summary>
    /// 综合排序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void A_sort_ServerClick(object sender, EventArgs e)
    {
        Pager_List.CurrentPageIndex = 1;
        fldSort = " CreateDate ";
        A_Type.Value = "0";
        A_sort.Attributes.Add("class", "hover");
        A_cshop.Attributes.Remove("class");
        A_Nes.Attributes.Remove("class");
        A_ShopCount.Attributes.Remove("class");
        BindData();
        TitleDiv.InnerHtml = "<i class='bt'>全部结果</i>>";
    }


    /// <summary>
    /// 行业分类单击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GtypeBtn_Click(object sender, EventArgs e)
    {
        CategoryIDUrl = this.GtypeHid.Value;
        DataBindClass();
        BindData();
    }
}