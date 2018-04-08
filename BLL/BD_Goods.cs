using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using DBUtility;

namespace Hi.BLL
{
    public partial class BD_Goods
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Adds(Hi.Model.BD_Goods model)
        {
            return dal.Adds(model);
        }

        /// <summary>
        /// 增加一条数据,带事务
        /// </summary>
        public int Add(SqlConnection sqlconn, Hi.Model.BD_Goods model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Goods](");
            strSql.Append("[CompID],[GoodsName],[GoodsCode],[CategoryID],[Unit],[SalePrice],[IsOffline],[IsIndex],[IsSale],[IsRecommended],[OfflineStateDate],[OfflineEndDate],[Pic],[Pic2],[Title],[Details],[memo],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[Pic3])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsName,@GoodsCode,@CategoryID,@Unit,@SalePrice,@IsOffline,@IsIndex,@IsSale,@IsRecommended,@OfflineStateDate,@OfflineEndDate,@Pic,@Pic2,@Title,@Details,@memo,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser,@Pic3)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,128),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,32),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@Unit", SqlDbType.VarChar,10),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@IsOffline", SqlDbType.Int),
                    new SqlParameter("@IsIndex", SqlDbType.Int),
                    new SqlParameter("@IsSale", SqlDbType.Int),
                    new SqlParameter("@IsRecommended", SqlDbType.Int),
                    new SqlParameter("@OfflineStateDate", SqlDbType.DateTime),
                    new SqlParameter("@OfflineEndDate", SqlDbType.DateTime),
                    new SqlParameter("@Pic", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic2", SqlDbType.VarChar,128),
                    new SqlParameter("@Title", SqlDbType.VarChar,100),
                    new SqlParameter("@Details", SqlDbType.Text),
                    new SqlParameter("@memo", SqlDbType.Text),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                            new SqlParameter("@Pic3", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsName;

            if (model.GoodsCode != null)
                parameters[2].Value = model.GoodsCode;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.CategoryID;

            if (model.Unit != null)
                parameters[4].Value = model.Unit;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.SalePrice;
            parameters[6].Value = model.IsOffline;
            parameters[7].Value = model.IsIndex;
            parameters[8].Value = model.IsSale;
            parameters[9].Value = model.IsRecommended;

            if (model.OfflineStateDate != DateTime.MinValue)
                parameters[10].Value = model.OfflineStateDate;
            else
                parameters[10].Value = DBNull.Value;


            if (model.OfflineEndDate != DateTime.MinValue)
                parameters[11].Value = model.OfflineEndDate;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Pic != null)
                parameters[12].Value = model.Pic;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Pic2 != null)
                parameters[13].Value = model.Pic2;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Title != null)
                parameters[14].Value = model.Title;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Details != null)
                parameters[15].Value = model.Details;
            else
                parameters[15].Value = DBNull.Value;


            if (model.memo != null)
                parameters[16].Value = model.memo;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.IsEnabled;
            parameters[18].Value = model.CreateUserID;
            parameters[19].Value = model.CreateDate;
            parameters[20].Value = model.ts;
            parameters[21].Value = model.modifyuser;
            if (model.Pic3 != null)
                parameters[22].Value = model.Pic3;
            else
                parameters[22].Value = DBNull.Value;

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
        /// 商品属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="compid"></param>
        /// <returns></returns>
        public DataTable GoodsAttr(string id, string compid)
        {
            return dal.GoodsAttr(id, compid);
        }

        /// <summary>
        /// 商品属性、属性值
        /// </summary>
        /// <param name="Id">商品Id</param>
        /// <param name="compid">企业Id</param>
        /// <returns></returns>
        public DataTable GoodsAttributeValue(string Id, string compid)
        {
            return dal.GoodsAttributeValue(Id, compid);
        }

        /// <summary>
        /// 全部上架
        /// </summary>
        /// <returns></returns>
        public int Updates()
        {
            return dal.Updates();
        }

        /// <summary>
        /// 是否上下架插入
        /// </summary>
        /// <returns></returns>
        public int Addss(Hi.Model.BD_Goods model)
        {
            return dal.Addss(model);
        }

        public List<Hi.Model.BD_Goods> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_Goods>;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Goods model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Goods model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Goods GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }

        public DataTable GetList(int pageSize, int pageIndex, string fldSort, bool sort, string fldName, string TbName, string strCondition, out int pageCount, out int count, string CustomFldName = "", bool IsDist = true)
        {
            return dal.GetList(pageSize, pageIndex, fldSort, sort, fldName, TbName, strCondition, out  pageCount, out  count, CustomFldName,IsDist);
        }

        public DataTable GetPic(string strwhere)
        {
            return dal.GetPic(strwhere);
        }

        public DataTable GoodsAttr(string sql)
        {
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        }
    }
}
