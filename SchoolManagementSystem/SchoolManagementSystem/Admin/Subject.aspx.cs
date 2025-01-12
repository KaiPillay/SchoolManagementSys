using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class Subject : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetSubjects();
            }
        }

        private void GetSubjects()
        {
            try
            {
                string query = "SELECT SubjectID, SubjectName FROM Subject";
                DataTable dt = fn.Fetch(query);
                GridView1.DataSource = dt;
                GridView1.DataBind();

                lblStatus.Text = "Subject list loaded successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the subject list: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
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

                    string selectQuery = "SELECT COUNT(*) FROM Subject WHERE SubjectName = @SubjectName";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@SubjectName", txtSubject.Text.Trim());

                    int subjectCount = Convert.ToInt32(selectCmd.ExecuteScalar());

                    if (subjectCount == 0)
                    {
                        string insertQuery = "INSERT INTO Subject (SubjectName) VALUES (@SubjectName)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                        insertCmd.Parameters.AddWithValue("@SubjectName", txtSubject.Text.Trim());

                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblStatus.Text = "Subject added successfully.";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "Failed to add the subject.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Subject already exists.";
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

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int subjectId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["SubjectID"]);
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                string deleteQuery = "DELETE FROM Subject WHERE SubjectID = @SubjectID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblStatus.Text = "Subject deleted successfully.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Failed to delete the subject.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }

                GetSubjects();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while deleting the subject: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetSubjects();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetSubjects();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetSubjects();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int subjectId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["SubjectID"]);
                string subjectName = (row.FindControl("txtSubjectEdit") as TextBox).Text;

                string updateQuery = "UPDATE Subject SET SubjectName = @SubjectName WHERE SubjectID = @SubjectID";
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
                    cmd.Parameters.AddWithValue("@SubjectName", subjectName);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                GridView1.EditIndex = -1;
                GetSubjects();

                lblStatus.Text = "Subject updated successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred during the update: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
