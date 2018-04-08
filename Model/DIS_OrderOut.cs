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
    /// 实体类 DIS_OrderOut
    /// </summary>
    public class DIS_OrderOut
    {
        public DIS_OrderOut()
        { }
        #region Model
        private int _id;
        private int _orderid;
        private int _disid;
        private int _compid;
        private string _receiptno;
        private DateTime _senddate;
        private string _actionuser;
        private string _remark;
        private int _isaudit;
        private int _audituserid;
        private DateTime _auditdate;
        private string _auditremark;
        private DateTime _signdate;
        private int _issign;
        private int _signuserid;
        private string _signuser;
        private string _signremark;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
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
        public int CompID
        {
            set { _compid = value; }
            get { return _compid; }
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
        public DateTime SendDate
        {
            set { _senddate = value; }
            get { return _senddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ActionUser
        {
            set { _actionuser = value; }
            get { return _actionuser; }
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
        public DateTime SignDate
        {
            set { _signdate = value; }
            get { return _signdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsSign
        {
            set { _issign = value; }
            get { return _issign; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SignUserId
        {
            set { _signuserid = value; }
            get { return _signuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SignUser
        {
            set { _signuser = value; }
            get { return _signuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SignRemark
        {
            set { _signremark = value; }
            get { return _signremark; }
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
        #endregion Model
    }
}
