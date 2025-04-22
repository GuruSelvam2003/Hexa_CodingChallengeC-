using Loan_Management_System;
LoanRepositoryImpl repo = new LoanRepositoryImpl();

bool r = true;

while(r)
{
    Console.WriteLine("1. Apply Loan");
    Console.WriteLine("2. Get All Loans");
    Console.WriteLine("3. Get Loan by ID");
    Console.WriteLine("4. Repay Loan");
    Console.WriteLine("5. Calculate Interest");
    Console.WriteLine("6. Get Loan Status");
    Console.WriteLine("7. Calculate EMI");
    Console.WriteLine("8. Exit");
    Console.Write("Enter choice: ");

    string choice = Console.ReadLine();
    

    switch(choice)
    {
        case "1":
            try
            {
                Console.Write("Enter Customer ID: ");
                int custId = int.Parse(Console.ReadLine());

                Console.Write("Enter Principal Amount: ");
                decimal principal = decimal.Parse(Console.ReadLine());

                Console.Write("Enter Interest Rate: ");
                decimal rate = decimal.Parse(Console.ReadLine());

                Console.Write("Enter Loan Term (in months): ");
                int term = int.Parse(Console.ReadLine());

                Console.Write("Enter Loan Type (CarLoan/HomeLoan): ");
                string type = Console.ReadLine();

                Customer customer = new Customer { CustomerId = custId };
                Loan loan = new Loan(0,customer, principal, rate, term, type, "Pending");
                repo.ApplyLoan(loan);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            break;

        case "2":
            List<Loan> loans = repo.GetAllLoans();
            foreach (var loan in loans)
            {
                Console.WriteLine($"\nLoan ID: {loan.LoanId}");
                Console.WriteLine($"Amount: {loan.PrincipalAmount}");
                Console.WriteLine($"Rate: {loan.InterestRate}");
                Console.WriteLine($"Term: {loan.LoanTerm} months");
                Console.WriteLine($"Type: {loan.LoanType}");
                Console.WriteLine($"Status: {loan.LoanStatus}");
            }
            break;

        case "3":
            try
            {
                Console.Write("Enter Loan ID: ");
                int loanId = int.Parse(Console.ReadLine());

                Loan loan = repo.GetLoanById(loanId);
                Console.WriteLine($"\nLoan ID: {loan.LoanId}");
                Console.WriteLine($"Customer ID: {loan.Customer.CustomerId}");
                Console.WriteLine($"Amount: {loan.PrincipalAmount}");
                Console.WriteLine($"Rate: {loan.InterestRate}");
                Console.WriteLine($"Term: {loan.LoanTerm} months");
                Console.WriteLine($"Type: {loan.LoanType}");
                Console.WriteLine($"Status: {loan.LoanStatus}");
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            break;

        case "4":
            try
            {
                Console.Write("Enter Loan ID: ");
                int loanId = int.Parse(Console.ReadLine());

                Console.Write("Enter Repayment Amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                repo.LoanRepayment(loanId, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            break;

        case "5":
            {
                try
                {
                    Console.Write("Enter Loan ID: ");
                    int loanId = int.Parse(Console.ReadLine());
                    decimal interest;
                    if(loanId==0)
                    {
                        Console.Write("Enter Principal: ");
                        decimal principal = decimal.Parse(Console.ReadLine());
                        Console.Write("Enter Rate: ");
                        decimal rate = decimal.Parse(Console.ReadLine());
                        Console.Write("Enter Term: ");
                        int term = int.Parse(Console.ReadLine());

                        interest = repo.CalculateInterest(principal,rate,term);
                    }
                    else
                    {
                        interest = repo.CalculateInterest(loanId);
                    }
                        
                    Console.WriteLine($"Calculated Interest: {interest}");
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                break;
            }

        case "6":
            {
                try
                {
                    Console.Write("Enter Loan ID: ");
                    int loanId = int.Parse(Console.ReadLine());

                    repo.LoanStatus(loanId); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                break;
            }

        case "7":
            {
                try
                {
                    Console.Write("Enter Loan ID: ");
                    int loanId = int.Parse(Console.ReadLine());
                    decimal emi;
                    emi = repo.CalculateEMI(loanId);
                    Console.WriteLine($"Calculated EMI: {emi}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                break;
            }


        case "8":
            r = false;
            Console.WriteLine("Exiting");
            break;

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;


    }
}