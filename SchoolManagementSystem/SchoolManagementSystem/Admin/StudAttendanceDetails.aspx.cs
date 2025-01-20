using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Admin
{
    public partial class StudAttendanceDetails : System.Web.UI.Page
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
                    lblStatus.Text = "Error: " + ex.Message;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        // Handle the submit button click to record attendance
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch data from the form
                string studentID = ddlStudent.SelectedValue;
                string attendanceDate = txtDate.Text.Trim();
                string status = ddlStatus.SelectedValue;

                // Ensure that all fields are filled
                if (string.IsNullOrEmpty(studentID) || string.IsNullOrEmpty(attendanceDate) || string.IsNullOrEmpty(status))
                {
                    lblStatus.Text = "Please fill in all fields.";
                    lblStatus.ForeColor = System.Drawing.Color.Orange;
                    return;
                }

                // Database insertion logic
                string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    string query = "INSERT INTO studentattendance (StudentID, Status, Date) " +
                                   "VALUES (@StudentID, @Status, @Date)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Date", attendanceDate);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblStatus.Text = "Attendance recorded successfully.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Failed to record attendance.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
