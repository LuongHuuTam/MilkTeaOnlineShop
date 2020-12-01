CREATE DATABASE E_Commerce_MilkTea
GO
USE E_Commerce_MilkTea
GO
CREATE TABLE User_Accounts(
	Username NCHAR(200) NOT NULL,
	[Type] int not null,
	Password NCHAR(200) NOT NULL,
	FirstName NVARCHAR(200) ,
	LastName NVARCHAR(200),
	Email NCHAR(200),
	PhoneNumber VARCHAR(200),
	[Address] NVARCHAR(200),
	Avatar_url varchar(100),
	PRIMARY KEY(Username)
)
GO
CREATE TABLE Category(
	CatId int primary key,
	CatName nvarchar(200)
)
GO
create table Size(
	SizeName char primary key
)
go

CREATE TABLE Products_detail(
	ProductId INT identity PRIMARY KEY,
	Seller NCHAR(200) REFERENCES dbo.User_Accounts(Username),
	Name NVARCHAR(200) NOT NULL,
	Imgage_url VARCHAR(250),
	Desciption NVARCHAR(500),
	CatId int references dbo.Category(CatId),
	Size char references Size(SizeName),
	Price REAL NOT NULL,
	Quantity INT DEFAULT 1,
)
GO

CREATE TABLE Carts(
	Username NCHAR(200) REFERENCES dbo.User_Accounts(Username),
	ProductId INT REFERENCES dbo.Products_detail(ProductId),
	Amount INT NOT NULL,
	Status bit,
	PRIMARY KEY(Username, ProductId)
)
GO


create table DeliveryProduct(
	Seller NCHAR(200) NOT NULL REFERENCES User_Accounts(Username),
	ProductId INT NOT NULL REFERENCES Products_detail(ProductId),
	Customer NCHAR(200) NOT NULL REFERENCES User_Accounts(Username),
	Status bit
	primary key(Seller,ProductId,Customer)
)
go 
