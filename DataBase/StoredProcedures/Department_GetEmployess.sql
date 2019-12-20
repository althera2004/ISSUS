





CREATE PROCEDURE [dbo].[Department_GetEmployess]
	@DepartmentId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		C.Id,
		C.Description,
		C2.Id,
		C2.Description,
		E.Id,
		E.Name,
		E.LastName,
		ISNULL(E.NIF,''),
		ISNULL(E.Email,''),
		ISNULL(E.Phone,'')
	FROM Cargos C WITH(NOLOCK)
	LEFT JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
		LEFT JOIN Employee E WITH(NOLOCK)
		ON  ECA.EmployeeId = E.Id
		AND E.Active = 1
		AND ECA.FechaBaja IS NULL
		AND E.FechaBaja IS NULL
	ON	ECA.CargoId = C.Id
	AND C.Active = 1
	LEFT JOIN Cargos C2 WITH(NOLOCK)
	ON	C2.Id = C.ResponsableId
	WHERE
		C.CompanyId = @CompanyId
	AND C.DepartmentId = @DepartmentId
	AND C.Active = 1
END






