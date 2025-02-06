using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Teacher
{
    public partial class AddClassAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind classes to dropdown
                BindClasses();
            }
        }

        private void BindClasses()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    string query = "SELECT ClassID, ClassName FROM class";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    ddlClass.DataSource = reader;
                    ddlClass.DataTextField = "ClassName";
                    ddlClass.DataValueField = "ClassID";
                    ddlClass.DataBind();
                    ddlClass.Items.Insert(0, new ListItem("Select Class", ""));
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error: " + ex.Message;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStudents();
        }

        private void BindStudents()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    string query = "SELECT StudentID, Name FROM student WHERE ClassID = @ClassID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ClassID", ddlClass.SelectedValue);
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    rptStudents.DataSource = reader;
                    rptStudents.DataBind();
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error: " + ex.Message;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    foreach (RepeaterItem item in rptStudents.Items)
                    {
                        HiddenField hfStudentID = (HiddenField)item.FindControl("hfStudentID");
                        DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");

                        if (!string.IsNullOrEmpty(hfStudentID.Value) && !string.IsNullOrEmpty(ddlStatus.SelectedValue))
                        {
                            string query = "INSERT INTO studentattendance (StudentID, Status, Date) VALUES (@StudentID, @Status, @Date)";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@StudentID", hfStudentID.Value);
                            cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@Date", txtDate.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                lblStatus.Text = "Attendance recorded successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}