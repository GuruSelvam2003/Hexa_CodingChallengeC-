using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan_Management_System
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int CreditScore { get; set; }

        public Customer() { }

        public Customer(int customerId, string name, string email, string phone, string address, int creditScore)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            CreditScore = creditScore;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"CustomerID: {CustomerId}, Name: {Name}, Email: {Email}, Phone: {Phone}, Address: {Address}, CreditScore: {CreditScore}");
        }
    }
}
