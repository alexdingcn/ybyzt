//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/4/12 15:47:49
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
    /// 实体类 SYS_UserMessage
    /// </summary>
    public class SYS_UserMessage
    {
        public SYS_UserMessage()
        { }
        #region Model
        private int _id;
        private string _username;
        private string _userphone;
        private string _usermailqq;
        private string _usermessge;
        private DateTime _createdate;
        private DateTime _modifydate;
        private int _modifyuser;
        private int _state;
        private string _remark;
        private int _dr;
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
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserPhone
        {
            set { _userphone = value; }
            get { return _userphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserMailQQ
        {
            set { _usermailqq = value; }
            get { return _usermailqq; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserMessge
        {
            set { _usermessge = value; }
            get { return _usermessge; }
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
        public DateTime ModifyDate
        {
            set { _modifydate = value; }
            get { return _modifydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ModifyUser
        {
            set { _modifyuser = value; }
            get { return _modifyuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
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
        public int dr
        {
            set { _dr = value; }
            get { return _dr; }
        }
        #endregion Model
    }
}
