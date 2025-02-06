using System;
using System.Data;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn; // Using CommonFn class
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Admin
{
    public partial class Register : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClasses(); // Load classes into dropdown on page load
            }
        }

        private void LoadClasses()
        {
            string query = "SELECT ClassID, ClassName FROM class";
            DataTable dtClasses = fn.ExecuteParameterizedQueryWithResult(query, new MySqlParameter[] { });

            ddlStudentClassID.DataSource = dtClasses;
            ddlStudentClassID.DataTextField = "ClassName";
            ddlStudentClassID.DataValueField = "ClassID";
            ddlStudentClassID.DataBind();
            ddlStudentClassID.Items.Insert(0, new ListItem("Select Class", ""));
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            string role = ddlRole.SelectedValue;

            string query = "INSERT INTO users (username, passwordHash, role) VALUES (@Username, @PasswordHash, @Role)";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Username", username),
                new MySqlParameter("@PasswordHash", hashedPassword),
                new MySqlParameter("@Role", role)
            };

            fn.ExecuteParameterizedQuery(query, parameters);
            int userId = Convert.ToInt32(fn.ExecuteParameterizedQueryWithResult("SELECT LAST_INSERT_ID();", new MySqlParameter[] { }).Rows[0][0]);

            if (role == "Student")
            {
                string studentName = txtStudentName.Text.Trim();
                DateTime studentDOB = Convert.ToDateTime(txtStudentDOB.Text);
                string studentGender = ddlStudentGender.SelectedValue;
                int studentClassID = int.Parse(ddlStudentClassID.SelectedValue);

                string studentQuery = "INSERT INTO student (Name, DOB, Gender, ClassID, UserID) VALUES (@Name, @DOB, @Gender, @ClassID, @UserID)";
                MySqlParameter[] studentParams = {
                    new MySqlParameter("@Name", studentName),
                    new MySqlParameter("@DOB", studentDOB),
                    new MySqlParameter("@Gender", studentGender),
                    new MySqlParameter("@ClassID", studentClassID),
                    new MySqlParameter("@UserID", userId)
                };

                fn.ExecuteParameterizedQuery(studentQuery, studentParams);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx");
        }
    }
}
