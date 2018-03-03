<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="LogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 105px;
            height: 97px;
        }

        .auto-style2 {
            margin-left: 80px;
        }

        .auto-style3 {
            margin-left: 80px;
            width: 789px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <p class="auto-style3">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Log In<br />
            <br />
            User Name&nbsp;
            <asp:TextBox ID="UserNameTextBox" runat="server"></asp:TextBox>
            <br />
            Password&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" ToolTip="Password must contains at leat 4 characters"></asp:TextBox>
        </p>
        <p class="auto-style2">
            <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" ValidateRequestMode="Disabled"></asp:Label>
            <br />
            <asp:Button ID="loginBtn" runat="server" OnClick="LoginBtn_Click" Text="log In" Width="275px" />
            <br />
            <br />
        </p>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <img class="auto-style1" src="Image/Logo_miniTorrent.png" />
        </p>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="backButton" runat="server" OnClick="Button1_Click" Text="Back" Style="margin-left: 17px" Width="168px" BackColor="WhiteSmoke" />
        </p>
        <p>
            &nbsp;
        </p>
    </form>
</body>
</html>
