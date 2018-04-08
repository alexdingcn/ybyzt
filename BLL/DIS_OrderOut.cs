using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using DBUtility;

namespace Hi.BLL
{
    public partial class DIS_OrderOut
    {
        /// <summary>
        /// 增加一条数据,带事务
        /// </summary>
        public int Add(Hi.Model.DIS_OrderOut model, SqlTransaction TranSaction = null)
        {
            return dal.Add(model, TranSaction);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="ID">订单ID</param>
        /// <returns></returns>
        public Hi.Model.DIS_OrderOut GetOutModel(int ID)
        {
            return dal.GetOutModel(ID);
        }

        /// <summary>
        /// 更新一条数据,带事务
        /// </summary>
        public bool Update(Hi.Model.DIS_OrderOut model, SqlTransaction TranSaction = null)
        {
            return dal.Update(model, TranSaction);
        }

         /// <summary>
        /// 查询订单信息 code 2.5  by 2016-08-01 szj
        /// </summary>
        /// <param name="strWhat"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetList(string strWhat, string where)
        {
            return dal.GetList(strWhat, where);
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="omodel">订单主表</param>
        /// <param name="ll">订单明细</param>
        /// <param name="outmodel">订单发货表</param>
        /// <param name="llo">订单发货明细表</param>
        /// <param name="log">订单物流表</param>
        /// <param name="stockOModel">发货出库主表</param>
        /// <param name="llinOut">发货出库从表</param>
        /// <returns></returns>
        public int GetOutOrder(Hi.Model.DIS_Order omodel, List<Hi.Model.DIS_OrderDetail> ll, Hi.Model.DIS_OrderOut outmodel, List<Hi.Model.DIS_OrderOutDetail> llo, Hi.Model.DIS_Logistics log,Hi.Model.DIS_StockOrder stockOModel , List<Hi.Model.DIS_StockInOut> llinOut)
        {
            SqlTransaction sqlTrans = DBUtility.SqlHelper.CreateStoreTranSaction();

            int outid = 0;

            try
            {
                //新增发货单
                outid = Add(outmodel, sqlTrans);

                if (outid > 0)
                {
                    //新增发货单明细
                    foreach (Hi.Model.DIS_OrderOutDetail item in llo)
                    {
                        if (item.OutNum <= 0)
                            continue;
                        item.OrderOutID = outid;
                        if (new Hi.BLL.DIS_OrderOutDetail().Add(item, sqlTrans) < 0)
                        {
                            outid = 0;
                            return outid;
                        }

                        //发货减扣库存
                        if (!new Hi.BLL.DIS_GoodsStock().UpdateStock(item.GoodsinfoID.ToString(), item.Batchno, item.OutNum, sqlTrans))
                        {
                            outid = 0;
                            return outid;
                        }

                    }
                    
                    //发货新增出库单
                    stockOModel.OrderID = outid;
                    int stockOrderID = new Hi.BLL.DIS_StockOrder().Add(stockOModel, sqlTrans);
                    if (stockOrderID > 0)
                    {
                        foreach (Hi.Model.DIS_StockInOut item in llinOut)
                        {
                            item.StockOrderID = stockOrderID;
                            if (new Hi.BLL.DIS_StockInOut().Add(item, sqlTrans) < 0)
                            {
                                outid = 0;
                                return outid;
                            }
                        }
                    }
                    else
                    {
                        outid = 0;
                        return outid;
                    }

                    //修改订单主表状态
                    if (new Hi.BLL.DIS_Order().UpdateOrder(sqlTrans.Connection, omodel, sqlTrans) < 0)
                    {
                        outid = 0;
                        return outid;
                    }
                           
                    //修改订单明细状态
                    foreach (Hi.Model.DIS_OrderDetail item in ll)
                    {
                        if (new Hi.BLL.DIS_OrderDetail().UpdateOrderDetail(sqlTrans.Connection, item, sqlTrans) < 0)
                        {
                            outid = 0;
                            return outid;
                        }
                    }
                    //新增物流
                    log.OrderOutID = outid;
                    if (new Hi.BLL.DIS_Logistics().Add(log, sqlTrans) < 0)
                    {
                        outid = 0;
                        return outid;
                    }
                    sqlTrans.Commit();
                }
            }
            catch (Exception)
            {
                if (sqlTrans.Connection != null)
                    sqlTrans.Rollback();
                throw;
            }

            return outid;
        }

        /// <summary>
        /// 修改发货单
        /// </summary>
        /// <param name="outmodel">发货单</param>
        /// <param name="omodel">订单</param>
        /// <param name="ll">订单明细</param>
        /// <param name="llo">发货单明细</param>
        /// <returns></returns>
        public int GetOutUpOrder(Hi.Model.DIS_OrderOut outmodel,Hi.Model.DIS_Order omodel, List<Hi.Model.DIS_OrderDetail> ll, List<Hi.Model.DIS_OrderOutDetail> llo)
        {
            SqlTransaction sqlTrans = DBUtility.SqlHelper.CreateStoreTranSaction();

            int outid = 0;
            try
            {
                //修改发货单主表状态
                if (!new Hi.BLL.DIS_OrderOut().Update(outmodel, sqlTrans))
                {
                    outid = 0;
                    return outid;
                }

                //修改订单主表状态
                if (new Hi.BLL.DIS_Order().UpdateOrder(sqlTrans.Connection, omodel, sqlTrans) < 0)
                {
                    outid = 0;
                    return outid;
                }
                //修改订单明细状态
                foreach (Hi.Model.DIS_OrderDetail item in ll)
                {
                    if (new Hi.BLL.DIS_OrderDetail().UpdateOrderDetail(sqlTrans.Connection, item, sqlTrans) < 0)
                    {
                        outid = 0;
                        return outid;
                    }
                }
                //修改发货单明细
                foreach (Hi.Model.DIS_OrderOutDetail item in llo)
                {
                    if (new Hi.BLL.DIS_OrderOutDetail().Update(item, sqlTrans) < 0)
                    {
                        outid = 0;
                        return outid;
                    }
                }
                sqlTrans.Commit();
                outid = 1;
            }
            catch (Exception)
            {
                if (sqlTrans.Connection != null)
                    sqlTrans.Rollback();
                throw;
            }

            return outid;
        }

        /// <summary>
        /// 作废发货单
        /// </summary>
        /// <param name="omodel">订单</param>
        /// <param name="outmodel">发货单</param>
        /// <param name="ol">订单明细</param>
        /// <param name="loud">本次发货明细</param>
        /// <returns></returns>
        public int GetCancelOut(Hi.Model.DIS_Order omodel, Hi.Model.DIS_OrderOut outmodel, List<Hi.Model.DIS_OrderDetail> ol, List<Hi.Model.DIS_OrderOutDetail> loud, string stockOids, string stockInids)
        {
            SqlTransaction sqlTrans = DBUtility.SqlHelper.CreateStoreTranSaction();

            int outid = 0;
            try
            {
                //修改订单主表状态
                if (!new Hi.BLL.DIS_Order().Update(omodel, sqlTrans))
                {
                    outid = 0;
                    return outid;
                }
                //修改订单明细状态
                foreach (Hi.Model.DIS_OrderDetail item in ol)
                {
                    if (new Hi.BLL.DIS_OrderDetail().UpdateOrderDetail(sqlTrans.Connection, item, sqlTrans) < 0)
                    {
                        outid = 0;
                        return outid;
                    }
                }

                foreach (var item in loud)
                {
                    //发货减扣库存
                    if (!new Hi.BLL.DIS_GoodsStock().UpdateStockAdd(item.GoodsinfoID.ToString(), item.Batchno, item.OutNum, sqlTrans))
                    {
                        outid = 0;
                        return outid;
                    }
                }

                if (!string.IsNullOrWhiteSpace(stockOids))
                {
                    string sql="update DIS_StockOrder set dr=1 where ID in ("+stockOids+")";
                    if (NonQuery(sql, sqlTrans.Connection, sqlTrans) <= 0)
                    {
                        outid = 0;
                        return outid;
                    }
                }

                if (!string.IsNullOrWhiteSpace(stockInids))
                {
                    string sql = "update DIS_StockInOut set dr=1 where ID in (" + stockInids + ")";
                    if (NonQuery(sql, sqlTrans.Connection, sqlTrans) <= 0)
                    {
                        outid = 0;
                        return outid;
                    }
                }

                //修改发货单主表状态
                if (!Update(outmodel, sqlTrans))
                {
                    outid = 0;
                    return outid;
                }
                sqlTrans.Commit();
                outid = 1;
            }
            catch (Exception)
            {
                if (sqlTrans.Connection != null)
                    sqlTrans.Rollback();
                throw;
            }

            return outid;
        }

        public DataTable getDataTable(int pageSize, int pageIndex, string strwhere, out int pageCount, out int Counts)
        {
            return dal.getDataTable(pageSize, pageIndex, strwhere,out pageCount,out Counts);
        }

        /// <summary>
        /// 修改订单删除商品时，返还库存
        /// </summary>
        /// <param name="str">修改库存的sql</param>
        /// <returns></returns>
        public int NonQuery(string str, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            SqlCommand cmd = new SqlCommand(str, sqlconn, sqltans);

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }
    }
}
