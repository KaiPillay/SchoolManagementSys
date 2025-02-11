using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem.Admin
{
    public partial class Teacher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblStatus.Text = "";
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

                    string selectQuery = "SELECT COUNT(*) FROM Teacher WHERE Email = @Email";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    int teacherCount = Convert.ToInt32(selectCmd.ExecuteScalar());

                    if (teacherCount == 0)
                    {
                        string insertQuery = "INSERT INTO Teacher (Name, DOB, Gender, Mobile, Email, Address, Password, PhotoUrl) " +
                                             "VALUES (@Name, @DOB, @Gender, @Mobile, @Email, @Address, @Password, @PhotoUrl)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                        insertCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@DOB", txtDOB.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@PhotoUrl", txtPhotoUrl.Text.Trim());  // Add Photo URL

                        insertCmd.ExecuteNonQuery();
                        lblStatus.Text = "Teacher added successfully.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
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
    }
}
