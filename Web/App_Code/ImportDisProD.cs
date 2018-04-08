using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DBUtility;
using DataExtraction;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;

/// <summary>
///ImportDisProD 的摘要说明
/// </summary>
public class ImportDisProD
{
    DataTable ErrTb = null; //出错提示
    DataTable FindTb = new DataTable();
    DataRow row = null;
    int CompID = 0;
    int UserID = 0;
    int Erptype = 0;
    string ReturnBill = string.Empty;
    SqlTransaction TranSaction = null;

    public ImportDisProD()
    {
        ReturnBill = string.Empty;
        ErrTb = new DataTable();
        ErrTb.Columns.Add("Etype");
        ErrTb.Columns.Add("Ename");
        LogManager.LogPath2 = AppDomain.CurrentDomain.BaseDirectory + "/ERPTransFerLog/";
        LogManager.LogFielPrefix2 = "ERP_TB";
    }

    /// <summary>
    /// 带有事务获取数据源
    /// </summary>
    /// <param name="StrHead">查询的字段默认（*）</param>
    /// <param name="StrTable">要查询的表名</param>
    /// <param name="StrWhere">查询条件</param>
    /// <param name="StrOrder">排序</param>
    /// <param name="Tran">Sql事务</param>
    /// <returns></returns>
    public static DataTable GetDataSource(string StrHead, string StrTable, string StrWhere, string StrOrder, SqlTransaction Tran)
    {
        if (string.IsNullOrWhiteSpace(StrHead))
        {
            StrHead = "*";
        }
        string strSql = "select " + StrHead + " from " + StrTable + " where 1=1";
        if (!string.IsNullOrWhiteSpace(StrHead))
        {
            strSql += StrWhere;
        }
        if (!string.IsNullOrWhiteSpace(StrOrder))
        {
            strSql += " order by " + StrOrder + "";
        }
        return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran).Tables[0];
    }

    public void CreateReturnXML(string code, bool result, string err = "")
    {
        string bill = "<bill>";
        bill += "<code>" + code + "</code>";
        bill += "<result>" + result.ToString() + "</result>";
        bill += "<err>" + err + "</err>";
        bill += "</bill>";
        ReturnBill += bill;
    }

    /// <summary>
    /// 访问WebService方法
    /// </summary>
    /// <param name="Url">访问的Url</param>
    /// <param name="Coding">规定返回的编码格式（可不填）</param>
    /// <returns></returns>
    public static StreamReader VisitWebService(string Url, Encoding Coding = null)
    {
        StreamReader StReder = null;
        try
        {
            if (string.IsNullOrEmpty(Url.Trim()))
            {
                throw new Exception("URL地址不能为空");
            }
            HttpWebRequest Requst = (HttpWebRequest)HttpWebRequest.Create(Url.Trim());
            HttpWebResponse Respons = (HttpWebResponse)Requst.GetResponse();
            if (Coding != null)
                StReder = new StreamReader(Respons.GetResponseStream(), Coding);
            else
                StReder = new StreamReader(Respons.GetResponseStream());
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return StReder;
    }

    /// <summary>
    /// ERP基础档案同步
    /// </summary>
    /// <param name="TranType"></param>
    public string TransFerBasics(string XML)
    {
        string Method = "";
        try
        {
            XmlDocument Xdoc = new XmlDocument();
            Xdoc.LoadXml(XML);
            Method = Xdoc.DocumentElement.GetAttribute("method").Trim();
            LogManager.WriteLog2(LogFile.Trace.ToString(), "----------------ERP基础档案（" + SetName(Method) + "） " + DateTime.Now.ToString() + " Start----------------\r\n");
            LogManager.WriteLog2(LogFile.Trace.ToString(), "---------------" + XML + "----------------\r\n");
            initiParams(Xdoc.DocumentElement.GetAttribute("compid").Trim());
            if (!string.IsNullOrWhiteSpace(Method))
            {
                if (Method == "InsertCusClass" || Method == "InsertCus" || Method == "InsertCusAddress")
                {
                    DisTransfer(Xdoc, Method);
                }
                else if (Method == "InsertInvClass" || Method == "InsertItem" || Method == "InsertInvPrice")
                {
                    GoodsTransfer(Xdoc, Method);
                }
                else if (Method == "GetOrder")
                {
                    return TransFerOrder();
                }
                else
                {
                    throw new Exception("未找到方法：" + Method + "");
                }
            }
        }
        catch (Exception ex)
        {
            if (ex is XmlException)
            {
                ex = new Exception("XML格式错误:" + ex.Message);
            }
            CreateReturnXML("", false, ex.Message);
            LogManager.WriteLog2(LogFile.Error.ToString(), ex.Message + "\r\n" + XML);
            LogManager.WriteLog2(LogFile.Error.ToString(), "----------------" + DateTime.Now.ToString() + "----------------\r\n");
        }
        finally
        {
            LogManager.WriteLog2(LogFile.Trace.ToString(), "----------------ERP基础档案（" + SetName(Method) + "）  " + DateTime.Now.ToString() + " End----------------\r\n");
        }
        return "<ufinterface version=\"1.0\">" + ReturnBill + "</ufinterface>";
    }

    public string SetName(string Method)
    {
        switch (Method)
        {
            case "InsertCusClass": return "代理商分类";
            case "InsertCus": return "代理商";
            case "InsertCusAddress": return "代理商地址";
            case "InsertInvClass": return "商品分类";
            case "InsertItem": return "商品档案";
            case "InsertInvPrice": return "商品价格";
            case "GetOrder": return "获取订单";
            default: return "";
        }
    }

    /// <summary>
    /// 获取订单数据
    /// </summary>
    /// <param name="TranType"></param>
    public string TransFerOrder()
    {
        try
        {
            LogManager.WriteLog2(LogFile.Trace.ToString(), "----------------ERP获取订单开始：" + DateTime.Now.ToString() + "----------------");
            return GetOrder();
        }
        catch
        {

        }
        finally
        {
            LogManager.WriteLog2(LogFile.Trace.ToString(), "----------------ERP获取订单结束：" + DateTime.Now.ToString() + "----------------");
        }
        return "";
    }

    /// <summary>
    /// 订单同步ERP方法
    /// </summary>
    private string GetOrder()
    {
        string bill = "<ufinterface  version=\"1.0\">";
        try
        {
            int OrderTB = 0;
            List<Hi.Model.DIS_Order> order = new Hi.BLL.DIS_Order().GetList("", " isnull(dr,0)=0 and Compid=" + CompID + " and OState=2 ", "");
            foreach (Hi.Model.DIS_Order model in order)
            {
                try
                {
                    List<Hi.Model.DIS_OrderDetail> orderDetail = new Hi.BLL.DIS_OrderDetail().GetList("", " isnull(dr,0)=0 and vdef1<>'1' and OrderID=" + model.ID + " ", "");
                    if (orderDetail.Count > 0)
                    {
                        bill += "<bill>";
                        bill += "<header>";
                        bill += "<soid>" + model.ID + "</soid>";
                        bill += "<socode>" + model.ReceiptNo + "</socode>";
                        bill += "<date>" + model.AuditDate.ToString("yyyy-MM-dd") + "</date>";
                        List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("", " isnull(dr,0)=0 and ID=" + model.DisID + "", "");
                        if (Dis.Count > 0)
                        {
                            bill += "<customercode>" + Dis[0].DisCode + "</customercode>";
                        }
                        else
                        {
                            bill += "<customercode></customercode>";
                        }
                        List<Hi.Model.BD_DisAddr> Addr = new Hi.BLL.BD_DisAddr().GetList("", " isnull(dr,0)=0 and ID=" + model.AddrID + "", "");
                        if (Addr.Count > 0)
                        {
                            bill += "<addresscode>" + Addr[0].Code + "</addresscode>";
                        }
                        else
                        {
                            bill += "<addresscode></addresscode>";
                        }
                        bill += "<linkperson>" + model.Principal + "</linkperson>";
                        bill += "<address>" + model.Address + "</address>";
                        bill += "<memo>" + model.Remark + "</memo>";
                        bill += "</header>";
                        bill += "<body>";
                        string entry = string.Empty;
                        foreach (Hi.Model.DIS_OrderDetail MoDetail in orderDetail)
                        {
                            entry += "<entry>";
                            entry += "<sodid>" + MoDetail.ID + "</sodid>";
                            List<Hi.Model.BD_GoodsInfo> goods = new Hi.BLL.BD_GoodsInfo().GetList("", " isnull(dr,0)=0 and ID=" + MoDetail.GoodsinfoID + "", "");
                            if (goods.Count > 0)
                            {
                                entry += "<inventorycode>" + goods[0].Value10 + "</inventorycode>";
                            }
                            else
                            {
                                entry += "<inventorycode></inventorycode>";
                            }
                            entry += "<qty>" + MoDetail.GoodsNum + "</qty>";
                            entry += "<price>" + MoDetail.AuditAmount + "</price>";
                            entry += "<memo>" + MoDetail.Remark + "</memo>";
                            entry += "</entry>";
                        }
                        bill += entry + "</body>";
                        bill += "</bill>";
                        OrderTB++;
                    }
                    else
                    {
                        SetErrorTb("订单获取失败", "异常订单，无订单明细【订单ID：" + model.ID + "，订单编号：" + model.ReceiptNo + "】");
                    }
                }
                catch (Exception ex)
                {
                    if (TranSaction != null)
                    {
                        if (TranSaction.Connection != null)
                        {
                            TranSaction.Rollback();
                        }
                    }
                    SetErrorTb("订单获取失败", "" + ex.Message + "【订单ID：" + model.ID + "，订单编号：" + model.ReceiptNo + "】");
                }
            }
            LogManager.WriteLog2(LogFile.Trace.ToString(), "共获取订单：" + OrderTB + " 单");
        }
        catch (Exception ex)
        {
            ErrTb.Rows.Clear();
            SetErrorTb("订单获取全部失败", "" + ex.Message + "");
        }
        finally
        {
            bill += "</ufinterface>";
            TableWriteLog();
        }
        return bill;
    }

    ///// <summary>
    ///// ERP订单发货信息同步到本系统
    ///// </summary>
    //private void OrderOutTransFer()
    //{
    //    try
    //    {
    //        int OrderOuterTB = 0;
    //        List<Hi.Model.DIS_Order> order = new Hi.BLL.DIS_Order().GetList("", " isnull(dr,0)=0 and Compid=" + CompID + " and OState=2", "");
    //        foreach (Hi.Model.DIS_Order model in order)
    //        {
    //            try
    //            {
    //                string bill = string.Empty;
    //                bill += "<bill>";
    //                bill += "<header>";
    //                bill += "<soid>" + model.ID + "</soid>";
    //                bill += "<socode>" + model.ReceiptNo + "</socode>";
    //                bill += "</header>";
    //                bill += "</bill>";
    //                XmlDocument XDoc = ImportDisProD.GetServiceXmlStr(Enum.GetName(typeof(Enums.Erptype), Erptype), "GetDisp", "查询发货情况", bill);
    //                XmlElement elment = XDoc.DocumentElement;
    //                if (elment.GetAttribute("result") == "True")
    //                {
    //                    if (elment.HasChildNodes)
    //                    {
    //                        XmlNodeList nodeList = elment.SelectNodes("bill");
    //                        if (nodeList.Count > 0)
    //                        {
    //                            XmlNode Node = nodeList[0].SelectSingleNode("header");
    //                            if (Node != null)
    //                            {
    //                                TranSaction = DBUtility.SqlHelper.CreateStoreTranSaction();
    //                                string Outcode = Node.SelectSingleNode("dispcode").InnerText.Trim();
    //                                DateTime OutDate = Node.SelectSingleNode("date").InnerText.Trim().ToDateTime();
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    SetErrorTb("订单（发货状态查询）失败", "" + elment.GetAttribute("err") + "【订单ID：" + model.ID + "，订单编号：" + model.ReceiptNo + "】");
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                if (TranSaction != null)
    //                {
    //                    if (TranSaction.Connection != null)
    //                    {
    //                        TranSaction.Rollback();
    //                    }
    //                }
    //                SetErrorTb("订单（发货同步）失败", "" + ex.Message + "【订单ID：" + model.ID + "，订单编号：" + model.ReceiptNo + "】");
    //            }
    //        }
    //        LogManager.WriteLog2(LogFile.Trace.ToString(), "共同步订单发货：" + OrderOuterTB + "条");
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrTb.Rows.Clear();
    //        SetErrorTb("订单（发货同步）全部失败", "" + ex.Message + "");
    //    }
    //    finally
    //    {
    //        TableWriteLog();
    //    }
    //}

    /// <summary>
    /// Table表错误信息写入日志方法
    /// </summary>
    private void TableWriteLog()
    {
        try
        {
            if (ErrTb.Rows.Count > 0)
            {
                foreach (DataRow Row in ErrTb.Rows)
                {
                    LogManager.WriteLog2(LogFile.Error.ToString(), Row["Etype"].ToString() + "  " + Row["Ename"].ToString());
                }
                LogManager.WriteLog2(LogFile.Error.ToString(), "----------------" + DateTime.Now.ToString() + "----------------\r\n");
            }
            ErrTb.Rows.Clear();
        }
        catch
        {

        }
    }

    /// <summary>
    /// 初始化参数
    /// </summary>
    private void initiParams(string Compid)
    {
        List<Hi.Model.BD_Company> Comp = new Hi.BLL.BD_Company().GetList("", " isnull(dr,0)=0 and ID=" + Compid + "", "");
        if (Comp.Count > 0)
        {
            CompID = Comp[0].ID;
            Erptype = Comp[0].Erptype;
            List<Hi.Model.SYS_Users> User = new Hi.BLL.SYS_Users().GetList("", " isnull(dr,0)=0 and Type=4 and CompID=" + CompID + "", "");
            if (User.Count > 0)
            {
                UserID = User[0].ID;
            }
        }
        else
        {
            throw new Exception("数据导入失败，查找不到compid：（" + Compid + "）的企业");
        }
    }

    /// <summary>
    /// 同步代理商基本档案方法
    /// </summary>
    private void DisTransfer(XmlDocument Doc, string Method)
    {
        Regex rgx = null;
        try
        {
            int SumDisAdd = 0;
            int SumDisTB = 0;
            int SumTypeAdd = 0;
            int SumTypeTB = 0;
            int SumAddressAdd = 0;
            int SumAddressTB = 0;
            DataTable dt = null;
            int Disid = 0;
            TranSaction = DBUtility.SqlHelper.CreateStoreTranSaction();
            #region 代理商分类新增/同步方法
            if (Method == "InsertCusClass")
            {
                XmlElement elment = Doc.DocumentElement;
                XmlNodeList nodeList = Doc.DocumentElement.SelectNodes("bill");
                foreach (XmlNode nodel in nodeList)
                {
                    XmlNode nodel1 = nodel.SelectSingleNode("header");
                    string TypeCode = nodel1.SelectSingleNode("customerclasscode").InnerText.Trim();
                    string ParentCode = nodel1.SelectSingleNode("customerclassprecode").InnerText.Trim();
                    string TypeName = nodel1.SelectSingleNode("customerclassname").InnerText.Trim();
                    Hi.Model.BD_DisType type = null;
                    dt = ImportDisProD.GetDataSource("*", "BD_DisType", " and isnull(dr,0)=0 and TypeCode='" + TypeCode + "' and CompID=" + CompID + "", "", TranSaction);
                    if (dt.Rows.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(TypeName))
                        {
                            CreateReturnXML(TypeCode, false, "分类（同步）：分类名称为空");
                            SetErrorTb("分类（同步）", "分类名称为空【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(TypeCode))
                        {
                            CreateReturnXML(TypeCode, false, "分类（同步）：分类编码为空");
                            SetErrorTb("分类（同步）", "分类编码为空【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        else if ((FindTb = ImportDisProD.GetDataSource("1 name", "BD_DisType", " and isnull(dr,0)=0 and TypeName='" + TypeName + "' and Typecode<>'" + TypeCode + "' and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
                            CreateReturnXML(TypeCode, false, "分类（同步）：分类名称已存在");
                            SetErrorTb("分类（同步）", "分类名称已存在【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        type = Common.GetEntity<Hi.Model.BD_DisType>(dt);
                        type.TypeName = TypeName;
                        if (new Hi.BLL.BD_DisType().Update(type, TranSaction))
                        {
                            CreateReturnXML(TypeCode, true);
                            SumTypeTB++;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(TypeName))
                        {
                            CreateReturnXML(TypeCode, false, "分类（新增）：分类名称为空");
                            SetErrorTb("分类（新增）", "分类名称为空【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(TypeCode))
                        {
                            CreateReturnXML(TypeCode, false, "分类（新增）：分类编码为空");
                            SetErrorTb("分类（新增）", "分类编码为空【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "BD_DisType", " and isnull(dr,0)=0 and (TypeName='" + TypeName + "') and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
                            CreateReturnXML(TypeCode, false, "分类（新增）：分类名称或编码已存在");
                            SetErrorTb("分类（新增）", "分类名称或编码已存在【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(ParentCode))
                        {
                            FindTb = ImportDisProD.GetDataSource(" ID", "BD_DisType", " and isnull(dr,0)=0 and TypeCode='" + ParentCode + "' and CompID=" + CompID + "", "", TranSaction);
                            if (FindTb.Rows.Count > 0)
                            {
                                type = new Hi.Model.BD_DisType();
                                type.CreateDate = DateTime.Now;
                                type.CreateUserID = UserID;
                                type.IsEnabled = 1;
                                type.modifyuser = UserID;
                                type.TypeName = TypeName;
                                type.TypeCode = TypeCode;
                                type.ts = DateTime.Now;
                                type.CompID = CompID;
                                type.ParentId = FindTb.Rows[0]["ID"].ToString().ToInt(0);
                                if (new Hi.BLL.BD_DisType().Add(type, TranSaction) > 0)
                                {
                                    CreateReturnXML(TypeCode, true);
                                    SumTypeAdd++;
                                }

                            }
                            else
                            {
                                CreateReturnXML(TypeCode, false, "分类（新增）：查找不到父分类");
                                SetErrorTb("分类（新增）", "查找不到父分类，【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            }
                        }
                        else
                        {
                            type = new Hi.Model.BD_DisType();
                            type.CreateDate = DateTime.Now;
                            type.CreateUserID = UserID;
                            type.IsEnabled = 1;
                            type.CompID = CompID;
                            type.modifyuser = UserID;
                            type.TypeName = TypeName;
                            type.TypeCode = TypeCode;
                            type.ts = DateTime.Now;
                            type.ParentId = 0;
                            if (new Hi.BLL.BD_DisType().Add(type, TranSaction) > 0)
                            {
                                CreateReturnXML(TypeCode, true);
                                SumTypeAdd++;
                            }
                        }
                    }
                }
            #endregion
            }
            else if (Method == "InsertCus")
            {
                #region 代理商新增/同步
                XmlElement elment = Doc.DocumentElement;
                XmlNodeList nodeList = Doc.DocumentElement.SelectNodes("bill");
                foreach (XmlNode nodel in nodeList)
                {
                    XmlNode nodel1 = nodel.SelectSingleNode("header");
                    string DisCode = nodel1.SelectSingleNode("customercode").InnerText.Trim();
                    string DisName = nodel1.SelectSingleNode("customername").InnerText.Trim();
                    string TypeCode = nodel1.SelectSingleNode("customerclasscode").InnerText.Trim();
                    string Address = nodel1.SelectSingleNode("address").InnerText.Trim();
                    string Principal = nodel1.SelectSingleNode("person").InnerText.Trim();
                    string Phone = nodel1.SelectSingleNode("handphone").InnerText.Trim();
                    string Leading = nodel1.SelectSingleNode("lperson").InnerText.Trim();
                    string Tel = nodel1.SelectSingleNode("phone").InnerText.Trim();
                    string Zip = nodel1.SelectSingleNode("postcode").InnerText.Trim();
                    string Fax = nodel1.SelectSingleNode("fax").InnerText.Trim();
                    dt = ImportDisProD.GetDataSource("*", "BD_Distributor", " and isnull(dr,0)=0 and DisCode='" + DisCode + "' and CompID=" + CompID + " and AuditState=2", "", TranSaction);
                    if (dt.Rows.Count > 0)
                    {
                        rgx = new Regex(@"^0?1[0-9]{10}$");
                        if (string.IsNullOrWhiteSpace(DisName))
                        {
                            CreateReturnXML(TypeCode, false, "代理商（同步）：代理商名称为空");
                            SetErrorTb("代理商（同步）", "代理商名称为空【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(DisCode))
                        {
                            CreateReturnXML(TypeCode, false, "代理商（同步）：代理商编码为空");
                            SetErrorTb("代理商（同步）", "代理商编码为空【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if (!rgx.IsMatch(Phone))
                        {
                            //CreateReturnXML(TypeCode, false, "代理商（同步）：手机号码格式错误");
                            //SetErrorTb("代理商（同步）", "手机号码格式错误【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            //continue;
                            Phone = "";
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "BD_Distributor", " and isnull(dr,0)=0 and (DisName='" + DisName + "' and DisCode<>'" + DisCode + "') and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
                            CreateReturnXML(TypeCode, false, "代理商（同步）：代理商名称已存在");
                            SetErrorTb("代理商（同步）", "代理商名称已存在【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        Hi.Model.BD_Distributor Dis = Common.GetEntity<Hi.Model.BD_Distributor>(dt);
                        Dis.DisName = DisName;
                        if ((FindTb = ImportDisProD.GetDataSource("ID", "BD_DisType", " and isnull(dr,0)=0 and TypeCode='" + TypeCode + "' and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
                            Dis.DisTypeID = FindTb.Rows[0]["ID"].ToString().ToInt(0);
                        }
                        Dis.Address = Address;
                        Dis.Principal = Principal;
                        Dis.Leading = Leading;
                        Dis.Phone = Phone;
                        Dis.LeadingPhone = Phone;
                        Dis.Tel = Tel;
                        Dis.Zip = Zip;
                        Dis.Fax = Fax;
                        if (new Hi.BLL.BD_Distributor().Update(Dis, TranSaction))
                        {
                            CreateReturnXML(TypeCode, true);
                            SumDisTB++;
                        }
                    }
                    else
                    {
                        rgx = new Regex(@"^0?1[0-9]{10}$");
                        if (string.IsNullOrWhiteSpace(DisName))
                        {
                            CreateReturnXML(TypeCode, false, "代理商（新增）：代理商名称为空");
                            SetErrorTb("代理商（新增）", "代理商名称为空【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(DisCode))
                        {
                            CreateReturnXML(TypeCode, false, "代理商（新增）：代理商编码为空");
                            SetErrorTb("代理商（新增）", "代理商编码为空【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if (!rgx.IsMatch(Phone))
                        {
                            //CreateReturnXML(TypeCode, false, "代理商（新增）：手机号码格式错误");
                            //SetErrorTb("代理商（新增）", "手机号码格式错误【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            //continue;
                            Phone = "";
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "SYS_Users", " and isnull(dr,0)=0 and (Phone='" + Phone + "' and isnull(Phone,'')<>'')", "", TranSaction)).Rows.Count > 0)
                        {
                            CreateReturnXML(TypeCode, false, "代理商（新增）：代理商手机号码已存在");
                            SetErrorTb("代理商（新增）", "代理商手机号码已存在【代理商名称：" + DisName + "，代理商编码：" + DisCode + "，手机号码：" + Phone + "】");
                            continue;
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "BD_Distributor", " and isnull(dr,0)=0 and (DisName='" + DisName + "') and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
                            CreateReturnXML(TypeCode, false, "代理商（新增）：代理商名称/编码已存在");
                            SetErrorTb("代理商（新增）", "代理商名称/编码已存在【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        Hi.Model.BD_Distributor Dis = new Hi.Model.BD_Distributor();
                        Dis.DisName = DisName;
                        Dis.DisCode = DisCode;
                        if ((FindTb = ImportDisProD.GetDataSource("ID", "BD_DisType", " and isnull(dr,0)=0 and TypeCode='" + TypeCode + "' and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
                            Dis.DisTypeID = FindTb.Rows[0]["ID"].ToString().ToInt(0);
                        }
                        Dis.Address = Address;
                        Dis.Province = "上海市";
                        Dis.City = "市辖区";
                        Dis.Area = "徐汇区";
                        Dis.Principal = Principal;
                        Dis.Leading = Leading;
                        Dis.Phone = Phone;
                        Dis.LeadingPhone = Phone;
                        Dis.Tel = Tel;
                        Dis.Zip = Zip;
                        Dis.Fax = Fax;
                        Dis.SMID = 0;
                        Dis.IsCheck = 0;
                        Dis.CreditType = 0;
                        Dis.AuditState = 2;
                        Dis.AuditUser = UserID.ToString();
                        Dis.CompID = CompID;
                        Dis.AuditDate = DateTime.Now;
                        Dis.CreateDate = DateTime.Now;
                        Dis.CreateUserID = UserID;
                        Dis.ts = DateTime.Now;
                        Dis.modifyuser = UserID;
                        Dis.IsEnabled = 1;
                        Dis.Paypwd = Util.md5("123456");
                        Dis.Remark = "";
                        Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                        if ((Disid = new Hi.BLL.BD_Distributor().Add(Dis, TranSaction)) > 0)
                        {
                            CreateReturnXML(TypeCode, true);
                            user.UserName = Enum.GetName(typeof(Enums.Erptype), Erptype) + DisCode;
                            user.UserPwd = Util.md5("123456");
                            user.IsEnabled = 1;
                            user.AuditState = 2;
                            user.IsFirst = 0;
                            user.Type = 5;
                            user.CompID = CompID;
                            user.DisID = Disid;
                            user.AddType = 0;
                            user.Phone = Phone;
                            user.TrueName = Principal;
                            user.Tel = Tel;
                            user.RoleID = 0;
                            user.CreateDate = DateTime.Now;
                            user.CreateUserID = UserID;
                            user.ts = DateTime.Now;
                            user.modifyuser = UserID;
                            if (new Hi.BLL.SYS_Users().Add(user, TranSaction) > 0)
                            {
                                SumDisAdd++;
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region  代理商地址新增/同步
                XmlElement elment = Doc.DocumentElement;
                rgx = new Regex(@"^0?1[0-9]{10}$");
                XmlNodeList nodeList1 = Doc.DocumentElement.SelectNodes("bill");
                foreach (XmlNode nodel2 in nodeList1)
                {
                    XmlNode nodel3 = nodel2.SelectSingleNode("header");
                    string addcode = nodel3.SelectSingleNode("addresscode").InnerText.Trim();
                    string DisCode = nodel3.SelectSingleNode("customercode").InnerText.Trim();
                    string Principal = nodel3.SelectSingleNode("linkperson").InnerText.Trim();
                    string Address = nodel3.SelectSingleNode("address").InnerText.Trim();
                    string Phone = nodel3.SelectSingleNode("handphone").InnerText.Trim();
                    string Tel = nodel3.SelectSingleNode("phone").InnerText.Trim();
                    int isdefault = nodel3.SelectSingleNode("default").InnerText.Trim().ToInt(0);
                    if ((FindTb = ImportDisProD.GetDataSource("ID", "BD_Distributor", " and isnull(dr,0)=0 and DisCode='" + DisCode + "' and CompID=" + CompID + " ", "", TranSaction)).Rows.Count == 0)
                    {
                        CreateReturnXML(addcode, false, "代理商地址：查找不到代理商，代理商编码:" + DisCode + "");
                        SetErrorTb("代理商地址", "查找不到代理商【地址：" + Address + "，地址编码：" + addcode + "，代理商编码：" + DisCode + "】");
                        continue;
                    }
                    Disid = FindTb.Rows[0]["ID"].ToString().ToInt(0);
                    dt = ImportDisProD.GetDataSource("*", "BD_DisAddr", " and isnull(dr,0)=0 and Code='" + addcode + "' and Disid=" + Disid + "", "", TranSaction);
                    if (dt.Rows.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(addcode))
                        {
                            CreateReturnXML(addcode, false, "代理商地址（同步）：地址编码为空");
                            SetErrorTb("代理商地址（同步）", "地址编码为空【地址：" + Address + "，地址编码：" + addcode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(Principal))
                        {
                            //CreateReturnXML(addcode, false, "代理商地址（同步）：联系人为空");
                            //SetErrorTb("代理商地址（同步）", "联系人为空【地址：" + Address + "，地址编码：" + addcode + "】");
                            //continue;
                            Principal = "";
                        }
                        else if (string.IsNullOrWhiteSpace(Address))
                        {
                            CreateReturnXML(addcode, false, "代理商地址（同步）：地址为空");
                            SetErrorTb("代理商地址（同步）", "地址为空【地址：" + Address + "，地址编码：" + addcode + "】");
                            continue;
                        }
                        else if (!rgx.IsMatch(Phone))
                        {
                            //CreateReturnXML(addcode, false, "代理商地址（同步）：手机格式不正确");
                            //SetErrorTb("代理商地址（同步）", "手机格式不正确【地址：" + Address + "，地址编码：" + addcode + "】");
                            //continue;
                            Phone = "";
                        }
                        Hi.Model.BD_DisAddr addr = Common.GetEntity<Hi.Model.BD_DisAddr>(dt);
                        addr.Address = addr.Province + addr.City + addr.Area + Address;
                        addr.Principal = Principal;
                        addr.Phone = Phone;
                        addr.Tel = Tel;
                        addr.IsDefault = isdefault;
                        if (new Hi.BLL.BD_DisAddr().Update(addr, TranSaction))
                        {
                            CreateReturnXML(addcode, true);
                            SumAddressTB++;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(addcode))
                        {
                            CreateReturnXML(addcode, false, "代理商地址（同步）：地址编码为空");
                            SetErrorTb("代理商地址（新增）", "地址编码为空【地址：" + Address + "，地址编码：" + addcode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(Principal))
                        {
                            //CreateReturnXML(addcode, false, "代理商地址（同步）：联系人为空");
                            //SetErrorTb("代理商地址（新增）", "联系人为空【地址：" + Address + "，地址编码：" + addcode + "】");
                            //continue;
                            Principal = "";
                        }
                        else if (string.IsNullOrWhiteSpace(Address))
                        {
                            CreateReturnXML(addcode, false, "代理商地址（同步）：地址为空");
                            SetErrorTb("代理商地址（新增）", "地址为空【地址：" + Address + "，地址编码：" + addcode + "】");
                            continue;
                        }
                        else if (!rgx.IsMatch(Phone))
                        {
                            //CreateReturnXML(addcode, false, "代理商地址（同步）：手机格式不正确");
                            //SetErrorTb("代理商地址（新增）", "手机格式不正确【地址：" + Address + "，地址编码：" + addcode + "】");
                            //continue;
                            Phone = "";
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "BD_DisAddr", " and isnull(dr,0)=0 and (Code='" + addcode + "') and Disid=" + Disid + "", "", TranSaction)).Rows.Count > 0)
                        {
                            CreateReturnXML(addcode, false, "代理商地址（同步）：地址编码已存在");
                            SetErrorTb("代理商地址（新增）", "地址编码已存在【地址：" + Address + "，地址编码：" + addcode + "】");
                            continue;
                        }
                        Hi.Model.BD_DisAddr addr = new Hi.Model.BD_DisAddr();
                        addr.DisID = Disid;
                        addr.Province = "上海市";
                        addr.City = "市辖区";
                        addr.Area = "徐汇区";
                        addr.Code = addcode;
                        addr.Address = addr.Province + addr.City + addr.Area + Address;
                        addr.Phone = Phone;
                        addr.Principal = Principal;
                        addr.Tel = Tel;
                        addr.IsDefault = isdefault;
                        addr.CreateDate = DateTime.Now;
                        addr.CreateUserID = UserID;
                        addr.ts = DateTime.Now;
                        addr.modifyuser = UserID;
                        if (new Hi.BLL.BD_DisAddr().Add(addr, TranSaction) > 0)
                        {
                            CreateReturnXML(addcode, true);
                            SumAddressAdd++;
                        }
                    }
                }
                #endregion
            }
            TranSaction.Commit();
            LogManager.WriteLog2(LogFile.Trace.ToString(), "新增（代理商：" + SumDisAdd + "条；类别：" + SumTypeAdd + "条；地址：" + SumAddressAdd + "条），同步（代理商：" + SumDisTB + "条；类别：" + SumTypeTB + "条；地址：" + SumAddressTB + "条）");
        }
        catch (Exception ex)
        {
            ReturnBill = "";
            ErrTb.Rows.Clear();
            if (ex is XmlException)
            {
                CreateReturnXML("", false, "读取XML出现错误");
                SetErrorTb("", "读取XML出现错误");
            }
            else if (ex is XPathException)
            {
                CreateReturnXML("", false, "格式错误");
                SetErrorTb("", "XML格式错误");
            }
            else
            {
                CreateReturnXML("", false, ex.Message);
                SetErrorTb("", "同步失败：" + ex.Message + "");
            }
            if (TranSaction != null)
            {
                if (TranSaction.Connection != null)
                {
                    TranSaction.Rollback();
                }
            }
        }
        finally
        {
            if (TranSaction != null)
            {
                if (TranSaction.Connection != null)
                {
                    TranSaction.Rollback();
                }
            }
            TableWriteLog();
        }
    }

    /// <summary>
    /// 同步商品基本档案方法
    /// </summary>
    private void GoodsTransfer(XmlDocument xml, string Method)
    {
        int GoodsAdd = 0;
        int GoodsTB = 0;
        int CategoryAdd = 0;
        int CategoryTB = 0;
        int PayAdd = 0;

        try
        {
            #region 商品分类新增/同步方法
            XmlElement elment = xml.DocumentElement;
            TranSaction = DBUtility.SqlHelper.CreateStoreTranSaction();
            XmlNodeList xmllist = xml.DocumentElement.SelectNodes("bill");
            if (Method == "InsertInvClass")
            {
                foreach (XmlNode xmlbill in xmllist)
                {
                    XmlNode xmlheader = xmlbill.SelectSingleNode("header");
                    string goodsflcode = xmlheader.SelectSingleNode("inventoryclasscode").InnerText;
                    string goodsflname = xmlheader.SelectSingleNode("inventoryclassname").InnerText;
                    string goodsflprecode = xmlheader.SelectSingleNode("inventoryclassprecode").InnerText;//父

                    Hi.Model.BD_GoodsCategory goodsfl = null;

                    DataTable dt = ImportDisProD.GetDataSource("", "BD_GoodsCategory", "and CategoryCode='" + goodsflcode + "' and compid=" + CompID, "", TranSaction);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt2 = ImportDisProD.GetDataSource("", "BD_GoodsCategory", "and Categoryname='" + goodsflname + "' and CategoryCode<>'" + goodsflcode + "' and compid=" + CompID, "", TranSaction);
                        if (dt2.Rows.Count > 0)
                        {
                            SetErrorTb("商品分类（同步）", "分类名称已存在。分类编码：" + goodsflcode);
                            CreateReturnXML(goodsflcode, false, "分类名称不能相同");
                            continue;
                        }

                        if (string.IsNullOrEmpty(goodsflname))
                        {
                            SetErrorTb("商品分类（同步）", "分类名称不能为空。分类编码：" + goodsflcode);
                            CreateReturnXML(goodsflcode, false, "分类名称不能为空");
                            continue;
                        }

                        goodsfl = Common.GetEntity<Hi.Model.BD_GoodsCategory>(dt);
                        goodsfl.CategoryName = goodsflname;
                        if (goodsflprecode != "")
                        {
                            DataTable dt1 = ImportDisProD.GetDataSource("id", "BD_GoodsCategory", "and CategoryCode='" + goodsflprecode + "' and compid=" + CompID, "", TranSaction);
                            if (dt1.Rows.Count > 0)
                            {
                                goodsfl.ParentId = int.Parse(dt1.Rows[0]["id"].ToString());
                            }
                            else
                            {
                                CreateReturnXML(goodsflcode, false, "父类不存在");
                                SetErrorTb("商品分类（同步）", "父分类不存在：分类编码：" + goodsflcode);
                            }
                        }
                        else
                        {
                            goodsfl.ParentId = 0;
                        }
                        goodsfl.ts = DateTime.Now;
                        goodsfl.modifyuser = UserID;
                        if (new Hi.BLL.BD_GoodsCategory().Update(goodsfl, TranSaction))
                        {
                            CreateReturnXML(goodsflcode, true);
                            CategoryTB++;
                        }

                    }
                    else
                    {
                        DataTable dt2 = ImportDisProD.GetDataSource("", "BD_GoodsCategory", "and Categoryname='" + goodsflname + "' and compid=" + CompID, "", TranSaction);
                        if (dt2.Rows.Count > 0)
                        {
                            SetErrorTb("商品分类（新增）", "分类名称已存在。分类编码：" + goodsflcode);
                            CreateReturnXML(goodsflcode, false, "分类名称不能相同");
                            continue;
                        }
                        if (string.IsNullOrEmpty(goodsflname))
                        {
                            SetErrorTb("商品分类（新增）", "分类名称不能为空。分类编码：" + goodsflcode);
                            CreateReturnXML(goodsflcode, false, "分类名称不能为空");
                            continue;
                        }
                        goodsfl = new Hi.Model.BD_GoodsCategory();
                        goodsfl.CompID = CompID;
                        goodsfl.GoodsTypeID = 17;
                        goodsfl.CategoryCode = goodsflcode;
                        goodsfl.CategoryName = goodsflname;
                        if (goodsflprecode != "")
                        {
                            DataTable dt1 = ImportDisProD.GetDataSource("id", "BD_GoodsCategory", "and CategoryCode='" + goodsflprecode + "' and compid=" + CompID, "", TranSaction);
                            if (dt1.Rows.Count > 0)
                            {
                                goodsfl.ParentId = int.Parse(dt1.Rows[0]["id"].ToString());
                            }
                            else
                            {
                                CreateReturnXML(goodsflcode, false, "父类不存在");
                                SetErrorTb("商品分类（新增）", "父分类不存在：分类编码：" + goodsflcode);
                            }
                        }
                        else
                        {
                            goodsfl.ParentId = 0;
                        }
                        goodsfl.IsEnabled = 1;
                        goodsfl.CreateUserID = UserID;
                        goodsfl.CreateDate = DateTime.Now;
                        goodsfl.ts = DateTime.Now;
                        goodsfl.dr = 0;
                        goodsfl.modifyuser = UserID;
                        if (new Hi.BLL.BD_GoodsCategory().Add(goodsfl, TranSaction) > 0)
                        {
                            CreateReturnXML(goodsflcode, true);
                            CategoryAdd++;
                        }
                    }
                }
            }
            #endregion

            #region 商品基本信息、商品详情新增/同步

            else if (Method == "InsertItem")
            {
                List<Hi.Model.BD_Goods> goodsList = new List<Hi.Model.BD_Goods>();
                foreach (XmlNode xmlbill in xmllist)
                {
                    XmlNode xmlheader = xmlbill.SelectSingleNode("header");
                    string goodscode = xmlheader.SelectSingleNode("inventorycode").InnerText;
                    string goodsname = xmlheader.SelectSingleNode("customername").InnerText;
                    string goodstypecode = xmlheader.SelectSingleNode("inventoryclasscode").InnerText;
                    string unit = xmlheader.SelectSingleNode("unit").InnerText;
                    string startdate = xmlheader.SelectSingleNode("startdate").InnerText;
                    string enddate = xmlheader.SelectSingleNode("enddate").InnerText;
                    Hi.Model.BD_Goods goods = null;
                    DataTable dt = ImportDisProD.GetDataSource("", "BD_Goods", " and goodscode='" + goodscode + "' and compid=" + CompID, "", TranSaction);
                    if (dt.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(goodsname))
                        {
                            CreateReturnXML(goodscode, false, "商品名称不能为空");
                            SetErrorTb("商品（同步）", "商品名称不能为空。产品编码：" + goodscode);
                            continue;
                        }
                        else
                            if (string.IsNullOrEmpty(unit))
                            {
                                CreateReturnXML(goodscode, false, "商品单位不能为空");
                                SetErrorTb("商品（同步）", "商品单位不能为空。产品编码：" + goodscode);
                                continue;
                            }
                        DataTable dt1 = ImportDisProD.GetDataSource("top 1 id", "BD_GoodsCategory", "and CategoryCode='" + goodstypecode + "' and compid=" + CompID, "", TranSaction);
                        goods = Common.GetEntity<Hi.Model.BD_Goods>(dt);
                        goods.GoodsName = goodsname;
                        if (dt1.Rows.Count > 0)
                        {
                            goods.CategoryID = (int)dt1.Rows[0]["id"];
                        }
                        else
                        {
                            goods.CategoryID = 0;
                        }
                        goods.Unit = unit;
                        if (!string.IsNullOrEmpty(startdate))
                        {
                            goods.OfflineStateDate = DateTime.Parse(startdate);
                        }
                        if (!string.IsNullOrEmpty(enddate))
                        {
                            goods.OfflineEndDate = DateTime.Parse(enddate);
                        }
                        if (DateTime.Now >= goods.OfflineStateDate && DateTime.Now <= goods.OfflineEndDate)
                        {
                            goods.IsOffline = 1;
                        }
                        else
                        {
                            goods.IsOffline = 0;
                        }
                        goods.ts = DateTime.Now;
                        goods.modifyuser = UserID;
                        if (new Hi.BLL.BD_Goods().Update(goods, TranSaction))
                        {
                            Hi.Model.BD_GoodsInfo goodsinfo;
                            DataTable dt2 = ImportDisProD.GetDataSource("top 1 id", "BD_GoodsInfo", "and Value10='" + goods.GoodsCode + "' and compid=" + CompID, "", TranSaction);
                            if (dt2.Rows.Count == 0)
                            {
                                goodsinfo = null;
                                goodsinfo.CompID = CompID;
                                goodsinfo.GoodsID = goods.ID;
                                goodsinfo.SalePrice = 0;
                                goodsinfo.TinkerPrice = 0;
                                goodsinfo.IsEnabled = true;
                                goodsinfo.CreateDate = DateTime.Now;
                                goodsinfo.CreateUserID = UserID;
                                goodsinfo.ts = DateTime.Now;
                                goodsinfo.modifyuser = UserID;
                                if (new Hi.BLL.BD_GoodsInfo().Add(goodsinfo, TranSaction) > 0)
                                {
                                    CreateReturnXML(goodscode, true);
                                    GoodsTB++;
                                }
                            }
                            CreateReturnXML(goodscode, true);
                            GoodsTB++;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(goodsname))
                        {
                            CreateReturnXML(goodscode, false, "商品单位不能为空");
                            SetErrorTb("商品（添加）", "商品名称不能为空。产品编码：" + goodscode);
                            continue;
                        }
                        else
                            if (string.IsNullOrEmpty(unit))
                            {
                                CreateReturnXML(goodscode, false, "商品单位不能为空");
                                SetErrorTb("商品（添加）", "商品单位不能为空。产品编码：" + unit);
                                continue;
                            }
                        DataTable dt1 = ImportDisProD.GetDataSource("top 1 id", "BD_GoodsCategory", "and CategoryCode='" + goodstypecode + "' and compid=" + CompID, "", TranSaction);
                        goods = new Hi.Model.BD_Goods();
                        if (dt1.Rows.Count > 0)
                        {
                            goods.CategoryID = (int)dt1.Rows[0]["id"];
                        }
                        else
                        {
                            goods.CategoryID = 0;
                        }
                        goods.CompID = CompID;
                        goods.GoodsName = goodsname;
                        goods.GoodsCode = goodscode;
                        goods.Unit = unit;
                        if (startdate != "" && enddate != "")
                        {
                            goods.OfflineStateDate = DateTime.Parse(startdate);
                            goods.OfflineEndDate = DateTime.Parse(enddate);
                            if (DateTime.Now >= goods.OfflineStateDate && DateTime.Now <= goods.OfflineEndDate)
                            {
                                goods.IsOffline = 1;
                            }
                            else
                            {
                                goods.IsOffline = 0;
                            }
                        }
                        else
                        {
                            goods.IsOffline = 1;
                        }
                        goods.IsIndex = 0;
                        goods.IsSale = 0;
                        goods.IsRecommended = 1;
                        goods.IsEnabled = 1;
                        goods.CreateUserID = UserID;
                        goods.CreateDate = DateTime.Now;
                        goods.ts = DateTime.Now;
                        goods.modifyuser = UserID;

                        goods.SalePrice = 0;
                        int goodsid = new Hi.BLL.BD_Goods().Add(goods, TranSaction);
                        if (goodsid > 0)
                        {
                            Hi.Model.BD_GoodsInfo goodsinfo = new Hi.Model.BD_GoodsInfo();
                            goodsinfo.CompID = CompID;
                            goodsinfo.GoodsID = goodsid;
                            goodsinfo.Value10 = goods.GoodsCode;
                            goodsinfo.SalePrice = 0;
                            goodsinfo.TinkerPrice = 0;
                            goodsinfo.IsEnabled = true;
                            goodsinfo.CreateDate = DateTime.Now;
                            goodsinfo.CreateUserID = UserID;
                            goodsinfo.ts = DateTime.Now;
                            goodsinfo.modifyuser = UserID;
                            if (new Hi.BLL.BD_GoodsInfo().Add(goodsinfo, TranSaction) > 0)
                            {
                                CreateReturnXML(goodscode, true);
                                GoodsAdd++;
                            }
                        }
                    }
                    goodsList.Add(goods);
                }
                //初始化商品
                new Common().InitialGoods(goodsList);
            }
            else
            {
                foreach (XmlNode xmlbill in xmllist)
                {
                    XmlNode xmlheader = xmlbill.SelectSingleNode("header");
                    string goodscode = xmlheader.SelectSingleNode("inventorycode").InnerText;
                    string pay = xmlheader.SelectSingleNode("price").InnerText;
                    Hi.Model.BD_Goods goods;
                    Hi.Model.BD_GoodsInfo goodsinfo;
                    DataTable dt = ImportDisProD.GetDataSource("", "BD_Goods", " and goodscode='" + goodscode + "' and compid=" + CompID, "", TranSaction);
                    DataTable dt1 = ImportDisProD.GetDataSource("", "BD_GoodsInfo", " and Value10='" + goodscode + "' and compid=" + CompID, "", TranSaction);
                    if (dt.Rows.Count > 0)
                    {
                        goods = Common.GetEntity<Hi.Model.BD_Goods>(dt);
                        goods.SalePrice = pay.ToDecimal(0);
                        if (new Hi.BLL.BD_Goods().Update(goods))
                        {
                            if (dt1.Rows.Count > 0)
                            {
                                goodsinfo = Common.GetEntity<Hi.Model.BD_GoodsInfo>(dt1);
                                goodsinfo.SalePrice = pay.ToDecimal(0);
                                goodsinfo.TinkerPrice = pay.ToDecimal(0);
                                if (new Hi.BLL.BD_GoodsInfo().Update(goodsinfo, TranSaction))
                                {
                                    CreateReturnXML(goodscode, true);
                                    PayAdd++;
                                }
                            }
                            else
                            {
                                CreateReturnXML(goodscode, false, "未查找到商品信息");
                                SetErrorTb("商品价格（同步）", "未查找到商品信息");
                                continue;
                            }
                        }
                    }
                    else
                    {
                        CreateReturnXML(goodscode, false, "未查找到商品档案：商品编码：" + goodscode + "");
                        SetErrorTb("商品价格（同步）", "未查找到商品档案：商品编码：" + goodscode + "");
                        continue;
                    }
                }
            }
            TranSaction.Commit();
            #endregion
        }
        catch (Exception ex)
        {
            ReturnBill = "";
            ErrTb.Rows.Clear();
            if (ex is XmlException)
            {
                CreateReturnXML("", false, "读取XML出现错误");
                SetErrorTb("", "读取XML出现错误");
            }
            else if (ex is XPathException)
            {
                CreateReturnXML("", false, "XML格式错误");
                SetErrorTb("", "XML格式错误");
            }
            else
            {
                CreateReturnXML("", false, ex.Message);
                SetErrorTb("", "同步失败：" + ex.Message + "");
            }
            if (TranSaction != null)
            {
                if (TranSaction.Connection != null)
                {
                    TranSaction.Rollback();
                }
            }
        }
        finally
        {
            if (TranSaction != null)
            {
                if (TranSaction.Connection != null)
                {
                    TranSaction.Rollback();
                }
            }
            LogManager.WriteLog2(LogFile.Trace.ToString(), "新增（商品、商品详情：" + GoodsAdd + "条；商品类别：" + CategoryAdd + "条；），同步（商品、商品详情：" + GoodsTB + "条；商品类别：" + CategoryTB + "条；价格：" + PayAdd + "条");
            TableWriteLog();
        }
    }

    public void SetErrorTb(string type, string name)
    {
        row = ErrTb.NewRow();
        row["Etype"] = type;
        row["Ename"] = name;
        ErrTb.Rows.Add(row);
    }
}