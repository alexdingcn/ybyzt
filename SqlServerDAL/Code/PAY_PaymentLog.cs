//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/19 13:10:06
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
    /// 数据访问类 PAY_PaymentLog
    /// </summary>
    public class PAY_PaymentLog
    {
        public PAY_PaymentLog()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.PAY_PaymentLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_PaymentLog](");
            strSql.Append("[OrderId],[CompID],[OrgCode],[MarkNumber],[Price],[PayorgCode],[PayCode],[DisID],[PayTime],[Remark],[Start],[Message],[CreateUser],[CreateDate])");
            strSql.Append(" values (");
            strSql.Append("@OrderId,@CompID,@OrgCode,@MarkNumber,@Price,@PayorgCode,@PayCode,@DisID,@PayTime,@Remark,@Start,@Message,@CreateUser,@CreateDate)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@OrgCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@MarkNumber", SqlDbType.NVarChar,50),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@PayorgCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@PayCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@PayTime", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@Start", SqlDbType.NVarChar,50),
                    new SqlParameter("@Message", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateUser", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.OrderId;
            parameters[1].Value = model.CompID;

            if (model.OrgCode != null)
                parameters[2].Value = model.OrgCode;
            else
                parameters[2].Value = DBNull.Value;


            if (model.MarkNumber != null)
                parameters[3].Value = model.MarkNumber;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.Price;

            if (model.PayorgCode != null)
                parameters[5].Value = model.PayorgCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.PayCode != null)
                parameters[6].Value = model.PayCode;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.DisID;

            if (model.PayTime != DateTime.MinValue)
                parameters[8].Value = model.PayTime;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[9].Value = model.Remark;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Start != null)
                parameters[10].Value = model.Start;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Message != null)
                parameters[11].Value = model.Message;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.CreateUser;

            if (model.CreateDate != DateTime.MinValue)
                parameters[13].Value = model.CreateDate;
            else
                parameters[13].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.PAY_PaymentLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_PaymentLog] set ");
            strSql.Append("[OrderId]=@OrderId,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[OrgCode]=@OrgCode,");
            strSql.Append("[MarkNumber]=@MarkNumber,");
            strSql.Append("[Price]=@Price,");
            strSql.Append("[PayorgCode]=@PayorgCode,");
            strSql.Append("[PayCode]=@PayCode,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[PayTime]=@PayTime,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[Start]=@Start,");
            strSql.Append("[Message]=@Message,");
            strSql.Append("[CreateUser]=@CreateUser,");
            strSql.Append("[CreateDate]=@CreateDate");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderId", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@OrgCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@MarkNumber", SqlDbType.NVarChar,50),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@PayorgCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@PayCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@PayTime", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@Start", SqlDbType.NVarChar,50),
                    new SqlParameter("@Message", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateUser", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderId;
            parameters[2].Value = model.CompID;

            if (model.OrgCode != null)
                parameters[3].Value = model.OrgCode;
            else
                parameters[3].Value = DBNull.Value;


            if (model.MarkNumber != null)
                parameters[4].Value = model.MarkNumber;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.Price;

            if (model.PayorgCode != null)
                parameters[6].Value = model.PayorgCode;
            else
                parameters[6].Value = DBNull.Value;


            if (model.PayCode != null)
                parameters[7].Value = model.PayCode;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.DisID;

            if (model.PayTime != DateTime.MinValue)
                parameters[9].Value = model.PayTime;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[10].Value = model.Remark;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Start != null)
                parameters[11].Value = model.Start;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Message != null)
                parameters[12].Value = model.Message;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.CreateUser;

            if (model.CreateDate != DateTime.MinValue)
                parameters[14].Value = model.CreateDate;
            else
                parameters[14].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [PAY_PaymentLog] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[PAY_PaymentLog]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PAY_PaymentLog]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.PAY_PaymentLog GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [PAY_PaymentLog] ");
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
        public IList<Hi.Model.PAY_PaymentLog> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [PAY_PaymentLog]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.PAY_PaymentLog> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.PAY_PaymentLog> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[PAY_PaymentLog]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.PAY_PaymentLog GetModel(DataRow r)
        {
            Hi.Model.PAY_PaymentLog model = new Hi.Model.PAY_PaymentLog();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrderId = SqlHelper.GetInt(r["OrderId"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.OrgCode = SqlHelper.GetString(r["OrgCode"]);
            model.MarkNumber = SqlHelper.GetString(r["MarkNumber"]);
            model.Price = SqlHelper.GetDecimal(r["Price"]);
            model.PayorgCode = SqlHelper.GetString(r["PayorgCode"]);
            model.PayCode = SqlHelper.GetString(r["PayCode"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.PayTime = SqlHelper.GetDateTime(r["PayTime"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.Start = SqlHelper.GetString(r["Start"]);
            model.Message = SqlHelper.GetString(r["Message"]);
            model.CreateUser = SqlHelper.GetInt(r["CreateUser"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.PAY_PaymentLog> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.PAY_PaymentLog>(ds.Tables[0]);
        }
    }
}
