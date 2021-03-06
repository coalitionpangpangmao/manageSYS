﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSYS.Web.MateriaService;
using System.Xml;
using System.Collections;
namespace MSYS.Web
{
    public class UpdateSysConfig : UpdateFromMaster
    {
        public UpdateSysConfig()
        {

        }

        public override string InsertLocalFromMaster()
        {

            MSYS.Web.MateriaService.WsBaseDataInterfaceService service = new MSYS.Web.MateriaService.WsBaseDataInterfaceService();
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            tPubUnit[] units = service.getUnitList();
            List<string> commandlist = new List<string>();
            string[] seg = { "id", "unitCode", "unitName", "originCode", "unitGroupCode", "unitGroupName",
                                 "unitGroupType", "xyUnitCode", "isDel", "isValid"};

            foreach (tPubUnit unit in units)
            {

                string[] value = { unit.id.ToString(), unit.unitCode, unit.unitName, unit.originCode, unit.unitGroupCode, unit.unitGroupName, unit.unitGroupType, unit.xyUnitCode, unit.isDel, unit.isValid };
                string temp = opt.getMergeStr(seg, value, 1, "HT_INNER_UNIT");
                commandlist.Add(temp);
                if (opt.UpDateOra(temp) != "Success")
                    System.Diagnostics.Debug.Write(temp);

            }
            return opt.TransactionCommand(commandlist);


        }

        protected override void InsertLocalFromMasterAsyn()
        {
            MSYS.Web.MateriaService.WsBaseDataInterfaceService service = new MSYS.Web.MateriaService.WsBaseDataInterfaceService();
            service.getUnitListCompleted += new getUnitListCompletedEventHandler(service_Completed);
            service.getUnitListAsync();

            MSYS.Web.StoreService.StoreServiceInterfaceService storeservice = new StoreService.StoreServiceInterfaceService();

           

        }

        private static void service_Completed(object sender, getUnitListCompletedEventArgs e)
        {
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            tPubUnit[] units = e.Result;
            //List<string> commandlist = new List<string>();
            string[] seg = { "id", "unitCode", "unitName", "originCode", "unitGroupCode", "unitGroupName",
                                 "unitGroupType", "xyUnitCode", "isDel", "isValid"};

            foreach (tPubUnit unit in units)
            {

                string[] value = { unit.id.ToString(), unit.unitCode, unit.unitName, unit.originCode, unit.unitGroupCode, unit.unitGroupName, unit.unitGroupType, unit.xyUnitCode, unit.isDel, unit.isValid };
                string temp = opt.getMergeStr(seg, value, 1, "HT_INNER_UNIT");
                //   commandlist.Add(temp);
                if (opt.UpDateOra(temp) != "Success")
                    System.Diagnostics.Debug.Write(temp);
            }
            //   return opt.TransactionCommand(commandlist);
        }
    }


}

