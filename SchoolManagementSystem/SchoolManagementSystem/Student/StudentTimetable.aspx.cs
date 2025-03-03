using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Data;

namespace SchoolManagementSystem.Student
{
    public partial class StudentTimetable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    LoadStudentTimetable();
                }
                else
                {
                    lblError.Text = "Error: You are not logged in.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                }
            }
        }

        private void LoadStudentTimetable()
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
                    string studentName = studentData.Rows[0]["Name"].ToString();

                    // Fetch timetable for the logged-in student
                    FetchStudentTimetable(studentId);
                }
                else
                {
                    lblError.Text = "Error: Student record not found.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Visible = true;
            }
        }

        private void FetchStudentTimetable(int studentId)
        {
            // Set up the connection string
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get timetable entries for the student
                    string query = @"
                        SELECT t.Day, t.Session, c.ClassName AS ClassName, s.SubjectName, te.Name AS TeacherName, t.Room
                        FROM timetable t
                        INNER JOIN class c ON t.ClassID = c.ClassID
                        INNER JOIN subject s ON t.SubjectID = s.SubjectID
                        INNER JOIN teacher te ON t.TeacherID = te.TeacherID
                        WHERE t.StudentID = @StudentID
                        ORDER BY t.Day, t.Session";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Bind the data to the GridView
                    gvTimetable.DataSource = dt;
                    gvTimetable.DataBind();
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
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

        // Back Button Click Event
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentHome.aspx"); // Redirect to the student dashboard or home page
        }
    }
}