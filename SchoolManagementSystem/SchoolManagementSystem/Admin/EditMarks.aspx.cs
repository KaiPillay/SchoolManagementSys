using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Admin
{
    public partial class EditMarks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind the students and subjects dropdown lists
                BindStudents();
                BindSubjects();

                // Populate the marks if a record is selected
                if (Request.QueryString["markId"] != null)
                {
                    int markId = int.Parse(Request.QueryString["markId"]);
                    LoadMarks(markId);
                }
            }
        }

        private void BindStudents()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    string query = "SELECT StudentID, Name FROM student";
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    ddlStudents.DataSource = reader;
                    ddlStudents.DataTextField = "Name";
                    ddlStudents.DataValueField = "StudentID";
                    ddlStudents.DataBind();
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
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    string query = "SELECT SubjectID, SubjectName FROM subject";
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    ddlSubject.DataSource = reader;
                    ddlSubject.DataTextField = "SubjectName";
                    ddlSubject.DataValueField = "SubjectID";
                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, new ListItem("Select a subject", ""));
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void LoadMarks(int markId)
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    string query = "SELECT StudentId, SubjectId, TotalMarks, OutOfMarks FROM exam WHERE MarkID = @MarkID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MarkID", markId);
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ddlStudents.SelectedValue = reader["StudentId"].ToString();
                        ddlSubject.SelectedValue = reader["SubjectId"].ToString();
                        txtTotalMarks.Text = reader["TotalMarks"].ToString();
                        txtOutOfMarks.Text = reader["OutOfMarks"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnEditMarks_Click(object sender, EventArgs e)
        {
            if (ddlStudents.SelectedValue == "" || ddlSubject.SelectedValue == "" || string.IsNullOrEmpty(txtTotalMarks.Text) || string.IsNullOrEmpty(txtOutOfMarks.Text))
            {
                lblError.Text = "All fields are required.";
                return;
            }

            try
            {
                int studentId = int.Parse(ddlStudents.SelectedValue);
                int subjectId = int.Parse(ddlSubject.SelectedValue);
                int totalMarks = int.Parse(txtTotalMarks.Text.Trim());
                int outOfMarks = int.Parse(txtOutOfMarks.Text.Trim());

                string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    string query = "UPDATE exam SET TotalMarks = @TotalMarks, OutOfMarks = @OutOfMarks WHERE StudentId = @StudentId AND SubjectId = @SubjectId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@SubjectId", subjectId);
                    cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                    cmd.Parameters.AddWithValue("@OutOfMarks", outOfMarks);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblMessage.Text = "Marks updated successfully.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblError.Text = "Failed to update marks.";
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
