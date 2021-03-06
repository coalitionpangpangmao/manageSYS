﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Authority_GroupConfig : MSYS.Web.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageLoad(sender, e);
        if (!IsPostBack)
        {
            bindData();
            BindList(RightTree.Nodes, "");
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            opt.bindDropDownList(listPrt, "select * from ht_svr_prt_menu where IS_DEL = '0' order by ID", "NAME", "ID");

        }

    }

    protected void bindData()
    {
        string query = "select t.mapid as MapID,r.f_pid as 父菜单,r.f_menu as 菜单名称,t.url as URL,t.remark  as 描述 from ht_inner_map t left join ht_svr_sys_menu r on r.f_mapid = t.mapid and r.f_type = '0' where t.is_del = '0'  order by t.mapid ";
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra(query);
        GridView1.DataSource = data;
        GridView1.DataBind();
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                DataRowView mydrv = data.Tables[0].DefaultView[i];
                ((DropDownList)GridView1.Rows[i].FindControl("listPrt")).SelectedValue = mydrv["父菜单"].ToString();
                ((TextBox)GridView1.Rows[i].FindControl("txtID")).Text = mydrv["MapID"].ToString();
                ((TextBox)GridView1.Rows[i].FindControl("txtMenu")).Text = mydrv["菜单名称"].ToString();
                ((TextBox)GridView1.Rows[i].FindControl("txtURL")).Text = mydrv["URL"].ToString();
                ((TextBox)GridView1.Rows[i].FindControl("txtDscrp")).Text = mydrv["描述"].ToString();
            }
        }

    }
    protected void btnAdds_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "select t.mapid as MapID,r.f_pid as 父菜单,r.f_menu as 菜单名称,t.url as URL,t.remark  as 描述 from ht_inner_map t left join ht_svr_sys_menu r on r.f_mapid = t.mapid and r.f_type = '0' where t.is_del = '0'  order by t.mapid ";
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            DataSet set = opt.CreateDataSetOra(query);
            DataTable data = new DataTable();
            string newMapID = (Convert.ToInt16(opt.GetSegValue("select max(MapID) as ID from ht_inner_map", "ID")) + 1).ToString().PadLeft(5, '0');

            if (set == null)
            {
                data.Columns.Add("MapID");
                data.Columns.Add("父菜单");
                data.Columns.Add("菜单名称");
                data.Columns.Add("URL");
                data.Columns.Add("描述");
            }
            else
                data = set.Tables[0];
            object[] value = { newMapID, "", "", "", "" };
            data.Rows.Add(value);
            GridView1.DataSource = data;
            GridView1.DataBind();
            if (data != null && data.Rows.Count > 0)
            {
                for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
                {
                    DataRowView mydrv = data.DefaultView[i];
                    ((DropDownList)GridView1.Rows[i].FindControl("listPrt")).SelectedValue = mydrv["父菜单"].ToString();
                    ((TextBox)GridView1.Rows[i].FindControl("txtID")).Text = mydrv["MapID"].ToString();
                    ((TextBox)GridView1.Rows[i].FindControl("txtMenu")).Text = mydrv["菜单名称"].ToString();
                    ((TextBox)GridView1.Rows[i].FindControl("txtURL")).Text = mydrv["URL"].ToString();
                    ((TextBox)GridView1.Rows[i].FindControl("txtDscrp")).Text = mydrv["描述"].ToString();
                }
            }
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void BindList(TreeNodeCollection ncTree, string pID)
    {
        ncTree.Clear();
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        string query = "select ID,Name,MenuLEVEL,PID from ht_svr_prt_menu ";
        if (pID == "")
            query += " where pid is null ";
        else
            query += " where PID = '" + pID + "'";
        DataSet data = opt.CreateDataSetOra(query);
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            DataRow[] Rows = data.Tables[0].Select();
            foreach (DataRow row in Rows)
            {
                TreeNode pnode = new TreeNode(row["Name"].ToString(), row["ID"].ToString() + "_" + row["MenuLEVEL"].ToString() + "_" + row["PID"].ToString());
                BindList(pnode.ChildNodes, row["ID"].ToString());
                ncTree.Add(pnode);


            }
        }
    }
    protected void RightTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        string[] array = RightTree.SelectedValue.Split('_');
        txtMenu.Text = RightTree.SelectedNode.Text;
        listLevel.SelectedValue = array[1];
        listPrt.SelectedValue = array[2];
    }
    protected void btnSave2_Click(object sender, EventArgs e)
    {
     
            if (txtMenu.Text == "")
                ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "", "alert('请输入菜单名');", true);
            else
            {
                MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
                string prtID;
                if ("NoRecord" == (prtID = opt.GetSegValue("select * from HT_SVR_PRT_MENU where NAME = '" + txtMenu.Text + "'", "ID")))
                {
                    prtID = (Convert.ToInt16(opt.GetSegValue("select Max(ID) as ID from HT_SVR_PRT_MENU ", "ID")) + 1).ToString().PadLeft(2, '0');
                }
               
                string[] seg = { "ID", "NAME", "PID", "MENULEVEL" };
                string[] value = { prtID, txtMenu.Text, listPrt.SelectedValue, listLevel.SelectedValue };
                
                string log_message = opt.MergeInto(seg, value, 1,"HT_SVR_PRT_MENU") == "Success" ? "保存父级菜单成功" : "保存父级菜单失败";
                log_message += "--详情:" + string.Join(",", value);
                InsertTlog(log_message);
                BindList(RightTree.Nodes, "");
            }
    
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
       
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            string query = "delete from HT_SVR_PRT_MENU where NAME = '" + txtMenu.Text + "'";
            string log_message = opt.UpDateOra(query) == "Success" ? "删除父级菜单成功" : "删除父级菜单失败";
            log_message += "--标识:" + txtMenu.Text;
            InsertTlog(log_message);
            BindList(RightTree.Nodes, "");
            SetBlank();
     
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        string id = GridView1.DataKeys[rowIndex].Value.ToString();

        GridViewRow row = GridView1.Rows[rowIndex];
        string mapID, RightID;
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        ///在映射表中插入URL映射
        if ("NoRecord" == (mapID = opt.GetSegValue("select * from ht_inner_map where URL = '" + ((TextBox)row.FindControl("txtURL")).Text + "'", "MAPID")))
        {
            mapID = id;
            
            opt.MergeInto(new string[] { "MAPID", "URL" }, new string[] { mapID, ((TextBox)row.FindControl("txtURL")).Text }, 1,"HT_INNER_MAP");
        }
        //在权限表中插入菜单权限 
        if ("NoRecord" == (RightID = opt.GetSegValue("select * from ht_svr_sys_menu where F_MENU = '" + ((TextBox)row.FindControl("txtMenu")).Text + "'", "F_ID")))
        {
            RightID = (Convert.ToInt16(opt.GetSegValue("select Max(F_ID) as ID from ht_svr_sys_menu ", "ID")) + 1).ToString().PadLeft(5, '0');
        }
        
        string[] seg = { "F_ID", "F_MENU", "F_MAPID", "F_PID", "F_DESCRIPT", "F_TYPE" };
        string[] value = { RightID, ((TextBox)row.FindControl("txtMenu")).Text, mapID, ((DropDownList)row.FindControl("listPrt")).SelectedValue, ((TextBox)row.FindControl("txtDscrp")).Text, "0" };
       
        //插入操作权限
        if("Success" == opt.MergeInto(seg, value,1, "ht_svr_sys_menu"))
        value[0] = (Convert.ToInt16(RightID) + 1).ToString().PadLeft(5, '0');        
        value[5] = "1";
         if(value[1] == "")
             value[1] = "子框架" + ((DropDownList)row.FindControl("listPrt")).SelectedItem.Text;
       
        string log_message =  opt.InsertData(seg, value, "ht_svr_sys_menu")== "Success" ? "插入操作权限成功" : "插入操作权限失败";
        log_message += "--详情:" + string.Join(",", value);
        InsertTlog(log_message);
         
        bindData();


    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        string id = GridView1.DataKeys[rowIndex].Value.ToString();
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        List<string> commandlist = new List<string>();
        commandlist.Add("update  ht_inner_map  set is_del = '1' where MAPID = '" + id + "'");
        commandlist.Add("update ht_svr_sys_menu set IS_DEL = '1' where F_MAPID = '" + id + "'");
        string log_message = opt.TransactionCommand(commandlist) == "Success" ? "删除Map表映射成功" : "删除Map表映射失败";
        log_message += "--标识:" + id;
        InsertTlog(log_message);
        bindData();
    }


    protected DataSet bindprt()
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        return opt.CreateDataSetOra("select NAME,ID from ht_svr_prt_menu where IS_DEL = '0' union select '' as Name,'' as ID from dual  order by ID");
    }




    protected void SetBlank()
    {
        txtMenu.Text = "";
        listLevel.SelectedValue = "";
        listPrt.SelectedValue = "";
    }


}