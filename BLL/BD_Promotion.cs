using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Data;

namespace Hi.BLL
{
    public partial class BD_Promotion
    {
        public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

        /// <summary>
        /// 新增促销活动
        /// </summary>
        /// <param name="proModel">促销活动主信息</param>
        /// <param name="proList">促销活动商品信息</param>
        /// <returns></returns>
        public int AddPro(Hi.Model.BD_Promotion proModel, List<Hi.Model.BD_PromotionDetail> proList)
        {
            int Id = 0;
            SqlTransaction TranSaction = null;
            SqlConnection Connection = new SqlConnection(LocalSqlServer);

            try
            {
                Connection.Open();
                TranSaction = Connection.BeginTransaction();

                Id = ProAdd(proModel, Connection, TranSaction);

                if (Id == 0)
                {
                    Id = 0;
                    TranSaction.Rollback();
                }

                if (proList != null && proList.Count > 0)
                {
                    foreach (var item in proList)
                    {
                        item.ProID = Id;
                        int count = ProDAdd(item, Connection, TranSaction);

                        if (count == 0)
                        {
                            Id = 0;
                            TranSaction.Rollback();
                        }

                    }
                }

                TranSaction.Commit();

            }
            catch (Exception ex)
            {
                Id = 0;
                TranSaction.Rollback();
                throw ex;
            }
            finally
            {
                Connection.Close();
            }

            return Id;
        }

        /// <summary>
        /// 新增促销主表（事务）
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="sqlconn"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int ProAdd(Hi.Model.BD_Promotion model, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Promotion](");
            strSql.Append("[CompID],[ProTitle],[Type],[ProType],[Discount],[ProInfos],[IsEnabled],[ProStartTime],[ProEndTime],[CreateUserID],[CreateDate],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@ProTitle,@Type,@ProType,@Discount,@ProInfos,@IsEnabled,@ProStartTime,@ProEndTime,@CreateUserID,@CreateDate,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ProTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ProType", SqlDbType.Int),
                    new SqlParameter("@Discount", SqlDbType.Decimal),
                    new SqlParameter("@ProInfos", SqlDbType.NVarChar,2000),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@ProStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ProEndTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.Char,19),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;

            if (model.ProTitle != null)
                parameters[1].Value = model.ProTitle;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.Type;
            parameters[3].Value = model.ProType;
            parameters[4].Value = model.Discount;

            if (model.ProInfos != null)
                parameters[5].Value = model.ProInfos;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsEnabled;
            parameters[7].Value = model.ProStartTime;
            parameters[8].Value = model.ProEndTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.dr;
            parameters[13].Value = model.modifyuser;

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
        /// 新增促销商品明细（事务）
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="sqlconn"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int ProDAdd(Hi.Model.BD_PromotionDetail model, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_PromotionDetail](");
            strSql.Append("[ProID],[CompID],[GoodsID],[GoodsName],[GoodsUnit],[Goodsmemo],[GoodInfoID],[GoodsPrice],[SendGoodsinfoID],[CreateUserID],[CreateDate],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@ProID,@CompID,@GoodsID,@GoodsName,@GoodsUnit,@Goodsmemo,@GoodInfoID,@GoodsPrice,@SendGoodsinfoID,@CreateUserID,@CreateDate,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,128),
                    new SqlParameter("@GoodsUnit", SqlDbType.VarChar,10),
                    new SqlParameter("@Goodsmemo", SqlDbType.Text),
                    new SqlParameter("@GoodInfoID", SqlDbType.Int),
                    new SqlParameter("@GoodsPrice", SqlDbType.Decimal),
                    new SqlParameter("@SendGoodsinfoID", SqlDbType.Decimal),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.Char,19),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ProID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsID;
            parameters[3].Value = model.GoodsName;

            if (model.GoodsUnit != null)
                parameters[4].Value = model.GoodsUnit;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Goodsmemo != null)
                parameters[5].Value = model.Goodsmemo;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.GoodInfoID;
            parameters[7].Value = model.GoodsPrice;
            parameters[8].Value = model.SendGoodsinfoID;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.dr;
            parameters[13].Value = model.modifyuser;

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
        /// 修改促销活动（事务）
        /// </summary>
        /// <param name="proModel"></param>
        /// <param name="proList"></param>
        /// <returns></returns>
        public int ProUpdate(Hi.Model.BD_Promotion proModel, List<Hi.Model.BD_PromotionDetail> proList)
        {
            int Id = 0;
            SqlTransaction TranSaction = null;
            SqlConnection Connection = new SqlConnection(LocalSqlServer);
            try
            {
                Connection.Open();
                TranSaction = Connection.BeginTransaction();

                //修改促销活动主表信息
                Id = UpdateProSql(proModel, Connection, TranSaction);

                if (Id == 0)
                {
                    Id = 0;
                    TranSaction.Rollback();
                    return Id;
                }

                //修改促销活动时 先删除原商品信息
                int delId = DelProDSql(proModel.ID, Connection, TranSaction);

                //新增促销商品明细
                if (proList != null && proList.Count > 0)
                {
                    foreach (var item in proList)
                    {
                        item.ProID = proModel.ID;
                        int count = ProDAdd(item, Connection, TranSaction);

                        if (count == 0)
                        {
                            Id = 0;
                            TranSaction.Rollback();
                        }

                    }
                }

                TranSaction.Commit();

            }
            catch (Exception ex)
            {
                Id = 0;
                TranSaction.Rollback();
                throw ex;
            }
            finally
            {
                Connection.Close();
            }
            return Id;
        }

        /// <summary>
        /// 修改促销主表（事务）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sqlconn"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int UpdateProSql(Hi.Model.BD_Promotion model, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Promotion] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[ProTitle]=@ProTitle,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[ProType]=@ProType,");
            strSql.Append("[Discount]=@Discount,");
            strSql.Append("[ProInfos]=@ProInfos,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[ProStartTime]=@ProStartTime,");
            strSql.Append("[ProEndTime]=@ProEndTime,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ProTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ProType", SqlDbType.Int),
                    new SqlParameter("@Discount", SqlDbType.Decimal),
                    new SqlParameter("@ProInfos", SqlDbType.NVarChar,2000),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@ProStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ProEndTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.Char,19),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.ProTitle != null)
                parameters[2].Value = model.ProTitle;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.Type;
            parameters[4].Value = model.ProType;
            parameters[5].Value = model.Discount;

            if (model.ProInfos != null)
                parameters[6].Value = model.ProInfos;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.IsEnabled;
            parameters[8].Value = model.ProStartTime;
            parameters[9].Value = model.ProEndTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;

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
        /// 修改促销活动时 先删除原商品信息
        /// </summary>
        /// <param name="ProId"></param>
        /// <param name="sqlconn"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int DelProDSql(int ProId, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_PromotionDetail] where ProId=@ProID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProID", SqlDbType.Int)
            };
            parameters[0].Value = ProId;


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
        /// 修改促销明细（事务）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sqlconn"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int UpdateProDSql(Hi.Model.BD_PromotionDetail model, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_PromotionDetail] set ");
            strSql.Append("[ProID]=@ProID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[GoodsUnit]=@GoodsUnit,");
            strSql.Append("[Goodsmemo]=@Goodsmemo,");
            strSql.Append("[GoodInfoID]=@GoodInfoID,");
            strSql.Append("[GoodsPrice]=@GoodsPrice,");
            strSql.Append("[SendGoodsinfoID]=@SendGoodsinfoID,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,128),
                    new SqlParameter("@GoodsUnit", SqlDbType.VarChar,10),
                    new SqlParameter("@Goodsmemo", SqlDbType.Text),
                    new SqlParameter("@GoodInfoID", SqlDbType.Int),
                    new SqlParameter("@GoodsPrice", SqlDbType.Decimal),
                    new SqlParameter("@SendGoodsinfoID", SqlDbType.Decimal),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.Char,19),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ProID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.GoodsID;
            parameters[4].Value = model.GoodsName;

            if (model.GoodsUnit != null)
                parameters[5].Value = model.GoodsUnit;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Goodsmemo != null)
                parameters[6].Value = model.Goodsmemo;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.GoodInfoID;
            parameters[8].Value = model.GoodsPrice;
            parameters[9].Value = model.SendGoodsinfoID;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;

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
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Promotion model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Promotion GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Promotion model, SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }

        /// <summary>
        /// 查询商品使用的促销方式
        /// </summary>
        /// <param name="proID">促销ID</param>
        /// <returns></returns>
        public DataTable ProType(string proID)
        {
            return dal.ProType(proID);
        }
    }
}
