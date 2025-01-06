using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;
using System.Configuration;

namespace SchoolManagementSystem.Admin
{
    public partial class AddClass : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Ensure it only runs on initial load
            {
                GetClass();
            }
        }

        private void GetClass()
        {
            try
            {
                // MySQL query using ROW_NUMBER()
                string query = @"
                SELECT 
                    ROW_NUMBER() OVER(ORDER BY ClassID) AS SrNo, 
                    ClassID, 
                    ClassName 
                FROM Class";

                // Fetching data
                DataTable dt = fn.Fetch(query);
                GridView1.DataSource = dt;
                GridView1.DataBind();

                lblStatus.Text = "Class list loaded successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green; // Success message in green
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the class list: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red; // Error message in red
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch the connection string from Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the class already exists
                    string selectQuery = "SELECT COUNT(*) FROM Class WHERE ClassName = @ClassName";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@ClassName", txtClass.Text.Trim());

                    int classCount = Convert.ToInt32(selectCmd.ExecuteScalar());

                    if (classCount == 0)
                    {
                        // Insert a new class if it doesn't exist
                        string insertQuery = "INSERT INTO Class (ClassName) VALUES (@ClassName)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                        insertCmd.Parameters.AddWithValue("@ClassName", txtClass.Text.Trim());

                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblStatus.Text = "Class added successfully.";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "Failed to add the class.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Class already exists.";
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


    }
}
