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
    /// 实体类 BD_PromotionDetail
    /// </summary>
    public class BD_PromotionDetail
    {
        public BD_PromotionDetail()
        { }
        #region Model
        private int _id;
        private int _proid;
        private int _compid;
        private int _goodsid;
        private string _goodsname;
        private string _goodsunit;
        private string _goodsmemo;
        private int _goodinfoid;
        private decimal _goodsprice;
        private decimal _sendgoodsinfoid;
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
        public int ProID
        {
            set { _proid = value; }
            get { return _proid; }
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
        public string GoodsName
        {
            set { _goodsname = value; }
            get { return _goodsname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GoodsUnit
        {
            set { _goodsunit = value; }
            get { return _goodsunit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Goodsmemo
        {
            set { _goodsmemo = value; }
            get { return _goodsmemo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GoodInfoID
        {
            set { _goodinfoid = value; }
            get { return _goodinfoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal GoodsPrice
        {
            set { _goodsprice = value; }
            get { return _goodsprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SendGoodsinfoID
        {
            set { _sendgoodsinfoid = value; }
            get { return _sendgoodsinfoid; }
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
