





CREATE PROCEDURE [dbo].[DocumentProcedencia_Insert]
	@DocumentProcedenciaId int out,
	@CompanyId int,
	@Description nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO Procedencia
	(
		CompanyId,
        Description,
        Editable
    )
    VALUES    
    (
		@CompanyId,
		@Description,
		1
    ) 
    
    SET @DocumentProcedenciaId = @@IDENTITY
END






