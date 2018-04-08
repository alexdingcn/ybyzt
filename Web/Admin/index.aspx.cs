using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Admin_index : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                Hi.Model.SYS_AdminUser model = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser;
                //判断是否存在装修审核权限,不加业务员ID查询
                string sql = "select rf.* from SYS_RoleSysFun rf join SYS_AdminUser u on u.RoleID=rf.RoleID where rf.FunCode='3215' and u.ID=" + UserID;
                DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0 )
                {
                    Binds();
                }
                else if (model.UserType == 1)
                {
                    Binds();
                }
                else
                    ulComp.Visible = false;
            }
            catch (Exception ex)
            {
                Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            }
        }
    }

    private void Binds()
    {
        //待审的企业
        List<Hi.Model.BD_Company> l = new Hi.BLL.BD_Company().GetList("id", "isnull(dr,0)=0 and orgid=" + OrgID + " and isenabled=1 and Auditstate<>2" + (this.OrgID == 0 ? "" : " and isnull(orgid,0)=" + this.OrgID), "");
        this.lblCompcount.InnerText = l.Count.ToString();
        //待处理留言
        List<Hi.Model.SYS_UserMessage> l2 = new Hi.BLL.SYS_UserMessage().GetList("id", "isnull(dr,0)=0 and State=0", "");
        this.lblMessage.InnerText = l2.Count.ToString();
        //前台显示的企业
        List<Hi.Model.BD_Company> ll = new Hi.BLL.BD_Company().GetList("id", "Auditstate=2 and orgid=" + OrgID + "  and isenabled=1 and ISNULL(dr,0)=0 and FirstShow=1" + (this.OrgID == 0 ? "" : " and isnull(orgid,0)=" + this.OrgID), "");
        this.lblShowcount.InnerText = ll.Count.ToString();
        //机构
        if (OrgID == 0)
        {

        }
        List<Hi.Model.BD_Org> lll = new Hi.BLL.BD_Org().GetList("id", "isenabled=1 and ISNULL(dr,0)=0", "");
        this.lblOrgcount.InnerText = lll.Count.ToString();
        NewsBind();// 新闻公告
        Bind();//统计

    }

    /// <summary>
    /// 新闻公告
    /// </summary>
    /// <returns></returns>
    public void NewsBind()
    {
        List<Hi.Model.SYS_NewsNotice> l = new Hi.BLL.SYS_NewsNotice().GetList("top 4 *", "isnull(dr,0)=0 and isenabled=1 ", "CreateDate desc,IsTop desc");
        rptNews.DataSource = l;
        rptNews.DataBind();
    }
    /// <summary>
    /// 类型
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetType(string id)
    {
        if (id == "1")
        {
            return "新闻";
        }
        else if (id == "2")
        {
            return "公告";
        }
        else if (id == "3")
        {
            return "资讯";
        }
        else
        {
            return "其他";
        }
    }
    /// <summary>
    /// 统计
    /// </summary>
    public void Bind()
    {
        string str = string.Empty;//compid 
        if (this.OrgID != 0)
        {
            List<Hi.Model.BD_Company> l = new Hi.BLL.BD_Company().GetList("ID", "ISNULL(dr,0)=0 and IsEnabled=1  and Auditstate=2 and isnull(orgid,0)=" + this.OrgID, "");
            if (l.Count > 0)
            {
                str = string.Join(",", l.Select(T => T.ID));
                if (!Util.IsEmpty(str))
                {
                    str = str.Substring(0, str.Length - 1);
                    str = " and compid in(" + str + ")";
                }
            }
        }
        //企业统计
        Bind2(this.rptComplist, "BD_Company comp", "comp", "");
        //代理商统计
        Bind2(this.rptDisList, "BD_Distributor dis left join BD_Company comp on dis.CompID=comp.ID", "dis", str);
        //订单统计
        Bind3(this.rptOrderList, str);
        //收款统计
        Bind4(this.rptPriceList, str);
    }
    /// <summary>
    ///      企业统计\代理商统计
    /// </summary>
    /// <returns></returns>
    public void Bind2(Repeater rpt, string table, string type,string str)
    {
        string sum = "sumcount1";
        if(type== "dis")
            sum = "sumcount2";
        string sql = string.Empty;
        if (!Util.IsEmpty(str))
        {
            sql = string.Format(@"SELECT count(*) as {3}  FROM {0} WHERE datediff(day,{2}.CreateDate,getdate())=0 and ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}
union all
SELECT count(*) as {3} FROM {0} WHERE datediff(week,{2}.CreateDate,getdate())=0 and ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}
union all
SELECT count(*) as {3} FROM {0} WHERE DATEDIFF(month,{2}.CreateDate,GETDATE())=0 and ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}
union all
SELECT count(*) as {3} FROM {0} WHERE DATEDIFF(YYYY,{2}.CreateDate,GETDATE())=0 and ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}
union all
SELECT count(*) as {3} FROM {0} where   ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}", table, str, type,sum);
        }
        else
        {
            sql = string.Format(@"SELECT count(*) as {3}  FROM {0} WHERE datediff(day,{2}.CreateDate,getdate())=0 and ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}
union all
SELECT count(*) as {3} FROM {0} WHERE datediff(week,{2}.CreateDate,getdate())=0 and ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}
union all
SELECT count(*) as {3} FROM {0} WHERE DATEDIFF(month,{2}.CreateDate,GETDATE())=0 and ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}
union all
SELECT count(*) as {3} FROM {0} WHERE DATEDIFF(YYYY,{2}.CreateDate,GETDATE())=0 and ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}
union all
SELECT count(*) as {3} FROM {0} where   ISNULL({2}.dr,0)=0 and {2}.IsEnabled=1  and {2}.Auditstate=2  {1}", table, this.OrgID == 0 ? "" : " and isnull(orgid,0)=" + this.OrgID, type,sum);
        }
        DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        rpt.DataSource = ds;
        rpt.DataBind();
    }
    /// <summary>
    /// 订单统计
    /// </summary>
    /// <param name="rpt"></param>
    /// <param name="table"></param>
    public void Bind3(Repeater rpt, string str)
    {
        string sql = string.Format(@"SELECT count(*) as sumcount3 FROM DIS_Order WHERE datediff(day,CreateDate,getdate())=0 and OState in(1,2,4,5) and Otype!=9 and isnull(dr,0)=0 {0}
union all
SELECT count(*) as sumcount3 FROM DIS_Order WHERE datediff(week,CreateDate,getdate())=0 and OState in(1,2,4,5) and Otype!=9 and isnull(dr,0)=0 {0}
union all
SELECT count(*) as sumcount3 FROM DIS_Order WHERE DATEDIFF(month,CreateDate,GETDATE())=0 and OState in(1,2,4,5) and Otype!=9 and isnull(dr,0)=0 {0}
union all
SELECT count(*) as sumcount3 FROM DIS_Order WHERE DATEDIFF(YYYY,CreateDate,GETDATE())=0 and OState in(1,2,4,5) and Otype!=9 and isnull(dr,0)=0 {0}
union all
SELECT count(*) as sumcount3 FROM DIS_Order where   OState in(2,4,5) and Otype!=9 and isnull(dr,0)=0 {0}", str);
        DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        rpt.DataSource = ds;
        rpt.DataBind();
    }
    /// <summary>
    /// shoukuan
    /// </summary>
    /// <param name="rpt"></param>
    public void Bind4(Repeater rpt, string str)
    {
        string sql = string.Format(@"
SELECT isNUll(sum(PayedAmount),0) as sumcount4 FROM DIS_Order WHERE datediff(day,CreateDate,getdate())=0 and OState in(2,4,5) and Otype!=9 and PayState in(1,2) and ReturnState=0 and ISNULL(dr,0)=0  {0}
union all
SELECT isNUll(sum(PayedAmount),0) as sumcount4 FROM DIS_Order WHERE datediff(week,CreateDate,getdate())=0 and OState in(2,4,5) and Otype!=9 and PayState in(1,2) and ReturnState=0 and ISNULL(dr,0)=0 {0}
union all
SELECT isNUll(sum(PayedAmount),0) as sumcount4 FROM DIS_Order WHERE DATEDIFF(month,CreateDate,GETDATE())=0 and OState in(2,4,5) and Otype!=9 and PayState in(1,2) and ReturnState=0 and ISNULL(dr,0)=0 {0}
union all
SELECT isNUll(sum(PayedAmount),0) as sumcount4 FROM DIS_Order WHERE DATEDIFF(YYYY,CreateDate,GETDATE())=0 and OState in(2,4,5) and Otype!=9 and PayState in(1,2) and ReturnState=0 and ISNULL(dr,0)=0 {0}
union all
SELECT isNUll(sum(PayedAmount),0) as sumcount4 FROM DIS_Order where   OState in(2,4,5) and PayState in(1,2) and ReturnState=0 and Otype!=9 and ISNULL(dr,0)=0 {0}", str);
        DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        rpt.DataSource = ds;
        rpt.DataBind();
    }
}