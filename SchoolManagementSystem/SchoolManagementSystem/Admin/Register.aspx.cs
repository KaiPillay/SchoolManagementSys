using System;
using System.Data;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using static SchoolManagementSystem.Models.CommonFn; // Using your CommonFn class
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Admin
{
    public partial class Register : System.Web.UI.Page
    {
        // Create an instance of your Commonfnx class (assuming it's used for DB operations)
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Ensure it only runs on initial load
            {
                // Any logic you want to run on the first page load
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Check if passwords match
            if (password != confirmPassword)
            {
                lblMsg.Text = "Passwords do not match!";
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
                return;
            }

            // Check if the user already exists using CommonFn
            if (UserExists(username))
            {
                lblMsg.Text = "The username is already registered.";
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
                return;
            }

            // Get the selected role from the dropdown
            string role = ddlRole.SelectedValue;

            // Hash the password for security using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // SQL query to insert the new user into the users table
            string query = "INSERT INTO users (username, passwordHash, role) VALUES (@Username, @PasswordHash, @Role)";

            // Define parameters for the query
            MySqlParameter[] parameters = new MySqlParameter[]
            {
            new MySqlParameter("@Username", MySqlDbType.VarChar) { Value = username },
            new MySqlParameter("@PasswordHash", MySqlDbType.VarChar) { Value = hashedPassword },
            new MySqlParameter("@Role", MySqlDbType.VarChar) { Value = role }
            };

            try
            {
                // Execute the query using the ExecuteParameterizedQuery method from CommonFn (no need for rowsAffected)
                fn.ExecuteParameterizedQuery(query, parameters);

                lblMsg.Text = "User added successfully!";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;

                // Clear form fields after successful registration
                ClearFormFields();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }


        private bool UserExists(string username)
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @Username";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Username", MySqlDbType.VarChar) { Value = username }
            };

            try
            {
                // Execute the query and get the result (returning true if the user exists)
                int userCount = Convert.ToInt32(fn.ExecuteParameterizedQueryWithResult(query, parameters).Rows[0][0]);

                return userCount > 0; // User already exists if count is greater than 0
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred while checking if the user exists: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
                return false;
            }
        }

        // Helper function to clear the form fields after successful registration
        private void ClearFormFields()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect to AdminHome.aspx
            Response.Redirect("AdminHome.aspx");
        }

    }
}
