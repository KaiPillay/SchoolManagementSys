using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace SchoolManagementSystem.Models
{
    public class CommonFn
    {
        public class Commonfnx
        {
            private readonly string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            /// <summary>
            /// Executes a non-query SQL command (e.g., INSERT, UPDATE, DELETE).
            /// </summary>
            /// <param name="query">The SQL query to execute.</param>
            public void ExecuteQuery(string query)
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
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Database error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            /// <summary>
            /// Fetches data from the database and returns it as a DataTable.
            /// </summary>
            /// <param name="query">The SQL query to execute.</param>
            /// <returns>A DataTable containing the query results.</returns>
            public DataTable Fetch(string query)
            {
                DataTable dt = new DataTable();

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                            {
                                sda.Fill(dt);
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Database error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                return dt;
            }

            /// <summary>
            /// Tests the connection to the database.
            /// </summary>
            public void TestConnection()
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        Console.WriteLine("Database connection successful.");
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Database connection failed: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            /// <summary>
            /// Prepares a parameterised query to avoid SQL injection.
            /// </summary>
            /// <param name="query">The SQL query with placeholders for parameters.</param>
            /// <param name="parameters">An array of MySqlParameter objects.</param>
            public void ExecuteParameterizedQuery(string query, MySqlParameter[] parameters)
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Parameterized query executed successfully.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Database error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
