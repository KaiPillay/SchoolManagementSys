using System;
using System.Data;
using System.Web.UI;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;
using BCrypt.Net;

namespace SchoolManagementSystem
{
    public partial class Login : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();  // Instantiate the Commonfnx class to handle DB operations

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
                lblMsg.Visible = true; // Ensure the label is visible
                return;
            }

            try
            {
                // Debugging output
                Debug.WriteLine("Checking user: " + username);

                // Using parameterized query to get the user details from the database
                string query = "SELECT Id, Username, PasswordHash, Role FROM users WHERE Username = @Username";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@Username", MySqlDbType.VarChar) { Value = username }
                };

                // Using the ExecuteParameterizedQueryWithResult method from Commonfnx class
                DataTable dt = fn.ExecuteParameterizedQueryWithResult(query, parameters);

                Debug.WriteLine("Fetched user count: " + dt.Rows.Count);

                if (dt.Rows.Count == 1)
                {
                    string storedPasswordHash = dt.Rows[0]["PasswordHash"].ToString();

                    // Verify the password using BCrypt
                    if (VerifyPassword(password, storedPasswordHash))
                    {
                        Debug.WriteLine("Password verified");

                        string role = dt.Rows[0]["Role"].ToString();
                        Session["UserId"] = dt.Rows[0]["Id"];
                        Session["Username"] = username;
                        Session["Role"] = role;

                        // Check if there's a ReturnUrl and redirect accordingly
                        string returnUrl = Request.QueryString["ReturnUrl"];
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            Response.Redirect(returnUrl); // Redirect to the original requested page
                        }
                        else
                        {
                            // Default redirect based on role
                            RedirectUserBasedOnRole(role);
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Incorrect password. Please try again.";
                        lblMsg.CssClass = "alert alert-danger";
                        lblMsg.Visible = true; // Ensure the label is visible
                    }
                }
                else
                {
                    lblMsg.Text = "Username not found. Please check your username.";
                    lblMsg.CssClass = "alert alert-danger";
                    lblMsg.Visible = true; // Ensure the label is visible
                }
            }
            catch (MySqlException dbEx)
            {
                // Specific database error handling
                lblMsg.Text = "Database error: " + dbEx.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true; // Ensure the label is visible
            }
            catch (Exception ex)
            {
                // General error handling
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true; // Ensure the label is visible
            }
        }

        // Password verification method using BCrypt
        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }

        // Method to redirect users based on their role
        private void RedirectUserBasedOnRole(string role)
        {
            switch (role)
            {
                case "Admin":
                    Response.Redirect("~/Admin/AdminHome.aspx");
                    break;
                case "Teacher":
                    Response.Redirect("~/Teacher/TeacherHome.aspx");
                    break;
                case "Student":
                    Response.Redirect("~/Student/StudentHome.aspx");
                    break;
                case "Parent":
                    Response.Redirect("~/Parents/ParentHome.aspx");
                    break;
                default:
                    lblMsg.Text = "Unknown role. Please contact support.";
                    lblMsg.CssClass = "alert alert-danger";
                    break;
            }
        }
    }
}
