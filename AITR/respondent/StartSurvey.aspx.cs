﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using AITR.Utils;

namespace AITR
{
    public partial class survey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void ButtonNext_Click(object sender, EventArgs e)
        {           

            if (anonymous_yes.Checked)
            {


                Response.Redirect("Survey.aspx");
                

            } else
            {
                Response.Redirect("RespondentRegister.aspx");
            }
        }
    }
}