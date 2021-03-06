﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Inspect_Process.aspx.cs"
    Inherits="Quality_Inspect_Process" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>过程检验</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <script type="text/javascript" src="../js/vue.js"></script>
     <script type="text/javascript" src="../js/axios.js"></script>
      <script language="javascript" type="text/javascript" src="../My97DatePicker/WdatePicker.js"></script>  
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                  <li><a href="#">质量评估与分析</a></li>
                  <li><a href="#">离线数据评估</a></li>
                <li><a href="#">过程检验</a></li>
            </ul>
        </div>
        <div class="formbody" style="width:auto">
             <div id="usual1" class="usual">
                <div class="itab">
                    <ul>
                        <li><a href="#tab1" class="selected" id="tabtop1">批量检验详情</a></li>
                        <li><a href="#tab2" id="tabtop2">检验详情</a></li>
                  
                    </ul>
                </div>
            </div>
                           <div id="tab1" class="tabson">
                <div class="framelist">
                    <div class="listtitle">
                批量检验详情 <span style="position: relative; float: right">
                    </span>

                </div>
                    <table class="tablelist">
                <tbody>
                    <tr>
                           <td width="100">起始时间：
                        </td>
                        <td>
                            <asp:TextBox ID="start_time" runat="server" class="dfinput1" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>  
                        </td>
                                                   <td width="100">结束时间：
                        </td>
                        <td>
                            <asp:TextBox ID="end_time" runat="server" class="dfinput1" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>  
                        </td>
                        <td width="100">产品：
                        </td>
                        <td><asp:DropDownList ID="prod_code" runat="server" CssClass="drpdwnlist">
                            </asp:DropDownList>
                            
                        </td> 
                       
                        </tr>
                    <tr>

                       <td width="100">班组：
                        </td>
                        <td>
                            <asp:DropDownList ID="team_code" runat="server" CssClass="drpdwnlist">
                                <asp:ListItem Value="00">全选</asp:ListItem>
                                <asp:ListItem Value="01">甲班</asp:ListItem>
                                <asp:ListItem Value="02">乙班</asp:ListItem>
                                <asp:ListItem Value="03">丙班</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                         <td width="100">班时：
                        </td>
                        <td>
                            <asp:DropDownList ID="schedule_time" runat="server" CssClass="drpdwnlist">
                                <asp:ListItem Value="00">全时段</asp:ListItem>
                                <asp:ListItem Value="01">早班</asp:ListItem>
                                <asp:ListItem Value="02">中班</asp:ListItem>
                                <asp:ListItem Value="03">晚班</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>

                        </td>
                    </tr>
                </tbody>
            </table>
                    <div id="gridview">
                        <div id="view">
                            <div>
                            <div style="display:inline-block; text-align:center; margin-left:3px; float:right" class="btn1"  @click="getRows">查询</div>
                            <div style="display:inline-block; float:right" class="btn1"  @click="saveAll">全部保存</div>
                             </div>
                           <table class="grid" cellspacing="0" rules="all" id="grid" style=" border-collapse:collapse; width:max-content" @keydown="keyDown($event)">
                               <tbody>
                                   <tr class="gridheader">
                                       <th class="proc" scope="col">记录时间</th>
                                       <th class="proc" scope="col">产品</th>
                                       <th class="proc" scope="col">班组</th>
                                       <th class="proc" scope="col">班时</th>
                                       <th class="proc" v-for="ti in title"  scope="col">{{ti}}</th>
                                       <th class="proc" scope="col">操作</th>
                                    </tr>
                                    <tr v-for="(date , dindex) in dates" class="gridrow">
                                         <td><input class="tbinput1" id="Text1" type="text" :value="date[0]"></td>
                                         <td><input class="tbinput1" id="Text2" type="text" :value="prod"></td>
                                         <td><input class="tbinput1" id="Text3" type="text" :value="date[4]"></td>
                                         <td>
                                             <input class="tbinput1" id="Text4" type="text" :value="date[2]"></td>
                                         </td>
                                        <td v-if="rowsDates.indexOf(date[0])!=-1 &&-1!= rows.findIndex((ele)=>{if(ele[0]==date[0] && date[4]==ele[2]&&date[2]==ele[3]){return true;}}) && index>3 &&index<(title.length+4)" v-for="(item, index) in rows[rows.findIndex((ele)=>{if(ele[0]==date[0] && date[4]==ele[2]&&date[2]==ele[3]){return true;}})]">
                                            <input :id="dindex.toString()+(index-4).toString()" class="tbinput1" @change="change(dindex, rows.findIndex((ele)=>{if(ele[0]==date[0] && date[4]==ele[2]&&date[2]==ele[3]){return true;}}),index, $event.target.value)" type="text" :value="item">
                                        </td>
                                        <td v-if="rowsDates.indexOf(date[0])==-1||-1== rows.findIndex((ele)=>{if(ele[0]==date[0] && date[4]==ele[2]&&date[2]==ele[3]){return true;}})"  v-for="(item, index) in title">
                                            <input class="tbinput1" @change="add(dindex)" :id="dindex.toString()+index.toString()" type="text" value="">
                                        </td>
                                        <td>
                                            <div class="btn1" @click="show(dindex)">详情</div>
                                            <div class="btn1" style="display:none" @click="save(dindex)">保存</div>
                                        </td>
                                    </tr>
                                </tbody>
                           </table>
                            </div>
                        </div>
                 </div>
            </div>
            <div id="tab2" class="tabson">
            <div class="listtitle">
                检验详情<span style="position: relative; float: right">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btnview auth" OnClick="btnSave_Click" />
                    </span>
            </div>
            <table class="tablelist">
                <tbody>
                    <tr>
                           <td width="100">生产日期：
                        </td>
                        <td>
                            <asp:TextBox ID="txtProdTime" runat="server" class="dfinput1"  OnTextChanged="listProd_SelectedIndexChanged" AutoPostBack="True" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>  
                        </td>
                        <td width="100">产品：
                        </td>
                        <td>
                            <asp:DropDownList ID="listProd" runat="server" CssClass="drpdwnlist" OnSelectedIndexChanged ="listProd_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>  <td width="100">记录人：
                        </td>
                        <td>
                            <asp:DropDownList ID="listEditor" runat="server" CssClass="drpdwnlist">
                            </asp:DropDownList>
                        </td>
                       
                        </tr>
                    <tr>
                       <td width="100">班组：
                        </td>
                        <td>
                            <asp:DropDownList ID="listTeam"  OnTextChanged="listProd_SelectedIndexChanged" AutoPostBack="True" runat="server" CssClass="drpdwnlist">
                            </asp:DropDownList>
                        </td>
                         <td width="100">班时：
                        </td>
                        <td>
                            <asp:DropDownList ID="listShift" runat="server" CssClass="drpdwnlist">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GridView1" runat="server" class="grid" AllowPaging="false" AutoGenerateColumns="False"
                            DataKeyNames="inspect_code">
                            <Columns>
                                <asp:BoundField DataField="section_name" HeaderText="工艺段" />
                                <asp:BoundField HeaderText="检验项目" DataField="inspect_name"/>
                                <asp:TemplateField HeaderText="检测值">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPara" runat="server" CssClass='tbinput1'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="标准范围">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValue" runat="server" CssClass='tbinput1' Enabled="False"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="状态">
                                    <ItemTemplate>
                                        <asp:Label ID="labStatus" runat="server" Text="" CssClass="labstatu" Width ="50px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="扣分">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtScore" runat="server" CssClass='tbinput1' Enabled="False"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                           
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <RowStyle CssClass="gridrow" />
                            <AlternatingRowStyle CssClass="gridalterrow" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="listProd" />
                        <asp:AsyncPostBackTrigger ControlID ="btnSave" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            </div>

        </div>
    </form>
        <script type="text/javascript">
            $("#usual1 ul").idTabs();
    </script>
        <script type="text/javascript" src="../js/msys/Inspect_Process.js"></script>
</body>
</html>
