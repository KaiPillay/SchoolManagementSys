using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

namespace SchoolManagementSystem.Teacher
{
    public partial class TeacherChat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure only teachers are allowed to access this page
            if (Session["UserID"] == null || Session["Role"] == null || Session["Role"].ToString() != "Teacher")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        // Fetch teacher's name based on logged-in UserID
        [System.Web.Services.WebMethod]
        public static string GetTeacherName()
        {
            string teacherName = string.Empty;

            try
            {
                int userId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

                string query = @"
                    SELECT t.Name 
                    FROM teacher t
                    INNER JOIN users u ON u.Id = t.UserId
                    WHERE u.Id = @UserID AND u.Role = 'Teacher'";

                DataTable dt = FetchData(query, new MySqlParameter("@UserID", userId));

                if (dt.Rows.Count > 0)
                {
                    teacherName = dt.Rows[0]["Name"].ToString();
                }
                else
                {
                    teacherName = "Teacher not found or invalid role";
                }
            }
            catch (Exception ex)
            {
                teacherName = "Error: " + ex.Message;
            }

            return teacherName;
        }

        // Fetch all users with their roles for populating the dropdown
        [System.Web.Services.WebMethod]
        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();

            try
            {
                string query = "SELECT Id, Username AS Name, Role FROM users WHERE Role IN ('Teacher', 'Student', 'Parent')";

                DataTable dt = FetchData(query);

                foreach (DataRow row in dt.Rows)
                {
                    users.Add(new User
                    {
                        UserId = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString(),
                        Role = row["Role"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching users: " + ex.Message);
            }

            return users;
        }

        // Save message to the database
        [System.Web.Services.WebMethod]
        public static void SaveMessage(int receiverId, string message)
        {
            try
            {
                // Retrieve the senderId from the session (Teacher's ID)
                int senderId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

                // Assuming chatRoomId is static or needs to be dynamic depending on the conversation
                int chatRoomId = 1; // Replace this with dynamic logic if needed

                string query = @"
                    INSERT INTO messages (SenderId, ReceiverId, ChatRoomId, MessageText, Timestamp, IsRead)
                    VALUES (@SenderId, @ReceiverId, @ChatRoomId, @Message, NOW(), 0)";

                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@SenderId", senderId);
                        cmd.Parameters.AddWithValue("@ReceiverId", receiverId);
                        cmd.Parameters.AddWithValue("@ChatRoomId", chatRoomId);
                        cmd.Parameters.AddWithValue("@Message", message);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving message: " + ex.Message);
            }
        }

        // Fetch data from the database (e.g., users or chat messages)
        private static DataTable FetchData(string query, params MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddRange(parameters);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }

    // User class to represent users in the dropdown
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
