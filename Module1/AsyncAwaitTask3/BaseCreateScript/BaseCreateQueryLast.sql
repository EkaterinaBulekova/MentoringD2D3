/*
** Copyright Microsoft, Inc. 1994 - 2000
** All Rights Reserved.
*/

SET NOCOUNT ON
GO

USE master
GO
if exists (select * from sysdatabases where name='ShopBase')
		drop database ShopBase
go

DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1

EXECUTE (N'CREATE DATABASE ShopBase
  ON PRIMARY (NAME = N''ShopBase'', FILENAME = N''' + @device_directory + N'shopbase.mdf'')
  LOG ON (NAME = N''ShopBase_log'',  FILENAME = N''' + @device_directory + N'shopbase.ldf'')')
go


set quoted_identifier on
GO

/* Set DATEFORMAT so that the date strings are interpreted correctly regardless of
   the default DATEFORMAT on the server.
*/
SET DATEFORMAT mdy
GO
use "ShopBase"
go

if exists (select * from sysobjects where id = object_id('dbo.Products') and sysstat & 0xf = 3)
	drop table "dbo"."Products"
GO
if exists (select * from sysobjects where id = object_id('dbo.Categories') and sysstat & 0xf = 3)
	drop table "dbo"."Categories"
GO

if exists (select * from sysobjects where id = object_id('dbo.Carts') and sysstat & 0xf = 3)
	drop table "dbo"."Carts"
GO

if exists (select * from sysobjects where id = object_id('dbo.Orders') and sysstat & 0xf = 3)
	drop table "dbo"."Orders"
GO
if exists (select * from sysobjects where id = object_id('dbo.OrderDetails') and sysstat & 0xf = 3)
	drop table "dbo"."OrderDetails"
GO


CREATE TABLE "Categories" (
	"Id" "int" IDENTITY (1, 1) NOT NULL ,
	"Name" nvarchar (15) NOT NULL ,
	CONSTRAINT "PK_Categories" PRIMARY KEY  CLUSTERED 
	(
		"Id"
	)
)
GO
 CREATE  INDEX "CategoryName" ON "dbo"."Categories"("Name")
GO

CREATE TABLE "Products" (
	"Id" "int" IDENTITY (1, 1) NOT NULL ,
	"Name" nvarchar (40) NOT NULL ,
	"CategoryId" int NULL ,
	"Price" decimal NOT NULL DEFAULT (0),
	CONSTRAINT "PK_Products" PRIMARY KEY  CLUSTERED 
	(
		"Id"
	),
	CONSTRAINT "FK_Products_Categories" FOREIGN KEY 
	(
		"CategoryId"
	) REFERENCES "dbo"."Categories" (
		"Id"
	))
	GO

	CREATE TABLE "Carts" (
	"Id" "int" IDENTITY (1, 1) NOT NULL ,
	"CartId" nvarchar (40) NOT NULL ,
	"ProductId" int NOT NULL ,
	"Count" int NOT NULL DEFAULT (0),
	CONSTRAINT "PK_Carts" PRIMARY KEY  CLUSTERED 
	(
		"Id"
	),
	CONSTRAINT "FK_Carts_Products" FOREIGN KEY 
	(
		"ProductId"
	) REFERENCES "dbo"."Products" (
		"Id"
	))
	GO

	CREATE TABLE "Orders" (
	"Id" "int" IDENTITY (1, 1) NOT NULL ,
	"OrderDate" datetime NOT NULL default (Getdate()),
	"Name"  nvarchar (40) NOT NULL ,
	"Phone" nvarchar (40) NOT NULL ,
	"Address" nvarchar (60) NULL ,
	"City" nvarchar (15) NULL ,
	"State" nvarchar (15) NULL ,
	"Zip" nvarchar (10) NULL ,
	CONSTRAINT "PK_Orders" PRIMARY KEY  CLUSTERED 
	(
		"Id"
	)
	)
	GO

	CREATE TABLE "OrderDetails" (
	"Id" "int" IDENTITY (1, 1) NOT NULL ,
	"OrderId" int NOT NULL ,
	"ProductName" nvarchar (40) NOT NULL ,
	"UnitPrice" decimal NOT NULL ,
	"Count" int NOT NULL DEFAULT (0),
	CONSTRAINT "PK_OrderDetails" PRIMARY KEY  CLUSTERED 
	(
		"Id"
	),
	CONSTRAINT "FK_OrderDetails_Orders" FOREIGN KEY 
	(
		"OrderId"
	) REFERENCES "dbo"."Orders" (
		"Id"
	))
	GO

set quoted_identifier on
go
set identity_insert "Categories" on
go
ALTER TABLE "Categories" NOCHECK CONSTRAINT ALL
go
INSERT "Categories"("Id","Name") VALUES(1,'Beverages')
INSERT "Categories"("Id","Name") VALUES(2,'Condiments')
INSERT "Categories"("Id","Name") VALUES(3,'Confections')
INSERT "Categories"("Id","Name") VALUES(4,'Dairy Products')
INSERT "Categories"("Id","Name") VALUES(5,'Grains/Cereals')
INSERT "Categories"("Id","Name") VALUES(6,'Meat/Poultry')
INSERT "Categories"("Id","Name") VALUES(7,'Produce')
INSERT "Categories"("Id","Name") VALUES(8,'Seafood')

set identity_insert "Categories" off
go
ALTER TABLE "Categories" CHECK CONSTRAINT ALL
go

set quoted_identifier on
go
set identity_insert "Products" on
go
ALTER TABLE "Products" NOCHECK CONSTRAINT ALL
go
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(1,'Chai',1,18)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(2,'Chang',1,19)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(3,'Aniseed Syrup',2,10)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(4,'Chef Anton''s Cajun Seasoning',2,22)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(5,'Chef Anton''s Gumbo Mix',2,21)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(6,'Grandma''s Boysenberry Spread',2,25)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(7,'Uncle Bob''s Organic Dried Pears',7,30)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(8,'Northwoods Cranberry Sauce',2,40)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(9,'Mishi Kobe Niku',6,97)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(10,'Ikura',8,31)
go										
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(11,'Queso Cabrales',4,21)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(12,'Queso Manchego La Pastora',4,38)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(13,'Konbu',8,6)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(14,'Tofu',7,23)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(15,'Genen Shouyu',2,15)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(16,'Pavlova',3,17)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(17,'Alice Mutton',6,39)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(18,'Carnarvon Tigers',8,62)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(19,'Teatime Chocolate Biscuits',3,10)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(20,'Sir Rodney''s Marmalade',3,81)
go										
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(21,'Sir Rodney''s Scones',3,10)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(22,'Gustaf''s Knäckebröd',5,21)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(23,'Tunnbröd',5,9)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(24,'Guaraná Fantástica',1,5)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(25,'NuNuCa Nuß-Nougat-Creme',3,14)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(26,'Gumbär Gummibärchen',3,31)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(27,'Schoggi Schokolade',3,44)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(28,'Rössle Sauerkraut',7,46)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(29,'Thüringer Rostbratwurst',6,123)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(30,'Nord-Ost Matjeshering',8,25)
go										
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(31,'Gorgonzola Telino',4,12)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(32,'Mascarpone Fabioli',4,32)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(33,'Geitost',4,3)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(34,'Sasquatch Ale',1,14)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(35,'Steeleye Stout',1,18)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(36,'Inlagd Sill',8,19)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(37,'Gravad lax',8,26)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(38,'Côte de Blaye',1,263)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(39,'Chartreuse verte',1,18)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(40,'Boston Crab Meat',8,18)
go										
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(41,'Jack''s New England Clam Chowder',8,10)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(42,'Singaporean Hokkien Fried Mee',5,14)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(43,'Ipoh Coffee',1,46)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(44,'Gula Malacca',2,20)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(45,'Rogede sild',8,9)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(46,'Spegesild',8,13)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(47,'Zaanse koeken',3,10)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(48,'Chocolade',3,13)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(49,'Maxilaku',3,20)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(50,'Valkoinen suklaa',3,16)
go										
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(51,'Manjimup Dried Apples',7,53)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(52,'Filo Mix',5,7)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(53,'Perth Pasties',6,32)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(54,'Tourtière',6,8)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(55,'Pâté chinois',6,24)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(56,'Gnocchi di nonna Alice',5,38)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(57,'Ravioli Angelo',5,19)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(58,'Escargots de Bourgogne',8,13)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(59,'Raclette Courdavault',4,55)
INSERT "Products"("Id","Name","CategoryId","Price") VALUES(60,'Camembert Pierrot',4,34)
go
set identity_insert "Products" off
go
ALTER TABLE "Products" CHECK CONSTRAINT ALL
go