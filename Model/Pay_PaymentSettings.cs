//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/4/11 10:25:44
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
    /// 实体类 Pay_PaymentSettings
    /// </summary>
    public class Pay_PaymentSettings
    {
        public Pay_PaymentSettings()
        { }
        #region Model
        private int _id;
        private int _compid;
        private int _pay_sxfsq;
        private int _pay_zffs;
        private decimal _pay_kjzfbl;
        private decimal _pay_kjzfstart;
        private decimal _pay_kjzfend;
        private decimal _pay_ylzfbl;
        private decimal _pay_ylzfstart;
        private decimal _pay_ylzfend;
        private decimal _pay_b2cwyzfbl;
        private decimal _pay_b2bwyzf;
        private int _pay_mfcs;
        private int _createuser;
        private DateTime _createdate;
        private int _start;
        private string _remark;
        private string _vdef1;
        private string _vdef2;
        private string _vdef3;
        private string _vdef4;
        private string _vdef5;
        private string _vdef6;
        private string _vdef7;
        private string _vdef8;
        private string _vdef9;
        private string _vdef10;
        private string _vdef11;
        private string _vdef12;
        private string _vdef13;
        private string _vdef14;
        private string _vdef15;
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
        /// 手续费收取(0,平台 1，经销商 2，企业)
        /// </summary>
        public int pay_sxfsq
        {
            set { _pay_sxfsq = value; }
            get { return _pay_sxfsq; }
        }
        /// <summary>
        /// 支付方式(0,线上支付，1，线下支付)
        /// </summary>
        public int pay_zffs
        {
            set { _pay_zffs = value; }
            get { return _pay_zffs; }
        }
        /// <summary>
        /// B2C-快捷支付比例(2‰ )
        /// </summary>
        public decimal pay_kjzfbl
        {
            set { _pay_kjzfbl = value; }
            get { return _pay_kjzfbl; }
        }
        /// <summary>
        /// B2C-快捷支付封底
        /// </summary>
        public decimal pay_kjzfstart
        {
            set { _pay_kjzfstart = value; }
            get { return _pay_kjzfstart; }
        }
        /// <summary>
        /// B2C-快捷支付封顶
        /// </summary>
        public decimal pay_kjzfend
        {
            set { _pay_kjzfend = value; }
            get { return _pay_kjzfend; }
        }
        /// <summary>
        /// B2C-银联支付 比例(3‰ )
        /// </summary>
        public decimal pay_ylzfbl
        {
            set { _pay_ylzfbl = value; }
            get { return _pay_ylzfbl; }
        }
        /// <summary>
        /// B2C-银联支付封底
        /// </summary>
        public decimal pay_ylzfstart
        {
            set { _pay_ylzfstart = value; }
            get { return _pay_ylzfstart; }
        }
        /// <summary>
        /// B2C-银联支付 封顶
        /// </summary>
        public decimal pay_ylzfend
        {
            set { _pay_ylzfend = value; }
            get { return _pay_ylzfend; }
        }
        /// <summary>
        /// B2C-网银支付比例(2‰ )
        /// </summary>
        public decimal pay_b2cwyzfbl
        {
            set { _pay_b2cwyzfbl = value; }
            get { return _pay_b2cwyzfbl; }
        }
        /// <summary>
        /// B2B-网银支付 默认7元一笔
        /// </summary>
        public decimal pay_b2bwyzf
        {
            set { _pay_b2bwyzf = value; }
            get { return _pay_b2bwyzf; }
        }
        /// <summary>
        /// 免手续费支付次数
        /// </summary>
        public int Pay_mfcs
        {
            set { _pay_mfcs = value; }
            get { return _pay_mfcs; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public int createUser
        {
            set { _createuser = value; }
            get { return _createuser; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int Start
        {
            set { _start = value; }
            get { return _start; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
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
        public string vdef4
        {
            set { _vdef4 = value; }
            get { return _vdef4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef5
        {
            set { _vdef5 = value; }
            get { return _vdef5; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef6
        {
            set { _vdef6 = value; }
            get { return _vdef6; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef7
        {
            set { _vdef7 = value; }
            get { return _vdef7; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef8
        {
            set { _vdef8 = value; }
            get { return _vdef8; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef9
        {
            set { _vdef9 = value; }
            get { return _vdef9; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef10
        {
            set { _vdef10 = value; }
            get { return _vdef10; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef11
        {
            set { _vdef11 = value; }
            get { return _vdef11; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef12
        {
            set { _vdef12 = value; }
            get { return _vdef12; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef13
        {
            set { _vdef13 = value; }
            get { return _vdef13; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef14
        {
            set { _vdef14 = value; }
            get { return _vdef14; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vdef15
        {
            set { _vdef15 = value; }
            get { return _vdef15; }
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
