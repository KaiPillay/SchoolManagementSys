using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;


namespace SchoolManagementSystem.Teacher
{
    public partial class StudsentAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Call method to bind students to dropdown
                BindStudents();
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
                    // SQL query to get student IDs and Names
                    string query = "SELECT StudentID, Name FROM student";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Bind the students to the dropdown list
                    ddlStudent.DataSource = reader;
                    ddlStudent.DataTextField = "Name";  // Display the student name
                    ddlStudent.DataValueField = "StudentID"; // Use StudentID as the value
                    ddlStudent.DataBind();

                    // Add a default "Select Student" option
                    ddlStudent.Items.Insert(0, new ListItem("Select Student", ""));
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    lblStatus.Text = "Error: " + ex.Message;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Fetch selected student's attendance records
            int studentId;
            if (int.TryParse(ddlStudent.SelectedValue, out studentId) && studentId > 0)
            {
                // Fetch and bind attendance data for the selected student
                BindAttendanceData(studentId);
            }
            else
            {
                gvAttendance.DataSource = null;
                gvAttendance.DataBind();
            }
        }

        private void BindAttendanceData(int studentId)
        {
            // Set up the connection string
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to fetch attendance details of the selected student
                    string query = "SELECT s.Name, sa.Date, " +
                                   "CASE WHEN sa.Status = 1 THEN 'Present' ELSE 'Absent' END AS Status " +
                                   "FROM studentattendance sa " +
                                   "JOIN student s ON sa.StudentID = s.StudentID " +
                                   "WHERE sa.StudentID = @StudentID ORDER BY sa.Date DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the data to the GridView
                    gvAttendance.DataSource = dt;
                    gvAttendance.DataBind();
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    lblStatus.Text = "Error: " + ex.Message;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}
