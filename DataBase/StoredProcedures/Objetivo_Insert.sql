



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Insert]
	@ObjetivoId int output,
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

INSERT INTO [dbo].[Objetivo]
           ([Name]
           ,[Description]
           ,[ResponsibleId]
           ,[StartDate]
           ,[VinculatedToIndicator]
           ,[IndicatorId]
           ,[RevisionId]
           ,[Methodology]
           ,[Resources]
           ,[Notes]
           ,[PreviewEndDate]
           ,[EndDate]
           ,[ResponsibleClose]
           ,[CompanyId]
		   ,[MetaComparer]
		   ,[Meta]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
	 (
		@Name,
		@Description,
		@ResponsibleId,
		@StartDate,
		@VinculatedToIndicator,
		@IndicatorId,
		@RevisionId,
		@Methodology,
		@Resources,
		@Notes,
		@PreviewEndDate,
		@EndDate,
		@ResponsibleClose,
		@CompanyId,
		@MetaComparer,
		@Meta,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	 )

	 SET @ObjetivoId = @@IDENTITY

	 

	IF @IndicatorId IS NOT NULL
	BEGIN
		INSERT INTO IndicadorObjetivo (IndicadorId, ObjetivoId, CompanyId, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Active)
		VALUES
		(@IndicatorId, @ObjetivoId, @CompanyId, @ApplicationUserId, GETDATE(), @ApplicationUserId, GETDATE(), 1)
	END

END




