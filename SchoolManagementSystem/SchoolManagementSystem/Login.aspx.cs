using System;
using System.Data;
using System.Web.UI;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem
{
    public partial class Login : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Page load logic, if needed
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblMsg.Text = "Please enter both username and password.";
                lblMsg.CssClass = "alert alert-warning";
                return;
            }

            try
            {
                // Fetch the user data
                string query = "SELECT Id, Username, PasswordHash, Role FROM users WHERE Username = @Username";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@Username", username)
                };
                DataTable dt = fn.Fetch(query, parameters);

                if (dt.Rows.Count == 1)
                {
                    // Check if the password matches
                    string storedPasswordHash = dt.Rows[0]["PasswordHash"].ToString();
                    if (VerifyPassword(password, storedPasswordHash))
                    {
                        string role = dt.Rows[0]["Role"].ToString();
                        Session["UserId"] = dt.Rows[0]["Id"];
                        Session["Username"] = username;
                        Session["Role"] = role;

                        // Redirect based on role
                        if (role == "Admin")
                        {
                            Response.Redirect("~/Admin/AdminHome.aspx");
                        }
                        else if (role == "Teacher")
                        {
                            Response.Redirect("~/Teacher/TeacherHome.aspx");
                        }
                        else if (role == "Student")
                        {
                            Response.Redirect("~/Student/StudentHome.aspx");
                        }
                        else
                        {
                            lblMsg.Text = "Unknown role.";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Invalid username or password.";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMsg.Text = "User not found.";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }

        // Password verification method (you can use a more secure hashing algorithm here)
        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // For demonstration purposes, using a simple string comparison.
            // Consider using a proper hashing library such as bcrypt or PBKDF2 for real applications.
            return enteredPassword == storedPasswordHash;
        }
    }
}
