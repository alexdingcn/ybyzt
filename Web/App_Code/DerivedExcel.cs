using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;

/// <summary>
///DerivedExcel 的摘要说明
/// </summary>
public class DerivedExcel
{
    /**
     * 导出Excel
     * **/
    public DerivedExcel()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 订单导出
    /// </summary>
    /// <param name="title">订单导出主表标题</param>
    /// <param name="dtSources">订单主表信息</param>
    /// <param name="dtDtlSources">订单商品明细</param>
    /// <param name="strFileName">导出excel名称</param>
    /// <returns></returns>
    public static string ExportEasy(string title, DataTable dtSources, DataTable dtDtlSources, string strFileName)
    {
        //创建表
        IWorkbook wb = new HSSFWorkbook();

        foreach (DataRow item in dtSources.Rows)
        {
            //创建簿
            //ISheet sh = wb.CreateSheet(item.ReceiptNo);
            HSSFSheet sh = wb.CreateSheet(item["ReceiptNo"].ToString()) as HSSFSheet;

            //设置所有单元的宽度 
            sh.DefaultColumnWidth = 5 * 1;
            sh.DefaultRowHeight = 20 * 20;
            //sh.DefaultRowHeightInPoints = 20;
            //设置单元的宽度  
            //sh.SetColumnWidth(0, 15 * 256);

            //标题合并单元格
            //CellRangeAddress（）该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。
            //NPOI.SS.Util.CellRangeAddress region = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 23); 
            //sh.AddMergedRegion(region);

            #region 第一行
            //主表标题行
            IRow row0 = sh.CreateRow(0);
            row0.Height = 30 * 20;
            ICell icell1top0 = row0.CreateCell(0);
            icell1top0.CellStyle = Getcellstyle(wb, stylexls.头, CellRange(sh, 0, 0, 0, 23), sh);
            icell1top0.SetCellValue(title);
            #endregion

            #region 第二行
            IRow row1 = sh.CreateRow(1);
            row1.Height = 25 * 20;

            ICell icell1top = row1.CreateCell(0);
            icell1top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 1, 1, 0, 1), sh);
            icell1top.SetCellValue("订单编号");

            ICell icell2top = row1.CreateCell(2);
            icell2top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 1, 1, 2, 5), sh);
            icell2top.SetCellValue(item["ReceiptNo"].ToString());

            ICell icell3top = row1.CreateCell(6);
            icell3top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 1, 1, 6, 7), sh);
            icell3top.SetCellValue("客户名称");

            ICell icell4top = row1.CreateCell(8);
            icell4top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 1, 1, 8, 12), sh);
            icell4top.SetCellValue(item["DisName"].ToString());

            ICell icell5top = row1.CreateCell(13);
            icell5top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 1, 1, 13, 14), sh);
            icell5top.SetCellValue("状态");

            ICell icell6top = row1.CreateCell(15);
            icell6top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 1, 1, 15, 17), sh);
            icell6top.SetCellValue(OrderType.GetOState(item["OState"].ToString(), item["IsOutState"].ToString()));

            ICell icell7top = row1.CreateCell(18);
            icell7top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 1, 1, 18, 19), sh);
            icell7top.SetCellValue("订单日期");

            ICell icell8top = row1.CreateCell(20);
            icell8top.CellStyle = Getcellstyle(wb, stylexls.时间, CellRange(sh, 1, 1, 20, 23), sh);
            icell8top.SetCellValue(item["CreateDate"].ToString().ToDateTime());

            #endregion

            #region 第三行
            IRow row2 = sh.CreateRow(2);
            row2.Height = 25 * 20;

            ICell icell9top = row2.CreateCell(0);
            icell9top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 2, 2, 0, 1), sh);
            icell9top.SetCellValue("商品总额");

            ICell icell10top = row2.CreateCell(2);
            icell10top.CellStyle = Getcellstyle(wb, stylexls.钱, CellRange(sh, 2, 2, 2, 5), sh);
            if (item["TotalAmount"].ToString() != "")
                icell10top.SetCellValue(item["TotalAmount"].ToString().ToDecimal().ToString("0.00"));
            else
                icell10top.SetCellValue("0.00");

            ICell icell11top = row2.CreateCell(6);
            icell11top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 2, 2, 6, 7), sh);
            icell11top.SetCellValue("应付总额");

            ICell icell12top = row2.CreateCell(8);
            icell12top.CellStyle = Getcellstyle(wb, stylexls.钱, CellRange(sh, 2, 2, 8, 12), sh);
            if (item["AuditAmount"].ToString() != "")
                icell12top.SetCellValue(item["AuditAmount"].ToString().ToDecimal().ToString("0.00"));
            else
                icell12top.SetCellValue("0.00");

            ICell icell13top = row2.CreateCell(13);
            icell13top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 2, 2, 13, 14), sh);
            icell13top.SetCellValue("促销优惠");

            ICell icell14top = row2.CreateCell(15);
            icell14top.CellStyle = Getcellstyle(wb, stylexls.钱, CellRange(sh, 2, 2, 15, 17), sh);
            if (item["ProAmount"].ToString() != "")
                icell14top.SetCellValue(item["ProAmount"].ToString().ToDecimal().ToString("0.00"));
            else
                icell14top.SetCellValue("0.00");

            ICell icell15top = row2.CreateCell(18);
            icell15top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 2, 2, 18, 19), sh);
            icell15top.SetCellValue("返利抵扣");

            ICell icell16top = row2.CreateCell(20);
            icell16top.CellStyle = Getcellstyle(wb, stylexls.钱, CellRange(sh, 2, 2, 20, 23), sh);
            if (item["bateAmount"].ToString() != "")
                icell16top.SetCellValue(item["bateAmount"].ToString().ToDecimal().ToString("0.00"));
            else
                icell16top.SetCellValue("0.00");

            #endregion

            #region 第四行

            IRow row3 = sh.CreateRow(3);
            row3.Height = 25 * 20;

            ICell icell17top = row3.CreateCell(0);
            icell17top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 3, 3, 0, 1), sh);
            icell17top.SetCellValue("交货日期");

            ICell icell18top = row3.CreateCell(2);
            icell18top.CellStyle = Getcellstyle(wb, stylexls.时间, CellRange(sh, 3, 3, 2, 5), sh);
            if (item["ArriveDate"].ToString() != "")
                icell18top.SetCellValue(item["ArriveDate"].ToString().ToDateTime());
            else
                icell18top.SetCellValue("");

            ICell icell19top = row3.CreateCell(6);
            icell19top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 3, 3, 6, 7), sh);
            icell19top.SetCellValue("配送方式");

            ICell icell20top = row3.CreateCell(8);
            icell20top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 3, 3, 8, 12), sh);
            icell20top.SetCellValue(item["GiveMode"].ToString());

            ICell icell21top = row3.CreateCell(13);
            icell21top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 3, 3, 13, 14), sh);
            icell21top.SetCellValue("发票号");

            ICell icell22top = row3.CreateCell(15);
            icell22top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 3, 3, 15, 17), sh);
            icell22top.SetCellValue(item["BillNo"].ToString());

            ICell icell23top = row3.CreateCell(18);
            icell23top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 3, 3, 18, 19), sh);
            icell23top.SetCellValue("运 费");

            ICell icell24top = row3.CreateCell(20);
            icell24top.CellStyle = Getcellstyle(wb, stylexls.钱, CellRange(sh, 3, 3, 20, 23), sh);
            if (item["PostFee"].ToString() != "")
                icell24top.SetCellValue(item["PostFee"].ToString().ToDecimal().ToString("0.00"));
            else
                icell24top.SetCellValue("0.00");

            #endregion

            #region 第五行

            IRow row4 = sh.CreateRow(4);
            row4.Height = 25 * 20;

            ICell icell25top = row4.CreateCell(0);
            icell25top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 4, 4, 0, 1), sh);
            icell25top.SetCellValue("收货人");

            ICell icell26top = row4.CreateCell(2);
            icell26top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 4, 4, 2, 5), sh);
            icell26top.SetCellValue(item["Principal"].ToString());

            ICell icell27top = row4.CreateCell(6);
            icell27top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 4, 4, 6, 7), sh);
            icell27top.SetCellValue("联系电话");

            ICell icell28top = row4.CreateCell(8);
            icell28top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 4, 4, 8, 12), sh);
            icell28top.SetCellValue(item["Phone"].ToString());

            ICell icell29top = row4.CreateCell(13);
            icell29top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 4, 4, 13, 14), sh);
            icell29top.SetCellValue("收货地址");

            ICell icell30top = row4.CreateCell(15);
            icell30top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 4, 4, 15, 23), sh);
            icell30top.SetCellValue(item["Address"].ToString());
            #endregion

            #region 第六行
            IRow row5 = sh.CreateRow(5);
            row5.Height = 30 * 20;

            ICell icell31top = row5.CreateCell(0);
            icell31top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 5, 5, 0, 1), sh);
            icell31top.SetCellValue("开票信息");

            ICell icell32top = row5.CreateCell(2);
            icell32top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, 5, 5, 2, 23), sh);
            string Billing = string.Empty;
            //开票信息
            if (item["IsOBill"].ToString().ToInt(0) > 0)
            {
                Billing += "发票抬头：" + item["Rise"].ToString();
                Billing += "，发票内容：" + item["Content"].ToString();
                if (item["IsOBill"].ToString().ToInt(0) == 2)
                {
                    Billing += "，开户银行：" + item["OBank"].ToString();
                    Billing += "，开户账户：" + item["OAccount"].ToString();
                    Billing += "，纳税人登记号：" + item["TRNumber"].ToString();
                }
            }
            icell32top.SetCellValue(Billing);

            #endregion

            //从表信息 
            IRow rowD0 = sh.CreateRow(6);
            rowD0.Height = 30 * 20;
            ICell icellD1top0 = rowD0.CreateCell(0);
            icellD1top0.CellStyle = Getcellstyle(wb, stylexls.头, CellRange(sh, 6, 6, 0, 23), sh);
            icellD1top0.SetCellValue("商 品 明 细");

            DataRow[] dr = dtDtlSources.Select(string.Format("OrderID='{0}'", item["ID"]));

            if (dr.Length > 0)
            {
                #region 从表表头信息
                IRow rowDtop = sh.CreateRow(7);
                rowDtop.Height = 25 * 20;
                ICell icellDtop = rowDtop.CreateCell(0);
                icellDtop.CellStyle = Getcellstyle(wb, stylexls.居中, null, null);
                icellDtop.SetCellValue("序 号");

                ICell icellDtop1 = rowDtop.CreateCell(1);
                icellDtop1.CellStyle = Getcellstyle(wb, stylexls.居中, CellRange(sh, 7, 7, 1, 3), sh);
                icellDtop1.SetCellValue("商 品 编 码");

                ICell icellDtop2 = rowDtop.CreateCell(4);
                icellDtop2.CellStyle = Getcellstyle(wb, stylexls.居中, CellRange(sh, 7, 7, 4, 6), sh);
                icellDtop2.SetCellValue("商 品 名 称");

                ICell icellDtop3 = rowDtop.CreateCell(7);
                icellDtop3.CellStyle = Getcellstyle(wb, stylexls.居中, CellRange(sh, 7, 7, 7, 10), sh);
                icellDtop3.SetCellValue("规 格 属 性");

                ICell icellDtop4 = rowDtop.CreateCell(11);
                icellDtop4.CellStyle = Getcellstyle(wb, stylexls.居中, CellRange(sh, 7, 7, 11, 12), sh);
                icellDtop4.SetCellValue("单 位");

                ICell icellDtop5 = rowDtop.CreateCell(13);
                icellDtop5.CellStyle = Getcellstyle(wb, stylexls.居中, CellRange(sh, 7, 7, 13, 14), sh);
                icellDtop5.SetCellValue("单 价");

                ICell icellDtop6 = rowDtop.CreateCell(15);
                icellDtop6.CellStyle = Getcellstyle(wb, stylexls.居中, CellRange(sh, 7, 7, 15, 17), sh);
                icellDtop6.SetCellValue("数 量");

                ICell icellDtop7 = rowDtop.CreateCell(18);
                icellDtop7.CellStyle = Getcellstyle(wb, stylexls.居中, CellRange(sh, 7, 7, 18, 19), sh);
                icellDtop7.SetCellValue("小 计");

                ICell icellDtop8 = rowDtop.CreateCell(20);
                icellDtop8.CellStyle = Getcellstyle(wb, stylexls.居中, CellRange(sh, 7, 7, 20, 23), sh);
                icellDtop8.SetCellValue("备 注");
                #endregion

                IRow rowD = null;
                ICell icellD1top = null;
                ICell icellD2top = null;
                ICell icellD3top = null;
                ICell icellD4top = null;
                ICell icellD5top = null;
                ICell icellD6top = null;
                ICell icellD7top = null;
                ICell icellD8top = null;
                ICell icellD9top = null;

                int rowi = 8; //序号
                foreach (DataRow i in dr)
                {
                    #region 商品明细
                    rowD = sh.CreateRow(rowi);
                    rowD.Height = 20 * 20;

                    icellD1top = rowD.CreateCell(0);
                    icellD1top.CellStyle = Getcellstyle(wb, stylexls.默认, null, null);
                    icellD1top.SetCellValue((rowi - 7).ToString());

                    icellD2top = rowD.CreateCell(1);
                    icellD2top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 1, 3), sh);
                    icellD2top.SetCellValue(i["GoodsCode"].ToString());

                    icellD3top = rowD.CreateCell(4);
                    icellD3top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 4, 6), sh);
                    icellD3top.SetCellValue(i["GoodsName"].ToString());

                    icellD4top = rowD.CreateCell(7);
                    icellD4top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 7, 10), sh);
                    icellD4top.SetCellValue(i["GoodsInfos"].ToString());

                    icellD5top = rowD.CreateCell(11);
                    icellD5top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 11, 12), sh);
                    icellD5top.SetCellValue(i["Unit"].ToString());

                    icellD6top = rowD.CreateCell(13);
                    icellD6top.CellStyle = Getcellstyle(wb, stylexls.钱, CellRange(sh, rowi, rowi, 13, 14), sh);
                    if (i["AuditAmount"].ToString() != "")
                        icellD6top.SetCellValue(i["AuditAmount"].ToString().ToDecimal().ToString("0.00"));
                    else
                        icellD6top.SetCellValue("0.00");

                    icellD7top = rowD.CreateCell(15);
                    icellD7top.CellStyle = Getcellstyle(wb, stylexls.数字, CellRange(sh, rowi, rowi, 15, 17), sh);
                    if (i["GoodsNum"].ToString() != "")
                        icellD7top.SetCellValue(i["GoodsNum"].ToString().ToDecimal().ToString("0.00"));
                    else
                        icellD7top.SetCellValue("");

                    icellD8top = rowD.CreateCell(18);
                    icellD8top.CellStyle = Getcellstyle(wb, stylexls.钱, CellRange(sh, rowi, rowi, 18, 19), sh);
                    if (i["sumAmount"].ToString() != "")
                        icellD8top.SetCellValue(i["sumAmount"].ToString().ToDecimal().ToString("0.00"));
                    else
                        icellD8top.SetCellValue("0.00");

                    icellD9top = rowD.CreateCell(20);
                    icellD9top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 20, 23), sh);
                    icellD9top.SetCellValue(i["Remark"].ToString());
                    #endregion

                    rowi++;
                }

                #region 最后空白行
                rowD = sh.CreateRow(rowi);
                rowD.Height = 20 * 20;
                icellD1top = rowD.CreateCell(0);
                icellD1top.CellStyle = Getcellstyle(wb, stylexls.默认, null, null);
                icellD1top.SetCellValue("");

                icellD2top = rowD.CreateCell(1);
                icellD2top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 1, 3), sh);
                icellD2top.SetCellValue("");

                icellD3top = rowD.CreateCell(4);
                icellD3top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 4, 6), sh);
                icellD3top.SetCellValue("");

                icellD4top = rowD.CreateCell(7);
                icellD4top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 7, 10), sh);
                icellD4top.SetCellValue("");

                icellD5top = rowD.CreateCell(11);
                icellD5top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 11, 12), sh);
                icellD5top.SetCellValue("");

                icellD6top = rowD.CreateCell(13);
                icellD6top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 13, 14), sh);
                icellD6top.SetCellValue("");

                icellD7top = rowD.CreateCell(15);
                icellD7top.CellStyle = Getcellstyle(wb, stylexls.数字, CellRange(sh, rowi, rowi, 15, 17), sh);
                icellD7top.SetCellValue("");

                icellD8top = rowD.CreateCell(18);
                icellD8top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 18, 19), sh);
                icellD8top.SetCellValue("");

                icellD9top = rowD.CreateCell(20);
                icellD9top.CellStyle = Getcellstyle(wb, stylexls.默认, CellRange(sh, rowi, rowi, 20, 23), sh);
                icellD9top.SetCellValue("");

                #endregion
            }
        }

        string fileName = HttpContext.Current.Server.MapPath("UploadFile/") + strFileName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
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
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename*=utf-8'zh_cn'" + System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(strFileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls")));
            HttpContext.Current.Response.ContentType = "application/ms-excel;charset=UTF-8";//DownloadFile.FullNameoctet-stream 
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("shift-jis");
            HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            HttpContext.Current.Response.WriteFile(fileName);
            HttpContext.Current.Response.Flush();

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            return "1";
        }
        catch (Exception e)
        {
            return e.Message;
        }

    }

    public static string ExportEasy()
    {
        //创建表
        IWorkbook wb = new HSSFWorkbook();

        //创建簿
        ISheet sh = wb.CreateSheet("测试合并单元格");

        //设置单元的宽度  
        sh.SetColumnWidth(0, 15 * 256);
        sh.SetColumnWidth(1, 35 * 256);
        sh.SetColumnWidth(2, 15 * 256);
        sh.SetColumnWidth(3, 10 * 256);

        int i = 0;

        #region 练习合并单元格
        sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 3));
        //CellRangeAddress（）该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。

        IRow row0 = sh.CreateRow(0);
        row0.Height = 20 * 20;
        ICell icell1top0 = row0.CreateCell(0);
        //icell1top0.CellStyle = Getcellstyle(wb, stylexls.头);
        icell1top0.SetCellValue("标题合并单元格");
        #endregion

        i++;

        #region 设置表头
        IRow row1 = sh.CreateRow(1);
        row1.Height = 20 * 20;

        ICell icell1top = row1.CreateCell(0);
        //icell1top.CellStyle = Getcellstyle(wb, stylexls.头);
        icell1top.SetCellValue("网站名");

        ICell icell2top = row1.CreateCell(1);
        //icell2top.CellStyle = Getcellstyle(wb, stylexls.头);
        icell2top.SetCellValue("网址");

        ICell icell3top = row1.CreateCell(2);
        //icell3top.CellStyle = Getcellstyle(wb, stylexls.头);
        icell3top.SetCellValue("百度快照");

        ICell icell4top = row1.CreateCell(3);
        //icell4top.CellStyle = Getcellstyle(wb, stylexls.头);
        icell4top.SetCellValue("百度收录");
        #endregion

        using (FileStream stm = File.OpenWrite(@"D:/myMergeCell.xls"))
        {
            wb.Write(stm);
            return "提示：创建成功！";
        }

    }

    /// <summary>
    /// 合并单元格
    /// </summary>
    /// <param name="sh">工作簿</param>
    /// <param name="firstRow">开始行号</param>
    /// <param name="lastRow">结束行号</param>
    /// <param name="firstCol">开始列号</param>
    /// <param name="lastCol">结束列号</param>
    /// <returns></returns>
    public static NPOI.SS.Util.CellRangeAddress CellRange(ISheet sh, int firstRow, int lastRow, int firstCol, int lastCol)
    {
        NPOI.SS.Util.CellRangeAddress region = new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
        sh.AddMergedRegion(region);

        return region;
    }

    #region 定义单元格常用到样式
    public static ICellStyle Getcellstyle(IWorkbook wb, stylexls str, NPOI.SS.Util.CellRangeAddress region, HSSFSheet sh)
    {
        ICellStyle cellStyle = wb.CreateCellStyle();

        //定义几种字体  
        //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的  
        IFont font12 = wb.CreateFont();
        font12.FontHeightInPoints = 12;
        font12.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
        font12.FontName = "微软雅黑";

        IFont font = wb.CreateFont();
        font.FontName = "微软雅黑";
        //font.Underline = 1;下划线  

        IFont fontcolorblue = wb.CreateFont();
        fontcolorblue.Color = HSSFColor.OliveGreen.Black.Index;
        fontcolorblue.IsItalic = true;//下划线  
        fontcolorblue.FontName = "微软雅黑";

        //边框  
        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin; //BorderLeft
        if (region != null)
        {
            for (int i = region.FirstRow; i <= region.LastRow; i++)
            {
                IRow row = HSSFCellUtil.GetRow(i, sh);
                for (int j = region.FirstColumn; j <= region.LastColumn; j++)
                {
                    ICell singleCell = HSSFCellUtil.GetCell(row, (short)j);
                    singleCell.CellStyle = cellStyle;
                }
            }
        }

        //边框颜色  
        cellStyle.BottomBorderColor = HSSFColor.OliveGreen.Black.Index;
        cellStyle.TopBorderColor = HSSFColor.OliveGreen.Black.Index;
        cellStyle.LeftBorderColor = HSSFColor.OliveGreen.Black.Index;
        cellStyle.RightBorderColor = HSSFColor.OliveGreen.Black.Index;

        //背景图形，我没有用到过。感觉很丑  
        //cellStyle.FillBackgroundColor = HSSFColor.OliveGreen.Black.Index;  
        //cellStyle.FillForegroundColor = HSSFColor.OliveGreen.Black.Index;  
        //cellStyle.FillPattern = FillPatternType.NO_FILL; 
        //cellStyle.FillForegroundColor = HSSFColor.White.Index;
        //cellStyle.FillBackgroundColor = HSSFColor.Black.Index;

        //水平对齐  
        cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
        //垂直对齐  
        cellStyle.VerticalAlignment = VerticalAlignment.Center;
        //自动换行  
        cellStyle.WrapText = true;

        //缩进;当设置为1时，前面留的空白太大了。希旺官网改进。或者是我设置的不对  
        //cellStyle.Indention = 0;

        //上面基本都是设共公的设置  
        //下面列出了常用的字段类型  
        switch (str)
        {
            case stylexls.头:
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                HSSFPalette palette = ((HSSFWorkbook)wb).GetCustomPalette();
                //HSSFColor newColor = palette.AddColor((byte)153, (byte)204, (byte)255);  
                palette.SetColorAtIndex((short)10, (byte)227, (byte)232, (byte)227);
                cellStyle.FillPattern = FillPattern.SolidForeground; // NoFill; 
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;

                cellStyle.SetFont(font12);
                break;
            case stylexls.时间:
                IDataFormat datastyle = wb.CreateDataFormat();

                cellStyle.DataFormat = datastyle.GetFormat("yyyy-mm-dd");
                cellStyle.SetFont(font);
                break;
            case stylexls.数字:
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                cellStyle.SetFont(font);
                break;
            case stylexls.钱:
                //IDataFormat format = wb.CreateDataFormat();
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("¥#,##0");
                cellStyle.SetFont(font);
                break;
            case stylexls.url:
                fontcolorblue.Underline = FontUnderlineType.None;
                cellStyle.SetFont(fontcolorblue);
                break;
            case stylexls.百分比:
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                cellStyle.SetFont(font);
                break;
            case stylexls.中文大写:
                IDataFormat format1 = wb.CreateDataFormat();
                cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                cellStyle.SetFont(font);
                break;
            case stylexls.科学计数法:
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                cellStyle.SetFont(font);
                break;
            case stylexls.默认:
                cellStyle.SetFont(font);
                break;
            case stylexls.居中:
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                cellStyle.SetFont(font);
                break;
            case stylexls.商品导入头:
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                HSSFPalette pale = ((HSSFWorkbook)wb).GetCustomPalette();
                pale.SetColorAtIndex((short)30, (byte)11, (byte)87, (byte)235);
                cellStyle.FillPattern = FillPattern.SolidForeground; // NoFill; 
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.CornflowerBlue.Index;

                cellStyle.SetFont(font12);
                break;
            case stylexls.导入说明:

                HSSFPalette pale1 = ((HSSFWorkbook)wb).GetCustomPalette();
                pale1.SetColorAtIndex((short)61, (byte)227, (byte)232, (byte)227);
                cellStyle.FillPattern = FillPattern.SolidForeground; // NoFill; 
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightCornflowerBlue.Index;
                IFont font2 = wb.CreateFont();
                font2.FontName = "微软雅黑";
                font2.FontHeightInPoints = 9;
                cellStyle.SetFont(font2);
                break;
            case stylexls.导入红字:
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                HSSFPalette palebgss = ((HSSFWorkbook)wb).GetCustomPalette();
                palebgss.SetColorAtIndex((short)62, (byte)255, (byte)0, (byte)0);
                cellStyle.FillPattern = FillPattern.SolidForeground; // NoFill; 
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                IFont fontRed = wb.CreateFont();
                fontRed.FontName = "微软雅黑";
                fontRed.Color = HSSFColor.OliveGreen.Red.Index;
                cellStyle.SetFont(fontRed);
                break;
            case stylexls.导入错误提示:
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                HSSFPalette paleError = ((HSSFWorkbook)wb).GetCustomPalette();
                paleError.SetColorAtIndex((short)10, (byte)255, (byte)0, (byte)0);
                cellStyle.FillPattern = FillPattern.SolidForeground; // NoFill; 
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;

                cellStyle.SetFont(font);
                break;
            case stylexls.导入背景色:
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                HSSFPalette palebgs = ((HSSFWorkbook)wb).GetCustomPalette();
                palebgs.SetColorAtIndex((short)62, (byte)255, (byte)0, (byte)0);
                cellStyle.FillPattern = FillPattern.SolidForeground; // NoFill; 
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;

                cellStyle.SetFont(font);
                break;
        }
        return cellStyle;


    }
    #endregion

    #region 定义单元格常用到样式的枚举
    public enum stylexls
    {
        居中,
        头,
        url,
        时间,
        数字,
        钱,
        百分比,
        中文大写,
        科学计数法,
        商品导入头,
        导入说明,
        导入红字,
        导入错误提示,
        导入背景色,
        默认
    }
    #endregion
}