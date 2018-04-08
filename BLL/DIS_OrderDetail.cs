using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Data;

namespace Hi.BLL
{
    public partial class DIS_OrderDetail
    {

        /// <summary>
        /// 订单明细添加 （事务）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddOrderDetail(SqlConnection sqlconn, Hi.Model.DIS_OrderDetail model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderDetail](");
            strSql.Append("[OrderID],[DisID],[GoodsinfoID],[GoodsName],[GoodsCode],[Unit],[GoodsInfos],[GoodsNum],[OutNum],[IsOut],[GoodsPrice],[AuditAmount],[sumAmount],[Price],[SharePrice],[ProID],[ProNum],[Protype],[Remark],[ts],[modifyuser],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@DisID,@GoodsinfoID,@GoodsName,@GoodsCode,@Unit,@GoodsInfos,@GoodsNum,@OutNum,@IsOut,@GoodsPrice,@AuditAmount,@sumAmount,@Price,@SharePrice,@ProID,@ProNum,@Protype,@Remark,@ts,@modifyuser,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsinfoID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,200),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,200),
                    new SqlParameter("@Unit", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsInfos", SqlDbType.VarChar,500),
                    new SqlParameter("@GoodsNum", SqlDbType.Decimal),
                    new SqlParameter("@OutNum", SqlDbType.Decimal),
                    new SqlParameter("@IsOut", SqlDbType.Int),
                    new SqlParameter("@GoodsPrice", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@sumAmount", SqlDbType.Decimal),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@SharePrice", SqlDbType.Decimal),
                    new SqlParameter("@ProID", SqlDbType.VarChar,128),
                    new SqlParameter("@ProNum", SqlDbType.VarChar,128),
                    new SqlParameter("@Protype", SqlDbType.VarChar,128),
                    new SqlParameter("@Remark", SqlDbType.VarChar,300),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.GoodsinfoID;

            if (model.GoodsName != null)
                parameters[3].Value = model.GoodsName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.GoodsCode != null)
                parameters[4].Value = model.GoodsCode;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[5].Value = model.Unit;
            else
                parameters[5].Value = DBNull.Value;


            if (model.GoodsInfos != null)
                parameters[6].Value = model.GoodsInfos;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.GoodsNum;
            parameters[8].Value = model.OutNum;
            parameters[9].Value = model.IsOut;
            parameters[10].Value = model.GoodsPrice;
            parameters[11].Value = model.AuditAmount;
            parameters[12].Value = model.sumAmount;
            parameters[13].Value = model.Price;
            parameters[14].Value = model.SharePrice;

            if (model.ProID != null)
                parameters[15].Value = model.ProID;
            else
                parameters[15].Value = DBNull.Value;


            if (model.ProNum != null)
                parameters[16].Value = model.ProNum;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Protype != null)
                parameters[17].Value = model.Protype;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[18].Value = model.Remark;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.ts;
            parameters[20].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[21].Value = model.vdef1;
            else
                parameters[21].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[22].Value = model.vdef2;
            else
                parameters[22].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[23].Value = model.vdef3;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[24].Value = model.vdef4;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[25].Value = model.vdef5;
            else
                parameters[25].Value = DBNull.Value;

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
        /// 订单明细修改 （事务）
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="model"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int UpdateOrderDetail(SqlConnection sqlconn, Hi.Model.DIS_OrderDetail model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderDetail] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[GoodsinfoID]=@GoodsinfoID,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[GoodsCode]=@GoodsCode,");
            strSql.Append("[Unit]=@Unit,");
            strSql.Append("[GoodsInfos]=@GoodsInfos,");
            strSql.Append("[GoodsNum]=@GoodsNum,");
            strSql.Append("[OutNum]=@OutNum,");
            strSql.Append("[IsOut]=@IsOut,");
            strSql.Append("[GoodsPrice]=@GoodsPrice,");
            strSql.Append("[AuditAmount]=@AuditAmount,");
            strSql.Append("[sumAmount]=@sumAmount,");
            strSql.Append("[Price]=@Price,");
            strSql.Append("[SharePrice]=@SharePrice,");
            strSql.Append("[ProID]=@ProID,");
            strSql.Append("[ProNum]=@ProNum,");
            strSql.Append("[Protype]=@Protype,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsinfoID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,200),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,200),
                    new SqlParameter("@Unit", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsInfos", SqlDbType.VarChar,500),
                    new SqlParameter("@GoodsNum", SqlDbType.Decimal),
                    new SqlParameter("@OutNum", SqlDbType.Decimal),
                    new SqlParameter("@IsOut", SqlDbType.Int),
                    new SqlParameter("@GoodsPrice", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@sumAmount", SqlDbType.Decimal),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@SharePrice", SqlDbType.Decimal),
                    new SqlParameter("@ProID", SqlDbType.VarChar,128),
                    new SqlParameter("@ProNum", SqlDbType.VarChar,128),
                    new SqlParameter("@Protype", SqlDbType.VarChar,128),
                    new SqlParameter("@Remark", SqlDbType.VarChar,300),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.GoodsinfoID;

            if (model.GoodsName != null)
                parameters[4].Value = model.GoodsName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.GoodsCode != null)
                parameters[5].Value = model.GoodsCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[6].Value = model.Unit;
            else
                parameters[6].Value = DBNull.Value;


            if (model.GoodsInfos != null)
                parameters[7].Value = model.GoodsInfos;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.GoodsNum;
            parameters[9].Value = model.OutNum;
            parameters[10].Value = model.IsOut;
            parameters[11].Value = model.GoodsPrice;
            parameters[12].Value = model.AuditAmount;
            parameters[13].Value = model.sumAmount;
            parameters[14].Value = model.Price;
            parameters[15].Value = model.SharePrice;

            if (model.ProID != null)
                parameters[16].Value = model.ProID;
            else
                parameters[16].Value = DBNull.Value;


            if (model.ProNum != null)
                parameters[17].Value = model.ProNum;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Protype != null)
                parameters[18].Value = model.Protype;
            else
                parameters[18].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[19].Value = model.Remark;
            else
                parameters[19].Value = DBNull.Value;

            parameters[20].Value = model.ts;
            parameters[21].Value = model.dr;
            parameters[22].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[23].Value = model.vdef1;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[24].Value = model.vdef2;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[25].Value = model.vdef3;
            else
                parameters[25].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[26].Value = model.vdef4;
            else
                parameters[26].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[27].Value = model.vdef5;
            else
                parameters[27].Value = DBNull.Value;

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

        public string GetDs(string orderid)
        {
            DataSet ds = dal.GetDs(orderid);
            string goodsinfoids = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                goodsinfoids += ds.Tables[0].Rows[i]["goodsinfoid"] + ",";
            }
            return goodsinfoids;
        }

        public Hi.Model.DIS_OrderDetail GetModel(string orderid, string goodsid)
        {
            return dal.GetModel(orderid, goodsid);
        }

        /// <summary>
        /// 删除订单商品明细
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetDel(string str, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            return dal.GetDel(str, sqlconn, sqltans);
        }

        ///// <summary>
        ///// 删除订单商品明细
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public int GetDel(SqlConnection sqlconn,string str,SqlTransaction sqltans)
        //{
        //    return dal.GetDel(str);
        //}

        /// <summary>
        /// 订单明细与商品的连接查询
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrderDetail(string strWhere, string strOrderby)
        {
            return dal.GetOrderDetail(strWhere, strOrderby);
        }

        /// <summary>
        /// 修改单个商品信息
        /// </summary>
        /// <param name="strWhat">修改的信息</param>
        /// <param name="parameters">修改的参数</param>
        /// <param name="strWhere">修改条件</param>
        /// <returns></returns>
        public bool UpdateOrderDetail(string strWhat, SqlParameter[] parameters, string strWhere)
        {
            return dal.UpdateOrderDetail(strWhat, parameters, strWhere);
        }

        /// <summary>
        /// 修改订单删除商品时，返还库存
        /// </summary>
        /// <param name="str">修改库存的sql</param>
        /// <returns></returns>
        public int GetUpdateInventory(string str, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            return dal.GetUpdateInventory(str, sqlconn, sqltans);
        }

        /// <summary>
        /// 判断商品库存是否满足
        /// </summary>
        /// <param name="goodsInfoID">商品信息ID</param>
        /// <param name="OrderId">订单ID</param>
        /// <param name="num">下单商品库存数量</param>
        /// <returns>0、没有库存  1、返回商品库存数量，包括修改订单时，修改订单商品明细数量</returns>
        public static decimal GetInevntory(int goodsInfoID, int OrderId, decimal num)
        {
            decimal Inventory = 0;

            if (OrderId != 0)
            {
                //修改订单时，判断商品库存加上订单明细上的该商品数量
                List<Hi.Model.DIS_OrderDetail> l = new Hi.BLL.DIS_OrderDetail().GetList("", " IsNull(dr,0)=0 and GoodsInfoID=" + goodsInfoID + " and OrderID=" + OrderId, "");

                if (l != null && l.Count <= 0)
                    Inventory = 0;
                else
                    Inventory = Convert.ToDecimal(l[0].GoodsNum.ToString());
            }

            Hi.Model.BD_GoodsInfo infoModel = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoID);
            Inventory += infoModel.Inventory;

            if (num != 0 && Inventory < num)
                return 0;
            return Inventory;
        }

         /// <summary>
        /// 查询订单商品明细
        /// </summary>
        /// <param name="strWhat"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetOrderDe(string strWhat, string strWhere)
        {
            return dal.GetOrderDe(strWhat, strWhere);
        }

        /// <summary>
        /// 订单分摊商品价格
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="TotalAmount">订单商品总价</param>
        /// <param name="AuditAmount">订单应收总价</param>
        /// <returns></returns>
        public int GetSharePrice(int OrderID, decimal TotalAmount, decimal AuditAmount)
        {
            int sh = 0;
            try
            {
                //订单商品总价小于等于0，不能分摊
                if (TotalAmount <= 0)
                    return 0;

                List<Hi.Model.DIS_OrderDetail> odl = new BLL.DIS_OrderDetail().GetList("", " isnull(dr,0)=0 and OrderID=" + OrderID, "");

                //分摊价格=（商品小计/订单商品总价）* 订单应收总价
                //最后一个商品分摊价格=订单应收总价-(分摊价格+分摊价格n);
                if (odl != null && odl.Count > 0)
                {
                    SqlTransaction sqlTrans = DBUtility.SqlHelper.CreateStoreTranSaction();

                    //订单商品个数
                    int ldline = odl.Count;
                    int i = 0;
                    //分摊商品小计总价
                    decimal SumShare = 0;

                    foreach (var item in odl)
                    {
                        //分摊商品单价小计
                        decimal Share = 0;

                        i++;
                        if (ldline == i)
                        {
                            //最后一个商品分摊价格
                            Share = AuditAmount - SumShare;
                            //分摊商品小计
                            item.SharePrice = Share;
                            //分摊商品单价
                            item.Price = Share / item.GoodsNum;
                        }
                        else
                        {
                            //Floor 返回小于或等于指定的取大整数
                            Share = (item.sumAmount / TotalAmount) * AuditAmount;
                            //垒加分摊商品小计总价Math.Floor()
                            SumShare += Share;
                            //分摊商品小计
                            item.SharePrice = Share;
                            //分摊商品单价
                            item.Price = Share / item.GoodsNum;
                        }

                        sh = new Hi.BLL.DIS_OrderDetail().UpdateOrderDetail(sqlTrans.Connection, item, sqlTrans);
                        if (sh == 0)
                        {
                            sqlTrans.Rollback();
                            return sh;
                        }
                    }
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sh;
        }
    }
}
