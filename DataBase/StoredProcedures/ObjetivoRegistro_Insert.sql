



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObjetivoRegistro_Insert]
	@ObjetivoRegistroId int output,
	@CompanyId int,
	@ObjetivoId int,
	@Fecha datetime,
	@Valor decimal (18,6),
	@Meta decimal (18,6),
	@MetaComparer nvarchar(10),
	@Comentari nvarchar(500),
	@ResponsibleId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].ObjetivoRegistro
	(
		[CompanyId],
		[ObjetivoId],
		[Date],
		[Value],
		[MetaComparer],
		[Meta],
		[Comments],
		[ResponsibleId],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@ObjetivoId,
		@Fecha,
		@Valor,
		@MetaComparer,
		@Meta,
		@Comentari,
		@ResponsibleId,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	)

	SET @ObjetivoRegistroId = @@IDENTITY



END




