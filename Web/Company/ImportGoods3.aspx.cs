using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;

public partial class Company_ImportGoods3 : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.Session["GoodsTable"] != null)
            {
                DataTable dt = HttpContext.Current.Session["GoodsTable"] as DataTable;
                int cg = 0;//导入成功的条数
                int sb = 0;//导入失败的条数
                string errorli = string.Empty;//错误列表
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["chkstr"].ToString() == "数据正确！")
                    {
                        cg++;
                    }
                    else
                    {
                        sb++;
                        errorli += "<li>" + dt.Rows[i]["chkstr"] + "</li>";
                    }
                }
                string str = string.Empty;//是否显示导入成功
                if (cg == dt.Rows.Count)
                {
                    str = " $(\".imNo\").eq(1).show();$(\".imNo\").eq(0).hide();";
                }
                else
                {
                    str = " $(\".imNo\").eq(0).show();$(\".imNo\").eq(1).hide();";
                }
                //var result= from p in dt.AsEnumerable()  select new { rowcount = p.Field<int>("rowcount"),count= p.Field<int>("count") };
                ClientScript.RegisterClientScriptBlock(this.GetType(), "daoru", "<script>$(function(){$(\".oclor2\").text(" + dt.Rows.Count + ");$(\".oclor1\").text(" + cg + ");$(\".oclor3\").text(" + sb + ");" + str + " $(\".le2\").html(\"" + errorli + "\");})</script>");
                HttpContext.Current.Session["GoodsTable2"] = null;
                HttpContext.Current.Session["GoodsTable2"] = dt;
                HttpContext.Current.Session["GoodsTable"] = null;
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
        chkstr();
    }
    /// <summary>
    /// 导出错误列表
    /// </summary>
    public void chkstr()
    {
        if (HttpContext.Current.Session["GoodsTable2"] != null)
        {
            DataTable dt = HttpContext.Current.Session["GoodsTable2"] as DataTable;

            if (dt != null && dt.Rows.Count > 0)
            {
                //创建表
                IWorkbook wb = new HSSFWorkbook();
                HSSFSheet sh = wb.CreateSheet("商品导入模版") as HSSFSheet;

                //设置所有单元的宽度 
                sh.SetColumnWidth(0, 20 * 256);
                sh.SetColumnWidth(1, 27 * 256);
                sh.SetColumnWidth(2, 27 * 256);
                sh.SetColumnWidth(3, 30 * 256);
                sh.SetColumnWidth(4, 20 * 256);
                sh.SetColumnWidth(5, 15 * 256);

                sh.SetColumnWidth(6, 30 * 256);
                sh.SetColumnWidth(7, 30 * 256);
                sh.SetColumnWidth(8, 30 * 256);
                sh.SetColumnWidth(9, 30 * 256);

                sh.SetColumnWidth(10, 15 * 256);
                sh.SetColumnWidth(11, 40 * 256);
                sh.SetColumnWidth(12, 10 * 256);
                sh.SetColumnWidth(13, 10 * 256);
                sh.SetColumnWidth(14, 35 * 256);
                sh.DefaultRowHeight = 20 * 20;

                #region 第一行
                //标题行
                IRow row0 = sh.CreateRow(0);
                row0.Height = 30 * 20;
                ICell icell1top0 = row0.CreateCell(0);
                icell1top0.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.商品导入头, DerivedExcel.CellRange(sh, 0, 0, 0, 10), sh);
                icell1top0.SetCellValue("商品表格导入模版");
                #endregion

                #region 第二行
                IRow row1 = sh.CreateRow(1);
                row1.Height = 100 * 20;
                ICell icell2top0 = row1.CreateCell(0);
                icell2top0.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入说明, DerivedExcel.CellRange(sh, 1, 1, 0, 10), sh);
                string str = @"导入说明：      
1.请不要修改模版基本格式，包括标题，否则会导致错误；
2.列头为红色的字段为必填项，不能为空，其他为非必填项，如果填写了，以填写内容为准；
3.所填写的分类必须是在厂商管理平台>商品>商品分类界面已经维护好的商品分类信息；例如：食品(一级分类);食品/牛奶(二级分类);食品/牛奶/伊利(三级分类)；
4.如果厂商管理平台设置启用了库存，则库存必须填写；设置不启用库存，库存可以不填；
5.如果导入时出现错误提示，请按照系统给出的错误提示修正出错内容后重新导入； 
6.导入模板有三条示例，您可以删除示例后进行填写，也可以在示例的下一行开始填写，示例将不会被导入。
7.如果设置了多规格，商品编码将由系统自动生成！
";

                icell2top0.SetCellValue(str);
                #endregion

                #region 第三行
                IRow row2 = sh.CreateRow(2);
                row2.Height = 40 * 20;

                ICell icell3top = row2.CreateCell(0);
                icell3top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入错误提示, null, sh);
                icell3top.SetCellValue("出错原因，按照提示调整后从新导入");

                ICell icell4top = row2.CreateCell(1);
                icell4top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell4top.SetCellValue("分类*（必填项，“/”号分割分类）");

                ICell icell5top = row2.CreateCell(2);
                icell5top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell5top.SetCellValue("商品名称*（必填项，不超过30个汉字）");

                ICell icell6top = row2.CreateCell(3);
                icell6top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell6top.SetCellValue("商品编码（允许为空，如果填写，不超过15个字符）");

                ICell icell7top = row2.CreateCell(4);
                icell7top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell7top.SetCellValue("计量单位*（必填项，如：件、个）");

                ICell icell8top = row2.CreateCell(5);
                icell8top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell8top.SetCellValue("价格*（必填项，最多两位小数）");



                ICell icell9top = row2.CreateCell(6);
                icell9top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell9top.SetCellValue("多规格字段设置（不同规格用“/”号分割）");

                ICell icel20top = row2.CreateCell(7);
                icel20top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icel20top.SetCellValue("规格1内容");

                ICell icel21top = row2.CreateCell(8);
                icel21top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icel21top.SetCellValue("规格2内容");

                ICell icel22top = row2.CreateCell(9);
                icel22top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icel22top.SetCellValue("规格3内容");



                ICell icel23top = row2.CreateCell(10);
                icel23top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icel23top.SetCellValue("库存（可设置是否启用）");

                ICell icell10top = row2.CreateCell(11);
                icell10top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell10top.SetCellValue("卖点/关键词（允许为空，所填内容用来简单描述商品卖点信息）");

                ICell icell11top = row2.CreateCell(12);
                icell11top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell11top.SetCellValue("店铺显示（是/否）");

                ICell icell12top = row2.CreateCell(13);
                icell12top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell12top.SetCellValue("上架（是/否）");

                ICell icell13top = row2.CreateCell(14);
                icell13top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell13top.SetCellValue("商品详情描述（允许为空）");
                #endregion

                int i = 3;
                IRow row = null;

                ICell icell0 = null;
                ICell icell1 = null;
                ICell icell2 = null;
                ICell icell3 = null;
                ICell icell4 = null;
                ICell icell5 = null;

                ICell icell6 = null;
                ICell icell7 = null;
                ICell icell8 = null;
                ICell icell9 = null;

                ICell icell10 = null;
                ICell icell11 = null;
                ICell icell12 = null;
                ICell icell13 = null;
                ICell icell14 = null;

                foreach (DataRow item in dt.Rows)
                {
                    if (item["chkstr"].ToString() != "数据正确！")
                    {
                        #region 第i行
                        row = sh.CreateRow(i);
                        row.Height = 25 * 20;

                        icell0 = row.CreateCell(0);
                        icell0.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell0.SetCellValue(item["chkstr"].ToString());

                        icell1 = row.CreateCell(1);
                        icell1.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell1.SetCellValue(item["category"].ToString());

                        icell2 = row.CreateCell(2);
                        icell2.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell2.SetCellValue(item["goodsname"].ToString());

                        icell3 = row.CreateCell(3);
                        icell3.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell3.SetCellValue(item["barcode"].ToString());

                        icell4 = row.CreateCell(4);
                        icell4.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell4.SetCellValue(item["unit"].ToString());

                        icell5 = row.CreateCell(5);
                        icell5.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.钱, null, sh);
                        icell5.SetCellValue(item["price"].ToString());


                        icell6 = row.CreateCell(6);
                        icell6.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.钱, null, sh);
                        icell6.SetCellValue(string.Join("/", (string[])item["spec"]));

                        icell7 = row.CreateCell(7);
                        icell7.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.钱, null, sh);
                        icell7.SetCellValue(item["value1"].ToString());

                        icell8 = row.CreateCell(8);
                        icell8.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.钱, null, sh);
                        icell8.SetCellValue(item["value2"].ToString());

                        icell9 = row.CreateCell(9);
                        icell9.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.钱, null, sh);
                        icell9.SetCellValue(item["value3"].ToString());



                        icell10 = row.CreateCell(10);
                        icell10.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.数字, null, sh);
                        icell10.SetCellValue(item["inventory"].ToString());

                        icell11 = row.CreateCell(11);
                        icell11.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell11.SetCellValue(item["title"].ToString());

                        icell12 = row.CreateCell(12);
                        icell12.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell12.SetCellValue(item["isrecommended"].ToString());

                        icell13 = row.CreateCell(13);
                        icell13.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell13.SetCellValue(item["isoffline"].ToString());

                        icell14 = row.CreateCell(14);
                        icell14.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell14.SetCellValue(item["details"].ToString());

                        i++;
                        #endregion
                    }
                }

                string fileName = Server.MapPath("GoodsNew/TemplateFile/") + "导出错误列表_" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
                using (FileStream stm = File.OpenWrite(fileName))
                {
                    wb.Write(stm);
                }

                try
                {
                    FileInfo DownloadFile = new FileInfo(fileName);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename*=utf-8'zh_cn'" + System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes("导出错误列表_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls")));
                    HttpContext.Current.Response.ContentType = "application/ms-excel;charset=UTF-8";//DownloadFile.FullNameoctet-stream 
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("shift-jis");
                    HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    HttpContext.Current.Response.WriteFile(fileName);
                    HttpContext.Current.Response.Flush();

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        else
        {
            //JScript.AlertMethod(this, "请先导入", JScript.IconOption.错误, "function(){location.href='ImportGoods.aspx'}");
        }
    }
}