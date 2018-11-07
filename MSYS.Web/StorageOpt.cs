﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSYS.Web.StoreService;
using System.Data;
using System.Xml;
using System.Collections;
namespace MSYS.Web
{
    public class StorageOpt
    {
        public StorageOpt()
        {
         
        }

        public string InOrOut(string PZ_code,string name,string nameno)//出入库单据号及当前操作人员
        {
            MSYS.Web.StoreService.StoreServiceInterfaceService service = new MSYS.Web.StoreService.StoreServiceInterfaceService();
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            string query = "select t.*,r.name as creator,s.name as modifier,q.formula_code a from ht_strg_materia t left join ht_svr_user r on r.id = t.creator_id left join ht_svr_user s on s.id = t.modify_id left join ht_qa_mater_formula q on q.prod_code = substr(t.monthplanno,9,7) where t.ORDER_SN = '" + PZ_code + "'";
            DataSet data = opt.CreateDataSetOra(query);
            if (data != null && data.Tables[0].Rows.Count > 0)
            {
                DataRow row = data.Tables[0].Rows[0];
                materInAndOutVO r = new materInAndOutVO();
                r.loginname = nameno;
                r.name = name;
                r.planNo = row["MONTHPLANNO"].ToString();
                r.prodCode = row["MONTHPLANNO"].ToString().Substring(8,7);
                r.shiftCode = row["SHIFT_CODE"].ToString();
                r.teamCode = row["TEAM_CODE"].ToString();
                r.FDate =Convert.ToDateTime( row["FDATE"].ToString());
                r.createId = row["CREATOR_ID"].ToString();
                r.creator = row["creator"].ToString();
                r.modifyId = row["Modify_ID"].ToString();
                r.modifyMan = row["modifier"].ToString();
                r.modifyTime = row["MODIFY_TIME"].ToString();
                r.batchNumber =Convert.ToDouble( row["BATCHNUM"].ToString());
                r.caboSum = Convert.ToDouble(row["CABOSUM"].ToString());
                r.piecesSum = Convert.ToDouble(row["PEICESSUM"].ToString());
                r.remark = row["REMARK"].ToString();
                r.inoutType = row["STRG_TYPE"].ToString();
                r.cwarehouseid = row["WARE_HOUSE_ID"].ToString();
                r.formulaId = Convert.ToInt64(row["formula_code"].ToString());  
                List<tShopMaterInoutSub> s = new List<tShopMaterInoutSub>();
                DataSet details = opt.CreateDataSetOra("select t.*,r.material_name from ht_strg_mater_sub t left join ht_pub_materiel r on r.material_code = t.mater_code  where t.main_code = '" + PZ_code + "' and t.is_del = '0'");
                if (details != null && details.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in details.Tables[0].Rows)
                    {
                        tShopMaterInoutSub sub = new tShopMaterInoutSub();
                        sub.mainId = drow["MAIN_CODE"].ToString();
                        sub.materCode = drow["MATER_CODE"].ToString();
                        sub.materName = drow["material_name"].ToString();
                        sub.materType = drow["MATER_FLAG"].ToString();
                        sub.unitCode = drow["UNIT_CODE"].ToString();
                        sub.unitName = drow["TEAM_CODE"].ToString();
                        sub.occurQty = drow["ORIGINAL_DEMAND"].ToString();
                        sub.remark = drow["REMARK"].ToString();
                        sub.warehouseCode = drow["WAREHOUSECODE"].ToString();
                        sub.packingNumbers = Convert.ToDouble( drow["PACKNUM"].ToString());
                        sub.substance = Convert.ToDouble( drow["SUBSTANCE"].ToString());
                        sub.oddQty = Convert.ToDouble( drow["ODDQTY"].ToString());
                        s.Add(sub);
                    }                    
                }
                r.subList = s.ToArray();
                return service.yuanliaoInAndOut4ws_03(r).status ;
            }
            else
                return "Falied";
        }
    }


}

