using System;
using System.Data.SqlClient;
using System.IO;

namespace DataManager
{
    public class DBApplicationInsights
    {
        private readonly string connectionString;
        public DBApplicationInsights(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddAction(string action, DateTime time)
        {
            string sqlExpression = "AddAction";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Transaction = transaction;

                try
                {
                    SqlParameter actionParam = new SqlParameter { ParameterName = "@Action", Value = action };
                    SqlParameter timeParam = new SqlParameter { ParameterName = "@Time", Value = time };
                    command.Parameters.Add(actionParam);
                    command.Parameters.Add(timeParam);

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorFile.txt"), true))
                    {
                        sw.WriteLine($"Ошибка в методе DBApplicationInsights.AddAction():{e.Message} \t {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    }
                    transaction.Rollback();
                }
            }
        }
        public void ClearAction()
        {
            string sqlExpression = "ClearAction";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Transaction = transaction;

                try
                {
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorFile.txt"), true))
                    {
                        sw.WriteLine($"Ошибка в методе DBApplicationInsights.AddAction():{e.Message} \t {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    }
                    transaction.Rollback();
                }
            }
        }
    }
}