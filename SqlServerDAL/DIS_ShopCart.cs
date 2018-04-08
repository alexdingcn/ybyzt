using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DBUtility;
using System.Reflection;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 DIS_ShopCart
    /// </summary>
    public partial class DIS_ShopCart
    {
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
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from ");

            if (string.IsNullOrEmpty(TbName))
                TbName = "[DIS_ShopCart]";
            else
                strSql.Append(TbName);

            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// datatable转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> GetListEntity<T>(DataTable table)
        {
            if (table != null && table.Rows.Count > 0)
            {
                List<T> resultList = new List<T>();
                foreach (DataRow dr in table.Rows)
                {
                    T tModel = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in typeof(T).GetProperties())
                    {
                        if (table.Columns.Contains(property.Name))
                        {
                            if (DBNull.Value != dr[property.Name])

                                property.SetValue(tModel, dr[property.Name], null);

                        }
                    }
                    resultList.Add(tModel);
                }
                return resultList;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fldSort"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Hi.Model.DIS_ShopCart> GetModel(string fldSort, string strWhere)
        {
            DataTable dt = null;
            string strWhat = "*";
            if (!string.IsNullOrEmpty(fldSort))
            {
                strWhat = fldSort;
            }
            string Sql = "select " + strWhat + " from DIS_ShopCart where " + strWhere + "";
            dt = SqlHelper.Query(SqlHelper.LocalSqlServer, Sql).Tables[0];
            return GetListEntity<Hi.Model.DIS_ShopCart>(dt);
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        /// <param name="strWhat">字段</param>
        /// <param name="TbName">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderby">排序字段</param>
        /// <returns></returns>
        public DataTable GetListCart(string strWhat, string TbName, string strWhere, string strOrderby)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_ShopCart]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 购物车与商品的连接查询
        /// </summary>
        /// <returns></returns>
        public DataTable GetGoodsCart(string strWhere, string strOrderby)
        {
            StringBuilder strSql = new StringBuilder("select sc.[ID],sc.[CompID],sc.[DisID],sc.[GoodsID],sc.[GoodsinfoID],sc.[GoodsInfos],sc.[GoodsNum],sc.[Price],sc.[AuditAmount],sc.[sumAmount],sc.[ProID],sc.[ProType],sc.[DisCount],sc.[ProNum],sc.[CreateDate],gd.[GoodsName],gd.[Pic],gd.[Unit],info.barCode,sc.[vdef1],dc.ID as dcID,info.BarCode,info.Inventory from DIS_ShopCart as sc left join BD_Goods as gd on sc.goodsid =gd.ID left join BD_DisCollect as dc on sc.goodsid=dc.GoodsID and dc.DisID= sc.DisID left join BD_GoodsInfo as info on sc.GoodsInfoID=info.ID");

            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);

            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="wheresrt"></param>
        /// <returns></returns>
        public bool CartEmpty(string wheresrt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_ShopCart] ");

            if (!string.IsNullOrEmpty(wheresrt))
                strSql.Append(" where " + wheresrt);

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString()) > 0;
        }

        /// <summary>
        /// 返回单个商品小计
        /// </summary>
        /// <param name="CompID"></param>
        /// <param name="DisID"></param>
        /// <param name="GoodsinfoID"></param>
        /// <returns></returns>
        public decimal GetAuditAmount(string CompID, string DisID, string GoodsinfoID)
        {
            StringBuilder strSql = new StringBuilder("select sumAmount from [DIS_ShopCart]   where CompID=" + CompID + " and DisID=" + DisID+" and GoodsInfoID="+GoodsinfoID);
            DataTable dt=SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToDecimal(dt.Rows[0]["sumAmount"].ToString());
            return 0;
        }

        /// <summary>
        /// 返回购物车商品总数量，总价
        /// </summary>
        /// <returns></returns>
        public DataTable SumCartNum(string CompID,string DisID)
        {
            StringBuilder strSql = new StringBuilder("select count(ID) as cart,SUM(GoodsNum) as cartsum,sum(sumAmount) as sumAmount,sum(Price*GoodsNum) as sumPrice from [DIS_ShopCart]   where CompID=" + CompID + " and DisID=" + DisID);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
        }

    }
}
