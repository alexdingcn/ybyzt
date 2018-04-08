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
    /// 实体类 SYS_SysBusinessLog
    /// </summary>
    public class SYS_SysBusinessLog
    {
        public SYS_SysBusinessLog()
        { }
        #region Model
        private int _id;
        private int _compid;
        private string _logclass;
        private int _applicationid;
        private string _logtype;
        private DateTime _logtime;
        private int _operatepersonid;
        private string _operateperson;
        private string _logremark;
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
        public string LogClass
        {
            set { _logclass = value; }
            get { return _logclass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ApplicationId
        {
            set { _applicationid = value; }
            get { return _applicationid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LogType
        {
            set { _logtype = value; }
            get { return _logtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LogTime
        {
            set { _logtime = value; }
            get { return _logtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OperatePersonId
        {
            set { _operatepersonid = value; }
            get { return _operatepersonid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperatePerson
        {
            set { _operateperson = value; }
            get { return _operateperson; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LogRemark
        {
            set { _logremark = value; }
            get { return _logremark; }
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
