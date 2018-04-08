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
    /// 实体类 DIS_Logistics
    /// </summary>
    public class DIS_Logistics
    {
        public DIS_Logistics()
        { }
        #region Model
        private int _id;
        private int _orderid;
        private int _orderoutid;
        private string _compname;
        private string _logisticsno;
        private string _caruser;
        private string _carno;
        private string _car;
        private string _context;
        private int _type;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
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
        public int OrderOutID
        {
            set { _orderoutid = value; }
            get { return _orderoutid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ComPName
        {
            set { _compname = value; }
            get { return _compname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LogisticsNo
        {
            set { _logisticsno = value; }
            get { return _logisticsno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarUser
        {
            set { _caruser = value; }
            get { return _caruser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarNo
        {
            set { _carno = value; }
            get { return _carno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Car
        {
            set { _car = value; }
            get { return _car; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Context
        {
            set { _context = value; }
            get { return _context; }
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
