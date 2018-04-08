//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/3/21 14:10:30
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
    /// 实体类 BD_Promotion
    /// </summary>
    public class BD_Promotion
    {
        public BD_Promotion()
        { }
        #region Model
        private int _id;
        private int _compid;
        private string _protitle;
        private int _type;
        private int _protype;
        private decimal _discount;
        private string _proinfos;
        private int _isenabled;
        private DateTime _prostarttime;
        private DateTime _proendtime;
        private int _createuserid;
        private DateTime _createdate;
        private string _ts;
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
        public string ProTitle
        {
            set { _protitle = value; }
            get { return _protitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ProType
        {
            set { _protype = value; }
            get { return _protype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Discount
        {
            set { _discount = value; }
            get { return _discount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProInfos
        {
            set { _proinfos = value; }
            get { return _proinfos; }
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
        public DateTime ProStartTime
        {
            set { _prostarttime = value; }
            get { return _prostarttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ProEndTime
        {
            set { _proendtime = value; }
            get { return _proendtime; }
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
        public string ts
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
