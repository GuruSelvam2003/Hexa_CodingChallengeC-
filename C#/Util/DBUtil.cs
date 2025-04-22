using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan_Management_System
{
    public class DBUtil
    {
        private static string connectionString = "Server=DESKTOP-MLFAPL8\\SQLEXPRESS;Database=Loan_Management_System;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetDBConn()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                //Console.WriteLine("Database connection successful");
                return conn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database connection failed: " + ex.Message);
                throw;
            }
        }
    }

    
}
