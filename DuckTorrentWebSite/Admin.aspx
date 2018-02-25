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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-left: 560px">
            <asp:Image ID="Image1" runat="server" Height="155px" ImageUrl="~/Image/Logo_miniTorrent.png" Width="166px" />
        </div>
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Text="Total Users : "></asp:Label>
        <asp:Label ID="TotalUsers" runat="server" Text="Label"></asp:Label>
        <p style="margin-left: 480px">
            <asp:Label ID="Label2" runat="server" Text="Online Users: "></asp:Label>
            <asp:Label ID="OnlineUsers" runat="server" Text="Label"></asp:Label>
        </p>
        <div style="height: 43px; margin-left: 480px">
            <asp:Label ID="Label3" runat="server" Text="Total Files: "></asp:Label>
        <asp:Label ID="TotalFiles" runat="server" Text="Label"></asp:Label>
            <br />
        </div>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Height="403px" style="margin-bottom: 52px">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="True" SortExpression="UserName" />
                <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
            </Columns>
               <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" BorderColor="#FF9933" />
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
        <asp:Label ID="Label4" runat="server" Text="File name:" ToolTip="Choose a file to show" ></asp:Label>
        <select id="Select1" name="D1">
            <option></option>
        </select><br />
        <br />
        <br />
        <asp:GridView ID="GridView2" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
           <Columns>
                <asp:BoundField DataField="FileName" HeaderText="File Name" ReadOnly="True" SortExpression="FileName" />
                <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="True" SortExpression="Size" />
                <asp:BoundField DataField="UseerName" HeaderText="User Name" ReadOnly="True" SortExpression="UseerName" />
                <asp:BoundField DataField="Ip" HeaderText="Ip" ReadOnly="True" SortExpression="Ip" />
            </Columns>
             <FooterStyle BackColor="White" ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
        <br />
    </form>
</body>
</html>
