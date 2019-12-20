


CREATE PROCEDURE [dbo].[Auditory_Close]
	@AuditoryId bigint,
	@CompanyId int,
	@ClosedBy int,
	@ClosedOn datetime,
	@QuestionaryStart datetime,
	@QuestionaryEnd datetime,
	@Notes nvarchar(2000),
	@PuntosFuertes nvarchar(2000),
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
		[Status] = 4,
		[ReportStart] = @QuestionaryStart,
		[ReportEnd] = @Questionaryend,
		[ClosedBy] = @ClosedBy,
		[ClosedOn] = @ClosedOn,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @AuditoryId
	AND CompanyId = @CompanyId
END

