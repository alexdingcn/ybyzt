using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Service
{
    [Serializable]
    public class FinancingOrder
    {
        public string msghd_trdt { get; set; }
        public string srl_ptnsrl { get; set; }
        public string cltacc_cltno { get; set; }
        public string cltacc_cltnm { get; set; }
        public string loan_loancd { get; set; }
        public string loan_totalamt { get; set; }
        public string loan_loanamt { get; set; }
        public string loan_selfamt { get; set; }
        public string useage { get; set; }
        public List<BillInfo> billInfoList { set; get; }
        public List<Ereceipt> ereceiptList { set; get; }

    }

    [Serializable]
    public class BillInfo
    {
        public string pcustno { get; set; }
        public string pcustnm { get; set; }
        public string pecustno { get; set; }
        public string pecustnm { get; set; }
        public string billtype { get; set; }
        public string orderno { get; set; }
        public string totalamt { get; set; }
        public string paidamt { get; set; }
        public string unpaidamt { get; set; }
        public string gdsnm { get; set; }
        public string gdsdic { get; set; }
        public string dly { get; set; }
        public string sgndt { get; set; }
        public string duedate { get; set; }
    }
    [Serializable]
    public class Ereceipt
    {
        public string rtbill { get; set; }
        public string kd { get; set; }
        public string nm { get; set; }
        public string std { get; set; }
        public string grd { get; set; }
        public string unit { get; set; }
        public string num { get; set; }
        public string price { get; set; }
        public string value { get; set; }
        public string brd { get; set; }
        public string chkbill { get; set; }
        public string duedate { get; set; }
        public string gdsdic { get; set; }
        public string hder { get; set; }
        public string gds { get; set; }
        public string whnm { get; set; }
        public string whno { get; set; }
        public string batchno { get; set; }
        public string sgndt { get; set; }
        public string mfters { get; set; }
    }
}
