//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/6/22 16:38:56
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
    /// 实体类 BD_Rebate
    /// </summary>
    public class BD_Rebate
    {
        public BD_Rebate()
        { }
        #region Model
        private int _id;
        private string _receiptno;
        private int _compid;
        private int _disid;
        private int _rebatetype;
        private decimal _rebateamount;
        private decimal _userdamount;
        private decimal _enableamount;
        private DateTime _startdate;
        private DateTime _enddate;
        private int _rebatestate;
        private int _createuserid;
        private DateTime _createdate;
        private string _remark;
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
        public int RebateType
        {
            set { _rebatetype = value; }
            get { return _rebatetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal RebateAmount
        {
            set { _rebateamount = value; }
            get { return _rebateamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UserdAmount
        {
            set { _userdamount = value; }
            get { return _userdamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal EnableAmount
        {
            set { _enableamount = value; }
            get { return _enableamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate
        {
            set { _startdate = value; }
            get { return _startdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RebateState
        {
            set { _rebatestate = value; }
            get { return _rebatestate; }
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
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
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
