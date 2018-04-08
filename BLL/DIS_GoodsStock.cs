using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBUtility;

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 DIS_GoodsStock
    /// </summary>
    public partial class DIS_GoodsStock
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_GoodsStock model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_GoodsStock model, SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }

        /// <summary>
        /// 修改商品库存(减扣)
        /// </summary>
        /// <param name="goodsinfoid">商品ID</param>
        /// <param name="BatchNO">批次号</param>
        /// <param name="outnum">减扣数量</param>
        /// <returns>false 减扣失败 true 成功</returns>
        public bool UpdateStock(string goodsinfoid, string BatchNO, decimal outnum, SqlTransaction Tran)
        {
            string sql = "";
            if (!string.IsNullOrWhiteSpace(BatchNO))
            {
                sql = string.Format(" update DIS_GoodsStock set StockNum=StockNum-{0} where GoodsInfo={1} and BatchNo='{2}'", outnum, goodsinfoid, BatchNO);
                sql += string.Format(" update DIS_GoodsStock set StockToTalNum=StockToTalNum-{0} where GoodsInfo={1}", outnum, goodsinfoid);
            }
            else
            {
                sql = string.Format(" update DIS_GoodsStock set StockNum=StockNum-{0} where GoodsInfo={1} and BatchNo=''", outnum, goodsinfoid);
                sql += string.Format(" update DIS_GoodsStock set StockToTalNum=StockToTalNum-{0} where GoodsInfo={1}", outnum, goodsinfoid);
            }

            if (dal.GetUpdateStock(sql, Tran.Connection, Tran) <= 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改商品库存(加)
        /// </summary>
        /// <param name="goodsinfoid">商品ID</param>
        /// <param name="BatchNO">批次号</param>
        /// <param name="outnum">加数量</param>
        /// <returns>false 失败 true 成功</returns>
        public bool UpdateStockAdd(string goodsinfoid, string BatchNO, decimal outnum, SqlTransaction Tran)
        {
            string sql = "";
            if (!string.IsNullOrWhiteSpace(BatchNO))
            {
                sql = string.Format(" update DIS_GoodsStock set StockNum=StockNum+{0} where GoodsInfo={1} and BatchNo='{2}';", outnum, goodsinfoid, BatchNO);

                sql += string.Format(" update DIS_GoodsStock set StockToTalNum=StockToTalNum+{0} where GoodsInfo={1}", outnum, goodsinfoid);
            }
            else
            {
                sql = string.Format(" update DIS_GoodsStock set StockNum=StockNum+{0} where GoodsInfo={1} and BatchNo='{2}';", outnum, goodsinfoid, BatchNO);
                sql += string.Format(" update DIS_GoodsStock set StockToTalNum=StockToTalNum+{0} where GoodsInfo={1}", outnum, goodsinfoid);
            }

            if (dal.GetUpdateStock(sql, Tran.Connection, Tran) <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
