﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupConfig.aspx.cs" Inherits="Authority_GroupConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>权限配置</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <script type="text/javascript">
        function selChange() {
            var str = $("#listType").val()
            if ($("#listType").val() == 0)
                $("#prtMenu").show();
            else
                $("#prtMenu").hide();
            $("#hide").val($("#listType").val());
            str = $("#hide").val();
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="#">权限管理</a></li>
                <li><a href="#">角色管理</a></li>
            </ul>
        </div>
        <div class="formbody">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div id="usual1" class="usual">
                <div class="itab">
                    <ul>
                        <li><a href="#tab1" class="selected">角色管理</a></li>
                        <li><a href="#tab2">权限配置</a></li>
                    </ul>
                </div>
                <div id="tab1" class="tabson">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <asp:Label ID="Right" runat="server" Text="0000000000000000000000000000000000000000"
                                Visible="False"></asp:Label>

                            <table>
                                <tr>
                                    <td style="border: thin solid #a7b5bc" valign="top">
                                        <asp:TreeView ID="RightTree" runat="server" Width="150px" OnSelectedNodeChanged="RightTree_SelectedNodeChanged">
                                        </asp:TreeView>
                                    </td>
                                    <td>
                                        <ul class="forminfo">
                                            <li>
                                                <label>
                                                    角色<b>*</b></label><asp:TextBox ID="Role" runat="server" class="dfinput" Text="请填写用户组名"
                                                        Style="width: 518px;"></asp:TextBox></li>
                                            <li>
                                                <label>
                                                    组权限<b>*</b></label>
                                                <table>
                                                    <tr>
                                                        <td style="border: thin solid #a7b5bc;">
                                                            <li style="font-size: small; font-weight: bold; vertical-align: middle;">己有权限</li>
                                                            <div style="overflow: auto; width: 200px; height: 200px; border-top: solid 1px #a7b5bc;"  id="Div1" runat="server" align="left">
                                                                <asp:TreeView ID="Acesslist" runat="server" BackColor="White" ForeColor="Black" Height="300px"
                                                                    NodeIndent="10" ShowExpandCollapse="False"
                                                                    Width="180px" ShowCheckBoxes="Leaf">
                                                                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="0px"
                                                                        NodeSpacing="0px" VerticalPadding="0px" />
                                                                    <ParentNodeStyle Font-Bold="False" />
                                                                    <SelectedNodeStyle ForeColor="#5555DD" Font-Underline="True" HorizontalPadding="0px"
                                                                        VerticalPadding="0px" />
                                                                </asp:TreeView>
                                                            </div>
                                                        </td>
                                                        <td align="center" width="100">
                                                            <asp:Button ID="btnAdd" runat="server" Text="<<" class="btn1  auth" OnClick="btnAdd_Click" /><br />
                                                            <br />
                                                            <asp:Button ID="btnMinus" runat="server" Text=">>" class="btn1  auth" OnClick="btnMinus_Click" />
                                                        </td>
                                                        <td style="border: thin solid #a7b5bc;">
                                                            <li style="font-size: small; font-weight: bold; vertical-align: middle;">未分配权限</li>
                                                            <div style="overflow: auto; width: 200px; height: 200px; border-top: solid 1px #a7b5bc;"  id="Div2" runat="server">
                                                                <asp:TreeView ID="Denylist" runat="server" Width="180px" Height="300px" BackColor="White"
                                                                    ForeColor="Black"  ShowExpandCollapse="False"
                                                                    NodeIndent="10" ShowCheckBoxes="Leaf">
                                                                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="0px"
                                                                        NodeSpacing="0px" VerticalPadding="0px" />
                                                                    <ParentNodeStyle Font-Bold="False" />
                                                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                                                                        VerticalPadding="0px" />
                                                                </asp:TreeView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </li>
                                            <li>
                                                <label>
                                                    &nbsp;</label>&nbsp;&nbsp;
                                            <asp:Button ID="btnSave2" runat="server" Text="保存" class="btnmodify  auth" OnClick="btnSave2_Click" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnDel" runat="server" Text="删除" class="btndel  auth" OnClick="btnDel_Click" OnClientClick="javascript:return confirm('确认删除？这将导致部分角色用户无法正常使用！');"/>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnReset" runat="server" Text="重置" class="btnset  auth" OnClick="btnReset_Click" /></li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tab2" class="tabson">
                    <div class="listtitle">
                        权限明细表<span style="position: relative; float: right">
                         
                        </span>
                    </div>
                    <div >
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="GridView1" runat="server" class="grid" AutoGenerateColumns="False" AllowPaging="true"
                                    DataKeyNames="权限ID" PageSize="16"  OnPageIndexChanging ="GridView1_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField      HeaderText="权限ID">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtID" runat="server" DataValueField="权限ID" DataTextField="权限ID"
                                                    Width="50px" CssClass="tbinput" Enabled="False"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField      HeaderText="权限类型">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="listType" runat="server" CssClass="drpdwnlist" Width="80px">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value='0'>菜单</asp:ListItem>
                                                    <asp:ListItem Value='1'>操作</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField      HeaderText="父节点名">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="listPrt" runat="server" CssClass="drpdwnlist" Width="120px" DataSource="<%#bindprt() %>" DataTextField="NAME" DataValueField="ID" OnSelectedIndexChanged="listPrt_OnSelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField      HeaderText="Mapping">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="listMap" runat="server" CssClass="drpdwnlist" Width="250px">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField      HeaderText="权限名称" >
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMenu" runat="server" DataValueField="权限名称" DataTextField="权限名称" Width="180px"  CssClass="tbinput1"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField      HeaderText="描述">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDscrp" runat="server" DataValueField="描述" DataTextField="描述"
                                                    Width="200px" CssClass="tbinput"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField      HeaderText="操作" >
                                            <ItemTemplate>
                                                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn1 auth" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="btn1  auth" OnClick="btnDelete_Click"  OnClientClick="javascript:return confirm('确认删除？');"/>
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
                                <asp:AsyncPostBackTrigger ControlID="GridView1" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
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
