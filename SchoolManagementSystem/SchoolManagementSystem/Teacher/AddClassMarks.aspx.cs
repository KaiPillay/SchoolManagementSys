using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Teacher
{
    public partial class AddClassMarks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind the classes and subjects dropdown lists
                BindClasses();
                BindSubjects();
            }
        }

        private void BindClasses()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get all classes
                    string query = "SELECT ClassID, ClassName FROM class";

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Bind data to the class dropdown list
                    ddlClass.DataSource = reader;
                    ddlClass.DataTextField = "ClassName"; // Display class name
                    ddlClass.DataValueField = "ClassID"; // Use ClassID as the value
                    ddlClass.DataBind();

                    // Add a default "Select Class" option
                    ddlClass.Items.Insert(0, new ListItem("Select a class", ""));
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void BindSubjects()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get all subjects
                    string query = "SELECT SubjectID, SubjectName FROM subject";

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Bind data to the subject dropdown list
                    ddlSubject.DataSource = reader;
                    ddlSubject.DataTextField = "SubjectName"; // Display subject name
                    ddlSubject.DataValueField = "SubjectID"; // Use SubjectID as the value
                    ddlSubject.DataBind();

                    // Add a default "Select Subject" option
                    ddlSubject.Items.Insert(0, new ListItem("Select a subject", ""));
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure that a class is selected before querying
            if (string.IsNullOrEmpty(ddlClass.SelectedValue))
            {
                return;
            }

            // Bind students based on selected class
            BindStudents();
        }

        private void BindStudents()
        {
            // Ensure a class is selected before querying
            if (string.IsNullOrEmpty(ddlClass.SelectedValue))
            {
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get students based on selected class
                    string query = "SELECT StudentID, Name FROM student WHERE ClassID = @ClassID";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ClassID", ddlClass.SelectedValue);

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Bind data to the GridView
                    if (reader.HasRows)
                    {
                        gvStudents.DataSource = reader;
                        gvStudents.DataBind();
                    }
                    else
                    {
                        lblError.Text = "No students found for this class.";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnAddMarks_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (ddlClass.SelectedValue == "" || ddlSubject.SelectedValue == "" || string.IsNullOrEmpty(txtOutOfMarks.Text))
            {
                lblError.Text = "All fields are required.";
                return;
            }

            try
            {
                // Get the data from the form
                int classId = int.Parse(ddlClass.SelectedValue);
                int subjectId = int.Parse(ddlSubject.SelectedValue);
                int outOfMarks = int.Parse(txtOutOfMarks.Text.Trim());

                // Set up the connection string
                string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();

                    foreach (GridViewRow row in gvStudents.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            int studentId = Convert.ToInt32(gvStudents.DataKeys[row.RowIndex].Value);
                            TextBox txtMarks = (TextBox)row.FindControl("txtMarks");

                            if (int.TryParse(txtMarks.Text, out int obtainedMarks))
                            {
                                string query = "INSERT INTO exam (ClassId, SubjectId, StudentId, TotalMarks, OutOfMarks) " +
                                               "VALUES (@ClassId, @SubjectId, @StudentId, @TotalMarks, @OutOfMarks)";

                                MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
                                cmd.Parameters.AddWithValue("@ClassId", classId);
                                cmd.Parameters.AddWithValue("@SubjectId", subjectId);
                                cmd.Parameters.AddWithValue("@StudentId", studentId);
                                cmd.Parameters.AddWithValue("@TotalMarks", obtainedMarks);
                                cmd.Parameters.AddWithValue("@OutOfMarks", outOfMarks);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    lblMessage.Text = "Marks added successfully.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
