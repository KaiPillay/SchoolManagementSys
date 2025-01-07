using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class ClassFees : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass();
                GetFees();
            }
        }

        private void GetClass()
        {
            try
            {
                // Fetch all classes to populate the dropdown
                DataTable dt = fn.Fetch("SELECT * FROM Class");
                ddlClass.DataSource = dt;
                ddlClass.DataTextField = "ClassName";
                ddlClass.DataValueField = "ClassID";
                ddlClass.DataBind();
                ddlClass.Items.Insert(0, "Select Class");
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the class list: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a valid class is selected
                if (ddlClass.SelectedIndex == 0)
                {
                    lblStatus.Text = "Please select a class.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Get the selected ClassID and fee amount
                string classID = ddlClass.SelectedItem.Value;
                string feesAmount = txtFeesAmounts.Text.Trim();

                // Fetch the connection string from Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if fees already exist for the selected class
                    string selectQuery = "SELECT COUNT(*) FROM Fees WHERE ClassID = @ClassID";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@ClassID", classID);

                    int classCount = Convert.ToInt32(selectCmd.ExecuteScalar());

                    if (classCount == 0)
                    {
                        // Insert new fee record for the class
                        string insertQuery = "INSERT INTO Fees (ClassID, FeesAmount) VALUES (@ClassID, @FeesAmount)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                        insertCmd.Parameters.AddWithValue("@ClassID", classID);
                        insertCmd.Parameters.AddWithValue("@FeesAmount", feesAmount);

                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblStatus.Text = "Fees added successfully.";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "Failed to add the fees.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Fees already exist for this class.";
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

        private void GetFees()
        {
            try
            {
                // Fetch all fees and display them in the GridView
                DataTable dt = fn.Fetch("SELECT f.FeeID, c.ClassName, f.FeesAmount FROM Fees f INNER JOIN Class c ON f.ClassID = c.ClassID");
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the fees: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
