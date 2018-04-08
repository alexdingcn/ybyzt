//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/11/30 10:32:07
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
    /// 实体类 BD_Goods
    /// </summary>
    public class BD_Goods
    {
        public BD_Goods()
        { }
        #region Model
        private int _id;
        private int _compid;
        private string _goodsname;
        private string _goodscode;
        private int _categoryid;
        private string _unit;
        private decimal _saleprice;
        private int _isoffline;
        private int _isindex;
        private int _issale;
        private int _isrecommended;
        private DateTime _offlinestatedate;
        private DateTime _offlineenddate;
        private string _pic;
        private string _pic2;
        private string _pic3;
        private string _title;
        private string _hideinfo1;
        private string _hideinfo2;
        private string _details;
        private int _isattribute;
        private int _templateid;
        private string _value1;
        private string _value2;
        private string _value3;
        private string _value4;
        private string _value5;
        private string _memo;
        private int _isenabled;
        private int _createuserid;
        private DateTime _createdate;
        private DateTime _ts;
        private int _dr;
        private int _modifyuser;
        private string _viewinfos;
        private int _viewinfoid;
        private bool _isfirstshow;
        private int _sortindex;
        private string _newpic;
        private string _showname;
        private int _isls;
        private decimal _lsprice;
        private string _registeredCertificate;
        private DateTime _validity;

        /// <summary>
        /// 
        /// </summary>
        public string registeredCertificate
        {
            set { _registeredCertificate = value; }
            get { return _registeredCertificate; }
        }


        /// <summary>
        /// 
        /// </summary>
        public DateTime validity
        {
            set { _validity = value; }
            get { return _validity; }
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
        public int CompID
        {
            set { _compid = value; }
            get { return _compid; }
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
        public string GoodsCode
        {
            set { _goodscode = value; }
            get { return _goodscode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CategoryID
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
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
        public int IsOffline
        {
            set { _isoffline = value; }
            get { return _isoffline; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsIndex
        {
            set { _isindex = value; }
            get { return _isindex; }
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
        public int IsRecommended
        {
            set { _isrecommended = value; }
            get { return _isrecommended; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OfflineStateDate
        {
            set { _offlinestatedate = value; }
            get { return _offlinestatedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OfflineEndDate
        {
            set { _offlineenddate = value; }
            get { return _offlineenddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Pic
        {
            set { _pic = value; }
            get { return _pic; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Pic2
        {
            set { _pic2 = value; }
            get { return _pic2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Pic3
        {
            set { _pic3 = value; }
            get { return _pic3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HideInfo1
        {
            set { _hideinfo1 = value; }
            get { return _hideinfo1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HideInfo2
        {
            set { _hideinfo2 = value; }
            get { return _hideinfo2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Details
        {
            set { _details = value; }
            get { return _details; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsAttribute
        {
            set { _isattribute = value; }
            get { return _isattribute; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TemplateId
        {
            set { _templateid = value; }
            get { return _templateid; }
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
        public string memo
        {
            set { _memo = value; }
            get { return _memo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled
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
        public string ViewInfos
        {
            set { _viewinfos = value; }
            get { return _viewinfos; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ViewInfoID
        {
            set { _viewinfoid = value; }
            get { return _viewinfoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsFirstShow
        {
            set { _isfirstshow = value; }
            get { return _isfirstshow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sortindex
        {
            set { _sortindex = value; }
            get { return _sortindex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NewPic
        {
            set { _newpic = value; }
            get { return _newpic; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShowName
        {
            set { _showname = value; }
            get { return _showname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsLS
        {
            set { _isls = value; }
            get { return _isls; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal LSPrice
        {
            set { _lsprice = value; }
            get { return _lsprice; }
        }
        #endregion Model
    }
}
