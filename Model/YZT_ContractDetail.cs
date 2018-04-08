//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/12/23 16:31:42
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
    /// 实体类 YZT_ContractDetail
    /// </summary>
    public class YZT_ContractDetail
    {
        public YZT_ContractDetail()
        { }
        #region Model
        private int _id;
        private int _contid;
        private int _goodsid;
        private string _goodscode;
        private string _goodsname;
        private string _valueinfo;
        private int _htid;
        private decimal _saleprice;
        private decimal _discount;
        private decimal _tinkerprice;
        private decimal _target;
        private string _remark;
        private string _vdef1;
        private string _vdef2;
        private string _vdef3;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private int _fcid;
        private int _areaid;


        /// <summary>
        /// 首营id
        /// </summary>
        public int FCID
        {
            set { _fcid = value; }
            get { return _fcid; }
        }


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
        public int ContID
        {
            set { _contid = value; }
            get { return _contid; }
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
        public string GoodsCode
        {
            set { _goodscode = value; }
            get { return _goodscode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GoodsName
        {
            set { _goodsname = value; }
            get { return _goodsname; }
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
        public int HtID
        {
            set { _htid = value; }
            get { return _htid; }
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
        public decimal discount
        {
            set { _discount = value; }
            get { return _discount; }
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
        public decimal target
        {
            set { _target = value; }
            get { return _target; }
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
        public string vdef1
        {
            set { _vdef1 = value; }
            get { return _vdef1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef2
        {
            set { _vdef2 = value; }
            get { return _vdef2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef3
        {
            set { _vdef3 = value; }
            get { return _vdef3; }
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
        public int AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        #endregion Model
    }
}
