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

public partial class Company_ImportDis3 : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.Session["DisTable"] != null)
            {
                DataTable dt = HttpContext.Current.Session["DisTable"] as DataTable;
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "daoru", "<script>$(function(){$(\".oclor2\").text(" + dt.Rows.Count + ");$(\".oclor1\").text(" + cg + ");$(\".oclor3\").text(" + sb + ");" + str + " $(\".le2\").html(\"" + errorli + "\");})</script>");
                HttpContext.Current.Session["DisTable2"] = null;
                HttpContext.Current.Session["DisTable2"] = dt;
                HttpContext.Current.Session["DisTable"] = null;
            }
            else
            {
                JScript.AlertMethod(this, "请先导入", JScript.IconOption.错误, "function(){location.href='ImportDis.aspx'}");
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
        if (HttpContext.Current.Session["DisTable2"] != null)
        {
            DataTable dt = HttpContext.Current.Session["DisTable2"] as DataTable;

            if (dt != null && dt.Rows.Count > 0)
            {
                //创建表
                IWorkbook wb = new HSSFWorkbook();
                HSSFSheet sh = wb.CreateSheet("代理商导入模版") as HSSFSheet;

                //设置所有单元的宽度 
                sh.SetColumnWidth(0, 20 * 256);
                sh.SetColumnWidth(1, 27 * 256);
                sh.SetColumnWidth(2, 27 * 256);
                sh.SetColumnWidth(3, 30 * 256);
                sh.SetColumnWidth(4, 20 * 256);
                sh.SetColumnWidth(5, 15 * 256);
                sh.SetColumnWidth(6, 15 * 256);
                sh.SetColumnWidth(7, 40 * 256);
                sh.SetColumnWidth(8, 10 * 256);
                sh.SetColumnWidth(9, 10 * 256);
                sh.SetColumnWidth(10, 10 * 256);
                sh.SetColumnWidth(11, 35 * 256);
                sh.DefaultRowHeight = 20 * 20;

                #region 第一行
                //标题行
                IRow row0 = sh.CreateRow(0);
                row0.Height = 30 * 20;
                ICell icell1top0 = row0.CreateCell(0);
                icell1top0.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.商品导入头, DerivedExcel.CellRange(sh, 0, 0, 0, 11), sh);
                icell1top0.SetCellValue("代理商表格导入模版");
                #endregion

                #region 第二行
                IRow row1 = sh.CreateRow(1);
                row1.Height = 100 * 20;
                ICell icell2top0 = row1.CreateCell(0);
                icell2top0.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入说明, DerivedExcel.CellRange(sh, 1, 1, 0, 11), sh);
                string str = @"导入说明：      
1.请千万不要修改模版基本格式，包括标题，否则会导致错误；同时请千万不要修改或删除Sheet1、Sheet2页签；
2.列头为红色的字段，为必填项，不能为空，其他为非必填，如果填写了，以填写内容为准；
3.不同代理商（代理商）名称、管理员登录帐号、管理员手机不能重复；
4.如果导入时出现错误提示，请按照系统给出的错误提示修正出错内容后重新导入； 
5.导入模版有系统给出的三条示例导入数据，这三条示例数据将不被导入。您可以在示例数据后的下一行填入您所需要的内容， 如果不需要示例数据，可以清空该内容后填入您所需要的内容。";

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
                icell4top.SetCellValue("代理商名称 *（2-20个汉字或字母，推荐使用中文名称）");

                ICell icell5top = row2.CreateCell(2);
                icell5top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell5top.SetCellValue("管理员姓名 *（请填写真实姓名，以便更好地为您服务）");

                ICell icell6top = row2.CreateCell(3);
                icell6top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell6top.SetCellValue("管理员登录帐号 *（2-20个文字、字母、数字，可以录入代理商姓名、简称等，一经设定无法更改，将来可用手机号进行登录）");

                ICell icell7top = row2.CreateCell(4);
                icell7top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell7top.SetCellValue("管理员手机 *（登录、发送验证短信）");

                ICell icell8top = row2.CreateCell(5);
                icell8top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell8top.SetCellValue("所在省*");

                ICell icell9top = row2.CreateCell(6);
                icell9top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell9top.SetCellValue("所在市*");

                ICell icell10top = row2.CreateCell(7);
                icell10top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell10top.SetCellValue("所在区*");

                ICell icell11top = row2.CreateCell(8);
                icell11top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入红字, null, sh);
                icell11top.SetCellValue("详细地址 *（常用收货地址）");

                ICell icell12top = row2.CreateCell(9);
                icell12top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell12top.SetCellValue("代理商分类");

                ICell icell13top = row2.CreateCell(10);
                icell13top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell13top.SetCellValue("代理商区域");
                ICell icell14top = row2.CreateCell(11);
                icell14top.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.导入背景色, null, sh);
                icell14top.SetCellValue("备注");
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
                        icell1.SetCellValue(item["disname"].ToString());

                        icell2 = row.CreateCell(2);
                        icell2.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell2.SetCellValue(item["principal"].ToString());

                        icell3 = row.CreateCell(3);
                        icell3.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell3.SetCellValue(item["username"].ToString());

                        icell4 = row.CreateCell(4);
                        icell4.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell4.SetCellValue(item["phone"].ToString());

                        icell5 = row.CreateCell(5);
                        icell5.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell5.SetCellValue(item["pro"].ToString());

                        icell6 = row.CreateCell(6);
                        icell6.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell6.SetCellValue(item["city"].ToString());

                        icell7 = row.CreateCell(7);
                        icell7.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell7.SetCellValue(item["quxian"].ToString());

                        icell8 = row.CreateCell(8);
                        icell8.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell8.SetCellValue(item["address"].ToString());

                        icell9 = row.CreateCell(9);
                        icell9.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell9.SetCellValue(item["distype"].ToString());

                        icell10 = row.CreateCell(10);
                        icell10.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell10.SetCellValue(item["area"].ToString());

                        icell11 = row.CreateCell(11);
                        icell11.CellStyle = DerivedExcel.Getcellstyle(wb, DerivedExcel.stylexls.默认, null, sh);
                        icell11.SetCellValue(item["remark"].ToString());

                        i++;
                        #endregion
                    }
                }

                string fileName = Server.MapPath("SysManager/TemplateFile/") + "导出错误列表_" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
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