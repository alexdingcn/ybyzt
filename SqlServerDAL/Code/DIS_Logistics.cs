//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/8/31 9:27:51
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
    /// 数据访问类 DIS_Logistics
    /// </summary>
    public partial class DIS_Logistics
    {
        public DIS_Logistics()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_Logistics model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_Logistics](");
            strSql.Append("[OrderID],[OrderOutID],[ComPName],[LogisticsNo],[CarUser],[CarNo],[Car],[Context],[Type],[CreateUserID],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@OrderOutID,@ComPName,@LogisticsNo,@CarUser,@CarNo,@Car,@Context,@Type,@CreateUserID,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@OrderOutID", SqlDbType.BigInt),
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
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.OrderOutID;

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

            if (model.ts != DateTime.MinValue)
                parameters[10].Value = model.ts;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_Logistics model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Logistics] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[OrderOutID]=@OrderOutID,");
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
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@OrderOutID", SqlDbType.BigInt),
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
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderOutID;

            if (model.ComPName != null)
                parameters[3].Value = model.ComPName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.LogisticsNo != null)
                parameters[4].Value = model.LogisticsNo;
            else
                parameters[4].Value = DBNull.Value;


            if (model.CarUser != null)
                parameters[5].Value = model.CarUser;
            else
                parameters[5].Value = DBNull.Value;


            if (model.CarNo != null)
                parameters[6].Value = model.CarNo;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Car != null)
                parameters[7].Value = model.Car;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Context != null)
                parameters[8].Value = model.Context;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.Type;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;

            if (model.ts != DateTime.MinValue)
                parameters[12].Value = model.ts;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_Logistics] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[Id]", "[DIS_Logistics]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_Logistics]");
            strSql.Append(" where [Id]= @Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_Logistics GetModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_Logistics] ");
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
        public IList<Hi.Model.DIS_Logistics> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_Logistics]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_Logistics> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_Logistics> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_Logistics]", null, pageSize, pageIndex, fldSort, sort, strCondition, "Id", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_Logistics GetModel(DataRow r)
        {
            Hi.Model.DIS_Logistics model = new Hi.Model.DIS_Logistics();
            model.Id = SqlHelper.GetInt(r["Id"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.OrderOutID = SqlHelper.GetInt(r["OrderOutID"]);
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
        private IList<Hi.Model.DIS_Logistics> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_Logistics>(ds.Tables[0]);
        }
    }
}
