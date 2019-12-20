





CREATE PROCEDURE [dbo].[Rules_Insert]
	@Id bigint out,
	@CompanyId int,
	@Description nvarchar(50),
	@Notes nvarchar(500),
	@Limit int,
	@UserId int
AS
BEGIN
	INSERT INTO Rules
	(
		CompanyId,
		Description,
        Notes,
        Limit,
        CreatedBy,
        CreatedOn,
        ModifiedBy,
        ModifiedOn,
        Active
	)
    VALUES
	(
		@CompanyId,
        @Description,
        @Notes,
        @Limit,
        @UserId,
        GETDATE(),
        @UserId,
        GETDATE(),
        1
	)
	SET @Id = @@IDENTITY

	INSERT INTO [RulesHistory]
           ([RuleId]
		   ,[IPR]
           ,[Reason]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@Id
		   ,@Limit
           ,'Init'
           ,@UserId
           ,GETDATE()
           ,@UserId
           ,GETDATE()
           ,1)
END







