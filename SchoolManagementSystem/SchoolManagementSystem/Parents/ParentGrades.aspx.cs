using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace SchoolManagementSystem.Parents
{
    public partial class ParentGrades : System.Web.UI.Page
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
                    lblError.Text = "Error: You are not logged in.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (Request.QueryString["data"] == "true")
            {
                Response.ContentType = "application/json";
                Response.Write(GetGradesData());
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

                        // Fetch Grades Data for the Student
                        string gradesQuery = @"
                            SELECT s.SubjectName, e.TotalMarks, e.OutOfMarks 
                            FROM exam e
                            INNER JOIN subject s ON e.SubjectId = s.SubjectId
                            WHERE e.StudentId = @StudentId";

                        DataTable gradesData = Fetch(gradesQuery, new MySqlParameter("@StudentId", studentId));

                        gvGrades.DataSource = gradesData;
                        gvGrades.DataBind();
                    }
                    else
                    {
                        lblError.Text = "Error: Student record not found.";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblError.Text = "Error: No student linked to this parent.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        public string GetGradesData()
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
                    string gradesQuery = @"
                        SELECT s.SubjectName, SUM(e.TotalMarks) AS TotalMarks 
                        FROM exam e
                        INNER JOIN subject s ON e.SubjectId = s.SubjectId
                        WHERE e.StudentId = @StudentId
                        GROUP BY s.SubjectName";

                    DataTable gradesData = Fetch(gradesQuery, new MySqlParameter("@StudentId", studentId));

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(ConvertDataTableToList(gradesData));
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
                    SubjectName = row["SubjectName"].ToString(),
                    TotalMarks = Convert.ToInt32(row["TotalMarks"])
                });
            }
            return list;
        }
    }
}