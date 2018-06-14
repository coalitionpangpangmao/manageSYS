﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Authority_UserConfig : MSYS.Web.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageLoad(sender, e);
        if (!IsPostBack)
        {
           DataBaseOperator opt =new DataBaseOperator();
            opt.bindDropDownList(listApt, "select F_CODE,F_NAME from HT_SVR_ORG_GROUP", "F_NAME", "F_CODE");
            opt.bindDropDownList(listRole, "select * from ht_svr_sys_role t", "F_ROLE", "F_ID");
            bindData();
        }
    }

    protected void bindData()
    {
        string query = "select  t.ID  as 人员ID, t.NAME  as 人员名称,t.MOBILE  as 手机, t.PHONE  as 座机, t.RTXID  as 传真, t.GENDER  as 性别,  t.EMAIL  as 电子邮件, r.f_name  as 组织机构名称,s.f_role as 角色, t.DESCRIPTION  as 描述 from ht_svr_user t left join ht_svr_org_group r on r.f_code = t.levelgroupid left join ht_svr_sys_role s on s.f_id = t.role where t.IS_DELETE = '0' order by t.ID";
       DataBaseOperator opt =new DataBaseOperator();
        GridView1.DataSource = opt.CreateDataSetOra(query);
        GridView1.DataBind();
    }
    protected void ck_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ck = (CheckBox)sender;
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            ((CheckBox)GridView1.Rows[i].FindControl("ck")).Checked = ck.Checked;
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {

        int num = 0;
        string id = "";
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            if (((CheckBox)GridView1.Rows[i].FindControl("ck")).Checked)
            {
                num++;
                id = GridView1.DataKeys[i].Value.ToString();
            }
        }
        if (num == 0 || num > 1)
            ScriptManager.RegisterStartupScript(updtpanel1, this.Page.GetType(), "", "alert('请选择需查看用户!!!');", true);
        else if (num == 1)
        {
            txtID.Text = id;
            string query = "select * from ht_svr_user where ID = '" + txtID.Text + "'";
           DataBaseOperator opt =new DataBaseOperator();
            DataSet data = opt.CreateDataSetOra(query);
            if (data != null && data.Tables[0].Rows.Count > 0)
            {
                txtName.Text = data.Tables[0].Rows[0]["NAME"].ToString();
                txtWeight.Text = data.Tables[0].Rows[0]["WEIGHT"].ToString();
                txtPrt.Text = data.Tables[0].Rows[0]["PARENTID"].ToString();
                txtPhone.Text = data.Tables[0].Rows[0]["MOBILE"].ToString();
                txtCallNO.Text = data.Tables[0].Rows[0]["PHONE"].ToString();
                txtFax.Text = data.Tables[0].Rows[0]["RTXID"].ToString();
                setGender(data.Tables[0].Rows[0]["GENDER"].ToString());
                txtUser.Text = data.Tables[0].Rows[0]["LOGINNAME"].ToString();
                txtPswd.Text = data.Tables[0].Rows[0]["PASSWORD"].ToString();
                txtEmail.Text = data.Tables[0].Rows[0]["EMAIL"].ToString();
                listApt.SelectedValue = data.Tables[0].Rows[0]["LEVELGROUPID"].ToString();
                rdLocal.Checked = (data.Tables[0].Rows[0]["IS_LOCAL"].ToString() == "1");
                rdAsyn.Checked = (data.Tables[0].Rows[0]["IS_SYNC"].ToString() == "1");
                rdDel.Checked = (data.Tables[0].Rows[0]["IS_DELETE"].ToString() == "1");
                txtDscp.Text = data.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                listRole.SelectedValue = data.Tables[0].Rows[0]["ROLE"].ToString();
            }              
              ScriptManager.RegisterStartupScript(updtpanel1, this.Page.GetType(), "", " $('.shade').fadeIn(200);", true);
        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SetBlank();
       DataBaseOperator opt =new DataBaseOperator();
  	string id =  (Convert.ToInt16(opt.GetSegValue("select nvl(max(substr(id,3,5)),0) as id from ht_svr_user ", "ID")) + 1).ToString().PadLeft(5,'0');
        txtID.Text = "07" + id;
        ScriptManager.RegisterStartupScript(updtpanel1, this.Page.GetType(), "", " $('.shade').fadeIn(200);", true);

    }
    protected string getGender()
    {
        if (rdMale.Checked)
            return "男";
        else
            return "女";
    }
    protected void setGender(string gender)
    {
        if (gender == "男")
            rdMale.Checked = true;
        else
            rdFemale.Checked = true;
    }
    protected void btnModify_Click(object sender, EventArgs e)
    {
       DataBaseOperator opt =new DataBaseOperator();
 	opt.UpDateOra("delete from ht_svr_user  where ID = '" + txtID.Text + "'");
        string[] seg = { "ID", "NAME", "WEIGHT", "PARENTID", "MOBILE", "PHONE", "RTXID", "GENDER", "LOGINNAME", "PASSWORD", "EMAIL", "LEVELGROUPID",  "IS_LOCAL", "IS_SYNC", "IS_DELETE", "DESCRIPTION", "ROLE" };
        string[] value = { txtID.Text, txtName.Text, txtWeight.Text, txtPrt.Text, txtPhone.Text, txtCallNO.Text, txtFax.Text, getGender(), txtUser.Text, txtPswd.Text, txtEmail.Text, listApt.SelectedValue,  Convert.ToInt16(rdLocal.Checked).ToString(), Convert.ToInt16(rdAsyn.Checked).ToString(), Convert.ToInt16(rdDel.Checked).ToString(), txtDscp.Text, listRole.SelectedValue };
        if(opt.InsertData(seg, value, "ht_svr_user")!="Success")
            opt.InsertTlog(Session["UserName"].ToString(), Page.Request.UserHostName.ToString(), "修改用户失败， 数据值：" + string.Join(" ", value));
        else
            opt.InsertTlog(Session["UserName"].ToString(), Page.Request.UserHostName.ToString(), "修改用户成功， 数据值：" + string.Join(" ", value));

        

        bindData();
        ScriptManager.RegisterStartupScript(updtpanel1, this.Page.GetType(), "", " $('.shade').fadeOut(200);", true);
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
       DataBaseOperator opt =new DataBaseOperator();
  	for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            if (((CheckBox)GridView1.Rows[i].FindControl("ck")).Checked)
            {
                if(opt.UpDateOra("delete from ht_svr_user where ID = '" + GridView1.DataKeys[i].Value.ToString() + "'")!="Success")
      		    opt.InsertTlog(Session["UserName"].ToString(), Page.Request.UserHostName.ToString(), "删除用户失败， ID：" + GridView1.DataKeys[i].Value.ToString());
        	else
        	    
        	     opt.InsertTlog(Session["UserName"].ToString(), Page.Request.UserHostName.ToString(), "删除用户成功， ID：" + GridView1.DataKeys[i].Value.ToString());    
            }
        	       	     
        	           	    
        }

           


        bindData();
    }

    protected void SetBlank()
    {
        txtID.Text = "";
        txtName.Text = "";
        txtWeight.Text = "";
        txtPrt.Text = "";
        txtPhone.Text = "";
        txtCallNO.Text = "";
        txtFax.Text = "";
        txtUser.Text = "";
        txtPswd.Text = "";
        txtEmail.Text = "";
        listApt.SelectedValue = "";
        listRole.SelectedValue = "";
        rdLocal.Checked = false;
        rdAsyn.Checked = false;
        rdDel.Checked = false;
        txtDscp.Text = "";


    }


}