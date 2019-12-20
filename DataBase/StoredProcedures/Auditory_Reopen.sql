


CREATE PROCEDURE [dbo].[Auditory_Reopen]
	@AuditoryId bigint,
	@CompanyId int,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[Auditory] SET
		[Status] = 3,
		[ClosedBy] = NULL,
		[ClosedOn] = NULL,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @AuditoryId
	AND CompanyId = @CompanyId
END

