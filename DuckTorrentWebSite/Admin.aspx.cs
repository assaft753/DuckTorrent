using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DuckTorrentDB;

public partial class Admin : System.Web.UI.Page
{
    ClientHandler clientHandler = new ClientHandler();
    FileHandler fileHandler = new FileHandler();


    protected void Page_Load(object sender, EventArgs e)
    {
        setData();
        GridViewSearch.Visible = false;


    }

    private void setData()
    {

        OnlineUsers.Text = clientHandler.getNumberOfOnlineUsers().ToString();
        TotalUsers.Text = clientHandler.GetTotalNumberOfUsers().ToString();
        TotalFiles.Text = fileHandler.GetNumberOfFiles().ToString();


    }


    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        setData();
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        if (SearchText.Text.Length > 0)
        {
            GridViewSearch.Visible = true;
            GridView2.Visible = false;
        }
        else
        {
            GridViewSearch.Visible = false;
            GridView2.Visible = true;
        }

    }

    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
}