CREATE TABLE [dbo].[Account]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Email] NCHAR(20) NULL, 
    [Mobile] NCHAR(10) NULL, 
    [Nric] NVARCHAR(MAX) NULL, 
    [PasswordHash] NVARCHAR(MAX) NULL, 
    [PasswordSalt] NVARCHAR(MAX) NULL, 
    [DateTimeRegistered] DATETIME NULL, 
    [MobileVerified] NCHAR(2) NULL, 
    [EmailVerified] NCHAR(2) NULL
)
