using System;
using System.Data;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn; // Using CommonFn class
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Admin
{
    public partial class RegisterParent : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadParents();
                LoadStudents();
            }
        }

        // Load all parents into the dropdown
        private void LoadParents()
        {
            string query = "SELECT Id, Username FROM users WHERE Role = 'Parent'";
            DataTable dtParents = fn.ExecuteParameterizedQueryWithResult(query, new MySqlParameter[] { });

            ddlParents.DataSource = dtParents;
            ddlParents.DataTextField = "Username";
            ddlParents.DataValueField = "Id";
            ddlParents.DataBind();
            ddlParents.Items.Insert(0, new ListItem("Select Parent", ""));
        }

        // Load all students into the checkbox list
        private void LoadStudents()
        {
            string query = "SELECT Id, Username FROM users WHERE Role = 'Student'";
            DataTable dtStudents = fn.ExecuteParameterizedQueryWithResult(query, new MySqlParameter[] { });

            chkStudents.DataSource = dtStudents;
            chkStudents.DataTextField = "Username";
            chkStudents.DataValueField = "Id";
            chkStudents.DataBind();
        }

        // Handle parent assignment to selected students
        protected void btnAssign_Click(object sender, EventArgs e)
        {
            // Check if a parent is selected
            if (ddlParents.SelectedIndex == 0)
            {
                lblMessage.Text = "Please select a parent.";
                lblMessage.CssClass = "alert alert-warning";
                lblMessage.Visible = true;
                return;
            }

            int parentId = int.Parse(ddlParents.SelectedValue);

            // Remove previous assignments to prevent duplicates
            string deleteQuery = "DELETE FROM parent_student WHERE ParentUserID = @ParentID";
            fn.ExecuteParameterizedQuery(deleteQuery, new MySqlParameter[] { new MySqlParameter("@ParentID", parentId) });

            bool studentAssigned = false;

            // Loop through each selected student
            foreach (ListItem student in chkStudents.Items)
            {
                if (student.Selected)
                {
                    studentAssigned = true;
                    int studentId = int.Parse(student.Value);

                    // Check if the combination of ParentUserID and StudentUserID already exists
                    string checkQuery = "SELECT COUNT(*) FROM parent_student WHERE ParentUserID = @ParentID AND StudentUserID = @StudentID";
                    MySqlParameter[] checkParameters = {
                        new MySqlParameter("@ParentID", parentId),
                        new MySqlParameter("@StudentID", studentId)
                    };

                    int count = Convert.ToInt32(fn.ExecuteParameterizedQueryWithResult(checkQuery, checkParameters).Rows[0][0]);

                    // If the combination does not exist, insert the new assignment
                    if (count == 0)
                    {
                        string insertQuery = "INSERT INTO parent_student (ParentUserID, StudentUserID) VALUES (@ParentID, @StudentID)";
                        MySqlParameter[] insertParameters = {
                            new MySqlParameter("@ParentID", parentId),
                            new MySqlParameter("@StudentID", studentId)
                        };
                        fn.ExecuteParameterizedQuery(insertQuery, insertParameters);
                    }
                }
            }

            // Display success or error message
            if (studentAssigned)
            {
                lblMessage.Text = "Parent assigned to selected students successfully!";
                lblMessage.CssClass = "alert alert-success";
            }
            else
            {
                lblMessage.Text = "No students selected!";
                lblMessage.CssClass = "alert alert-danger";
            }
            lblMessage.Visible = true;
        }

        // Redirect back to Admin home page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx");
        }
    }
}
