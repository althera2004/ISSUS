
CREATE PROCEDURE [dbo].[Objetivo_Anulate]
	@ObjetivoId int,
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
	UPDATE Objetivo SET
		EndDate = @EndDate,
		EndReason = @EndReason,
		ResponsibleClose = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @ObjetivoId
	AND	CompanyId = @CompanyId

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
           ,@EndDate
           ,@EndReason
           ,@EndResponsible
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

END
