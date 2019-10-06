SET NOCOUNT ON
GO

USE master
GO

if exists (select * from sysdatabases where name='TestBase')
		drop database [TestBase]
GO

DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1

EXECUTE (N'CREATE DATABASE TestBase
  ON PRIMARY (NAME = N''TestBase'', FILENAME = N''' + @device_directory + N'testbase.mdf'')
  LOG ON (NAME = N''TestBase_log'',  FILENAME = N''' + @device_directory + N'testbase.ldf'')')
go

set quoted_identifier on
GO

/* Set DATEFORMAT so that the date strings are interpreted correctly regardless of
   the default DATEFORMAT on the server.
*/
SET DATEFORMAT mdy
GO


if exists (select * from sysobjects where id = object_id('dbo.Users') and sysstat & 0xf = 3)
	drop table [dbo].[Users]
GO

USE [TestBase]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](60) NOT NULL,
	[LastName] [nvarchar](60) NOT NULL,
	[Age] [int] NOT NULL,
 CONSTRAINT [PK_UsersT] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

set quoted_identifier on
GO

ALTER TABLE [dbo].[Users] NOCHECK CONSTRAINT ALL
GO

INSERT [dbo].[Users] VALUES('845cfd9c-d773-400f-a6d8-73d26065fddd','Maria', 'Anders', 57)
GO

INSERT [dbo].[Users] VALUES('f1512f3a-5874-4f0d-8c6c-22dd25ab6ca0','Ana', 'Trujillo', 45)
GO

INSERT [dbo].[Users] VALUES('3b0e5fe7-1cd3-436f-8f1d-75eb052ef709','Antonio', 'Moreno', 32)
GO

INSERT [dbo].[Users] VALUES('9473b38d-42e3-4e21-a610-d39f4887a61f','Thomas', 'Hardy', 50)
GO

INSERT [dbo].[Users] VALUES('a716cd30-e144-4a63-ae3b-37828e79aeaf','Christina', 'Berglund',34)
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT ALL
GO

set quoted_identifier on
GO
