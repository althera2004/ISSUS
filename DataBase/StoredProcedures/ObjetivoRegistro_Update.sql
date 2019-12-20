



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObjetivoRegistro_Update]
	@ObjetivoRegistroId int output,
	@CompanyId int,
	@ObjetivoId int,
	@Fecha datetime,
	@Valor decimal (18,6),
	@MetaComparer nvarchar(10),
	@Meta decimal (18,6),
	@Comentari nvarchar(500),
	@ResponsibleId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].ObjetivoRegistro SET 
       [ObjetivoId] = @ObjetivoId
      ,[Date] = @Fecha
      ,[Value] = @Valor
	  ,[MetaComparer] = @MetaComparer
	  ,[Meta] = @Meta
      ,[Comments] = @Comentari
      ,[ResponsibleId] = @ResponsibleId
      ,[ModifiedBy] = @ApplicationUserId
      ,[ModifiedOn] = GETDATE()
	WHERE
		Id = @ObjetivoRegistroId
	AND CompanyId = @CompanyId



END




