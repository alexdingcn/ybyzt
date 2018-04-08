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
    /// 数据访问类 YZT_PaymentDetail
    /// </summary>
    public partial class YZT_PaymentDetail
    {

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_PaymentDetail model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_PaymentDetail](");
            strSql.Append("[CompID],[DisID],[PaymentID],[LibraryID],[LibraryDetailID],[GoodsCode],[GoodsName],[ValueInfo],[Unit],[BatchNO],[validDate],[Num],[AuditAmount],[sumAmount],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@PaymentID,@LibraryID,@LibraryDetailID,@GoodsCode,@GoodsName,@ValueInfo,@Unit,@BatchNO,@validDate,@Num,@AuditAmount,@sumAmount,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@PaymentID", SqlDbType.NChar,10),
                    new SqlParameter("@LibraryID", SqlDbType.NChar,10),
                    new SqlParameter("@LibraryDetailID", SqlDbType.NChar,10),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@Unit", SqlDbType.VarChar,30),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@Num", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@sumAmount", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;

            if (model.PaymentID != null)
                parameters[2].Value = model.PaymentID;
            else
                parameters[2].Value = DBNull.Value;


            if (model.LibraryID != null)
                parameters[3].Value = model.LibraryID;
            else
                parameters[3].Value = DBNull.Value;


            if (model.LibraryDetailID != null)
                parameters[4].Value = model.LibraryDetailID;
            else
                parameters[4].Value = DBNull.Value;


            if (model.GoodsCode != null)
                parameters[5].Value = model.GoodsCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[6].Value = model.GoodsName;
            else
                parameters[6].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[7].Value = model.ValueInfo;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[8].Value = model.Unit;
            else
                parameters[8].Value = DBNull.Value;


            if (model.BatchNO != null)
                parameters[9].Value = model.BatchNO;
            else
                parameters[9].Value = DBNull.Value;


            if (model.validDate != DateTime.MinValue)
                parameters[10].Value = model.validDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.Num;
            parameters[12].Value = model.AuditAmount;
            parameters[13].Value = model.sumAmount;

            if (model.Remark != null)
                parameters[14].Value = model.Remark;
            else
                parameters[14].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[15].Value = model.vdef1;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[16].Value = model.vdef2;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[17].Value = model.vdef3;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[19].Value = model.CreateDate;
            else
                parameters[19].Value = DBNull.Value;

            parameters[20].Value = model.ts;
            parameters[21].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_PaymentDetail model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_PaymentDetail] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[PaymentID]=@PaymentID,");
            strSql.Append("[LibraryID]=@LibraryID,");
            strSql.Append("[LibraryDetailID]=@LibraryDetailID,");
            strSql.Append("[GoodsCode]=@GoodsCode,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[ValueInfo]=@ValueInfo,");
            strSql.Append("[Unit]=@Unit,");
            strSql.Append("[BatchNO]=@BatchNO,");
            strSql.Append("[validDate]=@validDate,");
            strSql.Append("[Num]=@Num,");
            strSql.Append("[AuditAmount]=@AuditAmount,");
            strSql.Append("[sumAmount]=@sumAmount,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@PaymentID", SqlDbType.NChar,10),
                    new SqlParameter("@LibraryID", SqlDbType.NChar,10),
                    new SqlParameter("@LibraryDetailID", SqlDbType.NChar,10),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@Unit", SqlDbType.VarChar,30),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@Num", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@sumAmount", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;

            if (model.PaymentID != null)
                parameters[3].Value = model.PaymentID;
            else
                parameters[3].Value = DBNull.Value;


            if (model.LibraryID != null)
                parameters[4].Value = model.LibraryID;
            else
                parameters[4].Value = DBNull.Value;


            if (model.LibraryDetailID != null)
                parameters[5].Value = model.LibraryDetailID;
            else
                parameters[5].Value = DBNull.Value;


            if (model.GoodsCode != null)
                parameters[6].Value = model.GoodsCode;
            else
                parameters[6].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[7].Value = model.GoodsName;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[8].Value = model.ValueInfo;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[9].Value = model.Unit;
            else
                parameters[9].Value = DBNull.Value;


            if (model.BatchNO != null)
                parameters[10].Value = model.BatchNO;
            else
                parameters[10].Value = DBNull.Value;


            if (model.validDate != DateTime.MinValue)
                parameters[11].Value = model.validDate;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.Num;
            parameters[13].Value = model.AuditAmount;
            parameters[14].Value = model.sumAmount;

            if (model.Remark != null)
                parameters[15].Value = model.Remark;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[16].Value = model.vdef1;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[17].Value = model.vdef2;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[18].Value = model.vdef3;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[20].Value = model.CreateDate;
            else
                parameters[20].Value = DBNull.Value;

            parameters[21].Value = model.ts;
            parameters[22].Value = model.dr;
            parameters[23].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_PaymentDetail] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

       
        #endregion

        
    }
}
