//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/12/20 14:21:55
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
    /// 实体类 SYS_Hospital
    /// </summary>
    public class SYS_Hospital
    {
        public SYS_Hospital()
        { }
        #region Model
        private int _id;
        private string _hospitalcode;
        private string _hospitalname;
        private string _hospitallevel;
        private string _province;
        private string _city;
        private string _area;
        private string _address;
        private bool _isenabled;
        private string _svdef1;
        private string _svdef2;
        private string _svdef3;
        private string _createuser;
        private DateTime _createdate;
        private DateTime _ts;
        private bool _dr;
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
        public string HospitalCode
        {
            set { _hospitalcode = value; }
            get { return _hospitalcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HospitalName
        {
            set { _hospitalname = value; }
            get { return _hospitalname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HospitalLevel
        {
            set { _hospitallevel = value; }
            get { return _hospitallevel; }
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
        public bool IsEnabled
        {
            set { _isenabled = value; }
            get { return _isenabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SVdef1
        {
            set { _svdef1 = value; }
            get { return _svdef1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SVdef2
        {
            set { _svdef2 = value; }
            get { return _svdef2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SVdef3
        {
            set { _svdef3 = value; }
            get { return _svdef3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUser
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
        public DateTime ts
        {
            set { _ts = value; }
            get { return _ts; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool dr
        {
            set { _dr = value; }
            get { return _dr; }
        }
        #endregion Model
    }
}
