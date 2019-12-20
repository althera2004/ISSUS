

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[IndicadorObjetivo_GetByIndicadorId]
	@IndicadorId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		IO.IndicadorId,
		IO.ObjetivoId,
		IO.CompanyId,
		IO.Active
	FROM IndicadorObjetivo IO WITH(NOLOCK)
	WHERE
		IndicadorId = @IndicadorId
	AND CompanyId = @CompanyId
	AND Active = 1

END


