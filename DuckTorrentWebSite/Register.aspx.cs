using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DuckTorrentDB;
public partial class Register : System.Web.UI.Page
{
    ClientHandler clientHandler;
    protected void Page_Load(object sender, EventArgs e)
    {
        clientHandler = new ClientHandler();
    }




    protected void MainBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("MainMenu.aspx");
    }



    protected void RegtBtn_Click(object sender, EventArgs e)
    {
        String name = UserNameTextBox.Text;
        String password = PasswordTextBox.Text;

        try
        {
            checkValid(name, password);
            clientHandler.AddUser(name, password);
            Response.Redirect("MainMenu.aspx");

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
        else if (password.Length < 4)
            throw new Exception("Password must contains at least 4 characters");
        else if (password.Length > 10)
            throw new Exception("Password must conatains at most 10 characters");
        else if (password.Contains(" "))
            throw new Exception("Password can't contains spaces");


    }
}