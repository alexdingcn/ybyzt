using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Hi.BLL
{
    public partial class SYS_RoleUser
    {
        public int Add(Hi.Model.SYS_RoleUser model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
    }
}
