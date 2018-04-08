using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Service
{
    [Serializable]
    public class OrderPay
    {
        public string msghd_trdt { get; set; }
        public string srl_ptnsrl { get; set; }
        public string billinfo_pcltno { get; set; }
        public string billinfo_pnm { get; set; }
        public string billinfo_rcltno { get; set; }
        public string billinfo_rcltnm { get; set; }
        public string billinfo_orderno { get; set; }
        public string billinfo_billno { get; set; }
        public string billinfo_aclamt { get; set; }
        public string billinfo_payfee { get; set; }
        public string billinfo_payeefee { get; set; }
        public string billinfo_usage { get; set; }
        public string trsflag { get; set; }
        public string suminfo_sum1 { get; set; }
        public string suminfo_sum2 { get; set; }
    }
}
