//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/18 12:07:26
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
    /// 实体类 BD_PrePayment
    /// </summary>
    public class BD_PrePayment
    {
        public BD_PrePayment()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _disid;
        private int _orderid;
        private int _start;
        private int _pretype;
        private decimal _money;
        private DateTime _creatdate;
        private int _oldid;
        private int _crateuser;
        private int _auditstate;
        private string _audituser;
        private int _isenabled;
        private DateTime _auditdate;
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
        public int DisID
        {
            set { _disid = value; }
            get { return _disid; }
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
        public int Start
        {
            set { _start = value; }
            get { return _start; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PreType
        {
            set { _pretype = value; }
            get { return _pretype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Money
        {
            set { _money = value; }
            get { return _money; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatDate
        {
            set { _creatdate = value; }
            get { return _creatdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OldId
        {
            set { _oldid = value; }
            get { return _oldid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CrateUser
        {
            set { _crateuser = value; }
            get { return _crateuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AuditState
        {
            set { _auditstate = value; }
            get { return _auditstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AuditUser
        {
            set { _audituser = value; }
            get { return _audituser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled
        {
            set { _isenabled = value; }
            get { return _isenabled; }
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
