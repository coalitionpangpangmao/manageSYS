﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Plan.aspx.cs" Inherits="Product_Plan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>生产计划管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <script language="javascript" type="text/javascript" src="../My97DatePicker/WdatePicker.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="#">生产管理</a></li>
                <li><a href="#">月度计划管理</a></li>
            </ul>
        </div>
        <div class="formbody">
            <div id="usual1" class="usual">
                <div class="itab">
                    <ul>
                        <li><a href="#tab1" class="selected" id="tabtop1">月度生产计划</a></li>
                        <li><a href="#tab2" id="tabtop2">生产任务编制</a></li>
                    </ul>
                </div>
            </div>
            <div id="tab1" class="tabson">
                <div class="framelist">
                    <div class="listtitle">
                        查询条件<span style="position: relative; float: right">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btnview" OnClick="btnSearch_Click" />
                        </span>
                    </div>
                    <table class="tablelist">
                        <tbody>
                            <tr>
                                <td width="100">时间
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStart" runat="server" class="dfinput1" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>至
                                <asp:TextBox ID="txtStop" runat="server" class="dfinput1" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="listtitle" style="margin-top: 10px">
                        月度生产计划表<span style="position:relative; float:right">
                            <asp:Button ID ="btnUpdate" CssClass ="btnpatch auth" runat ="server" Text ="同步数据" OnClick  ="btnUpdate_Click"  OnClientClick="javascript:return confirm('确认同步？');"   Width ="100px"/>
                               </span>
                        <span style="position: relative; float: right">
                            <asp:Button ID="btnAddPlan" runat="server" Text="新增" class="btnadd auth" OnClick="btnAddPlan_Click"  OnClientClick ="$('#tabtop2').click();"/>
                        </span>
                    </div>
                    <div style="overflow: auto">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="GridView1" runat="server" class="grid" DataKeyNames="ID" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="10" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField   HeaderText="审批状态">
                                            <ItemTemplate>
                                                <asp:Label ID="labAprv" runat="server" CssClass="labstatu" Width="60" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField   HeaderText="下发状态">
                                            <ItemTemplate>
                                                <asp:Label ID="labIssue" runat="server" CssClass="labstatu" Width="60" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:BoundField    DataField="计划名" HeaderText="计划名" />
                                         <asp:BoundField    DataField="是否有调整" HeaderText="是否有调整" />
                                         <asp:BoundField    DataField="编制人" HeaderText="编制人" />
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnGridEdit" runat="server" Text="编制计划" CssClass="btn1" Width="75" OnClientClick ="$('#tabtop2').click();"
                                                    OnClick="btnGridEdit_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnSubmit" runat="server" Text="提交审批" CssClass="btn1 auth" Width="75"
                                                    OnClick="btnSubmit_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnFLow" runat="server" Text="审批进度" CssClass="btn1 auth" Width="75"
                                                    OnClick="btnFLow_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnIssued" runat="server" Text="下发" class="btn1 auth" OnClick="btnIssued_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnGridDel" runat="server" Text="删除" class="btn1 auth" OnClick="btnGridDel_Click" OnClientClick="javascript:return confirm('确认删除该月生产计划吗？');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnGridAlter" runat="server" Text="调整计划" class="btn1 adjust" OnClick="btnGridAlter_Click" Width="75" OnClientClick ="$('#tabtop2').click();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="gridrow" />
                                    <AlternatingRowStyle CssClass="gridalterrow" />
                                    <PagerStyle CssClass="gridpager" />
                                <PagerTemplate>
                                    <asp:Label ID="lblPage" runat="server" Text='<%# "第" + (((GridView)Container.NamingContainer).PageIndex + 1)  + "页/共" + (((GridView)Container.NamingContainer).PageCount) + "页" %> ' Width="120px"></asp:Label>
                                    <asp:LinkButton ID="lbnFirst" runat="Server" Text="首页" Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>' CommandName="Page" CommandArgument="First"></asp:LinkButton>
                                    <asp:LinkButton ID="lbnPrev" runat="server" Text="上一页" Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>' CommandName="Page" CommandArgument="Prev"></asp:LinkButton>
                                    <asp:LinkButton ID="lbnNext" runat="Server" Text="下一页" Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != (((GridView)Container.NamingContainer).PageCount - 1) %>' CommandName="Page" CommandArgument="Next"></asp:LinkButton>
                                    <asp:LinkButton ID="lbnLast" runat="Server" Text="尾页" Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != (((GridView)Container.NamingContainer).PageCount - 1) %>' CommandName="Page" CommandArgument="Last"></asp:LinkButton>
                                    到第
                                <asp:TextBox ID="txtNewPageIndex" runat="server" Width="20px" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />
                                    页  
             <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-2"
                 CommandName="Page" Text="跳转" />
                                </PagerTemplate>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                                <asp:AsyncPostBackTrigger ControlID="btnModify" />
                                <asp:AsyncPostBackTrigger ControlID="Gridview1" />

                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div id="tab2" class="tabson">
               
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="listtitle">
                                    月度生产计划<span style="position: relative; float: right" class="click2">
                                        <asp:Button ID="btnModify" class="btnmodify auth" runat="server" Text="保存" OnClick="btnModify_Click" />
                                    </span>
                                </div>
                                <table class="tablelist">
                                    <tbody>
                                        <tr>
                                            <td width="100">年度
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtYear" runat="server" class="dfinput1" onclick="WdatePicker({dateFmt:'yyyy'})"></asp:TextBox>
                                            </td>
                                            <td width="100">月份
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="listMonth" runat="server" CssClass="drpdwnlist" Width="70px">
                                                    <asp:ListItem Value="01">1月</asp:ListItem>
                                                    <asp:ListItem Value="02">2月</asp:ListItem>
                                                    <asp:ListItem Value="03">3月</asp:ListItem>
                                                    <asp:ListItem Value="04">4月</asp:ListItem>
                                                    <asp:ListItem Value="05">5月</asp:ListItem>
                                                    <asp:ListItem Value="06">6月</asp:ListItem>
                                                    <asp:ListItem Value="07">7月</asp:ListItem>
                                                    <asp:ListItem Value="08">8月</asp:ListItem>
                                                    <asp:ListItem Value="09">9月</asp:ListItem>
                                                    <asp:ListItem Value="10">10月</asp:ListItem>
                                                    <asp:ListItem Value="11">11月</asp:ListItem>
                                                    <asp:ListItem Value="12">12月</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100">备注
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtRemark" runat="server" class="dfinput1" Height="80px"
                                                    Width="600px"></asp:TextBox>
                                            </td>

                                        </tr>
                                    </tbody>
                                </table>
                                <div class="listtitle" style="margin-top: 10px">
                                    生产任务编制<span style="position: relative; float: right">
                                          <asp:Button ID="btnCkAll" runat="server" CssClass="btnset" Text="全选" OnClick="btnCkAll_Click" />
                                        <asp:Button ID="btnAdd" runat="server"
                                        CssClass="btnadd  auth" Text="新增" OnClick="btnAdd_Click" />  
                                        <asp:Button ID="btnDelSel" runat="server" CssClass="btndel auth" Text="删除" OnClick="btnDelSel_Click" OnClientClick="javascript:return confirm('确认删除所选生产计划吗？');" />
                                              <asp:Button ID="btnGrid2Modify" class="btnmodify auth" runat="server" Text="全部保存" Width ="90px" OnClick="btnGrid2Modify_Click" />
                                    </span>
                                </div>
                                <asp:HiddenField ID="hidePlanID" runat="server" />
                                <asp:HiddenField ID="hidePzcode" runat="server" />
                                <asp:HiddenField ID="hideAdjust" runat="server" />
                                <asp:GridView ID="GridView2" runat="server" class="grid" AllowPaging="True" AutoGenerateColumns="False"
                                    DataKeyNames="计划号" HeaderStyle-Wrap="False">
                                    <Columns>
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                            <ItemTemplate>
                                                 <asp:Button ID="btnUp" runat="server"  CssClass="btnup auth" OnClick="btnUp_Click"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                            <ItemTemplate>
                                                   <asp:Button ID="btnDown" runat="server"  CssClass="btndown auth" OnClick="btnDown_Click"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField   HeaderText="顺序号" SortExpression="顺序号">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOrder" runat="server" DataValueField="顺序号" DataTextField="顺序号"
                                                    CssClass="tbinput1" Width="40px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField   HeaderText="计划号" SortExpression="计划号">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPlanNo" runat="server" DataValueField="计划号" DataTextField="计划号" Width="150px" CssClass="tbinput1" Enabled="False"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField   HeaderText="产品名称" SortExpression="产品名称">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="listProd" runat="server" DataSource='<%# ddlbind()%>' DataValueField="prod_code"
                                                    CssClass="drpdwnlist" DataTextField="产品名称">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField   HeaderText="计划产量(吨)" SortExpression="计划产量">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOutput" runat="server" DataValueField="计划产量" DataTextField="计划产量" onkeyup="value=value.replace(/[^\d\.]/g,'')"
                                                    CssClass="tbinput"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField   HeaderText="路径编码">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPathCode" runat="server" DataValueField="路径编码" DataTextField="路径编码" Enabled="false" Width="150px"
                                                    CssClass="tbinput"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      
                                     
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnGrid2Save" runat="server" Text="保存" CssClass="btn1 auth" OnClick="btnGrid2Save_Click" OnClientClick="javascript:return confirm('确认保存该生产计划吗？');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnGrid2Del" runat="server" Text="删除" CssClass="btn1 auth" OnClick="btnGrid2Del_Click" OnClientClick="javascript:return confirm('确认删除该生产计划吗？');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField   HeaderText="生产状态">
                                            <ItemTemplate>
                                                <asp:Label ID="labexe" runat="server" CssClass="labstatu" Width="60" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField   HeaderText="原料状态">
                                            <ItemTemplate>
                                                <asp:Label ID="labmater" runat="server" CssClass="labstatu" Width="60" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField   HeaderText="回填液状态">
                                            <ItemTemplate>
                                                <asp:Label ID="labcoat" runat="server" CssClass="labstatu" Width="60" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField   HeaderText="香精香料状态">
                                            <ItemTemplate>
                                                <asp:Label ID="labflavor" runat="server" CssClass="labstatu" Width="60" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnGrid2Apply" runat="server" Text="要料" CssClass="btn1 auth" OnClick="btnGrid2Apply_Click" OnClientClick="javascript:return confirm('确认要料？');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:Button ID="btnGrid2Feed" runat="server" Text="到料" CssClass="btn1 auth" OnClick="btnGrid2Feed_Click" OnClientClick="javascript:return confirm('确认到料？');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="gridrow" />
                                    <AlternatingRowStyle CssClass="gridalterrow" />
                                </asp:GridView>
                                 <AlternatingRowStyle CssClass="gridalterrow" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelSel" />
                                <asp:AsyncPostBackTrigger ControlID="GridView2" />
                                <asp:AsyncPostBackTrigger ControlID="GridView1" />
                                <asp:AsyncPostBackTrigger ControlID="btnAddPlan" />                               
                                <asp:AsyncPostBackTrigger ControlID ="btnGrid2Modify" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>               

            </div>

            <div class="aprvinfo" id="flowinfo">
                <div class="tiptop">
                    <span>审批流程详情</span><a onclick=" $('#flowinfo').fadeOut(100);"></a>
                </div>
                <div class="flowinfo">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="GridView3" runat="server" class="grid">
                                <HeaderStyle CssClass="gridheader" />
                                <RowStyle CssClass="gridrow" />
                                <AlternatingRowStyle CssClass="gridalterrow" />
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="GridView1" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            
            <script type="text/javascript">
                $("#usual1 ul").idTabs();
            </script>
            <script type="text/javascript">
                $('.tablelist tbody tr:odd').addClass('odd');
            </script>
        </div>
    </form>
</body>
</html>
