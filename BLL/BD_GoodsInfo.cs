using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

namespace Hi.BLL
{
    public partial class BD_GoodsInfo
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Adds(Hi.Model.BD_GoodsInfo model)
        {
            return dal.Adds(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(SqlConnection sqlconn, Hi.Model.BD_GoodsInfo model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_GoodsInfo](");
            strSql.Append("[CompID],[GoodsID],[BarCode],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9],[Value10],[ValueInfo],[SalePrice],[TinkerPrice],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsID,@BarCode,@Value1,@Value2,@Value3,@Value4,@Value5,@Value6,@Value7,@Value8,@Value9,@Value10,@ValueInfo,@SalePrice,@TinkerPrice,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50),
                    new SqlParameter("@Value1", SqlDbType.VarChar,50),
                    new SqlParameter("@Value2", SqlDbType.VarChar,50),
                    new SqlParameter("@Value3", SqlDbType.VarChar,50),
                    new SqlParameter("@Value4", SqlDbType.VarChar,50),
                    new SqlParameter("@Value5", SqlDbType.VarChar,50),
                    new SqlParameter("@Value6", SqlDbType.VarChar,50),
                    new SqlParameter("@Value7", SqlDbType.VarChar,50),
                    new SqlParameter("@Value8", SqlDbType.VarChar,50),
                    new SqlParameter("@Value9", SqlDbType.VarChar,50),
                    new SqlParameter("@Value10", SqlDbType.VarChar,50),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsID;

            if (model.BarCode != null)
                parameters[2].Value = model.BarCode;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Value1 != null)
                parameters[3].Value = model.Value1;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[4].Value = model.Value2;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[5].Value = model.Value3;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[6].Value = model.Value4;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[7].Value = model.Value5;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Value6 != null)
                parameters[8].Value = model.Value6;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Value7 != null)
                parameters[9].Value = model.Value7;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Value8 != null)
                parameters[10].Value = model.Value8;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Value9 != null)
                parameters[11].Value = model.Value9;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Value10 != null)
                parameters[12].Value = model.Value10;
            else
                parameters[12].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[13].Value = model.ValueInfo;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.SalePrice;
            parameters[15].Value = model.TinkerPrice;
            parameters[16].Value = model.IsEnabled;
            parameters[17].Value = model.CreateUserID;
            parameters[18].Value = model.CreateDate;
            parameters[19].Value = model.ts;
            parameters[20].Value = model.modifyuser;

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
        /// 批量插入BD_GoodsInfo表
        /// </summary>
        /// <param name="ll"></param>
        public bool InserGoodsInfo(List<Hi.Model.BD_GoodsInfo> ll)
        {
            return dal.InserGoodsInfo(ll);
        }
        /// <summary>
        /// 删除GoodInfo
        /// </summary>
        /// <returns></returns>
        public int Deletes(string goodsid, int compid)
        {
            return dal.Deletes(goodsid, compid);
        }
        /// <summary>
        /// 根据GoodsID查找GoodsInfoID
        /// </summary>
        /// <returns></returns>
        public string GetGoodsInfoID(string GoodsID)
        {
            return dal.GetGoodsInfoID(GoodsID);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_GoodsInfo model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsInfo model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }

        public List<Hi.Model.BD_GoodsInfo> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_GoodsInfo>;
        }
        /// <summary>
        /// 获取新的goodsinfo排序实体
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="goodsId"></param>
        /// <param name="categoryId"></param>
        /// <param name="Tran"></param>
        /// <returns></returns>
        public DataTable GetGoodsInfoModel(string compId, string goodsId, string categoryId, string infoIdList, SqlTransaction Tran)
        {
            return dal.GetGoodsInfoModel(compId, goodsId, categoryId, infoIdList, Tran);
        }
        public DataTable GetGoodsInfoModel(string compId, string goodsId, string categoryId)
        {
            return dal.GetGoodsInfoModel(compId, goodsId, categoryId, "", null);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_GoodsInfo GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }
        /// <summary>
        /// 获取goods表没删除的商品信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetGoodsModel(string where)
        {
            return dal.GetGoodsModel(where);
        }

        /// <summary>
        /// 获取goods表没删除的商品信息  关联首营ID
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet getGoodsModels(string where) {
            return dal.getGoodsModels(where);
        }

         /// <summary>
        /// 获取代理商信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet getGoodsCMerchants(string where)
        {
            return dal.getGoodsCMerchants(where);
        }

        public DataSet getDisGoodsStock(string where) {
            return dal.getDisGoodsStock(where);
        }


    }
}
