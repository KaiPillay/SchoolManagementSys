using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Data;

namespace SchoolManagementSystem.Student
{
    public partial class StudentGrades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    LoadStudentGrades();
                }
                else
                {
                    lblError.Text = "Error: You are not logged in.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void LoadStudentGrades()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                // Fetch StudentID using UserID
                string studentQuery = "SELECT StudentID, Name FROM student WHERE UserID = @UserID";
                DataTable studentData = Fetch(studentQuery, new MySqlParameter("@UserID", userId));

                if (studentData.Rows.Count > 0)
                {
                    int studentId = Convert.ToInt32(studentData.Rows[0]["StudentID"]);
                    lblStudentName.Text = studentData.Rows[0]["Name"].ToString();

                    // Fetch grades for the logged-in student
                    FetchStudentGrades(studentId);
                }
                else
                {
                    lblError.Text = "Error: Student record not found.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void FetchStudentGrades(int studentId)
        {
            // Set up the connection string
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get grades and subject names for the student
                    string query = @"
                        SELECT e.ExamId, s.SubjectName, e.TotalMarks, e.OutOfMarks 
                        FROM exam e
                        INNER JOIN subject s ON e.SubjectId = s.SubjectId
                        WHERE e.StudentId = @StudentId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Bind the data to the GridView
                    gvExamResults.DataSource = dt;
                    gvExamResults.DataBind();
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private DataTable Fetch(string query, MySqlParameter parameter)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.Add(parameter);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
