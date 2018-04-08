using System;
using System.Collections.Generic;
using System.Text;
using Hi.Model;
using System.Data;

namespace Hi.BLL
{
    public partial class SYS_SysFun
    {

        #region  成员方法

        /// <summary>
        /// 得到所有父节点
        /// </summary>
        /// <returns></returns>
        public IList<Model.SYS_SysFun> GetParentList()
        {
            return dal.GetParentList();
        }

        //根据父节点获得所有子节点信息
        public IList<Model.SYS_SysFun> GetSysFunByParentNodeId(string parentNodeId)
        {
            return dal.GetSysFunByParentNodeId(parentNodeId);
        }

        //根据登录用户Id获得所有父节点信息
        //public DataSet GetParentListByUserId(string userId)
        //{
        //    return dal.GetParentListByUserId(userId);
        //}

        ////根据登录用户Id获得所有子节点点信息
        //public DataSet GetSysFunByParentNodeIdAndUserId(int parentNodeId, string userId)
        //{
        //    return dal.GetSysFunByParentNodeIdAndUserId(parentNodeId, userId);
        //}

        #endregion

        public IList<Model.SYS_SysFun> GetAllListCascade()
        {
            return dal.GetAllListCascade();
        }
    }
}
