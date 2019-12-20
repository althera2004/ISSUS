
CREATE PROCEDURE [dbo].[Indicador_Restore]
	@IndicadorId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE INdicador SET
		EndDate = NULL,
		EndReason = NULL,
		EndResponsible = NULL,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IndicadorId
	AND	CompanyId = @CompanyId

	DECLARE @EmployeeId int
	SELECT @EmployeeId = EmployeeId FROM EmployeeUserAsignation WHERE UserId = @ApplicationUserId
	
	
	INSERT INTO [dbo].[IndicadorHistorico]
           ([CompanyId]
           ,[IndicadorId]
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
           ,@IndicadorId
           ,GETDATE()
           ,'Restore'
           ,@EmployeeId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

END
