using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class TeacherSubject : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTeacherDropdown();
                LoadClassDropdown();
                LoadSubjectDropdown();
                LoadAssignments();
            }
        }

        // Load Teacher dropdown
        private void LoadTeacherDropdown()
        {
            try
            {
                string query = "SELECT TeacherID, Name FROM Teacher";
                DataTable dt = fn.Fetch(query);
                ddlTeacher.DataSource = dt;
                ddlTeacher.DataTextField = "Name";  // Correct field name
                ddlTeacher.DataValueField = "TeacherID";
                ddlTeacher.DataBind();
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }


        // Load Class dropdown
        private void LoadClassDropdown()
        {
            try
            {
                string query = "SELECT ClassID, ClassName FROM Class";
                DataTable dt = fn.Fetch(query);
                ddlClass.DataSource = dt;
                ddlClass.DataTextField = "ClassName";
                ddlClass.DataValueField = "ClassID";
                ddlClass.DataBind();
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        // Load Subject dropdown
        private void LoadSubjectDropdown()
        {
            try
            {
                string query = "SELECT SubjectID, SubjectName FROM Subject";
                DataTable dt = fn.Fetch(query);
                ddlSubject.DataSource = dt;
                ddlSubject.DataTextField = "SubjectName";
                ddlSubject.DataValueField = "SubjectID";
                ddlSubject.DataBind();
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        // Load Teacher-Subject-Class Assignments
        private void LoadAssignments()
        {
            try
            {
                string query = @"SELECT ts.Id, t.Name, c.ClassName, s.SubjectName 
                                 FROM teachersubject ts
                                 JOIN Teacher t ON ts.TeacherID = t.TeacherID
                                 JOIN Class c ON ts.ClassID = c.ClassID
                                 JOIN Subject s ON ts.SubjectID = s.SubjectID";

                DataTable dt = fn.Fetch(query);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        // Assign Teacher to Class and Subject
        protected void btnAssignTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                int teacherId = int.Parse(ddlTeacher.SelectedValue);
                int classId = int.Parse(ddlClass.SelectedValue);
                int subjectId = int.Parse(ddlSubject.SelectedValue);

                // Insert Teacher-Subject-Class Assignment into the database
                string query = "INSERT INTO teachersubject (TeacherID, ClassID, SubjectID) VALUES (@TeacherID, @ClassID, @SubjectID)";
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadAssignments();  // Reload the assignments grid
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        // GridView paging event
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            LoadAssignments();
        }

        // GridView row deletion event
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
                string query = "DELETE FROM teachersubject WHERE Id = @Id";
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadAssignments();  // Reload the assignments grid
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
    }
}
