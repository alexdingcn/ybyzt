using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///Logistics 的摘要说明
/// </summary>
public class Logistics
{
    public Logistics()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    public string nu { get; set; }//物流单号
    public string comcontact { get; set; }//快递电话
    public string companytype { get; set; }
    public string com { get; set; }//物流公司编号
    public string status { get; set; }//查询结果状态： 0：物流单暂无结果， 1：查询成功， 2：接口出现异常，
    public string codenumber { get; set; }
    public string state { get; set; }//快递单当前的状态 ：　 
    //0：在途，即货物处于运输过程中；
    //1：揽件，货物已由快递公司揽收并且产生了第一条跟踪信息；
    //2：疑难，货物寄送过程出了问题；
    //3：签收，收件人已签收；
    //4：退签，即货物由于用户拒签、超区等原因退回，而且发件人已经签收；
    //5：派件，即快递正在进行同城派件；
    //6：退回，货物正处于退回发件人的途中；
    public List<Information> data { get; set; }
    public string message { get; set; }
    public string comurl { get; set; }
    public string condition { get; set; }
    public string ischeck { get; set; }
    public string errCode { get; set; }

}