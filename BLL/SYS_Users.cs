using System;
using System.Collections.Generic;
using System.Text;

namespace Hi.BLL
{
    public partial class SYS_Users
    {
        public Hi.Model.SYS_Users GetModel(string username)
        {
            return dal.GetModel(username);
        }

        public Hi.Model.SYS_Users GetModelphone(string phone)
        {
            return dal.GetModelphone(phone);
        }

        public Hi.Model.SYS_Users GetDisid(string disid)
        {
            return dal.GetDisid(disid);
        }

        /// <summary>
        /// 修改密码
        /// <param name="Id">用户ID</param>
        /// <param name="NewPassWord">新密码</param>
        /// </summary>
        public bool UpdatePassWord(string NewPassWord, string Id)
        {
            return dal.UpdatePassWord(NewPassWord, Id);
        }

        public int Add(Hi.Model.SYS_Users model,System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model,Tran);
        }

        public List<Hi.Model.SYS_Users> GetList(string strWhat, string strWhere, string strOrderby,System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.SYS_Users>;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_Users model,System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model,Tran);
        }


        public List<Hi.Model.SYS_Users> GetListUser(string strWhat, string Key, string Value, string strOrderby)
        {
            return dal.GetListUser(strWhat, Key, Value, strOrderby) as List<Hi.Model.SYS_Users>;
        }
    }
}
