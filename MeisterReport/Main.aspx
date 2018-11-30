<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Main" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
      <%--  <asp:ScriptManager runat="server" ID="scriptManager" EnablePageMethods="True">
</asp:ScriptManager>--%>
            Meister Reporter
            <br />
            <br />
            Select an options:<br />
            
            <br />
            <asp:Button ID="Button7" runat="server" OnClick="Button7_Click" Text="Show Agenda" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button3" runat="server" Text="Show my Reports" OnClick="Button3_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button4" runat="server" Text="Schedule a Report" OnClick="Button4_Click" />
            <br />
            <br />
            <div id="Retrieval" runat="server">
            <br />
            <asp:GridView ID="GridView3" runat="server" AutoGenerateSelectButton="True" Caption="Reports found for user" AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanged="GridView3_PageIndexChanged" OnPageIndexChanging="GridView3_PageIndexChanging" OnSelectedIndexChanged="GridView3_SelectedIndexChanged" OnRowDeleting="GridView3_RowDeleting">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
            <br />
            <br />
            <br />
            </div>
         <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
            <div id="SearchSAP" runat ="server">
            <asp:Label ID="Label1" runat="server" Text="Enter a t-code or report hint"></asp:Label>
            
            <br />
            <br />
            <br />
                        <asp:TextBox ID="TextBox1" runat="server" ToolTip="Try RFAUSZ00"></asp:TextBox>
               
                <asp:DropDownList ID="ddpDemo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddpDemo_SelectedIndexChanged">
                    <asp:ListItem>RFAUSZ00</asp:ListItem>
                    <asp:ListItem>SD_SALES_ORDERS_VIEW</asp:ListItem>
                    <asp:ListItem>RFSKPL00</asp:ListItem>
                    <asp:ListItem>RFEPOJ00</asp:ListItem>
                </asp:DropDownList>
               
            <asp:Button ID="Button1" runat="server" Text="Search SAP" OnClick="Button1_Click" />
                 </div>
                <div id="BeforeSearch" runat="server">
            <br />
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateSelectButton="True" CellPadding="4" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ShowFooter="True" ShowHeaderWhenEmpty="True" Width="677px" Caption="Reports found at SAP matching the Hint passed " OnPageIndexChanging="GridView1_PageIndexChanging" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
                    <br />
            <br />
                    </div>
            <div id="AfterSeach" runat ="server">
                <asp:GridView ID="GridView4" runat="server" AutoGenerateSelectButton="True" Caption="Variants found for chosen report" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GridView4_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
                </div>
                <br />
                <br />
                 
        <div id="ShowParms" runat="server">
            <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True" CellPadding="4" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ShowFooter="True" ShowHeaderWhenEmpty="True" Width="830px" AutoGenerateEditButton="True" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating" Caption="Parameters Found for report" ForeColor="#333333" GridLines="None" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound" OnRowUpdated="GridView2_RowUpdated">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView><br />
            </div>
            <div id="VariantSave" runat="server">
                <asp:Label ID="Foo" runat="server">Please provide a name and description for this Variant</asp:Label>
                <br />
                <br />
                <asp:Label ID="Label7" runat="server" Text="Variant Name:"></asp:Label>
&nbsp;
                <asp:TextBox ID="TextBox4" runat="server" AutoPostBack="True" OnTextChanged ="TextBox4_TextChanged" Width="243px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label8" runat="server" Text="Description:"></asp:Label>
&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBox5" runat="server" AutoPostBack="True" OnTextChanged="TextBox5_TextChanged" Width="245px"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="Button6" runat="server" Text="Save as New Variant" OnClick="Button6_Click" Enabled="False" />

                &nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="CheckBox1" runat="server" Text="Override" />

            </div>
            <br />
                  <div id="BeforeB2" runat="server">
                <asp:Label ID="Label3" runat="server" Text="Selections for Response:"></asp:Label>
                
                      <br />
                
            <br />
                <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" Text="Return EDM" />
                &nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="RadioButton2" runat="server" Text="Named Columns" />
                      <br />
                      <br />
                      Recurrence:<br />
                <br />
                      <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" RepeatDirection="Horizontal">
                          <asp:ListItem Selected="True" Value="I">Immediate</asp:ListItem>
                          <asp:ListItem Value="D">Daily</asp:ListItem>
                          <asp:ListItem Value="W">Weekly</asp:ListItem>
                          <asp:ListItem Value="Q">Quarterly</asp:ListItem>
                          <asp:ListItem Value="S">Semi-Annually</asp:ListItem>
                          <asp:ListItem Value="A">Annually</asp:ListItem>
                      </asp:RadioButtonList>
                      <br />
                      <asp:Panel ID="DOWs" runat="server">
                          <asp:Label ID="lbDOW" runat="server"></asp:Label>
                          <br />
                          <asp:RadioButtonList ID="RadioButtonList2" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonList2_SelectedIndexChanged">
                              <asp:ListItem Value="S">Sunday</asp:ListItem>
                              <asp:ListItem Value="M">Monday</asp:ListItem>
                              <asp:ListItem Value="T">Tuesday</asp:ListItem>
                              <asp:ListItem Value="W">Wednesday</asp:ListItem>
                              <asp:ListItem Value="R">Thursday</asp:ListItem>
                              <asp:ListItem Value="F">Friday</asp:ListItem>
                              <asp:ListItem Value="X">Saturday</asp:ListItem>
                          </asp:RadioButtonList>
                          <br />
                          Hour Slot:&nbsp;&nbsp;&nbsp;
                          <asp:TextBox ID="TextBox7" runat="server" CausesValidation="True" MaxLength="2" Width="33px" OnTextChanged="TextBox7_TextChanged" ToolTip="00-23 as Hour" AutoPostBack="True"></asp:TextBox>
                          <br />
                          <br />
                          <asp:Label ID="Label9" runat="server" Text="Nickname:"></asp:Label>
                          &nbsp;&nbsp;
                          <asp:TextBox ID="txtNickName" runat="server" Width="375px"></asp:TextBox>
                          <br />
                          <br />
                          <asp:Button ID="Button8" runat="server" OnClick="Button8_Click" Text="Create Agenda Item" />
                          <br />                 
                     </asp:Panel>
        <p>
            <asp:Button ID="Button2" runat="server" Text="Execute" OnClick="Button2_Click" />
        </p>    
            </div>
       <div id="AfterB2" runat="server">
        <p>
            <asp:Label ID="Label4" runat="server" Text="Report Unique Identifier"></asp:Label>
            <br />
            <br />
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        </p>
          </div>
       <asp:Button ID="Button9" runat="server" Text="Delete Item" onclick="ConfirmDelete_Click" Visible="False" />
                    &nbsp;&nbsp;
            <asp:CheckBox ID="CheckBox2" AutoPostBack="true" runat ="server" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Confirm Deletion?" Visible="False" />
      
        <p>
            <hr />
            <asp:Label ID="msgLabel" runat="server" Text="SAP Messages .... "></asp:Label>
            <asp:TextBox ID="TextBox3" runat="server" Height="45px" Width="749px" BorderStyle="None" AutoPostBack="True"></asp:TextBox>
            <p>
            </p>
        </p>
             </ContentTemplate>
               <Triggers>
<%--                <asp:AsyncPostBackTrigger ControlID="Button9" />--%>
            </Triggers>
       </asp:UpdatePanel>    
    </form>
</body>
</html>
