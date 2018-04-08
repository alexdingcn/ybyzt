//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/3/31 10:13:25
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
    /// 实体类 PAY_PayLog
    /// </summary>
    public class PAY_PayLog
    {
        public PAY_PayLog()
        { }
        #region Model
        private int _id;
        private int _orderid;
        private string _ordercode;
        private string _number;
        private int _compid;
        private string _orgcode;
        private string _markname;
        private string _marknumber;
        private string _accountname;
        private string _bankcode;
        private string _bankaddress;
        private string _bankprivate;
        private string _bankcity;
        private decimal _price;
        private string _remark;
        private string _start;
        private string _resultmessage;
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
        public string MarkName
        {
            set { _markname = value; }
            get { return _markname; }
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
        public string AccountName
        {
            set { _accountname = value; }
            get { return _accountname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string bankcode
        {
            set { _bankcode = value; }
            get { return _bankcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string bankAddress
        {
            set { _bankaddress = value; }
            get { return _bankaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string bankPrivate
        {
            set { _bankprivate = value; }
            get { return _bankprivate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string bankCity
        {
            set { _bankcity = value; }
            get { return _bankcity; }
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
        #endregion Model
    }
}
