create database Loan_Management_System

create table Customer(
Customer_ID int primary key identity,
Name varchar(200),
Email varchar(400),
Phone varchar(50),
Address varchar(max),
Credit_Score int)

create table Loan(
Loan_ID int primary key identity,
Customer_ID int,
Principal_Amount money,
interestRate decimal(5,2),
Loan_Term int,
Loan_Type varchar(100),
Loan_Status varchar(100)
constraint fk_CID foreign key (Customer_ID) references Customer(Customer_ID))

INSERT INTO Customer (Name, Email, Phone, Address, Credit_Score) VALUES
('Ramesh Kumar', 'ramesh.kumar@example.com', '9876543210', 'Chennai, TN', 720),
('Suresh Iyer', 'suresh.iyer@example.com', '9845123456', 'Bangalore, KA', 680),
('Meena Patel', 'meena.patel@example.com', '9765432109', 'Ahmedabad, GJ', 600),
('Karan Singh', 'karan.singh@example.com', '9123456780', 'Delhi', 750),
('Priya Sharma', 'priya.sharma@example.com', '9988776655', 'Mumbai, MH', 800),
('Arun Nair', 'arun.nair@example.com', '9012345678', 'Kochi, KL', 620),
('Divya Reddy', 'divya.reddy@example.com', '9034567891', 'Hyderabad, TS', 700),
('Rajesh Rao', 'rajesh.rao@example.com', '9944556677', 'Pune, MH', 690),
('Sneha Das', 'sneha.das@example.com', '9777665544', 'Kolkata, WB', 710),
('Anil Verma', 'anil.verma@example.com', '9865321478', 'Lucknow, UP', 640);


INSERT INTO Loan (Customer_ID, Principal_Amount, interestRate, Loan_Term, Loan_Type, Loan_Status) VALUES
(1, 500000, 7.5, 60, 'HomeLoan', 'Pending'),
(2, 300000, 9.2, 48, 'CarLoan', 'Pending'),
(3, 400000, 8.5, 36, 'HomeLoan', 'Pending'),
(4, 250000, 10.0, 24, 'CarLoan', 'Pending'),
(5, 1000000, 6.8, 120, 'HomeLoan', 'Pending'),
(6, 200000, 9.0, 36, 'CarLoan', 'Pending'),
(7, 350000, 8.0, 60, 'HomeLoan', 'Pending'),
(8, 150000, 10.5, 24, 'CarLoan', 'Pending'),
(9, 750000, 7.2, 96, 'HomeLoan', 'Pending'),
(10, 180000, 9.5, 36, 'CarLoan', 'Pending');



create table HomeLoan (
    Loan_ID INT PRIMARY KEY ,
    Property_Address VARCHAR(MAX),
    Property_Value INT,
    CONSTRAINT FK_LID FOREIGN KEY (Loan_ID) REFERENCES Loan(Loan_ID)
);

create table CarLoan (
    Loan_ID INT PRIMARY KEY ,
    Car_Model VARCHAR(255),
    Car_Value INT,
    CONSTRAINT FK_CLID FOREIGN KEY (Loan_ID) REFERENCES Loan(Loan_ID)
);

select * from Customer
select * from Loan
select * from HomeLoan
select * from CarLoan