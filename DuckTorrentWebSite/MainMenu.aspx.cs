﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MainMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


    }


    protected void Regst_Click(object sender, EventArgs e)
    {
        Response.Redirect("Register.aspx");
    }



    protected void Admin_Click(object sender, EventArgs e)
    {
        //Response.Redirect("LogIn.aspx");
        //Response.Redirect("Admin.aspx");
    }
}