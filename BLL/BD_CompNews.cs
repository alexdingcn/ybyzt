using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 BD_CompNews
    /// </summary>
    public partial class BD_CompNews
    {
        public int Add(Hi.Model.BD_CompNews model,SqlTransaction Tran)
        {
            return dal.Add(model,Tran);
        }


    }
}
