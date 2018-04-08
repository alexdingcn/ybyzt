using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Com.Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.3
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段
        private static string partner = "";
        private static string key = "";
        private static string input_charset = "";
        private static string sign_type = "";
        #endregion

        static Config()
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            //partner = "2088811467352658";
            Hi.Model.Pay_PayWxandAli model= Common.GetPayWxandAli(1046);
            partner = model.ali_partner;  // ConfigurationManager.AppSettings["partner"] == null ? "2088811467352658" : ConfigurationManager.AppSettings["partner"].ToString().Trim();

            //交易安全检验码，由数字和字母组成的32位字符串
            //key = model.ali_key;// ConfigurationManager.AppSettings["PayKey"] == null ? "g3sxskbx8udfcwnjl9rzmonny6bsas9r" : ConfigurationManager.AppSettings["PayKey"].ToString().Trim();
            //key = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBANDptrV3pWWdbnr6wT8lPkZ6kbRgsjf8CfXpQNrJyOsmktOaX8DQRh5nd0lSFTtbfeGV4oWXi26bv+1Vt+CfAsYssf+futRj0l6Cxx0psJapK7QEXq3HqjB0tM11M9ym75WFFvTSZs1DysUkRV35g5rXuRtBOWR2sqm9JRAen+29AgMBAAECgYEAzZ+L1xb5c4e960uOE1Hb9tDDQs/9+j6XqzQ3QmFj4Zeo4p9KaeRVb62U6lThUvgdcYDuYWEkuuyPvtEk1/CKb61AvEW69ehwLeDXOy9AzEgQpGFPb1bgJ+kU8YCpgcOGR9G55iVc0ZW7B2iyx111Wvij8pc+A2ZeuByAG1f8PoECQQD7wvCyBJRNb7Gv7iHF+zx2lDxG6LTX2rCAZdr8FyWVjZEBfL6uPI+/2J2AqtFPaLa25+jQc3b74r4wuGKsvxBhAkEA1G4aDQovfB9RO/c4I+NX4mmitpNt66IuqKp0a9pOL/YfpNtr5GBgmK4LMVASqIG74bw5wAV7zJkunlGGPusK3QJBALLiUm/KvS1AXbqpsymfV9jRfvrLQiPVaW/x72ULdVMMIaoy3rGiqmkgGtlfhhWsS5cutMfYIwTamVS4zrP7lkECQFTvDJVoHCI5d0ZNivG2ZR4OdFMhURKkTpl7RX8V0qsUcgR9An9WFWkWNT1rMXqUHGWd100yJBRirqP4Hn+rhDUCQBPVgm4jNd5WjKD0Oj39FmF5D89OviTgQK4xYxYUIfqLBhuGZ3kWDvAmeE6uRgVXjQncv/c68W8pS2huT0syom4=";
            key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";

            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";

            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "RSA";
        }

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设交易安全校验码
        /// </summary>
        public static string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string Sign_type
        {
            get { return sign_type; }
        }
        #endregion
    }
}