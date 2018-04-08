using System;
using System.Collections.Generic;
using System.Text;


namespace Hi.BLL
{
    public partial class BD_CategoryAttribute
    {

        /// <summary>
        /// 删除类别属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Deletes(List<int> l, string compid)
        {
            foreach (int ID in l)
            {
                dal.Deletes(ID.ToString(), compid);
            }
        }
        /// <summary>
        /// 类别和属性显示
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public string Bind(string where)
        {
            return dal.Bind(where);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_CategoryAttribute model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_CategoryAttribute model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        public List<Hi.Model.BD_CategoryAttribute> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_CategoryAttribute>;
        }
    }
}