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
    public partial class Survey : System.Web.UI.Page
    {
        private bool openQuestion = false;
        private int questionOptions = 0;
        private int nextQuestion = 0;
        private int previousQuestion = 0;
        private string havePreviousQuestion = String.Empty;
        private string haveNextQuestion = String.Empty;

        private TextBox textBox = new TextBox();
        private DropDownList dropDownList = new DropDownList();
        private CheckBoxList checkBoxList = new CheckBoxList();
        private RadioButtonList radioBtnList = new RadioButtonList();
        
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
                SqlCommand cmd = new SqlCommand("SELECT question.*, question_type.* FROM question INNER JOIN question_type ON question.question_type_id = question_type.question_type_id", conn);
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
                            if (Convert.ToInt32(question["max_answer"]) == 1)
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
                        if (questionOptions == 1)
                        {                            
                            Option.Controls.Add(textBox);
                            textBox.Text = String.Empty;
                        }

                        questionDB.Rows.Add(question);
                        dbTableView.DataSource = questionDB;
                        dbTableView.DataBind();

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
                            button_finish.Visible = true;
                        }
                        else
                        {
                            button_next.Visible = true;
                            button_finish.Visible = false;
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
                    dbSubQuestion.DataSource = Q_OptionDB;
                    dbSubQuestion.DataBind();
                    Console.WriteLine(questionOptions);

                     if (questionOptions == 2)
                    {
                        ListItem item = new ListItem();
                        item.Text = Q_OptionRow["title"].ToString();
                        item.Value = Q_OptionRow["id"].ToString();

                        radioBtnList.Items.Add(item);
                        Option.Controls.Add(radioBtnList);
                    } 
                    else if (questionOptions == 3 || questionOptions == 4 || questionOptions == 6 || questionOptions == 7 || questionOptions == 8) 
                    {
                        ListItem item = new ListItem();
                        item.Text = Q_OptionRow["title"].ToString();
                        item.Value = Q_OptionRow["id"].ToString();

                        checkBoxList.Items.Add(item);
                        Option.Controls.Add(checkBoxList);

                    } 
                    else if (questionOptions == 5)
                    {

                        ListItem item = new ListItem();
                        item.Text = Q_OptionRow["title"].ToString();
                        item.Value = Q_OptionRow["id"].ToString();
                        
                        dropDownList.Items.Add(item);
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
            //textBox
            if(questionOptions == 1)
            {
                SaveAnswerInDb(textBox.Text);
            }
            //radioButton
            else if (questionOptions == 2)
            {
                foreach(ListItem item in radioBtnList.Items)
                {
                    if (item.Selected == true)
                    {
                        SaveAnswerInDb(item.Value);
                    }
                }
                
            }
            //Checkbox
            else if (questionOptions == 3 || questionOptions == 4 || questionOptions == 6 || questionOptions == 7 || questionOptions == 8)
            {
                
                foreach(ListItem item in checkBoxList.Items)
                {
                    if (item.Selected == true)
                    {
                        SaveAnswerInDb(item.Value);
                    }
                }
               

            }
            //dropdownlist
            else if (questionOptions == 5)
            {
                foreach (ListItem item in dropDownList.Items)
                {
                    if (item.Selected == true)
                    {
                        SaveAnswerInDb(item.Value);
                    }
                }

            }
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

        private void SaveAnswerInDb(string answer)
        {
            ResultStatus resultStatus = new ResultStatus();

            using (SqlConnection conn = Utils.Utils.GetConnection())
            {
                String query = "INSERT INTO research_answer (research_id, question_id, answer) VALUES (@research_id, @question_id, @answer)";

                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@research_id", Session["research_id"]);
                    command.Parameters.AddWithValue("@question_id", Session["question_nb"]);
                    command.Parameters.AddWithValue("@answer", answer);

                    int result = command.ExecuteNonQuery();

                    // Error/Success message
                    if (result < 0)
                    {
                        resultStatus.ResultStatusCode = 3;
                        resultStatus.Message = "Error in registration";
                    }
                    else
                    {

                        resultStatus.ResultStatusCode = 1;
                        resultStatus.Message = "Registration succeed";
                    }
                }
            }
        }

        
        

    }
}