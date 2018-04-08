//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/23 13:26:21
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
    /// 实体类 BD_Distributor
    /// </summary>
    public class BD_Distributor
    {
        public BD_Distributor()
        { }
        #region Model
        private int _id;
        private int _compid;
        private string _discode;
        private string _disname;
        private int _smid;
        private string _shortname;
        private string _shortindex;
        private int _distypeid;
        private int _areaid;
        private string _dislevel;
        private string _province;
        private string _city;
        private string _area;
        private string _address;
        private string _principal;
        private string _phone;
        private string _leading;
        private string _leadingphone;
        private string _licence;
        private string _tel;
        private string _zip;
        private string _fax;
        private string _remark;
        private decimal _disaccount;
        private int _ischeck;
        private int _credittype;
        private decimal _creditamount;
        private decimal _integral;
        private int _zz_flag;
        private int _ka_flag;
        private int _sdfx_flag;
        private string _paypwd;
        private int _auditstate;
        private string _audituser;
        private DateTime _auditdate;
        private int _isenabled;
        private int _financingratio;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private string _pic;
        private string _creditCode;
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
        public int CompID
        {
            set { _compid = value; }
            get { return _compid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DisCode
        {
            set { _discode = value; }
            get { return _discode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DisName
        {
            set { _disname = value; }
            get { return _disname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SMID
        {
            set { _smid = value; }
            get { return _smid; }
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
        public string ShortIndex
        {
            set { _shortindex = value; }
            get { return _shortindex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DisTypeID
        {
            set { _distypeid = value; }
            get { return _distypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DisLevel
        {
            set { _dislevel = value; }
            get { return _dislevel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Province
        {
            set { _province = value; }
            get { return _province; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Area
        {
            set { _area = value; }
            get { return _area; }
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
        public string Leading
        {
            set { _leading = value; }
            get { return _leading; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LeadingPhone
        {
            set { _leadingphone = value; }
            get { return _leadingphone; }
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
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
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
        public string Fax
        {
            set { _fax = value; }
            get { return _fax; }
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
        public decimal DisAccount
        {
            set { _disaccount = value; }
            get { return _disaccount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsCheck
        {
            set { _ischeck = value; }
            get { return _ischeck; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreditType
        {
            set { _credittype = value; }
            get { return _credittype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal CreditAmount
        {
            set { _creditamount = value; }
            get { return _creditamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Integral
        {
            set { _integral = value; }
            get { return _integral; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int zz_flag
        {
            set { _zz_flag = value; }
            get { return _zz_flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ka_flag
        {
            set { _ka_flag = value; }
            get { return _ka_flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int sdfx_flag
        {
            set { _sdfx_flag = value; }
            get { return _sdfx_flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Paypwd
        {
            set { _paypwd = value; }
            get { return _paypwd; }
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
        public int IsEnabled
        {
            set { _isenabled = value; }
            get { return _isenabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FinancingRatio
        {
            set { _financingratio = value; }
            get { return _financingratio; }
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
        public string pic
        {
            set { _pic = value; }
            get { return _pic; }
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
