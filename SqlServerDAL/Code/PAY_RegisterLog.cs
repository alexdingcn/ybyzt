//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/7/14 16:39:46
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
    /// 数据访问类 PAY_RegisterLog
    /// </summary>
    public class PAY_RegisterLog
    {
        public PAY_RegisterLog()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.PAY_RegisterLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_RegisterLog](");
            strSql.Append("[OrderId],[Ordercode],[number],[Price],[fees],[Payuse],[PayName],[DisID],[PayTime],[Remark],[DisName],[BankID],[Typea],[Start],[ResultMessage],[CreateUser],[CreateDate],[LogType],[PlanMessage])");
            strSql.Append(" values (");
            strSql.Append("@OrderId,@Ordercode,@number,@Price,@fees,@Payuse,@PayName,@DisID,@PayTime,@Remark,@DisName,@BankID,@Typea,@Start,@ResultMessage,@CreateUser,@CreateDate,@LogType,@PlanMessage)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", SqlDbType.BigInt),
                    new SqlParameter("@Ordercode", SqlDbType.NVarChar,20),
                    new SqlParameter("@number", SqlDbType.NVarChar,50),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@fees", SqlDbType.Decimal),
                    new SqlParameter("@Payuse", SqlDbType.NVarChar,50),
                    new SqlParameter("@PayName", SqlDbType.NVarChar,50),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@PayTime", SqlDbType.NVarChar,50),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@DisName", SqlDbType.NVarChar,100),
                    new SqlParameter("@BankID", SqlDbType.NVarChar,20),
                    new SqlParameter("@Typea", SqlDbType.NVarChar,10),
                    new SqlParameter("@Start", SqlDbType.NVarChar,50),
                    new SqlParameter("@ResultMessage", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateUser", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@LogType", SqlDbType.Int),
                    new SqlParameter("@PlanMessage", SqlDbType.Text)
            };
            parameters[0].Value = model.OrderId;

            if (model.Ordercode != null)
                parameters[1].Value = model.Ordercode;
            else
                parameters[1].Value = DBNull.Value;


            if (model.number != null)
                parameters[2].Value = model.number;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.Price;
            parameters[4].Value = model.fees;

            if (model.Payuse != null)
                parameters[5].Value = model.Payuse;
            else
                parameters[5].Value = DBNull.Value;


            if (model.PayName != null)
                parameters[6].Value = model.PayName;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.DisID;

            if (model.PayTime != null)
                parameters[8].Value = model.PayTime;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[9].Value = model.Remark;
            else
                parameters[9].Value = DBNull.Value;


            if (model.DisName != null)
                parameters[10].Value = model.DisName;
            else
                parameters[10].Value = DBNull.Value;


            if (model.BankID != null)
                parameters[11].Value = model.BankID;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Typea != null)
                parameters[12].Value = model.Typea;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Start != null)
                parameters[13].Value = model.Start;
            else
                parameters[13].Value = DBNull.Value;


            if (model.ResultMessage != null)
                parameters[14].Value = model.ResultMessage;
            else
                parameters[14].Value = DBNull.Value;

            parameters[15].Value = model.CreateUser;

            if (model.CreateDate != DateTime.MinValue)
                parameters[16].Value = model.CreateDate;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.LogType;

            if (model.PlanMessage != null)
                parameters[18].Value = model.PlanMessage;
            else
                parameters[18].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.PAY_RegisterLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_RegisterLog] set ");
            strSql.Append("[OrderId]=@OrderId,");
            strSql.Append("[Ordercode]=@Ordercode,");
            strSql.Append("[number]=@number,");
            strSql.Append("[Price]=@Price,");
            strSql.Append("[fees]=@fees,");
            strSql.Append("[Payuse]=@Payuse,");
            strSql.Append("[PayName]=@PayName,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[PayTime]=@PayTime,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[DisName]=@DisName,");
            strSql.Append("[BankID]=@BankID,");
            strSql.Append("[Typea]=@Typea,");
            strSql.Append("[Start]=@Start,");
            strSql.Append("[ResultMessage]=@ResultMessage,");
            strSql.Append("[CreateUser]=@CreateUser,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[LogType]=@LogType,");
            strSql.Append("[PlanMessage]=@PlanMessage");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderId", SqlDbType.BigInt),
                    new SqlParameter("@Ordercode", SqlDbType.NVarChar,20),
                    new SqlParameter("@number", SqlDbType.NVarChar,50),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@fees", SqlDbType.Decimal),
                    new SqlParameter("@Payuse", SqlDbType.NVarChar,50),
                    new SqlParameter("@PayName", SqlDbType.NVarChar,50),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@PayTime", SqlDbType.NVarChar,50),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@DisName", SqlDbType.NVarChar,100),
                    new SqlParameter("@BankID", SqlDbType.NVarChar,20),
                    new SqlParameter("@Typea", SqlDbType.NVarChar,10),
                    new SqlParameter("@Start", SqlDbType.NVarChar,50),
                    new SqlParameter("@ResultMessage", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateUser", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@LogType", SqlDbType.Int),
                    new SqlParameter("@PlanMessage", SqlDbType.Text)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderId;

            if (model.Ordercode != null)
                parameters[2].Value = model.Ordercode;
            else
                parameters[2].Value = DBNull.Value;


            if (model.number != null)
                parameters[3].Value = model.number;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.Price;
            parameters[5].Value = model.fees;

            if (model.Payuse != null)
                parameters[6].Value = model.Payuse;
            else
                parameters[6].Value = DBNull.Value;


            if (model.PayName != null)
                parameters[7].Value = model.PayName;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.DisID;

            if (model.PayTime != null)
                parameters[9].Value = model.PayTime;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[10].Value = model.Remark;
            else
                parameters[10].Value = DBNull.Value;


            if (model.DisName != null)
                parameters[11].Value = model.DisName;
            else
                parameters[11].Value = DBNull.Value;


            if (model.BankID != null)
                parameters[12].Value = model.BankID;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Typea != null)
                parameters[13].Value = model.Typea;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Start != null)
                parameters[14].Value = model.Start;
            else
                parameters[14].Value = DBNull.Value;


            if (model.ResultMessage != null)
                parameters[15].Value = model.ResultMessage;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.CreateUser;

            if (model.CreateDate != DateTime.MinValue)
                parameters[17].Value = model.CreateDate;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.LogType;

            if (model.PlanMessage != null)
                parameters[19].Value = model.PlanMessage;
            else
                parameters[19].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [PAY_RegisterLog] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[PAY_RegisterLog]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PAY_RegisterLog]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.PAY_RegisterLog GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [PAY_RegisterLog] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
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
        public IList<Hi.Model.PAY_RegisterLog> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [PAY_RegisterLog]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.PAY_RegisterLog> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.PAY_RegisterLog> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[PAY_RegisterLog]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.PAY_RegisterLog GetModel(DataRow r)
        {
            Hi.Model.PAY_RegisterLog model = new Hi.Model.PAY_RegisterLog();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrderId = SqlHelper.GetInt(r["OrderId"]);
            model.Ordercode = SqlHelper.GetString(r["Ordercode"]);
            model.number = SqlHelper.GetString(r["number"]);
            model.Price = SqlHelper.GetDecimal(r["Price"]);
            model.fees = SqlHelper.GetDecimal(r["fees"]);
            model.Payuse = SqlHelper.GetString(r["Payuse"]);
            model.PayName = SqlHelper.GetString(r["PayName"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.PayTime = SqlHelper.GetString(r["PayTime"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.DisName = SqlHelper.GetString(r["DisName"]);
            model.BankID = SqlHelper.GetString(r["BankID"]);
            model.Typea = SqlHelper.GetString(r["Typea"]);
            model.Start = SqlHelper.GetString(r["Start"]);
            model.ResultMessage = SqlHelper.GetString(r["ResultMessage"]);
            model.CreateUser = SqlHelper.GetInt(r["CreateUser"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.LogType = SqlHelper.GetInt(r["LogType"]);
            model.PlanMessage = SqlHelper.GetString(r["PlanMessage"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.PAY_RegisterLog> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.PAY_RegisterLog>(ds.Tables[0]);
        }
    }
}
