using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Parents
{
    public partial class ParentHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    LoadStudentData();
                }
                else
                {
                    lblMessage.Text = "Error: You are not logged in.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void LoadStudentData()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                // Fetch StudentID and Name using UserID
                string studentQuery = "SELECT StudentID, Name FROM student WHERE UserID = @UserID";
                DataTable studentData = Fetch(studentQuery, new MySqlParameter("@UserID", userId));

                if (studentData.Rows.Count > 0)
                {
                    int studentId = Convert.ToInt32(studentData.Rows[0]["StudentID"]);
                    string studentName = studentData.Rows[0]["Name"].ToString();
                    lblStudentName.Text = studentName;

                    // Fetch Attendance Data for the Student
                    string attendanceQuery = "SELECT Status, Date FROM StudentAttendance WHERE StudentID = @StudentID ORDER BY Date DESC LIMIT 5";
                    DataTable attendanceData = Fetch(attendanceQuery, new MySqlParameter("@StudentID", studentId));

                    GridViewAttendance.DataSource = attendanceData;
                    GridViewAttendance.DataBind();
                }
                else
                {
                    lblMessage.Text = "Error: Student record not found.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
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