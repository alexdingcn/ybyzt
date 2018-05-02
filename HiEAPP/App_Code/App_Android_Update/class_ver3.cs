using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///class_ver3 的摘要说明
/// </summary>
public class class_ver3
{
	public class_ver3()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    #region//商品
    /// <summary>
    /// 产品的实体
    /// </summary>
    public class Product
    {
        public String ProductID { get; set; }
        public String ProductName { get; set; }
        public String SalePrice { get; set; }
        public String IsCollect { get; set; }
        public String IsPro { get; set; }
        public String IsSale { get; set; }
        public String Title { get; set; }
        public String Details { get; set; }
        public String Unit { get; set; }
        public String InStock { get; set; }
        public List<Pic> ProductPicUrlList { get; set; }//图片list
        public List<ProductAttribute> ProductAttributeList { get; set; }//产品属性list
        public List<SKU> SKUList { get; set; }//该产品所有的SKU list
        public String IsShop { get; set; }//是否店铺显示（0：表示不显示，1：表示显示）
        public String IsRecommend { get; set; }//是否店铺推荐（0：表示不是，1：表示是）
        public String SortIndex { get; set; }//排序号
        public String GoodsSpan { get; set; }//商品标签
    }
    /// <summary>
    /// 产品图片
    /// </summary>
    public class Pic
    {
        public String ProductID { get; set; }
        public String IsDeafult { get; set; }
        public String PicUrl { get; set; }
    }
    /// <summary>
    /// 产品属性
    /// </summary>
    public class ProductAttribute
    {
        public String ProductID { get; set; }
        public String ProductAttributeID { get; set; }//产品属性ID
        public String ProductAttributeName { get; set; }//产品属性名称
        public List<ProductAttValue> ProductAttValueList { get; set; }//产品属性值LIST
    }
    /// <summary>
    /// 产品属性值
    /// </summary>
    public class ProductAttValue
    {
        public String ProductID { get; set; }
        public String ProductAttributeID { get; set; }//产品属性ID
        public String ProductAttValueID { get; set; }//产品属性值ID
        public String ProductAttValueName { get; set; }//产品属性值名称
    }
    /// <summary>
    /// SKU实体
    /// </summary>
    public class SKU
    {
        public String ProductID { get; set; }
        public String SKUID { get; set; }
        public String IsPro { get; set; }
        public String SKUName { get; set; }
        public String BarCode { get; set; }
        public String ValueInfo { get; set; }
        public String SalePrice { get; set; }//基础价格
        public String TinkerPrice { get; set; }//调价后价格
        public String InStock { get; set; }
        public List<ProductAttValueID> ProductAttValueIDList { get; set; }//产品属性Value ID List
        public PromotionInfo ProInfo { get; set; }//促销信息
        public String Inventory { get; set; }
        public String ProductCode { get; set; }//商品编码
        public String ProductName { get; set; }//商品名称
    }
    /// <summary>
    /// 促销详情
    /// </summary>
    public class PromotionInfo
    {
        public String ProID { get; set; }
        public String ProTitle { get; set; }
        public String ProInfos { get; set; }
        public String Type { get; set; }//促销类型  0、特价促销 1、商品促销
        public String ProType { get; set; }//促销方式  特价促销（1、赠品  2、优惠 ）  商品促销（3、满送  4、打折）
        public String Discount { get; set; }
        public String ProStartTime { get; set; }
        public String ProEndTime { get; set; }
    }
    /// <summary>
    /// 产品属性值ID
    /// </summary>
    public class ProductAttValueID
    {
        public String ProductAttributeValueID { get; set; }
    }
    #endregion

    #region//订单
    /// <summary>
    /// 订单实体
    /// </summary>
    public class Order
    {
        public String OrderID { get; set; }
        public String CompID { get; set; }
        public String CompName { get; set; }
        public String DisID { get; set; }
        public String DisName { get; set; }
        public String Otype { get; set; }//订单类型（0：订单 9：账单)
        public String AddType { get; set; }//提交类型（3：经销商下单 4：核心企业下单）
        public String OtherAmount { get; set; }//其他费用
        public String DisUserName { get; set; }//制单人
        public String Ostate { get; set; }//订单状态(1：已提交  2：已审核  3：退货处理  4：已发货  5：已到货（完成）  6：已作废)
        public String PayState { get; set; }//支付状态：0：未支付  1：部分支付  2：已支付  4、申请退款 5、已退款 
        public String AuditUserName { get; set; }
        public String DisUserID { get; set; }//下单用户ID
        public String AddrID { get; set; }//收货地址ID
        public String ReceiptNo { get; set; }
        public String ArriveDate { get; set; }//要求发货日期
        public String Address { get; set; }//详细地址
        public String Zip { get; set; }//邮编
        public String Contact { get; set; }//收货人
        public String Phone { get; set; }//收货人手机
        public String TotalAmount { get; set; }//总价
        public String AuditTotalAmount { get; set; }//审核后总价
        public String PayedAmount { get; set; }//已付金额
        public String CreateUserID { get; set; }
        public String CreateDate { get; set; }
        public String ReturnMoneyDate { get; set; }//申请退款时间
        public String ReturnMoneyUserId { get; set; }//申请退款人Id
        public String ReturnMoneyUser { get; set; }//申请退款人
        public String OrderRemark { get; set; }
        public String IsAudit { get; set; }
        public String AuditUserID { get; set; }
        public String AuditDate { get; set; }
        public String AuditRemark { get; set; }
        public String IsDel { get; set; }
        public List<OrderDetail> OrderDetailList { get; set; }//订单明细List
        public List<Operating> LogList { get; set; }//操作日志list
        public OrderExt Invoice { get; set; }//发票实体
        public List<Pay> PayLogList { get; set; }//支付日志list
        public String Rebate { get; set; }
        public OrderPro ProInfo { get; set; }//订单促销信息
        public String IsOrderPro { get; set; }//是否整单优惠0不是1是
        public String IsOutState { get; set; }//发货状态1 部分发货，2 部分收货，3 全部发货，4 全部收货
        public String GiveMode { get; set; } //配送方式(送货/自提)
        public String PostFee { get; set; }
        public List<OrderOut> OrderOutList { get; set; }//发货单列表
        public List<Att> AttaList { get; set; }//附件列表
        public List<UnSendoutDetail> UnSendoutDetailList { get; set; }//代发货列表
        public String UnSendoutNum { get; set; }//未发货商品类别数量
        public String ReturnState { get; set; }//退货状态
        public String ts { get;set;}
        //public Att AttList { get; set; }//附件
    }
    /// <summary>
    /// 附件
    /// </summary>
    public class Att
    {
        public String OrderId { get; set; }
        public String AttName { get; set; }//附件名
        public String AttUrl { get; set; }//附件地址
        public String AttSize { get; set; }//附件大小
    }
        /// <summary>
        /// 待发货商品的实体
        /// </summary>
        public class UnSendoutDetail
        {
            public String SKUID { get; set; }
            public String OrderID { get; set; }
            public String UnOutNum { get; set; }//待发货数量
            public String AllNum { get; set; }
            public String OutNum { get; set; }
            public OrderOutDetailInfo OrderOutDetailInfo { get; set; }//发货单明细对应的订单明细
        }
        /// <summary>
        /// 发货单实体
        /// </summary>
        public class OrderOut
        {
            public String OrderOutID { get; set; }
            public String OrderID { get; set; }
            public String SendDate { get; set; }
            public String ActionUser { get; set; }//经办人
            public String Remark { get; set; }//备注
            public String IsAudit { get; set; }//2：已审  1：提交  0：未审  3：作废
            public String CreateUserID { get; set; }
            public String CreateDate { get; set; }
            public String SignDate { get; set; }//签收时间
            public String IsSign { get; set; }//签收确认 (0：未签收 1：已签收)
            public String SignUserId { get; set; }//签收操作人Id
            public String SignUser { get; set; }//签收操作人
            public String SignRemark { get; set; }//签收备注
            public List<SendOutDetail> SendOutDetailList { get; set; }//订单发货明细
            public Wuliu Logistics { get; set; }
            public String ReceiptNo { get; set; }
            public String OrderOutNo { get; set; }//发货单编号
            public String ts { get; set; }
        }
        /// <summary>
        /// 物流信息实体
        /// </summary>
        public class Wuliu
        {
            public String OrderID { get; set; }
            public String OrderOutID { get; set; }
            public String ComPName { get; set; }//物流公司
            public String LogisticsNo { get; set; }//物流单号
            public String CarUser { get; set; }//司机姓名
            public String CarNo { get; set; }//司机手机
            public String Car { get; set; }//车牌号
            public String Context { get; set; }//物流信息 (json格式)
            public String Type { get; set; }//1 物流公司 2 其他物流
            public String GoodsNum { get; set; }//商品类别数
        }
        /// <summary>
        /// 发货单子表实体
        /// </summary>
        public class SendOutDetail
        {
            public String OrderOutID { get; set; }
            public String SKUID { get; set; }
            public String OrderID { get; set; }
            public String OutNum { get; set; }//发货数量
            public String SignNum { get; set; }//签收数量
            public String AllNum { get; set; }//订购数量
            public String ProInfo { get; set; }//满赠详情（满足满赠条件才会有值）
            public String Remark { get; set; }
            public OrderOutDetailInfo OrderOutDetailInfo { get; set; }//发货单明细对应的订单明细
        }
        /// <summary>
        /// 发货单拓展表实体
        /// </summary>
        public class OrderExt
        {
            public String OrderID { get; set; }
            public String Rise { get; set; }//抬头
            public String Content { get; set; }//发票内容
            public String OBank { get; set; }//开户银行
            public String OAccount { get; set; }//开户账户
            public String TRNumber { get; set; }//纳税人登记号
            public String IsOBill { get; set; }//是否开发票 0、不开发票  1、开普通发票  2、开增值税发票
            public String BillNo { get; set; }//发票号
            public String IsBill { get; set; }//发票是否已开完 0、未完成  1、已完成
        }
        /// <summary>
        /// 促销信息实体
        /// </summary>
        public class OrderPro
        {
            public String ProID { get; set; }
            public String ProType { get; set; }//促销类型 
            public String OrderPrice { get; set; }//订单满减的金额
            public String Discount { get; set; }//订单促销描述
        }
        /// <summary>
        /// 操作日志实体
        /// </summary>
        public class Operating
        {
            public String LogType { get; set; }//操作说明
            public String LogTime { get; set; }
            public String OperatePerson { get; set; }//操作人
            public String LogRemark { get; set; }
        }
        /// <summary>
        /// 支付日志实体
        /// </summary>
        public class Pay
        {
            public String CompName { get; set; }
            public String ResellerName { get; set; }
            public String PayLogType { get; set; }//类型
            public String PayAmount { get; set; }//支付金额
            public String PayDate { get; set; }
            public String FeeAmount { get; set; }//手续费
            public String Guid { get; set; }
            public String vedf9 { get; set; }
            public String PreType { get; set; }
        }
        /// <summary>
        /// 物流信息实体
        /// </summary>
        public class OrderDetail
        {
            public String ProductID { get; set; }
            public String OrderDetailID { get; set; }
            public String SKUID { get; set; }
            public String IsPro { get; set; }
            public String ProductName { get; set; }
            public String SKUName { get; set; }
            public String ValueInfo { get; set; }//描述
            public String SalePrice { get; set; }//基础价格
            public String TinkerPrice { get; set; }//调整后的价格
            public String Num { get; set; }//购买数量
            public String Unit { get; set; }//计量单位
            public String Remark { get; set; }//备注，-1代表商品不可用 信息异常、被删除、或下架
            public List<Pic> ProductPicUrlList { get; set; }
            public String ProNum { get; set; }//如果是满送，则存赠送数量
            public PromotionInfo proInfo { get; set; }//商品促销信息
            public String Inventory { get; set; }//商品库存
            public String BarCode { get; set; }
            public String NumEnable { get; set; }
            public String NewPrice { get; set; }
        }

    /// <summary>
    /// 发货单明细中对应的订单明细实体
    /// </summary>
        public class OrderOutDetailInfo
        {
            public String ProductID { get; set; }
            public String SKUID { get; set; }
            public String ProductName { get; set; }
            public String SKUName { get; set; }
            public String ValueInfo { get; set; }//描述
            public String TinkerPrice { get; set; }//调整后的价格
            public String Unit { get; set; }//计量单位
            public List<Pic> ProductPicUrlList { get; set; }
            public String ProductCode { get; set; }//商品编码

        }
        /// <summary>
        /// 促销详细信息实体
        /// </summary>
        //public class PromotionInfo
        //{
        //    public String ProID { get; set; }
        //    public String ProTitle { get; set; }
        //    public String ProInfos { get; set; }//促销描述
        //    public String Tpye { get; set; }//促销类型  0、特价促销 1、商品促销
        //    public String ProTpye { get; set; }//促销方式  特价促销（1、赠品  2、优惠 ）  商品促销（3、满送  4、打折）
        //    public String Discount { get; set; }
        //    public String ProStartTime { get; set; }
        //    public String ProEndTime { get; set; }
        //}
    /// <summary>
    /// 促销详细信息实体
    /// </summary>
        public class BillInfo
        {
            public String DisID { get; set; }
            public String Rise { get; set; }//发票抬头
            public String Content { get; set; }//发票内容
            public String OBank { get; set; }//开户银行
            public String OAccount { get; set; }//开户账号
            public String TRNumber { get; set; }//纳税人登记号
        }

    //列表中使用的商品实体
        public class ProductSimple
        {
            public String ProductID { get; set; }
            public String ProductCode { get; set; }//商品编码
            public String ProductName { get; set; }
            public String SalePrice { get; set; }
            public String IsCollect { get; set; }
            public String IsPro { get; set; }
            public String IsSale { get; set; }
            public String Title { get; set; }//标题
            public String Details { get; set; }//详细
            public String Unit { get; set; }
            public String InStock { get; set; }
            public String SaleNum { get; set; }//销量
            public List<Pic> ProductPicUrlList { get; set; }
        }
    #endregion

    #region//经销商
    //经销商分类
        public class ResellerClassify
        {
            public String ClassifyID { get; set; }
            public String ClassifyName { get; set; }
            public String ClassifyCode { get; set; }
            public String ParentID { get; set; }
            public String SortIndex { get; set; }
            public String Remark { get; set; }
        }
    //简单经销商（list中用）
        public class ResellerSimple
        {
            public String ResellerID { get; set; }
            public String ResellerName { get; set; }
            public String ResellerAddr { get; set; }
            public String Principal { get; set; }
            public String Phone { get; set; }
        }
    //经销商
        public class Reseller
        {
            public String ResellerID { get; set; }//经销商ID
            public String ResellerName { get; set; }//经销商名称
            public String ResellerAddr { get; set; }//经销商地址
            public String ResellerProvince { get; set; }//经销商省份
            public String ResellerCity { get; set; }//经销商市
            public String ResellerArea { get; set; }//经销商区或县
            public String Address { get; set; }//经销商详细地址
            public String ResellerCode { get; set; }//经销商编码
            public String ResellerClassify { get; set; }//经销商分类名称
            public String ResellerClassifyID { get; set; }//经销商分类ID
            public String Zip { get; set; }//邮编
            public String Tel { get; set; }//电话
            public String Fax { get; set; }//传真
            public String Principal { get; set; }//联系人
            public String Phone { get; set; }//联系人手机
            public String AreaName { get; set; }//经销商区域名称
            public String AreaID { get; set; }//经销商区域ID
            public String IsEnabled { get; set; }//0：停用，1：启用
            public String ts { get; set; }
            public List<Invoce> InvoceList { get; set; }//开票信息list
            public Account Account { get; set; }//登录账号List
            public List<FCMaterial> FCMaterialList { get; set; }    // 首营材料
        }

    //首营材料
    public class FCMaterial
    {
        public String category { get; set; }
        public String validDate { get; set; }
        public String fileUrl { get; set; }
        public int dateDiff { get; set; }
        public int DisID { get; set; }
    }

    //开票信息
        public class Invoce
        {
            public String InvoceID { get; set; }//开票信息ID
            public String InvoceType { get; set; }//开票信息类型
            public String Rise { get; set; }//抬头
            public String Content { get; set; }//发票内容
            public String OBank { get; set; }//开户银行
            public String OAccount { get; set; }//开户账号
            public String TRNumber { get; set; }//纳税人登记号
        }

    //登录账号
        public class Account
        {
            public String AccountID { get; set; }//登录账号ID
            public String UserName { get; set; }//登录账号
            public String TrueName { get; set; }//姓名
            public String Phone { get; set; }//手机号
            public String ts { get; set; }
        }
    //经销商区域实体
        public class Area
        {
            public String AreaID { get; set; }//区域ID
            public String AreaName { get; set; }//区域名称
            public String AreaCode { get; set; }//区域编码
            public String ParentID { get; set; }//父分类ID（无父分类为0）
            public String SortIndex { get; set; }//排序号
        }
    #endregion

    #region 核心企业
        public class CompanyInfo
        {
            public string Result { get; set; }
            public string Description { get; set; }
            public String CompName { get; set; }//核心企业名称
            public String SPhone { get; set; }//核心企业手机号
            public CompAccount CompAccount { get; set; }//登录账号信息
            public CompInfo CompInfo { get; set; }//核心企业信息
            public List<PayAccount> PayAccountList { get; set; }//收款账号信息
            public SysSettings SysSettings { get; set; }//系统设置
        }
    //核心企业登录账号信息
        public class CompAccount
        {
            public String UserName { get; set; }//登录账号
            public String TrueName { get; set; }//姓名
            public String Phone { get; set; }//手机号码
            public String Email { get; set; }//邮箱
            public String Ts { get; set; }//时间戳
        }
    //核心企业信息
        public class CompInfo
        {
            public String CompName { get; set; }
            public String InduName { get; set; }//行业类别名称
            public String Address { get; set; }//详细地址
            public String Zip { get; set; }//邮编
            public String Tel { get; set; }//电话
            public String Fax { get; set; }//传真
            public String Principal { get; set; }//联系人
            public String Phone { get; set; }//手机
            public String ManageInfo { get; set; }//经营范围
            public String Ts { get; set; }//时间戳
        }

    //核心企业收款账号
        public class PayAccount
        {
            public String IsEnable { get; set; }//是否启用（0表示不启用，1表示启用）
            public String AccountName { get; set; }//账户名称
            public String BankName { get; set; }//开户银行名称
            public String AccountCode { get; set; }//账户号码
            public String BankAddress { get; set; }//银行地址
            public String AccountType { get; set; }//账户类型（个人账户，企业账户）
            public String BankPrivate { get; set; }//开户行省份
            public String BankCity { get; set; }//开户行城市
            public String CateType { get; set; }//证件类型
            public String CateCode { get; set; }//证件号码
            public String PayAccountID { get; set; }//收款账号ID
            public String Ts { get; set; }
        }

        public class SysSettings
        {
            public String IsInv { get; set; }//是否启用商品库存（0表示不启用，1表示启用）
            public String IsReseller { get; set; }//是否启用经销商加盟审核（0表示不启用，1表示启用）
            public String IsRebate { get; set; }//是否启用返利（0表示不启用，1表示启用）
            public String PayWay { get; set; }//支付方式（0表示普通支付，1表示担保支付）
        }
    #endregion

}