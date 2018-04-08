using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Hi.BLL
{
    public partial class BD_AttributeValues
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_AttributeValues model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_AttributeValues model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        public List<Hi.Model.BD_AttributeValues> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_AttributeValues>;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_AttributeValues GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }

    }
}
