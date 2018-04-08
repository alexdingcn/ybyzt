using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Configuration;

namespace Hi.BLL
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
            return dal.OrderDel(OrderInfo);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="OrderInfo"></param>
        /// <returns></returns>
        public bool OrderDel(string strwhere,out string msg)
        {
            bool falg = false;
            msg = "";
            string [] Id=strwhere.Split(new char[]{','});

            for (int i = 0; i < Id.Length; i++)
            {
                Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(Id[i]));

                if (OrderModel.OState == -1 || OrderModel.OState == 0)
                {
                    OrderModel.dr = 1;
                    if (OrderDel(OrderModel))
                    {
                        falg = true;
                    }
                }
                else
                {
                    msg += OrderModel.ReceiptNo + ",";
                }
            }

            return falg;
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public bool UpdateOrderState(string strSql)
        {
            return dal.UpdateOrderState(strSql);
        }

        /// <summary>
        /// 订单添加 （事务）
        /// </summary>
        /// <returns></returns>
        public int AddOrder(SqlConnection sqlconn, Hi.Model.DIS_Order model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_Order](");
            strSql.Append("[CompID],[DisID],[DisUserID],[AddType],[Otype],[IsAudit],[AddrID],[ReceiptNo],[GUID],[ArriveDate],[TotalAmount],[AuditAmount],[OtherAmount],[PayedAmount],[Principal],[Phone],[Address],[Remark],[OState],[PayState],[ReturnState],[AuditUserID],[AuditDate],[AuditRemark],[CreateUserID],[CreateDate],[ReturnMoneyDate],[ReturnMoneyUserId],[ReturnMoneyUser],[ts],[modifyuser],[Atta],[IsOutState],[IsPayColl],[CostSub],[bateAmount],[IsSettl],[GiveMode],[PostFee],[vdef1],[vdef2],[vdef3])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@DisUserID,@AddType,@Otype,@IsAudit,@AddrID,@ReceiptNo,@GUID,@ArriveDate,@TotalAmount,@AuditAmount,@OtherAmount,@PayedAmount,@Principal,@Phone,@Address,@Remark,@OState,@PayState,@ReturnState,@AuditUserID,@AuditDate,@AuditRemark,@CreateUserID,@CreateDate,@ReturnMoneyDate,@ReturnMoneyUserId,@ReturnMoneyUser,@ts,@modifyuser,@Atta,@IsOutState,@IsPayColl,@CostSub,@bateAmount,@IsSettl,@GiveMode,@PostFee,@vdef1,@vdef2,@vdef3)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
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
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.DisUserID;
            parameters[3].Value = model.AddType;
            parameters[4].Value = model.Otype;
            parameters[5].Value = model.IsAudit;
            parameters[6].Value = model.AddrID;
            parameters[7].Value = model.ReceiptNo;

            if (model.GUID != null)
                parameters[8].Value = model.GUID;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ArriveDate != DateTime.MinValue)
                parameters[9].Value = model.ArriveDate;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.TotalAmount;
            parameters[11].Value = model.AuditAmount;
            parameters[12].Value = model.OtherAmount;
            parameters[13].Value = model.PayedAmount;

            if (model.Principal != null)
                parameters[14].Value = model.Principal;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[15].Value = model.Phone;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Address != null)
                parameters[16].Value = model.Address;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[17].Value = model.Remark;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.OState;
            parameters[19].Value = model.PayState;
            parameters[20].Value = model.ReturnState;
            parameters[21].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[22].Value = model.AuditDate;
            else
                parameters[22].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[23].Value = model.AuditRemark;
            else
                parameters[23].Value = DBNull.Value;

            parameters[24].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[25].Value = model.CreateDate;
            else
                parameters[25].Value = DBNull.Value;


            if (model.ReturnMoneyDate != DateTime.MinValue)
                parameters[26].Value = model.ReturnMoneyDate;
            else
                parameters[26].Value = DBNull.Value;

            parameters[27].Value = model.ReturnMoneyUserId;

            if (model.ReturnMoneyUser != null)
                parameters[28].Value = model.ReturnMoneyUser;
            else
                parameters[28].Value = DBNull.Value;

            parameters[29].Value = model.ts;
            parameters[30].Value = model.modifyuser;

            if (model.Atta != null)
                parameters[31].Value = model.Atta;
            else
                parameters[31].Value = DBNull.Value;

            parameters[32].Value = model.IsOutState;

            if (model.IsPayColl != null)
                parameters[33].Value = model.IsPayColl;
            else
                parameters[33].Value = DBNull.Value;


            if (model.CostSub != null)
                parameters[34].Value = model.CostSub;
            else
                parameters[34].Value = DBNull.Value;

            parameters[35].Value = model.bateAmount;

            if (model.IsSettl != null)
                parameters[36].Value = model.IsSettl;
            else
                parameters[36].Value = DBNull.Value;


            if (model.GiveMode != null)
                parameters[37].Value = model.GiveMode;
            else
                parameters[37].Value = DBNull.Value;

            parameters[38].Value = model.PostFee;

            if (model.vdef1 != null)
                parameters[39].Value = model.vdef1;
            else
                parameters[39].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[40].Value = model.vdef2;
            else
                parameters[40].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[41].Value = model.vdef3;
            else
                parameters[41].Value = DBNull.Value;

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
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteScalar().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 修改订单（事务）
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="model"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int UpdateOrder(SqlConnection sqlconn, Hi.Model.DIS_Order model, SqlTransaction sqltans)
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
        /// 更新一条数据,带事务
        /// </summary>
        public bool Update(Hi.Model.DIS_Order model, SqlTransaction TranSaction=null)
        {
            return dal.Update(model, TranSaction);
        }

        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="OrderId">订单Id</param>
        /// <returns></returns>
        public bool DelOrder(int OrderId)
        {
            return dal.DelOrder(OrderId);
        }

        public Hi.Model.DIS_Order GetModel(string ReceiptNo)
        {
            return dal.GetModel(ReceiptNo);
        }

        /// <summary>
        /// 修改支付状态
        /// </summary>
        /// <param name="sqlconn">数据库连接</param>
        /// <param name="id">订单Id</param>
        /// <param name="sumPrice">本次支付金额</param>
        /// <param name="sqltans">事物</param>
        /// <returns></returns>
        public int UpdateOrderPstate(SqlConnection sqlconn, int id, decimal sumPrice, SqlTransaction sqltans)
        {
            Hi.Model.DIS_Order orderM = new Hi.BLL.DIS_Order().GetModel(id);
            int PayState = 0;
            if ((orderM.AuditAmount + orderM.OtherAmount) > (orderM.PayedAmount + sumPrice))
            {
                PayState = 1;
            }
            else if ((orderM.AuditAmount + orderM.OtherAmount) == (orderM.PayedAmount + sumPrice))
            {
                PayState = 2;
            }
            else
            {
                return 0;
            }
            //修改已支付金额时，如果金额丢失为零，提示修改失败。
            if (sumPrice == 0)
                return 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Order] set [PayState]=@PayState,[PayedAmount]+=@sumPrice,[ts]='" + DateTime.Now.ToString() + "' where [ID]=@ID");
            SqlParameter[] parameters = { new SqlParameter("@sumPrice", SqlDbType.Decimal), new SqlParameter("@ID", SqlDbType.Int), new SqlParameter("@PayState", SqlDbType.Int) };
            parameters[0].Value = sumPrice;
            parameters[1].Value = id;
            parameters[2].Value = PayState;

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
        /// 修改支付状态
        /// </summary>
        /// <param name="sqlconn">数据库连接</param>
        /// <param name="id">订单Id</param>
        /// <param name="sumPrice">本次支付金额</param>
        /// <param name="PayState">付款状态（已支付、部分支付）</param>
        /// <param name="sqltans">事物</param>
        /// <returns></returns>
        public int UpdateOrderPstate(SqlConnection sqlconn, int id, decimal sumPrice,int PayState,SqlTransaction sqltans)
        {
           
            //修改已支付金额时，如果金额丢失为零，提示修改失败。
            if (sumPrice == 0)
                return 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Order] set [PayState]=@PayState,[PayedAmount]+=@sumPrice,[ts]='" + DateTime.Now.ToString() + "' where [ID]=@ID");
            SqlParameter[] parameters = { new SqlParameter("@sumPrice", SqlDbType.Decimal), new SqlParameter("@ID", SqlDbType.Int), new SqlParameter("@PayState", SqlDbType.Int) };
            parameters[0].Value = sumPrice;
            parameters[1].Value = id;
            parameters[2].Value = PayState;

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
        /// 修改订单退款状态，modify by ggh
        /// 创建时间：2015-06-09
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="model"></param>
        /// <param name="sqltans"></param>
        /// <param name="returnID">退货单ID，根据退货单Id判断方法的重载</param>
        /// <param name="returnstart">退货单状态</param>
        /// <returns></returns>
        public int UpdateOrderByggh(SqlConnection sqlconn, Hi.Model.DIS_Order model, SqlTransaction sqltans, int returnID, int returnstart)
        {
           return  dal.UpdateOrderByggh(sqlconn, model, sqltans,returnID,returnstart);
        }

        public List<Hi.Model.DIS_Order> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction TranSaction = null)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, TranSaction) as List<Hi.Model.DIS_Order>;
        }

        public DataSet GetData(string ReceiptNo)
        {
            return dal.GetData(ReceiptNo);
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
            return dal.UpdateOrder(strWhat, parameters, strWhere);
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
            return dal.GetList(strWhat, TbName, strWhere, strOrderby);
        }

        /// <summary>
        ///  下单、修改订单时，修改商品库存
        /// </summary>
        /// <param name="str">修改订单时，加库存</param>
        /// <param name="OrderDetailList">新增商品信息</param>
        /// <returns></returns>
        public string GetSqlInventory(string str, List<Hi.Model.DIS_OrderDetail> OrderDetailList)
        {
            StringBuilder sb = new StringBuilder();
            
            if (!str.Equals(""))
            {
                //订单修改时，订单明细删除的商品，返还商品库存
                List<Hi.Model.DIS_OrderDetail> odl = new Hi.BLL.DIS_OrderDetail().GetList("", str, "");

                if (odl != null && odl.Count > 0)
                {
                    foreach (var item in odl)
                    {
                        sb.AppendFormat("update BD_GoodsInfo set Inventory+={0} where ID={1};", item.GoodsNum + Convert.ToDecimal(item.ProNum), item.GoodsinfoID);
                    }
                }
            }
            if (OrderDetailList != null && OrderDetailList.Count > 0)
            {
                foreach (var item in OrderDetailList)
                {
                    sb.AppendFormat("update BD_GoodsInfo set Inventory-={0} where ID={1};", item.GoodsNum + Convert.ToDecimal(item.ProNum), item.GoodsinfoID);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 修改商品库存，订单商品明细为NULL、传订单ID，不为空时传订单明细、订单为0；
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="odl">订单商品明细</param>
        /// <param name="type">0、加商品库存  1、减商品库存</param>
        /// <returns></returns>
        public string GetSqlAddInve(int OrderID, List<Hi.Model.DIS_OrderDetail> odl,int type)
        {
            StringBuilder sb = new StringBuilder();

            if (OrderID != 0)
            {
                //传订单ID，查询订单明细
                List<Hi.Model.DIS_OrderDetail> l = new Hi.BLL.DIS_OrderDetail().GetList("", " IsNUll(dr,0)=0 and OrderID=" + OrderID, "");
                if (l != null && l.Count > 0)
                {
                    foreach (var item in l)
                    {
                        if (type == 0)
                        {
                            sb.AppendFormat("update BD_GoodsInfo set Inventory+={0} where ID={1};", item.GoodsNum +Convert.ToDecimal( item.ProNum), item.GoodsinfoID);
                        }
                        else
                        {
                            sb.AppendFormat("update BD_GoodsInfo set Inventory-={0} where ID={1};", item.GoodsNum + Convert.ToDecimal(item.ProNum), item.GoodsinfoID);
                        }
                    }
                }
            }
            else if (odl != null && odl.Count > 0)
            {
                foreach (var item in odl)
                {
                    if (type == 0)
                    {
                        sb.AppendFormat("update BD_GoodsInfo set Inventory+={0} where ID={1};", item.GoodsNum + Convert.ToDecimal(item.ProNum), item.GoodsinfoID);
                    }
                    else
                    {
                        sb.AppendFormat("update BD_GoodsInfo set Inventory-={0} where ID={1};", item.GoodsNum + Convert.ToDecimal(item.ProNum), item.GoodsinfoID);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 查询订单信息 code 2.5  by 2016-08-01 szj
        /// </summary>
        /// <param name="strWhat"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetList(string strWhat, string where)
        {
            return dal.GetList(strWhat, where);
        }
        public DataTable GetLists(string sql)
        {
            return dal.GetLists(sql);

        }

        /// <summary>
        /// 判断修改的时间戳
        /// </summary>
        /// <param name="DatableName">表名</param>
        /// <param name="id">主键ID</param>
        /// <param name="ts">原时间戳</param>
        /// <returns>0，不能修改  1、可修改</returns>
        public int Getts(string DatableName, int id, DateTime ts, SqlTransaction TranSaction = null)
        {
            //可以做循环   
            try
            {
                string sql = "select ts from " + DatableName + " where ID=@ID";

                SqlParameter[] cmdParms = {
                    new SqlParameter("@ID", SqlDbType.Int)
                };
                cmdParms[0].Value = id;
                DataTable dt = null;

                if (TranSaction != null)
                {
                    SqlCommand comd = new SqlCommand(sql, TranSaction.Connection, TranSaction);
                    comd.CommandType = CommandType.Text;

                    if (cmdParms != null)
                    {
                        foreach (SqlParameter parameter in cmdParms)
                        {
                            if (parameter.SqlDbType == SqlDbType.DateTime)
                            {
                                if (parameter.Value == DBNull.Value)
                                {
                                    parameter.Value = DBNull.Value;
                                }

                            }
                            comd.Parameters.Add(parameter);
                        }
                    }
                }
                else
                {
                    dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql, cmdParms).Tables[0];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dt.Rows[0]["ts"]).ToString("yyyy/MM/dd HH:mm:ss") == ts.ToString("yyyy/MM/dd HH:mm:ss"))
                        return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                if (TranSaction != null)
                    TranSaction.Rollback();
                return 0;
            }
        }


        /// <summary>
        /// 订单完成节点设置
        /// </summary>
        /// <param name="CompID">核心企业</param>
        /// <returns></returns>
        public  int OstateAudit(int CompID)
        {
            int OAduit = 0;
            try
            {
                List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", " CompID=" + CompID + " and Name='订单完成节点设置' and dr=0", "");
                if (sl != null && sl.Count > 0)
                {
                    OAduit= Convert.ToInt32(sl[0].Value);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return OAduit;
        }
    }
}
