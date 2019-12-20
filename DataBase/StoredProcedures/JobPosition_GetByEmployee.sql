





CREATE PROCEDURE [dbo].[JobPosition_GetByEmployee]
	@EmployeeId int,
	@CompanyId int
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		C.Id,
		C.Description,
		C.DepartmentId
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
	ON	ECA.CargoId = C.Id
	WHERE
		ECA.EmployeeId = @EmployeeId
	AND C.CompanyId = @CompanyId
	ORDER BY
		C.Description ASC
END






