//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/4/23 11:28:57
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
    /// 数据访问类 BD_ShopImageList
    /// </summary>
    public partial class BD_ShopImageList
    {
        public BD_ShopImageList()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public  int Add(Hi.Model.BD_ShopImageList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_ShopImageList](");
            strSql.Append("[CompID],[Type],[ImageUrl],[ImageName],[ImageTitle],[GoodsID],[GoodsUrl],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@Type,@ImageUrl,@ImageName,@ImageTitle,@GoodsID,@GoodsUrl,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar,150),
                    new SqlParameter("@ImageName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ImageTitle", SqlDbType.NVarChar,50),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsUrl", SqlDbType.VarChar,150),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.ImageUrl;

            if (model.ImageName != null)
                parameters[3].Value = model.ImageName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.ImageTitle != null)
                parameters[4].Value = model.ImageTitle;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.GoodsID;

            if (model.GoodsUrl != null)
                parameters[6].Value = model.GoodsUrl;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_ShopImageList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_ShopImageList] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[ImageUrl]=@ImageUrl,");
            strSql.Append("[ImageName]=@ImageName,");
            strSql.Append("[ImageTitle]=@ImageTitle,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsUrl]=@GoodsUrl,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar,150),
                    new SqlParameter("@ImageName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ImageTitle", SqlDbType.NVarChar,50),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsUrl", SqlDbType.VarChar,150),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.ImageUrl;

            if (model.ImageName != null)
                parameters[4].Value = model.ImageName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ImageTitle != null)
                parameters[5].Value = model.ImageTitle;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.GoodsID;

            if (model.GoodsUrl != null)
                parameters[7].Value = model.GoodsUrl;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.ts;
            parameters[11].Value = model.dr;
            parameters[12].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_ShopImageList] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_ShopImageList]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_ShopImageList]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_ShopImageList GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_ShopImageList] ");
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
        public IList<Hi.Model.BD_ShopImageList> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_ShopImageList]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_ShopImageList> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_ShopImageList> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_ShopImageList]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_ShopImageList GetModel(DataRow r)
        {
            Hi.Model.BD_ShopImageList model = new Hi.Model.BD_ShopImageList();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.Type = SqlHelper.GetInt(r["Type"]);
            model.ImageUrl = SqlHelper.GetString(r["ImageUrl"]);
            model.ImageName = SqlHelper.GetString(r["ImageName"]);
            model.ImageTitle = SqlHelper.GetString(r["ImageTitle"]);
            model.GoodsID = SqlHelper.GetInt(r["GoodsID"]);
            model.GoodsUrl = SqlHelper.GetString(r["GoodsUrl"]);
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
        private IList<Hi.Model.BD_ShopImageList> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_ShopImageList>(ds.Tables[0]);
        }
    }
}
