using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AITR.Utils;
using System.Net;
using AITR.DTO;

namespace AITR.respondent
{
    public partial class EndSurvey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If the respondent arrived to this page, the survey is considered completed
            // So the DB is update to show a difference between the complete survey and the incomplete one
            using (SqlConnection conn = Utils.Utils.GetConnection())
            {
                String query = "UPDATE research_session SET complete = @complete WHERE research_id = @research_id";

                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@research_id", Session["research_id"]);
                    command.Parameters.AddWithValue("@complete", 1);

                    int result = command.ExecuteNonQuery();

                    // Error/Success message
                    if (result < 0)
                    {
                        LabelSuccess.Text = "An error happened during the recording of your information.";
                    }
                    else
                    {
                        LabelSuccess.Text = "Data successfully recorded.";
                    }

                    
                }
                conn.Close();
            }

        }
    }
}