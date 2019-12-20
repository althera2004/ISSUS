





CREATE PROCEDURE [dbo].[Log_Login]
	@UserName nvarchar(50),
	@Ip nvarchar(50),
	@Result int,
	@UserId int,
	@CompanyCode nvarchar(10),
	@CompanyId int
AS
BEGIN
	INSERT INTO Logins
           ([Id]
           ,[UserName]
           ,[Date]
           ,[IP]
           ,[Result]
           ,[UserId]
           ,[CompanyCode]
           ,[CompanyId])
     VALUES
           (NEWID()
           ,@UserName
           ,GETDATE()
           ,@Ip
           ,@Result
           ,@UserId
           ,@CompanyCode
           ,@CompanyId)
END






