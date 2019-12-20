
create PROCEDURE [dbo].[IndicadorObjetivo_Delete]
	@ObjetivoId int,
	@IndicadorId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE IndicadorObjetivo SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		IndicadorId = @IndicadorId
	AND	ObjetivoId = @ObjetivoId
	AND CompanyId = @CompanyId

END


