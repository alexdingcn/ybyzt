using System;
using System.Collections.Generic;
using System.Text;

namespace Hi.BLL
{
    public partial class BD_DisCollect
    {
        public string GetGoodsIDs(int DisID)
        {
            return dal.GetGoodsIDs(DisID);
        }

        public bool delete(int disid, int goodsid)
        {
            return dal.delete(disid, goodsid);
        }
    }
}
