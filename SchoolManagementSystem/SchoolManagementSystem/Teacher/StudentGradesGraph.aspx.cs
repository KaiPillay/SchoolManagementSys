using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem.Teacher
{
    public partial class StudentGradesGraph : System.Web.UI.Page
    {
        public string ChartData { get; set; } = "[]";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSubjects();
                LoadStudentGrades(null);
            }
        }

        // Method to load subjects (you can skip this if you're not filtering by subject)
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

        // Triggered when a subject is selected from the dropdown
        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentGrades(ddlSubjects.SelectedValue);
        }

        // Method to load student grades, filtering by subject if needed
        private void LoadStudentGrades(string subjectFilter)
        {
            try
            {
                // Query to fetch all students' grades
                string query;
                if (string.IsNullOrEmpty(subjectFilter))
                {
                    query = "SELECT s.SubjectName, SUM(e.TotalMarks) AS TotalMarks " +
                            "FROM exam e INNER JOIN subject s ON e.SubjectId = s.SubjectId " +
                            "GROUP BY s.SubjectName";
                }
                else
                {
                    query = "SELECT s.SubjectName, e.ExamId, e.TotalMarks, e.OutOfMarks " +
                            "FROM exam e INNER JOIN subject s ON e.SubjectId = s.SubjectId " +
                            "WHERE s.SubjectName = @SubjectName";
                }

                // Fetch the data
                DataTable studentGrades = Fetch(query, new MySqlParameter("@SubjectName", subjectFilter));

                // Build the chart data
                StringBuilder jsonData = new StringBuilder();
                jsonData.Append("[");
                foreach (DataRow row in studentGrades.Rows)
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
                if (studentGrades.Rows.Count > 0) jsonData.Length--;
                jsonData.Append("]");
                ChartData = jsonData.ToString();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        // Helper method to fetch data from the database
        private DataTable Fetch(string query, MySqlParameter parameter)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    if (parameter != null)
                    {
                        cmd.Parameters.Add(parameter);
                    }
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
