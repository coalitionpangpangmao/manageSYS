﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
public partial class Device_StorageParts : MSYS.Web.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageLoad(sender, e);
        if (!IsPostBack)
        {
            txtStart.Text = System.DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            txtStop.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
           MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
            opt.bindDropDownList(listApts, "select F_CODE,F_NAME from ht_svr_org_group ", "F_NAME", "F_CODE");
            opt.bindDropDownList(listApt, "select F_CODE,F_NAME from ht_svr_org_group", "F_NAME", "F_CODE");
            opt.bindDropDownList(listEditor, "select s.name,s.id from ht_svr_sys_role t left join ht_svr_sys_menu r on substr(t.f_right,r.f_id,1) = '1' left join ht_svr_user s on s.role = t.f_id where r.f_id = '" + this.RightId + "' union select q.name,q.id from ht_svr_sys_role t left join ht_svr_sys_menu r on substr(t.f_right,r.f_id,1) = '1' left join ht_svr_org_group  s on s.f_role = t.f_id  left join ht_svr_user q on q.levelgroupid = s.f_code  where r.f_id = '" + this.RightId + "'  order by id desc", "NAME", "ID");
           
            
            bindGrid1();
           
        }
 
    }

    /// <summary>
    /// tab1操作
    /// </summary>
    protected void bindGrid1()
    {
    
            string query = "select g2.f_name as 申请部门,g1.name  as 申请人,case g.is_pickup when '0' then '未领用' else '己领用' end  as 领用状态,g.pickup_date as 领用时间,g.remark  as 备注,(case g.Flow_Status when '-1' then '未提交' when '0' then '办理中' when '1' then '未通过' else '己通过' end)  as 审批状态,g.PZ_CODE from ht_eq_STG_PICKUP g left join ht_svr_user g1 on g.create_id = g1.id left join ht_svr_org_group g2 on g.create_dept_id = g2.f_code where g.is_del = '0'";
            if (txtStart.Text != "" && txtStop.Text != "")
                query += " and g.pickup_date between '" + txtStart.Text + "' and '" + txtStop.Text + "'";
            if (listApts.SelectedValue != "")
                query += " and g.create_dept_id = '" + listApts.SelectedValue + "'";               
           MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
            GridView1.DataSource = opt.CreateDataSetOra(query); ;
            GridView1.DataBind();
      
    }//绑定gridview1数据源
    protected void btnSearch1_Click(object sender, EventArgs e)
    {
        bindGrid1();
    }

    protected void btnGridDel_Click(object sender, EventArgs e)//删除选中记录
    {
        try
        {
            List<string> commandlist = new List<string>();
              MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                if (((CheckBox)GridView1.Rows[i].FindControl("chk")).Checked)
                {
                    commandlist.Clear();
                    string id = GridView1.DataKeys[i].Value.ToString();

                    commandlist.Add( "update HT_EQ_STG_PICKUP set IS_DEL = '1'  where PZ_CODE = '" + id + "'");
                    commandlist.Add("update HT_EQ_STG_PICKUP_detail set IS_DEL = '1'  where MAIN_ID = '" + id + "'");
                    commandlist.Add("delete from HT_PUB_APRV_FLOWINFO where BUSIN_ID = '" + id + "'");

                    string log_message = opt.TransactionCommand(commandlist) == "Success" ? "删除备品备件领退计划成功" : "删除备品备件领退计划失败";
                    log_message += "--标识:" + id;
                    InsertTlog(log_message);
                }
            }
            bindGrid1();
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    } 

    protected void btnGridIssue_Click(object sender, EventArgs e)//查看审批流程
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        string ID = GridView1.DataKeys[rowIndex].Value.ToString();
        string query = "select pos as 顺序号, workitemid as 审批环节,username as 负责人,comments as 意见,opiniontime 审批时间,(case status when '0' then '未审批'  when '1' then '未通过' else '己通过' end)  as 审批状态  from ht_pub_aprv_opinion r left join ht_pub_aprv_flowinfo s on r.gongwen_id = s.id where s.busin_id  = '" + ID + "' order by pos";
       MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
        GridView3.DataSource = opt.CreateDataSetOra(query);
        GridView3.DataBind();
       
    }

    protected void btnSubmit_Click(object sender, EventArgs e)//提交审批
    {
        try
        {
            Button btn = (Button)sender;
            int index = ((GridViewRow)btn.NamingContainer).RowIndex;//获得行号                 
            string id = GridView1.DataKeys[index].Value.ToString();
            /*启动审批TB_ZT标题,TBR_ID填报人id,TBR_NAME填报人name,TB_BM_ID填报部门id,TB_BM_NAME填报部门name,TB_DATE申请时间创建日期,MODULENAME审批类型编码,URL 单独登录url,BUSIN_ID业务数据id*/
            string[] subvalue = { GridView1.Rows[index].Cells[4].Text+ "备件领用", "13", id, Page.Request.UserHostName.ToString() };
           MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
           string log_message = MSYS.AprvFlow.createApproval(subvalue) ? "提交审批成功," : "提交审批失败，";
           log_message += ",业务数据ID：" + id;
           InsertTlog(log_message);


        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }

    protected void btnGridNew_Click(object sender, EventArgs e)// 新增领退明细
    {

        setBlank();
       MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
        txtPzcode.Text = "PK" + System.DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt16(opt.GetSegValue("select nvl(max(substr(PZ_CODE,3,11)),0) as ordernum from HT_EQ_STG_PICKUP where substr(PZ_CODE,1,10) ='PK" + System.DateTime.Now.ToString("yyyyMMdd") + "'", "ordernum")) + 1).ToString().PadLeft(3, '0');

      
    }

    protected void btnGridopt_Click(object sender, EventArgs e)//领用，调用接口，变更库存情况
    {
        try
        {
            Button btn = (Button)sender;
            int index = ((GridViewRow)btn.NamingContainer).RowIndex;//获得行号                 
            string id = GridView1.DataKeys[index].Value.ToString();
           MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
           List<string> commandlist = new List<string>();
           
            commandlist.Add("update HT_EQ_STG_PICKUP set IS_PICKUP = '1'  where PZ_CODE = '" + id + "'");
            commandlist.Add("delete from HT_PUB_APRV_FLOWINFO where BUSIN_ID = '" + id + "'");
            string log_message = opt.TransactionCommand(commandlist) == "Success" ? "删除备品备件领退计划成功" : "删除备品备件领退计划失败";
            log_message += "--标识:" + id;
            InsertTlog(log_message);
            bindGrid1();
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }

    protected void btnGridview_Click(object sender, EventArgs e)//查看领退明细
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        txtPzcode.Text = GridView1.DataKeys[rowIndex].Value.ToString();

       MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra("select * from HT_EQ_STG_PICKUP  where PZ_CODE =  '" + txtCode.Text + "'");
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            listApt.SelectedValue = data.Tables[0].Rows[0]["CREATE_DEPT_ID"].ToString();
            listEditor.SelectedValue = data.Tables[0].Rows[0]["CREATE_ID"].ToString();
            txtTime.Text = data.Tables[0].Rows[0]["PICKUP_DATE"].ToString();
            txtRemark.Text = data.Tables[0].Rows[0]["REMARK"].ToString();
           
            bindGrid2();
        }
       
    }    

    /// <summary>
    /// tab2操作
    /// </summary>
    protected void bindGrid2()
    {

        string query = "select t.storage as 仓库,t.Sp_Code as 编码,t.sp_name as 名称,t.sp_standard as 规格,t.sp_model as 型号,t.sp_unit as 单位,t.pickup_num as 数量,t.own_section as 工艺段,t.remark as 备注,t.ID from ht_eq_stg_pickup_detail t  where MAIN_CODE = '" + txtPzcode.Text + "' and IS_DEL = '0'";
       
       MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra(query);
        GridView2.DataSource = data;
        GridView2.DataBind();
        if (data != null && data.Tables[0].Rows.Count > 0)
        {          
            for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
            {
                DataRowView mydrv = data.Tables[0].DefaultView[i];
                ((DropDownList)GridView2.Rows[i].FindControl("listGridstrg")).SelectedValue = mydrv["仓库"].ToString();
                ((DropDownList)GridView2.Rows[i].FindControl("listGridName")).SelectedValue = mydrv["名称"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridCode")).Text = mydrv["编码"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridSpec")).Text = mydrv["规格"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridModel")).Text = mydrv["型号"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridUnit")).Text = mydrv["单位"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridAmount")).Text = mydrv["数量"].ToString();
                ((DropDownList)GridView2.Rows[i].FindControl("listGridSection")).SelectedValue = mydrv["工艺段"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridDscprt")).Text = mydrv["备注"].ToString();
                 
            }

        }

    }//绑定GridView2数据源


    protected DataSet gridTypebind()
    {
       MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
        return opt.CreateDataSetOra("select material_code,material_name from ht_pub_materiel  where is_valid = '1'  and is_del = '0' and TYPE_FLAG = 'BJ'");
    }
    protected DataSet sectionbind()
    {
       MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
        return opt.CreateDataSetOra("select section_code,section_name from ht_pub_tech_section where is_del= '0'");
    }
     protected void btnReset_Click(object sender, EventArgs e)
     {
         setBlank();
     }
     protected void setBlank()
     {
         txtPzcode.Text = "";
         listApt.SelectedValue = "";
         listEditor.SelectedValue = "";
         txtTime.Text = "";
         txtRemark.Text = "";
     }

     protected void btnModify_Click(object sender, EventArgs e)
     {
        MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
               
             //生成领用主表记录
         string[] seg = { "PZ_CODE", "CREATE_DEPT_ID", "CREATE_ID", "PICKUP_DATE", "REMARK"};
         string[] value = { txtPzcode.Text, listApt.SelectedValue, listEditor.SelectedValue, txtTime.Text, txtRemark.Text};

         string log_message = opt.MergeInto(seg, value, 1, "HT_EQ_STG_PICKUP") == "Success" ? "生成备品备件领用主表记录成功" : "生成备品备件领用主表记录失败";
         log_message += "--详情:" + string.Join(",", value);
         InsertTlog(log_message);
         bindGrid1();
         bindGrid2();

     }
      protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "select t.storage as 仓库,t.Sp_Code as 编码,t.sp_name as 名称,t.sp_standard as 规格,t.sp_model as 型号,t.sp_unit as 单位,t.pickup_num as 数量,t.own_section as 工艺段,t.remark as 备注,t.ID from ht_eq_stg_pickup_detail t  where MAIN_CODE = '" + txtPzcode.Text + "' and IS_DEL = '0'";
          
           MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
            DataSet set = opt.CreateDataSetOra(query);
            DataTable data = new DataTable();
            if (set != null && set.Tables[0].Rows.Count >0)
                data = set.Tables[0];
           
            else
            {
                data.Columns.Add("仓库");
                data.Columns.Add("编码");
                data.Columns.Add("名称");
                data.Columns.Add("规格");
                data.Columns.Add("型号");
                data.Columns.Add("单位");
                data.Columns.Add("数量");
                data.Columns.Add("工艺段");
                data.Columns.Add("备注");
                data.Columns.Add("ID");
            }
            object[] value = { "", "", "","","", "",0,"","",0 };
            data.Rows.Add(value);
            GridView2.DataSource = data;
            GridView2.DataBind();
            if (data != null && data.Rows.Count > 0)
            {
                 for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
            {
                DataRowView mydrv = data.DefaultView[i];
                ((DropDownList)GridView2.Rows[i].FindControl("listGridstrg")).SelectedValue = mydrv["仓库"].ToString();
                ((DropDownList)GridView2.Rows[i].FindControl("listGridName")).SelectedValue = mydrv["名称"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridCode")).Text = mydrv["编码"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridSpec")).Text = mydrv["规格"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridModel")).Text = mydrv["型号"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridUnit")).Text = mydrv["单位"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridAmount")).Text = mydrv["数量"].ToString();
                ((DropDownList)GridView2.Rows[i].FindControl("listGridSection")).SelectedValue = mydrv["工艺段"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridDscprt")).Text = mydrv["备注"].ToString();
               
            }

            }
        }
        catch (Exception ee)
        {
            string str = ee.Message;
        }
    }
      protected void btnCkAll_Click(object sender, EventArgs e)//全选
    {
        int ckno = 0;
        for (int i = 0; i < GridView2.Rows.Count; i++)
        {
            if (((CheckBox)GridView2.Rows[i].FindControl("chk")).Checked)
                ckno++;
        }
        bool check = (ckno < GridView2.Rows.Count);
        for (int i = 0; i < GridView2.Rows.Count; i++)
        {
            ((CheckBox)GridView2.Rows[i].FindControl("chk")).Checked = check;

        }
    }
    protected void btnDelSel_Click(object sender, EventArgs e)//删除选中记录
    {
        try
        {
            for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
            {
                if (((CheckBox)GridView2.Rows[i].FindControl("chk")).Checked)
                {
                    string ID = GridView2.DataKeys[i].Value.ToString();
                    string query = "update HT_EQ_STG_PICKUP_DETAIL set IS_DEL = '1'  where ID = '" + ID + "'";
                   MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
                   string log_message = opt.UpDateOra(query) == "Success" ? "删除备品备件明细成功" : "删除备品备件明细失败";
                   log_message += "--标识:" + ID ;
                   InsertTlog(log_message);
                }
            }
            bindGrid2();
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
 
    protected void btnGrid2Save_Click(object sender, EventArgs e)
    {
        try
        {
           MSYS.DAL.DbOperator opt =new MSYS.DAL.DbOperator();
            if (txtPzcode.Text == "")
                txtPzcode.Text = "PK" + System.DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt16(opt.GetSegValue("select nvl(max(substr(PZ_CODE,3,11)),0) as ordernum from HT_EQ_STG_PICKUP where substr(PZ_CODE,1,10) ='PK" + System.DateTime.Now.ToString("yyyyMMdd") + "'", "ordernum")) + 1).ToString().PadLeft(3, '0');
            Button btn = (Button)sender;
            int Rowindex = ((GridViewRow)btn.NamingContainer).RowIndex;//获得行号             
            string ID = GridView2.DataKeys[Rowindex].Value.ToString();
           if (ID == "0")
            {
                ID = opt.GetSegValue("select sub_pickup_id_seq.nextval as id from dual", "ID");               
            }
            string[] seg = {"ID","MAIN_CODE", "STORAGE", "Sp_Code", "sp_name", "sp_standard", "sp_model", "sp_unit", "pickup_num", "own_section", "remark" };
            string[] value = { ID,txtPzcode.Text, ((DropDownList)GridView2.Rows[Rowindex].FindControl("listGridstrg")).SelectedValue, ((TextBox)GridView2.Rows[Rowindex].FindControl("txtGridCode")).Text, ((DropDownList)GridView2.Rows[Rowindex].FindControl("listGridName")).SelectedItem.Text, ((TextBox)GridView2.Rows[Rowindex].FindControl("txtGridSpec")).Text, ((TextBox)GridView2.Rows[Rowindex].FindControl("txtGridModel")).Text, ((TextBox)GridView2.Rows[Rowindex].FindControl("txtGridUnit")).Text, ((TextBox)GridView2.Rows[Rowindex].FindControl("txtGridAmount")).Text, ((DropDownList)GridView2.Rows[Rowindex].FindControl("listGridSection")).SelectedValue, ((TextBox)GridView2.Rows[Rowindex].FindControl("txtGridDscprt")).Text };
             string log_message = opt.MergeInto(seg, value, 1, "HT_EQ_STG_PICKUP_DETAIL") == "Success" ? "保存备品备件领退明细成功" : "保存备品备件领退明细失败";
            log_message += "--详情:" + string.Join(",", value);
            InsertTlog(log_message);   
           
            bindGrid2();
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
   
    ////////////////////////////////////
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
    }

}