





CREATE PROCEDURE [dbo].[DocumentProcedencia_Delete]
	@DocumentProcedenciaId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM Procedencia 
	WHERE
		Id = @DocumentProcedenciaId
	AND CompanyId = @CompanyId
END






