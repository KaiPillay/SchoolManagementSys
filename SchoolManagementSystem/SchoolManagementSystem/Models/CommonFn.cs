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

            internal DataTable Fetch()
            {
                throw new NotImplementedException();
            }

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

            internal DataTable Fetch(string query, MySqlParameter[] parameters)
            {
                throw new NotImplementedException();
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

                        Console.WriteLine($"Fetched {dt.Rows.Count} rows.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        // Optionally log or display the error message
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

            public DataTable ExecuteParameterizedQueryWithResult(string query, MySqlParameter[] parameters)
            {
                DataTable dt = new DataTable();

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddRange(parameters);
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
