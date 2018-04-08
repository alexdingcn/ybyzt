using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Model
{
    
}

/// <summary>
/// 登录
/// </summary>
public class User
{
    public int UserID { get; set; }

    public string TrueName { get; set; }
    public string Sex { get; set; }
    public string Phone { get; set; }

    public int DisID { get; set; }
    public string DisName { get; set; }

    public int IsEnabled { get; set; }
    public int Erptype { get; set; }

    public int CompID { get; set; }
    public string CompName { get; set; }
    public int CType { get; set; } // 1：核心企业  2：经销商

    public int UType { get; set; }

    public int CompUserID { get; set; }

    public string isLastTime { get; set; }
}

/// <summary>
/// 物流公司信息
/// </summary>
public class LogisticsChoice
{
    public string LogisticsCode { get; set; }
    public string LogisticsName { get; set; }
}