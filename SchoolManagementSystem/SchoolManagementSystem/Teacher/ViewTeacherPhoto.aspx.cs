using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;

namespace SchoolManagementSystem.Teacher
{
    public partial class ViewTeacherPhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTeacherPhoto();
            }
        }

        private void LoadTeacherPhoto()
        {
            if (Request.QueryString["TeacherID"] != null)
            {
                int teacherId;
                if (int.TryParse(Request.QueryString["TeacherID"], out teacherId))
                {
                    try
                    {
                        string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = "SELECT PhotoUrl FROM Teacher WHERE TeacherID = @TeacherID";
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                imgTeacher.ImageUrl = result.ToString();
                            }
                            else
                            {
                                lblStatus.Text = "No photo found for this teacher.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "An error occurred: " + ex.Message;
                    }
                }
                else
                {
                    lblStatus.Text = "Invalid Teacher ID.";
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewTeacher.aspx");
        }
    }
}
