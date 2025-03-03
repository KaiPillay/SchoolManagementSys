using System;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class LinkParentStudent : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadParents();
                LoadStudents();
                LoadLinkedData();
            }
        }

        private void LoadParents()
        {
            string query = "SELECT Id, Username FROM users WHERE Role = 'Parent'";
            var dt = fn.ExecuteParameterizedQueryWithResult(query, new MySqlParameter[] { });

            if (dt == null || dt.Rows.Count == 0)
            {
                lblMsg.Text = "No parents found in the database.";
                lblMsg.CssClass = "alert alert-warning";
                lblMsg.Visible = true;
                return;
            }

            ddlParent.DataSource = dt;
            ddlParent.DataTextField = "Username"; // Display the username in the dropdown
            ddlParent.DataValueField = "Id"; // Use the Id as the value
            ddlParent.DataBind();
            ddlParent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Parent", ""));
        }

        private void LoadStudents()
        {
            string query = "SELECT s.StudentID, u.Username FROM student s " +
                           "INNER JOIN users u ON s.UserID = u.Id";
            var dt = fn.ExecuteParameterizedQueryWithResult(query, new MySqlParameter[] { });

            if (dt == null || dt.Rows.Count == 0)
            {
                lblMsg.Text = "No students found in the database.";
                lblMsg.CssClass = "alert alert-warning";
                lblMsg.Visible = true;
                return;
            }

            ddlStudent.DataSource = dt;
            ddlStudent.DataTextField = "Username"; // Display the username in the dropdown
            ddlStudent.DataValueField = "StudentID"; // Use the StudentID as the value
            ddlStudent.DataBind();
            ddlStudent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Student", ""));
        }

        private void LoadLinkedData()
        {
            string query = "SELECT ps.ParentUserID, p.Username AS ParentName, ps.StudentUserID, s.Username AS StudentName " +
                           "FROM parent_student ps " +
                           "INNER JOIN users p ON ps.ParentUserID = p.Id " +
                           "INNER JOIN users s ON ps.StudentUserID = s.Id";
            var dt = fn.ExecuteParameterizedQueryWithResult(query, new MySqlParameter[] { });
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnLink_Click(object sender, EventArgs e)
        {
            string parentId = ddlParent.SelectedValue;
            string studentId = ddlStudent.SelectedValue;

            if (string.IsNullOrEmpty(parentId) || string.IsNullOrEmpty(studentId))
            {
                lblMsg.Text = "Please select both parent and student.";
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
                return;
            }

            string query = "INSERT INTO parent_student (ParentUserID, StudentUserID) VALUES (@ParentUserID, @StudentUserID)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@ParentUserID", MySqlDbType.Int32) { Value = parentId },
                new MySqlParameter("@StudentUserID", MySqlDbType.Int32) { Value = studentId }
            };

            try
            {
                fn.ExecuteParameterizedQuery(query, parameters);
                lblMsg.Text = "Parent and student linked successfully!";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;
                LoadLinkedData(); // Refresh the linked data grid
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx");
        }
    }
}