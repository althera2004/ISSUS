


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[IndicadorObjetivo_Save]
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
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		IndicadorId = @IndicadorId
	AND	ObjetivoId = @ObjetivoId
	AND CompanyId = @CompanyId

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO IndicadorObjetivo
		(
			[IndicadorId]
           ,[ObjetivoId]
           ,[CompanyId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active]
		)
		VALUES
		(
			@IndicadorId
           ,@ObjetivoId
           ,@CompanyId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1
		)

	END
END


