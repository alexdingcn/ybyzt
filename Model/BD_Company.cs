//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/12/28 12:43:36
//
// 功能描述: 
//
// 修改标识: 
// 修改描述: 
//------------------------------------------------------------------------------

using System;

namespace Hi.Model
{
    /// <summary>
    /// 实体类 BD_Company
    /// </summary>
    public class BD_Company
    {
        public BD_Company()
        { }
        #region Model
        private int _id;
        private int _orgid;
        private int _salesmanid;
        private int _indid;
        private string _compcode;
        private string _compname;
        private string _sortindex;
        private string _shortname;
        private string _tel;
        private string _legal;
        private string _identitys;
        private string _legaltel;
        private string _zip;
        private string _manageinfo;
        private string _fax;
        private string _codes;
        private string _trade;
        private string _principal;
        private string _phone;
        private string _licence;
        private string _account;
        private string _organizationcode;
        private string _compaddr;
        private string _address;
        private string _complogo;
        private string _shoplogo;
        private string _brandinfo;
        private string _compinfo;
        private string _remark;
        private int _isenabled;
        private string _firstbanerimg;
        private int _hotshow;
        private int _firstshow;
        private string _customcompinfo;
        private string _customaddress;
        private string _financecode;
        private string _financename;
        private int _createuserid;
        private DateTime _createdate;
        private int _auditstate;
        private string _audituser;
        private DateTime _auditdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private string _attachment;
        private int _erptype;
        private string _compnewlogo;
        private string _qq;
        private DateTime _enabledstartdate;
        private DateTime _enabledenddate;
        private string _capital;
        private int _comptype;
        private int _iszxaudit;
        private DateTime _zxauditdate;
        private int _zxaudituser;
        private int _isorgzx;
        private DateTime _orgzxdate;
        private string _creditCode;

        public int Zxaudituser
        {
            get { return _zxaudituser; }
            set { _zxaudituser = value; }
        }

        public int Isorgzx
        {
            get { return _isorgzx; }
            set { _isorgzx = value; }
        }

        public DateTime Orgzxdate
        {
            get { return _orgzxdate; }
            set { _orgzxdate = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public int IsZXAudit
        {
            set { _iszxaudit = value; }
            get { return _iszxaudit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ZXAuditDate
        {
            set { _zxauditdate = value; }
            get { return _zxauditdate; }
        }


        /// <summary>
        /// 
        /// </summary>
        public int CompType
        {
            set { _comptype = value; }
            get { return _comptype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Capital
        {
            set { _capital = value; }
            get { return _capital; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OrgID
        {
            set { _orgid = value; }
            get { return _orgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SalesManID
        {
            set { _salesmanid = value; }
            get { return _salesmanid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IndID
        {
            set { _indid = value; }
            get { return _indid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompCode
        {
            set { _compcode = value; }
            get { return _compcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompName
        {
            set { _compname = value; }
            get { return _compname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SortIndex
        {
            set { _sortindex = value; }
            get { return _sortindex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShortName
        {
            set { _shortname = value; }
            get { return _shortname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Legal
        {
            set { _legal = value; }
            get { return _legal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Identitys
        {
            set { _identitys = value; }
            get { return _identitys; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LegalTel
        {
            set { _legaltel = value; }
            get { return _legaltel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Zip
        {
            set { _zip = value; }
            get { return _zip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ManageInfo
        {
            set { _manageinfo = value; }
            get { return _manageinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fax
        {
            set { _fax = value; }
            get { return _fax; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Codes
        {
            set { _codes = value; }
            get { return _codes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Trade
        {
            set { _trade = value; }
            get { return _trade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Principal
        {
            set { _principal = value; }
            get { return _principal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Licence
        {
            set { _licence = value; }
            get { return _licence; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Account
        {
            set { _account = value; }
            get { return _account; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrganizationCode
        {
            set { _organizationcode = value; }
            get { return _organizationcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompAddr
        {
            set { _compaddr = value; }
            get { return _compaddr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompLogo
        {
            set { _complogo = value; }
            get { return _complogo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShopLogo
        {
            set { _shoplogo = value; }
            get { return _shoplogo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BrandInfo
        {
            set { _brandinfo = value; }
            get { return _brandinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompInfo
        {
            set { _compinfo = value; }
            get { return _compinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled
        {
            set { _isenabled = value; }
            get { return _isenabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FirstBanerImg
        {
            set { _firstbanerimg = value; }
            get { return _firstbanerimg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int HotShow
        {
            set { _hotshow = value; }
            get { return _hotshow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FirstShow
        {
            set { _firstshow = value; }
            get { return _firstshow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomCompinfo
        {
            set { _customcompinfo = value; }
            get { return _customcompinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomAddress
        {
            set { _customaddress = value; }
            get { return _customaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FinanceCode
        {
            set { _financecode = value; }
            get { return _financecode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FinanceName
        {
            set { _financename = value; }
            get { return _financename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AuditState
        {
            set { _auditstate = value; }
            get { return _auditstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AuditUser
        {
            set { _audituser = value; }
            get { return _audituser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AuditDate
        {
            set { _auditdate = value; }
            get { return _auditdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ts
        {
            set { _ts = value; }
            get { return _ts; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int dr
        {
            set { _dr = value; }
            get { return _dr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int modifyuser
        {
            set { _modifyuser = value; }
            get { return _modifyuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Attachment
        {
            set { _attachment = value; }
            get { return _attachment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Erptype
        {
            set { _erptype = value; }
            get { return _erptype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompNewLogo
        {
            set { _compnewlogo = value; }
            get { return _compnewlogo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QQ
        {
            set { _qq = value; }
            get { return _qq; }
        }


        /// <summary>
        /// 
        /// </summary>
        public DateTime EnabledStartDate
        {
            set { _enabledstartdate = value; }
            get { return _enabledstartdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EnabledEndDate
        {
            set { _enabledenddate = value; }
            get { return _enabledenddate; }
        }

        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string creditCode
        {
            set { _creditCode = value; }
            get { return _creditCode; }
        }

        #endregion Model
    }
}
