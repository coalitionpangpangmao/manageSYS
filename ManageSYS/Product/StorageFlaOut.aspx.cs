﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
public partial class Product_StorageFlaOut : MSYS.Web.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageLoad(sender, e);
        if (!IsPostBack)
        {
            initView();

        }

    }
    protected void initView()
    {
        txtStart.Text = System.DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        txtStop.Text = System.DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        opt.bindDropDownList(DropDownListShift, "select * from ht_sys_shift where is_del='0' and is_valid='1'", "shift_name", "shift_code");
        opt.bindDropDownList(DropDownListTeam, "select * from ht_sys_team where is_del='0' and is_valid='1'", "team_name", "team_code");
        opt.bindDropDownList(listApt, "select F_CODE,F_NAME from ht_svr_org_group order by F_CODE", "F_NAME", "F_CODE");
        opt.bindDropDownList(listPrdct, "select prod_code,prod_name from ht_pub_prod_design where is_valid = '1' and is_del = '0' order by prod_code", "PROD_NAME", "PROD_CODE");
        opt.bindDropDownList(listPrdctPlan, "select PLAN_NO from ht_prod_month_plan_detail where EXE_STATUS <> '3' and is_DEL = '0' and FLAVOR_STATUS = '1' order by Plan_no", "PLAN_NO", "PLAN_NO");
        opt.bindDropDownList(listStorage, "select * from ht_inner_mat_depot order by ID", "NAME", "ID");
        // opt.bindDropDownList(listStatus, "select ID,ISSUE_NAME from HT_INNER_BOOL_DISPLAY ", "ISSUE_NAME", "ID");
        opt.bindDropDownList(listStatus, "select * from ht_inner_aprv_status order by ID", "NAME", "ID");
        opt.bindDropDownList(listCreator, "select s.name,s.id from ht_svr_sys_role t left join ht_svr_sys_menu r on substr(t.f_right,r.f_id,1) = '1' left join ht_svr_user s on s.role = t.f_id where r.f_id = '" + this.RightId + "' union select q.name,q.id from ht_svr_sys_role t left join ht_svr_sys_menu r on substr(t.f_right,r.f_id,1) = '1' left join ht_svr_org_group  s on s.f_role = t.f_id  left join ht_svr_user q on q.levelgroupid = s.f_code  where r.f_id = '" + this.RightId + "'  order by id desc", "Name", "ID");
        bindGrid1();
    }
    #region tab1
    protected void bindGrid1()
    {
        string query = " select distinct g.out_date as 领用日期,r.strg_name as 出入库类型，g.issue_status as 出入库状态,g.order_sn as 单据号 ,g.batchnum as 投料批次,b.prod_name as 产品,g.CABOSUM as 香精香料总量,g.PEICESSUM as 料液总量,s.name as 审批状态,h.name as 编制人,j.name as 收发人,g.MONTHPLANNO  from HT_STRG_FLAVOR g  left join HT_INNER_BOOL_DISPLAY r on r.id = g.strg_type left join ht_inner_Aprv_status s on s.id = g.audit_mark left join ht_svr_user h on h.id = g.creator_id left join ht_svr_user j on j.id = g.issuer_id left join ht_prod_month_plan_detail a on a.plan_no = g.monthplanno left join ht_pub_prod_design b on a.prod_code = b.prod_code where g.out_date between '" + txtStart.Text + "' and '" + txtStop.Text + "'  and g.IS_DEL = '0' and g.strg_type = '0'  order by g.order_sn";
        
       
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
                ((Label)row.FindControl("labStrg")).Text = mydrv["出入库类型"].ToString();
                ((Label)row.FindControl("labAudit")).Text = mydrv["审批状态"].ToString();
                if (((Label)row.FindControl("labStrg")).Text == "出库")
                {
                    if (mydrv["出入库状态"].ToString() == "0")
                        ((Label)row.FindControl("labIssue")).Text = "未出库";
                    else
                        ((Label)row.FindControl("labIssue")).Text = "己出库";
                }
                
                ((Button)row.FindControl("btnGridopt")).Text = mydrv["出入库类型"].ToString();
                if (mydrv["审批状态"].ToString() != "未提交")
                    ((Button)row.FindControl("btnSubmit")).Enabled = false;
                if (mydrv["出入库状态"].ToString() != "0")
                    ((Button)row.FindControl("btnGridopt")).Enabled = false;


                if (!(mydrv["审批状态"].ToString() == "未提交" || mydrv["审批状态"].ToString() == "未通过"))
                {
                    ((Button)row.FindControl("btnSubmit")).Enabled = false;
                    ((Button)row.FindControl("btnSubmit")).CssClass = "btngrey";
                    ((Button)row.FindControl("btnGridview")).Text = "查看";
                }
                else
                {
                    ((Button)row.FindControl("btnSubmit")).Enabled = true;
                    ((Button)row.FindControl("btnSubmit")).CssClass = "btn1 auth";
                    ((Button)row.FindControl("btnGridview")).Text = "编制";
                }
                if (mydrv["出入库状态"].ToString() != "0")
                {
                    ((Button)row.FindControl("btnGridopt")).Enabled = false;
                    ((Button)row.FindControl("btnGridopt")).CssClass = "btngrey";
                }
                else
                {
                    ((Button)row.FindControl("btnGridopt")).Enabled = true;
                    ((Button)row.FindControl("btnGridopt")).CssClass = "btn1 auth";
                }
            }
        }

    }//绑定gridview1数据源
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindGrid1();
    }
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
    protected void btnCkAll1_Click(object sender, EventArgs e)
    {
        int ckno = 0;
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            if (((CheckBox)GridView1.Rows[i].FindControl("chk")).Checked)
                ckno++;
        }
        bool check = (ckno < GridView1.Rows.Count);
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            ((CheckBox)GridView1.Rows[i].FindControl("chk")).Checked = check;

        }
    }
    protected void btnGridIssue_Click(object sender, EventArgs e)//查看审批流程
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        string ID = GridView1.DataKeys[rowIndex].Values[0].ToString();
        string query = "select pos as 顺序号, workitemid as 审批环节,username as 负责人,comments as 意见,opiniontime 审批时间,(case status when '0' then '未审批'  when '1' then '未通过' else '己通过' end)  as 审批状态  from ht_pub_aprv_opinion r left join ht_pub_aprv_flowinfo s on r.gongwen_id = s.id where s.busin_id  = '" + ID + "' order by pos";
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        GridView3.DataSource = opt.CreateDataSetOra(query);
        GridView3.DataBind();
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.Page.GetType(), "", "$('#flowinfo').fadeIn(200);", true);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)//提交审批
    {
        try
        {
            Button btn = (Button)sender;
            int index = ((GridViewRow)btn.NamingContainer).RowIndex;//获得行号                 
            string id = GridView1.DataKeys[index].Values[0].ToString();
            /*启动审批TB_ZT标题,TBR_ID填报人id,TBR_NAME填报人name,TB_BM_ID填报部门id,TB_BM_NAME填报部门name,TB_DATE申请时间创建日期,MODULENAME审批类型编码,URL 单独登录url,BUSIN_ID业务数据id*/
            string[] subvalue = { "仓储" + ((Label)GridView1.Rows[index].FindControl("labStrg")).Text + id, "18", id, Page.Request.UserHostName.ToString() };
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            if (MSYS.AprvFlow.createApproval(subvalue))
            {
                opt.UpDateOra("update HT_STRG_FLAVOR set AUDIT_MARK = '0'  where ORDER_SN = '" + id + "'");
            }
            bindGrid1();
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnGridopt_Click(object sender, EventArgs e)//出库，调用接口，变更库存情况
    {


        Button btn = (Button)sender;
        int index = ((GridViewRow)btn.NamingContainer).RowIndex;//获得行号  
        string id = GridView1.DataKeys[index].Values[0].ToString();
        string aprv = ((Label)GridView1.Rows[index].FindControl("labAudit")).Text;
        string planno = GridView1.DataKeys[index].Values[1].ToString();
        if (aprv == "己通过")
        {
            List<string> commandlist = new List<string>();
            commandlist.Add("update HT_STRG_FLAVOR set ISSUE_STATUS = '1',ISSUER_ID ='" + ((MSYS.Data.SysUser)Session["User"]).id + "'  where ORDER_SN = '" + id + "'");
            commandlist.Add("update HT_PROD_MONTH_PLAN_DETAIL set  FLAVOR_STATUS = '2' where PLAN_NO = '" + planno + "' and FLAVOR_STATUS = '1'");
            MSYS.Web.StorageOpt st = new MSYS.Web.StorageOpt();
            ////调用接口，变更库存////
         string flag = st.XiangInOrOut(id, ((MSYS.Data.SysUser)Session["User"]).text, ((MSYS.Data.SysUser)Session["User"]).id);
            /////
         if (flag.Length > 9 && flag.Substring(0, 9) == "notenough")
         {
             ScriptManager.RegisterStartupScript(GridView1, this.GetType(), "scriptKey", "alert(‘物料不足" + flag.Substring(9) + "’)", true);
         }
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            string log_message = opt.TransactionCommand(commandlist) == "Success" ? "出入库成功" : "出入库失败";
            log_message += "--标识:" + id;
            InsertTlog(log_message);
            bindGrid1();
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.Page.GetType(), "", "alert('请通过审批后再出库');", true);
        }


    }
    protected void btnGridview_Click(object sender, EventArgs e)//查看领退明细
    {
        Button btn = (Button)sender;
        int rowIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
        txtCode.Text = GridView1.DataKeys[rowIndex].Values[0].ToString();

        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra("select * from HT_STRG_FLAVOR  where ORDER_SN =  '" + txtCode.Text + "'");
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            txtPrdctdate.Text = data.Tables[0].Rows[0]["OUT_DATE"].ToString();
            listStatus.SelectedValue = data.Tables[0].Rows[0]["AUDIT_MARK"].ToString();
            listStorage.SelectedValue = data.Tables[0].Rows[0]["WARE_HOUSE_ID"].ToString();
            listApt.SelectedValue = data.Tables[0].Rows[0]["DEPT_ID"].ToString();
            if (listPrdctPlan.Items.FindByValue(data.Tables[0].Rows[0]["MONTHPLANNO"].ToString())!= null)
            {
                listPrdctPlan.SelectedValue = data.Tables[0].Rows[0]["MONTHPLANNO"].ToString();
                txtPrdctPlan.Text = listPrdctPlan.SelectedValue;
                txtPrdctPlan.Visible = false;
                listPrdctPlan.Visible = true;
            }
            else
            {
                listPrdctPlan.SelectedValue = "";
                txtPrdctPlan.Text = data.Tables[0].Rows[0]["MONTHPLANNO"].ToString();
                txtPrdctPlan.Visible = true;
                listPrdctPlan.Visible = false;
            }
            string temp = opt.GetSegValue("select Prod_code from ht_prod_month_plan_detail where plan_no = '" + txtPrdctPlan.Text + "'", "PROD_CODE");
            if (listPrdct.Items.FindByValue(temp) != null)
                listPrdct.SelectedValue = temp;
            else
                listPrdct.SelectedValue = "";
          
            txtBatchNum.Text = data.Tables[0].Rows[0]["BATCHNUM"].ToString();

            DropDownListShift.SelectedValue = data.Tables[0].Rows[0]["shift_code"].ToString();
            DropDownListTeam.SelectedValue = data.Tables[0].Rows[0]["team_code"].ToString();
            txtValiddate.Text = data.Tables[0].Rows[0]["EXPIRED_DATE"].ToString();
          
          
            txtStemSum.Text = data.Tables[0].Rows[0]["CABOSUM"].ToString();
            listCreator.SelectedValue = data.Tables[0].Rows[0]["CREATOR_ID"].ToString();
        }
        if (listStatus.SelectedItem.Text == "未提交")
            SetEnable(true);
        else
            SetEnable(false);
        bindGrid2();
       
    }
    private void SetEnable(bool enable)
    {
        //btnCreate.Visible = enable;
        //btnAdd.Visible = enable;
        btnCkAll.Visible = enable;
        btnDelSel.Visible = enable;
        btnModify.Visible = enable;
        if (GridView2.Columns.Count == 8)
        {
            GridView2.Columns[7].Visible = enable;

        }
    }
    protected void btnGridDel_Click(object sender, EventArgs e)//删除选中记录
    {
        List<String> commandlist = new List<String>();
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
        {
            if (((CheckBox)GridView1.Rows[i].FindControl("chk")).Checked)
            {
                commandlist.Clear();
                string order_sn = GridView1.DataKeys[i].Values[0].ToString();
                commandlist.Add("delete from  HT_STRG_FLAVOR  where ORDER_SN = '" + order_sn + "'");
                commandlist.Add("delete from ht_pub_aprv_flowinfo where BUSIN_ID = '" + order_sn + "'");
                string log_message = opt.TransactionCommand(commandlist) == "Success" ? "删除出入库计划成功" : "删除出入库计划失败";
                log_message += "--标识:" + order_sn;
                InsertTlog(log_message);
            }
        }
        bindGrid1();

    }
    protected void btnGridNew_Click(object sender, EventArgs e)// 新增领退明细
    {
        setBlank();
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        txtCode.Text = "SF" + System.DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt16(opt.GetSegValue("select  nvl(max(substr(ORDER_SN,11,3)),0)  as ordernum from HT_STRG_FLAVOR where substr(ORDER_SN,1,10) ='SF" + System.DateTime.Now.ToString("yyyyMMdd") + "'", "ordernum")) + 1).ToString().PadLeft(3, '0');
        MSYS.Data.SysUser user = (MSYS.Data.SysUser)Session["User"];
        listCreator.SelectedValue = user.id;
        listApt.SelectedValue = user.OwningBusinessUnitId;
        bindGrid2();
        SetEnable(true);
      
    }
    protected DataSet gridHTYbind(String code)
    {
        string s1_1 = code.Replace(",", "','"); //返回结果为：1','2','3','4','5','6
        code = string.Format("'{0}'", s1_1);
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        return opt.CreateDataSetOra("select material_code,material_name from ht_pub_materiel where substr(TYPE_CODE,1,4) in (" + code + ") or substr(material_code,1,4) in (" + code + ") and is_del = '0' union select '' as material_code, '' as material_name from dual order by material_code desc");
    }

    #endregion

    #region tab2
    protected void bindGrid2()
    {

        string query = " select STORAGE as  仓库,CLS_CODE as   类型 ,unit_code as  计量单位,mater_code as  物料编码,original_demand as   领料量,ID  from HT_STRG_FLAVOR_SUB where main_code = '" + txtCode.Text + "' and IS_DEL = '0'";

        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra(query);
        GridView2.DataSource = data;
        GridView2.DataBind();
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < GridView2.Rows.Count; i++)
            {
                GridViewRow row = GridView2.Rows[i];
                DataRowView mydrv = data.Tables[0].DefaultView[i];
                ((DropDownList)GridView2.Rows[i].FindControl("listGridstrg")).SelectedValue = mydrv["仓库"].ToString();

                ((TextBox)row.FindControl("txtGridcode")).Text = mydrv["物料编码"].ToString();
                ((DropDownList)row.FindControl("listGridName")).SelectedValue = mydrv["物料编码"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridUnit")).Text = mydrv["计量单位"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridAmount")).Text = mydrv["领料量"].ToString();
            }

        }

    }//绑定GridView2数据源

    protected void btnReset_Click(object sender, EventArgs e)
    {
        setBlank();
    }

    protected void setBlank()
    {
        txtPrdctdate.Text = "";
        listStatus.SelectedValue = "";
        listStorage.SelectedValue = "";
        listApt.SelectedValue = "";
        listPrdctPlan.SelectedValue = "";
        txtBatchNum.Text = "";
        listPrdct.SelectedValue = "";
       
        txtStemSum.Text = "";
        txtValiddate.Text = "";
        bindGrid2();
        listPrdctPlan.Visible = true;
        txtPrdctPlan.Visible = false;
      
    }

    protected void listGridName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList list = (DropDownList)sender;
        GridViewRow row = (GridViewRow)list.NamingContainer;
        ((TextBox)row.FindControl("txtGridcode")).Text = list.SelectedValue;
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        ((DropDownList)row.FindControl("listGridstrg")).SelectedValue = opt.GetSegValue("select data_origin_flag from ht_pub_materiel where material_code = '" + list.SelectedValue + "'", "data_origin_flag");
    }

    protected void listGridType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList list = (DropDownList)sender;
        GridViewRow row = (GridViewRow)list.NamingContainer;
        if (list.SelectedValue != "")
        {
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
            opt.bindDropDownList((DropDownList)row.FindControl("listGridName"), "select material_code,material_name from ht_pub_materiel  where  is_del = '0' and mat_category = '原材料'   and (substr(type_code,1,4) ='" + list.SelectedValue + "' or substr(material_code,1,4) = '" + list.SelectedValue + "')", "material_name", "material_code");
        }
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        List<string> commandlist = new List<string>();
        string log_message;
        if (txtPrdctdate.Text == "" || txtValiddate.Text == ""|| listPrdctPlan.SelectedValue == "" || listPrdct.SelectedValue == "" || DropDownListTeam.SelectedValue.ToString()=="" || DropDownListShift.SelectedValue.ToString()=="")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel2, this.Page.GetType(), "alert", "alert('请输入必要信息!!')", true);
            return;
        }
      
            //生成领用主表记录
            string[] seg = { "ORDER_SN", "OUT_DATE", "EXPIRED_DATE", "MODIFY_TIME", "WARE_HOUSE_ID", "DEPT_ID", "CREATOR_ID", "MONTHPLANNO", "BATCHNUM", "STRG_TYPE" ,"team_code", "shift_code"};
            string[] value = { txtCode.Text, txtPrdctdate.Text, txtValiddate.Text, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), listStorage.SelectedValue, listApt.SelectedValue, listCreator.SelectedValue, listPrdctPlan.SelectedValue, "", "0", DropDownListTeam.SelectedValue.ToString(), DropDownListShift.SelectedValue.ToString() };

            commandlist.Add(opt.getMergeStr(seg, value, 1, "HT_STRG_FLAVOR"));
            commandlist.Add("delete from HT_STRG_FLAVOR_SUB where MAIN_CODE = '" + txtCode.Text + "'");
            //根据生产计划对应的配方明细生成原料领用明细           
            //commandlist.Add("insert into HT_STRG_FLAVOR_SUB  select '' as ID, g3.mater_code ,g4.data_origin_flag as STORAGE,g3.batch_num*" + txtBatchNum.Text + " as ORIGINAL_DEMAND, '' as REAL_DEMAND,'' as Remark,g4.unit_code , '0' as IS_DEL,g3.coat_flag as mater_flag,'" + txtCode.Text + "' as MAIN_CODE,'' as PACKNUM ,'' as SUBSTANCE, '' as ODDQTY ,g3.CLASS_NAME as cls_code from ht_prod_month_plan_detail g1 left join ht_pub_prod_design g2 on g1.prod_code = g2.prod_code left join HT_QA_FLA_FORMULA_detail g3 on g3.formula_code = g2.FLA_FORMULA_CODE and g3.is_del = '0' left join ht_pub_materiel g4 on g4.material_code = g3.mater_code where g1.plan_no = '" + listPrdctPlan.SelectedValue + "' and g3.mater_code is not null");
           // commandlist.Add("insert into HT_STRG_FLAVOR_SUB  select '' as ID, g3.mater_code as MATER_CODE,g4.data_origin_flag as STORAGE,g3.batch_num*" + txtBatchNum.Text + " as ORIGINAL_DEMAND, '' as REAL_DEMAND,'' as Remark,g4.unit_code as UNIT_CODE , '0' as IS_DEL,g3.coat_flag as mater_flag,'" + txtCode.Text.Trim() + "' as MAIN_CODE,'' as PACKNUM ,'' as SUBSTANCE, '' as ODDQTY , '' as cls_code, '' as REMNANT, ''as PIECE_NUM from ht_prod_month_plan_detail g1 left join ht_pub_prod_design g2 on g1.prod_code = g2.prod_code left join HT_QA_FLA_FORMULA_detail g3 on g3.formula_code = g2.FLA_FORMULA_CODE and g3.is_del = '0' left join ht_pub_materiel g4 on g4.material_code = g3.mater_code where g1.plan_no = '" + listPrdctPlan.SelectedValue + "' and g3.mater_code is not null");
            commandlist.Add("insert into HT_STRG_FLAVOR_SUB  select '' as ID, g3.mater_code as MATER_CODE,g4.data_origin_flag as STORAGE,g3.batch_num*1 as ORIGINAL_DEMAND, '' as REAL_DEMAND,'' as Remark,g4.unit_code as UNIT_CODE , '0' as IS_DEL,g3.coat_flag as mater_flag,'" + txtCode.Text.Trim() + "' as MAIN_CODE,'' as PACKNUM ,'' as SUBSTANCE, '' as ODDQTY , '' as cls_code, '' as REMNANT, ''as PIECE_NUM from ht_prod_month_plan_detail g1 left join ht_pub_prod_design g2 on g1.prod_code = g2.prod_code left join HT_QA_FLA_FORMULA_detail g3 on g3.formula_code = g2.FLA_FORMULA_CODE and g3.is_del = '0' left join ht_pub_materiel g4 on g4.material_code = g3.mater_code where g1.plan_no = '" + listPrdctPlan.SelectedValue + "' and g3.mater_code is not null");
            log_message = opt.TransactionCommand(commandlist) == "Success" ? "生成原料领用主表记录成功" : "生成原料领用主表记录失败";
            log_message += "--详情:" + string.Join(",", value);
            InsertTlog(log_message);
            //////计算出入库单总各类型总量
            DataSet res = opt.CreateDataSetOra("select sum(t.original_demand) as amount ,t.mater_flag from HT_STRG_FLAVOR_SUB t  where t.main_code = '" + txtCode.Text + "' group by  t.mater_flag");
            if (res != null && res.Tables[0].Rows.Count > 0)
            {
                double CABOSUM=0;
                foreach (DataRow row in res.Tables[0].Rows)
                {

                        if(row["amount"].ToString()!="")
                            CABOSUM += Convert.ToDouble(row["amount"].ToString());
                   
                }
                txtStemSum.Text = CABOSUM.ToString("0.00");
                
                string[] seg1 = { "ORDER_SN", "CABOSUM"};
                string[] value1 = { txtCode.Text, txtStemSum.Text};
                opt.MergeInto(seg1, value1, 1, "HT_STRG_FLAVOR");
            }
      
        bindGrid1();
        bindGrid2();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        string query = " select STORAGE as  仓库,CLS_CODE as   类型 ,unit_code as  计量单位,mater_code as   物料编码,original_demand as   领料量,ID  from HT_STRG_FLAVOR_SUB where main_code = '" + txtCode.Text + "'  and IS_DEL = '0'";
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet set = opt.CreateDataSetOra(query);
        DataTable data = new DataTable();
        if (set != null && set.Tables[0].Rows.Count > 0)
            data = set.Tables[0];

        else
        {
            data.Columns.Add("仓库");
            data.Columns.Add("类型");
            data.Columns.Add("计量单位");
            data.Columns.Add("物料编码");
            data.Columns.Add("领料量");
            data.Columns.Add("ID");
        }
        object[] value = { "", "", "", "", 0, 0 };
        data.Rows.Add(value);
        GridView2.DataSource = data;
        GridView2.DataBind();
        if (data != null && data.Rows.Count > 0)
        {
            for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
            {
                DataRowView mydrv = data.DefaultView[i];
                GridViewRow row = GridView2.Rows[i];
                ((DropDownList)GridView2.Rows[i].FindControl("listGridstrg")).SelectedValue = mydrv["仓库"].ToString();

                DropDownList list = (DropDownList)row.FindControl("listGridType");
                list.SelectedValue = mydrv["类型"].ToString();
                opt.bindDropDownList((DropDownList)row.FindControl("listGridName"), "select material_code,material_name from ht_pub_materiel  where  is_del = '0' and mat_category = '原材料' and  substr(type_code,1,4) ='" + list.SelectedValue + "' or substr(mater_code,1,4) = '" + list.SelectedValue + "'", "material_name", "material_code");
                ((TextBox)row.FindControl("txtGridcode")).Text = mydrv["物料编码"].ToString();
                ((DropDownList)row.FindControl("listGridName")).SelectedValue = mydrv["物料编码"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridUnit")).Text = mydrv["计量单位"].ToString();
                ((TextBox)GridView2.Rows[i].FindControl("txtGridAmount")).Text = mydrv["领料量"].ToString();

            }

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

        for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
        {
            if (((CheckBox)GridView2.Rows[i].FindControl("chk")).Checked)
            {
                string ID = GridView2.DataKeys[i].Value.ToString();
                string query = "update HT_STRG_FLAVOR_SUB set IS_DEL = '1'  where ID = '" + ID + "'";
                MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
                opt.UpDateOra(query);
            }
        }
        bindGrid2();

    }

    protected void btnCreate_Click(object sender, EventArgs e)//按明细生成领退单
    {
        try
        {
            MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
           
                //生成领用主表记录
                string[] seg = { "OUT_DATE", "ORDER_SN", "EXPIRED_DATE", "MODIFY_TIME", "DEPT_NAME", "DEPT_ID", "CREATOR", "CREATOR_ID", "STRG_TYPE" };
                string[] value = { txtPrdctdate.Text, txtCode.Text, txtValiddate.Text, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), listApt.SelectedItem.Text, listApt.SelectedValue, "cookieName", "cookieID", "0" };
                opt.InsertData(seg, value, "HT_STRG_FLAVOR");

           

        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }

    protected void btnGrid2Save_Click(object sender, EventArgs e)
    {

        /*MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        if (txtCode.Text == "")
            txtCode.Text = "SF" + System.DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt16(opt.GetSegValue("select count(ORDER_SN) as ordernum from HT_STRG_FLAVOR where substr(ORDER_SN,1,10) ='SF" + System.DateTime.Now.ToString("yyyyMMdd") + "'", "ordernum")) + 1).ToString().PadLeft(3, '0');
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        string ID = GridView2.DataKeys[row.RowIndex].Value.ToString();
        if (ID == "0")
        {
            ID = opt.GetSegValue("select STRGMATER_ID_SEQ.nextval as id from dual", "ID");
        }
        string[] seg = { "ID", "STORAGE", "CLS_CODE", "unit_code", "mater_code", "ORIGINAL_DEMAND", "MAIN_CODE" };
        string[] value = { ID, ((DropDownList)row.FindControl("listGridstrg")).SelectedValue, ((DropDownList)row.FindControl("listGridType")).SelectedValue, ((TextBox)row.FindControl("txtGridUnit")).Text, ((TextBox)row.FindControl("txtGridcode")).Text, ((TextBox)row.FindControl("txtGridAmount")).Text, txtCode.Text };

        string log_message = opt.MergeInto(seg, value, 1, "HT_STRG_FLAVOR_SUB") == "Success" ? "保存原料领用明细成功" : "保存原料领用明细失败";
        log_message += "--详情:" + string.Join(",", value);
        InsertTlog(log_message);
        bindGrid2();*/
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        if (txtCode.Text == "")
            txtCode.Text = "SF" + System.DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt16(opt.GetSegValue("select count(ORDER_SN) as ordernum from HT_STRG_FLAVOR where substr(ORDER_SN,1,10) ='SF" + System.DateTime.Now.ToString("yyyyMMdd") + "'", "ordernum")) + 1).ToString().PadLeft(3, '0');
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        string ID = GridView2.DataKeys[row.RowIndex].Value.ToString();
        if (ID == "0")
        {
            ID = opt.GetSegValue("select STRGFLA_ID_SEQ.nextval as id from dual", "ID");
        }
        string[] seg = { "ID", "STORAGE", "unit_code", "mater_code", "ORIGINAL_DEMAND", "MAIN_CODE" };
        string[] value = { ID, ((DropDownList)row.FindControl("listGridstrg")).SelectedValue, ((TextBox)row.FindControl("txtGridUnit")).Text, ((TextBox)row.FindControl("txtGridcode")).Text, ((TextBox)row.FindControl("txtGridAmount")).Text, txtCode.Text };

        string log_message = opt.MergeInto(seg, value, 1, "HT_STRG_FLAVOR_SUB") == "Success" ? "保存原料领用明细成功" : "保存原料领用明细失败";
        log_message += "--详情:" + string.Join(",", value);
        InsertTlog(log_message);
        update_MainAmount(txtCode.Text);
        bindGrid2();
       

    }

    protected void update_MainAmount(string code)
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet res = opt.CreateDataSetOra("select sum(t.original_demand) as amount ,t.mater_flag from HT_STRG_FLAVOR_SUB t  where t.main_code = '" + code + "' group by  t.mater_flag");
        if (res != null && res.Tables[0].Rows.Count > 0)
        {
            double CABOSUM = 0;
            foreach (DataRow row in res.Tables[0].Rows)
            {

                CABOSUM += Convert.ToDouble(row["amount"].ToString());

            }
            txtStemSum.Text = CABOSUM.ToString("0.00");

            string[] seg1 = { "ORDER_SN", "CABOSUM" };
            string[] value1 = { txtCode.Text, txtStemSum.Text };
            opt.MergeInto(seg1, value1, 1, "HT_STRG_FLAVOR");
        }
    }

    protected void listPrdctPlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
      
            string prod_code = opt.GetSegValue("select Prod_code from ht_prod_month_plan_detail where plan_no = '" + listPrdctPlan.SelectedValue + "'", "PROD_CODE");
            if (listPrdct.Items.FindByValue(prod_code) != null)
                listPrdct.SelectedValue = prod_code;
            else
                listPrdct.SelectedValue = "";
       
      
    }
    #endregion
}