using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Teacher
{
    public partial class TeacherHome : System.Web.UI.Page
    {
         protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Ensure it only runs on initial load
            {
                LoadTeacherData();
            }
        }
        private void LoadTeacherData()
        {
            try
            {
                // Fetching Latest Student Attendance (No teacher-specific filter)
                string attendanceQuery = "SELECT s.Name, sa.Status, sa.Date FROM StudentAttendance sa JOIN Student s ON sa.StudentID = s.StudentID ORDER BY sa.Date DESC LIMIT 5";
                DataTable attendanceData = Fetch(attendanceQuery);
                GridViewAttendance.DataSource = attendanceData;
                GridViewAttendance.DataBind();
                lblStatus.Text = "Teacher page data loaded successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green; // Success message in green
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the teacher page data: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red; // Error message in red
            }
        }

        // Helper method to execute a scalar query and return the result
        private object GetScalarValue(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                return cmd.ExecuteScalar();
            }
        }

        // Helper method to fetch data and return a DataTable
        private DataTable Fetch(string query)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}