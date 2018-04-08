//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/12/23 16:31:42
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
    /// 数据访问类 YZT_Logistics
    /// </summary>
    public class YZT_Logistics
    {
        public YZT_Logistics()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Logistics model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_Logistics](");
            strSql.Append("[LibraryID],[ComPName],[LogisticsNo],[CarUser],[CarNo],[Car],[Context],[Type],[CreateUserID],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@LibraryID,@ComPName,@LogisticsNo,@CarUser,@CarNo,@Car,@Context,@Type,@CreateUserID,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@LibraryID", SqlDbType.Int),
                    new SqlParameter("@ComPName", SqlDbType.VarChar,50),
                    new SqlParameter("@LogisticsNo", SqlDbType.VarChar,50),
                    new SqlParameter("@CarUser", SqlDbType.VarChar,50),
                    new SqlParameter("@CarNo", SqlDbType.VarChar,50),
                    new SqlParameter("@Car", SqlDbType.VarChar,50),
                    new SqlParameter("@Context", SqlDbType.Text),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.LibraryID;

            if (model.ComPName != null)
                parameters[1].Value = model.ComPName;
            else
                parameters[1].Value = DBNull.Value;


            if (model.LogisticsNo != null)
                parameters[2].Value = model.LogisticsNo;
            else
                parameters[2].Value = DBNull.Value;


            if (model.CarUser != null)
                parameters[3].Value = model.CarUser;
            else
                parameters[3].Value = DBNull.Value;


            if (model.CarNo != null)
                parameters[4].Value = model.CarNo;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Car != null)
                parameters[5].Value = model.Car;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Context != null)
                parameters[6].Value = model.Context;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.Type;
            parameters[8].Value = model.CreateUserID;

            if (model.ts != DateTime.MinValue)
                parameters[9].Value = model.ts;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Logistics model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_Logistics] set ");
            strSql.Append("[LibraryID]=@LibraryID,");
            strSql.Append("[ComPName]=@ComPName,");
            strSql.Append("[LogisticsNo]=@LogisticsNo,");
            strSql.Append("[CarUser]=@CarUser,");
            strSql.Append("[CarNo]=@CarNo,");
            strSql.Append("[Car]=@Car,");
            strSql.Append("[Context]=@Context,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [Id]=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int),
                    new SqlParameter("@LibraryID", SqlDbType.Int),
                    new SqlParameter("@ComPName", SqlDbType.VarChar,50),
                    new SqlParameter("@LogisticsNo", SqlDbType.VarChar,50),
                    new SqlParameter("@CarUser", SqlDbType.VarChar,50),
                    new SqlParameter("@CarNo", SqlDbType.VarChar,50),
                    new SqlParameter("@Car", SqlDbType.VarChar,50),
                    new SqlParameter("@Context", SqlDbType.Text),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.Id;
            parameters[1].Value = model.LibraryID;

            if (model.ComPName != null)
                parameters[2].Value = model.ComPName;
            else
                parameters[2].Value = DBNull.Value;


            if (model.LogisticsNo != null)
                parameters[3].Value = model.LogisticsNo;
            else
                parameters[3].Value = DBNull.Value;


            if (model.CarUser != null)
                parameters[4].Value = model.CarUser;
            else
                parameters[4].Value = DBNull.Value;


            if (model.CarNo != null)
                parameters[5].Value = model.CarNo;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Car != null)
                parameters[6].Value = model.Car;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Context != null)
                parameters[7].Value = model.Context;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.Type;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;

            if (model.ts != DateTime.MinValue)
                parameters[11].Value = model.ts;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.dr;
            parameters[13].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_Logistics] ");
            strSql.Append(" where [Id]=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[Id]", "[YZT_Logistics]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [YZT_Logistics]");
            strSql.Append(" where [Id]= @Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.YZT_Logistics GetModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [YZT_Logistics] ");
            strSql.Append(" where [Id]=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
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
        public IList<Hi.Model.YZT_Logistics> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [YZT_Logistics]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.YZT_Logistics> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.YZT_Logistics> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[YZT_Logistics]", null, pageSize, pageIndex, fldSort, sort, strCondition, "Id", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.YZT_Logistics GetModel(DataRow r)
        {
            Hi.Model.YZT_Logistics model = new Hi.Model.YZT_Logistics();
            model.Id = SqlHelper.GetInt(r["Id"]);
            model.LibraryID = SqlHelper.GetInt(r["LibraryID"]);
            model.ComPName = SqlHelper.GetString(r["ComPName"]);
            model.LogisticsNo = SqlHelper.GetString(r["LogisticsNo"]);
            model.CarUser = SqlHelper.GetString(r["CarUser"]);
            model.CarNo = SqlHelper.GetString(r["CarNo"]);
            model.Car = SqlHelper.GetString(r["Car"]);
            model.Context = SqlHelper.GetString(r["Context"]);
            model.Type = SqlHelper.GetInt(r["Type"]);
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
        private IList<Hi.Model.YZT_Logistics> GetList(DataSet ds)
        {
            List<Hi.Model.YZT_Logistics> l = new List<Hi.Model.YZT_Logistics>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                l.Add(GetModel(r));
            }
            return l;
        }
    }
}
