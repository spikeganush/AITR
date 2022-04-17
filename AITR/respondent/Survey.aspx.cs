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

namespace AITR.respondent
{
    public partial class Survey : System.Web.UI.Page
    {
        private bool openQuestion = false;
        private int questionOptions = 0;
        private int nextQuestion = 0;
        private int previousQuestion = 0;
        private string havePreviousQuestion = String.Empty;
        private string haveNextQuestion = String.Empty;

        private DropDownList dropDownList = new DropDownList();
        private CheckBoxList checkBoxList = new CheckBoxList();
        private RadioButtonList radioBtnList = new RadioButtonList();
        private TextBox textBox = new TextBox();
        protected void Page_Load(object sender, EventArgs e)
        {            
            page_title.Text = "Question number " + Session["question_nb_display"].ToString();
            //We are testing if a survey session is running or not
            if (Session["research_id"] == null)
            {
                TestSessionId();
            }
            

            using (SqlConnection conn = Utils.Utils.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT Question.*, Question_type.* FROM question INNER JOIN Question_type ON Question.question_type_id = Question_type.question_type_id", conn);
                conn.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                // Database table for Question
                DataTable questionDB = new DataTable();
                questionDB.Columns.Add("id", System.Type.GetType("System.String"));
                questionDB.Columns.Add("question_type_id", System.Type.GetType("System.String"));
                questionDB.Columns.Add("title", System.Type.GetType("System.String"));
                questionDB.Columns.Add("description", System.Type.GetType("System.String"));
                questionDB.Columns.Add("previous question", System.Type.GetType("System.Int32"));
                questionDB.Columns.Add("next question", System.Type.GetType("System.Int32"));
                questionDB.Columns.Add("multiple_answer", System.Type.GetType("System.Boolean"));
                questionDB.Columns.Add("max_answer", System.Type.GetType("System.Int32"));
                DataRow question;
                while (rd.Read())
                {

                    question = questionDB.NewRow();
                    question["id"] = rd["question_id"].ToString();
                    question["question_type_id"] = rd["question_type_id"].ToString();
                    question["title"] = rd["title"].ToString();
                    question["description"] = rd["description"].ToString();
                    question["previous question"] =  rd["previous_question"];
                    question["next question"] = rd["next_question"];
                    question["multiple_answer"] = Convert.ToBoolean(rd["multiple_answer"]);
                    question["max_answer"] = Convert.ToInt32(rd["max_answer"]);

                    if (int.Parse((string)question["id"]) == Convert.ToInt32(Session["question_nb"]))
                    {
                        havePreviousQuestion = question["previous question"].ToString();
                        haveNextQuestion = question["next question"].ToString();
                        question_title.Text = question["title"].ToString() + "?";
                        question_description.Text = question["description"].ToString();
                        if (Convert.ToInt32(question["question_type_id"]) > 1)
                        {
                            if (Convert.ToInt32(question["max_answer"]) > 1)
                            {
                                question_option.Text = "Select " + question["max_answer"].ToString() + " option";
                            }
                            else
                            {
                                question_option.Text = "Select " + question["max_answer"].ToString() + " options";
                            }
                        } else
                        {
                            question_option.Text = "Type your answer";
                        }

                        questionOptions = Convert.ToInt32(question["question_type_id"]);
                        
                        questionDB.Rows.Add(question);
                        
                        if (Convert.ToInt32(question["question_type_id"]) != 1)
                        {
                            openQuestion = true;
                        } else
                        {
                            openQuestion = false;
                        }

                        if (havePreviousQuestion.Length == 0)
                        {
                            button_previous.Visible = false;
                        }
                        else
                        {
                            button_previous.Visible = true;
                            previousQuestion = Convert.ToInt32(question["previous question"]);

                        }

                        if (haveNextQuestion.Length == 0)
                        {
                            button_next.Visible = false;
                        }
                        else
                        {
                            button_next.Visible = true;
                            nextQuestion = Convert.ToInt32(question["next question"]);
                        }
                    }

                    

                }
                conn.Close();

                SqlCommand Q_OptionCommand;
                conn.Open();
                Q_OptionCommand = new SqlCommand("SELECT * FROM multiple_answer WHERE question_id = @id", conn);
                Q_OptionCommand.Parameters.AddWithValue("@id", Convert.ToInt32(Session["question_nb"]));

                SqlDataReader Q_OptionReader;
                Q_OptionReader = Q_OptionCommand.ExecuteReader();
                // Database table for Question Option
                DataTable Q_OptionDB = new DataTable();
                Q_OptionDB.Columns.Add("id", System.Type.GetType("System.String"));
                Q_OptionDB.Columns.Add("title", System.Type.GetType("System.String"));
                Q_OptionDB.Columns.Add("sub_question", System.Type.GetType("System.Boolean"));
                Q_OptionDB.Columns.Add("sub_question_id", System.Type.GetType("System.String"));
                DataRow Q_OptionRow;
                while (Q_OptionReader.Read())
                {
                    Q_OptionRow = Q_OptionDB.NewRow();
                    Q_OptionRow["id"] = Q_OptionReader["multiple_answer_id"].ToString();
                    Q_OptionRow["title"] = Q_OptionReader["title"].ToString();
                    Q_OptionRow["sub_question"] = Convert.ToBoolean(Q_OptionReader["sub_question"]);
                    Q_OptionRow["sub_question_id"] = Q_OptionReader["sub_question_id"].ToString();
                    Q_OptionDB.Rows.Add(Q_OptionRow);

                    if (questionOptions == 1) {                        
                        Option.Controls.Add(textBox);
                        textBox.Text = String.Empty;                        
                    }
                    else if (questionOptions == 2)
                    {
                        radioBtnList.Items.Add(new ListItem(Q_OptionRow["title"].ToString()));
                        Option.Controls.Add(radioBtnList);
                    } else if (questionOptions == 3 || questionOptions == 4 || questionOptions == 6 || questionOptions == 7 || questionOptions == 8) {                        
                        checkBoxList.Items.Add(new ListItem(Q_OptionRow["title"].ToString()));
                        Option.Controls.Add(checkBoxList);

                    } else if (questionOptions == 5)
                    {
                        
                        dropDownList.Items.Add(new ListItem(Q_OptionRow["title"].ToString()));
                        Option.Controls.Add(dropDownList);
                    }
                    

                    
                    
                }                
            }

        }

        private void TestSessionId()
        {

            String research_id =  String.Empty;           
                        
                using (SqlConnection conn = Utils.Utils.GetConnection())
                {
                    String query = "INSERT INTO research_session (respondent_id, date, ip) VALUES (@respondent_id, @date, @ip) select SCOPE_IDENTITY()";

                    conn.Open();

                    String dateString = DateTime.Today.ToShortDateString();
                    string[] dateTimeParts = dateString.Split('/');
                    int day = Convert.ToInt32(dateTimeParts[0]);
                    int month = Convert.ToInt32(dateTimeParts[1]);
                    int year = Convert.ToInt32(dateTimeParts[2]);

                    DateTime date = new DateTime(year, month, day);
                    String hostName = Dns.GetHostName();
                    String ip = Dns.GetHostByName(hostName).AddressList[0].ToString();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@respondent_id", Session["respondent_id"]);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@ip", ip);

                        String id = command.ExecuteScalar().ToString();

                        Session["research_id"] = id;                        
                    }
                    conn.Close();
                
            }
        }

        protected void button_next_Click(object sender, EventArgs e)
        {
            Session["question_nb_display"] = Convert.ToInt32(Session["question_nb_display"]) + 1;
            Session["question_nb"] = nextQuestion;
            Response.Redirect("Survey.aspx");
        }

        protected void button_previous_Click(object sender, EventArgs e)
        {
            Session["question_nb_display"] = Convert.ToInt32(Session["question_nb_display"]) - 1;
            Session["question_nb"] = previousQuestion;
            Response.Redirect("Survey.aspx");
        }
    }
}