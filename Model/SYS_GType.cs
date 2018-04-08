//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/6/30 15:10:16
//
// 功能描述: 
//
// 修改标识: 
// 修改描述: 
//------------------------------------------------------------------------------

using System;

namespace Hi.Model
{
    [Serializable]
    /// <summary>
    /// 实体类 SYS_GType
    /// </summary>
    public class SYS_GType
    {
        public SYS_GType()
        { }
        #region Model
        private int _id;
        private string _typecode;
        private string _typename;
        private int _parentid;
        private int _deep;
        private string _fullcode;
        private bool _isend;
        private string _sortindex;
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
        public string TypeCode
        {
            set { _typecode = value; }
            get { return _typecode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TypeName
        {
            set { _typename = value; }
            get { return _typename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Deep
        {
            set { _deep = value; }
            get { return _deep; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FullCode
        {
            set { _fullcode = value; }
            get { return _fullcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnd
        {
            set { _isend = value; }
            get { return _isend; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SortIndex
        {
            set { _sortindex = value; }
            get { return _sortindex; }
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
