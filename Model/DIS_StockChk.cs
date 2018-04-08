//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/2/6 13:03:24
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
    /// 实体类 DIS_StockChk
    /// </summary>
    public class DIS_StockChk
    {
        public DIS_StockChk()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _stockorderid;
        private int _goodsid;
        private int _goodsinfoid;
        private decimal _stockoldnum;
        private decimal _stocknum;
        private string _remark;
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
        public int StockOrderID
        {
            set { _stockorderid = value; }
            get { return _stockorderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GoodsID
        {
            set { _goodsid = value; }
            get { return _goodsid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GoodsInfoID
        {
            set { _goodsinfoid = value; }
            get { return _goodsinfoid; }
        }
        public decimal StockOldNum
        {
            set { _stockoldnum = value; }
            get { return _stockoldnum; }
            
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal StockNum
        {
            set { _stocknum = value; }
            get { return _stocknum; }
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
