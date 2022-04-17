using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace AITR
{
    public partial class survey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection myConn;

            myConn = new SqlConnection();

            String targetConnection = ConfigurationManager.ConnectionStrings["CurrentConnection"].ConnectionString;

            if (targetConnection.Equals("dev"))
            {
                myConn.ConnectionString = AppConstant.DevConnectionString;
            }
            else if (targetConnection.Equals("test"))
            {
                myConn.ConnectionString = AppConstant.TestConnectionString;
            }
            else if (targetConnection.Equals("prod"))
            {
                myConn.ConnectionString = AppConstant.ProdConnectionString;
            }
            else
            {
                throw new Exception();
            }

            myConn.Open();

            SqlCommand myCommand;
            myCommand = new SqlCommand("SELECT * FROM [question]", myConn);

        }

        protected void ButtonNext_Click(object sender, EventArgs e)
        {
            Session["RespondentEmail"] = TextBoxEmail.Text;

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