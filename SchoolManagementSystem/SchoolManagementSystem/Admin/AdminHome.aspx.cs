using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Admin
{
    public partial class AdminHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Ensure it only runs on initial load
            {
                LoadHomePageData();
            }
        }

        private void LoadHomePageData()
        {
            try
            {
                // Fetching Class Count
                string classQuery = "SELECT COUNT(*) FROM Class";
                int classCount = Convert.ToInt32(GetScalarValue(classQuery));  // Explicit cast
                lblClassCount.Text = $"Total Classes: {classCount}";

                // Fetching Student Count
                string studentQuery = "SELECT COUNT(*) FROM Student";
                int studentCount = Convert.ToInt32(GetScalarValue(studentQuery));  // Explicit cast
                lblStudentCount.Text = $"Total Students: {studentCount}";

                // Fetching Subject Count
                string subjectQuery = "SELECT COUNT(*) FROM Subject";
                int subjectCount = Convert.ToInt32(GetScalarValue(subjectQuery));  // Explicit cast
                lblSubjectCount.Text = $"Total Subjects: {subjectCount}";

                // Fetching Teacher Count
                string teacherQuery = "SELECT COUNT(*) FROM Teacher";
                int teacherCount = Convert.ToInt32(GetScalarValue(teacherQuery));  // Explicit cast
                lblTeacherCount.Text = $"Total Teachers: {teacherCount}";

                // Fetching Total Fees
                string feesQuery = "SELECT SUM(FeesAmount) FROM Fees";
                decimal totalFees = Convert.ToDecimal(GetScalarValue(feesQuery));  // Explicit cast for decimal
                lblTotalFees.Text = $"Total Fees Collected: £{totalFees:F2}";

                // Fetching Latest Student Attendance
                string attendanceQuery = "SELECT s.Name, sa.Status, sa.Date FROM StudentAttendance sa JOIN Student s ON sa.StudentID = s.StudentID ORDER BY sa.Date DESC LIMIT 5";
                DataTable attendanceData = Fetch(attendanceQuery);
                GridViewAttendance.DataSource = attendanceData;
                GridViewAttendance.DataBind();

                lblStatus.Text = "Home page data loaded successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green; // Success message in green
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the home page data: " + ex.Message;
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
