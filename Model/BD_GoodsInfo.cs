//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/8/31 9:27:51
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
    /// 实体类 BD_GoodsInfo
    /// </summary>
    [Serializable]
    public class BD_GoodsInfo
    {
        public BD_GoodsInfo()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _goodsid;
        private string _barcode;
        private int _isoffline;
        private decimal _inventory;
        private string _value1;
        private string _value2;
        private string _value3;
        private string _value4;
        private string _value5;
        private string _value6;
        private string _value7;
        private string _value8;
        private string _value9;
        private string _value10;
        private string _valueinfo;
        private decimal _saleprice;
        private decimal _tinkerprice;
        private bool _isenabled;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private string _batchno;
        private DateTime _validdate;
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
        public int GoodsID
        {
            set { _goodsid = value; }
            get { return _goodsid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BarCode
        {
            set { _barcode = value; }
            get { return _barcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsOffline
        {
            set { _isoffline = value; }
            get { return _isoffline; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Inventory
        {
            set { _inventory = value; }
            get { return _inventory; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value1
        {
            set { _value1 = value; }
            get { return _value1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value2
        {
            set { _value2 = value; }
            get { return _value2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value3
        {
            set { _value3 = value; }
            get { return _value3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value4
        {
            set { _value4 = value; }
            get { return _value4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value5
        {
            set { _value5 = value; }
            get { return _value5; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value6
        {
            set { _value6 = value; }
            get { return _value6; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value7
        {
            set { _value7 = value; }
            get { return _value7; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value8
        {
            set { _value8 = value; }
            get { return _value8; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value9
        {
            set { _value9 = value; }
            get { return _value9; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value10
        {
            set { _value10 = value; }
            get { return _value10; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ValueInfo
        {
            set { _valueinfo = value; }
            get { return _valueinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SalePrice
        {
            set { _saleprice = value; }
            get { return _saleprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal TinkerPrice
        {
            set { _tinkerprice = value; }
            get { return _tinkerprice; }
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

        /// <summary>
        /// 
        /// </summary>
        public string Batchno
        {
            get { return _batchno; }
            set { _batchno = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Validdate
        {
            get { return _validdate; }
            set { _validdate = value; }
        }
        #endregion Model
    }
}
