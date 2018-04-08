using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public class Common
    {
        /// <summary>
        /// datatable转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> GetListEntity<T>(DataTable table)
        {
            List<T> resultList = new List<T>();
            int  value=0;
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    T tModel = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in typeof(T).GetProperties())
                    {
                        if (table.Columns.Contains(property.Name))
                        {
                            if (DBNull.Value != dr[property.Name])
                            {
                                switch (dr[property.Name].GetType().Name)
                                {
                                    case "Int64":
                                        int.TryParse(dr[property.Name].ToString(), out value);
                                        property.SetValue(tModel, value, null);
                                        ; break;
                                    default:
                                        try
                                        {
                                            property.SetValue(tModel, dr[property.Name], null);
                                        }
                                        catch {
                                            LogHelper log = new LogHelper("D:\\log.txt", "转化出错：" + dr[property.Name].ToString());
                                            log.Write();
                                        }
                                        ; break;
                                }
                            }
                        }
                    }
                    resultList.Add(tModel);
                }
                return resultList;
            }
            return resultList;
        }

        /// <summary>
        /// datatable转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static T GetEntity<T>(DataRow row) where T : new()
        {
            T entity = default(T);
            foreach (var item in typeof(T).GetProperties())
            {
                if (row.Table.Columns.Contains(item.Name))
                {
                    if (DBNull.Value != row[item.Name])
                    {
                        if (entity == null)
                        {
                            entity = new T();
                        }
                        item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                    }

                }
            }
            return entity;
        }


        /// <summary>
        /// 公用分页方法
        /// </summary>
        /// <typeparam name="T">返回实例类型</typeparam>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="CurrentIndex">当前页码</param>
        /// <param name="PageCount">总页数（输出参数）</param>
        /// <param name="DataCount">数据总条数（输出参数）</param>
        /// <param name="Pagestart">分页开始页码数（输出参数）</param>
        /// <param name="PageEnd">分页结束页码数（输出参数</param>
        /// <param name="StrWhart">要查询的字段（默认查询所有）</param>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="OrderName">排序字段</param>
        /// <param name="TableName">要查询的表名（默认为传入的T类型名称）</param>
        /// <returns></returns>
        public static List<T> GetListByPage<T>(int PageSize, int CurrentIndex, out int PageCount, out int DataCount, out int Pagestart, out int PageEnd, string StrWhart, string SqlWhere, string OrderName, string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            CurrentIndex = CurrentIndex <= 0 ? 1 : CurrentIndex;//页码数如果小于等于0则等于1
            PageSize = PageSize <= 0 ? 10 : PageSize;//每页显示条数小于等于0则等于10
            if (string.IsNullOrEmpty(TableName))
            {
                TableName = typeof(T).Name;
            }
            string SearSqlCount = "select 1 from  " + TableName + " ";
            if (!string.IsNullOrEmpty(SqlWhere))
            {
                SearSqlCount += " where " + SqlWhere;
            }
            DataCount = SqlHelper.Query(SqlHelper.LocalSqlServer,SearSqlCount).Tables[0].Rows.Count;
            PageCount = DataCount % PageSize == 0 ? DataCount / PageSize : DataCount / PageSize + 1;
            Pagestart = CurrentIndex - 2;
            PageEnd = CurrentIndex + 2;
            if (Pagestart <= 0)
            {
                Pagestart = 1;
                if (PageCount <= 5)
                {
                    PageEnd = PageCount;
                }
                else
                {
                    PageEnd = Pagestart + 4;
                }
            }
            if (PageEnd > PageCount)
            {
                PageEnd = PageCount;
                if (PageCount <= 5)
                {
                    Pagestart = 1;
                }
                else
                {
                    Pagestart = PageEnd - 4;
                }
            }
            int startIndex = (CurrentIndex - 1) * PageSize + 1;
            int endIndex = CurrentIndex * PageSize;
            if (string.IsNullOrEmpty(StrWhart))
            {
                StrWhart = "*";
            }
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(OrderName.Trim()))
            {
                strSql.Append("order by " + OrderName);
            }
            else
            {
                strSql.Append("order by ID desc");
            }
            strSql.Append(")AS Row, " + StrWhart + "  from  " + TableName + "  ");
            if (!string.IsNullOrEmpty(SqlWhere.Trim()))
            {
                strSql.Append(" WHERE " + SqlWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);

            return GetListEntity<T>(SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0]);
        }


        /// <summary>
        /// 公用分页方法
        /// </summary>
        /// <typeparam name="T">返回实例类型</typeparam>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="CurrentIndex">当前页码</param>
        /// <param name="DataCount">数据总条数（输出参数）</param>
        /// <param name="StrWhart">要查询的字段（默认查询所有）</param>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="OrderName">排序字段</param>
        /// <param name="TableName">要查询的表名（默认为传入的T类型名称）</param>
        /// <returns></returns>
        public static List<T> GetListByAjaxPage<T>(int PageSize, int CurrentIndex, out int DataCount, string StrWhart, string SqlWhere, string OrderName, string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            CurrentIndex = CurrentIndex <= 0 ? 1 : CurrentIndex;//页码数如果小于等于0则等于1
            if (string.IsNullOrEmpty(TableName))
            {
                TableName = typeof(T).Name;
            }
            string SearSqlCount = "select 1 from  " + TableName + " ";
            if (!string.IsNullOrEmpty(SqlWhere))
            {
                SearSqlCount += " where " + SqlWhere;
            }
            DataCount = SqlHelper.Query(SqlHelper.LocalSqlServer, SearSqlCount).Tables[0].Rows.Count;
            int startIndex = (CurrentIndex - 1) * PageSize + 1;
            int endIndex = CurrentIndex * PageSize;
            if (string.IsNullOrEmpty(StrWhart))
            {
                StrWhart = "*";
            }
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(OrderName.Trim()))
            {
                strSql.Append("order by " + OrderName);
            }
            else
            {
                strSql.Append("order by ID desc");
            }
            strSql.Append(")AS Row, " + StrWhart + "  from  " + TableName + "  ");
            if (!string.IsNullOrEmpty(SqlWhere.Trim()))
            {
                strSql.Append(" WHERE " + SqlWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);

            return GetListEntity<T>(SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0]);
        }



        /// <summary>
        /// 公用分页方法
        /// </summary>
        /// <typeparam name="T">返回实例类型</typeparam>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="CurrentIndex">当前页码</param>
        /// <param name="DataCount">数据总条数（输出参数）</param>
        /// <param name="StrWhart">要查询的字段（默认查询所有）</param>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="OrderName">排序字段</param>
        /// <param name="TableName">要查询的表名（默认为传入的T类型名称）</param>
        /// <returns></returns>
        public static DataTable GetListByAjaxPage(int PageSize, int CurrentIndex, out int DataCount, string StrWhart, string SqlWhere, string OrderName, string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            CurrentIndex = CurrentIndex <= 0 ? 1 : CurrentIndex;//页码数如果小于等于0则等于1
            string SearSqlCount = "select 1 from  " + TableName + " ";
            if (!string.IsNullOrEmpty(SqlWhere))
            {
                SearSqlCount += " where " + SqlWhere;
            }
            DataCount = SqlHelper.Query(SqlHelper.LocalSqlServer, SearSqlCount).Tables[0].Rows.Count;
            int startIndex = (CurrentIndex - 1) * PageSize + 1;
            int endIndex = CurrentIndex * PageSize;
            if (string.IsNullOrEmpty(StrWhart))
            {
                StrWhart = "*";
            }
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(OrderName.Trim()))
            {
                strSql.Append("order by " + OrderName);
            }
            else
            {
                strSql.Append("order by ID desc");
            }
            strSql.Append(")AS Row, " + StrWhart + "  from  " + TableName + "  ");
            if (!string.IsNullOrEmpty(SqlWhere.Trim()))
            {
                strSql.Append(" WHERE " + SqlWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);

            //LogHelper.WriteLog(LogHelper.LogLevel.Info, strSql.ToString(), LogHelper.LogType.DataBase);

            return (SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0]);
        }

    }
}
