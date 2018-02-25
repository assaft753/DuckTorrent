<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainMenu.aspx.cs" Inherits="MainMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 183px;
            height: 169px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Welcome to DuckTorrent!<br />
        <br />
        <img class="auto-style1" src="Image/Logo_miniTorrent.png" /><br />
        <br />
        <asp:Button ID="new_user" runat="server" OnClick="Regst_Click" Text="New User Registration" Width="400px" BackColor="#FFC100" BorderStyle="Groove" BorderWidth="5px" ForeColor="Black" />
        <br />
        <br />
        <asp:Button ID="admin" runat="server" OnClick="Admin_Click" Text="Administration" Width="400px" BackColor="#FFC100" BorderStyle="Groove" BorderWidth="5px" />
    
    </div>
    </form>
</body>
</html>
