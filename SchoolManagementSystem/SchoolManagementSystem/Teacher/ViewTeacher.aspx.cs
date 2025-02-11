using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;

namespace SchoolManagementSystem.Teacher
{
    public partial class ViewTeacher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTeachers();
            }
        }

        // Method to load teachers' names, emails, and genders into the GridView
        private void LoadTeachers()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT Name, Email, IFNULL(Gender, '') AS Gender FROM Teacher";
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
    }
}