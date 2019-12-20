
CREATE PROCEDURE [dbo].[Objetivo_Restore]
	@ObjetivoId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Objetivo SET
		EndDate = NULL,
		EndReason = NULL,
		ResponsibleClose = NULL,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @ObjetivoId
	AND	CompanyId = @CompanyId

	DECLARE @EmployeeId int
	SELECT @EmployeeId = EmployeeId FROM EmployeeUserAsignation WHERE UserId = @ApplicationUserId
	
	
	INSERT INTO [dbo].[ObjetivoHistorico]
           ([CompanyId]
           ,[ObjetivoId]
           ,[ActionDate]
           ,[Reason]
           ,[EmployeeId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
           ,@ObjetivoId
           ,GETDATE()
           ,'Restore'
           ,@EmployeeId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

END
