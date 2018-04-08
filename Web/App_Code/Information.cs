using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DBUtility;

/// <summary>
///Information 的摘要说明
/// </summary>
public class Information
{
    public Information()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public string time { get; set; }
    public string location { get; set; }
    public string context { get; set; }
    public string content { get; set; }


    public static List<GoodsAttribute> GetGoodsAttribute(int CompID, string GoodsId = "")
    {
        if (CompID != 0)
        {
            try
            {
                string SqlQury = @"select  a.ID AttrId,AttrsName AttributeName,a.GoodsID,AttrsInfoName AttrValue,b.ID ValuesId  
from BD_GoodsAttrs  a join BD_GoodsAttrsInfo b on a.ID=b.AttrsID where a.dr=0 and a.CompID='" + CompID + "' and a.goodsid in(" + GoodsId + ") ";
                DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, SqlQury).Tables[0];
                return Hi.SQLServerDAL.Common.GetListEntity<GoodsAttribute>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return null;
    }


    public static List<GoodInfoIdPrice> GetGoodsInfoPrice(string QueryWhere, int CompID, int DisID)
    {
        try
        {
            if (!string.IsNullOrEmpty(QueryWhere))
            {
                QueryWhere = " and (" + QueryWhere + ")";
            }
            string SqlQuery = @"select finalPrice,Valueinfo,
SalePrice,Bginfo.id Infoid,Bginfo.GoodsID
 from bd_goodsinfo Bginfo join   dbo.GetGoodsInfoPrice(" + DisID + "," + CompID + @",'') tt on tt.GoodsInfoID=Bginfo.ID
where Bginfo.ID in(
select id from bd_goodsinfo Bginfo where Bginfo.compid=" + CompID + "  and Bginfo.dr=0  and  Bginfo.IsOffline=1  and Bginfo.IsEnabled=1  " + QueryWhere + @"
)  order by GoodsID desc";
            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, SqlQuery).Tables[0];
            return Hi.SQLServerDAL.Common.GetListEntity<GoodInfoIdPrice>(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

//    public static List<GoodInfoIdPrice> GetGoodsInfoPriceComp(string QueryWhere, int CompID)
//    {
//        try
//        {
//            if (!string.IsNullOrEmpty(QueryWhere))
//            {
//                QueryWhere = " and (" + QueryWhere + ")";
//            }
//            string SqlQuery = @"select (case  when GoodsPrice is not null  then GoodsPrice  else Bginfo.TinkerPrice end )finalPrice,Valueinfo,
//SalePrice,Bginfo.id Infoid,Bginfo.GoodsID,Proid,Type,ProType,Discount,GoodsCount
// from bd_goodsinfo Bginfo
//left join (select ROW_NUMBER() over(PARTITION BY goodinfoid order by bp.createdate desc) rowid, GoodInfoID Infoid,bp.id  Proid,bp.Type,bp.ProType,bp.Discount,(case  when bp.ProType  in(1,2,4)then bpd.GoodsPrice else null end)
// GoodsPrice,(case  when bp.ProType=3 then bpd.GoodsPrice else null end) GoodsCount from BD_PromotionDetail bpd  
// join BD_Promotion Bp on  bp.ID=bpd.ProID and bp.IsEnabled=1 and bp.dr=0 and Bp.CompID=" + CompID + @" and '" + DateTime.Now + @"' between ProStartTime and  
// dateadd(D,1,ProEndTime) )b on b.Infoid=Bginfo.id  and b.rowid=1
//where  Bginfo.compid=" + CompID + " " + QueryWhere + " and Bginfo.dr=0 and  Bginfo.IsOffline=1  and Bginfo.IsEnabled=1  order by GoodsID desc";
//            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, SqlQuery).Tables[0];
//            return Hi.SQLServerDAL.Common.GetListEntity<GoodInfoIdPrice>(dt);
//        }
//        catch (Exception ex)
//        {
//            throw ex;
//        }
//    }

    //商品规格属性类
    public class GoodsAttribute
    {
        public string AttributeName { get; set; }
        public int AttrId { get; set; }
        public int GoodsID { get; set; }
        public string AttrValue { get; set; }
        public int ValuesId { get; set; }
    }

    //商品详情与具体价格
    public class GoodInfoIdPrice
    {
        public decimal finalPrice { get; set; }
        public string Valueinfo { get; set; }
        public decimal SalePrice { get; set; }
        public long Infoid { get; set; }
        public long GoodsID { get; set; }
        //public int Proid { get; set; }
        //public int Type { get; set; }
        //public int ProType { get; set; }
        //public decimal Discount { get; set; }
        //public decimal GoodsCount { get; set; }
    }

    [Serializable]
    public class ResultMsg<T>
    {
        public ResultMsg()
        {
            Result = false;
            Msg = "";
            Code = "";
            SumCart = 0;
            SumAmount = 0;
            AuditAmount = 0;
            Error = false;
        }
        public bool Result;
        public string Msg;
        public string Code;
        public int SumCart;
        public decimal SumAmount;
        public decimal AuditAmount;
        public List<T> ListSource;
        public bool Error;
    }

    [Serializable]
    public class ResultMsg
    {
        public ResultMsg()
        {
            Result = false;
            Msg = "";
            Code = "";
            SumCart = 0;
            SumAmount = 0;
            AuditAmount = 0;
            Error = false;
        }
        public bool Result;
        public string Msg;
        public string Code;
        public int SumCart;
        public decimal SumAmount;
        public decimal AuditAmount;
        public bool Error;
    }
    #region 物流公司匹配
    public static string TypeCom(string typeCom)
    {
        if (typeCom == "AAE全球专递")
        {
            typeCom = "aae";
        }
        if (typeCom == "安捷快递")
        {
            typeCom = "anjiekuaidi";
        }
        if (typeCom == "安信达快递")
        {
            typeCom = "anxindakuaixi";
        }
        if (typeCom == "百福东方")
        {
            typeCom = "baifudongfang";
        }
        if (typeCom == "彪记快递")
        {
            typeCom = "biaojikuaidi";
        }
        if (typeCom == "BHT")
        {
            typeCom = "bht";
        }
        if (typeCom == "BHT")
        {
            typeCom = "bht";
        }
        if (typeCom == "希伊艾斯快递")
        {
            typeCom = "cces";
        }
        if (typeCom == "中国东方")
        {
            typeCom = "Coe";
        }
        if (typeCom == "长宇物流")
        {
            typeCom = "changyuwuliu";
        }
        if (typeCom == "大田物流")
        {
            typeCom = "datianwuliu";
        }
        if (typeCom == "德邦物流")
        {
            typeCom = "debangwuliu";
        }
        if (typeCom == "DPEX")
        {
            typeCom = "dpex";
        }
        if (typeCom == "DHL")
        {
            typeCom = "dhl";
        }
        if (typeCom == "D速快递")
        {
            typeCom = "dsukuaidi";
        }
        if (typeCom == "fedex")
        {
            typeCom = "fedex";
        }
        if (typeCom == "飞康达物流")
        {
            typeCom = "feikangda";
        }
        if (typeCom == "凤凰快递")
        {
            typeCom = "fenghuangkuaidi";
        }
        if (typeCom == "港中能达物流")
        {
            typeCom = "ganzhongnengda";
        }
        if (typeCom == "广东邮政物流")
        {
            typeCom = "guangdongyouzhengwuliu";
        }
        if (typeCom == "汇通快运")
        {
            typeCom = "huitongkuaidi";
        }
        if (typeCom == "恒路物流")
        {
            typeCom = "hengluwuliu";
        }
        if (typeCom == "华夏龙物流")
        {
            typeCom = "huaxialongwuliu";
        }
        if (typeCom == "佳怡物流")
        {
            typeCom = "jiayiwuliu";
        }
        if (typeCom == "京广速递")
        {
            typeCom = "jinguangsudikuaijian";
        }
        if (typeCom == "急先达")
        {
            typeCom = "jixianda";
        }
        if (typeCom == "佳吉物流")
        {
            typeCom = "jiajiwuliu";
        }
        if (typeCom == "加运美")
        {
            typeCom = "jiayunmeiwuliu";
        }
        if (typeCom == "快捷速递")
        {
            typeCom = "kuaijiesudi";
        }
        if (typeCom == "联昊通物流")
        {
            typeCom = "lianhaowuliu";
        }
        if (typeCom == "龙邦物流")
        {
            typeCom = "longbanwuliu";
        }
        if (typeCom == "民航快递")
        {
            typeCom = "minghangkuaidi";
        }
        if (typeCom == "配思货运")
        {
            typeCom = "peisihuoyunkuaidi";
        }
        if (typeCom == "全晨快递")
        {
            typeCom = "quanchenkuaidi";
        }
        if (typeCom == "全际通物流")
        {
            typeCom = "quanjitong";
        }
        if (typeCom == "全日通快递")
        {
            typeCom = "quanritongkuaidi";
        }
        if (typeCom == "全一快递")
        {
            typeCom = "quanyikuaidi";
        }
        if (typeCom == "盛辉物流")
        {
            typeCom = "shenghuiwuliu";
        }
        if (typeCom == "速尔物流")
        {
            typeCom = "suer";
        }
        if (typeCom == "盛丰物流")
        {
            typeCom = "shengfengwuliu";
        }
        if (typeCom == "天地华宇")
        {
            typeCom = "tiandihuayu";
        }
        if (typeCom == "天天快递")
        {
            typeCom = "tiantian";
        }
        if (typeCom == "TNT")
        {
            typeCom = "tnt";
        }
        if (typeCom == "UPS")
        {
            typeCom = "ups";
        }
        if (typeCom == "万家物流")
        {
            typeCom = "wanjiawuliu";
        }
        if (typeCom == "文捷航空速递")
        {
            typeCom = "wenjiesudi";
        }
        if (typeCom == "伍圆速递")
        {
            typeCom = "wuyuansudi";
        }
        if (typeCom == "万象物流")
        {
            typeCom = "wanxiangwuliu";
        }
        if (typeCom == "新邦物流")
        {
            typeCom = "xinbangwuliu";
        }
        if (typeCom == "信丰物流")
        {
            typeCom = "xinfengwuliu";
        }
        if (typeCom == "星晨急便")
        {
            typeCom = "xingchengjibian";
        }
        if (typeCom == "鑫飞鸿物流")
        {
            typeCom = "xinhongyukuaidi";
        }
        if (typeCom == "亚风速递")
        {
            typeCom = "yafengsudi";
        }
        if (typeCom == "一邦速递")
        {
            typeCom = "yibangwuliu";
        }
        if (typeCom == "优速物流")
        {
            typeCom = "youshuwuliu";
        }
        if (typeCom == "远成物流")
        {
            typeCom = "yuanchengwuliu";
        }
        if (typeCom == "圆通速递")
        {
            typeCom = "yuantong";
        }
        if (typeCom == "源伟丰快递")
        {
            typeCom = "yuanweifeng";
        }
        if (typeCom == "元智捷诚快递")
        {
            typeCom = "yuanzhijiecheng";
        }
        if (typeCom == "越丰物流")
        {
            typeCom = "yuefengwuliu";
        }
        if (typeCom == "韵达快递")
        {
            typeCom = "yunda";
        }
        if (typeCom == "源安达")
        {
            typeCom = "yuananda";
        }
        if (typeCom == "运通快递")
        {
            typeCom = "yuntongkuaidi";
        }
        if (typeCom == "宅急送")
        {
            typeCom = "zhaijisong";
        }
        if (typeCom == "中铁快运")
        {
            typeCom = "zhongtiewuliu";
        }
        if (typeCom == "中通速递")
        {
            typeCom = "zhongtong";
        }
        if (typeCom == "中通快递")
        {
            typeCom = "zhongtong";
        }
        if (typeCom == "中邮物流")
        {
            typeCom = "zhongyouwuliu";
        }
        if (typeCom == "顺丰速递" || typeCom == "顺丰快递")
        {
            typeCom = "shunfeng";
        }
        if (typeCom == "申通快递")
        {
            typeCom = "shentong";
        }
        if (typeCom == "EMS快递")
        {
            typeCom = "ems";
        }
        return typeCom;
    }
    #endregion
}

[Serializable]
public class AccountCheck
{
    private string url;
    /// <summary>
    /// 登录来源
    /// </summary>
    public string Url
    {
        get;
        set;
    }
    /// <summary>
    /// 用户帐号是否已验证   
    /// </summary>
    public bool IsAccountCheck
    {
        get;
        set;
    }
    /// <summary>
    /// 用户主ID
    /// </summary>
    public string UsersID
    {
        get;
        set;
    }


    /// <summary>
    /// 用户手机号码
    /// </summary>
    public string Phone
    {
        get;
        set;
    }

    /// <summary>
    /// 用户帐号列表
    /// </summary>
    public List<Hi.Model.SYS_Users> ListUser
    {
        get;
        set;
    }

    /// <summary>
    /// 用户是否通过手机登陆
    /// </summary>
    public bool IsPhoneLogin
    {
        get;
        set;
    }/// <summary>
    /// 是否窗口登陆选择角色
    /// </summary>
    public bool IsWindowLogin
    {
        get;
        set;
    }

    public Hi.Model.SYS_PhoneCode PhoneModel
    {
        get;
        set;
    }

}