



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_Update]
	@IndicadorId int,
	@CompanyId int,
	@Descripcion nvarchar(150),
	@ResponsableId int,
	@ProcessId int,
	--@ObjetivoId int,
	@Calculo nvarchar(2000),
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@AlarmaComparer nvarchar(10),
	@Alarma decimal(18,6),
	@Periodicity int,
	@StartDate datetime,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsable int,
	@UnidadId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Indicador SET
		Descripcion = @Descripcion,
		Meta = @Meta,
		MetaComparer = @MetaComparer,
		Alarma = @Alarma,
		AlarmaComparer = @AlarmaComparer,
		Calculo = @Calculo,
		StartDate = @StartDate,
		EndDate = @EndDate,
		EndReason = @EndReason,
		EndResponsible = @EndResponsable,
		ResponsableId = @ResponsableId,
		ProcessId = @ProcessId,
		--ObjetivoId = @ObjetivoId,
		UnidadId = @UnidadId,
		Periodicity = @Periodicity,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IndicadorId
	AND	CompanyId = @CompanyId

END




