﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
public partial class Device_LbrctPlan : MSYS.Web.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageLoad(sender, e);
        if (!IsPostBack)
        {
            txtStart.Text = System.DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            txtStop.Text = System.DateTime.Now.AddDays(45).ToString("yyyy-MM-dd");
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            opt.bindDropDownList(listEditor, "select s.name,s.id from ht_svr_sys_role t left join ht_svr_sys_menu r on substr(t.f_right,r.f_id,1) = '1' left join ht_svr_user s on s.role = t.f_id where r.f_id = '" + this.RightId + "' union select q.name,q.id from ht_svr_sys_role t left join ht_svr_sys_menu r on substr(t.f_right,r.f_id,1) = '1' left join ht_svr_org_group  s on s.f_role = t.f_id  left join ht_svr_user q on q.levelgroupid = s.f_code  where r.f_id = '" + this.RightId + "'  order by id desc", "name", "ID");
            opt.bindDropDownList(listApt, "select f_code,f_name  from ht_svr_org_group ", "f_name", "f_code");
            opt.bindDropDownList(listModel, "select pz_code,mt_name from ht_eq_lb_plan where is_model = '1' and is_del = '0' and FLOW_STATUS = '2'", "mt_name", "pz_code");
            opt.bindDropDownList(listdspcth, "select ID,name  from ht_svr_user t where is_del ='0' and  t.levelgroupid = '00700700' union select '' as ID,'' as Name from dual ", "name", "ID");
            bindGrid1();
            bindGrid4();

        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindGrid1();
        bindGrid4();
    }
    protected void bindGrid1()
    {

        string query = "select t.mt_name as 润滑计划,t1.f_name as 部门, t2.name as 审批状态,t3.name as 执行状态,t.remark as 备注,t.pz_code from ht_eq_lb_plan t left join ht_svr_org_group t1 on t1.f_code = t.create_dept_id    left join ht_inner_aprv_status t2 on t2.id = t.flow_status left join ht_inner_eqexe_status t3 on t3.id = t.task_status  where t.expired_date between '" + txtStart.Text + "' and '" + txtStop.Text + "'  and t.IS_DEL = '0'   and t.is_model = '0' ";

        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra(query);
        GridView1.DataSource = data;
        GridView1.DataBind();
        if (data != null && data.Tables[0].Rows.Count > 0)
        {            
            for (int i = GridView1.PageSize * GridView1.PageIndex; i < GridView1.PageSize * (GridView1.PageIndex + 1) && i < data.Tables[0].Rows.Count; i++)
            {
                int j = i - GridView1.PageSize * GridView1.PageIndex;
                DataRowView mydrv = data.Tables[0].DefaultView[i];
                GridViewRow row = GridView1.Rows[j];
                ((Label)row.FindControl("labAprv")).Text = mydrv["审批状态"].ToString();

                ((Label)row.FindControl("labexe")).Text = mydrv["执行状态"].ToString();
                if (!(mydrv["审批状态"].ToString() == "未提交" || mydrv["审批状态"].ToString() == "未通过"))
                   {
                       ((Button)row.FindControl("btnSubmit")).Enabled = false;
                       ((Button)row.FindControl("btnSubmit")).CssClass = "btngrey";
                       ((Button)row.FindControl("btnGridview")).Text = "查看计划";
                   }
                   else
                   {
                       ((Button)row.FindControl("btnSubmit")).Enabled = true;
                       ((Button)row.FindControl("btnSubmit")).CssClass = "btn1 auth";
                       ((Button)row.FindControl("btnGridview")).Text = "编制计划";
                   }

                   i++;
            }
        }


    }//绑定gridview1数据源
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (e.NewPageIndex == -3)
        {
            //点击跳转按钮
            TextBox txtNewPageIndex = null;

            //GridView较DataGrid提供了更多的API，获取分页块可以使用BottomPagerRow 或者TopPagerRow，当然还增加了HeaderRow和FooterRow
            GridViewRow pagerRow = theGrid.BottomPagerRow;

            if (pagerRow != null)
            {
                //得到text控件
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (txtNewPageIndex != null)
            {
                //得到索引
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        {
            //点击了其他的按钮
            newPageIndex = e.NewPageIndex;
        }
        //防止新索引溢出
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        //得到新的值
        theGrid.PageIndex = newPageIndex;
        //重新绑定

        bindGrid1();
    }  

    protected void btnGridDel_Click(object sender, EventArgs e)//删除选中记录
    {
        try
        {
            List<string> commandlist = new List<string>();
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                if (((CheckBox)GridView1.Rows[i].FindControl("chk")).Checked)
                {
                    commandlist.Clear();
                    string order_sn = GridView1.DataKeys[i].Value.ToString();
                    commandlist.Add("update ht_eq_lb_plan set IS_DEL = '1'  where PZ_CODE = '" + order_sn + "'");
                    commandlist.Add("update ht_eq_lb_plan_detail set IS_DEL = '1'  where MAIN_ID = '" + order_sn + "'");
                    commandlist.Add("delete from HT_PUB_APRV_FLOWINFO where BUSIN_ID = '" + order_sn + "'");
                    MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
                    string log_message = opt.TransactionCommand(commandlist) == "Success" ? "删除润滑计划成功" : "删除润滑计划失败";
                    log_message += "--标识:" + order_sn;
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
    protected void btnGridNew_Click(object sender, EventArgs e)
    {
        setBlank();
        SetEnable("未提交");
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        txtCode.Text = "LB" + System.DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt16(opt.GetSegValue("select nvl( max(substr(pz_code,11,3)),0) as ordernum from ht_eq_lb_plan where substr(pz_code,1,10) ='LB" + System.DateTime.Now.ToString("yyyyMMdd") + "'", "ordernum")) + 1).ToString().PadLeft(3, '0');
        MSYS.Data.SysUser user = (MSYS.Data.SysUser)Session["User"];
        listEditor.SelectedValue = user.id;
        listApt.SelectedValue = user.OwningBusinessUnitId;
        bindGrid2("");
       
    }
    protected void btnGridIssue_Click(object sender, EventArgs e)//查看审批流程
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        string ID = GridView1.DataKeys[rowIndex].Value.ToString();
        string query = "select pos as 顺序号, workitemid as 审批环节,username as 负责人,comments as 意见,opiniontime 审批时间,(case status when '0' then '未审批'  when '1' then '未通过' else '己通过' end)  as 审批状态  from ht_pub_aprv_opinion r left join ht_pub_aprv_flowinfo s on r.gongwen_id = s.id where s.busin_id  = '" + ID + "' order by pos";
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        GridView3.DataSource = opt.CreateDataSetOra(query);
        GridView3.DataBind();
        ScriptManager.RegisterStartupScript(UpdatePanel4, this.Page.GetType(), "", " $('#flowinfo').fadeIn(200);", true);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)//提交审批
    {
        try
        {
            Button btn = (Button)sender;
            int index = ((GridViewRow)btn.NamingContainer).RowIndex;//获得行号                 
            string id = GridView1.DataKeys[index].Value.ToString();
            /*启动审批TB_ZT标题,TBR_ID填报人id,TBR_NAME填报人name,TB_BM_ID填报部门id,TB_BM_NAME填报部门name,TB_DATE申请时间创建日期,MODULENAME审批类型编码,URL 单独登录url,BUSIN_ID业务数据id*/
            string[] subvalue = { GridView1.Rows[index].Cells[1].Text, "16", id, Page.Request.UserHostName.ToString() };
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            string log_message = MSYS.AprvFlow.createApproval(subvalue) ? "提交审批成功," : "提交审批失败，";
            log_message += ",业务数据ID：" + id;
            InsertTlog(log_message);
            bindGrid1();

        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnGridview_Click(object sender, EventArgs e)//查看明细
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        txtCode.Text = GridView1.DataKeys[rowIndex].Value.ToString();

        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra("select * from ht_eq_lb_plan  where PZ_CODE =  '" + txtCode.Text + "'");
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            DataRow row = data.Tables[0].Rows[0];
            txtName.Text = row["MT_NAME"].ToString();
            listEditor.SelectedValue = row["CREATE_ID"].ToString();
            listApt.SelectedValue = row["CREATE_DEPT_ID"].ToString();
            txtExptime.Text = row["EXPIRED_DATE"].ToString();
            txtdscrpt.Text = row["REMARK"].ToString();
            ckModel.Checked = ("1" == row["IS_MODEL"].ToString());
              bindGrid2(txtCode.Text);
              string aprvstatus = ((Label)GridView1.Rows[rowIndex].FindControl("labAprv")).Text;
            SetEnable(aprvstatus);
         }
       
    }

    protected void bindGrid4()
    {

        string query = "select t.mt_name as 润滑计划,t1.f_name as 部门, t2.name as 审批状态,t.remark as 备注,t.pz_code from ht_eq_lb_plan t left join ht_svr_org_group t1 on t1.f_code = t.create_dept_id    left join ht_inner_aprv_status t2 on t2.id = t.flow_status  where  t.IS_DEL = '0'   and t.is_model = '1' ";

        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra(query);
        GridView4.DataSource = data;
        GridView4.DataBind();
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            for (int i = GridView4.PageSize * GridView4.PageIndex; i < GridView4.PageSize * (GridView4.PageIndex + 1) && i < data.Tables[0].Rows.Count; i++)
            {
                int j = i - GridView4.PageSize * GridView4.PageIndex;
                DataRowView mydrv = data.Tables[0].DefaultView[i];
                GridViewRow row = GridView4.Rows[j];
                ((Label)row.FindControl("labAprv")).Text = mydrv["审批状态"].ToString();
                if (!(mydrv["审批状态"].ToString() == "未提交" || mydrv["审批状态"].ToString() == "未通过"))
                {
                    ((Button)row.FindControl("btnSubmit4")).Enabled = false;
                    ((Button)row.FindControl("btnSubmit4")).CssClass = "btngrey";
                    ((Button)row.FindControl("btnGridview4")).Text = "查看计划";
                }
                else
                {
                    ((Button)row.FindControl("btnSubmit4")).Enabled = true;
                    ((Button)row.FindControl("btnSubmit4")).CssClass = "btn1 auth";
                    ((Button)row.FindControl("btnGridview4")).Text = "编制计划";
                }

                i++;
            }
        }


    }//绑定GridView4数据源
    protected void GridView4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (e.NewPageIndex == -3)
        {
            //点击跳转按钮
            TextBox txtNewPageIndex = null;

            //GridView较DataGrid提供了更多的API，获取分页块可以使用BottomPagerRow 或者TopPagerRow，当然还增加了HeaderRow和FooterRow
            GridViewRow pagerRow = theGrid.BottomPagerRow;

            if (pagerRow != null)
            {
                //得到text控件
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (txtNewPageIndex != null)
            {
                //得到索引
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        {
            //点击了其他的按钮
            newPageIndex = e.NewPageIndex;
        }
        //防止新索引溢出
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        //得到新的值
        theGrid.PageIndex = newPageIndex;
        //重新绑定

        bindGrid4();
    }

    protected void btnGridDel4_Click(object sender, EventArgs e)//删除选中记录
    {
        try
        {
            List<string> commandlist = new List<string>();
            for (int i = 0; i <= GridView4.Rows.Count - 1; i++)
            {
                if (((CheckBox)GridView4.Rows[i].FindControl("chk")).Checked)
                {
                    commandlist.Clear();
                    string order_sn = GridView4.DataKeys[i].Value.ToString();
                    commandlist.Add("update ht_eq_lb_plan set IS_DEL = '1'  where PZ_CODE = '" + order_sn + "'");
                    commandlist.Add("update ht_eq_lb_plan_detail set IS_DEL = '1'  where MAIN_ID = '" + order_sn + "'");
                    commandlist.Add("delete from HT_PUB_APRV_FLOWINFO where BUSIN_ID = '" + order_sn + "'");
                    MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
                    string log_message = opt.TransactionCommand(commandlist) == "Success" ? "删除润滑计划模版成功" : "删除润滑计划模版失败";
                    log_message += "--标识:" + order_sn;
                    InsertTlog(log_message);
                }
            }
            bindGrid4();
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnGridNew4_Click(object sender, EventArgs e)
    {
        setBlank();
        SetEnable("未提交");
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        txtCode.Text = "LB" + System.DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt16(opt.GetSegValue("select nvl( max(substr(pz_code,11,3)),0) as ordernum from ht_eq_lb_plan where substr(pz_code,1,10) ='LB" + System.DateTime.Now.ToString("yyyyMMdd") + "'", "ordernum")) + 1).ToString().PadLeft(3, '0');
        MSYS.Data.SysUser user = (MSYS.Data.SysUser)Session["User"];
        listEditor.SelectedValue = user.id;
        listApt.SelectedValue = user.OwningBusinessUnitId;
        bindGrid2("");
      
    }
    protected void btnGridIssue4_Click(object sender, EventArgs e)//查看审批流程
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        string ID = GridView4.DataKeys[rowIndex].Value.ToString();
        string query = "select pos as 顺序号, workitemid as 审批环节,username as 负责人,comments as 意见,opiniontime 审批时间,(case status when '0' then '未审批'  when '1' then '未通过' else '己通过' end)  as 审批状态  from ht_pub_aprv_opinion r left join ht_pub_aprv_flowinfo s on r.gongwen_id = s.id where s.busin_id  = '" + ID + "' order by pos";
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        GridView3.DataSource = opt.CreateDataSetOra(query);
        GridView3.DataBind();
        ScriptManager.RegisterStartupScript(UpdatePanel4, this.Page.GetType(), "", " $('#flowinfo').fadeIn(200);", true);
    }
    protected void btnSubmit4_Click(object sender, EventArgs e)//提交审批
    {
        try
        {
            Button btn = (Button)sender;
            int index = ((GridViewRow)btn.NamingContainer).RowIndex;//获得行号                 
            string id = GridView4.DataKeys[index].Value.ToString();
            /*启动审批TB_ZT标题,TBR_ID填报人id,TBR_NAME填报人name,TB_BM_ID填报部门id,TB_BM_NAME填报部门name,TB_DATE申请时间创建日期,MODULENAME审批类型编码,URL 单独登录url,BUSIN_ID业务数据id*/
            string[] subvalue = { GridView4.Rows[index].Cells[1].Text, "16", id, Page.Request.UserHostName.ToString() };
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            string log_message = MSYS.AprvFlow.createApproval(subvalue) ? "提交审批成功," : "提交审批失败，";
            log_message += ",业务数据ID：" + id;
            InsertTlog(log_message);
            bindGrid4();

        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnGridview4_Click(object sender, EventArgs e)//查看明细
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        txtCode.Text = GridView4.DataKeys[rowIndex].Value.ToString();

        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra("select * from ht_eq_lb_plan  where PZ_CODE =  '" + txtCode.Text + "'");
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            DataRow row = data.Tables[0].Rows[0];
            txtName.Text = row["MT_NAME"].ToString();
            listEditor.SelectedValue = row["CREATE_ID"].ToString();
            listApt.SelectedValue = row["CREATE_DEPT_ID"].ToString();
            txtExptime.Text = row["EXPIRED_DATE"].ToString();
            txtdscrpt.Text = row["REMARK"].ToString();
            ckModel.Checked = ("1" == row["IS_MODEL"].ToString());
            bindGrid2(txtCode.Text);
            string aprvstatus = ((Label)GridView4.Rows[rowIndex].FindControl("labAprv")).Text;
            SetEnable(aprvstatus);
        }
       
    }   
    protected void SetEnable(string aprvstatus)
    {
        btnDispatch.Visible = false;
        btnTrack.Visible = false;
        btnDone.Visible = false;
        if (aprvstatus == "未提交")
        {
            btnSave.Visible = true;
            btnAdd.Visible = true;
            btnDelSel.Visible = true;
            if (GridView2.Columns.Count == 9)
            {
                GridView2.Columns[7].Visible = true;
                GridView2.Columns[8].Visible = false;
            }
        }
        else
        {
            btnSave.Visible = false;
            btnAdd.Visible = false;
            btnDelSel.Visible = false;
            if (GridView2.Columns.Count == 9)
            {
                GridView2.Columns[7].Visible = false;
                GridView2.Columns[8].Visible = true;
            }
            if (aprvstatus == "己通过")
            {
                btnDispatch.Visible = true;
                btnTrack.Visible = true;
                btnDone.Visible = true;
            }
        }
    }


    /// <summary>
    /// /tab2
    /// </summary>
    protected void setBlank()
    {
        txtName.Text = "";
        txtCode.Text = "";
        listEditor.SelectedValue = "";
        listApt.SelectedValue = "";
        txtExptime.Text = "";
        txtdscrpt.Text = "";
        ckModel.Checked = false;
    }
    protected void bindGrid2(string code)
    {

        string query = "select t.section as  工段,t.equipment_id as  设备名称,t.STATUS as 状态,t.EXP_FINISH_TIME as 期望完成时间,  t.r.name as 执行人,t.remark as  备注,t.ID from ht_eq_lb_plan_detail t  left join ht_svr_user r on r.id = t.responer   where t.main_id = '" + code + "' and t.is_del = '0'";

        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra(query);
        GridView2.DataSource = data;
        GridView2.DataBind();
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
            {
                DataRowView mydrv = data.Tables[0].DefaultView[i];
                GridViewRow row = GridView2.Rows[i];
                ((DropDownList)row.FindControl("listGridsct")).SelectedValue = mydrv["工段"].ToString();
                DropDownList list = (DropDownList)row.FindControl("listGridEq");
                opt.bindDropDownList(list, "select IDKEY,EQ_NAME  from ht_eq_eqp_tbl where section_code = '" + mydrv["工段"].ToString() + "'", "EQ_NAME", "IDKEY");
                list.SelectedValue = mydrv["设备名称"].ToString();
                ((DropDownList)row.FindControl("listGrid2Status")).SelectedValue = mydrv["状态"].ToString();
                ((TextBox)row.FindControl("txtGridExptime")).Text = mydrv["期望完成时间"].ToString();
                ((TextBox)row.FindControl("txtGridremark")).Text = mydrv["备注"].ToString();

            }

        }

    }//绑定GridView2数据源
    protected DataSet statusbind()
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        return opt.CreateDataSetOra("select ID, Name from ht_inner_eqexe_status order by ID ");
    }
    protected DataSet sectionbind()
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        return opt.CreateDataSetOra("select section_code,section_name from ht_pub_tech_section where is_del = '0' and is_valid = '1' union select '' as section_code,'' as section_name from dual order by section_code");
    }


    protected void listGridsct_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList list = (DropDownList)sender;
        GridViewRow row = (GridViewRow)list.NamingContainer;
        int rowindex = row.RowIndex;
        DropDownList list1 = (DropDownList)row.FindControl("listGridEq");
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        opt.bindDropDownList(list1, "select IDKEY,EQ_NAME  from ht_eq_eqp_tbl where section_code = '" + list.SelectedValue + "'  order by idkey", "EQ_NAME", "IDKEY");
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
                    string query = "update ht_eq_lb_plan_detail set IS_DEL = '1'  where ID = '" + ID + "'";
                    MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
                    string log_message = opt.UpDateOra(query) == "Success" ? "删除润滑计划明细成功" : "删除润滑计划明细失败";
                    log_message += "--标识:" + ID;
                    InsertTlog(log_message);
                }
            }
            bindGrid2(txtCode.Text);
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)//
    {
        try
        {
            string query = "select t.section as  工段,t.equipment_id as  设备名称,t.STATUS as 状态,t.EXP_FINISH_TIME as 期望完成时间,  t.r.name as 执行人,t.remark as  备注,t.ID from ht_eq_lb_plan_detail t  left join ht_svr_user r on r.id = t.responer   where t.main_id = '" + txtCode.Text + "' and t.is_del = '0'";
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            DataSet set = opt.CreateDataSetOra(query);
            DataTable data = new DataTable();
            if (set == null)
            {
                data.Columns.Add("工段");
                data.Columns.Add("设备名称");
                data.Columns.Add("期望完成时间");
                data.Columns.Add("状态");
                data.Columns.Add("执行人");
                data.Columns.Add("备注");
                data.Columns.Add("ID");
            }
            else
                data = set.Tables[0];
            object[] value = { "", "", txtExptime.Text, "","", "", 0 };
            data.Rows.Add(value);
            GridView2.DataSource = data;
            GridView2.DataBind();
            if (data != null && data.Rows.Count > 0)
            {
                for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
                {
                    DataRowView mydrv = data.DefaultView[i];
                    GridViewRow row = GridView2.Rows[i];
                    ((DropDownList)row.FindControl("listGridsct")).SelectedValue = mydrv["工段"].ToString();
                    DropDownList list = (DropDownList)row.FindControl("listGridEq");
                    opt.bindDropDownList(list, "select IDKEY,EQ_NAME  from ht_eq_eqp_tbl where section_code = '" + mydrv["工段"].ToString() + "'", "EQ_NAME", "IDKEY");
                    list.SelectedValue = mydrv["设备名称"].ToString();
                    ((TextBox)row.FindControl("txtGridExptime")).Text = mydrv["期望完成时间"].ToString();
                    ((DropDownList)row.FindControl("listGrid2Status")).SelectedValue = mydrv["状态"].ToString();
                    ((TextBox)row.FindControl("txtGridremark")).Text = mydrv["备注"].ToString();

                }
            }
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }

    protected void btnDone_Click(object sender, EventArgs e)//
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        if (opt.GetSegValue("select flow_status from ht_eq_lb_plan where pz_code = '" + txtCode.Text + "'", "flow_status") != "2")
            return;
        for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
        {
            if (((CheckBox)GridView2.Rows[i].FindControl("chk")).Checked)
            {
                string ID = GridView2.DataKeys[i].Value.ToString();
                string query = "update ht_eq_lb_plan_detail set STATUS = '5'  where ID = '" + ID + "' and status = '4'";
                string log_message = opt.UpDateOra(query) == "Success" ? "下发润滑反馈成功" : "下发润滑反馈失败";
                log_message += "--标识:" + ID;
                InsertTlog(log_message);
            }
        }
        string alter = opt.GetSegValue("select case  when total = done then 1 else 0 end as status from (select  count(distinct t.id) as total,count( distinct t1.id) as done from ht_eq_lb_plan_detail t left join ht_eq_lb_plan_detail t1 on t1.id = t.id and t1.status = '5' and t1.is_del = '0' where t.main_id = '" + txtCode.Text + "'   and t.is_del = '0')", "status");
        if (alter == "1")
        {
            opt.UpDateOra("update ht_eq_lb_plan set TASK_STATUS = '5' where PZ_CODE = '" + txtCode.Text + "'  and TASK_STATUS = '4'");
            bindGrid1();
        }
        bindGrid2(txtCode.Text);
    }


    protected void btnTrack_Click(object sender, EventArgs e)//
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        //if (opt.GetSegValue("select flow_status from ht_eq_lb_plan where pz_code = '" + txtCode.Text + "'", "flow_status") != "2")
        //{
        // ScriptManager.RegisterStartupScript(UpdatePanel2, this.Page.GetType(), "", "$('#dspcthor').hide();", true);
        //    return;
        //}
        for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
        {
            if (((CheckBox)GridView2.Rows[i].FindControl("chk")).Checked)
            {
                string ID = GridView2.DataKeys[i].Value.ToString();
                string query = "update ht_eq_lb_plan_detail set STATUS = '3'  where ID = '" + ID + "' and status = '2'";

                string log_message = opt.UpDateOra(query) == "Success" ? "下发润滑跟踪成功" : "下发润滑跟踪失败";
                log_message += "--标识:" + ID;
                InsertTlog(log_message);
            }
        }
        string alter = opt.GetSegValue("select case  when total = done then 1 else 0 end as status from (select  count(distinct t.id) as total,count( distinct t1.id) as done from ht_eq_lb_plan_detail t left join ht_eq_lb_plan_detail t1 on t1.id = t.id and t1.status >= '3' and t1.is_del = '0' where t.main_id = '" + txtCode.Text + "'   and t.is_del = '0')", "status");
        if (alter == "1")
        {
            opt.UpDateOra("update ht_eq_lb_plan set TASK_STATUS = '3' where PZ_CODE = '" + txtCode.Text + "'  and TASK_STATUS = '2'");
            bindGrid1();
        }
        bindGrid2(txtCode.Text);
    }

    protected void btnDspcth_Click(object sender, EventArgs e)//
    {

        bool ck = false;
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        //if (opt.GetSegValue("select flow_status from ht_eq_lb_plan where pz_code = '" + txtCode.Text + "'", "flow_status") != "2")
        //{
        // ScriptManager.RegisterStartupScript(UpdatePanel2, this.Page.GetType(), "", "$('#dspcthor').hide();", true);
        //    return;
        //}
        for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
        {
            if (((CheckBox)GridView2.Rows[i].FindControl("chk")).Checked)
            {
                ck = true;
                string ID = GridView2.DataKeys[i].Value.ToString();
                string query = "update ht_eq_lb_plan_detail set STATUS = '1' ,RESPONER = '" + listdspcth.SelectedValue + "' where ID = '" + ID + "' and status <= '1'";

                string log_message = opt.UpDateOra(query) == "Success" ? "下发润滑任务成功" : "下发润滑任务失败";
                log_message += "--标识:" + ID;
                InsertTlog(log_message);
            }
        }
        string alter = opt.GetSegValue("select case  when total = done then 1 else 0 end as status from (select  count(distinct t.id) as total,count( distinct t1.id) as done from ht_eq_lb_plan_detail t left join ht_eq_lb_plan_detail t1 on t1.id = t.id and t1.status >= '1' and t1.is_del = '0' where t.main_id = '" + txtCode.Text + "'  and t.is_del = '0')", "status");
        if (alter == "1")
        {
            opt.UpDateOra("update ht_eq_lb_plan set TASK_STATUS = '1' where PZ_CODE = '" + txtCode.Text + "'  and TASK_STATUS = '0'");
            bindGrid1();
        }
        ScriptManager.RegisterStartupScript(UpdatePanel2, this.Page.GetType(), "", "$('#dspcthor').hide();", true);
        if (ck == true)
            bindGrid2(txtCode.Text);
        else
            ScriptManager.RegisterStartupScript(UpdatePanel2, this.Page.GetType(), "", "alert('请选择派工任务');", true);

    }
  

    protected void btnSave_Click(object sender, EventArgs e)//
    {
        if (txtName.Text == "" || listEditor.SelectedValue == "" || txtExptime.Text == "")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel2, this.Page.GetType(), "", "alert('请将信息填写完全');", true);
            return;
        }
        try
        {
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();

            string[] seg = { "PZ_CODE", "MT_NAME", "CREATE_ID", "CREATE_DEPT_ID", "EXPIRED_DATE", "REMARK", "IS_MODEL", "CREATE_TIME" };
            string[] value = { txtCode.Text, txtName.Text, listEditor.SelectedValue, listApt.SelectedValue, txtExptime.Text, txtdscrpt.Text, Convert.ToInt16(ckModel.Checked).ToString(), System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            
            string log_message = opt.MergeInto(seg, value, 1, "ht_eq_lb_plan") == "Success" ? "新增润滑计划成功" : "新增润滑计划失败";
            log_message += "--详情：" + string.Join(",", value);
            InsertTlog(log_message);

            bindGrid1();
            bindGrid4();
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btngrid2Deal_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        txtID.Text = GridView2.DataKeys[row.RowIndex].Value.ToString();       
       
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra("select * from ht_eq_lb_plan_detail where ID = '" + txtID.Text + "'");
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            DataRow drow = data.Tables[0].Rows[0];
            txtScean.Text = drow["FEEDBACK"].ToString();
            txtDesp.Text = "润滑部位：" + drow["position"].ToString() + "；润滑点数：" + drow["pointnum"].ToString() + "；润滑油脂：" + drow["luboil"].ToString() + "；润滑周期：" + drow["periodic"].ToString() + "；润滑方式：" + drow["style"].ToString() + "；润滑量：" + drow["amount"].ToString();
            txtPlus.Text = drow["REMARKPLUS"].ToString();
        }
        else
        {
            txtScean.Text = "";
            txtPlus.Text = "";
        }
        ScriptManager.RegisterStartupScript(updtpanel1, this.Page.GetType(), "", " $('.shade').fadeIn(200);", true);

    }
    protected void btnGrid2Save_Click(object sender, EventArgs e)//
    {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            if (((DropDownList)row.FindControl("listGridsct")).SelectedValue == "" || ((DropDownList)row.FindControl("listGridEq")).SelectedValue == "" )
            {
                ScriptManager.RegisterStartupScript(UpdatePanel2, this.Page.GetType(), "", "alert('请将信息填写完全后再保存');", true);
                return;
            }
            int rowIndex = row.RowIndex;
            string id = GridView2.DataKeys[rowIndex].Value.ToString();
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            if (id == "0")
                id = opt.GetSegValue("select lbdetail_id_seq.nextval as id  from dual", "id");

            string[] seg = { "ID", "section", "equipment_id", "remark", "CREATE_TIME", "MAIN_ID", "EXP_FINISH_TIME" };
            string[] value = { id, ((DropDownList)row.FindControl("listGridsct")).SelectedValue, ((DropDownList)row.FindControl("listGridEq")).SelectedValue, ((TextBox)row.FindControl("txtGridremark")).Text, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txtCode.Text, ((TextBox)row.FindControl("txtGridExptime")).Text };
         
           string log_message = opt.MergeInto(seg,value,1,"ht_eq_lb_plan_detail") == "Success" ? "新增校准明细成功" : "新增校准明细失败";
            log_message += "--详情：" + string.Join(",", value);
            InsertTlog(log_message);
            bindGrid2(txtCode.Text);      
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        string query = "select * from ht_eq_lb_plan_detail where MAIN_ID = '" + listModel.SelectedValue + "' and is_del = '0'";
        DataSet data = opt.CreateDataSetOra(query);
        List<string> commandlist = new List<string>();
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string[] seg = { "section", "equipment_id", "Remark", "CREATE_TIME", "MAIN_ID", "EXP_FINISH_TIME" };
                string[] value = { row["section"].ToString(), row["equipment_id"].ToString(),  row["Remark"].ToString(), System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txtCode.Text, System.DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd") };
                commandlist.Add(opt.InsertDatastr(seg, value, "ht_eq_lb_plan_detail"));

            }
            string log_message = opt.TransactionCommand(commandlist) == "Success" ? "按模版生成维保计划成功" : "按模版生成维保计划失败";
            log_message += ",计划ID号：" + txtCode.Text;
            InsertTlog(log_message);
       
        }
        bindGrid2(txtCode.Text);
    }

}