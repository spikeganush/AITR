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

namespace AITR.staff
{
    public partial class Login : System.Web.UI.Page
    {
        List<String> staffMembersUsername = new List<string>();
        List<String> staffMembersPassword = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {

            //Question
            //Db connection + store the result in a Datatable
            using (SqlConnection conn = Utils.Utils.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM staff", conn);
                conn.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                // Database table for Question
                DataTable staffMemberDB = new DataTable();
                staffMemberDB.Columns.Add("id", System.Type.GetType("System.String"));
                staffMemberDB.Columns.Add("username", System.Type.GetType("System.String"));
                staffMemberDB.Columns.Add("password", System.Type.GetType("System.String"));
                DataRow staffMember;


                while (rd.Read())
                {
                    staffMember = staffMemberDB.NewRow();
                    //Different columns with their name and type
                    staffMember["id"] = rd["staff_id"].ToString();
                    staffMember["username"] = rd["username"].ToString();
                    staffMember["password"] = rd["password"].ToString();
                    staffMembersUsername.Add(staffMember["username"].ToString());
                    staffMembersPassword.Add(staffMember["password"].ToString());
                }
                conn.Close();                
            }
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach(String username in staffMembersUsername)
            {
                if(username == TextBoxLoginUsername.Text)
                {
                    labelErrorMessage.Text = "";
                    if (staffMembersPassword[count] == TextBoxLoginPassword.Text)
                    {
                        labelErrorMessage.Text = "";
                        Response.Redirect("Research.aspx");
                    } else
                    {
                        labelErrorMessage.Text = "Wrong password";
                    }
                } else
                {
                    labelErrorMessage.Text = "User not found";
                }
                count++;

            }
            
        }
    }
}