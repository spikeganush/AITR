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
        //Declaration of usefull variable
        private int questionOptions = 0; //Store the type of quesion [1: textBox| 2: radioButton| 3,4,6,7,8: checkBoxe| 5: dropdownList]  
        private int nextQuestion = 0; //Store the next question id
        private int maxAnswer = 0; //Store the max answer possible for a question
        private int success = 0; // return variable -> 0: failed | 1: success to register the answer in the db
        private int firstPageId = 0; //here to test if 2 question_nb are the same if not ↓
        private int secondPageId = 0; //store the question_nb if are different
        private string havePreviousQuestion = String.Empty; // if null no previous question, so no previous button
        private string haveNextQuestion = String.Empty; // if null no next question, show the finish button        
        string[] values; // Store if an answer have the answer id and the next question id inside his Value
        //Declaration for the different "Objects" use to answer the questions

        private TextBox textBox = new TextBox();
        private DropDownList dropDownList = new DropDownList();
        private CheckBoxList checkBoxList = new CheckBoxList();
        private RadioButtonList radioBtnList = new RadioButtonList();
        
        protected void Page_Load(object sender, EventArgs e)
        {   
            //Title with the question number
            page_title.Text = "Question number " + Session["question_nb_display"].ToString();
            //error message
            error_message.Text = Session["error_message"].ToString();
            //We are testing if a survey session is running or not
            if (Session["research_id"] == null)
            {
                TestSessionId();
            }
            
            //Question
            //Db connection + store the result in a Datatable
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
                questionDB.Columns.Add("previous_question", System.Type.GetType("System.Int32"));
                questionDB.Columns.Add("next_question", System.Type.GetType("System.Int32"));
                questionDB.Columns.Add("multiple_answer", System.Type.GetType("System.Boolean"));
                questionDB.Columns.Add("max_answer", System.Type.GetType("System.Int32"));
                DataRow question;


                while (rd.Read())
                {
                    question = questionDB.NewRow();
                    //Different columns with their name and type
                    question["id"] = rd["question_id"].ToString();
                    question["question_type_id"] = rd["question_type_id"].ToString();
                    question["title"] = rd["title"].ToString();
                    question["description"] = rd["description"].ToString();
                    question["previous_question"] =  rd["previous_question"];
                    question["next_question"] = rd["next_question"];
                    question["multiple_answer"] = Convert.ToBoolean(rd["multiple_answer"]);
                    question["max_answer"] = Convert.ToInt32(rd["max_answer"]);
                    //Store only the question info when the question_id correspondent to the question number
                    if (int.Parse((string)question["id"]) == Convert.ToInt32(Session["question_nb"]))
                    {
                        havePreviousQuestion = question["previous_question"].ToString();
                        haveNextQuestion = question["next_question"].ToString();
                        question_title.Text = question["title"].ToString() + "?";
                        question_description.Text = question["description"].ToString();
                        maxAnswer = Convert.ToInt32(question["max_answer"]);
                        // Show a different text depends of the max answer
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

                        // if question type answer is a textbox, create the textbox
                        if (questionOptions == 1)
                        {                            
                            Option.Controls.Add(textBox);
                            textBox.Text = String.Empty;
                        }

                        questionDB.Rows.Add(question);                        

                        // Next, Finish button visible or not
                        if (haveNextQuestion.Length == 0)
                        {
                            Button_next.Visible = false;
                            Button_finish.Visible = true;
                        }
                        else
                        {
                            Button_next.Visible = true;
                            Button_finish.Visible = false;
                            nextQuestion = Convert.ToInt32(haveNextQuestion);
                        }
                    }
                }
                conn.Close();
                
                //Answer
                //DB connection to find the question type, possible answers and show the correspondent html element

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
                int countLoop = 0;
                while (Q_OptionReader.Read())
                {
                    countLoop++;
                    Q_OptionRow = Q_OptionDB.NewRow();
                    //Database columns with their types
                    Q_OptionRow["id"] = Q_OptionReader["multiple_answer_id"].ToString();
                    Q_OptionRow["title"] = Q_OptionReader["title"].ToString();
                    Q_OptionRow["sub_question"] = Convert.ToBoolean(Q_OptionReader["sub_question"]);
                    Q_OptionRow["sub_question_id"] = Q_OptionReader["sub_question_id"].ToString(); 
                    Q_OptionDB.Rows.Add(Q_OptionRow);
                    //Create the html element
                    //radioButton
                     if (questionOptions == 2)
                    {
                        ListItem item = new ListItem();
                        if (countLoop == 1)
                        {
                            item.Selected = true;
                        }
                        
                        item.Text = Q_OptionRow["title"].ToString();
                        item.Value = Q_OptionRow["id"].ToString();

                        radioBtnList.Items.Add(item);
                        Option.Controls.Add(radioBtnList);
                    } 
                     //checkBox
                    else if (questionOptions == 3 || questionOptions == 4 || questionOptions == 6 || questionOptions == 7 || questionOptions == 8 || questionOptions == 9) 
                    {
                        ListItem item = new ListItem();
                        item.Text = Q_OptionRow["title"].ToString();
                            if (Q_OptionRow["sub_question_id"].ToString().Length == 0)
                        {
                            item.Value = Q_OptionRow["id"].ToString();
                        } else
                        {

                        item.Value = Q_OptionRow["id"].ToString() + " " + Q_OptionRow["sub_question_id"].ToString();
                        }
                        

                        checkBoxList.Items.Add(item);
                        Option.Controls.Add(checkBoxList);

                    } 
                     //dropDownList
                    else if (questionOptions == 5)
                    {
                        ListItem item = new ListItem
                        {
                            Text = Q_OptionRow["title"].ToString(),
                            Value = Q_OptionRow["id"].ToString()
                    };
                        
                        
                        dropDownList.Items.Add(item);
                        Option.Controls.Add(dropDownList);
                    } 
                }                
            }
        }

        //If no session id exist, create one and store the id in a Session variable
        private void TestSessionId()
        {         
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

        //                  //
        //                  //
        // BUTTONS ACTIONS  //
        //                  //
        //                  //

        protected void Button_next_Click(object sender, EventArgs e)
        {

            FirstStepToRegisterDB();            

           if (success == 1)
           {
                Session["error_message"] = "";
                //Increment the value for question number display in the page title
                Session["question_nb_display"] = Convert.ToInt32(Session["question_nb_display"]) + 1;
                //Test to know if an answer with a subquestion has been selected and been redirected to the correct next question
                if(questionOptions != 1)
                {
                    if (values.Length > 1)
                    {
                        if (secondPageId != 0)
                        {
                            Session["question_nb2"] = secondPageId;
                        }
                        else
                        {
                            Session["question_nb"] = values[1];
                        }

                        Response.Redirect("Survey.aspx");
                    }
                    else
                    {
                        if (Convert.ToInt32(Session["question_nb2"]) != 0)
                        {
                            Session["question_nb"] = Session["question_nb2"];
                            Session["question_nb2"] = null;
                            Response.Redirect("Survey.aspx");

                        }
                        Session["question_nb"] = nextQuestion;
                        Response.Redirect("Survey.aspx");
                    } 
                }
                else
                {
                    Session["question_nb"] = nextQuestion;
                    Response.Redirect("Survey.aspx");
                }

            } 
           else
           {
               Response.Redirect("Survey.aspx");
           }
           
        }

        protected void Button_finish_Click(object sender, EventArgs e)
        {
            FirstStepToRegisterDB();
            if (success == 1)
            {
                Session["error_message"] = "";
                //Increment the value for question number display in the page title
                Session["question_nb_display"] = Convert.ToInt32(Session["question_nb_display"]) + 1;
                if (values.Length > 1)
                {
                    if (secondPageId != 0)
                    {
                        Session["question_nb2"] = secondPageId;
                    }
                    else
                    {
                        Session["question_nb"] = values[1];
                    }

                    Response.Redirect("Survey.aspx");
                }
                else
                {
                    if (Convert.ToInt32(Session["question_nb2"]) != 0)
                    {
                        Session["question_nb"] = Session["question_nb2"];
                        Session["question_nb2"] = null;
                        Response.Redirect("Survey.aspx");

                    }
                    Session["question_nb"] = nextQuestion;
                    Response.Redirect("EndSurvey.aspx");
                }
            } else
            {
                Response.Redirect("Survey.aspx");
            }
        }

        //                        //
        //                        //
        //   END BUTTONS ACTIONS  //
        //                        //
        //                        //

        protected void FirstStepToRegisterDB()
        {
            //textBox
            if (questionOptions == 1)
            {
                if (textBox.Text.Length < 1)
                {
                    Session["error_message"] = "Please fill the field";
                    success = 0;
                }
                else
                {
                    SaveAnswerInDb(textBox.Text);
                    Session["error_message"] = "";
                    success = 1;
                }
            }
            //radioButton
            else if (questionOptions == 2)
            {

                success = PassingInfoForDb(radioBtnList.Items);

            }
            //Checkbox
            else if (questionOptions == 3 || questionOptions == 4 || questionOptions == 6 || questionOptions == 7 || questionOptions == 8 || questionOptions == 9)
            {
                success = PassingInfoForDb(checkBoxList.Items);
            }
            //dropdownlist
            else if (questionOptions == 5)
            {

                success = PassingInfoForDb(dropDownList.Items);
            }
        }        

        private int PassingInfoForDb(ListItemCollection info)
        {

            int count = 0;
            int result;
            // Count the number of items selected
            foreach (ListItem item in info)
            {
                if (item.Selected == true)
                {
                    count++;
                }
            }

            if (count > maxAnswer)
            {
                Session["error_message"] = "You can select only " + maxAnswer.ToString() + " option(s)";
                result = 0;
            }
            else if (count == 0) {
                Session["error_message"] = "You must select 1 option";
                result = 0;
            }
            else
            {
                error_message.Text = "";
                foreach (ListItem item in info)
                {
                    if (item.Selected == true)
                    {
                        SaveAnswerInDb(item.Value);
                    }
                }
                result = 1;
            }

            return result;
        }

        private void SaveAnswerInDb(string answer)
        {
            ResultStatus resultStatus = new ResultStatus();

            if(questionOptions != 1)
            {
                values = answer.Split(' ');
                if (values.Length > 1)
                {
                    answer = values[0];
                    // if the answers give the possibily to select 2 subquestions
                    // the question_nb for the second subquestion need to be store
                    if (firstPageId == 0)
                    {
                        firstPageId = Convert.ToInt32(values[1]);
                        Session["question_nb"] = Convert.ToInt32(values[1]);
                    }
                    else if (firstPageId != Convert.ToInt32(values[1]))
                    {
                        secondPageId = Convert.ToInt32(values[1]);
                    }
                }
            }
             

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