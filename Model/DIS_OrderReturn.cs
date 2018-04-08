//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/4/29 17:45:56
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
    /// 实体类 DIS_OrderReturn
    /// </summary>
    public class DIS_OrderReturn
    {
        public DIS_OrderReturn()
        { }
        #region Model
        private int _id;
        private string _receiptno;
        private int _orderid;
        private int _disid;
        private int _compid;
        private DateTime _returndate;
        private int _returnuserid;
        private string _returncontent;
        private string _express;
        private string _expressno;
        private int _returnstate;
        private int _audituserid;
        private DateTime _auditdate;
        private string _auditremark;
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
        public string ReceiptNo
        {
            set { _receiptno = value; }
            get { return _receiptno; }
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
        public DateTime ReturnDate
        {
            set { _returndate = value; }
            get { return _returndate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ReturnUserID
        {
            set { _returnuserid = value; }
            get { return _returnuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnContent
        {
            set { _returncontent = value; }
            get { return _returncontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Express
        {
            set { _express = value; }
            get { return _express; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExpressNo
        {
            set { _expressno = value; }
            get { return _expressno; }
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
