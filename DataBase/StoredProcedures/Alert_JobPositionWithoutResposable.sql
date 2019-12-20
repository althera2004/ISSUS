





CREATE PROCEDURE [dbo].[Alert_JobPositionWithoutResposable]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		C.Id,
		C.Description,
		2
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN Cargos C2 WITH(NOLOCK)
	ON	C2.Id = C.ResponsableId
	AND C2.Active = 1
	WHERE
		C.Active = 1
	AND C.CompanyId = @CompanyId
	AND C2.Id IS NULL
END






