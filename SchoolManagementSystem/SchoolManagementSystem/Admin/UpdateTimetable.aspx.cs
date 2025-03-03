using System;
using System.Data;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class UpdateTimetable : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTeachers();
                LoadClasses();
                LoadStudents();
                LoadSubjects();
            }
        }

        // Back Button Click Event
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx");
        }

        private void LoadStudents()
        {
            try
            {
                string query = "SELECT StudentID, Name FROM student";
                DataTable dt = fn.Fetch(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlStudents.DataSource = dt;
                    ddlStudents.DataTextField = "Name";
                    ddlStudents.DataValueField = "StudentID";
                    ddlStudents.DataBind();
                    ddlStudents.Items.Insert(0, new ListItem("Select Student", ""));
                }
                else
                {
                    lblMsg.Text = "No students found.";
                    lblMsg.CssClass = "alert alert-warning";
                    lblMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        private void LoadClasses()
        {
            try
            {
                string query = "SELECT ClassID, ClassName FROM class";
                DataTable dt = fn.Fetch(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlClass.DataSource = dt;
                    ddlClass.DataTextField = "ClassName";
                    ddlClass.DataValueField = "ClassID";
                    ddlClass.DataBind();
                    ddlClass.Items.Insert(0, new ListItem("Select Class", ""));
                }
                else
                {
                    lblMsg.Text = "No classes found.";
                    lblMsg.CssClass = "alert alert-warning";
                    lblMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        private void LoadTeachers()
        {
            try
            {
                string query = "SELECT TeacherID, Name FROM teacher";
                DataTable dt = fn.Fetch(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlTeacher.DataSource = dt;
                    ddlTeacher.DataTextField = "Name";
                    ddlTeacher.DataValueField = "TeacherID";
                    ddlTeacher.DataBind();
                    ddlTeacher.Items.Insert(0, new ListItem("Select Teacher", ""));
                }
                else
                {
                    lblMsg.Text = "No teachers found.";
                    lblMsg.CssClass = "alert alert-warning";
                    lblMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        private void LoadSubjects()
        {
            try
            {
                string query = "SELECT SubjectID, SubjectName FROM subject";
                DataTable dt = fn.Fetch(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlSubject.DataSource = dt;
                    ddlSubject.DataTextField = "SubjectName";
                    ddlSubject.DataValueField = "SubjectID";
                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, new ListItem("Select Subject", ""));
                }
                else
                {
                    lblMsg.Text = "No subjects found.";
                    lblMsg.CssClass = "alert alert-warning";
                    lblMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void ddlStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlStudents.SelectedValue))
            {
                string studentId = ddlStudents.SelectedValue;
                LoadTimetableIDs(studentId);
            }
        }

        private void LoadTimetableIDs(string studentId)
        {
            try
            {
                string query = "SELECT ID FROM timetable WHERE StudentID = @StudentID";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@StudentID", MySqlDbType.Int32) { Value = studentId }
                };

                DataTable dt = fn.ExecuteParameterizedQueryWithResult(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlTimetableID.DataSource = dt;
                    ddlTimetableID.DataTextField = "ID";
                    ddlTimetableID.DataValueField = "ID";
                    ddlTimetableID.DataBind();
                    ddlTimetableID.Items.Insert(0, new ListItem("Select Timetable ID", ""));
                }
                else
                {
                    ddlTimetableID.Items.Clear();
                    ddlTimetableID.Items.Insert(0, new ListItem("No Timetable Found", ""));
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void ddlTimetableID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTimetableID.SelectedValue))
            {
                string timetableId = ddlTimetableID.SelectedValue;
                LoadTimetableDetails(timetableId); // Load details for the selected timetable ID
            }
        }

        private void LoadTimetableDetails(string timetableId)
        {
            try
            {
                string query = "SELECT * FROM timetable WHERE ID = @TimetableID";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@TimetableID", MySqlDbType.Int32) { Value = timetableId }
                };

                DataTable dt = fn.ExecuteParameterizedQueryWithResult(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    ddlDay.SelectedValue = row["Day"].ToString();
                    ddlSession.SelectedValue = row["Session"].ToString(); // Set selected session
                    ddlClass.SelectedValue = row["ClassID"].ToString();
                    ddlSubject.SelectedValue = row["SubjectID"].ToString();
                    ddlTeacher.SelectedValue = row["TeacherID"].ToString();
                    txtRoom.Text = row["Room"].ToString();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void btnAddTimetable_Click(object sender, EventArgs e)
        {
            try
            {
                string studentId = ddlStudents.SelectedValue;
                string day = ddlDay.SelectedValue;
                string session = ddlSession.SelectedValue; // Use ddlSession instead of txtSession
                string classId = ddlClass.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string teacherId = ddlTeacher.SelectedValue;
                string room = txtRoom.Text.Trim();

                string query = "INSERT INTO timetable (Day, Session, ClassID, SubjectID, TeacherID, Room, StudentID) " +
                               "VALUES (@Day, @Session, @ClassID, @SubjectID, @TeacherID, @Room, @StudentID)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@Day", MySqlDbType.VarChar) { Value = day },
                    new MySqlParameter("@Session", MySqlDbType.VarChar) { Value = session },
                    new MySqlParameter("@ClassID", MySqlDbType.Int32) { Value = classId },
                    new MySqlParameter("@SubjectID", MySqlDbType.Int32) { Value = subjectId },
                    new MySqlParameter("@TeacherID", MySqlDbType.Int32) { Value = teacherId },
                    new MySqlParameter("@Room", MySqlDbType.VarChar) { Value = room },
                    new MySqlParameter("@StudentID", MySqlDbType.Int32) { Value = studentId }
                };

                fn.ExecuteParameterizedQuery(query, parameters);
                lblMsg.Text = "Timetable added successfully!";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;

                LoadTimetableIDs(studentId);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void btnUpdateTimetable_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure TimetableID is selected
                if (string.IsNullOrEmpty(ddlTimetableID.SelectedValue))
                {
                    lblMsg.Text = "Please select a Timetable ID to update.";
                    lblMsg.CssClass = "alert alert-warning";
                    lblMsg.Visible = true;
                    return;
                }

                // Retrieve form values
                string timetableId = ddlTimetableID.SelectedValue;
                string studentId = ddlStudents.SelectedValue;
                string day = ddlDay.SelectedValue;
                string session = ddlSession.SelectedValue;
                string classId = ddlClass.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string teacherId = ddlTeacher.SelectedValue;
                string room = txtRoom.Text.Trim();

                // Update query
                string query = "UPDATE timetable SET Day = @Day, Session = @Session, ClassID = @ClassID, SubjectID = @SubjectID, TeacherID = @TeacherID, Room = @Room, StudentID = @StudentID " +
                               "WHERE ID = @TimetableID";
                MySqlParameter[] parameters = {
            new MySqlParameter("@Day", MySqlDbType.VarChar) { Value = day },
            new MySqlParameter("@Session", MySqlDbType.VarChar) { Value = session },
            new MySqlParameter("@ClassID", MySqlDbType.Int32) { Value = classId },
            new MySqlParameter("@SubjectID", MySqlDbType.Int32) { Value = subjectId },
            new MySqlParameter("@TeacherID", MySqlDbType.Int32) { Value = teacherId },
            new MySqlParameter("@Room", MySqlDbType.VarChar) { Value = room },
            new MySqlParameter("@StudentID", MySqlDbType.Int32) { Value = studentId },
            new MySqlParameter("@TimetableID", MySqlDbType.Int32) { Value = timetableId }
        };

                // Execute the query
                fn.ExecuteParameterizedQuery(query, parameters);
                lblMsg.Text = "Timetable updated successfully!";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;

                // Reload timetable IDs for the selected student
                LoadTimetableIDs(studentId);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void btnDeleteTimetable_Click(object sender, EventArgs e)
        {
            try
            {
                string timetableId = ddlTimetableID.SelectedValue;

                string query = "DELETE FROM timetable WHERE ID = @TimetableID";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@TimetableID", MySqlDbType.Int32) { Value = timetableId }
                };

                fn.ExecuteParameterizedQuery(query, parameters);
                lblMsg.Text = "Timetable deleted successfully!";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;

                LoadTimetableIDs(ddlStudents.SelectedValue);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }
    }
}