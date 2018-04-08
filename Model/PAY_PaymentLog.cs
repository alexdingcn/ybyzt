//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/19 13:10:06
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
    /// 实体类 PAY_PaymentLog
    /// </summary>
    public class PAY_PaymentLog
    {
        public PAY_PaymentLog()
        { }
        #region Model
        private int _id;
        private int _orderid;
        private int _compid;
        private string _orgcode;
        private string _marknumber;
        private decimal _price;
        private string _payorgcode;
        private string _paycode;
        private int _disid;
        private DateTime _paytime;
        private string _remark;
        private string _start;
        private string _message;
        private int _createuser;
        private DateTime _createdate;
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
        public int CompID
        {
            set { _compid = value; }
            get { return _compid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrgCode
        {
            set { _orgcode = value; }
            get { return _orgcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MarkNumber
        {
            set { _marknumber = value; }
            get { return _marknumber; }
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
        public string PayorgCode
        {
            set { _payorgcode = value; }
            get { return _payorgcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PayCode
        {
            set { _paycode = value; }
            get { return _paycode; }
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
        public DateTime PayTime
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
        public string Start
        {
            set { _start = value; }
            get { return _start; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            set { _message = value; }
            get { return _message; }
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
        #endregion Model
    }
}
