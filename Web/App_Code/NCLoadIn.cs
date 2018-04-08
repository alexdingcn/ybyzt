using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Xml.XPath;

/// <summary>
///NCLoadIN 的摘要说明
/// </summary>
public class NCLoadIN
{
    DataTable ErrTb = null; //出错提示
    DataTable FindTb = new DataTable();
    string ReturnBill = string.Empty;
    SqlTransaction TranSaction = null;
    DataRow row = null;
    int CompID = 0;
    int UserID = 0;
    int Erptype = 0;

    public NCLoadIN()
    {
        ReturnBill = string.Empty;
        ErrTb = new DataTable();
        ErrTb.Columns.Add("Etype");
        ErrTb.Columns.Add("Ename");
        LogManager.LogPath2 = AppDomain.CurrentDomain.BaseDirectory + "/ERPTransFerLog/";
        LogManager.LogFielPrefix2 = "ERP_TB";
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
                if (Method == "InsertCusClass" || Method == "InsertDisArea" || Method == "InsertCus")
                {
                    DisTransfer(Xdoc, Method);
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

    /// <summary>
    /// 同步代理商基本档案方法
    /// </summary>
    public void DisTransfer(XmlDocument Doc, string Method)
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
            
            if (Method == "InsertCusClass")
            {
                #region 代理商分类新增/同步方法

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
            else if (Method == "InsertDisArea")
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
                    string TypeCode = IsSelectNode(nodel1, "customerclasscode") ? nodel1.SelectSingleNode("customerclasscode").InnerText.Trim() : "";
                    string Address = IsSelectNode(nodel1, "address") ? nodel1.SelectSingleNode("address").InnerText.Trim() : "";
                    string Principal = IsSelectNode(nodel1, "person") ? nodel1.SelectSingleNode("person").InnerText.Trim() : "";
                    string Phone = IsSelectNode(nodel1, "handphone") ? nodel1.SelectSingleNode("handphone").InnerText.Trim() : "";
                    string Leading = IsSelectNode(nodel1, "lperson") ? nodel1.SelectSingleNode("lperson").InnerText.Trim() : "";
                    string Tel = IsSelectNode(nodel1, "phone") ? nodel1.SelectSingleNode("phone").InnerText.Trim() : "";
                    string Zip = IsSelectNode(nodel1, "postcode") ? nodel1.SelectSingleNode("postcode").InnerText.Trim() : "";
                    string Fax = IsSelectNode(nodel1, "fax") ? nodel1.SelectSingleNode("fax").InnerText.Trim() : "";
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
                ReturnBill = "";
                ErrTb.Rows.Clear();
                CreateReturnXML("", false, "导入类型异常");
                SetErrorTb("", "导入类型异常");
                return;
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
    /// NC获取订单数据
    /// </summary>
    /// <param name="TranType"></param>
    private string TransNCOrder(string compID, string startDate = null, string endDate = null)
    {
        try
        {
            LogManager.WriteLog2(LogFile.Trace.ToString(), "----------------ERP获取订单开始：" + DateTime.Now.ToString() + "----------------");
            return GetNCOrder(compID, startDate, endDate);
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
    private string GetNCOrder(string compID, string startDate = null, string endDate = null)
    {
        string bill = "<?xml version=\"1.0\" encoding=\"gb2312\"?>";
        try
        {
            int OrderTB = 0;
            string strWhere = " isnull(dr,0)=0 and Compid=" + compID + " and OState<>0 and OState<>1 and OType<>9";
            if (startDate != "")
                strWhere += "and ts >='" + Convert.ToDateTime(startDate) + "'";
            if (endDate != "")
                strWhere += "and ts <='" + Convert.ToDateTime(endDate) + "'";
            List<Hi.Model.DIS_Order> order = new Hi.BLL.DIS_Order().GetList("", strWhere, "");
            bill += "<ufinterface roottag=\"so_order\" billtype=\"30\"  replace=\"Y\" receiver=\"\" sender=\"30\" isexchange=\"Y\" filename=\"销售订单头.xml\" proc=\"add\" operation=\"req\">";
            if (Convert.ToDateTime(startDate) > Convert.ToDateTime(endDate))
            {
                bill += "同步日期异常</ufinterface>";
                return bill;
            }
            foreach (Hi.Model.DIS_Order model in order)
            {
                try
                {
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(model.CompID);
                    if (comp == null)
                    {
                        bill += "企业异常</ufinterface>";
                        return bill;
                    }
                    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(model.CompID);
                    if (dis == null)
                    {
                        bill += "代理商异常</ufinterface>";
                        return bill;
                    }

                    List<Hi.Model.DIS_OrderDetail> orderDetail = new Hi.BLL.DIS_OrderDetail().GetList("", " isnull(dr,0)=0 and OrderID=" + model.ID + " ", "");
                    if (orderDetail.Count > 0)
                    {
                        OrderTB++;
                        bill += "<so_order id=\"" + model.ID + "\" subdoctype=\"\">";
                        bill += "<so_order_head>";

                        bill += "<pk_corp>" + comp.CompCode + "</pk_corp>";//公司PK,不能为空，参照基础数据"公司目录".暂时写死
                        bill += "<vreceiptcode>" + model.ReceiptNo + "</vreceiptcode>";//单据号,可空字段, 如果为空保存时系统自动获取
                        bill += "<creceipttype>30</creceipttype>";//单据类型,不能为空
                        bill += "<cbiztype>普通销售</cbiztype>";
                        bill += "<ccustomerid>" + dis.DisName + "</ccustomerid>";
                        bill += "<dbilldate>" + model.AuditDate.ToString("yyyy-MM-dd") + "</dbilldate>";//单据日期,不能为空,YYYY-MM-DD
                        bill += "<vreceiptcode>" + model.DisID + "</vreceiptcode>";//客商管理档案ID,不能为空（需要参照客商管理档案ID）
                        bill += "<cdeptid>999</cdeptid>";//部门档案ID,不能为空（需要参照部门档案基础数据
                        bill += "<coperatorid>" + model.DisUserID + "</coperatorid>";//制单人:人员档案ID,不能为空（需要参照人员档案基础数据）。填写收货方
                        bill += "<csalecorpid>999</csalecorpid>";//销售组织ID,不能为空（需要参照'销售组织'档案基础数据）
                        bill += "<ccalbodyid>999</ccalbodyid>";//库存组织ID,不能为空（需要参照库存组织档案基础数据
                        bill += "<creceiptcustomerid>" + dis.DisName + "</creceiptcustomerid>";//收货单位客商管理档案ID,不能为空（需要参照客商管理档案基础数据）                      
                        bill += "<vreceiveaddress>" + model.Address + "</vreceiveaddress>";//收货地址字符串,可以为空
                        bill += "<creceiptcorpid>" + dis.DisName + "</creceiptcorpid>";//开票单位客商管理档案ID,不能为空（需要参照客商管理档案基础数据）
                        bill += "<ndiscountrate>0</ndiscountrate>";//<!--整单折扣,不能为空-->
                        bill += "<bfreecustflag>N</bfreecustflag>";//<!--是否散户,默认为N-->
                        bill += "<ibalanceflag>0</ibalanceflag>";//<!--结算标志,默认为N-->
                        bill += "<dmakedate>" + model.CreateDate.ToString("yyyy-MM-dd") + "</dmakedate>";//<!--制单日期,不能为空,YYYY-MM-DD-->
                        bill += "<capproveid>" + model.AuditUserID + "</capproveid>";//<!--审批人:人员档案ID,为空（需要参照人员档案基础数据）-->
                        bill += "<dapprovedate>" + model.AuditDate.ToString("yyyy-MM-dd") + "</dapprovedate>";//<!--审批日期,为空,YYYY-MM-DD-->			

                        string type = OrderInfoType.OState(model.ID);
                        string state = "0";
                        //<!--状态,不能为空:0无状态,1自由,2审批,3冻结,4关闭,5作废,6结束,7正在审批,8审批未通过-->
                        switch (type)
                        {
                            case "":
                                state = "";
                                break;
                            default:
                                state = "0";
                                break;
                        }
                        bill += "<fstatus>" + state + "</fstatus><bretinvflag>";
                        bill += type == "已退货" ? "Y" : "N" + "</bretinvflag><boutendflag>";//<!--退货标记,默认为N-->
                        bill += type == "已发货" ? "Y" : "N" + "</boutendflag>";//<!--出库结束标记,默认为N-->
                        bill += "<binvoicendflag>N</binvoicendflag>";//<!--开票结束标记,默认为N-->
                        bill += "<breceiptendflag>N</breceiptendflag>";//<!--发货结束标记,默认为N-->                        ;
                        bill += "<bpayendflag>N</bpayendflag>";//<!--付款结束标记,默认为N-->			
                        bill += "</so_order_head>";
                        bill += "<so_order_body>";
                        string entry = string.Empty;
                        foreach (Hi.Model.DIS_OrderDetail MoDetail in orderDetail)
                        {
                            entry += "<entry>";
                            entry += "<pk_corp>" + comp.CompCode + "</pk_corp>";//<!--公司主键,参照基础数据"公司目录"-->                       
                            entry += "<cinventoryid>999</cinventoryid>";//<!--存货管理档案主键,不能为空，需要参照基础数据"存货档案"-->
                            List<Hi.Model.BD_GoodsInfo> goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("", " isnull(dr,0)=0 and ID=" + MoDetail.GoodsinfoID + "", "");
                            if (goodsInfo.Count > 0)
                            {
                                Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo[0].GoodsID);

                                if (goods != null)
                                    entry += "<cunitid>" + goods.Unit + "</cunitid>";//<!--主计量单位,不能为空，需要参照基础数据"计量档案"-->
                                else
                                    entry += "<cunitid>999</cunitid>";
                            }
                            entry += "<nnumber>" + MoDetail.GoodsNum + "</nnumber>";

                            DateTime dt = model.AuditDate;
                            if (type == "已发货")
                            {
                                List<Hi.Model.DIS_OrderOut> list = new Hi.BLL.DIS_OrderOut().GetList("", " isnull(dr,0)=0 and OrderID=" + model.ID + "", "");
                                if (list.Count > 0)
                                {
                                    dt = list[0].SendDate;
                                }
                            }
                            entry += "<dconsigndate>" + model.AuditDate + "</dconsigndate>";//<!--发货日期,不能为空,YYYY-MM-DD-->
                            entry += "<ddeliverdate>" + dt.AddDays(15) + "</ddeliverdate>";//<!--交货日期,不能为空,YYYY-MM-DD-->
                            entry += "<ccurrencytypeid>RMB</ccurrencytypeid>";//<!--原币,不能为空,参考币种档案-->           
                            entry += "<nitemdiscountrate>100</nitemdiscountrate>";//<!--单品折扣率,不能为空, 默认100--> 
                            entry += "<ndiscountrate>100</ndiscountrate>";//<!--整单折扣,不能为空, 默认100-->
                            entry += "<nexchangeotobrate>1.0</nexchangeotobrate>";//<!--折本汇率,不能为空-->
                            entry += "<ntaxrate>17</ntaxrate>";// <!--税率,不能为空-->
                            entry += "<noriginalcurprice>10.27</noriginalcurprice>";//<!--原币无税单价,不能为空-->
                            entry += "<noriginalcurtaxprice>12</noriginalcurtaxprice>";//<!--原币含税单价,不能为空-->
                            entry += "<noriginalcurnetprice>10.26</noriginalcurnetprice>";//<!--原币无税净价,不能为空-->
                            entry += "<noriginalcurtaxnetprice>12</noriginalcurtaxnetprice>";//<!--原币含税净价,不能为空-->
                            entry += "<noriginalcurtaxmny>172.62</noriginalcurtaxmny>";//<!--原币税额,不能为空-->
                            entry += "<noriginalcurmny>1015.38</noriginalcurmny>";//<!--原币无税金额,不能为空-->
                            entry += "<noriginalcursummny>1188</noriginalcursummny>";//<!--原币价税合计,不能为空-->
                            entry += "<noriginalcurdiscountmny>0</noriginalcurdiscountmny>";//<!--原币折扣额,不能为空-->
                            entry += "<nprice>10.27</nprice>";//<!--本币无税单价,不能为空-->
                            entry += "<ntaxprice>12</ntaxprice>";//<!--本币含税单价,不能为空-->
                            entry += "<nnetprice>10.26</nnetprice>";//<!--本币无税净价,不能为空-->
                            entry += "<ntaxnetprice>12</ntaxnetprice>";//<!--本币含税净价,不能为空-->
                            entry += "<ntaxmny>172.62</ntaxmny>";// <!--本币税额,不能为空-->			
                            entry += "<nmny>1015.38</nmny>";//<!--本币无税金额,不能为空-->
                            entry += "<nsummny>1188</nsummny>";//<!--本币价税合计,不能为空-->
                            entry += "<ndiscountmny>1</ndiscountmny>";//<!--本币折扣额,不能为空-->
                            entry += "<coperatorid>" + model.DisUserID + "</coperatorid>";//<!--制单人:人员档案ID,不能为空-->
                            entry += "<frowstatus>" + type + "</frowstatus>";//<!--行状态,不能为空:0无状态,1自由,2审批,3冻结,4关闭,5作废,6结束,7正在审批,8审批未通过-->
                            entry += "<cadvisecalbodyid>999</cadvisecalbodyid>";//<!--建议发货库存组织,不能为空（参照库存组织档案）-->
                            entry += "<creceiptareaid>999</creceiptareaid>";//<!--收货地区,不能为空（参照子表的收货单位所属地区的档案基础数据）-->
                            entry += "<vreceiveaddress>999</vreceiveaddress>";//<!--收货地址,不能为空(参照子表的收货单位的默认收货地址字符串-->
                            entry += "<creceiptcorpid>999</creceiptcorpid>";//<!--收货单位,不能为空（需要参照客商管理档案基础数据）-->
                            entry += "<crowno>" + OrderTB + "</crowno>";//<!--行号,不能为空，第一行为1，第二行为2，依此类推-->
                            entry += "</entry>";
                        }
                        bill += entry + "</so_order_body>";
                        bill += "</so_order>";
                    }
                    else
                    {
                        SetErrorTb("订单获取失败", "异常订单，无订单明细【订单ID：" + model.ID + "，订单编号：" + model.ReceiptNo + "】");
                        bill += "<error>订单：" + model.ReceiptNo + "异常。</error>";
                    }
                }
                catch (Exception ex)
                {
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
                    SetErrorTb("订单获取失败", "" + ex.Message + "【订单ID：" + model.ID + "，订单编号：" + model.ReceiptNo + "】");
                }
            }
            LogManager.WriteLog2(LogFile.Trace.ToString(), "共获取订单：" + OrderTB + " 单");
        }
        catch (Exception ex)
        {
            ErrTb.Rows.Clear();
            SetErrorTb("订单获取全部失败", "" + ex.Message + "");
            bill = "<?xml version=\"1.0\" encoding=\"gb2312\"?>";
            bill += "<ufinterface roottag=\"so_order\" billtype=\"30\"  replace=\"Y\" receiver=\"\" sender=\"yj\" isexchange=\"Y\" filename=\"销售订单头.xml\" proc=\"add\" operation=\"req\">";
            bill += "error";
        }
        finally
        {
            bill += "</ufinterface>";
            TableWriteLog();
        }
        return bill;
    }

    public string SetName(string Method)
    {
        switch (Method)
        {
            case "InsertCusClass": return "代理商分类";
            case "InsertDisArea": return "代理商区域";
            default: return "";
        }
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

    public void SetErrorTb(string type, string name)
    {
        row = ErrTb.NewRow();
        row["Etype"] = type;
        row["Ename"] = name;
        ErrTb.Rows.Add(row);
    }

    private static bool IsSelectNode(XmlNode xmlPath, string node)
    {
        System.Xml.XmlNode Xnode = xmlPath.SelectSingleNode(node); //key节点
        if (Xnode == null)
        {
            return false;
        }
        return true;
    }
}