using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Service
{
    /// <summary>
    /// 开销户接口参数
    /// </summary>
    [Serializable]
    public class OpenAccountParame
    {
        
        public string msghd_trdt { get; set; }
        public string srl_ptnsrl { get; set; }
        public string cltacc_cltno { get; set; }
        public string cltacc_subno { get; set; }
        public string cltacc_cltnm { get; set; }
        public string cltacc_pwd { get; set; }
        public string clt_nm { get; set; }
        public string clt_kd { get; set; }
        public string clt_cdtp { get; set; }
        public string clt_cdno { get; set; }
        public string clt_orgcd { get; set; }
        public string clt_bslic { get; set; }
        public string clt_gender { get; set; }
        public string clt_nationality { get; set; }
        public string clt_telno { get; set; }
        public string clt_faxno { get; set; }
        public string clt_mobno { get; set; }
        public string clt_email { get; set; }
        public string clt_postno { get; set; }
        public string clt_addr { get; set; }
        public string fcflg { get; set; }
        public string acctp { get; set; }

    }

    /// <summary>
    /// 结算账户绑定接口参数
    /// </summary>
    [Serializable]
    public class WithdrawalsParame
    {
        public string msghd_trdt { get; set; }
        public string srl_ptnsrl { get; set; }
        public string cltacc_cltno { get; set; }
        public string cltacc_cltnm { get; set; }
        public string bkacc_accno { get; set; }
        public string bkacc_accnm { get; set; }
        public string bkacc_acctp { get; set; }
        public string bkacc_crsmk { get; set; }
        public string bkacc_openbkcd { get; set; }
        public string bkacc_openbknm { get; set; }
        public string bkacc_prccd { get; set; }
        public string bkacc_citycd { get; set; }
        public string bkacc_openbkaddr { get; set; }
        public string fcflg { get; set; }

    }
}
