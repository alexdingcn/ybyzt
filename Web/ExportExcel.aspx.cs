using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public partial class ExportExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["intype"] == null)
            {
                //导出商品详细
                DataBind(Request["oID"].ToString());
            }
            else
            {
                string tbName = string.Empty;
                string tb = string.Empty;

                string strwhere = (Request["searchValue"] + "");
                //var ostate = Request["OState"];
                //string strwhereOState = string.Empty;
                //if (ostate != "-2")
                //{
                //    if (ostate.ToInt(0) == 0)
                //        strwhereOState += " and o.OState in (1,2,3,4) ";
                //    else if (ostate.ToInt(0) == 1)
                //        strwhereOState += " and o.OState=5 ";
                //    else
                //        strwhereOState += " and o.OState=6 ";
                //}
                //strwhere += strwhereOState;
                string orderby = string.Empty;
                int PageSize = Request["p"] != null ? Request["p"].ToString().ToInt(0) : 12;
                int CurrentPageIndex = Request["c"] != null ? Request["c"].ToString().ToInt(0) : 1;
                int intype = Request["intype"] != null ? Request["intype"].ToInt(0) : 1;

                string str = string.Empty;

                string strwhat = toExcel(intype, out tb, out tbName, out str, out orderby);
                //DataTable dt = new Hi.BLL.DIS_Order().GetList(strwhat,tbName, "o.ID in(2074,2075)", "");
                string sql = "select " + strwhat + " from " + tbName + " where 1=1 " + str;

                if (!string.IsNullOrEmpty(strwhere))
                    sql += strwhere;

            
                
                sql += orderby;
               
                MyPagination mypag = new MyPagination();
                DataTable dt = mypag.GetDt(CurrentPageIndex, PageSize, sql, sql);

                Bind(dt, tb);
            }
        }
    }

    /// <summary>
    /// 导出订单详细Excel
    /// </summary>
    /// <param name="oID"></param>
    public void DataBind(string oID)
    {
        string ID = string.Empty;
        if (Request.Cookies["oID"] != null)
        {
            ID = Server.UrlDecode(Request.Cookies["oID"].Values.ToString());

            DataTable l = new Hi.BLL.DIS_Order().GetList("o.*,oe.*,disName", "DIS_Order o left join Dis_OrderExt oe on o.id=oe.OrderID left join BD_Distributor dis on o.DisID=dis.id", " o.ID in(" + ID + ")", "");

            DataTable ll = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", "o.OrderID in(" + ID + ")");

            DerivedExcel.ExportEasy("销售订单", l, ll, "销售订单");

            HttpCookie c = new HttpCookie("oID");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);
        }
    }

    public void Bind(DataTable dt, string tb)
    {
        string fileName = tb + "_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
        MemoryStream ms = ExportDataTableToExcel(dt, tb) as MemoryStream;
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename*=utf-8'zh_cn'" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
        //强制输出bom 这样避免excel打开时乱码
        //HttpContext.Current.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
        HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        HttpContext.Current.Response.End();
        ms.Close();
        ms = null;
    }

    /// <summary>
    /// 导出的列名称
    /// </summary>
    /// <returns></returns>
    public string toExcel(int state, out string tb, out string tbName, out string str, out string orderby)
    {
        string strwhat = "*";
        tbName = "Dis_Order";
        tb = "订单";
        orderby = " Order by ID desc";
        str = "";

        LoginModel model = null;
        if (!(HttpContext.Current.Session["UserModel"] is LoginModel))
            HttpContext.Current.Response.Redirect("~/login.aspx", true);
        else
            model = HttpContext.Current.Session["UserModel"] as LoginModel;

        switch (state)
        {
            case 1:
                //导出订单
                strwhat = @"o.ReceiptNo 订单编号,disName 代理商名称, CONVERT(varchar(100),o.CreateDate, 23) 下单时间,
case o.OState when 1 then '待审核' when 2 then '待发货' when 3 then '退货处理' when 4 then '待收货' when 5 then '已到货' when 6 then '已作废' else '已退货' end 订单状态,case o.PayState when 0 then '未支付' when 1 then '部分支付' when 2 then '已支付' when 5 then '已退款' end 支付状态,
case o.AddType when 1 then '网页下单' when 2 then '企业补单' when 3 then 'App下单' else 'App企业补单' end 订单来源,
o.Principal 联系人,o.Phone 联系电话,o.[Address] 收货地址,CONVERT(varchar(100), o.ArriveDate, 23) 发货时间,o.GiveMode 配送方式,
Convert(decimal(18,2),o.TotalAmount) 商品总价,
o.bateAmount 使用返利,oe.ProAmount 促销金额,o.PostFee 运费,
Convert(decimal(18,2),o.AuditAmount) 应收金额,Convert(decimal(18,2),o.PayedAmount) 支付金额,o.Remark 备注";
                tbName = "DIS_Order o left join Dis_OrderExt oe on o.id=oe.OrderID left join BD_Distributor dis on o.DisID=dis.id";

                str += " and isnull(o.dr,0)=0 and o.Otype<>9";
                if (model.DisID != 0)
                    str += "and o.DisID=" + model.DisID;
                if (model.CompID != 0)
                    str += "and o.CompID=" + model.CompID;
                var ostate = Request["OState"];
                //string strwhereOState = string.Empty;
                if (ostate != "-2")
                {
                    if (ostate.ToInt(0) == 0)
                        str += " and o.OState in (1,2,3,4) ";
                    else if (ostate.ToInt(0) == 1)
                        str += " and o.OState=5 ";
                    else
                        str += " and o.OState=6 ";
                }
                //strwhere += strwhereOState;
                orderby = " Order by o.ID desc";
                tb = "订单";
                break;
            case 2:
                //导出退货单
                strwhat = @"o.ReceiptNo 退单编号,dis.disName 代理商名称,
case o.ReturnState when -1 then '已拒绝' when 1 then '待审核' when 2 then '已退货' else '已退货款' end
 退货状态,CONVERT(varchar(100), o.ReturnDate, 20) 退货时间,
o.ReturnContent  退货说明,u.TrueName 申请人";
                tbName = "DIS_OrderReturn o left join BD_Distributor dis on o.DisID=dis.ID left join SYS_Users u on o.CreateUserID=u.ID";
                str += " and isnull(o.dr,0)=0";

                if (model.DisID != 0)
                    str += "and o.DisID=" + model.DisID;
                if (model.CompID != 0)
                    str += "and o.CompID=" + model.CompID;

                orderby = " Order by o.ID desc";
                tb = "退货单";
                break;
            case 3:
                //导出商品
                strwhat = @" g.GoodsName 商品名称,c.CategoryName 商品分类,g.Unit 单位,
 Convert(decimal(18,2),g.SalePrice) 销售价格,case g.IsLS when 0 then '' else Convert(nvarchar,g.LSPrice) end 零售价,case g.IsOffline when 0 then '下架' else '上架' end 状态,case g.IsRecommended when 1 then '店铺显示' else '店铺推荐' end 是否店铺推荐,g.Title 关键词卖点,memo 备注";
                tbName = @"BD_Goods g left join BD_GoodsCategory c on g.CategoryID=c.id";

                str += " and isnull(g.dr,0)=0";
                if (model.CompID != 0)
                    str += "and g.CompID=" + model.CompID;

                orderby = " Order by g.ID desc";
                tb = "商品";
                break;
            
            case 4:
                //导出代理商
                strwhat = @"dis.DisName 代理商名称,t.TypeName 代理商分类,a.AreaName 代理商区域,
 case dis.AuditState when 0 then '待审核' else '已审核' end 审核状态,
CONVERT(varchar(100), dis.CreateDate, 23) 入住时间,dis.Principal 联系人,dis.phone 联系手机
,dis.Leading 负责人,dis.LeadingPhone 负责电话,dis.Licence 负责人身份证号码,(dis.Province+ dis.City+dis.Area+dis.Address) 地址";

                tbName = @"BD_Distributor dis left join BD_DisType t on dis.DisTypeId=t.ID
 left join BD_DisArea a on dis.AreaID=a.ID";

                str += " and isnull(dis.dr,0)=0";
                if (model.CompID != 0)
                    str += "and dis.CompID=" + model.CompID;

                orderby = " Order by dis.ID desc";
                tb = "代理商";
                break;
            case 5:
                //导出钱包查询
                break;
            case 6:
                //导出商品库存
                strwhat = @"g.GoodsName 商品名称,c.CategoryName 商品分类,info.BarCode 商品编码,info.ValueInfo 规格属性,
 info.Inventory 库存,g.Unit 单位,
 Convert(decimal(18,2),info.TinkerPrice) 销售价格,case info.IsOffline when 0 then '下架' else '上架' end 状态,case g.IsRecommended when 1 then '店铺显示' else '店铺推荐' end 是否店铺推荐,g.Title 关键词卖点,memo 备注";

                tbName = @"BD_GoodsInfo info left join BD_Goods g on info.GoodsID=g.ID 
 left join BD_GoodsCategory c on g.CategoryID=c.id";

                str += " and isnull(info.dr,0)=0";

                if (model.CompID != 0)
                    str += "and info.CompID=" + model.CompID;

                orderby = " Order by info.ID desc";
                tb = "商品库存";

                break;
        }

        return strwhat;
    }

    /// <summary>
    /// 导出处理方法
    /// </summary>
    /// <param name="sourceTable"></param>
    /// <param name="sheetName"></param>
    /// <returns></returns>
    private static Stream ExportDataTableToExcel(DataTable sourceTable, string sheetName)
    {
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        ISheet sheet = workbook.CreateSheet(sheetName);
        IRow headerRow = sheet.CreateRow(0);
        foreach (DataColumn column in sourceTable.Columns)
        {
            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
        }
        int rowIndex = 1;

        foreach (DataRow row in sourceTable.Rows)
        {
            IRow dataRow = sheet.CreateRow(rowIndex);

            foreach (DataColumn column in sourceTable.Columns)
            {
                ICell c = dataRow.CreateCell(column.Ordinal);

                Type temp_type = column.DataType;
                if (temp_type == typeof(decimal))
                    c.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                else if (temp_type == typeof(DateTime))
                {
                    IDataFormat datastyle = workbook.CreateDataFormat();
                    c.CellStyle.DataFormat = datastyle.GetFormat("yyyy-mm-dd");
                }
                //sheet.SetColumnWidth(column.Ordinal, (row[column].ToString().Length + 1) * 256);
                c.SetCellValue(row[column].ToString());
            }

            rowIndex++;
        }

        workbook.Write(ms);
        ms.Flush();
        ms.Position = 0;

        sheet = null;
        headerRow = null;
        workbook = null;

        return ms;
    }


}