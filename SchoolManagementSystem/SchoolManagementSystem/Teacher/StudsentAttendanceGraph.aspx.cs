using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace SchoolManagementSystem.Teacher
{
    public partial class StudsentAttendanceGraph : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    LoadStudents(); // Populate dropdown
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }

            if (Request.QueryString["data"] == "true" && Request.QueryString["studentId"] != null)
            {
                int studentId;
                if (int.TryParse(Request.QueryString["studentId"], out studentId))
                {
                    Response.ContentType = "application/json";
                    Response.Write(GetAttendanceData(studentId));
                    Response.End();
                }
            }
        }

        private void LoadStudents()
        {
            string query = "SELECT StudentID, Name FROM student ORDER BY Name";
            DataTable students = Fetch(query);

            ddlStudents.DataSource = students;
            ddlStudents.DataTextField = "Name";
            ddlStudents.DataValueField = "StudentID";
            ddlStudents.DataBind();

            ddlStudents.Items.Insert(0, new ListItem("-- Select a Student --", ""));
        }

        public string GetAttendanceData(int studentId)
        {
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

        private DataTable Fetch(string query, params MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

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