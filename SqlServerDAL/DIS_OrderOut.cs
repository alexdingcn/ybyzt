using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class DIS_OrderOut
    {
        /// <summary>
        /// 增加一条数据,带事务
        /// </summary>
        public int Add(Hi.Model.DIS_OrderOut model, SqlTransaction TranSaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderOut](");
            strSql.Append("[OrderID],[DisID],[CompID],[ReceiptNo],[SendDate],[ActionUser],[Remark],[IsAudit],[AuditUserID],[AuditDate],[AuditRemark],[SignDate],[IsSign],[SignUserId],[SignUser],[SignRemark],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@DisID,@CompID,@ReceiptNo,@SendDate,@ActionUser,@Remark,@IsAudit,@AuditUserID,@AuditDate,@AuditRemark,@SignDate,@IsSign,@SignUserId,@SignUser,@SignRemark,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@SendDate", SqlDbType.DateTime),
                    new SqlParameter("@ActionUser", SqlDbType.VarChar,50),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@SignDate", SqlDbType.DateTime),
                    new SqlParameter("@IsSign", SqlDbType.Int),
                    new SqlParameter("@SignUserId", SqlDbType.Int),
                    new SqlParameter("@SignUser", SqlDbType.VarChar,50),
                    new SqlParameter("@SignRemark", SqlDbType.VarChar,400),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.CompID;

            if (model.ReceiptNo != null)
                parameters[3].Value = model.ReceiptNo;
            else
                parameters[3].Value = DBNull.Value;


            if (model.SendDate != DateTime.MinValue)
                parameters[4].Value = model.SendDate;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ActionUser != null)
                parameters[5].Value = model.ActionUser;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[6].Value = model.Remark;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.IsAudit;
            parameters[8].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[9].Value = model.AuditDate;
            else
                parameters[9].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[10].Value = model.AuditRemark;
            else
                parameters[10].Value = DBNull.Value;


            if (model.SignDate != DateTime.MinValue)
                parameters[11].Value = model.SignDate;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.IsSign;
            parameters[13].Value = model.SignUserId;

            if (model.SignUser != null)
                parameters[14].Value = model.SignUser;
            else
                parameters[14].Value = DBNull.Value;


            if (model.SignRemark != null)
                parameters[15].Value = model.SignRemark;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.CreateUserID;
            parameters[17].Value = model.CreateDate;
            parameters[18].Value = model.ts;
            parameters[19].Value = model.modifyuser;

            if (TranSaction != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), TranSaction, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="ID">订单ID</param>
        /// <returns></returns>
        public Hi.Model.DIS_OrderOut GetOutModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_OrderOut] ");
            strSql.Append(" where [OrderID]=@ID");
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
        /// 更新一条数据,带事务
        /// </summary>
        public bool Update(Hi.Model.DIS_OrderOut model, SqlTransaction TranSaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderOut] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[SendDate]=@SendDate,");
            strSql.Append("[ActionUser]=@ActionUser,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsAudit]=@IsAudit,");
            strSql.Append("[AuditUserID]=@AuditUserID,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[AuditRemark]=@AuditRemark,");
            strSql.Append("[SignDate]=@SignDate,");
            strSql.Append("[IsSign]=@IsSign,");
            strSql.Append("[SignUserId]=@SignUserId,");
            strSql.Append("[SignUser]=@SignUser,");
            strSql.Append("[SignRemark]=@SignRemark,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@SendDate", SqlDbType.DateTime),
                    new SqlParameter("@ActionUser", SqlDbType.VarChar,50),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@SignDate", SqlDbType.DateTime),
                    new SqlParameter("@IsSign", SqlDbType.Int),
                    new SqlParameter("@SignUserId", SqlDbType.Int),
                    new SqlParameter("@SignUser", SqlDbType.VarChar,50),
                    new SqlParameter("@SignRemark", SqlDbType.VarChar,400),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.CompID;

            if (model.ReceiptNo != null)
                parameters[4].Value = model.ReceiptNo;
            else
                parameters[4].Value = DBNull.Value;


            if (model.SendDate != DateTime.MinValue)
                parameters[5].Value = model.SendDate;
            else
                parameters[5].Value = DBNull.Value;


            if (model.ActionUser != null)
                parameters[6].Value = model.ActionUser;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.IsAudit;
            parameters[9].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[10].Value = model.AuditDate;
            else
                parameters[10].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[11].Value = model.AuditRemark;
            else
                parameters[11].Value = DBNull.Value;


            if (model.SignDate != DateTime.MinValue)
                parameters[12].Value = model.SignDate;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.IsSign;
            parameters[14].Value = model.SignUserId;

            if (model.SignUser != null)
                parameters[15].Value = model.SignUser;
            else
                parameters[15].Value = DBNull.Value;


            if (model.SignRemark != null)
                parameters[16].Value = model.SignRemark;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.CreateUserID;
            parameters[18].Value = model.CreateDate;
            parameters[19].Value = model.ts;
            parameters[20].Value = model.dr;
            parameters[21].Value = model.modifyuser;

            if (TranSaction != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), TranSaction, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 查询订单信息 code 2.5  by 2016-08-01 szj
        /// </summary>
        /// <param name="strWhat"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetList(string strWhat, string where)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select ");

            if (strWhat != "")
                sql.AppendFormat("{0}", strWhat);
            else
                sql.Append("o.*,lo.ID loID,lo.ComPName,lo.LogisticsNo,lo.CarUser,lo.CarNo,lo.Car");

            sql.Append(" from DIS_OrderOut o left join DIS_Logistics lo on o.ID=lo.OrderOutID ");

            if (where != "")
            {
                sql.AppendFormat(" where {0}", where);
            }
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];
        }



        /// <summary>
        /// 代理商入库 生单获取所有 商品
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strwhere"></param>
        /// <param name="pageCount"></param>
        /// <param name="Counts"></param>
        /// <returns></returns>
        public DataTable getDataTable(int pageSize, int pageIndex, string strwhere, out int pageCount, out int Counts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select i.id,d.id as Detailid,o.ReceiptNo,o.SendDate,g.GoodsName,i.ValueInfo,d.BatchNO,d.validDate,d.SignNum    ");
            strSql.AppendFormat("from DIS_OrderOut o ,DIS_OrderOutDetail d,BD_Goods g,BD_GoodsInfo i ");
            strSql.AppendFormat("  where   o.IsSign=1 and d.GoodsinfoID =i.ID ");
            strSql.AppendFormat("and  i.goodsid=g.id and d.OrderOutID=o.ID  {0}   ", strwhere);

            DataTable pageCountTable = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
            Counts = pageCountTable.Rows.Count;
            if (Counts % pageSize == 0)
                pageCount = Counts / pageSize;
            else
                pageCount = (Counts / pageSize) + 1;

            StringBuilder sb = new StringBuilder();
            DataTable Table = new DataTable();
            if (Counts > 0)
            {
                sb.AppendFormat("select * from (select ROW_NUMBER() over( order by  SendDate desc) rwo,* from ");
                sb.AppendFormat(" ({0}) tab )b", strSql.ToString());
                sb.AppendFormat(" where b.rwo between ({0}-1)*{1}+1 and {2}*{3}", pageIndex, pageSize, pageIndex, pageSize);
                Table = SqlHelper.Query(SqlHelper.LocalSqlServer, sb.ToString()).Tables[0];
                return Table;
            }
            return pageCountTable;
        }



    }
}
