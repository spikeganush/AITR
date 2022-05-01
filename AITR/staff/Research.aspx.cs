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
using System.Text;
using System.Diagnostics;

namespace AITR.staff
{
    public partial class Research : System.Web.UI.Page
    {
        private int answerID;
        
        private string answerTitle;

        private TextBox textBox = new TextBox();
        private DropDownList dropDownListGender = new DropDownList();
        private DropDownList dropDownListState = new DropDownList();
        private DropDownList dropDownListAge = new DropDownList();
        private DropDownList dropDownListBank = new DropDownList();
        private DropDownList dropDownListBankService = new DropDownList();
        private DropDownList dropDownListNewspaperRead = new DropDownList();
        private DropDownList dropDownListNewspaperSections = new DropDownList();
        private DropDownList dropDownListNewspaperSports = new DropDownList();
        private DropDownList dropDownListNewspaperTravels = new DropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["refresh_search_page"] = Convert.ToInt32(Session["refresh_search_page"]) + 1;
            if (Convert.ToBoolean(Session["login"]))
            {
                resultResearch.DataSource = null;
                resultResearch.DataBind();
                DataGridResult.DataSource = null;
                DataGridResult.DataBind();
                ShowPossibleAnswers();
                
            } else
            {
                Response.Redirect("Login.aspx");
            }
            
        }

        protected void ShowPossibleAnswers()
        {
           
                using (SqlConnection conn = Utils.Utils.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM multiple_answer", conn);
                    conn.Open();

                    SqlDataReader rd = cmd.ExecuteReader();

                    // Database table for Question
                    DataTable answerDB = new DataTable();
                    answerDB.Columns.Add("multiple_answer_id", System.Type.GetType("System.String"));
                    answerDB.Columns.Add("title", System.Type.GetType("System.String"));
                    DataRow answer;
                    int countGender = 0;
                    int countState = 0;
                    int countAge = 0;
                    int countBanks = 0;
                    int countBankServices = 0;
                    int countNewspaper = 0;
                    int countNewspaperSections = 0;
                    int countSports = 0;
                    int countTravels = 0;



                    while (rd.Read())
                    {
                        answer = answerDB.NewRow();
                        //Different columns with their name OR type
                        answer["multiple_answer_id"] = rd["multiple_answer_id"].ToString();
                        answer["title"] = rd["title"].ToString();
                        answerID = Convert.ToInt32(answer["multiple_answer_id"]);
                        answerTitle = answer["title"].ToString();
                        ListItem item = new ListItem();
                        item.Text = answerTitle;
                        item.Value = answerID.ToString();
                        // if answer are for gender
                        if (answerID > 100 && answerID < 200)
                        {
                            if (countGender == 0)
                            {
                                ListItem firstItem = new ListItem();
                                firstItem.Text = "Select";
                                firstItem.Value = "null";
                                firstItem.Selected = false;
                                dropDownListGender.Items.Add(firstItem);
                                PlaceHolderGender.Controls.Add(dropDownListGender);
                            }
                            if(dropDownListGender.Items.Count <= 4)
                            {
                                dropDownListGender.Items.Add(item);
                                PlaceHolderGender.Controls.Add(dropDownListGender);
                            }
                            
                            countGender++;
                        }
                        // if answer are for age
                        else if (answerID > 200 && answerID < 300)
                        {
                        if (countAge == 0)
                        {
                            ListItem firstItem = new ListItem();
                            firstItem.Text = "Select";
                            firstItem.Value = "null";
                            firstItem.Selected = false;
                            dropDownListAge.Items.Add(firstItem);
                            PlaceHolderGender.Controls.Add(dropDownListAge);
                        }
                        if (dropDownListAge.Items.Count <= 7 )
                            {
                            dropDownListAge.Items.Add(item);
                                PlaceHolderAgeRange.Controls.Add(dropDownListAge);
                            }
                        countAge++;
                        }
                        // if answer are for state
                        else if (answerID > 300 && answerID < 400)
                        {
                            if (countState == 0)
                            {
                                ListItem firstItem = new ListItem();
                                firstItem.Text = "Select";
                                firstItem.Value = "null";
                                firstItem.Selected = true;
                                dropDownListState.Items.Add(firstItem);
                                PlaceHolderGender.Controls.Add(dropDownListState);
                            }
                            if(dropDownListState.Items.Count <= 9)
                            {
                                dropDownListState.Items.Add(item);
                                PlaceHolderState.Controls.Add(dropDownListState);
                            }
                            countState++;
                        }
                        // if answer are for banks
                        else if (answerID > 500 && answerID < 550)
                        {
                        if (countBanks == 0)
                        {
                            ListItem firstItem = new ListItem();
                            firstItem.Text = "Select";
                            firstItem.Value = "null";
                            firstItem.Selected = false;
                            dropDownListBank.Items.Add(firstItem);
                            PlaceHolderGender.Controls.Add(dropDownListBank);
                        }
                        if (dropDownListBank.Items.Count <= 6)
                            {
                                dropDownListBank.Items.Add(item);
                                PlaceHolderBank.Controls.Add(dropDownListBank);
                            }
                        countBanks++;
                        }
                        // if answer are for bank services
                        else if (answerID > 550 && answerID < 590)
                        {
                        if (countBankServices == 0)
                        {
                            ListItem firstItem = new ListItem();
                            firstItem.Text = "Select";
                            firstItem.Value = "null";
                            firstItem.Selected = false;
                            dropDownListBankService.Items.Add(firstItem);
                            PlaceHolderGender.Controls.Add(dropDownListBankService);
                        }
                        if (dropDownListBankService.Items.Count <= 6)
                            {
                                dropDownListBankService.Items.Add(item);
                                PlaceHolderBankService.Controls.Add(dropDownListBankService);
                            }
                        countBankServices++;
                        }
                        // if answer are for newspaper read
                        else if (answerID > 590 && answerID < 600)
                        {
                        if (countNewspaper == 0)
                        {
                            ListItem firstItem = new ListItem();
                            firstItem.Text = "Select";
                            firstItem.Value = "null";
                            firstItem.Selected = false;
                            dropDownListNewspaperRead.Items.Add(firstItem);
                            PlaceHolderGender.Controls.Add(dropDownListNewspaperRead);
                        }
                        if (dropDownListNewspaperRead.Items.Count <= 8)
                            {
                                dropDownListNewspaperRead.Items.Add(item);
                                PlaceHolderNewspaper.Controls.Add(dropDownListNewspaperRead);
                            }
                        countNewspaper++;
                        }
                        // if answer are for newspaper section
                        else if (answerID > 600 && answerID < 650)
                        {
                        if (countNewspaperSections == 0)
                        {
                            ListItem firstItem = new ListItem();
                            firstItem.Text = "Select";
                            firstItem.Value = "null";
                            firstItem.Selected = false;
                            dropDownListNewspaperSections.Items.Add(firstItem);
                            PlaceHolderGender.Controls.Add(dropDownListNewspaperSections);
                        }
                        if (dropDownListNewspaperSections.Items.Count <= 7)
                            {
                                dropDownListNewspaperSections.Items.Add(item);
                                PlaceHolderNewspaperSections.Controls.Add(dropDownListNewspaperSections);
                            }
                        countNewspaperSections++;
                        }
                        // if answer are for sports
                        else if (answerID > 650 && answerID < 660)
                        {
                        if (countSports == 0)
                        {
                            ListItem firstItem = new ListItem();
                            firstItem.Text = "Select";
                            firstItem.Value = "null";
                            firstItem.Selected = false;
                            dropDownListNewspaperSports.Items.Add(firstItem);
                            PlaceHolderGender.Controls.Add(dropDownListNewspaperSports);
                        }
                        if (dropDownListNewspaperSports.Items.Count <= 7)
                            {
                                dropDownListNewspaperSports.Items.Add(item);
                                PlaceHolderNewspaperSports.Controls.Add(dropDownListNewspaperSports);
                            }
                        countSports++;
                        }
                        // if answer are for travels
                        else if (answerID > 660 && answerID < 670)
                        {
                        if (countTravels == 0)
                        {
                            ListItem firstItem = new ListItem();
                            firstItem.Text = "Select";
                            firstItem.Value = "null";
                            firstItem.Selected = false;
                            dropDownListNewspaperTravels.Items.Add(firstItem);
                            PlaceHolderGender.Controls.Add(dropDownListNewspaperTravels);
                        }
                        if (dropDownListNewspaperTravels.Items.Count <= 8)
                            {
                                dropDownListNewspaperTravels.Items.Add(item);
                                PlaceHolderNewspaperTravels.Controls.Add(dropDownListNewspaperTravels);
                            }
                        countTravels++;
                        }

                        answerDB.Rows.Add(answer);
                    }
                    conn.Close();
                }
        }

        protected void ShowAllRespondent()
        {



            StringBuilder command = new StringBuilder("SELECT research_answer.*, research_session.*, respondent.*, multiple_answer.* FROM(((research_session INNER JOIN research_answer ON research_session.research_id = research_answer.research_id) INNER JOIN respondent ON research_session.respondent_id = respondent.respondent_id) INNER JOIN  multiple_answer on multiple_answer.multiple_answer_id = TRY_CAST(research_answer.answer AS int))");
            using (SqlConnection conn = Utils.Utils.GetConnection())
            {                
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if (TextBoxFirstName.Text.Trim() != "")
                { 
                    command.Append(" WHERE respondent.first_name LIKE '%'+@FirstName+'%'");                   
                    SqlParameter param = new SqlParameter("@FirstName", TextBoxFirstName.Text.Trim());
                    cmd.Parameters.Add(param);
                }

                if (TextBoxLastName.Text.Trim() != "")
                {
                    if (cmd.Parameters.Count == 0)
                    {
                        command.Append(" WHERE respondent.last_name LIKE '%' +@LastName+ '%'");
                    }
                    else
                    {
                        command.Append(" AND respondent.last_name LIKE '%' +@LastName+ '%'");
                    }
                    SqlParameter param = new SqlParameter("@LastName", TextBoxLastName.Text.Trim());
                    cmd.Parameters.Add(param);
                }

                if (TextBoxSuburb.Text.Trim() != "")
                {
                    if (cmd.Parameters.Count == 0)
                    {
                        command.Append(" WHERE research_answer.answer LIKE '%' +@Suburb+ '%'");
                    }
                    else
                    {
                        command.Append(" OR research_answer.answer LIKE '%' +@Suburb+ '%'");
                    }
                    SqlParameter param = new SqlParameter("@Suburb", TextBoxSuburb.Text.Trim());
                    cmd.Parameters.Add(param);
                }

                if (TextBoxPostcode.Text.Trim() != "")
                {
                    if (cmd.Parameters.Count == 0)
                    {
                        command.Append(" WHERE research_answer.answer=@Postcode");
                    }
                    else
                    {
                        command.Append(" OR research_answer.answer=@Postcode");
                    }
                    SqlParameter param = new SqlParameter("@Postcode", TextBoxPostcode.Text.Trim());
                    cmd.Parameters.Add(param);
                }

                foreach (ListItem item in dropDownListAge.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@Age");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@Age");
                            }
                            SqlParameter param = new SqlParameter("@Age", item.Value);
                            cmd.Parameters.Add(param);
                        }

                    }
                }

                foreach (ListItem item in dropDownListGender.Items)
                {
                    if (item.Selected)
                    {
                        if(item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@Gender");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@Gender");
                            }
                            SqlParameter param = new SqlParameter("@Gender", item.Value);
                            cmd.Parameters.Add(param);
                        }
                        
                    }
                }

                foreach (ListItem item in dropDownListState.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@State");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@State");
                            }
                            SqlParameter param = new SqlParameter("@State", item.Value);
                            cmd.Parameters.Add(param);
                        }
                    }
                }
                
                foreach (ListItem item in dropDownListBank.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@Bank");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@Bank");
                            }
                            SqlParameter param = new SqlParameter("@Bank", item.Value);
                            cmd.Parameters.Add(param);
                        }
                    }
                }

                foreach (ListItem item in dropDownListBankService.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@BankService");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@BankService");
                            }
                            SqlParameter param = new SqlParameter("@BankService", item.Value);
                            cmd.Parameters.Add(param);
                        }
                    }
                }

                foreach (ListItem item in dropDownListNewspaperRead.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@NewspaperRead");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@NewspaperRead");
                            }
                            SqlParameter param = new SqlParameter("@NewspaperRead", item.Value);
                            cmd.Parameters.Add(param);
                        }
                    }
                }

                foreach (ListItem item in dropDownListNewspaperSections.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@NewspaperSections");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@NewspaperSections");
                            }
                            SqlParameter param = new SqlParameter("@NewspaperSections", item.Value);
                            cmd.Parameters.Add(param);
                        }
                    }
                }

                foreach (ListItem item in dropDownListNewspaperSports.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@NewspaperSports");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@NewspaperSports");
                            }
                            SqlParameter param = new SqlParameter("@NewspaperSports", item.Value);
                            cmd.Parameters.Add(param);
                        }
                    }
                }

                foreach (ListItem item in dropDownListNewspaperTravels.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value != "null")
                        {
                            if (cmd.Parameters.Count == 0)
                            {
                                command.Append(" WHERE research_answer.answer=@NewspaperTravels");
                            }
                            else
                            {
                                command.Append(" OR research_answer.answer=@NewspaperTravels");
                            }
                            SqlParameter param = new SqlParameter("@NewspaperTravels", item.Value);
                            cmd.Parameters.Add(param);
                        }
                    }
                }

                command.Append(" ORDER BY respondent.last_name");

                cmd.CommandText = command.ToString();
                cmd.CommandType = CommandType.Text;
                

                conn.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                // Database table for Every Result
                DataTable respondentDB = new DataTable();
                respondentDB.Columns.Add("ID", System.Type.GetType("System.String"));
                respondentDB.Columns.Add("First name", System.Type.GetType("System.String"));
                respondentDB.Columns.Add("Last name", System.Type.GetType("System.String"));
                respondentDB.Columns.Add("Answer", System.Type.GetType("System.String"));
                DataRow respondent;
                //Datable to clean the number of rows | The final query will use the id in this table
                DataTable resultDB = new DataTable();
                resultDB.Columns.Add("ID", System.Type.GetType("System.String"));
                DataRow result;    

                while (rd.Read())
                {
                    respondent = respondentDB.NewRow();
                    //Different columns with their name and type
                    respondent["ID"] = rd["respondent_id"].ToString();
                    if(!Convert.ToBoolean(rd["register"]))
                    {
                        respondent["First name"] = "Anonymous";
                    } else
                    {
                        respondent["First name"] = rd["first_name"].ToString();
                    }                    
                    respondent["Last name"] = rd["last_name"].ToString();
                    respondent["Answer"] = rd["title"].ToString();
                     
                    respondentDB.Rows.Add(respondent);
                    int count = respondentDB.Select().Where(s => s["id"].ToString() == rd["respondent_id"].ToString()).Count();

                    if (cmd.Parameters.Count > 1)
                    {
                        if (count == cmd.Parameters.Count)
                        {
                            result = resultDB.NewRow();  
                            result["ID"] = rd["respondent_id"].ToString();
                            resultDB.Rows.Add(result);                           

                            //resultResearch.DataSource = resultDB;
                            //resultResearch.DataBind();
                        }                        
                    }
                    else
                    {
                        result = resultDB.NewRow();
                        result["ID"] = rd["respondent_id"].ToString();
                        resultDB.Rows.Add(result);

                        //resultResearch.DataSource = RemoveDuplicatesRecords(resultDB);
                        //resultResearch.DataBind();
                    }
                }
                    everyEntries.DataSource = respondentDB;
                    everyEntries.DataBind();
                
                // Database table for Final result
                DataTable finalResultDB = new DataTable();
                finalResultDB.Columns.Add("First Name", System.Type.GetType("System.String"));
                finalResultDB.Columns.Add("Last Name", System.Type.GetType("System.String"));
                finalResultDB.Columns.Add("DOB", System.Type.GetType("System.String"));
                finalResultDB.Columns.Add("Phone", System.Type.GetType("System.String"));
                finalResultDB.Columns.Add("Email", System.Type.GetType("System.String"));
                finalResultDB.Columns.Add("Survey finished", System.Type.GetType("System.String"));
                DataRow finalResult;

                foreach (DataRow row in RemoveDuplicatesRecords(resultDB).Rows)
                {
                    StringBuilder command2 = new StringBuilder("SELECT research_answer.*, research_session.*, respondent.* FROM ((research_session INNER JOIN research_answer ON research_session.research_id = research_answer.research_id) INNER JOIN respondent ON research_session.respondent_id = respondent.respondent_id) WHERE respondent.respondent_id = @RespondentID AND research_answer.question_id = @Email");
                    
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = conn;
                    SqlParameter param2 = new SqlParameter("@RespondentID", row["ID"].ToString());
                    SqlParameter param3 = new SqlParameter("@Email", "450");
                    cmd2.Parameters.Add(param2);
                    cmd2.Parameters.Add(param3);
                    cmd2.CommandText = command2.ToString();
                    cmd2.CommandType = CommandType.Text;


                    SqlDataReader rd3 = cmd2.ExecuteReader();



                    while (rd3.Read())
                    {
                        finalResult = finalResultDB.NewRow();
                        //Different columns with their name and type
                        if(Convert.ToBoolean(rd3["register"]))
                        {
                            finalResult["First name"] = rd3["first_name"].ToString();
                        }
                        else
                        {
                            finalResult["First name"] = "Anonymous";
                        }
                        
                        finalResult["Last name"] = rd3["last_name"].ToString();
                        finalResult["DOB"] = rd3["dob"].ToString();
                        finalResult["Phone"] = rd3["phone_number"].ToString();
                        finalResult["Email"] = rd3["answer"].ToString();
                        finalResult["Survey finished"] = rd3["complete"].ToString();

                        finalResultDB.Rows.Add(finalResult);
                    }

                    DataGridResult.DataSource = finalResultDB;
                    DataGridResult.DataBind();


                    
                }
                conn.Close();
            }            
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {            
            ShowAllRespondent();
        }

        private DataTable RemoveDuplicatesRecords(DataTable dt)
        {
            if(dt.Rows.Count > 0)
            {
                var UniqueRows = dt.AsEnumerable().Distinct(DataRowComparer.Default);
                DataTable dt2 = UniqueRows.CopyToDataTable();
                return dt2;
            } else
            {
                return dt;
            }
            
        }
    }
}