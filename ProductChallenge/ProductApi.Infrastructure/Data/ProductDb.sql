-- Create database
CREATE DATABASE ProductDb;
GO

-- Use database
USE ProductDb;
GO

-- Create Products table
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Status INT NOT NULL,
    Stock INT NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Price DECIMAL(18,2) NOT NULL
);
GO
