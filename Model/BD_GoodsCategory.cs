//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/4/29 17:45:56
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
    /// 实体类 BD_GoodsCategory
    /// </summary>
    [Serializable]
    public class BD_GoodsCategory
    {
        public BD_GoodsCategory()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _goodstypeid;
        private string _categorycode;
        private string _categoryname;
        private int _parentid;
        private string _sortindex;
        private string _code;
        private int _deep;
        private string _parcode;
        private string _othercode;
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
        public int GoodsTypeID
        {
            set { _goodstypeid = value; }
            get { return _goodstypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode
        {
            set { _categorycode = value; }
            get { return _categorycode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryName
        {
            set { _categoryname = value; }
            get { return _categoryname; }
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
        public string SortIndex
        {
            set { _sortindex = value; }
            get { return _sortindex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
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
        public string ParCode
        {
            set { _parcode = value; }
            get { return _parcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OtherCode
        {
            set { _othercode = value; }
            get { return _othercode; }
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
