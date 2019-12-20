





CREATE PROCEDURE [dbo].[DocumentProcedencia_Update]
	@DocumentProcedenciaId int,
	@CompanyId int,
	@Description nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Procedencia SET
		Description = @Description
	WHERE
		Id = @DocumentProcedenciaId
	AND CompanyId = @CompanyId
END






