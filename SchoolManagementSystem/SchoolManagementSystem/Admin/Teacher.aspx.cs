using System;
using System.Data;
using System.Data.SqlClient;

namespace SchoolManagementSystem.Admin
{
    public partial class Teacher : System.Web.UI.Page
    {
        // Define connection string (adjust accordingly to your setup)
        string connectionString = "your_connection_string_here";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Any necessary setup for page load
        }

        protected void btnAddTeacher_Click(object sender, EventArgs e)
        {
            // Collect data from the form fields
            string name = txtName.Text;
            DateTime dob = Convert.ToDateTime(txtDOB.Text);
            string gender = ddlGender.SelectedValue;
            string mobile = txtMobile.Text;
            string email = txtEmail.Text;
            string address = txtAddress.Text;
            string password = txtPassword.Text;

            // Save teacher data to the database
            if (AddTeacherToDatabase(name, dob, gender, mobile, email, address, password))
            {
                lblStatus.Text = "Teacher added successfully!";
            }
            else
            {
                lblStatus.Text = "An error occurred while adding the teacher.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private bool AddTeacherToDatabase(string name, DateTime dob, string gender, string mobile, string email, string address, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO teacher (Name, DOB, Gender, Mobile, Email, Address, Password) " +
                                   "VALUES (@Name, @DOB, @Gender, @Mobile, @Email, @Address, @Password)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@DOB", dob);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Mobile", mobile);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0; // Return true if a row was inserted
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any database errors (e.g., connection issues)
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return false;
            }
        }
    }
}
