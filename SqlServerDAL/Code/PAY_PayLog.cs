//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/3/31 10:13:25
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
    /// 数据访问类 PAY_PayLog
    /// </summary>
    public partial class PAY_PayLog
    {
        public PAY_PayLog()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.PAY_PayLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_PayLog](");
            strSql.Append("[OrderId],[Ordercode],[number],[CompID],[OrgCode],[MarkName],[MarkNumber],[AccountName],[bankcode],[bankAddress],[bankPrivate],[bankCity],[Price],[Remark],[Start],[ResultMessage],[CreateUser],[CreateDate])");
            strSql.Append(" values (");
            strSql.Append("@OrderId,@Ordercode,@number,@CompID,@OrgCode,@MarkName,@MarkNumber,@AccountName,@bankcode,@bankAddress,@bankPrivate,@bankCity,@Price,@Remark,@Start,@ResultMessage,@CreateUser,@CreateDate)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", SqlDbType.BigInt),
                    new SqlParameter("@Ordercode", SqlDbType.NVarChar,20),
                    new SqlParameter("@number", SqlDbType.VarChar,200),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@OrgCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@MarkName", SqlDbType.NVarChar,50),
                    new SqlParameter("@MarkNumber", SqlDbType.NVarChar,50),
                    new SqlParameter("@AccountName", SqlDbType.NVarChar,20),
                    new SqlParameter("@bankcode", SqlDbType.NVarChar,50),
                    new SqlParameter("@bankAddress", SqlDbType.NVarChar,50),
                    new SqlParameter("@bankPrivate", SqlDbType.NVarChar,20),
                    new SqlParameter("@bankCity", SqlDbType.NVarChar,20),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@Start", SqlDbType.NVarChar,50),
                    new SqlParameter("@ResultMessage", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateUser", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime)
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

            parameters[3].Value = model.CompID;

            if (model.OrgCode != null)
                parameters[4].Value = model.OrgCode;
            else
                parameters[4].Value = DBNull.Value;


            if (model.MarkName != null)
                parameters[5].Value = model.MarkName;
            else
                parameters[5].Value = DBNull.Value;


            if (model.MarkNumber != null)
                parameters[6].Value = model.MarkNumber;
            else
                parameters[6].Value = DBNull.Value;


            if (model.AccountName != null)
                parameters[7].Value = model.AccountName;
            else
                parameters[7].Value = DBNull.Value;


            if (model.bankcode != null)
                parameters[8].Value = model.bankcode;
            else
                parameters[8].Value = DBNull.Value;


            if (model.bankAddress != null)
                parameters[9].Value = model.bankAddress;
            else
                parameters[9].Value = DBNull.Value;


            if (model.bankPrivate != null)
                parameters[10].Value = model.bankPrivate;
            else
                parameters[10].Value = DBNull.Value;


            if (model.bankCity != null)
                parameters[11].Value = model.bankCity;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.Price;

            if (model.Remark != null)
                parameters[13].Value = model.Remark;
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

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.PAY_PayLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_PayLog] set ");
            strSql.Append("[OrderId]=@OrderId,");
            strSql.Append("[Ordercode]=@Ordercode,");
            strSql.Append("[number]=@number,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[OrgCode]=@OrgCode,");
            strSql.Append("[MarkName]=@MarkName,");
            strSql.Append("[MarkNumber]=@MarkNumber,");
            strSql.Append("[AccountName]=@AccountName,");
            strSql.Append("[bankcode]=@bankcode,");
            strSql.Append("[bankAddress]=@bankAddress,");
            strSql.Append("[bankPrivate]=@bankPrivate,");
            strSql.Append("[bankCity]=@bankCity,");
            strSql.Append("[Price]=@Price,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[Start]=@Start,");
            strSql.Append("[ResultMessage]=@ResultMessage,");
            strSql.Append("[CreateUser]=@CreateUser,");
            strSql.Append("[CreateDate]=@CreateDate");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderId", SqlDbType.BigInt),
                    new SqlParameter("@Ordercode", SqlDbType.NVarChar,20),
                    new SqlParameter("@number", SqlDbType.VarChar,200),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@OrgCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@MarkName", SqlDbType.NVarChar,50),
                    new SqlParameter("@MarkNumber", SqlDbType.NVarChar,50),
                    new SqlParameter("@AccountName", SqlDbType.NVarChar,20),
                    new SqlParameter("@bankcode", SqlDbType.NVarChar,50),
                    new SqlParameter("@bankAddress", SqlDbType.NVarChar,50),
                    new SqlParameter("@bankPrivate", SqlDbType.NVarChar,20),
                    new SqlParameter("@bankCity", SqlDbType.NVarChar,20),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@Start", SqlDbType.NVarChar,50),
                    new SqlParameter("@ResultMessage", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateUser", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime)
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

            parameters[4].Value = model.CompID;

            if (model.OrgCode != null)
                parameters[5].Value = model.OrgCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.MarkName != null)
                parameters[6].Value = model.MarkName;
            else
                parameters[6].Value = DBNull.Value;


            if (model.MarkNumber != null)
                parameters[7].Value = model.MarkNumber;
            else
                parameters[7].Value = DBNull.Value;


            if (model.AccountName != null)
                parameters[8].Value = model.AccountName;
            else
                parameters[8].Value = DBNull.Value;


            if (model.bankcode != null)
                parameters[9].Value = model.bankcode;
            else
                parameters[9].Value = DBNull.Value;


            if (model.bankAddress != null)
                parameters[10].Value = model.bankAddress;
            else
                parameters[10].Value = DBNull.Value;


            if (model.bankPrivate != null)
                parameters[11].Value = model.bankPrivate;
            else
                parameters[11].Value = DBNull.Value;


            if (model.bankCity != null)
                parameters[12].Value = model.bankCity;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.Price;

            if (model.Remark != null)
                parameters[14].Value = model.Remark;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Start != null)
                parameters[15].Value = model.Start;
            else
                parameters[15].Value = DBNull.Value;


            if (model.ResultMessage != null)
                parameters[16].Value = model.ResultMessage;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.CreateUser;

            if (model.CreateDate != DateTime.MinValue)
                parameters[18].Value = model.CreateDate;
            else
                parameters[18].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [PAY_PayLog] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[PAY_PayLog]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PAY_PayLog]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.PAY_PayLog GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [PAY_PayLog] ");
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
        public IList<Hi.Model.PAY_PayLog> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [PAY_PayLog]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.PAY_PayLog> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.PAY_PayLog> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[PAY_PayLog]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.PAY_PayLog GetModel(DataRow r)
        {
            Hi.Model.PAY_PayLog model = new Hi.Model.PAY_PayLog();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrderId = SqlHelper.GetInt(r["OrderId"]);
            model.Ordercode = SqlHelper.GetString(r["Ordercode"]);
            model.number = SqlHelper.GetString(r["number"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.OrgCode = SqlHelper.GetString(r["OrgCode"]);
            model.MarkName = SqlHelper.GetString(r["MarkName"]);
            model.MarkNumber = SqlHelper.GetString(r["MarkNumber"]);
            model.AccountName = SqlHelper.GetString(r["AccountName"]);
            model.bankcode = SqlHelper.GetString(r["bankcode"]);
            model.bankAddress = SqlHelper.GetString(r["bankAddress"]);
            model.bankPrivate = SqlHelper.GetString(r["bankPrivate"]);
            model.bankCity = SqlHelper.GetString(r["bankCity"]);
            model.Price = SqlHelper.GetDecimal(r["Price"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.Start = SqlHelper.GetString(r["Start"]);
            model.ResultMessage = SqlHelper.GetString(r["ResultMessage"]);
            model.CreateUser = SqlHelper.GetInt(r["CreateUser"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.PAY_PayLog> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.PAY_PayLog>(ds.Tables[0]);
        }
    }
}
