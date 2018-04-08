//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2018 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2018/1/29 13:10:53
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
    /// 实体类 DIS_GoodsStock
    /// </summary>
    public class DIS_GoodsStock
    {
        public DIS_GoodsStock()
        { }
        #region Model
        private int _id;
        private int _disid;
        private int _compid;
        private int _goodsid;
        private int _issale;
        private string _goodsinfo;
        private string _batchno;
        private DateTime _validdate;
        private decimal _stocktotalnum;
        private decimal _stockusenum;
        private decimal _stocknum;
        private decimal _minalertnum;
        private decimal _maxalertnum;
        private decimal _price;
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
        public int DisID
        {
            set { _disid = value; }
            get { return _disid; }
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
        public int GoodsID
        {
            set { _goodsid = value; }
            get { return _goodsid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsSale
        {
            set { _issale = value; }
            get { return _issale; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GoodsInfo
        {
            set { _goodsinfo = value; }
            get { return _goodsinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BatchNO
        {
            set { _batchno = value; }
            get { return _batchno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime validDate
        {
            set { _validdate = value; }
            get { return _validdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal StockTotalNum
        {
            set { _stocktotalnum = value; }
            get { return _stocktotalnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal StockUseNum
        {
            set { _stockusenum = value; }
            get { return _stockusenum; }
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
        public decimal MinAlertNum
        {
            set { _minalertnum = value; }
            get { return _minalertnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal MaxAlertNum
        {
            set { _maxalertnum = value; }
            get { return _maxalertnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
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
