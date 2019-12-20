





CREATE PROCEDURE [dbo].[Employee_GetJobPositionHistoric]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		ECA.CompanyId,
		ECA.CargoId,
		C.Description AS CargoDescription,
		D.Id AS DepartmentId,
		D.Name AS DepartmentName,
		E.Id,
		ISNULL(E.NIF,'') AS Nif,
		ISNULL(E.Name,'') AS EmployeeName,
		ISNULL(E.LastName,'') AS EmployeeLastName,
		ECA.FechaAlta,
		ECA.FechaBaja,
		C.Active,
		C2.Id,
		C2.Description
    FROM EmployeeCargoAsignation ECA WITH(NOLOCK)
    INNER JOIN Employee E WITH(NOLOCK)
    ON	E.Id = ECA.EmployeeId
    AND E.CompanyId = ECA.CompanyId
    INNER JOIN Cargos C WITH(NOLOCK)
		LEFT JOIN Cargos C2 WITH(NOLOCK)
		ON	C2.Id = C.ResponsableId
		AND C2.Active = 1
    ON	C.CompanyId = ECA.CompanyId
    AND C.Id = ECA.CargoId
    AND C.Active = 1
    INNER JOIN Department D WITH(NOLOCK)
    ON	D.CompanyId = C.CompanyId
    AND D.Id = C.DepartmentId
    WHERE
		ECA.EmployeeId = @EmployeeId
	AND	ECA.CompanyId = @CompanyId
	ORDER BY
		ECA.FechaAlta DESC
END






