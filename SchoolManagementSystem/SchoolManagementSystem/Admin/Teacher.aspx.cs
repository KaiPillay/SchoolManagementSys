using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class Teacher : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTeachers();
            }
        }

        private void GetTeachers()
        {
            try
            {
                // MySQL query to fetch teacher data
                string query = "SELECT TeacherID, Name, DOB, Gender, Mobile, Email FROM Teacher";

                // Fetching data
                DataTable dt = fn.Fetch(query);
                GridView1.DataSource = dt;
                GridView1.DataBind();

                lblStatus.Text = "Teacher list loaded successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the teacher list: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the teacher already exists by email
                    string selectQuery = "SELECT COUNT(*) FROM Teacher WHERE Email = @Email";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    int teacherCount = Convert.ToInt32(selectCmd.ExecuteScalar());

                    if (teacherCount == 0)
                    {
                        // Insert a new teacher if they don't exist
                        string insertQuery = "INSERT INTO Teacher (Name, DOB, Gender, Mobile, Email, Address, Password) " +
                                             "VALUES (@Name, @DOB, @Gender, @Mobile, @Email, @Address, @Password)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                        insertCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@DOB", txtDOB.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblStatus.Text = "Teacher added successfully.";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "Failed to add the teacher.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Teacher with this email already exists.";
                        lblStatus.ForeColor = System.Drawing.Color.Orange;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int teacherId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["TeacherID"]);
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                string deleteQuery = "DELETE FROM Teacher WHERE TeacherID = @TeacherID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblStatus.Text = "Teacher deleted successfully.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Failed to delete the teacher.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }

                GetTeachers();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while deleting the teacher: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetTeachers();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetTeachers();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetTeachers();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int teacherId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["TeacherID"]);
                string teacherName = (row.FindControl("txtTeacherEdit") as TextBox).Text;

                string updateQuery = "UPDATE Teacher SET TeacherName = @TeacherName WHERE TeacherID = @TeacherID";
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
                    cmd.Parameters.AddWithValue("@TeacherName", teacherName);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                GridView1.EditIndex = -1;
                GetTeachers();

                lblStatus.Text = "Teacher updated successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred during the update: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
