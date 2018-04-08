using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Hi.BLL
{
    public partial class BD_Attribute
    {
        /// <summary>
        /// 获取属性以及属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetAttributrList(string id,string compid)
        {
            return dal.GetAttrbuteList(id, compid);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Attribute model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Attribute model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        public List<Hi.Model.BD_Attribute> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_Attribute>;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Attribute GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }

    }
}
