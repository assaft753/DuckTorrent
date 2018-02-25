<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-bottom: 0px;
        }
        .auto-style2 {
            margin-left: 80px;
        }
        .auto-style3 {
            width: 66px;
            height: 61px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        &nbsp;
        <p class="auto-style2">
&nbsp;&nbsp;<img class="auto-style3" src="Image/Logo_miniTorrent.png" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; New User<br />
            <br />
            User Name&nbsp; <asp:TextBox ID="UserNameTextBox" runat="server" CssClass="auto-style1"></asp:TextBox>
            <br />
            Password&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" ToolTip="Password must contains at leat 4 characters"></asp:TextBox>
        </p>
        <p class="auto-style2">
            <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" ValidateRequestMode="Disabled"></asp:Label>
            <br />
            <asp:Button ID="RegtBtn" runat="server" OnClick="RegtBtn_Click" Text="Register" />
        <br />
        <br />
        <asp:Button ID="Main" runat="server" OnClick="MainBtn_Click" Text="Main" />
        </p>
    </form>
</body>
</html>
