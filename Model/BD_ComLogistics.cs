//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/3/15 12:47:34
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
    /// 实体类 BD_ComLogistics
    /// </summary>
    public class BD_ComLogistics
    {
        public BD_ComLogistics()
        { }
        #region Model
        private int _id;
        private int _compid;
        private string _logisticsname;
        private string _logisticscode;
        private int _enabled;
        private DateTime _createdate;
        private int _createuserid;
        private int _dr;
        private int _modifyuser;
        private DateTime _ts;
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
        public string LogisticsName
        {
            set { _logisticsname = value; }
            get { return _logisticsname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LogisticsCode
        {
            set { _logisticscode = value; }
            get { return _logisticscode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
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
        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
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
        public DateTime ts
        {
            set { _ts = value; }
            get { return _ts; }
        }
        #endregion Model
    }
}
