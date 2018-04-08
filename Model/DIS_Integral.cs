//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/20 17:36:24
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
    /// 实体类 DIS_Integral
    /// </summary>
    public class DIS_Integral
    {
        public DIS_Integral()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _disid;
        private int _orderid;
        private string _integraltype;
        private decimal _oldintegral;
        private decimal _integral;
        private decimal _newintegral;
        private string _source;
        private string _remarks;
        private DateTime _createdate;
        private int _type;
        private int _isview;
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
        public int DisID
        {
            set { _disid = value; }
            get { return _disid; }
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
        public string IntegralType
        {
            set { _integraltype = value; }
            get { return _integraltype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal OldIntegral
        {
            set { _oldintegral = value; }
            get { return _oldintegral; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Integral
        {
            set { _integral = value; }
            get { return _integral; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal NewIntegral
        {
            set { _newintegral = value; }
            get { return _newintegral; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Source
        {
            set { _source = value; }
            get { return _source; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
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
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsView
        {
            set { _isview = value; }
            get { return _isview; }
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
