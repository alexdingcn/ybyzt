//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/7/7 11:26:45
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
    /// 实体类 DIS_Suggest
    /// </summary>
    public class DIS_Suggest
    {
        public DIS_Suggest()
        { }
        #region Model
        private int _id;
        private string _title;
        private int _disuserid;
        private int _compid;
        private int _compuserid;
        private string _remark;
        private string _compremark;
        private DateTime _createdate;
        private DateTime _replydate;
        private int _stype;
        private string _receiptno;
        private int _suggest;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private int _disid;
        private int _isanswer;
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
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DisUserID
        {
            set { _disuserid = value; }
            get { return _disuserid; }
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
        public int CompUserID
        {
            set { _compuserid = value; }
            get { return _compuserid; }
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
        public string CompRemark
        {
            set { _compremark = value; }
            get { return _compremark; }
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
        public DateTime ReplyDate
        {
            set { _replydate = value; }
            get { return _replydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Stype
        {
            set { _stype = value; }
            get { return _stype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiptNo
        {
            set { _receiptno = value; }
            get { return _receiptno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Suggest
        {
            set { _suggest = value; }
            get { return _suggest; }
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
        public int DisID
        {
            set { _disid = value; }
            get { return _disid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsAnswer
        {
            set { _isanswer = value; }
            get { return _isanswer; }
        }
        #endregion Model
    }
}
