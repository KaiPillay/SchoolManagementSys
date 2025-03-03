using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace SchoolManagementSystem.Student
{
    public partial class StudentAttendanceGraph : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    LoadStudentName();
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }

            if (Request.QueryString["data"] == "true")
            {
                Response.ContentType = "application/json";
                Response.Write(GetAttendanceData());
                Response.End();
            }
        }

        private void LoadStudentName()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string query = "SELECT Name FROM student WHERE UserID = @UserID";
            DataTable studentData = Fetch(query, new MySqlParameter("@UserID", userId));

            if (studentData.Rows.Count > 0)
            {
                lblStudentName.Text = studentData.Rows[0]["Name"].ToString();
            }
        }

        public string GetAttendanceData()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string studentQuery = "SELECT StudentID FROM student WHERE UserID = @UserID";
            DataTable studentData = Fetch(studentQuery, new MySqlParameter("@UserID", userId));

            if (studentData.Rows.Count > 0)
            {
                int studentId = Convert.ToInt32(studentData.Rows[0]["StudentID"]);
                string attendanceQuery = @"
                    SELECT Date, 
                           SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS Present, 
                           SUM(CASE WHEN Status = 0 THEN 1 ELSE 0 END) AS Absent,
                           SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END) AS Late
                    FROM StudentAttendance 
                    WHERE StudentID = @StudentID 
                    GROUP BY Date";

                DataTable attendanceData = Fetch(attendanceQuery, new MySqlParameter("@StudentID", studentId));

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ConvertDataTableToList(attendanceData));
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
