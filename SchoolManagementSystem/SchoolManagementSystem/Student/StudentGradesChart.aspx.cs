using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem.Student
{
    public partial class StudentGradesChart : Page
    {
        public string ChartData { get; set; } = "[]";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    LoadSubjects();
                    LoadStudentGrades(null);
                }
                else
                {
                    lblError.Text = "Error: You are not logged in.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void LoadSubjects()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    string query = "SELECT DISTINCT s.SubjectName FROM exam e INNER JOIN subject s ON e.SubjectId = s.SubjectId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlSubjects.Items.Clear();
                    ddlSubjects.Items.Add(new ListItem("All Subjects", ""));
                    foreach (DataRow row in dt.Rows)
                    {
                        ddlSubjects.Items.Add(new ListItem(row["SubjectName"].ToString(), row["SubjectName"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentGrades(ddlSubjects.SelectedValue);
        }

        private void LoadStudentGrades(string subjectFilter)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                string studentQuery = "SELECT StudentID FROM student WHERE UserID = @UserID";
                DataTable studentData = Fetch(studentQuery, new MySqlParameter("@UserID", userId));

                if (studentData.Rows.Count > 0)
                {
                    int studentId = Convert.ToInt32(studentData.Rows[0]["StudentID"]);
                    FetchStudentGrades(studentId, subjectFilter);
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

        private void FetchStudentGrades(int studentId, string subjectFilter)
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    string query;
                    if (string.IsNullOrEmpty(subjectFilter))
                    {
                        query = "SELECT s.SubjectName, SUM(e.TotalMarks) AS TotalMarks FROM exam e INNER JOIN subject s ON e.SubjectId = s.SubjectId WHERE e.StudentId = @StudentId GROUP BY s.SubjectName";
                    }
                    else
                    {
                        query = "SELECT s.SubjectName, e.ExamId, e.TotalMarks, e.OutOfMarks FROM exam e INNER JOIN subject s ON e.SubjectId = s.SubjectId WHERE e.StudentId = @StudentId AND s.SubjectName = @SubjectName";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    if (!string.IsNullOrEmpty(subjectFilter))
                    {
                        cmd.Parameters.AddWithValue("@SubjectName", subjectFilter);
                    }
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    StringBuilder jsonData = new StringBuilder();
                    jsonData.Append("[");
                    foreach (DataRow row in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(subjectFilter))
                        {
                            jsonData.AppendFormat("['{0}', {1}],", row["SubjectName"], row["TotalMarks"]);
                        }
                        else
                        {
                            jsonData.AppendFormat("['Exam {0} - {1}', {2}],", row["ExamId"], row["SubjectName"], row["TotalMarks"]);
                        }
                    }
                    if (dt.Rows.Count > 0) jsonData.Length--;
                    jsonData.Append("]");
                    ChartData = jsonData.ToString();
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
