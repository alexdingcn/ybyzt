using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Hi.BLL
{
    public partial class BD_Distributor
    {
        /// <summary>
        /// 通过账号得到一个经销商实体
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Hi.Model.BD_Distributor GetDisID(string username)
        {
            return dal.GetDisID(username);
        }

        public List<Hi.Model.BD_Distributor> GetList(string strWhat, string strWhere, string strOrderby,System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby,Tran) as List<Hi.Model.BD_Distributor>;
        }

        /// <summary>
        /// 通过账号获取经销商地区,分类,资料
        /// </summary>
        public DataTable GetDis(string username)
        {
            return dal.GetDis(username).Tables[0];
        }

        public int Add(Hi.Model.BD_Distributor model,System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Distributor model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Distributor GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }

        /// <summary>
        /// 获取用户以及用户明细表数据，Distributor有别名A，注意
        /// </summary>
        public DataTable GetList(int pageSize, int pageIndex, string fldSort, bool sort, string fldName, string TbName, string strCondition, out int pageCount, out int count)
        {
            return dal.GetList(pageSize, pageIndex, fldSort, sort, fldName, TbName, strCondition, out pageCount, out count);
        }


        public DataTable getDataTable(int pageSize, int pageIndex, string strWhere, out int pageCount, out int Counts, int CountNum)
        {
            return dal.getDataTable(pageSize, pageIndex, strWhere, out pageCount, out Counts, CountNum);
        }
    }
}
