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
    /// 实体类 DIS_OrderLog
    /// </summary>
    public class DIS_OrderLog
    {
        public DIS_OrderLog()
        { }
        #region Model
        private int _id;
        private int _receiptid;
        private string _receiptno;
        private int _receipttype;
        private int _receiptstate;
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
        public int ReceiptID
        {
            set { _receiptid = value; }
            get { return _receiptid; }
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
        public int ReceiptType
        {
            set { _receipttype = value; }
            get { return _receipttype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ReceiptState
        {
            set { _receiptstate = value; }
            get { return _receiptstate; }
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
