using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class Expense : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClasses();
                GetSubjects();
                LoadExpenses();
            }
        }

        private void GetClasses()
        {
            try
            {
                string query = "SELECT ClassID, ClassName FROM Class";
                DataTable dt = fn.Fetch(query);
                ddlClass.DataSource = dt;
                ddlClass.DataTextField = "ClassName";
                ddlClass.DataValueField = "ClassID";
                ddlClass.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the class list: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void GetSubjects()
        {
            try
            {
                string query = "SELECT SubjectID, SubjectName FROM Subject";
                DataTable dt = fn.Fetch(query);
                ddlSubject.DataSource = dt;
                ddlSubject.DataTextField = "SubjectName";
                ddlSubject.DataValueField = "SubjectID";
                ddlSubject.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the subject list: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void LoadExpenses()
        {
            try
            {
                string query = "SELECT e.ExpenseId, c.ClassName, s.SubjectName, e.ChargeAmount, e.Details FROM Expense e " +
                               "JOIN Class c ON e.ClassId = c.ClassID " +
                               "JOIN Subject s ON e.SubjectId = s.SubjectID";
                DataTable dt = fn.Fetch(query);
                GridViewExpenses.DataSource = dt;
                GridViewExpenses.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading expenses: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }


        protected void btnAddExpense_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Expense (ClassId, SubjectId, ChargeAmount, Details) VALUES (@ClassId, @SubjectId, @ChargeAmount, @Details)";
                    MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@ClassId", ddlClass.SelectedValue);
                    cmd.Parameters.AddWithValue("@SubjectId", ddlSubject.SelectedValue);
                    cmd.Parameters.AddWithValue("@ChargeAmount", txtChargeAmount.Text.Trim());
                    cmd.Parameters.AddWithValue("@Details", txtDetails.Text.Trim());  // Add this line for the new Details field

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblStatus.Text = "Expense added successfully.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        LoadExpenses();
                    }
                    else
                    {
                        lblStatus.Text = "Failed to add expense.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }


        protected void GridViewExpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewExpenses.PageIndex = e.NewPageIndex;
            LoadExpenses();
        }

        protected void GridViewExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int expenseId = Convert.ToInt32(GridViewExpenses.DataKeys[e.RowIndex].Values["ExpenseId"]);

                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                string deleteQuery = "DELETE FROM Expense WHERE ExpenseId = @ExpenseId";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);
                    cmd.Parameters.AddWithValue("@ExpenseId", expenseId);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblStatus.Text = "Expense deleted successfully.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        LoadExpenses();
                    }
                    else
                    {
                        lblStatus.Text = "Failed to delete expense.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while deleting the expense: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
