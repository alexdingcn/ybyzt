//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/11/10 16:45:20
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
    /// 数据访问类 BD_ShopMessage
    /// </summary>
    public class BD_ShopMessage
    {
        public BD_ShopMessage()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_ShopMessage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_ShopMessage](");
            strSql.Append("[CompID],[Name],[Phone],[Remark],[IsRead],[CreateDate])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@Name,@Phone,@Remark,@IsRead,@CreateDate)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Name", SqlDbType.NVarChar,20),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,500),
                    new SqlParameter("@IsRead", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.CompID;

            if (model.Name != null)
                parameters[1].Value = model.Name;
            else
                parameters[1].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[2].Value = model.Phone;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[3].Value = model.Remark;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.IsRead;

            if (model.CreateDate != DateTime.MinValue)
                parameters[5].Value = model.CreateDate;
            else
                parameters[5].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_ShopMessage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_ShopMessage] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[Name]=@Name,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsRead]=@IsRead,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[dr]=@dr");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Name", SqlDbType.NVarChar,20),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,500),
                    new SqlParameter("@IsRead", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.Name != null)
                parameters[2].Value = model.Name;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[3].Value = model.Phone;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[4].Value = model.Remark;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.IsRead;

            if (model.CreateDate != DateTime.MinValue)
                parameters[6].Value = model.CreateDate;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.dr;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_ShopMessage] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_ShopMessage]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_ShopMessage]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_ShopMessage GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_ShopMessage] ");
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
        public IList<Hi.Model.BD_ShopMessage> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_ShopMessage]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_ShopMessage> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_ShopMessage> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_ShopMessage]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_ShopMessage GetModel(DataRow r)
        {
            Hi.Model.BD_ShopMessage model = new Hi.Model.BD_ShopMessage();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.Name = SqlHelper.GetString(r["Name"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.IsRead = SqlHelper.GetInt(r["IsRead"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_ShopMessage> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_ShopMessage>(ds.Tables[0]);
        }
    }
}
