﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Quality_QualitySet : MSYS.Web.BasePage
{    
    protected string subtvHtml;
    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageLoad(sender, e);
        if (!IsPostBack)
        {
           DataBaseOperator opt =new DataBaseOperator();
            opt.bindDropDownList(listVersion, "select QLT_CODE,QLT_NAME from ht_qlt_STDD_CODE where is_valid = '1' and is_del= '0'", "QLT_NAME", "QLT_CODE");        
         
            opt.bindDropDownList(listtech, "select QLT_CODE,QLT_NAME from ht_qlt_STDD_CODE where is_valid = '1' and is_del= '0'", "QLT_NAME", "QLT_CODE");
            opt.bindDropDownList(listtechC, "select QLT_CODE,QLT_NAME from ht_qlt_STDD_CODE where is_valid = '1' and is_del= '0'", "QLT_NAME", "QLT_CODE");
            
            subtvHtml = InitTreePrcss();
        }
    }

    public string InitTreePrcss()
    {

       DataBaseOperator opt =new DataBaseOperator();
        DataSet data = opt.CreateDataSetOra("select g.section_code,g.section_name from ht_pub_tech_section g where g.IS_VALID = '1' and g.IS_DEL = '0' order by g.section_code ");
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            string tvHtml = "<ul id='browser' class='filetree treeview-famfamfam'>";
            DataRow[] rows = data.Tables[0].Select();
            foreach (DataRow row in rows)
            {
                // tvHtml += "<li ><a href='Tech_Session.aspx?session_code=" + row["section_code"].ToString() + "' target='sessionFrame'><span class='folder'  onclick = \"$('#tabtop1').click()\">" + row["section_name"].ToString() + "</span></a>";   
                tvHtml += "<li ><span class='folder'  onclick = \"tabClick(" + row["section_code"].ToString() + ")\">" + row["section_name"].ToString() + "</span></a>";
                tvHtml += "</li>";
            }
            tvHtml += "</ul>";
            return tvHtml;
        }
        else
            return "";
    }
 
    protected void bindData(string qltcode)
    {
        string query = "select * from ht_qlt_STDD_CODE where is_del = '0' and is_valid = '1' and QLT_CODE  = '" + qltcode + "'";
       DataBaseOperator opt =new DataBaseOperator();
        DataSet data = opt.CreateDataSetOra(query);
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            txtCode.Text = qltcode;
            txtName.Text = data.Tables[0].Rows[0]["QLT_NAME"].ToString();           
            txtVersion.Text = data.Tables[0].Rows[0]["STANDARD_VOL"].ToString();
            txtExeDate.Text = data.Tables[0].Rows[0]["B_DATE"].ToString();
            txtEndDate.Text = data.Tables[0].Rows[0]["E_DATE"].ToString();
            txtCreator.Text = data.Tables[0].Rows[0]["CREATE_ID"].ToString();
            txtCrtDate.Text = data.Tables[0].Rows[0]["CREATE_DATE"].ToString();
            txtDscpt.Text = data.Tables[0].Rows[0]["REMARK"].ToString();
           
        }
        bindGrid(qltcode,hideprc.Value);
    }
    protected void bindGrid(string rcpcode,string section)
    {
        string query = "select g1.PARA_CODE as 参数编码,g1.lower as 下限,g1.upper as 上限,g1.QLT_TYPE as 考核类型,g2.section_name as 工艺段,g1.REMARK as 备注 from ht_QLT_stdd_code_detail g1 left join ht_pub_tech_section g2 on substr(g1.para_code ,1,5) = g2.section_code where g1.is_del = '0'  and g1.qlt_code = '" + rcpcode + "' and g2.section_code = '" + section + "'";
       DataBaseOperator opt =new DataBaseOperator();
        DataSet set = opt.CreateDataSetOra(query);
        DataTable data = set.Tables[0];
        if (data == null)
        {            
            data = new DataTable();
            data.Columns.Add("参数编码");
            data.Columns.Add("下限");
            data.Columns.Add("上限");
            data.Columns.Add("考核类型");
            data.Columns.Add("工艺段");
            data.Columns.Add("备注");
        }
        object[] value = { "", 0, 0, "1", section, "" };
        data.Rows.Add(value);

        bindgrid(data,section);
    }
    protected void bindgrid(DataTable data,string section)
    {
        DataBaseOperator opt = new DataBaseOperator();
        GridView1.DataSource = data;
        GridView1.DataBind();
        if (data != null && data.Rows.Count > 0)
        {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                DataRowView mydrv = data.DefaultView[i];
                DropDownList list = (DropDownList)GridView1.Rows[i].FindControl("listParaName");
                opt.bindDropDownList(list, "select * from HT_PUB_TECH_PARA where is_del = '0' and substr(PARA_CODE,1,5) = '" + section + "'", "PARA_NAME", "PARA_CODE");
                list.SelectedValue = mydrv["参数编码"].ToString();
                ((TextBox)GridView1.Rows[i].FindControl("txtLower")).Text = mydrv["下限"].ToString();
                ((TextBox)GridView1.Rows[i].FindControl("txtUpper")).Text = mydrv["上限"].ToString();
                ((DropDownList)GridView1.Rows[i].FindControl("listtype")).SelectedValue = mydrv["考核类型"].ToString();
                ((TextBox)GridView1.Rows[i].FindControl("txtSection")).Text = mydrv["工艺段"].ToString();
                ((TextBox)GridView1.Rows[i].FindControl("txtDscrptM")).Text = mydrv["备注"].ToString();


            }

        }
    }
    protected DataSet typebind()
    {
        DataBaseOperator opt = new DataBaseOperator();
        return opt.CreateDataSetOra("select * from ht_inner_qlt_type ");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bindGrid(txtCode.Text, hideprc.Value);
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        DataBaseOperator opt = new DataBaseOperator();
        string code = opt.GetSegValue("select nvl(max(substr(QLT_CODE,4,3)),0)+1 as code from ht_qlt_stdd_code t", "code");
        txtCode.Text = "QLT" + code.PadLeft(3, '0');
        MSYS.Data.SysUser user = (MSYS.Data.SysUser)Session["User"];
        txtCreator.Text = user.Name;
        txtCrtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        txtDscpt.Text = "";
        txtEndDate.Text = "";
        txtExeDate.Text = "";
        txtName.Text = "";
        txtVersion.Text = "";
        
    }
    protected void btnModify_Click(object sender, EventArgs e)
    {
       DataBaseOperator opt =new DataBaseOperator();
        

         string[] seg = { "QLT_CODE", "QLT_NAME", "STANDARD_VOL", "B_DATE", "E_DATE", "CREATE_ID", "CREATE_DATE", "REMARK" };
            string[] value = { txtCode.Text, txtName.Text,  txtVersion.Text, txtExeDate.Text, txtEndDate.Text,((MSYS.Data.SysUser)Session["User"]).Id, txtCrtDate.Text,  txtDscpt.Text, };
            opt.MergeInto(seg, value, 1, "HT_QLT_STDD_CODE");           
        bindGrid(txtCode.Text,hideprc.Value);
        opt.bindDropDownList(listVersion, "select QLT_CODE,QLT_NAME from ht_qlt_STDD_CODE where is_valid = '1' and is_del= '0'", "QLT_NAME", "QLT_CODE");
        opt.bindDropDownList(listtech, "select QLT_CODE,QLT_NAME from ht_qlt_STDD_CODE where is_valid = '1' and is_del= '0'", "QLT_NAME", "QLT_CODE");
        opt.bindDropDownList(listtechC, "select QLT_CODE,QLT_NAME from ht_qlt_STDD_CODE where is_valid = '1' and is_del= '0'", "QLT_NAME", "QLT_CODE");
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        try
        {
           DataBaseOperator opt =new DataBaseOperator();
            opt.UpDateOra("delete from HT_QLT_STDD_CODE where QLT_CODE = '" + txtCode.Text + "'");           
            opt.bindDropDownList(listVersion, "select QLT_CODE,QLT_NAME from ht_qlt_STDD_CODE where is_valid = '1' and is_del= '0'", "QLT_NAME", "QLT_CODE");   
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnGridDel_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            int Rowindex = ((GridViewRow)btn.NamingContainer).RowIndex;//获得行号  
            string mtr_code = ((TextBox)GridView1.Rows[Rowindex].FindControl("txtCodeM")).Text;
            string query = "update HT_QLT_STDD_CODE_DETAIL set IS_DEL = '1'  where QLT_CODE = '" + txtCode.Text + "' and PARA_CODE = '" + mtr_code + "'";
           DataBaseOperator opt =new DataBaseOperator();
            opt.UpDateOra(query);
            bindGrid(txtCode.Text, hideprc.Value);
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    
    protected void btnCkAll_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                ((CheckBox)GridView1.Rows[i].FindControl("chk")).Checked = true;
            }
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnDelSel_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                if (((CheckBox)GridView1.Rows[i].FindControl("chk")).Checked)
                {
                    string mtr_code = ((TextBox)GridView1.Rows[i].FindControl("txtCodeM")).Text;
                    string query = "update HT_QLT_STDD_CODE_DETAIL set IS_DEL = '1'  where QLT_CODE = '" + txtCode.Text + "' and PARA_CODE = '" + mtr_code + "'";
                   DataBaseOperator opt =new DataBaseOperator();
                    opt.UpDateOra(query);
                }
            }
            bindGrid(txtCode.Text, hideprc.Value);
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            
            if (row.RowIndex >= 0)
            {
                DataBaseOperator opt = new DataBaseOperator();            
              
                string[] seg = { "QLT_CODE", "PARA_CODE", "QLT_TYPE", "LOWER", "UPPER", "REMARK", };
                string[] value = { txtCode.Text, ((DropDownList)row.FindControl("listParaName")).SelectedValue, ((DropDownList)row.FindControl("listtype")).SelectedValue, ((TextBox)row.FindControl("txtLower")).Text, ((TextBox)row.FindControl("txtUpper")).Text, ((TextBox)row.FindControl("txtDscrptM")).Text };
                opt.MergeInto(seg, value, 3, "HT_QLT_STDD_CODE_DETAIL");
                    bindGrid(txtCode.Text, hideprc.Value);
            }
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }


    }



    protected void btnCopy_Click(object sender, EventArgs e)
    {
       DataBaseOperator opt =new DataBaseOperator();
        string query = "select * from HT_QLT_STDD_CODE_DETAIL where QLT_CODE = '" + listtech.SelectedValue + "' and is_del = '0'";
        DataSet data = opt.CreateDataSetOra(query);
        if (data != null && data.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string[] seg = { "QLT_CODE", "PARA_CODE", "QLT_TYPE", "LOWER", "UPPER", "REMARK", };
                string[] value = { listtechC.SelectedValue, row["PARA_CODE"].ToString(), row["QLT_TYPE"].ToString(), row["LOWER"].ToString(), row["UPPER"].ToString(), row["REMARK"].ToString()};
                opt.MergeInto(seg, value, 3, "HT_QLT_STDD_CODE_DETAIL");

            }

        }
        bindGrid(listtechC.SelectedValue, hideprc.Value);
     
    }
    protected void UpdateGrid_Click(object sender, EventArgs e)
    {
        bindGrid(txtCode.Text, hideprc.Value);
    }
    protected void listVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData(listVersion.SelectedValue);
    }
}