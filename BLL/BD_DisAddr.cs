using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Hi.BLL
{
    public partial class BD_DisAddr
    {
        /// <summary>
        /// 根据DisID查询当前经销商下的所有收货地址
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public DataSet GetModel(string username)
        {
            return dal.GetModel(username);
        }

        /// <summary>
        /// 根据用户修改当前经销商下的默认地址
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UpdateS(string username)
        {
            return dal.UpdateS(username);
        }

        public int Add(Hi.Model.BD_DisAddr model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }

        public bool Update(Hi.Model.BD_DisAddr model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }

        /// <summary>
        /// 根据用户查询当前经销商下的默认地址
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Hi.Model.BD_DisAddr GetDefaultAddr(string username)
        {
            return dal.GetDefaultAddr(username);
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        /// <param name="strWhat">字段</param>
        /// <param name="TbName">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderby">排序字段</param>
        /// <returns></returns>
        public DataTable GetList(string strWhat, string TbName, string strWhere, string strOrderby)
        {
            return dal.GetList(strWhat, TbName, strWhere, strOrderby);
        }
    }
}
