//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/8/31 9:27:51
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
    /// 实体类 PAY_Payment
    /// </summary>
    public class PAY_Payment
    {
        public PAY_Payment()
        { }
        #region Model
        private int _id;
        private int _orderid;
        private int _disid;
        private int _type;
        private string _channel;
        private DateTime _paydate;
        private string _payuser;
        private decimal _payprice;
        private string _remark;
        private int _isaudit;
        private int _state;
        private int _printnum;
        private int _audituserid;
        private DateTime _auditdate;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private string _guid;
        private int _verifystatus;
        private int _status;
        private string _vdef3;
        private string _vdef4;
        private string _vdef5;
        private int _jsxf_no;
        private string _vdef6;
        private string _vdef7;
        private string _vdef8;
        private string _vdef9;
        private string _attach;
        private string _payname;
        private string _paycode;
        private string _paybank;
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
        public int OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
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
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Channel
        {
            set { _channel = value; }
            get { return _channel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PayDate
        {
            set { _paydate = value; }
            get { return _paydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PayUser
        {
            set { _payuser = value; }
            get { return _payuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PayPrice
        {
            set { _payprice = value; }
            get { return _payprice; }
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
        public int IsAudit
        {
            set { _isaudit = value; }
            get { return _isaudit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PrintNum
        {
            set { _printnum = value; }
            get { return _printnum; }
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
        public string guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int verifystatus
        {
            set { _verifystatus = value; }
            get { return _verifystatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef3
        {
            set { _vdef3 = value; }
            get { return _vdef3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef4
        {
            set { _vdef4 = value; }
            get { return _vdef4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef5
        {
            set { _vdef5 = value; }
            get { return _vdef5; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int jsxf_no
        {
            set { _jsxf_no = value; }
            get { return _jsxf_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef6
        {
            set { _vdef6 = value; }
            get { return _vdef6; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef7
        {
            set { _vdef7 = value; }
            get { return _vdef7; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef8
        {
            set { _vdef8 = value; }
            get { return _vdef8; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef9
        {
            set { _vdef9 = value; }
            get { return _vdef9; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string attach
        {
            set { _attach = value; }
            get { return _attach; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string payName
        {
            set { _payname = value; }
            get { return _payname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string paycode
        {
            set { _paycode = value; }
            get { return _paycode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string paybank
        {
            set { _paybank = value; }
            get { return _paybank; }
        }
        #endregion Model
    }
}
