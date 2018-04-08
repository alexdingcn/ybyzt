//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/3/1 15:15:23
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
    /// 实体类 Pay_Service
    /// </summary>
    public class Pay_Service
    {
        public Pay_Service()
        { }
        #region Model
        private int _id;
        private int _compid;
        private string _compname;
        private int _servicetype;
        private DateTime _outdata;
        private decimal _price;
        private DateTime _createdate;
        private int _createuser;
        private byte _outofdata;
        private decimal _payedprice;
        private int _isaudit;
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
        public int CompID
        {
            set { _compid = value; }
            get { return _compid; }
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
        public int ServiceType
        {
            set { _servicetype = value; }
            get { return _servicetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OutData
        {
            set { _outdata = value; }
            get { return _outdata; }
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
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
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
        public byte OutOfData
        {
            set { _outofdata = value; }
            get { return _outofdata; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PayedPrice
        {
            set { _payedprice = value; }
            get { return _payedprice; }
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
