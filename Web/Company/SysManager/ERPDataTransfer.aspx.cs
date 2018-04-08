using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataExtraction;
using System.Xml;
using System.Xml.XPath;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

public partial class Company_SysManager_ERPDataTransfer : CompPageBase
{
    DataTable ErrTb = new DataTable(); //出错提示
    DataTable FindTb = new DataTable();
    DataRow row = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Erptype == (int)Enums.Erptype.平台企业)
        {
            Response.Redirect("../../NoOperable.aspx", true);
            Response.End();
        }
        else
        {
            lblDisIpResult.InnerHtml = "";
            lblGoodsIpResult.InnerHtml = "";
            ErrTb.Columns.Add("Etype");
            ErrTb.Columns.Add("Ename");
            DivError.Visible = false;
            Rpt_Error.DataSource = null;
            Rpt_Error.DataBind();
        }
    }


    protected void btnDisTransfer_Click(object sender, EventArgs e)
    {
        SqlTransaction TranSaction = null;
        Regex rgx = null;
        string ShowStr = "<i style='color:red;'>{0}</i>";
        try
        {
            int SumDisAdd = 0;
            int SumDisTB = 0;
            int SumTypeAdd = 0;
            int SumTypeTB = 0;
            int SumAddressAdd = 0;
            int SumAddressTB = 0;
            #region 代理商分类新增/同步方法
            XmlDocument Xdoc = null;//ImportDisProD.GetServiceXmlStr(Enum.GetName(typeof(Enums.Erptype), Erptype), "GetCusClass", "获取代理商分类");
            TranSaction = DBUtility.SqlHelper.CreateStoreTranSaction();
            XmlElement elment = Xdoc.DocumentElement;
            if (elment.GetAttribute("result") == "True")
            {
                XmlNodeList nodeList = Xdoc.DocumentElement.SelectNodes("bill");
                foreach (XmlNode nodel in nodeList)
                {
                    XmlNode nodel1 = nodel.SelectSingleNode("header");
                    string TypeCode = nodel1.SelectSingleNode("customerclasscode").InnerText.Trim();
                    string ParentCode = nodel1.SelectSingleNode("customerclassprecode").InnerText.Trim();
                    string TypeName = nodel1.SelectSingleNode("customerclassprecode").InnerText.Trim();
                    Hi.Model.BD_DisType type = null;
                    DataTable dt = ImportDisProD.GetDataSource("*", "BD_DisType", " and isnull(dr,0)=0 and TypeCode='" + TypeCode + "' and CompID=" + CompID + "", "", TranSaction);
                    if (dt.Rows.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(TypeName))
                        {
                            SetErrorTb("分类（同步）", "分类名称为空【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(TypeCode))
                        {
                            SetErrorTb("分类（同步）", "分类编码为空【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        else if ((FindTb = ImportDisProD.GetDataSource("1 name", "BD_DisType", " and isnull(dr,0)=0 and TypeName='" + TypeName + "' and Typecode<>'" + TypeCode + "' and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
                            SetErrorTb("分类（同步）", "分类名称已存在【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        type = Common.GetEntity<Hi.Model.BD_DisType>(dt);
                        type.TypeName = TypeName;
                        if (new Hi.BLL.BD_DisType().Update(type, TranSaction))
                        {
                            SumTypeTB++;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(TypeName))
                        {
                            SetErrorTb("分类（新增）", "分类名称为空【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(TypeCode))
                        {
                            SetErrorTb("分类（新增）", "分类编码为空【分类名称：" + TypeName + "，分类编码：" + TypeCode + "】");
                            continue;
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "BD_DisType", " and isnull(dr,0)=0 and (TypeName='" + TypeName + "' or Typecode='" + TypeCode + "') and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
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
                                if (new Hi.BLL.BD_DisType().Add(type) > 0)
                                {
                                    SumTypeAdd++;
                                }

                            }
                            else
                            {
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
                            if (new Hi.BLL.BD_DisType().Add(type) > 0)
                            {
                                SumTypeAdd++;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationException("获取分类错误：" + elment.GetAttribute("err"));
            }
            #endregion

            #region  代理商(地址)新增/同步方法

            #region 代理商新增/同步
            Xdoc = null;//ImportDisProD.GetServiceXmlStr(Enum.GetName(typeof(Enums.Erptype), Erptype), "GetCus", "获取代理商档案");
            elment = Xdoc.DocumentElement;
            if (elment.GetAttribute("result") == "True")
            {
                XmlNodeList nodeList = Xdoc.DocumentElement.SelectNodes("bill");
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
                    int Disid = 0;
                    DataTable dt = ImportDisProD.GetDataSource("*", "BD_Distributor", " and isnull(dr,0)=0 and DisCode='" + DisCode + "' and CompID=" + CompID + " and AuditState=2", "", TranSaction);
                    if (dt.Rows.Count > 0)
                    {
                        rgx = new Regex(@"^0?1[0-9]{10}$");
                        if (string.IsNullOrWhiteSpace(DisName))
                        {
                            SetErrorTb("代理商（同步）", "代理商名称为空【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(DisCode))
                        {
                            SetErrorTb("代理商（同步）", "代理商编码为空【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if (!rgx.IsMatch(Phone))
                        {
                            SetErrorTb("代理商（同步）", "手机号码格式错误【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "BD_Distributor", " and isnull(dr,0)=0 and (DisName='" + DisName + "' and DisCode<>'" + DisCode + "') and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
                            SetErrorTb("代理商（同步）", "代理商名称已存在【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        Hi.Model.BD_Distributor Dis = Common.GetEntity<Hi.Model.BD_Distributor>(dt);
                        Disid=Dis.ID;
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
                            SumDisTB++;
                        }
                    }
                    else
                    {
                        rgx = new Regex(@"^0?1[0-9]{10}$");
                        if (string.IsNullOrWhiteSpace(DisName))
                        {
                            SetErrorTb("代理商（新增）", "代理商名称为空【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if (string.IsNullOrWhiteSpace(DisCode))
                        {
                            SetErrorTb("代理商（新增）", "代理商编码为空【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if (!rgx.IsMatch(Phone))
                        {
                            SetErrorTb("代理商（新增）", "手机号码格式错误【代理商名称：" + DisName + "，代理商编码：" + DisCode + "】");
                            continue;
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "SYS_Users", " and isnull(dr,0)=0 and (Phone='" + Phone + "')", "", TranSaction)).Rows.Count > 0)
                        {
                            SetErrorTb("代理商（新增）", "代理商手机号码已存在【代理商名称：" + DisName + "，代理商编码：" + DisCode + "，手机号码：" + Phone + "】");
                            continue;
                        }
                        else if ((ImportDisProD.GetDataSource("1 name", "BD_Distributor", " and isnull(dr,0)=0 and (DisName='" + DisName + "' or DisCode='" + DisCode + "') and CompID=" + CompID + "", "", TranSaction)).Rows.Count > 0)
                        {
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
                            user.UserName = Enum.GetName(typeof(Enums.Erptype), Erptype) + DisCode;
                            user.UserPwd = Util.md5("123456");
                            user.IsEnabled = 1;
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
            #endregion

                    #region  代理商地址新增/同步
                    string XMLbody = "<bill><header><customercode>" + DisCode + "</customercode></header></bill>";
                    XmlDocument Doc1 = null;//ImportDisProD.GetServiceXmlStr(Enum.GetName(typeof(Enums.Erptype), Erptype), "GetCusAddress", "获取代理商收货地址", XMLbody);
                    elment = Xdoc.DocumentElement;
                    if (elment.GetAttribute("result") == "True")
                    {
                        rgx = new Regex(@"^0?1[0-9]{10}$");
                        XmlNodeList nodeList1 = Xdoc.DocumentElement.SelectNodes("bill");
                        foreach (XmlNode nodel2 in nodeList1)
                        {
                            XmlNode nodel3 = nodel2.SelectSingleNode("header");
                            string addcode = nodel3.SelectSingleNode("addresscode").InnerText.Trim();
                            Principal = nodel3.SelectSingleNode("linkperson").InnerText.Trim();
                            Address = nodel3.SelectSingleNode("address").InnerText.Trim();
                            Phone = nodel3.SelectSingleNode("handphone").InnerText.Trim();
                            Tel = nodel3.SelectSingleNode("phone").InnerText.Trim();
                            int isdefault = nodel3.SelectSingleNode("default").InnerText.Trim().ToInt(0);
                            dt = ImportDisProD.GetDataSource("*", "BD_DisAddr", " and isnull(dr,0)=0 and Code='" + addcode + "' and Disid=" + Disid + "", "", TranSaction);
                            if (dt.Rows.Count > 0)
                            {
                                if (string.IsNullOrWhiteSpace(addcode))
                                {
                                    SetErrorTb("代理商地址（同步）", "地址编码为空【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                else if (string.IsNullOrWhiteSpace(Principal))
                                {
                                    SetErrorTb("代理商地址（同步）", "联系人为空【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                else if (string.IsNullOrWhiteSpace(Address))
                                {
                                    SetErrorTb("代理商地址（同步）", "地址为空【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                else if (!rgx.IsMatch(Phone))
                                {
                                    SetErrorTb("代理商地址（同步）", "手机格式不正确【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                Hi.Model.BD_DisAddr addr = Common.GetEntity<Hi.Model.BD_DisAddr>(dt);
                                addr.Address = Address;
                                addr.Principal = Principal;
                                addr.Phone = Phone;
                                addr.Tel = Tel;
                                addr.IsDefault = isdefault;
                                if (new Hi.BLL.BD_DisAddr().Update(addr, TranSaction))
                                {
                                    SumAddressTB++;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(addcode))
                                {
                                    SetErrorTb("代理商地址（新增）", "地址编码为空【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                else if (string.IsNullOrWhiteSpace(Principal))
                                {
                                    SetErrorTb("代理商地址（新增）", "联系人为空【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                else if (string.IsNullOrWhiteSpace(Address))
                                {
                                    SetErrorTb("代理商地址（新增）", "地址为空【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                else if (!rgx.IsMatch(Phone))
                                {
                                    SetErrorTb("代理商地址（新增）", "手机格式不正确【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                else if ((ImportDisProD.GetDataSource("1 name", "BD_DisAddr", " and isnull(dr,0)=0 and (Code='" + addcode + "') and Disid=" + Disid + "", "", TranSaction)).Rows.Count > 0)
                                {
                                    SetErrorTb("代理商地址（新增）", "地址编码已存在【地址：" + Address + "，地址编码：" + addcode + "】");
                                    continue;
                                }
                                Hi.Model.BD_DisAddr addr = new Hi.Model.BD_DisAddr();
                                addr.DisID = Disid;
                                addr.Province = "上海市";
                                addr.City = "市辖区";
                                addr.Area = "徐汇区";
                                addr.Code = addcode;
                                addr.Address = Address;
                                addr.Phone = Phone;
                                addr.Principal = Principal;
                                addr.Tel = Tel;
                                addr.IsDefault = isdefault;
                                addr.CreateDate = DateTime.Now;
                                addr.CreateUserID = UserID;
                                addr.ts = DateTime.Now;
                                addr.modifyuser = UserID;
                                if (new Hi.BLL.BD_DisAddr().Add(addr) > 0) {
                                    SumAddressAdd++;
                                }
                            }

                        }
                    }

                    #endregion


                }
            }

            #endregion
            TranSaction.Commit();
            lblDisIpResult.InnerHtml = "新增（代理商：" + string.Format(ShowStr, SumDisAdd) + "条；类别：" + string.Format(ShowStr, SumTypeAdd) + "条；地址：" + string.Format(ShowStr, SumAddressAdd) + "条），同步（代理商：" + string.Format(ShowStr, SumDisTB) + "条；类别：" + string.Format(ShowStr, SumTypeTB) + "条；地址：" + string.Format(ShowStr, SumAddressTB) + "条）";
            if (ErrTb.Rows.Count > 0)
            {
                HERCount.InnerText = ErrTb.Rows.Count.ToString();
                DivError.Visible = true;
                Rpt_Error.DataSource = ErrTb;
                Rpt_Error.DataBind();
            }
        }
        catch (Exception ex)
        {
            if (ex is XmlException)
            {
                lblDisIpResult.InnerHtml = string.Format(ShowStr, "读取XML出现错误");
            }
            else if (ex is XPathException)
            {
                lblDisIpResult.InnerHtml = string.Format(ShowStr, "XML格式错误");
            }
            else
            {
                lblDisIpResult.InnerHtml = string.Format(ShowStr, "同步失败：" + ex.Message + "");
            }
            if (TranSaction != null)
            {
                if (TranSaction.Connection != null)
                {
                    TranSaction.Rollback();
                }
            }
        }
    }

    public void SetErrorTb(string type, string name)
    {
        row = ErrTb.NewRow();
        row["Etype"] = type;
        row["Ename"] = name;
        ErrTb.Rows.Add(row);
    }

    protected void btnGoodsTransfer_Click(object sender, EventArgs e)
    {
        SqlTransaction TranSaction = null;
        string ShowStr = "<i style='color:red;'>{0}</i>";
        int GoodsAdd = 0;
        int GoodsTB = 0;
        int CategoryAdd = 0;
        int CategoryTB = 0;
        int PayAdd = 0;
        int PayTB = 0;

        try
        {
            #region 商品分类新增/同步方法
            XmlDocument xml = null;//ImportDisProD.GetServiceXmlStr(Enum.GetName(typeof(Enums.Erptype), Erptype), "GetInvClass", "获取产品分类");
            XmlElement elment = xml.DocumentElement;
            if (!string.IsNullOrEmpty(elment.GetAttribute("err")))
            {
                lblGoodsIpResult.InnerText = elment.GetAttribute("err");
                return;
            }
            TranSaction = DBUtility.SqlHelper.CreateStoreTranSaction();
            XmlNodeList xmllist = xml.DocumentElement.SelectNodes("bill");
            foreach (XmlNode xmlbill in xmllist)
            {
                XmlNode xmlheader = xmlbill.SelectSingleNode("header");
                string goodsflcode = xmlheader.SelectSingleNode("inventoryclasscode").InnerText;
                string goodsflname = xmlheader.SelectSingleNode("inventoryclassname").InnerText;
                string goodsflprecode = xmlheader.SelectSingleNode("inventoryclassprecode").InnerText;
                Hi.Model.BD_GoodsCategory goodsfl = null;
                DataTable dt = ImportDisProD.GetDataSource("", "BD_GoodsCategory", "and CategoryCode=" + goodsflcode + " and compid=" + CompID, "", TranSaction);
                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(goodsflname))
                    {
                        SetErrorTb("商品分类（同步）", "分类名称不能为空。分类编码：" + goodsflcode);
                        continue;
                    }
                    DataTable dt1 = ImportDisProD.GetDataSource("", "BD_GoodsCategory", " and CategoryName='" + goodsflname + "' and ParentId=" + goodsflprecode.ToInt(0) + " and compid=" + CompID, "", TranSaction);
                    if (dt1.Rows.Count > 0)
                    {
                        continue;
                    }
                    else
                    {
                        goodsfl = Common.GetEntity<Hi.Model.BD_GoodsCategory>(dt);
                        goodsfl.CategoryName = goodsflname;
                        goodsfl.ParentId = goodsflprecode == "" ? 0 : int.Parse(goodsflprecode);
                        goodsfl.ts = DateTime.Now;
                        goodsfl.modifyuser = UserID;
                        if (new Hi.BLL.BD_GoodsCategory().Update(goodsfl, TranSaction))
                        {
                            CategoryTB++;
                        }
                    }
                }
                else
                {
                    goodsfl = new Hi.Model.BD_GoodsCategory();
                    goodsfl.CompID = CompID;
                    goodsfl.GoodsTypeID = 17;
                    goodsfl.CategoryCode = goodsflcode;
                    goodsfl.CategoryName = goodsflname;
                    goodsfl.ParentId = goodsflprecode == "" ? 0 : int.Parse(goodsflprecode);
                    goodsfl.IsEnabled = 1;
                    goodsfl.CreateUserID = UserID;
                    goodsfl.CreateDate = DateTime.Now;
                    goodsfl.ts = DateTime.Now;
                    goodsfl.dr = 0;
                    goodsfl.modifyuser = UserID;
                    if (new Hi.BLL.BD_GoodsCategory().Add(goodsfl) > 0)
                    {
                        CategoryAdd++;
                    }
                }

            }
            #endregion

            #region 商品基本信息、商品详情新增/同步
            xml = null;//ImportDisProD.GetServiceXmlStr(Enum.GetName(typeof(Enums.Erptype), Erptype), "GetInv", "获取产品档案");
            elment = xml.DocumentElement;
            if (!string.IsNullOrEmpty(elment.GetAttribute("err")))
            {
                lblGoodsIpResult.InnerText = elment.GetAttribute("err");
                return;
            }
            xmllist = xml.DocumentElement.SelectNodes("bill");
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
                        SetErrorTb("商品（同步）", "商品名称不能为空。产品编码：" + goodscode);
                        continue;
                    }
                    else
                        if (string.IsNullOrEmpty(unit))
                        {
                            SetErrorTb("商品（同步）", "商品单位不能为空。产品编码：" + unit);
                            continue;
                        }
                    DataTable dt1 = ImportDisProD.GetDataSource("top 1 *", "BD_GoodsCategory", "and CategoryCode=" + goodstypecode + " and compid=" + CompID, "", TranSaction);
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
                    xml = null;//ImportDisProD.GetServiceXmlStr(Enum.GetName(typeof(Enums.Erptype), Erptype), "GetInvPrice", "获取产品价格", "<bill><header><inventorycode>" + goodscode + "</inventorycode></header></bill>");
                    elment = xml.DocumentElement;
                    if (!string.IsNullOrEmpty(elment.GetAttribute("err")))
                    {
                        lblGoodsIpResult.InnerText = elment.GetAttribute("err");
                        return;
                    }
                    xmllist = xml.DocumentElement.SelectNodes("bill");

                    XmlNode xmlheaderinfo = xmllist[0].SelectSingleNode("header");
                    string goodsprice = xmlheaderinfo.SelectSingleNode("price").InnerText;
                    if (string.IsNullOrEmpty(goodsprice))
                    {
                        SetErrorTb("商品（同步）", "商品价格不能为空。产品编码：" + goodscode);
                        continue;
                    }
                    goods.SalePrice = decimal.Parse(goodsprice);
                    Hi.Model.BD_GoodsInfo goodsinfo = null;
                    DataTable dt2 = ImportDisProD.GetDataSource("top 1 *", "BD_GoodsInfo", "and value1=" + goods.GoodsCode + " and compid=" + CompID, "", TranSaction);
                    if (dt2.Rows.Count > 0)
                    {
                        goodsinfo = Common.GetEntity<Hi.Model.BD_GoodsInfo>(dt2);
                        goodsinfo.SalePrice = decimal.Parse(goodsprice);
                        goodsinfo.TinkerPrice = decimal.Parse(goodsprice);
                        goodsinfo.IsEnabled = true;
                        goodsinfo.ts = DateTime.Now;
                        goodsinfo.modifyuser = UserID;
                        goods.ts = DateTime.Now;
                        goods.modifyuser = UserID;
                        if (new Hi.BLL.BD_Goods().Update(goods, TranSaction))
                        {
                            GoodsTB++;
                        }
                        if (new Hi.BLL.BD_GoodsInfo().Update(goodsinfo, TranSaction))
                        {
                            PayTB++;
                        }
                    }
                    else
                    {
                        goodsinfo = new Hi.Model.BD_GoodsInfo();
                        goodsinfo.CompID = CompID;
                        goodsinfo.GoodsID = goods.ID;
                        goodsinfo.Value1 = goods.GoodsCode;
                        goodsinfo.SalePrice = decimal.Parse(goodsprice);
                        goodsinfo.TinkerPrice = decimal.Parse(goodsprice);
                        goodsinfo.IsEnabled = true;
                        goodsinfo.CreateUserID = UserID;
                        goodsinfo.CreateDate = DateTime.Now;
                        goodsinfo.ts = DateTime.Now;
                        goodsinfo.modifyuser = UserID;
                        goods.ts = DateTime.Now;
                        goods.modifyuser = UserID;
                        if (new Hi.BLL.BD_Goods().Update(goods, TranSaction))
                        {
                            GoodsTB++;
                        }
                        if (new Hi.BLL.BD_GoodsInfo().Add(goodsinfo, TranSaction) > 0)
                        {
                            PayAdd++;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(goodsname))
                    {
                        SetErrorTb("商品（添加）", "商品名称不能为空。产品编码：" + goodscode);
                        continue;
                    }
                    else
                        if (string.IsNullOrEmpty(unit))
                        {
                            SetErrorTb("商品（添加）", "商品单位不能为空。产品编码：" + unit);
                            continue;
                        }
                    DataTable dt1 = ImportDisProD.GetDataSource("top 1 *", "BD_GoodsCategory", "and CategoryCode=" + goodstypecode + " and compid=" + CompID, "", TranSaction);
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
                    if (DateTime.Now >= goods.OfflineStateDate && DateTime.Now <= goods.OfflineEndDate)
                    {
                        goods.IsOffline = 1;
                    }
                    else
                    {
                        goods.IsOffline = 0;
                    }
                    goods.IsIndex = 0;
                    goods.IsSale = 0;
                    goods.IsRecommended = 1;
                    goods.IsEnabled = 1;
                    goods.CreateUserID = UserID;
                    goods.CreateDate = DateTime.Now;
                    goods.ts = DateTime.Now;
                    goods.modifyuser = UserID;
                    xml = null;// ImportDisProD.GetServiceXmlStr(Enum.GetName(typeof(Enums.Erptype), Erptype), "GetInvPrice", "获取产品价格", "<bill><header><inventorycode>" + goodscode + "</inventorycode></header></bill>");
                    elment = xml.DocumentElement;
                    if (!string.IsNullOrEmpty(elment.GetAttribute("err")))
                    {
                        lblGoodsIpResult.InnerText = elment.GetAttribute("err");
                        return;
                    }
                    xmllist = xml.DocumentElement.SelectNodes("bill");
                    XmlNode xmlheaderinfo = xmllist[0].SelectSingleNode("header");
                    string goodsprice = xmlheaderinfo.SelectSingleNode("price").InnerText;
                    if (string.IsNullOrEmpty(goodsprice))
                    {
                        SetErrorTb("商品（添加）", "商品价格不能为空。产品编码：" + goodscode);
                        continue;
                    }
                    goods.SalePrice = decimal.Parse(goodsprice);
                    int goodsid=new Hi.BLL.BD_Goods().Add(goods, TranSaction);
                    if (goodsid>0)
                    {
                        GoodsAdd++;
                        Hi.Model.BD_GoodsInfo goodsinfo = new Hi.Model.BD_GoodsInfo();
                        goodsinfo.CompID = CompID;
                        goodsinfo.GoodsID = goodsid;
                        goodsinfo.Value1 = goodscode;
                        goodsinfo.SalePrice = decimal.Parse(goodsprice);
                        goodsinfo.TinkerPrice = decimal.Parse(goodsprice);
                        goodsinfo.IsEnabled = true;
                        goodsinfo.CreateDate = DateTime.Now;
                        goodsinfo.CreateUserID = UserID;
                        goodsinfo.ts = DateTime.Now;
                        goodsinfo.modifyuser = UserID;
                        if (new Hi.BLL.BD_GoodsInfo().Add(goodsinfo, TranSaction) > 0)
                        {
                            PayAdd++;
                        }
                    }
                }
            }
            #endregion
        }
        catch
        {
            lblGoodsIpResult.InnerText = "读取文件错误，请联系管理员";
        }
        finally
        {
            if (ErrTb.Rows.Count > 0)
            {
                HERCount.InnerText = ErrTb.Rows.Count.ToString();
                DivError.Visible = true;
                Rpt_Error.DataSource = ErrTb;
                Rpt_Error.DataBind();
            }
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        List<Hi.Model.BD_Distributor> dis = new Hi.BLL.BD_Distributor().GetList("", " Compid=" + CompID + "", "");
        foreach (Hi.Model.BD_Distributor model in dis)
        {
            new Hi.BLL.BD_Distributor().Delete(model.ID);
            List<Hi.Model.BD_DisAddr> Daddr = new Hi.BLL.BD_DisAddr().GetList("", " disid='" + model.ID + "'", "");
            foreach (Hi.Model.BD_DisAddr mo in Daddr)
            {
                new Hi.BLL.BD_DisAddr().Delete(mo.ID);
            }
            List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", " disid='" + model.ID + "' and type=5", "");
            foreach (Hi.Model.SYS_Users mo in user)
            {
                new Hi.BLL.SYS_Users().Delete(mo.ID);
            }
        }
        List<Hi.Model.BD_DisType> Dtype = new Hi.BLL.BD_DisType().GetList("", " Compid=" + CompID + "", "");
        foreach (Hi.Model.BD_DisType model in Dtype)
        {
            new Hi.BLL.BD_DisType().Delete(model.ID);
        }
    }
}