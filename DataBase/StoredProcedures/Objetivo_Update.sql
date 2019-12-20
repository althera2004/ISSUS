



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Update]
	@ObjetivoId int,
	@Name nvarchar(100),
	@Description nvarchar(2000),
	@ResponsibleId int,
	@StartDate datetime,
	@VinculatedToIndicator bit,
	@IndicatorId int,
	@RevisionId int,
	@Methodology nvarchar(2000),
	@Resources nvarchar(2000),
	@Notes nvarchar(2000),
	@PreviewEndDate datetime,
	@EndDate datetime,
	@ResponsibleClose int,
	@CompanyId int,
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE Objetivo SET
		Name = @Name,
		[Description] = @Description,
		ResponsibleId = @ResponsibleId,
		StartDate = @StartDate,
		Methodology = @Methodology,
		Resources = @Resources,
		Notes = @Notes,
		PreviewEndDate = @PreviewEndDate,
		--EndDate = @EndDate,
		VinculatedToIndicator = @VinculatedToIndicator,
		IndicatorId = @IndicatorId,
		RevisionId = @RevisionId,
		--ResponsibleClose = @ResponsibleClose,
		MetaComparer = @MetaComparer,
		Meta = @Meta,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ObjetivoId
	AND CompanyId  = @CompanyId

	DELETE FROM IndicadorObjetivo WHERe ObjetivoId = @ObjetivoId

	IF @IndicatorId IS NOT NULL
	BEGIN
		INSERT INTO IndicadorObjetivo (IndicadorId, ObjetivoId, CompanyId, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Active)
		VALUES
		(@IndicatorId, @ObjetivoId, @CompanyId, @ApplicationUserId, GETDATE(), @ApplicationUserId, GETDATE(), 1)
	END

END




