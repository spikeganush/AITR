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
            Session["research_id"] = null;
            Session["question_nb"] = 100;
            Session["question_nb_display"] = 1;

        }

        protected void ButtonNext_Click(object sender, EventArgs e)
        {           

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

                    Response.Redirect("Survey.aspx");
                

            } else
            {
                Response.Redirect("RespondentRegister.aspx");
            }
        }
    }
}