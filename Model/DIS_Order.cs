//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/10/12 17:27:21
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
    /// 实体类 DIS_Order
    /// </summary>
    public class DIS_Order
    {
        public DIS_Order()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _disid;
        private int _disuserid;
        private int _addtype;
        private int _otype;
        private int _isaudit;
        private int _addrid;
        private string _receiptno;
        private string _guid;
        private DateTime _arrivedate;
        private decimal _totalamount;
        private decimal _auditamount;
        private decimal _otheramount;
        private decimal _payedamount;
        private string _principal;
        private string _phone;
        private string _address;
        private string _remark;
        private int _ostate;
        private int _paystate;
        private int _returnstate;
        private int _audituserid;
        private DateTime _auditdate;
        private string _auditremark;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _returnmoneydate;
        private int _returnmoneyuserid;
        private string _returnmoneyuser;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private string _atta;
        private int _isoutstate;
        private string _ispaycoll;
        private string _costsub;
        private decimal _bateamount;
        private string _issettl;
        private string _givemode;
        private decimal _postfee;
        private string _vdef1;
        private string _vdef2;
        private string _vdef3;
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
        public int DisID
        {
            set { _disid = value; }
            get { return _disid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DisUserID
        {
            set { _disuserid = value; }
            get { return _disuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AddType
        {
            set { _addtype = value; }
            get { return _addtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Otype
        {
            set { _otype = value; }
            get { return _otype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsAudit
        {
            set { _isaudit = value; }
            get { return _isaudit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AddrID
        {
            set { _addrid = value; }
            get { return _addrid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiptNo
        {
            set { _receiptno = value; }
            get { return _receiptno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GUID
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ArriveDate
        {
            set { _arrivedate = value; }
            get { return _arrivedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal TotalAmount
        {
            set { _totalamount = value; }
            get { return _totalamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal AuditAmount
        {
            set { _auditamount = value; }
            get { return _auditamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal OtherAmount
        {
            set { _otheramount = value; }
            get { return _otheramount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PayedAmount
        {
            set { _payedamount = value; }
            get { return _payedamount; }
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
        public string Address
        {
            set { _address = value; }
            get { return _address; }
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
        public int OState
        {
            set { _ostate = value; }
            get { return _ostate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PayState
        {
            set { _paystate = value; }
            get { return _paystate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ReturnState
        {
            set { _returnstate = value; }
            get { return _returnstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AuditUserID
        {
            set { _audituserid = value; }
            get { return _audituserid; }
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
        public string AuditRemark
        {
            set { _auditremark = value; }
            get { return _auditremark; }
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
        public DateTime ReturnMoneyDate
        {
            set { _returnmoneydate = value; }
            get { return _returnmoneydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ReturnMoneyUserId
        {
            set { _returnmoneyuserid = value; }
            get { return _returnmoneyuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnMoneyUser
        {
            set { _returnmoneyuser = value; }
            get { return _returnmoneyuser; }
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
        public string Atta
        {
            set { _atta = value; }
            get { return _atta; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsOutState
        {
            set { _isoutstate = value; }
            get { return _isoutstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IsPayColl
        {
            set { _ispaycoll = value; }
            get { return _ispaycoll; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CostSub
        {
            set { _costsub = value; }
            get { return _costsub; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal bateAmount
        {
            set { _bateamount = value; }
            get { return _bateamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IsSettl
        {
            set { _issettl = value; }
            get { return _issettl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GiveMode
        {
            set { _givemode = value; }
            get { return _givemode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PostFee
        {
            set { _postfee = value; }
            get { return _postfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef1
        {
            set { _vdef1 = value; }
            get { return _vdef1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef2
        {
            set { _vdef2 = value; }
            get { return _vdef2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef3
        {
            set { _vdef3 = value; }
            get { return _vdef3; }
        }
        #endregion Model
    }
}
