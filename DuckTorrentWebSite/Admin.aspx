<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #Select1 {
            width: 496px;
            height: 9px;
        }

        .auto-style3 {
            margin-right: 0px;
            margin-top: 58px;
        }

        .auto-style4 {
            height: 96px;
            width: 121px;
        }

        .auto-style5 {
            margin-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-left: 560px" class="auto-style4">
            <asp:Image ID="Image1" runat="server" Height="92px" ImageUrl="~/Image/Logo_miniTorrent.png" Width="102px" ImageAlign="Left" />
            <br />
        </div>
        <asp:Panel ID="Panel1" runat="server" BackColor="#FFC300" BorderStyle="Dashed" CssClass="auto-style5" Height="127px" HorizontalAlign="Center" Width="255px">
            <asp:Label ID="Label1" runat="server" Text="Total Users : "></asp:Label>
            <asp:Label ID="TotalUsers" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Online Users: "></asp:Label>
            <asp:Label ID="OnlineUsers" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label3" runat="server" Text="Total Files: "></asp:Label>
            <asp:Label ID="TotalFiles" runat="server" Text="Label"></asp:Label>
            <br />
        </asp:Panel>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Height="290px" Style="margin-bottom: 52px" AutoGenerateColumns="False" CssClass="auto-style3" DataKeyNames="UserName" DataSourceID="SqlDataSource1" Width="450px" OnRowDeleted="GridView1_RowDeleted" AllowPaging="True" CellSpacing="1" DataMember="DefaultView">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" ButtonType="Button" />
                <asp:CommandField ShowDeleteButton="True" ButtonType="Button" />
                <asp:CommandField ShowEditButton="True" ButtonType="Button" />
                <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
                <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                <asp:BoundField DataField="IsEnable" HeaderText="IsEnable" SortExpression="IsEnable" />
            </Columns>
            <EditRowStyle BackColor="#7C6F57" BorderColor="#FF9933" HorizontalAlign="Center" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" BorderColor="#FF9900" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#D4DFE1" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DuckTorrentDBConnectionString %>" SelectCommand="SELECT [UserName], [Password], [IsEnable] FROM [Users]" ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [Users] WHERE [UserName] = @original_UserName AND [Password] = @original_Password AND [IsEnable] = @original_IsEnable" InsertCommand="INSERT INTO [Users] ([UserName], [Password], [IsEnable]) VALUES (@UserName, @Password, @IsEnable)" OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [Users] SET [Password] = @Password, [IsEnable] = @IsEnable WHERE [UserName] = @original_UserName AND [Password] = @original_Password AND [IsEnable] = @original_IsEnable" OnSelecting="SqlDataSource1_Selecting">
            <DeleteParameters>
                <asp:Parameter Name="original_UserName" Type="String" />
                <asp:Parameter Name="original_Password" Type="String" />
                <asp:Parameter Name="original_IsEnable" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="UserName" Type="String" />
                <asp:Parameter Name="Password" Type="String" />
                <asp:Parameter Name="IsEnable" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="Password" Type="String" />
                <asp:Parameter Name="IsEnable" Type="Int32" />
                <asp:Parameter Name="original_UserName" Type="String" />
                <asp:Parameter Name="original_Password" Type="String" />
                <asp:Parameter Name="original_IsEnable" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <br />
        <asp:Label ID="Label4" runat="server" Text="File name:" ToolTip="Choose a file to show"></asp:Label>
        <asp:TextBox ID="SearchText" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Search" OnClick="Button1_Click" Height="45px" />
        <br />
        <asp:GridView ID="GridViewSearch" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="UserName,IP,Port,FIleName" DataSourceID="SqlDataSource3" Visible="False" Width="900px">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
                <asp:BoundField DataField="IP" HeaderText="IP" ReadOnly="True" SortExpression="IP" />
                <asp:BoundField DataField="Port" HeaderText="Port" ReadOnly="True" SortExpression="Port" />
                <asp:BoundField DataField="FIleName" HeaderText="FIleName" ReadOnly="True" SortExpression="FIleName" />
                <asp:BoundField DataField="FileSize" HeaderText="FileSize" SortExpression="FileSize" />
            </Columns>
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <RowStyle BackColor="White" ForeColor="#003399" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <SortedAscendingCellStyle BackColor="#EDF6F6" />
            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
            <SortedDescendingCellStyle BackColor="#D6DFDF" />
            <SortedDescendingHeaderStyle BackColor="#002876" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:DuckTorrentDBConnectionString %>" SelectCommand="SELECT * FROM [Files] WHERE ([FIleName] = @FIleName)">
            <SelectParameters>
                <asp:ControlParameter ControlID="SearchText" Name="FIleName" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <br />
        <br />
        <asp:GridView ID="GridView2" runat="server" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="False" DataKeyNames="UserName,IP,Port,FIleName" DataSourceID="SqlDataSource2" Width="900px">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
                <asp:BoundField DataField="IP" HeaderText="IP" ReadOnly="True" SortExpression="IP" />
                <asp:BoundField DataField="Port" HeaderText="Port" ReadOnly="True" SortExpression="Port" />
                <asp:BoundField DataField="FIleName" HeaderText="FIleName" ReadOnly="True" SortExpression="FIleName" />
                <asp:BoundField DataField="FileSize" HeaderText="FileSize" SortExpression="FileSize" />
            </Columns>
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <RowStyle ForeColor="#003399" BackColor="White" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <SortedAscendingCellStyle BackColor="#EDF6F6" />
            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
            <SortedDescendingCellStyle BackColor="#D6DFDF" />
            <SortedDescendingHeaderStyle BackColor="#002876" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:DuckTorrentDBConnectionString %>" SelectCommand="SELECT * FROM [Files]"></asp:SqlDataSource>
        <br />
    </form>
</body>
</html>
