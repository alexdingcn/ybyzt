using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

/// <summary>
/// 招商信息新增
/// </summary>
public partial class Company_CMerchants_CMerchantsAdd : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hidCompID.Value = this.CompID.ToString();
            this.txtForceDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtInvalidDate.Value = "2099-12-31";
            bind();
        }
    }


    /// <summary>
    /// 招商信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.YZT_CMerchants cmModel = new Hi.BLL.YZT_CMerchants().GetModel(this.KeyID);

            this.hidGoodsID.Value = cmModel.GoodsID.ToString();
            //this.txtCMCode.Value = cmModel.CMCode;
            this.txtCMName.Value = cmModel.CMName;
            this.txtGoodsCode.Value = Common.GetGoodsName(cmModel.GoodsID.ToString(), "GoodsCode");
            this.txtGoodsName.Value = Common.GetGoodsName(cmModel.GoodsID.ToString(), "GoodsName");
            this.OrderNote.Value = cmModel.Remark;
            this.txtValueInfo.Value = cmModel.ValueInfo;
            this.txtInvalidDate.Value = cmModel.InvalidDate == DateTime.MinValue ? "" : cmModel.InvalidDate.ToString("yyyy-MM-dd");
            this.txtForceDate.Value = cmModel.ForceDate == DateTime.MinValue ? "" : cmModel.ForceDate.ToString("yyyy-MM-dd");
            this.txtCategoryID.Value = Common.GetCategoryName(Common.GetGoodsName(cmModel.GoodsID.ToString(), "CategoryID"));
            this.hidCategoryID.Value = cmModel.CategoryID.ToString();
            this.ddrtype.Value = cmModel.Type.ToString();

            if (cmModel.Type.ToString() == "2" || cmModel.Type.ToString() == "3")
            {
                string sql = @"select cm.ID,d.DisID,dis.DisName,a.AreaID,da.AreaName,a.Province,a.City,a.Area from YZT_CMerchants cm left join 
YZT_FCArea a on cm.ID=a.CMID left join BD_DisArea da on a.AreaID=da.ID  left join YZT_FCDis d left join BD_Distributor dis on d.DisID=dis.ID on cm.ID=d.CMID where cm.ID=" + this.KeyID;
                string fc = string.Empty;
                string fcid = string.Empty;
                DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (cmModel.Type.ToString() == "2")
                        {
                            fc += fc == "" ? item["Province"] + "|" + item["City"] + "|" + item["Area"] : "," + item["Province"] + "|" + item["City"] + "|" + item["Area"];
                            fcid += fcid == "" ? item["Province"] + "|" + item["City"] + "|" + item["Area"] : "," + item["Province"] + "|" + item["City"] + "|" + item["Area"];
                        }
                        else
                        {
                            fc += fc == "" ? item["DisName"] : "," + item["DisName"];
                            fcid += fcid == "" ? item["ID"] : "," + item["ID"];
                        }
                    }
                }
                this.txtFC.Value = fc;
                this.hidfc.Value = fcid;
                this.selType.Attributes.Add("class", "lb fl");
            }

            if (cmModel.ProvideData.IndexOf("1") > -1)
            {
                this.chk1.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("2") > -1)
            {
                this.chk2.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("3") > -1)
            {
                this.chk3.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("4") > -1)
            {
                this.chk4.Checked = true;
            }

        }
    }


    /// <summary>
    /// 获取单号
    /// </summary>
    /// <returns></returns>
    public string GetNo()
    {
        string title = string.Empty;
        string no = string.Empty;
        int ID = new Hi.BLL.YZT_CMerchants().GetMaxId();
        if (ID < 1)
        {
            no = DateTime.Now.ToString("yyyyMMdd")+"000001";
        }
        else
        {
            no = new Hi.BLL.YZT_CMerchants().GetModel(ID).CMCode;
        }

        int num = no.Length >= 8 ? Convert.ToInt32(no.Substring(8)) + 1 : 1;
        if (num < 10)
        {
            no = "00000" + num;
        }
        else if (num < 100)
        {
            no = "0000" + num;
        }
        else if (num < 1000)
        {
            no = "000" + num.ToString();
        }
        else if (num < 10000)
        {
            no = "00" + num.ToString();
        }
        else if (num < 100000)
        {
            no = "0" + num.ToString();
        }
        else
        {
            no = num.ToString();
        }

        string No = title + DateTime.Now.ToString("yyyyMMdd") + no;
        return No;
    }

    /// <summary>
    /// 新增招商信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hi.Model.YZT_CMerchants cmModel = null;

        string [] ProvideData=new string[4];
        if (this.chk1.Checked)
            ProvideData[0] = "1";
        if (this.chk2.Checked)
            ProvideData[1] = "2";
        if (this.chk3.Checked)
            ProvideData[2] = "3";
        if (this.chk4.Checked)
            ProvideData[3] = "4";

        if (KeyID != 0)
        {
            cmModel = new Hi.BLL.YZT_CMerchants().GetModel(this.KeyID);

            SqlTransaction Tran = null;
            try
            {
                Tran = DBUtility.SqlHelper.CreateStoreTranSaction();

                if ("2".Equals(cmModel.Type.ToString())) {
                    if (!new Hi.BLL.YZT_FCArea().CMDelete(this.KeyID, Tran)) {
                        //Tran.Rollback();
                        //return;
                    }
                }
                else if ("3".Equals(cmModel.Type.ToString()))
                {
                    if (!new Hi.BLL.YZT_FCDis().CMDelete(this.KeyID, Tran))
                    {
                        //Tran.Rollback();
                        //return;
                    }
                }

                //cmModel.CMCode = this.txtCMCode.Value.Trim();
                cmModel.CMName = this.txtCMName.Value.Trim();

                cmModel.GoodsID = this.hidGoodsID.Value.ToInt(0);
                cmModel.GoodsCode = this.txtGoodsCode.Value.Trim();
                cmModel.GoodsName = this.txtGoodsName.Value.Trim();
                cmModel.Remark = this.OrderNote.Value.Trim();
                cmModel.ValueInfo = this.txtValueInfo.Value.Trim();
                cmModel.InvalidDate = this.txtInvalidDate.Value != "" ? Convert.ToDateTime(this.txtInvalidDate.Value.Trim()) : DateTime.MinValue;
                cmModel.ForceDate = this.txtForceDate.Value != "" ? Convert.ToDateTime(this.txtForceDate.Value.Trim()) : DateTime.MinValue;
                cmModel.ProvideData = string.Join(",", ProvideData);
                cmModel.CategoryID = this.hidCategoryID.Value.Trim().ToInt(0);
                cmModel.Type = ddrtype.Value.ToString().ToInt(0);
                cmModel.IsEnabled = 0;
                cmModel.ts = DateTime.Now;
                cmModel.modifyuser = this.UserID;

                string fc = this.hidfc.Value;
                if (new Hi.BLL.YZT_CMerchants().Update(cmModel, Tran))
                {
                    if (!"".Equals(fc))
                    {
                        string[] fcval = fc.Split(new char[] { ',' });
                        if (ddrtype.Value.ToString() == "2")
                        {
                            //指定区域
                            Hi.Model.YZT_FCArea areaModel = null;
                            foreach (var item in fcval)
                            {
                                string[] fcvals = item.ToString().Split(new char[] { '|' });

                                areaModel = new Hi.Model.YZT_FCArea();
                                //areaModel.AreaID = item.ToInt(0);
                                areaModel.Province = fcvals[0] + "";
                                areaModel.City = fcvals[1] + "";
                                areaModel.Area = fcvals[2] + "";

                                areaModel.CMID = this.KeyID;
                                areaModel.CompID = this.CompID;
                                areaModel.ts = DateTime.Now;
                                areaModel.CreateDate = DateTime.Now;
                                areaModel.CreateUserID = this.UserID;
                                areaModel.modifyuser = this.UserID;
                                new Hi.BLL.YZT_FCArea().Add(areaModel, Tran);
                            }

                        }
                        else if (ddrtype.Value.ToString() == "3")
                        {
                            //指定代理商
                            Hi.Model.YZT_FCDis fcdisModel = null;
                            foreach (var item in fcval)
                            {
                                fcdisModel = new Hi.Model.YZT_FCDis();
                                fcdisModel.DisID = item.ToInt(0);
                                fcdisModel.CMID = this.KeyID;
                                fcdisModel.CompID = this.CompID;

                                fcdisModel.ts = DateTime.Now;
                                fcdisModel.CreateDate = DateTime.Now;
                                fcdisModel.CreateUserID = this.UserID;
                                fcdisModel.modifyuser = this.UserID;

                                new Hi.BLL.YZT_FCDis().Add(fcdisModel, Tran);
                            }
                        }
                    }
                    Tran.Commit();
                    Response.Redirect("CMerchantsInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
                }
                else {
                    Tran.Rollback();
                }
            }
            catch (Exception)
            {
                //Tran.Rollback();
                throw;
            }
        }
        else
        {

            cmModel = new Hi.Model.YZT_CMerchants();

            cmModel.CompID = this.CompID;
            cmModel.CMCode = GetNo(); //this.txtCMCode.Value.Trim();
            cmModel.CMName = this.txtCMName.Value.Trim();

            cmModel.GoodsCode = this.txtGoodsCode.Value.Trim();
            cmModel.GoodsName = this.txtGoodsName.Value.Trim();
            cmModel.Remark = this.OrderNote.Value.Trim();
            cmModel.ValueInfo = this.txtValueInfo.Value.Trim();
            cmModel.InvalidDate = this.txtInvalidDate.Value != "" ? Convert.ToDateTime(this.txtInvalidDate.Value.Trim()) : DateTime.MinValue;
            cmModel.ForceDate = this.txtForceDate.Value != "" ? Convert.ToDateTime(this.txtForceDate.Value.Trim()) : DateTime.MinValue;
            cmModel.ProvideData = string.Join(",", ProvideData);
            cmModel.CategoryID = this.txtCategoryID.Value.Trim().ToInt(0);
            cmModel.Type = ddrtype.Value.ToString().ToInt(0);
            cmModel.IsEnabled = 0;
            cmModel.GoodsID = this.hidGoodsID.Value.ToInt(0);
            cmModel.ts = DateTime.Now;
            cmModel.CreateDate = DateTime.Now;
            cmModel.CreateUserID = this.UserID;
            cmModel.modifyuser = this.UserID;

            string fc = this.hidfc.Value;

            SqlTransaction Tran = null;
            try
            {
                Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                int id = new Hi.BLL.YZT_CMerchants().Add(cmModel, Tran);
                if (id > 0)
                {
                    if (!"".Equals(fc))
                    {
                        string[] fcval = fc.Split(new char[] { ',' });
                        if (ddrtype.Value.ToString() == "2")
                        {
                            //指定区域
                            Hi.Model.YZT_FCArea areaModel = null;
                            foreach (var item in fcval)
                            {
                                string[] fcvals = item.ToString().Split(new char[] { '|' });
                                areaModel = new Hi.Model.YZT_FCArea();
                                //areaModel.AreaID = item.ToInt(0);
                                areaModel.Province = fcvals[0] + "";
                                areaModel.City = fcvals[1] + "";
                                areaModel.Area = fcvals[2] + "";
                                areaModel.CMID = id;
                                areaModel.CompID = this.CompID;
                                areaModel.ts = DateTime.Now;
                                areaModel.CreateDate = DateTime.Now;
                                areaModel.CreateUserID = this.UserID;
                                areaModel.modifyuser = this.UserID;
                                new Hi.BLL.YZT_FCArea().Add(areaModel, Tran);
                            }

                        }
                        else if (ddrtype.Value.ToString() == "3")
                        {
                            //指定代理商

                            Hi.Model.YZT_FCDis fcdisModel = null;
                            foreach (var item in fcval)
                            {
                                fcdisModel = new Hi.Model.YZT_FCDis();
                                fcdisModel.DisID = item.ToInt(0);
                                fcdisModel.CMID = id;
                                fcdisModel.CompID = this.CompID;

                                fcdisModel.ts = DateTime.Now;
                                fcdisModel.CreateDate = DateTime.Now;
                                fcdisModel.CreateUserID = this.UserID;
                                fcdisModel.modifyuser = this.UserID;

                                new Hi.BLL.YZT_FCDis().Add(fcdisModel, Tran);
                            }
                        }
                    }

                    Tran.Commit();
                    Response.Redirect("CMerchantsInfo.aspx?KeyID=" + Common.DesEncrypt(id.ToString(), Common.EncryptKey));
                }
                else
                {
                    Tran.Rollback();
                }
            }
            catch (Exception)
            {
                //Tran.Rollback();
                throw;
            }
        }
    }
}