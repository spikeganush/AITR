using System;
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
    public partial class Survey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session variables used during the survey
            Session["research_id"] = null; //research_session id
            Session["question_nb"] = 100; //question number - question_id
            Session["question_nb_display"] = 1; // number to display in the title          

        }

        protected void ButtonNext_Click(object sender, EventArgs e)
        {           
            // The respondent want to stay anonymous - Still to create an id and record the ip when the survey start
            if (anonymous_yes.Checked)
            {
                using (SqlConnection conn = Utils.Utils.GetConnection())
                {
                    String query = "INSERT INTO respondent (register) VALUES (@register) select SCOPE_IDENTITY()";

                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@register", 0);                        

                        String id = command.ExecuteScalar().ToString();

                        Session["respondent_id"] = id;
                    }

                    conn.Close();
                }
                // redirect to the beginning of the survey
                    Response.Redirect("Survey.aspx");
                

            } else
            {
                // redirect to the register page
                Response.Redirect("RespondentRegister.aspx");
            }
        }
    }
}