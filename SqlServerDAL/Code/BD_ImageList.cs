//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/3/30 10:10:07
//
// 功能描述: 
//
// 修改标识: 
// 修改描述: 
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Collections;
using System.Collections.Generic;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 BD_ImageList
    /// </summary>
    public partial class BD_ImageList
    {
        public BD_ImageList()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_ImageList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_ImageList](");
            strSql.Append("[CompID],[GoodsID],[Pic],[Pic2],[Pic3],[IsIndex],[CreateUserID],[CreateDate],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsID,@Pic,@Pic2,@Pic3,@IsIndex,@CreateUserID,@CreateDate,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@Pic", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic2", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic3", SqlDbType.VarChar,128),
                    new SqlParameter("@IsIndex", SqlDbType.SmallInt),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsID;

            if (model.Pic != null)
                parameters[2].Value = model.Pic;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Pic2 != null)
                parameters[3].Value = model.Pic2;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Pic3 != null)
                parameters[4].Value = model.Pic3;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.IsIndex;
            parameters[6].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[7].Value = model.CreateDate;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[8].Value = model.ts;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.dr;
            parameters[10].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_ImageList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_ImageList] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[Pic]=@Pic,");
            strSql.Append("[Pic2]=@Pic2,");
            strSql.Append("[Pic3]=@Pic3,");
            strSql.Append("[IsIndex]=@IsIndex,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@Pic", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic2", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic3", SqlDbType.VarChar,128),
                    new SqlParameter("@IsIndex", SqlDbType.SmallInt),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsID;

            if (model.Pic != null)
                parameters[3].Value = model.Pic;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Pic2 != null)
                parameters[4].Value = model.Pic2;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Pic3 != null)
                parameters[5].Value = model.Pic3;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsIndex;
            parameters[7].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[8].Value = model.CreateDate;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[9].Value = model.ts;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.dr;
            parameters[11].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_ImageList] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_ImageList]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_ImageList]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_ImageList GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_ImageList] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow r = ds.Tables[0].Rows[0];
                return GetModel(r);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取数据集,建议只在多表联查时使用
        /// </summary>
        public DataSet GetDataSet(string strSql)
        {
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql);
        }

        /// <summary>
        /// 获取泛型数据列表,建议只在多表联查时使用
        /// </summary>
        public IList<Hi.Model.BD_ImageList> GetList(string strSql)
        {
            return GetList(GetDataSet(strSql));
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_ImageList]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_ImageList> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_ImageList> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_ImageList]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_ImageList GetModel(DataRow r)
        {
            Hi.Model.BD_ImageList model = new Hi.Model.BD_ImageList();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.GoodsID = SqlHelper.GetInt(r["GoodsID"]);
            model.Pic = SqlHelper.GetString(r["Pic"]);
            model.Pic2 = SqlHelper.GetString(r["Pic2"]);
            model.Pic3 = SqlHelper.GetString(r["Pic3"]);
            model.IsIndex = SqlHelper.GetInt(r["IsIndex"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_ImageList> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_ImageList>(ds.Tables[0]);
        }
    }
}
