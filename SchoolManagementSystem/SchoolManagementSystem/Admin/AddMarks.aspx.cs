using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;


namespace SchoolManagementSystem.Admin
{
    public partial class AddMarks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind the students and subjects dropdown lists
                BindStudents();
                BindSubjects();
            }
        }

        private void BindStudents()
        {
            // Set up the connection string
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get all students
                    string query = "SELECT StudentID, Name FROM student";

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Bind data to the dropdown list
                    ddlStudents.DataSource = reader;
                    ddlStudents.DataTextField = "Name"; // Display the student's name
                    ddlStudents.DataValueField = "StudentID"; // Use StudentID as the value
                    ddlStudents.DataBind();

                    // Add a default "Select Student" option
                    ddlStudents.Items.Insert(0, new ListItem("Select a student", ""));
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
            // Set up the connection string
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get all subjects
                    string query = "SELECT SubjectID, SubjectName FROM subject"; // Replace with actual table name for subjects

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Bind data to the dropdown list
                    ddlSubject.DataSource = reader;
                    ddlSubject.DataTextField = "SubjectName"; // Display the subject name
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

        protected void btnAddMarks_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (ddlStudents.SelectedValue == "" || ddlSubject.SelectedValue == "" || string.IsNullOrEmpty(txtTotalMarks.Text) || string.IsNullOrEmpty(txtOutOfMarks.Text))
            {
                lblError.Text = "All fields are required.";
                return;
            }

            try
            {
                // Get the data from the form
                int studentId = int.Parse(ddlStudents.SelectedValue);
                int subjectId = int.Parse(ddlSubject.SelectedValue);
                int totalMarks = int.Parse(txtTotalMarks.Text.Trim());
                int outOfMarks = int.Parse(txtOutOfMarks.Text.Trim());

                // Set up the connection string
                string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    // SQL query to insert marks into the exam table
                    string query = "INSERT INTO exam (StudentId, SubjectId, TotalMarks, OutOfMarks) VALUES (@StudentId, @SubjectId, @TotalMarks, @OutOfMarks)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@SubjectId", subjectId);
                    cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                    cmd.Parameters.AddWithValue("@OutOfMarks", outOfMarks);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblMessage.Text = "Marks added successfully.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblError.Text = "Failed to add marks.";
                    }
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
