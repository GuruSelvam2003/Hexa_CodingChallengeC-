using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Loan_Management_System
{
    public class LoanRepositoryImpl : LoanRepository
    {
        public void ApplyLoan(Loan loan)
        {
            Console.WriteLine("Want Loan? (Yes/No)");
            string confirmation = Console.ReadLine();
            if(confirmation!="Yes")
            {
                Console.WriteLine("Loan cancelled.");
                return ;
            }
            using(SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "insert into Loan(Customer_ID,Principal_Amount,interestRate,Loan_Term,Loan_Type,Loan_Status) values (@customerID,@PrincipalAmount,@InterestRate,@LoanTerm,@LoanType,@LoanStatus)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CustomerID", loan.Customer.CustomerId);
                cmd.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                cmd.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
                cmd.Parameters.AddWithValue("@LoanStatus", "Pending");
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Applied successfully");
                    if(loan.LoanType=="HomeLoan")
                    {
                        Console.Write("Enter Property Address: ");
                        string propertyAddress = Console.ReadLine();

                        Console.Write("Enter Property Value: ");
                        int propertyValue = int.Parse(Console.ReadLine());

                        Console.Write("Enter LoanID: ");
                        int ln_id = int.Parse(Console.ReadLine());

                        string query1 = "insert into HomeLoan (Loan_ID,Property_Address,Property_Value) values(@LoanId,@propertyAddress,@propertyValue)";
                        SqlCommand cmd1 = new SqlCommand(query1, connection);
                        cmd1.Parameters.AddWithValue("@LoanId", ln_id);
                        cmd1.Parameters.AddWithValue("@propertyAddress", propertyAddress);
                        cmd1.Parameters.AddWithValue("@propertyValue", propertyValue);
                        int rowsAffected1 = cmd1.ExecuteNonQuery();
                        if(rowsAffected1>0)
                        {
                            Console.WriteLine("Successful");
                        }

                    }
                    if (loan.LoanType == "CarLoan")
                    {
                        Console.Write("Enter Model: ");
                        string Model = Console.ReadLine();

                        Console.Write("Enter Car  Value: ");
                        int Value = int.Parse(Console.ReadLine());

                        Console.Write("Enter LoanID: ");
                        int ln_id = int.Parse(Console.ReadLine());

                        string query1 = "insert into CarLoan (Loan_ID,Car_Model,Car_Value) values(@LoanId,@Model,@Value)";
                        SqlCommand cmd1 = new SqlCommand(query1, connection);
                        cmd1.Parameters.AddWithValue("@LoanId", ln_id);
                        cmd1.Parameters.AddWithValue("@Model", Model);
                        cmd1.Parameters.AddWithValue("@Value", Value);
                        int rowsAffected1 = cmd1.ExecuteNonQuery();
                        if (rowsAffected1 > 0)
                        {
                            Console.WriteLine("Successful");
                        }
                    }
                }

                else
                {
                    Console.WriteLine("Failed");
                }

            }
        }

        public decimal CalculateInterest(int loanId)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "select Principal_Amount,interestRate, Loan_Term from Loan where Loan_ID=@LoanID";
                SqlCommand cmd = new SqlCommand(query,connection);
                cmd.Parameters.AddWithValue("@LoanID", loanId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal principal = (decimal)reader["Principal_Amount"];
                        decimal rate = (decimal)reader["interestRate"];
                        int term = (int)reader["Loan_Term"];

                        decimal interest = (principal * rate * term) / 12;
                        return interest;
                    }
                    else
                    {
                        throw new InvalidLoanException("Loan ID not found.");
                    }
                }
            }
        }

        public decimal CalculateInterest(decimal principal, decimal rate, int term)
        {
            return (principal * rate * term) / 12;
        }

        public void LoanStatus(int loanId)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "select c.Credit_Score from Customer c join Loan l on c.Customer_ID=l.Customer_ID where l.Loan_ID=@LoanID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@LoanID", loanId);
                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    throw new InvalidLoanException("Loan ID not found.");
                }
                int creditScore = Convert.ToInt32(result);
                string status = creditScore >= 650 ? "Approved" : "Rejected";

                string updateQuery = "update Loan SET Loan_Status = @Status where Loan_ID = @LoanID";
                SqlCommand updateCmd = new SqlCommand(updateQuery, connection);
                updateCmd.Parameters.AddWithValue("@Status", status);
                updateCmd.Parameters.AddWithValue("@LoanID", loanId);
                updateCmd.ExecuteNonQuery();

                Console.WriteLine($"Status: {status}");
            }
        }

        public decimal CalculateEMI(int loanId)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "select Principal_Amount, interestRate, Loan_Term from Loan where Loan_ID = @LoanID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@LoanID", loanId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal P = (decimal)reader["Principal_Amount"];
                        decimal annualRate = (decimal)reader["interestRate"];
                        int N = (int)reader["Loan_Term"];

                        decimal R = annualRate / 12 / 100;
                        decimal EMI = (P * R * (decimal)Math.Pow((double)(1 + R), N)) / ((decimal)Math.Pow((double)(1 + R), N) - 1);
                        return EMI;
                    }
                    else
                    {
                        throw new InvalidLoanException("Loan ID not found.");
                    }
                }
            }
        }

        public decimal CalculateEMI(decimal principal, decimal rate, int term)
        {
            double r = (double)rate / 12 / 100;
            double emi = (double)principal * r * Math.Pow(1 + r, term) / (Math.Pow(1 + r, term) - 1);
            return (decimal)emi;
        }

        public void LoanRepayment(int loanId, decimal amount)
        {
            decimal emi = CalculateEMI(loanId);

            if (amount < emi)
            {
                Console.WriteLine("Amount less than EMI");
                return;
            }

            int numberOfEmis = (int)(amount / emi);
            Console.WriteLine($"Number of EMIs covered is {numberOfEmis}");
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string getQuery = "SELECT Principal_Amount FROM Loan WHERE Loan_ID = @LoanID";
                SqlCommand getCmd = new SqlCommand(getQuery, connection);
                getCmd.Parameters.AddWithValue("@LoanID", loanId);
                decimal remainingBalance = (decimal)getCmd.ExecuteScalar();
                decimal totalPaid = emi * numberOfEmis;
                decimal newRemainingBalance = remainingBalance - totalPaid;
                string updateQuery = "UPDATE Loan SET Principal_Amount = @NewRemainingBalance WHERE Loan_ID = @LoanID";
                SqlCommand updateCmd = new SqlCommand(updateQuery, connection);
                updateCmd.Parameters.AddWithValue("@NewRemainingBalance", newRemainingBalance);
                updateCmd.Parameters.AddWithValue("@LoanID", loanId);
                updateCmd.ExecuteNonQuery();
                Console.WriteLine($"Updated Remaining Balance: {newRemainingBalance}");
            }

        }

        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "select * from Loan";
                SqlCommand cmd = new SqlCommand(query, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Loan loan = new Loan
                        {
                            LoanId = (int)reader["Loan_ID"],
                            PrincipalAmount = (decimal)reader["Principal_Amount"],
                            InterestRate = (decimal)reader["interestRate"],
                            LoanTerm = (int)reader["Loan_Term"],
                            LoanType = reader["Loan_Type"].ToString(),
                            LoanStatus = reader["Loan_Status"].ToString()
                        };
                        loans.Add(loan);
                    }
                }
            }

            return loans;

        }

        public Loan GetLoanById(int loanId)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "select * from Loan where Loan_ID = @LoanID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@LoanID", loanId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Loan loan =  new Loan
                        {
                            LoanId = (int)reader["Loan_ID"],
                            PrincipalAmount = (decimal)reader["Principal_Amount"],
                            InterestRate = (decimal)reader["interestRate"],
                            LoanTerm = (int)reader["Loan_Term"],
                            LoanType = reader["Loan_Type"].ToString(),
                            LoanStatus = reader["Loan_Status"].ToString(),
                            Customer = new Customer { CustomerId = (int)reader["Customer_ID"] }
                        };
                        return loan;
                    }
                    else
                    {
                        throw new InvalidLoanException("Loan ID not found.");
                    }
                }
            }
        }
    }
}
