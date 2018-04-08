using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data;

namespace Hi.SQLServerDAL
{
    public partial class SYS_NewsNotice
    {

        public List<Hi.Model.SYS_NewsNotice> QueryGroupNew(string strWhart)
        {
            if (string.IsNullOrEmpty(strWhart))
            {
                strWhart = "*";
            }
            string SqlQuery = @"select " + strWhart + @", row_number() over(order by CreateDate desc ) rowsid from SYS_NewsNotice where dr='0' and NewsType=1 and IsTop='1' and IsEnabled='1'   
union all
select " + strWhart + @", row_number() over(order by CreateDate desc ) rowsid from SYS_NewsNotice where dr='0' and NewsType=3 and IsTop='1' and IsEnabled='1'    
union all
select " + strWhart + @", row_number() over(order by CreateDate desc ) rowsid from SYS_NewsNotice where dr='0' and NewsType=4 and IsTop='1' and IsEnabled='1'   

";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, SqlQuery.ToString());
            return Common.GetListEntity<Hi.Model.SYS_NewsNotice>(ds.Tables[0]);
        }

        public List<Hi.Model.SYS_NewsNotice> QueryGroupNewList(string strWhart)
        {
            if (string.IsNullOrEmpty(strWhart))
            {
                strWhart = "*";
            }
            string SqlQuery = @"select " + strWhart + @", row_number() over(order by CreateDate desc ) rowsid from SYS_NewsNotice where dr='0' and (NewsType=1 or NewsType=3) and IsTop='1' and IsEnabled='1'   
union all
select " + strWhart + @", row_number() over(order by CreateDate desc ) rowsid from SYS_NewsNotice where dr='0' and NewsType=2 and IsTop='1' and IsEnabled='1' 
";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, SqlQuery.ToString());
            return Common.GetListEntity<Hi.Model.SYS_NewsNotice>(ds.Tables[0]);
        }

    }
}
