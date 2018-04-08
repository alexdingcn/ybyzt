//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/5/23 15:54:04
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
    /// 实体类 SYS_CompUser
    /// </summary>
    public class SYS_CompUser
    {
        public SYS_CompUser()
        { }
        #region Model
        private int _id;
        private int _userid;
        private int _compid;
        private int _disid;
        private int _ctype;
        private int _utype;
        private int _roleid;
        private int _isaudit;
        private int _isenabled;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private int _dissalesmanid;
        private int _distypeid;
        private int _areaid;
        private int _ischeck;
        private int _credittype;
        private decimal _creditamount;
        private string _audituser;
        private DateTime _auditdate;

        /// <summary>
        /// 
        /// </summary>
        public int DisSalesManID
        {
            set { _dissalesmanid = value; }
            get { return _dissalesmanid; }
        }

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
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
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
        public int CType
        {
            set { _ctype = value; }
            get { return _ctype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UType
        {
            set { _utype = value; }
            get { return _utype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RoleID
        {
            set { _roleid = value; }
            get { return _roleid; }
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
        public int IsEnabled
        {
            set { _isenabled = value; }
            get { return _isenabled; }
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
        public int DisTypeID
        {
            set { _distypeid = value; }
            get { return _distypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsCheck
        {
            set { _ischeck = value; }
            get { return _ischeck; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreditType
        {
            set { _credittype = value; }
            get { return _credittype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal CreditAmount
        {
            set { _creditamount = value; }
            get { return _creditamount; }
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
        public DateTime AuditDate
        {
            set { _auditdate = value; }
            get { return _auditdate; }
        }

        #endregion Model
    }
}
