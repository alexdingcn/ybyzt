//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/23 13:23:52
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
    /// 实体类 BD_CompNews
    /// </summary>
    public class BD_CompNews
    {
        public BD_CompNews()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _pmid;
        private int _newstype;
        private string _newstitle;
        private string _newscontents;
        private int _istop;
        private string _showtype;
        private int _isenabled;
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
        public int PmID
        {
            set { _pmid = value; }
            get { return _pmid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int NewsType
        {
            set { _newstype = value; }
            get { return _newstype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NewsTitle
        {
            set { _newstitle = value; }
            get { return _newstitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NewsContents
        {
            set { _newscontents = value; }
            get { return _newscontents; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsTop
        {
            set { _istop = value; }
            get { return _istop; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShowType
        {
            set { _showtype = value; }
            get { return _showtype; }
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
        #endregion Model
    }
}
