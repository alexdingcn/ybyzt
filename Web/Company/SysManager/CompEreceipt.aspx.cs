using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_CompEreceipt : CompPageBase
{
    public List<Hi.Model.BD_Ereceipt> erl = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    protected void Bind()
    {
        erl = new Hi.BLL.BD_Ereceipt().GetList("", " CompID=" + this.CompID + " and isnull(dr,0)=0", "");

        if (erl != null && erl.Count > 0)
        {
            foreach (Hi.Model.BD_Ereceipt item in erl)
            {
                this.txtereceipt_batchno.Value = item.ereceipt_batchno.ToString();
                //this.txtereceipt_brd.Value = item.ereceipt_brd.ToString();
                //this.txtereceipt_chkbill.Value = item.ereceipt_chkbill.ToString();
                this.txtereceipt_duedate.Value = item.ereceipt_duedate.ToString("yyyy-MM-dd");
                //this.txtereceipt_gds.Value = item.ereceipt_gds.ToString();
                //this.txtereceipt_gdsdic.Value = item.ereceipt_gdsdic.ToString();
                //this.txtereceipt_grd.Value = item.ereceipt_grd.ToString();
                //this.txtereceipt_hder.Value = item.ereceipt_hder.ToString();
                //this.txtereceipt_kd.Value = item.ereceipt_kd.ToString();
                this.txtereceipt_mfters.Value = item.ereceipt_mfters.ToString();
                //this.txtereceipt_nm.Value = item.ereceipt_nm.ToString();
                //this.txtereceipt_num.Value = item.ereceipt_num.ToString();
                //this.txtereceipt_price.Value = item.ereceipt_price.ToString("0.00");
                //this.txtereceipt_rtbill.Value = item.ereceipt_rtbill.ToString();
                //this.txtereceipt_sgndt.Value = item.ereceipt_sgndt.ToString("yyyy-MM-dd");
                this.txtereceipt_std.Value = item.ereceipt_std.ToString();
                //this.txtereceipt_unit.Value = item.ereceipt_unit.ToString();
                //this.txtereceipt_value.Value = item.ereceipt_value.ToString();
                this.txtereceipt_whnm.Value = item.ereceipt_whnm.ToString();
                this.txtereceipt_whno.Value = item.ereceipt_whno.ToString();
            }
        }
    }

    /// <summary>
    /// 保存仓单信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Save(object sender, EventArgs e)
    {
        string str = string.Empty;

        #region
        string ereceipt_rtbill = string.Empty;  //仓位编号
        string ereceipt_kd = string.Empty;   //品种编号
        string ereceipt_nm = string.Empty;   //品种名称
        string ereceipt_std = string.Empty;  //规 格
        string ereceipt_grd = string.Empty;  //等 级 *
        string ereceipt_unit = string.Empty; //单 位
        decimal ereceipt_num = 0;  //数 量
        decimal ereceipt_price = 0;  //参考价格
        decimal ereceipt_value = 0;  //参考价值
        string ereceipt_brd = string.Empty;  //品 牌 *
        string ereceipt_chkbill = string.Empty;  //检验证书编号 *
        DateTime ereceipt_duedate = DateTime.Now;  //失效日期
        string ereceipt_gdsdic = string.Empty;  //商品描述 *
        string ereceipt_hder = string.Empty;  //货物归属方 *
        string ereceipt_gds = string.Empty;   //货 位 *
        string ereceipt_whnm = string.Empty;  //仓库名称
        string ereceipt_whno = string.Empty;  //仓库编号
        string ereceipt_batchno = string.Empty;  //批次号
        DateTime ereceipt_sgndt = DateTime.Now;  //签发日期  *
        string ereceipt_mfters = string.Empty;  //生产厂家

        //if (this.txtereceipt_rtbill.Value.Trim().ToString() == "")
        //{
        //    str += "仓位编号不能为空。\\r\\n";
        //}
        //else
        //{
        //    ereceipt_rtbill = this.txtereceipt_rtbill.Value.Trim().ToString();
        //}

        //if (this.txtereceipt_kd.Value.Trim().ToString() == "")
        //{
        //    str += "品种编号不能为空。\\r\\n";
        //}
        //else
        //{
        //    ereceipt_kd = this.txtereceipt_kd.Value.Trim().ToString();
        //}

        //if (this.txtereceipt_nm.Value.Trim().ToString() == "")
        //{
        //    str += "品种名称不能为空。\\r\\n";
        //}
        //else
        //{
        //    ereceipt_nm = this.txtereceipt_nm.Value.Trim().ToString();
        //}

        if (this.txtereceipt_std.Value.Trim().ToString() == "")
        {
            str += "规格不能为空。\\r\\n";
        }
        else
        {
            ereceipt_std = this.txtereceipt_std.Value.Trim().ToString();
        }

        //ereceipt_grd = this.txtereceipt_grd.Value.Trim().ToString();

        //if (this.txtereceipt_unit.Value.Trim().ToString() == "")
        //{
        //    str += "单位不能为空。\\r\\n";
        //}
        //else
        //{
        //    ereceipt_unit = this.txtereceipt_unit.Value.Trim().ToString();
        //}

        //if (this.txtereceipt_num.Value.Trim().ToString() == "")
        //{
        //    str += "数量不能为空。\\r\\n";
        //}
        //else
        //{
        //    ereceipt_num = this.txtereceipt_num.Value.Trim().ToString().ToDecimal(0);
        //}

        //if (this.txtereceipt_price.Value.Trim().ToString() == "")
        //{
        //    str += "参考价格不能为空。\\r\\n";
        //}
        //else
        //{
        //    ereceipt_price = this.txtereceipt_price.Value.Trim().ToString().ToDecimal(0);
        //}

        //if (this.txtereceipt_value.Value.Trim().ToString() == "")
        //{
        //    str += "参考价值不能为空。\\r\\n";
        //}
        //else
        //{
        //    ereceipt_value = this.txtereceipt_value.Value.Trim().ToString().ToDecimal(0);
        //}

        //if (this.txtereceipt_brd.Value.Trim().ToString() == "")
        //{
        //    str += "品牌不能为空。\\r\\n";
        //}
        //else
        //{
        //    ereceipt_brd = this.txtereceipt_brd.Value.Trim().ToString();
        //}

        //ereceipt_chkbill = this.txtereceipt_chkbill.Value.Trim().ToString();

        if (this.txtereceipt_duedate.Value.Trim().ToString() == "")
        {
            str += "失效日期不能为空。\\r\\n";
        }
        else
        {
            ereceipt_duedate = this.txtereceipt_duedate.Value.Trim().ToString().ToDateTime();
        }

        //ereceipt_gdsdic = this.txtereceipt_gdsdic.Value.Trim().ToString();
        //ereceipt_hder = this.txtereceipt_hder.Value.Trim().ToString();
        //ereceipt_gds = this.txtereceipt_gds.Value.Trim().ToString();

        if (this.txtereceipt_whnm.Value.Trim().ToString() == "")
        {
            str += "仓库名称不能为空。\\r\\n";
        }
        else
        {
            ereceipt_whnm = this.txtereceipt_whnm.Value.Trim().ToString();
        }

        if (this.txtereceipt_whno.Value.Trim().ToString() == "")
        {
            str += "仓库编号不能为空。\\r\\n";
        }
        else
        {
            ereceipt_whno = this.txtereceipt_whno.Value.Trim().ToString();
        }

        if (this.txtereceipt_batchno.Value.Trim().ToString() == "")
        {
            str += "批次号不能为空。\\r\\n";
        }
        else
        {
            ereceipt_batchno = this.txtereceipt_batchno.Value.Trim().ToString();
        }

        //签发日期
        //if (this.txtereceipt_sgndt.Value.Trim().ToString() != "")
        //{
        //    ereceipt_sgndt = this.txtereceipt_sgndt.Value.Trim().ToString().ToDateTime();
        //}

        if (this.txtereceipt_mfters.Value.Trim().ToString() == "")
        {
            str += "生产厂家不能为空。\\r\\n";
        }
        else
        {
            ereceipt_mfters = this.txtereceipt_mfters.Value.Trim().ToString();
        }


        if (str != "")
        {
            JScript.AlertMsgOne(this, str, JScript.IconOption.错误);
            return;
        }

        #endregion

        try
        {
            Hi.Model.BD_Ereceipt Emodel = null;

            erl = new Hi.BLL.BD_Ereceipt().GetList("", " CompID=" + this.CompID + " and isnull(dr,0)=0", "");
            if (erl != null && erl.Count > 0)
            {
                foreach (Hi.Model.BD_Ereceipt item in erl)
                {
                    Emodel = new Hi.Model.BD_Ereceipt();

                    Emodel.ID = item.ID;
                    Emodel.CompID = this.CompID;
                    Emodel.ereceipt_batchno = ereceipt_batchno;
                    Emodel.ereceipt_brd = ereceipt_brd;
                    Emodel.ereceipt_chkbill = ereceipt_chkbill;
                    Emodel.ereceipt_duedate = ereceipt_duedate;
                    Emodel.ereceipt_gds = ereceipt_gds;
                    Emodel.ereceipt_gdsdic = ereceipt_gdsdic;
                    Emodel.ereceipt_grd = ereceipt_grd;
                    Emodel.ereceipt_hder = ereceipt_hder;
                    Emodel.ereceipt_kd = ereceipt_kd;
                    Emodel.ereceipt_mfters = ereceipt_mfters;
                    Emodel.ereceipt_nm = ereceipt_nm;
                    Emodel.ereceipt_num = ereceipt_num;
                    Emodel.ereceipt_price = ereceipt_price;
                    Emodel.ereceipt_rtbill = ereceipt_rtbill;
                    Emodel.ereceipt_sgndt = ereceipt_sgndt;
                    Emodel.ereceipt_std = ereceipt_std;
                    Emodel.ereceipt_unit = ereceipt_unit;
                    Emodel.ereceipt_value = ereceipt_value;
                    Emodel.ereceipt_whnm = ereceipt_whnm;
                    Emodel.ereceipt_whno = ereceipt_whno;
                    Emodel.modifyuser = this.UserID;
                    Emodel.ts = DateTime.Now;
                }

                if (new Hi.BLL.BD_Ereceipt().Update(Emodel))
                {
                    ClientScript.RegisterStartupScript(GetType(), "", "<script>location.href='CompEreceiptInfo.aspx';</script>");
                }
            }
            else
            {
                Emodel = new Hi.Model.BD_Ereceipt();
                Emodel.CompID = this.CompID;
                Emodel.ereceipt_batchno = ereceipt_batchno;
                Emodel.ereceipt_brd = ereceipt_brd;
                Emodel.ereceipt_chkbill = ereceipt_chkbill;
                Emodel.ereceipt_duedate = ereceipt_duedate;
                Emodel.ereceipt_gds = ereceipt_gds;
                Emodel.ereceipt_gdsdic = ereceipt_gdsdic;
                Emodel.ereceipt_grd = ereceipt_grd;
                Emodel.ereceipt_hder = ereceipt_hder;
                Emodel.ereceipt_kd = ereceipt_kd;
                Emodel.ereceipt_mfters = ereceipt_mfters;
                Emodel.ereceipt_nm = ereceipt_nm;
                Emodel.ereceipt_num = ereceipt_num;
                Emodel.ereceipt_price = ereceipt_price;
                Emodel.ereceipt_rtbill = ereceipt_rtbill;
                Emodel.ereceipt_sgndt = ereceipt_sgndt;
                Emodel.ereceipt_std = ereceipt_std;
                Emodel.ereceipt_unit = ereceipt_unit;
                Emodel.ereceipt_value = ereceipt_value;
                Emodel.ereceipt_whnm = ereceipt_whnm;
                Emodel.ereceipt_whno = ereceipt_whno;
                Emodel.modifyuser = this.UserID;
                Emodel.ts = DateTime.Now;

                if (new Hi.BLL.BD_Ereceipt().Add(Emodel) > 0)
                {
                    ClientScript.RegisterStartupScript(GetType(), "", "<script>location.href='CompEreceiptInfo.aspx';</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}