using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem.Admin
{
    public partial class AdminViewAnnouncements : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAnnouncements();
            }
        }

        private void BindAnnouncements()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"]?.ConnectionString;
            if (string.IsNullOrEmpty(connString))
            {
                lblError.Text = "Database connection string is missing.";
                lblError.Visible = true;
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    string query = @"SELECT a.* 
                                    FROM Announcements a
                                    JOIN AnnouncementTargets t ON a.AnnouncementID = t.AnnouncementID
                                    WHERE t.TargetType = 'AllAdmins'
                                       OR t.TargetType = 'AllStaff'
                                       OR t.TargetType = 'AllStudents'
                                    ORDER BY a.CreatedDate DESC";

                    // Debugging: Print the query
                    System.Diagnostics.Debug.WriteLine($"Query: {query}");

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Debugging: Print the number of rows fetched
                    System.Diagnostics.Debug.WriteLine($"Rows Fetched: {dt.Rows.Count}");

                    if (dt.Rows.Count > 0)
                    {
                        rptAnnouncements.DataSource = dt;
                        rptAnnouncements.DataBind();
                    }
                    else
                    {
                        lblError.Text = "No announcements found.";
                        lblError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                    lblError.Visible = true;
                }
            }
        }
    }
}