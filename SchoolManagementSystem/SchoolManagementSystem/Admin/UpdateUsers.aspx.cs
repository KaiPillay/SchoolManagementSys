using System;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class UpdateUsers : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userId = Request.QueryString["UserId"];
                if (!string.IsNullOrEmpty(userId))
                {
                    LoadUserDetails(userId);
                }
            }
        }

        private void LoadUserDetails(string userId)
        {
            string query = "SELECT username, role FROM users WHERE userId = @UserId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@UserId", MySqlDbType.Int32) { Value = userId }
            };

            try
            {
                var dt = fn.ExecuteParameterizedQueryWithResult(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    txtUsername.Text = dt.Rows[0]["username"].ToString();
                    ddlRole.SelectedValue = dt.Rows[0]["role"].ToString();
                }
                else
                {
                    lblMsg.Text = "User not found.";
                    lblMsg.CssClass = "alert alert-danger";
                    lblMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserId"];
            string username = txtUsername.Text.Trim();
            string role = ddlRole.SelectedValue;

            string query = "UPDATE users SET username = @Username, role = @Role WHERE userId = @UserId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Username", MySqlDbType.VarChar) { Value = username },
                new MySqlParameter("@Role", MySqlDbType.VarChar) { Value = role },
                new MySqlParameter("@UserId", MySqlDbType.Int32) { Value = userId }
            };

            try
            {
                fn.ExecuteParameterizedQuery(query, parameters);
                lblMsg.Text = "User updated successfully!";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserId"];

            if (string.IsNullOrEmpty(userId))
            {
                lblMsg.Text = "User ID is not valid.";
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
                return;
            }

            string query = "DELETE FROM users WHERE userId = @UserId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@UserId", MySqlDbType.Int32) { Value = userId }
            };

            try
            {
                fn.ExecuteParameterizedQuery(query, parameters);
                lblMsg.Text = "User deleted successfully!";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;
                // Optionally redirect to a different page after deletion
                Response.Redirect("ManageUsers.aspx");
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred while deleting the user: " + ex.Message;
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
