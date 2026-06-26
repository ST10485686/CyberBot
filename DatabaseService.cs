using System;
using System.Data;
using System.Data.SqlClient;

namespace CyberBotGUI
{
    public class DatabaseService
    {
        private string connectionString;

        public DatabaseService(string connectionString = null)
        {
            // Default connection string - Windows Authentication
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=localhost;Database=CyberBotDB;Integrated Security=True;";
            }
            this.connectionString = connectionString;
        }

        // ========== TASK OPERATIONS ==========

        public bool AddTask(string userName, string title, string description = "",
                            string reminder = "", DateTime? reminderDate = null)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Tasks 
                                    (UserName, Title, Description, Reminder, ReminderDate) 
                                    VALUES (@UserName, @Title, @Description, @Reminder, @ReminderDate)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description ?? "");
                        cmd.Parameters.AddWithValue("@Reminder", reminder ?? "");
                        cmd.Parameters.AddWithValue("@ReminderDate", (object)reminderDate ?? DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return false;
            }
        }

        public DataTable GetTasks(string userName, bool showCompleted = true)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT Id, Title, Description, Reminder, ReminderDate, 
                                    IsCompleted, CreatedAt FROM Tasks WHERE UserName = @UserName";

                    if (!showCompleted)
                        query += " AND IsCompleted = 0";

                    query += " ORDER BY IsCompleted ASC, CreatedAt DESC";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return new DataTable();
            }
        }

        public bool UpdateTaskStatus(int taskId, bool isCompleted)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE Id = @TaskId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IsCompleted", isCompleted);
                        cmd.Parameters.AddWithValue("@TaskId", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Tasks WHERE Id = @TaskId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return false;
            }
        }

        public bool UpdateTaskReminder(int taskId, string reminder, DateTime? reminderDate)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Tasks SET Reminder = @Reminder, ReminderDate = @ReminderDate WHERE Id = @TaskId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Reminder", reminder);
                        cmd.Parameters.AddWithValue("@ReminderDate", (object)reminderDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TaskId", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return false;
            }
        }

        // ========== ACTIVITY LOG OPERATIONS ==========

        public bool AddActivityLog(string userName, string action, string details = "")
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO ActivityLog (UserName, Action, Details) VALUES (@UserName, @Action, @Details)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@Action", action);
                        cmd.Parameters.AddWithValue("@Details", details ?? "");
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return false;
            }
        }

        public DataTable GetActivityLog(string userName, int limit = 10)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT TOP (@Limit) Action, Details, CreatedAt 
                                   FROM ActivityLog 
                                   WHERE UserName = @UserName 
                                   ORDER BY CreatedAt DESC";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@Limit", limit);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return new DataTable();
            }
        }

        // ========== QUIZ SCORE OPERATIONS ==========

        public bool SaveQuizScore(string userName, int score, int totalQuestions, double percentage)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO QuizScores 
                                    (UserName, Score, TotalQuestions, Percentage) 
                                    VALUES (@UserName, @Score, @TotalQuestions, @Percentage)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@Score", score);
                        cmd.Parameters.AddWithValue("@TotalQuestions", totalQuestions);
                        cmd.Parameters.AddWithValue("@Percentage", percentage);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return false;
            }
        }

        public DataTable GetQuizScores(string userName, int limit = 5)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT TOP (@Limit) Score, TotalQuestions, Percentage, CreatedAt 
                                   FROM QuizScores 
                                   WHERE UserName = @UserName 
                                   ORDER BY CreatedAt DESC";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@Limit", limit);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return new DataTable();
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connection Error: " + ex.Message);
                return false;
            }
        }
    }
}