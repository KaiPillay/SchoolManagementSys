using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

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

        private void BindGridView()
        {
            try
            {
                // MySQL query to fetch data
                string query = "SELECT * FROM Class";
                DataTable dt = fn.Fetch(query); // Assuming `fn.Fetch` is properly implemented in Commonfnx
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while binding the GridView: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetClass();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetClass();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetClass(); // Refresh the GridView in edit mode
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Retrieve the row being updated
                GridViewRow row = GridView1.Rows[e.RowIndex];

                // Get the primary key value
                int cId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["ClassID"]);

                // Find the textbox in the row and get the updated value
                string ClassName = (row.FindControl("txtClassEdit") as TextBox).Text;

                // Update query using parameterised command
                string updateQuery = "UPDATE Class SET ClassName = @ClassName WHERE ClassID = @ClassID";
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
                    cmd.Parameters.AddWithValue("@ClassName", ClassName);
                    cmd.Parameters.AddWithValue("@ClassID", cId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                GridView1.EditIndex = -1;
                GetClass(); // Refresh the GridView after the update

                lblStatus.Text = "Class updated successfully.";
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
