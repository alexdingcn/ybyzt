
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

/// <summary>
///Enums 的摘要说明
/// </summary>
public class Enums
{
    /// <summary>
    /// 下单类型
    /// </summary>
    public enum AddType
    {
        网页下单 = 1,
        企业补单 = 2,
        App下单 = 3,
        App企业补单 = 4
    }

    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OType
    {
        销售订单 = 0,
        赊销订单 = 1,
        特价订单 = 2,  //（兼容赠品订单，单价为0）) 
        推送账单 = 9
    }

    /// <summary>
    /// 销售订单状态
    /// </summary>
    public enum OrderState
    {
        退回 = -1,
        未提交 = 0,
        待审核 = 1,
        已审 = 2,
        已发货 = 4,
        已到货 = 5,
        退货处理 = 3,
        已退货 = 7,
        已作废 = 6
    }

    /// <summary>
    /// 订单发货状态
    /// </summary>
    public enum IsOutState
    {
        部分发货 = 1,
        部分到货 = 2,
        全部发货 = 3,
        全部到货 = 4
    }

    /// <summary>
    /// 支付状态
    /// </summary>
    public enum PayState
    {
        未支付 = 0,
        部分支付 = 1,
        已支付 = 2,
        申请退款 = 4,
        已退款 = 5,
        已结算=6,
        支付处理中=7
    }

    /// <summary>
    /// 订单是否退货、退款
    /// </summary>
    public enum ReturnState
    {
        拒绝退货 = -1,
        未退货 = 0,
        新增退货=1,
        申请退货 = 2,
        退货退款 = 3,
        申请退款 = 4,
    }

    /// <summary>
    /// 退货审核状态
    /// </summary>
    public enum AuditState
    {
        退回 = -1,   //已拒绝
        未提交 = 0,  
        提交 = 1,   //待审核
        已审 = 2,   //已退货
        已签收 = 3, 
        已完结 = 4   //已退货款
    }

    /// <summary>
    /// 企业钱包表审核状态
    /// </summary>
    public enum PrePayState
    {
        [Description("未审")]
        未审 = 0,
        [Description("已提交")]
        已提交 = 1,
        [Description("已审")]
        已审 = 2
    }

    /// <summary>
    /// 企业钱包支付状态
    /// </summary>
    public enum PrePayMentState
    {
        [Description("成功")]
        成功 = 1,
        [Description("失败")]
        失败 = 2,
        [Description("处理中")]
        处理中 = 3,
        [Description("已结算")]
        已结算 = 4
    }
    /// <summary>
    /// 企业钱包款项类型
    /// </summary>
    public enum PrePayType
    {
        [Description("充值")]
        充值 = 1,
        [Description("企业钱包补录")]
        企业钱包补录 = 2,
        [Description("企业钱包冲正")]
        企业钱包冲正 = 3,
        [Description("退款")]
        退款 = 4,
        [Description("订单付款")]
        订单付款 = 5,
        [Description("转账汇款")]
        转账汇款 = 6,
        [Description("订单收款补录")]
        订单收款补录 = 7,
        [Description("账单收款补录")]
        账单收款补录 = 8,
        [Description("利息收益")]
        利息收益 = 9
    }

    /// <summary>
    /// 证件类型（在线融资）
    /// </summary>
    public enum CertificatesNature
    {
        [Description("身份证")]
        身份证 = 1,
        [Description("户口簿")]
        户口簿 = 2,
        [Description("军官证")]
        军官证 = 3,
        [Description("警官证")]
        警官证 = 4,
        [Description("护照")]
        护照 = 5,
        [Description("港澳通行证")]
        港澳通行证 = 6,
        [Description("文职干部证")]
        文职干部证 = 7,
        [Description("士兵证")]
        士兵证 = 8,
        [Description("台湾通行证")]
        台湾通行证 = 9,
        [Description("其他")]
        其他 = 10
    }



    /// <summary>
    /// 企业业务员类型
    /// </summary>
    public enum DisSMType
    {
        业务员 = 1,
        业务经理 = 2
    }

    /// <summary>
    /// 经销商性质
    /// </summary>
    public enum AccountNature
    {
        个人 = 0,
        公司 = 1,
        企业=2
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        男 = 0,
        女 = 1
    }

    /// <summary>
    /// 开销户状态
    /// </summary>
    public enum Pay_OpenState {
        成功 = 1,
        失败 = 2
    }

    /// <summary>
    /// 开销户状态
    /// </summary>
    public enum Pay_AccTp
    {
        公司 = 1,
        个人 = 2
    }

    /// <summary>
    /// 开销户状态
    /// </summary>
    public enum Pay_CrsMk
    {
        本行 = 1,
        跨行 = 2
    }

    /// <summary>
    /// 融资交易明细类型
    /// </summary>
    public enum FinacingType
    { 
        入金=1,
        出金=2,
        账户余额支付=3,
        借款申请=4
    }

    /// <summary>
    /// 积分类型
    /// </summary>
    public enum IntegralType
    {
        订单积分 = 1,
        账单积分 = 2
    }

    /// <summary>
    /// 企业来源
    /// </summary>
    public enum Erptype
    {
        平台企业 = 0,
        U9 = 1,
        U8 = 2,
        NC = 3
    }

}