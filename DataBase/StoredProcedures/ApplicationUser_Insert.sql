




CREATE PROCEDURE [dbo].[ApplicationUser_Insert]
	@Id int output,
	@CompanyId int,
	@Login nvarchar(50),
	@Email nvarchar(50),
	@Language nvarchar(50),
	@Password nvarchar(50),
	@Admin bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[ApplicationUser]
           ([CompanyId]
           ,[Login]
           ,[Password]
		   ,[Email]
		   ,[Admin]
           ,[Status]
           ,[LoginFailed]
           ,[MustResetPassword]
           ,[Language]
           ,[ShowHelp]
           ,[Avatar])
     VALUES
           (@CompanyId,
           @Login
           ,@Password
		   ,@Email
		   ,@Admin
           ,1
           ,0
           ,1
		   ,@Language
           ,0
           ,'avatar2.png')

		   SET @Id = @@IDENTITY

	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,1,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,2,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,3,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,4,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,5,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,6,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,7,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,8,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,9,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,10,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,11,0,0,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,12,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,13,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,14,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,15,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,16,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [Appli