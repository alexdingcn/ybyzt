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
    /// 实体类 DIS_OrderOutDetail
    /// </summary>
    public class DIS_OrderOutDetail
    {
        public DIS_OrderOutDetail()
        { }
        #region Model
        private int _id;
        private int _orderoutid;
        private int _disid;
        private int _goodsinfoid;
        private int _orderid;
        private decimal _outnum;
        private decimal _signnum;
        private string _remark;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private string _batchno;
        private DateTime _validdate;
        private decimal _storageNum;

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
        public int OrderOutID
        {
            set { _orderoutid = value; }
            get { return _orderoutid; }
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
        public int GoodsinfoID
        {
            set { _goodsinfoid = value; }
            get { return _goodsinfoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal OutNum
        {
            set { _outnum = value; }
            get { return _outnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SignNum
        {
            set { _signnum = value; }
            get { return _signnum; }
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
        /// <summary>
        /// 
        /// </summary>
        public decimal StorageNum
        {
            get { return _storageNum; }
            set { _storageNum = value; }
        }
        #endregion Model
    }
}
