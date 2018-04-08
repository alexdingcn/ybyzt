using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hi.BLL
{
    public partial class SYS_NewsNotice
    {
        public List<Hi.Model.SYS_NewsNotice> QueryGroupNew(string strWhart)
        {
            return dal.QueryGroupNew(strWhart);
        }

        public List<Hi.Model.SYS_NewsNotice> QueryGroupNewList(string strWhart)
        {
            return dal.QueryGroupNewList(strWhart);
        }
    }
}
