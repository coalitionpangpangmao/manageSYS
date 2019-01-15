﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Inspect_online.aspx.cs" Inherits="Quality_Inspect_online" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>在线数据统计</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script language="javascript" type="text/javascript" src="../My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="#">质量评估与分析</a></li>
                <li><a href="#">在线数据评估</a></li>     
                <li><a href="#">在线数据统计</a></li>
            </ul>
        </div>

        <div class="mainbox">
              <div class="listtitle">
                        查询条件<span style="position: relative; float: right">
                               <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btnview" OnClick="btnSearch_Click" />
                        </span>
                    </div>
              <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
            <table class="tablelist" style="margin-bottom: 5px">
                <tbody>
                    <tr>
                        <td >                          
                                    时间  
                            </td>
                        <td>   
                    <asp:TextBox ID="txtBtime" runat="server" CssClass="dfinput1"
                        onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" OnTextChanged="txtBtime_TextChanged" AutoPostBack="true" Width ="90px"></asp:TextBox>至
                                    <asp:TextBox ID="txtEtime" runat="server" CssClass="dfinput1"
                                        onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" OnTextChanged="txtEtime_TextChanged"  AutoPostBack="true" Width ="90px"></asp:TextBox>
                            </td>
                        <td>
                                    产品：
                            </td>
                        <td><asp:DropDownList ID="listProd" runat="server" CssClass="drpdwnlist" Width ="200px"></asp:DropDownList></td>
                        <td>
                                    工艺段：</td>
                        <td><asp:DropDownList ID="listSection" runat="server" CssClass="drpdwnlist" OnSelectedIndexChanged="listSection_SelectedIndexChanged" AutoPostBack="true" Width ="200px"></asp:DropDownList>
                                  </td></tr>
                    <tr>
                        <td>  批次：</td>
                        <td><asp:DropDownList ID="listBatch" runat="server" CssClass="drpdwnlist" Width ="200px"></asp:DropDownList>
                                                    
                          </td>
                        <td>  班组：</td>
                        <td><asp:DropDownList ID="listTeam" runat="server" CssClass="drpdwnlist" Width ="200px"></asp:DropDownList>
                                                    
                          </td>
                        <td>  数据点：</td>
                        <td><asp:DropDownList ID="listPoint" runat="server" CssClass="drpdwnlist" Width ="200px"></asp:DropDownList>
                                                    
                          </td>
                    </tr>
                </tbody>
            </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtBtime" />
                                    <asp:AsyncPostBackTrigger ControlID ="txtEtime" />
                                    <asp:AsyncPostBackTrigger ControlID="listSection" />
                                </Triggers>
                            </asp:UpdatePanel>
                      
            <div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GridView1" runat="server" class="grid" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="15" HeaderStyle-Wrap="False" RowStyle-Wrap="False">

                            <HeaderStyle CssClass="gridheader" />
                            <RowStyle CssClass="gridrow" BorderStyle="Solid" BorderWidth="1" />
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
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>





    </form>
</body>

</html>
