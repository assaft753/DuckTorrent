using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DuckTorrentDB;

public partial class LogIn : System.Web.UI.Page
{
    ClientHandler clientHandler;

    protected void Page_Load(object sender, EventArgs e)
    {
        clientHandler = new ClientHandler();

    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("MainMenu.aspx");
    }



    protected void LoginBtn_Click(object sender, EventArgs e)
    {
        string name = UserNameTextBox.Text, password = PasswordTextBox.Text;
        try
        {
            checkValid(name, password);
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.Message.ToString();
        }



    }
    private void checkValid(String name, String password)
    {
        if (name.Length == 0)
            throw new Exception("Enter User name");
        else if (password.Length == 0)
            throw new Exception("Enter password");
        else if (clientHandler.CheckUser(name, password))
            Response.Redirect("Admin.aspx");
        else
            throw new Exception("Wrong input!");

    }
}