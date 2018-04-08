using DBUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Hi.SQLServerDAL
{
    public partial class DIS_Order
    {
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="OrderInfo"></param>
        /// <returns></returns>
        public bool OrderDel(Hi.Model.DIS_Order OrderInfo)
        {
            SqlConnection con = new SqlConnection(SqlHelper.LocalSqlServer);
            con.Open();
            SqlTransaction sqlTrans = con.BeginTransaction();
            //可以做循环   
            try
            {


                int O = DelOrder(con, OrderInfo, sqlTrans);
                int Od = DelOrderDetail(con, OrderInfo, sqlTrans);

                if (O > 0 && Od > 0)
                {
                    sqlTrans.Commit();
                    return true;
                }
                else
                {
                    sqlTrans.Rollback();
                }
            }
            catch (Exception)
            {
                sqlTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
            return false;
        }

        /// <summary>
        /// 删除订单表
        /// </summary>
        /// <returns></returns>
        public int DelOrder(SqlConnection sqlconn, Hi.Model.DIS_Order OrderInfo, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Order] set ");
            strSql.Append("[dr]=@dr");
            strSql.Append(" where [ID]=@ID;");

            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@dr", SqlDbType.Int),
                };
            parameters[0].Value = OrderInfo.ID;
            parameters[1].Value = OrderInfo.dr;

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value == DBNull.Value)
                        {
                            parameter.Value = DBNull.Value;
                        }

                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="OrderInfo"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int DelOrderDetail(SqlConnection sqlconn, Hi.Model.DIS_Order OrderInfo, SqlTransaction sqltans)
        {
            string sql = "select * from DIS_OrderDetail where ISNULL(dr,0)=0 and OrderId=@OrderId";
            SqlParameter[] cmdParms = {
                    new SqlParameter("@OrderID", SqlDbType.Int),
            };
            cmdParms[0].Value = OrderInfo.ID;

            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql, cmdParms).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count <= 0)
                {
                    //不存在订单明细
                    return 1;
                }
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderDetail] set ");
            strSql.Append("[dr]=@Ddr");
            strSql.Append(" where [OrderID]=@OrderID;");

            SqlParameter[] parameters = {
                    new SqlParameter("@Ddr", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.Int),
                };
            parameters[0].Value = OrderInfo.dr;
            parameters[1].Value = OrderInfo.ID;

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value == DBNull.Value)
                        {
                            parameter.Value = DBNull.Value;
                        }

                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public bool UpdateOrderState(string strSql)
        {
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql) > 0;
        }

        /// <summary>
        /// 更新一条数据，带事务
        /// </summary>
        public bool Update(Hi.Model.DIS_Order model, SqlTransaction TranSaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Order] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[DisUserID]=@DisUserID,");
            strSql.Append("[AddType]=@AddType,");
            strSql.Append("[Otype]=@Otype,");
            strSql.Append("[IsAudit]=@IsAudit,");
            strSql.Append("[AddrID]=@AddrID,");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[GUID]=@GUID,");
            strSql.Append("[ArriveDate]=@ArriveDate,");
            strSql.Append("[TotalAmount]=@TotalAmount,");
            strSql.Append("[AuditAmount]=@AuditAmount,");
            strSql.Append("[OtherAmount]=@OtherAmount,");
            strSql.Append("[PayedAmount]=@PayedAmount,");
            strSql.Append("[Principal]=@Principal,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[OState]=@OState,");
            strSql.Append("[PayState]=@PayState,");
            strSql.Append("[ReturnState]=@ReturnState,");
            strSql.Append("[AuditUserID]=@AuditUserID,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[AuditRemark]=@AuditRemark,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ReturnMoneyDate]=@ReturnMoneyDate,");
            strSql.Append("[ReturnMoneyUserId]=@ReturnMoneyUserId,");
            strSql.Append("[ReturnMoneyUser]=@ReturnMoneyUser,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[Atta]=@Atta,");
            strSql.Append("[IsOutState]=@IsOutState,");
            strSql.Append("[IsPayColl]=@IsPayColl,");
            strSql.Append("[CostSub]=@CostSub,");
            strSql.Append("[bateAmount]=@bateAmount,");
            strSql.Append("[IsSettl]=@IsSettl,");
            strSql.Append("[GiveMode]=@GiveMode,");
            strSql.Append("[PostFee]=@PostFee,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@DisUserID", SqlDbType.Int),
                    new SqlParameter("@AddType", SqlDbType.Int),
                    new SqlParameter("@Otype", SqlDbType.Int),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@AddrID", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@GUID", SqlDbType.VarChar,50),
                    new SqlParameter("@ArriveDate", SqlDbType.DateTime),
                    new SqlParameter("@TotalAmount", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@OtherAmount", SqlDbType.Decimal),
                    new SqlParameter("@PayedAmount", SqlDbType.Decimal),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,300),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@OState", SqlDbType.Int),
                    new SqlParameter("@PayState", SqlDbType.Int),
                    new SqlParameter("@ReturnState", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnMoneyDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnMoneyUserId", SqlDbType.Int),
                    new SqlParameter("@ReturnMoneyUser", SqlDbType.VarChar,50),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Atta", SqlDbType.VarChar,1024),
                    new SqlParameter("@IsOutState", SqlDbType.Int),
                    new SqlParameter("@IsPayColl", SqlDbType.VarChar,128),
                    new SqlParameter("@CostSub", SqlDbType.VarChar,128),
                    new SqlParameter("@bateAmount", SqlDbType.Decimal),
                    new SqlParameter("@IsSettl", SqlDbType.VarChar,134),
                    new SqlParameter("@GiveMode", SqlDbType.VarChar,50),
                    new SqlParameter("@PostFee", SqlDbType.Decimal),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.DisUserID;
            parameters[4].Value = model.AddType;
            parameters[5].Value = model.Otype;
            parameters[6].Value = model.IsAudit;
            parameters[7].Value = model.AddrID;
            parameters[8].Value = model.ReceiptNo;

            if (model.GUID != null)
                parameters[9].Value = model.GUID;
            else
                parameters[9].Value = DBNull.Value;


            if (model.ArriveDate != DateTime.MinValue)
                parameters[10].Value = model.ArriveDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.TotalAmount;
            parameters[12].Value = model.AuditAmount;
            parameters[13].Value = model.OtherAmount;
            parameters[14].Value = model.PayedAmount;

            if (model.Principal != null)
                parameters[15].Value = model.Principal;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[16].Value = model.Phone;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Address != null)
                parameters[17].Value = model.Address;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[18].Value = model.Remark;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.OState;
            parameters[20].Value = model.PayState;
            parameters[21].Value = model.ReturnState;
            parameters[22].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[23].Value = model.AuditDate;
            else
                parameters[23].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[24].Value = model.AuditRemark;
            else
                parameters[24].Value = DBNull.Value;

            parameters[25].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[26].Value = model.CreateDate;
            else
                parameters[26].Value = DBNull.Value;


            if (model.ReturnMoneyDate != DateTime.MinValue)
                parameters[27].Value = model.ReturnMoneyDate;
            else
                parameters[27].Value = DBNull.Value;

            parameters[28].Value = model.ReturnMoneyUserId;

            if (model.ReturnMoneyUser != null)
                parameters[29].Value = model.ReturnMoneyUser;
            else
                parameters[29].Value = DBNull.Value;

            parameters[30].Value = model.ts;
            parameters[31].Value = model.dr;
            parameters[32].Value = model.modifyuser;

            if (model.Atta != null)
                parameters[33].Value = model.Atta;
            else
                parameters[33].Value = DBNull.Value;

            parameters[34].Value = model.IsOutState;

            if (model.IsPayColl != null)
                parameters[35].Value = model.IsPayColl;
            else
                parameters[35].Value = DBNull.Value;


            if (model.CostSub != null)
                parameters[36].Value = model.CostSub;
            else
                parameters[36].Value = DBNull.Value;

            parameters[37].Value = model.bateAmount;

            if (model.IsSettl != null)
                parameters[38].Value = model.IsSettl;
            else
                parameters[38].Value = DBNull.Value;


            if (model.GiveMode != null)
                parameters[39].Value = model.GiveMode;
            else
                parameters[39].Value = DBNull.Value;

            parameters[40].Value = model.PostFee;

            if (model.vdef1 != null)
                parameters[41].Value = model.vdef1;
            else
                parameters[41].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[42].Value = model.vdef2;
            else
                parameters[42].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[43].Value = model.vdef3;
            else
                parameters[43].Value = DBNull.Value;

            if (TranSaction != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), TranSaction, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        public bool DelOrder(int OrderId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE [DIS_OrderDetail] ");
            strSql.Append(" where [OrderID]=" + OrderId);

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString()) > 0;

        }

        public Hi.Model.DIS_Order GetModel(string ReceiptNo)
        {
            string sqlstr = string.Format("select * from dis_order where receiptno='{0}'", ReceiptNo);
            if (SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr).Tables[0].Rows.Count > 0)
            {
                return GetModel(SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr).Tables[0].Rows[0]);
            }
            return null;
        }

        public DataSet GetData(string ReceiptNo)
        {
            string sqlstr = string.Format("select * from dis_order where receiptno like ('%{0}%') and dr=0", ReceiptNo);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);
        }

        /// <summary>
        /// 修改订单退款状态，modify by ggh
        /// 创建时间：2015-06-09
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="model"></param>
        /// <param name="sqltans"></param>
        /// <param name="returnID">退货单ID，根据退货单Id判断方法的重载</param>
        /// <param name="returnstart">退货单状态</param>
        /// <returns></returns>
        public int UpdateOrderByggh(SqlConnection sqlconn, Hi.Model.DIS_Order model, SqlTransaction sqltans, int returnID,int returnstart)
        {
            int rowsAffected = 0;
            if (returnID == 0)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update [DIS_Order] set [PayState]=@PayState ,ReturnState=@ReturnState,ts=@ts,modifyuser=@modifyuser where [ID]=@ID");
                SqlParameter[] parameters = { new SqlParameter("@PayState", SqlDbType.Int),
                                        new SqlParameter("@ReturnState", SqlDbType.Int),
                                        new SqlParameter("@ts", SqlDbType.DateTime),
                                        new SqlParameter("@modifyuser", SqlDbType.Int) ,
                                        new SqlParameter("@ID", SqlDbType.Int) 
                                        };
                parameters[0].Value = model.PayState;
                parameters[1].Value = model.ReturnState;
                parameters[2].Value = model.ts;
                parameters[3].Value = model.modifyuser;
                parameters[4].Value = model.ID;
                SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        if (parameter.SqlDbType == SqlDbType.DateTime)
                        {
                            if (parameter.Value == DBNull.Value)
                            {
                                parameter.Value = DBNull.Value;
                            }

                        }
                        cmd.Parameters.Add(parameter);
                    }
                }

                rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());


            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update [DIS_OrderReturn] set [ReturnState]=@ReturnState  where [ID]=@ID");
                SqlParameter[] parameters = { new SqlParameter("@ReturnState", SqlDbType.Int),                                       
                                        new SqlParameter("@ID", SqlDbType.Int) 
                                        };
                parameters[0].Value =returnstart;
                parameters[1].Value =returnID;
               
                SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        if (parameter.SqlDbType == SqlDbType.DateTime)
                        {
                            if (parameter.Value == DBNull.Value)
                            {
                                parameter.Value = DBNull.Value;
                            }

                        }
                        cmd.Parameters.Add(parameter);
                    }
                }

                rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            }
            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;

        }

        public IList<Hi.Model.DIS_Order> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction sqltans)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby, sqltans));
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction sqltans)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_Order]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            if (sqltans!=null)
                return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(),sqltans);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 修改单个商品信息
        /// </summary>
        /// <param name="strWhat">修改的信息</param>
        /// <param name="parameters">修改的参数</param>
        /// <param name="strWhere">修改条件</param>
        /// <returns></returns>
        public bool UpdateOrder(string strWhat, SqlParameter[] parameters, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Order] set ");
            strSql.Append(strWhat);
            strSql.Append(" where " + strWhere);

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        /// <param name="strWhat">字段</param>
        /// <param name="TbName">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderby">排序字段</param>
        /// <returns></returns>
        public DataTable GetList(string strWhat, string TbName, string strWhere, string strOrderby)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from ");

            if (!string.IsNullOrEmpty(TbName))
                strSql.Append(TbName);
            else
                strSql.Append("[DIS_Order]");

            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
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

            if (strWhat != "")
                sql.AppendFormat("select {0} from  dis_order o left join DIS_OrderExt oe on o.ID=oe.OrderID where 1=1", strWhat);
            else
                sql.AppendFormat("select * from  dis_order o left join DIS_OrderExt oe on o.ID=oe.OrderID where 1=1", strWhat);

            if (where != "")
            {
                sql.AppendFormat("{0}", where);
            }
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetLists(string sql)
        {
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];

        }
        }
}
