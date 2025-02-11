using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.UI.WebControls;


namespace SchoolManagementSystem.Admin
{
    public partial class EditTeacher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTeachers();
            }
        }

        // Method to load all teachers into the GridView
        private void LoadTeachers()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT TeacherId, Name, DOB, Gender, Mobile, Email, Address, PhotoUrl FROM Teacher";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(selectQuery, connection);
                    DataTable teacherTable = new DataTable();
                    dataAdapter.Fill(teacherTable);

                    gvTeachers.DataSource = teacherTable;
                    gvTeachers.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        // Method to handle the Edit button click
        protected void gvTeachers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditTeacher")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gvTeachers.Rows[index];

                string teacherId = selectedRow.Cells[0].Text; // TeacherId is in the first column

                // Retrieve teacher details based on TeacherId
                LoadTeacherDetails(teacherId);
            }
        }

        // Method to load teacher details into the form for editing
        private void LoadTeacherDetails(string teacherId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM Teacher WHERE TeacherId = @TeacherId";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@TeacherId", teacherId);

                    MySqlDataReader reader = selectCmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtName.Text = reader["Name"].ToString();
                        txtDOB.Text = Convert.ToDateTime(reader["DOB"]).ToString("yyyy-MM-dd");
                        ddlGender.SelectedValue = reader["Gender"].ToString();
                        txtMobile.Text = reader["Mobile"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtAddress.Text = reader["Address"].ToString();
                        txtPassword.Text = reader["Password"].ToString();
                        txtPhotoUrl.Text = reader["PhotoUrl"].ToString();  // Load Photo URL
                        hfTeacherId.Value = teacherId;  // Store TeacherId for update
                    }
                    else
                    {
                        lblStatus.Text = "Teacher not found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        // Method to handle the Update button click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string teacherId = hfTeacherId.Value; // Get TeacherId from hidden field

                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Teacher SET Name = @Name, DOB = @DOB, Gender = @Gender, Mobile = @Mobile, " +
                                         "Email = @Email, Address = @Address, Password = @Password, PhotoUrl = @PhotoUrl WHERE TeacherId = @TeacherId";

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@DOB", txtDOB.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@PhotoUrl", txtPhotoUrl.Text.Trim());  // Update Photo URL
                    updateCmd.Parameters.AddWithValue("@TeacherId", teacherId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblStatus.Text = "Teacher updated successfully.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        LoadTeachers();  // Reload the grid after update
                    }
                    else
                    {
                        lblStatus.Text = "No changes were made.";
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
    }
}
