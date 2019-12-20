





CREATE PROCEDURE [dbo].[Alert_JobPositionWithoutEmployees]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		C.Id,
		C.Description,
		1
	FROM Cargos C WITH(NOLOCK)
	LEFT JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
	ON	ECA.CargoId = C.Id
	WHERE
		C.Active = 1
	AND C.CompanyId = @CompanyId
	AND ECA.EmployeeId IS NULL
END






