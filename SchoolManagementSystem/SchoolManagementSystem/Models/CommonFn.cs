using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem.Models
{
    public class CommonFn
    {
        public class Commonfnx
        {
            private readonly string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            public void Query(string query)
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Query executed successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Query execution failed: {ex.Message}");
                    }
                }
            }

            public DataTable Fetch(string query)
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connString))
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                            {
                                sda.Fill(dt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                return dt;
            }
        }
}
}