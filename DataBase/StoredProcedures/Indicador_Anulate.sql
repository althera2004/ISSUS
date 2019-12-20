
CREATE PROCEDURE [dbo].[Indicador_Anulate]
	@IndicadorId int,
	@CompanyId int,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsible int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Indicador SET
		EndDate = @EndDate,
		EndReason = @EndReason,
		EndResponsible = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IndicadorId
	AND	CompanyId = @CompanyId

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
           ,@EndDate
           ,@EndReason
           ,@EndResponsible
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

END




