using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Text;

public partial class ComList : LoginPageBase
{
    public static string GetTxtcontent = string.Empty;
    public string GoodsName = "";//二级搜索名称
    public string GoodsNameWeb = "请输入店铺名称";//二级搜索名称前台显示
    public string AddName = "";//所在地区查询
    public string AddNameWeb = "请选择";//所在地区查询前台显示
    public string CategoryID = "";//行业类别
    public string SortName = " ID desc,coun desc  ";//排序方式
    public string SqlQuery = " ";//查询语句
    public string Sqlwhere = "";//查询条件
    public string types = "0";//记录行业种类
    public string page = string.Empty;//页码数
    public string indid = "0";
    public bool islogUser = false;  //是否登录
    public string keywords = "B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统";
    public string description = "医站通-B2B电子商务平台,是为入驻b2b企业提供商品分销,批发及采购,帮助企业建立在线下单,在线支付,商品库存查询,物流信息查询等全面高效的订单管理流程.分销、批发就上医站通,40077-40088.";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["indid"]))
            indid = Request["indid"];
        if (!IsPostBack)
        {
            LoginModel logUser = Session["UserModel"] as LoginModel;

            if (!string.IsNullOrEmpty(Request.QueryString["compname"]))
            {
                GetTxtcontent = Common.NoHTML(Request.QueryString["compname"]);
                HttpCookie SelectComp = Request.Cookies["SelectComp"];
                if (SelectComp != null)
                {
                    SelectComp.HttpOnly = true;
                    string str = HttpUtility.UrlDecode(SelectComp.Value, Encoding.GetEncoding("UTF-8"));

                    str = str.Replace(GetTxtcontent + "&", "");

                    str += GetTxtcontent + "&";
                    SelectComp.Value = HttpUtility.UrlEncode(str, Encoding.GetEncoding("UTF-8"));
                    SelectComp.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(SelectComp);

                }
                else
                {

                    SelectComp = new HttpCookie("SelectComp");
                    SelectComp.HttpOnly = true;
                    string str = ""; str += GetTxtcontent + "&";
                    SelectComp.Value = HttpUtility.UrlEncode(str, Encoding.GetEncoding("UTF-8"));
                    SelectComp.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(SelectComp);
                }


                Hi.Model.SYS_Select select = new Hi.Model.SYS_Select();
                select.SelectNamestring = GetTxtcontent;
                select.Type = 1;

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
            { GetTxtcontent = "请输入厂商名称"; }

            if (logUser != null)
            {
                islogUser = true;
            }

            BindData();
            DataBindClass();
        }
    }
  
    /// <summary>
    /// 分页控件事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PagerList_PageChanged(object sender, EventArgs e)
    {
        if (A_Type.Value == "2")//分页时候判断  如果是最新  则加上最新的搜索条件
        {
            Sqlwhere = " and FirstShow in (1,2) ";
        }
        else if (A_Type.Value=="1")
        {
            SortName = " FirstShow,SortIndex desc ";
        }
        AddName =Common.NoHTML( AddName_.Value);
        GoodsName =Common.NoHTML( GoodsName_.Value);
        page = Pager_List.CurrentPageIndex.ToString();
        BindData();
    }
    /// <summary>
    /// 绑定分类
    /// </summary>
    public void DataBindClass()
    {
        
        List<Hi.Model.SYS_GType> Lindustry = new Hi.BLL.SYS_GType().GetList("top 16 *", "dr='0' and IsEnabled=1 and Deep=1 ", "ID asc");
        this.RepList.DataSource = Lindustry;
        this.RepList.DataBind();
    }

    /// <summary>
    /// 绑定数据源
    /// </summary>
    public void BindData()
    {
        int Counts = 0;
        string SqlQueryCount = string.Empty;
        SqlQuery = @"(  select FirstShow,b.coun,SortIndex,CreateDate,BD_Company.ID,CompName,ManageInfo,CompAddr,Principal,Phone,Address,ShopLogo,CompLogo,ShortName
                        from BD_Company   join ( select CompID,COUNT(CompID) coun from BD_Goods where
                        Pic2<>'X' and isnull(Pic2,'')<>'' and BD_Goods.dr=0 and IsOffline=1   group by CompID having COUNT(CompID)>0 )b on b.CompID=BD_Company.ID 
                        where 1=1 " + SearchWhere() + " )";
        SqlQueryCount = @"(  select 1
                        from BD_Company   join ( select CompID,COUNT(CompID) coun from BD_Goods where
                        Pic2<>'X' and isnull(Pic2,'')<>'' and BD_Goods.dr=0 and IsOffline=1   group by CompID having COUNT(CompID)>0 )b on b.CompID=BD_Company.ID 
                        where 1=1 " + SearchWhere() + " )";
        DataTable ListGoods = null;
        string sql = Sql(Pager_List.CurrentPageIndex, Pager_List.PageSize, SortName, SqlQuery + " s ");
        ListGoods = new Hi.BLL.BD_Goods().GoodsAttr(sql);//分页的数据源
        RepComp.DataSource = ListGoods;
        RepComp.DataBind();
        if (CategoryID_type.Value != Request.QueryString["indid"]|| A_Type.Value == "1" || A_Type.Value=="2")//如果状态未发生变化则不更新分页总数
        {
            CategoryID_type.Value =Common.NoHTML(Request.QueryString["indid"]);
            DataTable dt = new Hi.BLL.BD_Goods().GoodsAttr(SqlQueryCount);
            Counts = dt.Rows.Count;
        }
        Pager_List.RecordCount = Counts;
        //Pager_List.TextBeforePageIndexBox = "<i class='tf2'>共" + Pager_List.PageCount + "页</i> <span class='tf2'>到第:</span>";
        page = Pager_List.CurrentPageIndex.ToString();
        Page_index.InnerHtml = Pager_List.CurrentPageIndex.ToString();
        Page_Count.InnerHtml = Pager_List.PageCount.ToString();
        Count_goods.InnerHtml = Counts.ToString();
    }

    public string SearchWhere()
    {
        GoodsNameWeb = "请输入店铺名称";
        AddNameWeb = "请选择";
        //new不为空  或者sqlwhere 初始值不等‘and FirstShow=1’于则最新按钮默认选中 执行最新搜索条件
        if (!string.IsNullOrWhiteSpace(Request["new"]) && SortName != " FirstShow,SortIndex desc ")
        {
            SortName = " ID desc ";
            Sqlwhere = " and FirstShow in (1,2) ";
            A_Type.Value = "2";
            A_sort.Attributes.Remove("class");
            A_Nes.Attributes.Add("class", "hover");
            //操作日志统计开始
            Utils.WritePageLog(Request, "最新入驻");
            //操作日志统计结束
        }
        Sqlwhere += " and BD_Company.dr='0' and BD_Company.AuditState=2 and BD_Company.IsEnabled=1  ";

        //行业种类搜索条件
        if (!string.IsNullOrEmpty(indid) && indid.ToString() != "0")
        {
            Sqlwhere += " and BD_Company.indid=" +Common.NoHTML(indid);
        }
        //根据名称模糊搜索
        if (!string.IsNullOrWhiteSpace(Request["compname"]))
        {
            TitleDiv.InnerHtml = InnerHtmlWhere();
            string likeComp = Request["compname"].Trim();
            Sqlwhere += " and FirstShow in(1,2) ";

            try
            {
                String Str = DBUtility.HttpHelp.HttpGet("http://120.26.6.172/get.php?source=" + likeComp + "&param1=0&param2=1"); //调用分词接口

                String[] arry = Str.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);  //转换成分词数组

                arry = arry.Where(T => T.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].ToDecimal(0) > 0.75m).ToArray(); //只取相似度大于0.75的分词

                arry = arry.Select(T => T.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0]).ToArray(); //去掉相似度字符 取分词字符

                List<String> ListStr = StringDistinct(arry.ToList());//去除相似的分词

                List<string> ListExcept = arry.ToList().Except(ListStr).ToList();  //分词差集

                Sqlwhere += " and ( CompName like '%" + likeComp + "%'   " + (ListStr.Count > 0 ? "or  CompName like '%" + string.Join("%", ListStr) + "%' " : "") + " " + (ListExcept.Count > 0 ? "or CompName like '%" + string.Join("%", ListExcept) + "%' " : "") + " )";

                SortName= " (case when CompName like '%" + likeComp + "%' then 2 when CompName like '%" + string.Join("%", ListStr) + "%' then 1 else 0  end)desc ";
            }
            catch
            {
                Sqlwhere += " and CompName like '%" + likeComp + "%' ";
            }

        }
        if (string.IsNullOrEmpty(Request["compname"]))
        {
            //操作日志统计开始
            Utils.WritePageLog(Request, "首页店铺搜索");
            //操作日志统计结束
        }
        //根据名称模糊搜索
        if (!string.IsNullOrWhiteSpace(GoodsName) && GoodsName != "请输入店铺名称")
        {
            GoodsNameWeb = GoodsName;
            Sqlwhere += " and FirstShow in(1,2) and CompName like '%" +Common.NoHTML( GoodsName) + "%'";
            //操作日志统计开始
            Utils.WritePageLog(Request, "店铺搜索");
            //操作日志统计结束
        }
        //根据地区搜索
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            AddNameWeb =Common.NoHTML( AddName);
            Sqlwhere += " and Address like '%" +Common.NoHTML( AddName) + "%'";
        }
        //精品
        if (!string.IsNullOrWhiteSpace(Request["type"])&& Request["type"]== A_Type.Value||A_Type.Value=="1")
        {
            SortName += SortName == "" ? " FirstShow,SortIndex desc " : " ,FirstShow,SortIndex desc ";
            Sqlwhere += " and FirstShow in (1,2) ";
            //操作日志统计开始
            Utils.WritePageLog(Request, "精品品牌");
            //操作日志统计结束
        }

        return Sqlwhere;
    }



    //去除相似的分词
    public List<string> StringDistinct(List<string> listStr)
    {
        List<string> arrys = new List<string>();
        foreach (string str in listStr)
        {

            bool IsFind = false;
            foreach (string child in arrys)
            {
                if (child.IndexOf(str) >= 0 || str.IndexOf(child) >= 0)
                {
                    IsFind = true;
                    break;
                }
            }
            if (IsFind)
                continue;
            arrys.Add(str);
        }
        return arrys;
    }


    protected string ShowComImg(string ShopLogo, string CompLogo, string CompName, string ShortName)
    {
        string retunconct = "";
        int compnamelenght = CompName.Length;
        string shortname = compnamelenght >10 ? CompName.ToString().Substring(0, 10) : CompName.ToString();
        if (!string.IsNullOrEmpty(ShopLogo))//新增logo
        {
            return "<img src='" + ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + ShopLogo + "' width=\"140\" height=\"75\"  title='" + CompName + "' alt='" + CompName + "' />";
        }
        else if (!string.IsNullOrEmpty(CompLogo))
        {
            return "<img src='" + ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + CompLogo + "' width=\"140\" height=\"75\"  title='" + CompName + "' alt='" + CompName + "' />";
        }
        else //空值，默认logo
        {
            retunconct = @"<div  style='margin:5px 0 0 5px; color:#047dc6;text-align:center; font-size:22px;padding:0px 5px ; vertical-align: middle;  display: table-cell;  width: 175px;'>" + (!string.IsNullOrWhiteSpace(ShortName) ? ShortName : CompName) + "</div>";
            return retunconct;
        }
    }


    public string GetTiTle()
    {
        int indids = Convert.ToInt32(indid);

      
            if (!string.IsNullOrWhiteSpace(Request["type"]))
            {
                //精品品牌
                keywords = "精品品牌,商品批发,品牌商品";
                description = "医站通-B2B电子商务平台,是为入驻b2b企业提供商品分销,批发及采购,帮助企业建立在线下单,在线支付,商品库存查询,物流信息查询等全面高效的订单管理流程.分销、批发就上医站通,40077-40088.";
                return "精品品牌丨品牌商品供应,品牌商品批发采购平台医站通";
            }
            else if (!string.IsNullOrWhiteSpace(Request["new"]))
            {
                //最新入驻
                keywords = "最新入驻商家,最新入驻,最新商品信息,最新入驻企业,医站通入驻";
                description = "医站通-B2B电子商务平台,是为入驻b2b企业提供商品分销,批发及采购,帮助企业建立在线下单,在线支付,商品库存查询,物流信息查询等全面高效的订单管理流程.分销、批发就上医站通,40077-40088.";
                return "最新入驻丨品质商家最新入驻,获取一手最新商品信息医站通";
            }

        switch (indids)
        {
            case 0:
                return "商家搜索-热门推荐、最新入驻-医站通";
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
                return "商家搜索-热门推荐、最新入驻-医站通";
        }

       
    }
    //二级搜索按钮
    protected void Select_ServerClick(object sender, EventArgs e)
    {
        GoodsName =Common.NoHTML( Request["Select_Text"]);
        AddName = Common.NoHTML(AddName_.Value);
        string InnerHtmls = InnerHtmlWhere();
        TitleDiv.InnerHtml = InnerHtmls;
        Pager_List.CurrentPageIndex = 1;
        //BindData();
    }
    //省市条件搜索
    protected void Unnamed_ServerClick(object sender, EventArgs e)
    {
        
        AddName = Common.NoHTML(AddName_.Value);
        GoodsName = Common.NoHTML(GoodsName_.Value);
        string InnerHtmls = InnerHtmlWhere();
        TitleDiv.InnerHtml = InnerHtmls;
        Pager_List.CurrentPageIndex = 1;
        //BindData();
    }

    /// <summary>
    /// 头部条件导航栏 条件取消事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Titlt_close_ServerClick(object sender, EventArgs e)
    {
        GoodsName = Common.NoHTML(GoodsName_.Value);
        AddName = Common.NoHTML(AddName_.Value);
        //Request.QueryString["CategoryID"] = CategoryID_.Value;
        string InnerHtmls = "<i class='bt'>全部结果</i>>";
        if (!string.IsNullOrWhiteSpace(GoodsName) && GoodsName != "请输入店铺名称")
        {
            InnerHtmls += "<i class='cdn'>" + GoodsName + "<a href='javascript:;' onclick='GoodName_title()' class='close'></a></i>";
        }
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            InnerHtmls += "<i class='cdn'>" + AddName + "<a href='javascript:;' onclick='Province_title()'  class='close'></a></i>";
        }
        TitleDiv.InnerHtml = InnerHtmls;
        Pager_List.CurrentPageIndex = 1;
        
        //BindData();
    }

    /// <summary>
    ///  最新 
    /// </summary>
    protected void A_Nes_ServerClick(object sender, EventArgs e)
    {
        
        TitleDiv.InnerHtml = "<i class='bt'>全部结果</i>>";
        A_Type.Value = "2";//表示最新
        SortName = "  ID desc,coun desc ";
        Sqlwhere = " and FirstShow in (1,2) ";
        A_sort.Attributes.Remove("class");
        A_Nes.Attributes.Add("class", "hover");
        Pager_List.CurrentPageIndex = 1;
    }
    //截取经营范围长度
    public string ManageInfoSubstring(string ManageInfo)
    {
        if (ManageInfo.Length < 40)
        {
            return ManageInfo;
        }
        else
        {
            return ManageInfo.Substring(0, 40) + "...";
        }
    }

    /// <summary>
    /// RepComp行绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RepComp_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater GoodsList = e.Item.FindControl("GoodsList") as Repeater;
            DataRowView model = (DataRowView)e.Item.DataItem;
            string where = string.Format(@" Pic2<>'X' and isnull(Pic2,'')<>'' and BD_Goods.dr=0 and IsOffline=1 and CompID={0}", model["ID"].ToString());
            List<Hi.Model.BD_Goods> list = new Hi.BLL.BD_Goods().GetList(" top 4 * ", where, " BD_Goods.isindex desc ,BD_Goods.CreateDate ");   // BD_Goods.isindex desc ,BD_Goods.CreateDate
            //if (list.Count==0)
            //{
            //    Panel LI_panel = e.Item.FindControl("Li_" + model["ID"].ToString()) as Panel;
            //    LI_panel.Visible = false;
            //}
            GoodsList.DataSource = list;
            GoodsList.DataBind();
        }
    }
    //头部导航搜索条件
    public string InnerHtmlWhere()
    {
        string InnerHtmls = "<i class='bt'>全部结果</i>>";
        if (!string.IsNullOrWhiteSpace(GoodsName)&& GoodsName != "请输入店铺名称")
        {
            InnerHtmls += "<i class='cdn'>" + Common.NoHTML(GoodsName) + "<a href='javascript:;' onclick='GoodName_title()' class='close'></a></i>";
        }
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            InnerHtmls += "<i class='cdn'>" + Common.NoHTML(AddName) + "<a href='javascript:;' onclick='Province_title()'  class='close'></a></i>";
        }
        if (!string.IsNullOrWhiteSpace(Request["compname"]))
        {
            InnerHtmls += "<i class='cdn'>" + Common.NoHTML(Request["compname"]) + "<a href='javascript:;' id='Top_Select' class='close'></a></i>";
        }
        return InnerHtmls;
    }
    /// <summary>
    /// 销量 促销 调用分页
    /// </summary>
    /// <param name="page">当前第几页</param>
    /// <param name="pagecount">一页几行</param>
    /// <param name="fldSort">排序方式</param>
    /// <param name="tablename">表名称（可以是子查询）</param>
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
    /// 综合排序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void A_sort_ServerClick(object sender, EventArgs e)
    { 
       
        Sqlwhere = " and FirstShow in(1,2) ";
        SortName = " FirstShow,SortIndex desc ";
        TitleDiv.InnerHtml = "<i class='bt'>全部结果</i>>";
        A_sort.Attributes.Add("class", "hover");
        A_Nes.Attributes.Remove("class");
        A_Type.Value = "1";//表示综合排序
        Pager_List.CurrentPageIndex = 1;
        //BindData();
    }
    /// <summary>
    /// 商品名称截取
    /// </summary>
    /// <param name="GoodsName"></param>
    /// <returns></returns>
    public string GoodsNameSubstring(string GoodsName)
    {
        if (GoodsName.Length < 8)
        {
            return GoodsName;
        }
        else
        {
            return GoodsName.Substring(0, 8) + "...";
        }
    }


    /// <summary>
    /// 行业分类单击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GtypeBtn_Click(object sender, EventArgs e)
    {
        indid = this.GtypeHid.Value;
        DataBindClass();
        BindData();
    }
}