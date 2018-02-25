using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DuckTorrentDB;

public partial class Admin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientHandler clientHandler = new ClientHandler();
        var x = clientHandler.FindUser("asaf");
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}