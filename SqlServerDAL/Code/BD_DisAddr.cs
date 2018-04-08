//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/18 12:07:26
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
    /// 数据访问类 BD_DisAddr
    /// </summary>
    public partial class BD_DisAddr
    {
        public BD_DisAddr()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DisAddr model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_DisAddr](");
            strSql.Append("[Name],[DisID],[Principal],[Phone],[Tel],[Province],[City],[Area],[Zip],[Address],[Remark],[IsDefault],[CreateUserID],[CreateDate],[ts],[modifyuser],[Code])");
            strSql.Append(" values (");
            strSql.Append("@Name,@DisID,@Principal,@Phone,@Tel,@Province,@City,@Area,@Zip,@Address,@Remark,@IsDefault,@CreateUserID,@CreateDate,@ts,@modifyuser,@Code)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.NChar,50),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Province", SqlDbType.VarChar,50),
                    new SqlParameter("@City", SqlDbType.VarChar,50),
                    new SqlParameter("@Area", SqlDbType.VarChar,100),
                    new SqlParameter("@Zip", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,100),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsDefault", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Code", SqlDbType.VarChar,50)
            };

            if (model.Name != null)
                parameters[0].Value = model.Name;
            else
                parameters[0].Value = DBNull.Value;

            parameters[1].Value = model.DisID;

            if (model.Principal != null)
                parameters[2].Value = model.Principal;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[3].Value = model.Phone;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[4].Value = model.Tel;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Province != null)
                parameters[5].Value = model.Province;
            else
                parameters[5].Value = DBNull.Value;


            if (model.City != null)
                parameters[6].Value = model.City;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Area != null)
                parameters[7].Value = model.Area;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Zip != null)
                parameters[8].Value = model.Zip;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Address != null)
                parameters[9].Value = model.Address;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[10].Value = model.Remark;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.IsDefault;
            parameters[12].Value = model.CreateUserID;
            parameters[13].Value = model.CreateDate;
            parameters[14].Value = model.ts;
            parameters[15].Value = model.modifyuser;
            if (model.Code != null)
                parameters[16].Value = model.Code;
            else
                parameters[16].Value = DBNull.Value;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_DisAddr model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_DisAddr] set ");
            strSql.Append("[Name]=@Name,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[Principal]=@Principal,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Tel]=@Tel,");
            strSql.Append("[Province]=@Province,");
            strSql.Append("[City]=@City,");
            strSql.Append("[Area]=@Area,");
            strSql.Append("[Zip]=@Zip,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsDefault]=@IsDefault,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[Code]=@Code");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@Name", SqlDbType.NChar,50),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Province", SqlDbType.VarChar,50),
                    new SqlParameter("@City", SqlDbType.VarChar,50),
                    new SqlParameter("@Area", SqlDbType.VarChar,100),
                    new SqlParameter("@Zip", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,100),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsDefault", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Code", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.ID;

            if (model.Name != null)
                parameters[1].Value = model.Name;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.DisID;

            if (model.Principal != null)
                parameters[3].Value = model.Principal;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[4].Value = model.Phone;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[5].Value = model.Tel;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Province != null)
                parameters[6].Value = model.Province;
            else
                parameters[6].Value = DBNull.Value;


            if (model.City != null)
                parameters[7].Value = model.City;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Area != null)
                parameters[8].Value = model.Area;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Zip != null)
                parameters[9].Value = model.Zip;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Address != null)
                parameters[10].Value = model.Address;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[11].Value = model.Remark;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.IsDefault;
            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.CreateDate;
            parameters[15].Value = model.ts;
            parameters[16].Value = model.dr;
            parameters[17].Value = model.modifyuser;
            if (model.Code != null)
                parameters[18].Value = model.Code;
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
            strSql.Append("delete [BD_DisAddr] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_DisAddr]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_DisAddr]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_DisAddr GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_DisAddr] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
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
        public IList<Hi.Model.BD_DisAddr> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_DisAddr]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_DisAddr> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_DisAddr> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_DisAddr]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_DisAddr GetModel(DataRow r)
        {
            Hi.Model.BD_DisAddr model = new Hi.Model.BD_DisAddr();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.Name = SqlHelper.GetString(r["Name"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.Principal = SqlHelper.GetString(r["Principal"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.Tel = SqlHelper.GetString(r["Tel"]);
            model.Province = SqlHelper.GetString(r["Province"]);
            model.City = SqlHelper.GetString(r["City"]);
            model.Area = SqlHelper.GetString(r["Area"]);
            model.Zip = SqlHelper.GetString(r["Zip"]);
            model.Address = SqlHelper.GetString(r["Address"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.IsDefault = SqlHelper.GetInt(r["IsDefault"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.Code = SqlHelper.GetString(r["Code"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_DisAddr> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_DisAddr>(ds.Tables[0]);
        }
    }
}
