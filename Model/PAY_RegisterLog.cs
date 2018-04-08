//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/7/14 16:39:46
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
    /// 实体类 PAY_RegisterLog
    /// </summary>
    public class PAY_RegisterLog
    {
        public PAY_RegisterLog()
        { }
        #region Model
        private int _id;
        private int _orderid;
        private string _ordercode;
        private string _number;
        private decimal _price;
        private decimal _fees;
        private string _payuse;
        private string _payname;
        private int _disid;
        private string _paytime;
        private string _remark;
        private string _disname;
        private string _bankid;
        private string _typea;
        private string _start;
        private string _resultmessage;
        private int _createuser;
        private DateTime _createdate;
        private int _logtype;
        private string _planmessage;
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
        public int OrderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Ordercode
        {
            set { _ordercode = value; }
            get { return _ordercode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string number
        {
            set { _number = value; }
            get { return _number; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal fees
        {
            set { _fees = value; }
            get { return _fees; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Payuse
        {
            set { _payuse = value; }
            get { return _payuse; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PayName
        {
            set { _payname = value; }
            get { return _payname; }
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
        public string PayTime
        {
            set { _paytime = value; }
            get { return _paytime; }
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
        public string DisName
        {
            set { _disname = value; }
            get { return _disname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BankID
        {
            set { _bankid = value; }
            get { return _bankid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Typea
        {
            set { _typea = value; }
            get { return _typea; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Start
        {
            set { _start = value; }
            get { return _start; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResultMessage
        {
            set { _resultmessage = value; }
            get { return _resultmessage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreateUser
        {
            set { _createuser = value; }
            get { return _createuser; }
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
        public int LogType
        {
            set { _logtype = value; }
            get { return _logtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PlanMessage
        {
            set { _planmessage = value; }
            get { return _planmessage; }
        }
        #endregion Model
    }
}
