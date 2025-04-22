using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan_Management_System
{
    public interface LoanRepository
    {
        void ApplyLoan(Loan loan);
        decimal CalculateInterest(int loanId);
        decimal CalculateInterest(decimal principal, decimal rate, int term);
        void LoanStatus(int loanId);
        decimal CalculateEMI(int loanId);
        decimal CalculateEMI(decimal principal, decimal rate, int term);
        void LoanRepayment(int loanId, decimal amount);
        List<Loan> GetAllLoans();
        Loan GetLoanById(int loanId);
    }
}
