using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Service
{
    public class PAY_FastPayMent
    {
        public PAY_FastPayMent() { }

        public List<FastPayMent> GetFastPayMent(List<Hi.Model.PAY_FastPayMent> fastList)
        {
            List<FastPayMent> FastPayMentList = new List<FastPayMent>();
            foreach (Hi.Model.PAY_FastPayMent fastM in fastList)
            {
                FastPayMent FastPayMent = new FastPayMent();
                FastPayMent.FastPaymentBankID = fastM.ID;
                FastPayMent.BankName = fastM.bankName;
                FastPayMent.BankCode = "**" + fastM.bankcode.Substring(fastM.bankcode.Length - 4, 4);
                FastPayMent.Telephone = fastM.phone.Substring(0, 3) + "****" + fastM.phone.Substring(fastM.phone.Length - 4, 4);
                FastPayMent.BankID = fastM.BankID;
                FastPayMentList.Add(FastPayMent);
            }
            return FastPayMentList;
        }

        #region 返回实体
        public class FastPayMent
        {
            public int FastPaymentBankID { get; set; }
            public string BankName { get; set; }
            public string BankCode { get; set; }
            public string Telephone { get; set; }
            public int BankID { get; set; }
        }
        #endregion
    }
}
