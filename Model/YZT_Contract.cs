//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/12/23 16:31:42
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
    /// 实体类 YZT_Contract
    /// </summary>
    public class YZT_Contract
    {
        public YZT_Contract()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _disid;
        private string _contractno;
        private DateTime _contractdate;
        private int _cstate;
        private DateTime _forcedate;
        private DateTime _invaliddate;
        private string _remark;
        private string _vdef1;
        private string _vdef2;
        private string _vdef3;
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
        public string contractNO
        {
            set { _contractno = value; }
            get { return _contractno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime contractDate
        {
            set { _contractdate = value; }
            get { return _contractdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CState
        {
            set { _cstate = value; }
            get { return _cstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ForceDate
        {
            set { _forcedate = value; }
            get { return _forcedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime InvalidDate
        {
            set { _invaliddate = value; }
            get { return _invaliddate; }
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
        public string vdef1
        {
            set { _vdef1 = value; }
            get { return _vdef1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef2
        {
            set { _vdef2 = value; }
            get { return _vdef2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef3
        {
            set { _vdef3 = value; }
            get { return _vdef3; }
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
