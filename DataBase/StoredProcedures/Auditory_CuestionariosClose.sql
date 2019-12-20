


CREATE PROCEDURE [dbo].[Auditory_CuestionariosClose]
	@AuditoryId bigint,
	@CompanyId int,
	@QuestionaryStart datetime,
	@QuestionaryEnd datetime,
	@PuntosFuertes nvarchar(2000),
	@Notes nvarchar(2000),
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[Auditory] SET
		[Notes] = @Notes,
		[PuntosFuertes] = @PuntosFuertes,
		[Status] = 3,
		[ReportStart] = @QuestionaryStart,
		[ReportEnd] = @Questionaryend,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @AuditoryId
	AND CompanyId = @CompanyId
END

