using System;
using System.Configuration;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Teacher
{
    public partial class TeacherAddAnnouncement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in and is a teacher
            //if (Session["UserID"] == null || Session["UserRole"].ToString() != "Teacher")
            //{
            //    Response.Redirect("~/Login.aspx"); // Redirect to login if not authorized
           // }

            if (!IsPostBack)
            {
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
                    string query = "SELECT ClassID, ClassName FROM class"; // Use your existing class table
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (lstClasses != null)
                    {
                        lstClasses.DataSource = reader;
                        lstClasses.DataTextField = "ClassName";
                        lstClasses.DataValueField = "ClassID";
                        lstClasses.DataBind();
                    }
                    else
                    {
                        lblError.Text = "ListBox control (lstClasses) not found.";
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error loading classes: " + ex.Message;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (txtTitle == null || string.IsNullOrEmpty(txtTitle.Text) ||
                txtContent == null || string.IsNullOrEmpty(txtContent.Text))
            {
                lblError.Text = "Title and Content are required";
                return;
            }

            if (chkAudience == null || (!chkAudience.Items.Cast<ListItem>().Any(li => li.Selected) &&
                (lstClasses == null || lstClasses.GetSelectedIndices().Length == 0)))
            {
                lblError.Text = "Please select at least one target audience";
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    // Insert announcement
                    string insertAnnouncement = @"INSERT INTO Announcements (Title, Content, AuthorID, CreatedDate) 
                                               VALUES (@Title, @Content, @AuthorID, NOW());
                                               SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(insertAnnouncement, conn, transaction);
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Content", txtContent.Text);
                    cmd.Parameters.AddWithValue("@AuthorID", Convert.ToInt32(Session["UserID"])); // Use the logged-in user's ID

                    int announcementId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Insert targets
                    if (chkAudience != null)
                    {
                        foreach (ListItem item in chkAudience.Items)
                        {
                            if (item.Selected)
                            {
                                InsertTarget(conn, transaction, announcementId, item.Value, null);
                            }
                        }
                    }

                    // Insert classes
                    if (lstClasses != null)
                    {
                        foreach (var classId in lstClasses.GetSelectedIndices().Select(index => lstClasses.Items[index].Value))
                        {
                            InsertTarget(conn, transaction, announcementId, "Class", classId);
                        }
                    }

                    transaction.Commit();
                    lblMessage.Text = "Announcement published successfully!";
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    lblError.Text = "Error: " + ex.Message;
                }
            }
        }

        private void InsertTarget(MySqlConnection conn, MySqlTransaction transaction, int announcementId, string targetType, string classId)
        {
            string query = @"INSERT INTO AnnouncementTargets (AnnouncementID, TargetType, ClassID)
                            VALUES (@AnnouncementID, @TargetType, @ClassID)";

            MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
            cmd.Parameters.AddWithValue("@AnnouncementID", announcementId);
            cmd.Parameters.AddWithValue("@TargetType", targetType);
            cmd.Parameters.AddWithValue("@ClassID", string.IsNullOrEmpty(classId) ? null : (object)classId);

            cmd.ExecuteNonQuery();
        }
    }
}