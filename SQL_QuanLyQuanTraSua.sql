CREATE DATABASE E_Commerce_MilkTea
GO

USE E_Commerce_MilkTea
GO

CREATE TABLE User_Accounts(
	Username NCHAR(200) NOT NULL,
	[Type] INT NOT NULL,
	Password NCHAR(200) NOT NULL,
	FirstName NVARCHAR(200) ,
	LastName NVARCHAR(200),
	Email NCHAR(200),
	PhoneNumber VARCHAR(200),
	[Address] NVARCHAR(200),
	Avatar_url VARCHAR(100),
	PRIMARY KEY(Username)
)
GO

CREATE TABLE Category(
	CatId INT PRIMARY KEY,
	CatName NVARCHAR(200)
)
GO

CREATE TABLE Size(
	SizeName CHAR PRIMARY KEY
)
go

CREATE TABLE Products_Detail(
	ProductId INT identity PRIMARY KEY,
	Seller NCHAR(200) REFERENCES dbo.User_Accounts(Username),
	Name NVARCHAR(200) NOT NULL,
	Imgage_url VARCHAR(250),
	Desciption NVARCHAR(500),
	CatId INT REFERENCES dbo.Category(CatId),
	Size CHAR REFERENCES Size(SizeName),
	Price REAL NOT NULL,
	Quantity INT DEFAULT 1,
)
GO

CREATE TABLE Carts(
	CartID INT IDENTITY PRIMARY KEY,
	Username NCHAR(200) REFERENCES dbo.User_Accounts(Username),
	ProductId INT REFERENCES dbo.Products_Detail(ProductId),
	Amount INT NOT NULL
)
GO

create table Ship_Method(
	ShipId INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100),
	Fee REAL NOT NULL,
)
GO

create table Orders_Detail(
	OrderDetailId INT IDENTITY PRIMARY KEY,
	OrderDate DATETIME,
	ReceiveDate DATETIME,
	Note NTEXT,
	TotalAmount INT,
	TotalMoney REAL,
	ShipId INT REFERENCES dbo.Ship_Method(ShipId),
	Customer NCHAR(200) NOT NULL REFERENCES dbo.User_Accounts(Username),
)
GO

create table Orders(
	OrderId INT IDENTITY PRIMARY KEY,
	ProductId INT NOT NULL REFERENCES dbo.Products_Detail(ProductId),
	Amount INT NOT NULL,
	[Status] TINYINT NOT NULL DEFAULT 0,
	Orders_Detail INT NOT NULL REFERENCES dbo.Orders_Detail(OrderDetailId)
)
GO
