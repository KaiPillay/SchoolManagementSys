using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Data;

namespace SchoolManagementSystem.Teacher
{
    public partial class TeacherStudentGrades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind students to dropdown
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
                    // SQL query to get all students
                    string query = "SELECT StudentID, Name FROM student";

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Bind data to the dropdown list
                    ddlStudents.DataSource = reader;
                    ddlStudents.DataTextField = "Name"; // Display the student's name
                    ddlStudents.DataValueField = "StudentID"; // Use StudentID as the value
                    ddlStudents.DataBind();

                    // Add a default "Select Student" option
                    ddlStudents.Items.Insert(0, new ListItem("Select a student", ""));
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void ddlStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if a student is selected
            if (ddlStudents.SelectedValue != "")
            {
                // Fetch and display marks for the selected student
                FetchExamData(int.Parse(ddlStudents.SelectedValue));
            }
        }

        private void FetchExamData(int studentId)
        {
            // Set up the connection string
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get the exam results and SubjectName for the selected student
                    string query = @"
                        SELECT e.ExamId, s.SubjectName, e.TotalMarks, e.OutOfMarks 
                        FROM exam e
                        INNER JOIN subject s ON e.SubjectId = s.SubjectId
                        WHERE e.StudentId = @StudentId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Bind the data to the GridView
                    gvExamResults.DataSource = dt;
                    gvExamResults.DataBind();

                    // Prepare data for the chart
                    string subjects = string.Empty;
                    string marks = string.Empty;

                    foreach (DataRow row in dt.Rows)
                    {
                        subjects += "\"" + row["SubjectName"].ToString() + "\",";
                        marks += row["TotalMarks"].ToString() + ",";
                    }

                    // Remove the last comma
                    subjects = subjects.TrimEnd(',');
                    marks = marks.TrimEnd(',');

                    // Send the data to the client-side for the chart
                    ClientScript.RegisterStartupScript(this.GetType(), "CreateChart",
                        "createChart([" + subjects + "], [" + marks + "]);", true);
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}
