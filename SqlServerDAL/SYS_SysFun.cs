using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data;

namespace Hi.SQLServerDAL
{
    public partial class SYS_SysFun
    {

        #region  成员方法


        //获得所有父节点信息
        public IList<Hi.Model.SYS_SysFun> GetParentList()
        {
            return GetList(GetDataSet(null, "ParentCode = ''", "SortIndex"));

        }

        //根据父节点获得所有子节点信息
        public IList<Hi.Model.SYS_SysFun> GetSysFunByParentNodeId(string FunCode)
        {
            return GetList(GetDataSet(null, "ParentCode = '" + FunCode + "'", "SortIndex"));
        }
        ////根据登录用户Id获得所有父节点信息
        //public DataSet GetParentListByUserId(string userId)
        //{
        //    string strSql = "select * from View_AdminUser where FunCode = '0' and LoginID='" + userId + "' order by SortIndex";
        //    DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        //    return ds;
        //}

        ////根据登录用户Id获得所有子节点点信息
        //public DataSet GetSysFunByParentNodeIdAndUserId(int FunCode, string userId)
        //{
        //    string sql = "select * from View_AdminUser where FunCode = " + FunCode + " and LoginID='" + userId + "' order by SortIndex";
        //    DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString());
        //    return ds;
        //}


        #endregion


        public IList<Hi.Model.SYS_SysFun> GetAllListCascade()
        {
            IList<Hi.Model.SYS_SysFun> lst = new List<Hi.Model.SYS_SysFun>();
            IList<Hi.Model.SYS_SysFun> parentList = this.GetParentList();
            for (int i = 0; i < parentList.Count; i++)
            {
                lst.Add(parentList[i]);
                IList<Hi.Model.SYS_SysFun> secondList = this.GetSysFunByParentNodeId(parentList[i].FunCode);
                for (int j = 0; j < secondList.Count; j++)
                {
                    secondList[j].FunName = "┣━" + secondList[j].FunName;
                    lst.Add(secondList[j]);
                }
            }
            return lst;
        }

    }
}
