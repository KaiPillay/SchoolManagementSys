using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace SchoolManagementSystem.Parents
{
    public partial class ParentAttendance : System.Web.UI.Page
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

            if (Request.QueryString["data"] == "true")
            {
                Response.ContentType = "application/json";
                Response.Write(GetAttendanceData());
                Response.End();
            }
        }

        private void LoadStudentData()
        {
            try
            {
                int parentUserId = Convert.ToInt32(Session["UserID"]);

                // Fetch the StudentUserID linked to this ParentUserID
                string parentStudentQuery = "SELECT StudentUserID FROM parent_student WHERE ParentUserID = @ParentUserID";
                DataTable parentStudentData = Fetch(parentStudentQuery, new MySqlParameter("@ParentUserID", parentUserId));

                if (parentStudentData.Rows.Count > 0)
                {
                    int studentUserId = Convert.ToInt32(parentStudentData.Rows[0]["StudentUserID"]);

                    // Fetch StudentID and Name using StudentUserID
                    string studentQuery = "SELECT StudentID, Name FROM student WHERE UserID = @StudentUserID";
                    DataTable studentData = Fetch(studentQuery, new MySqlParameter("@StudentUserID", studentUserId));

                    if (studentData.Rows.Count > 0)
                    {
                        int studentId = Convert.ToInt32(studentData.Rows[0]["StudentID"]);
                        string studentName = studentData.Rows[0]["Name"].ToString();
                        lblStudentName.Text = studentName;

                        // Fetch Attendance Data for the Student
                        string attendanceQuery = "SELECT Status, Date FROM StudentAttendance WHERE StudentID = @StudentID ORDER BY Date DESC";
                        DataTable attendanceData = Fetch(attendanceQuery, new MySqlParameter("@StudentID", studentId));

                        gvAttendance.DataSource = attendanceData;
                        gvAttendance.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "Error: Student record not found.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblMessage.Text = "Error: No student linked to this parent.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        public string GetAttendanceData()
        {
            int parentUserId = Convert.ToInt32(Session["UserID"]);
            string studentQuery = "SELECT StudentUserID FROM parent_student WHERE ParentUserID = @ParentUserID";
            DataTable studentData = Fetch(studentQuery, new MySqlParameter("@ParentUserID", parentUserId));

            if (studentData.Rows.Count > 0)
            {
                int studentUserId = Convert.ToInt32(studentData.Rows[0]["StudentUserID"]);
                string studentIdQuery = "SELECT StudentID FROM student WHERE UserID = @StudentUserID";
                DataTable studentIdData = Fetch(studentIdQuery, new MySqlParameter("@StudentUserID", studentUserId));

                if (studentIdData.Rows.Count > 0)
                {
                    int studentId = Convert.ToInt32(studentIdData.Rows[0]["StudentID"]);
                    string attendanceQuery = @"
                        SELECT Date, 
                               SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS Present, 
                               SUM(CASE WHEN Status = 0 THEN 1 ELSE 0 END) AS Absent
                        FROM StudentAttendance 
                        WHERE StudentID = @StudentID 
                        GROUP BY Date";

                    DataTable attendanceData = Fetch(attendanceQuery, new MySqlParameter("@StudentID", studentId));

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(ConvertDataTableToList(attendanceData));
                }
            }
            return "[]";
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

        private object ConvertDataTableToList(DataTable dt)
        {
            var list = new System.Collections.Generic.List<object>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new
                {
                    Date = Convert.ToDateTime(row["Date"]).ToString("yyyy-MM-dd"),
                    Present = Convert.ToInt32(row["Present"]),
                    Absent = Convert.ToInt32(row["Absent"])
                });
            }
            return list;
        }
    }
}