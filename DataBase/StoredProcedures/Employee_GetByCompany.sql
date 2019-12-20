





CREATE PROCEDURE [dbo].[Employee_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,
		ISNULL(E.Email,'') AS Email,
		ISNULL(E.Phone,'') AS Phone,
		D.Id AS DepartmentId,
		AU.Id AS UserId,
		ISNULL(D.Name,'') AS DepartmentName,
		ISNULL(E.NIF ,'') AS Nif,
		ISNULL(E.Address,'') AS Address,
		ISNULL(E.PostalCode,'') AS PostalCode,
		ISNULL(E.City,'') AS City,
		ISNULL(E.Province,'') AS Province,
		ISNULL(E.Country,'') AS Country,
		ISNULL(E.Notes,'') AS Notes,
		E.CompanyId AS CompanyId,
		AU.Id AS ModifiedByUserId,
		0 AS ModifiedByEmployeeId,
		AU.[Login] AS ModifiedName,
		'' AS ModifiedByLastName,
		E.ModifiedOn,
		C.Id AS CargoId,
		C.Description AS CargoDescription,
		E.FechaBaja,
		E.Active
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
		LEFT JOIN Cargos C WITH(NOLOCK)
			LEFT JOIN Department D WITH (NOLOCK)
			ON	D.CompanyId = C.CompanyId
			AND	D.Id = C.DepartmentId
			AND D.Deleted = 0
		ON	C.Id = ECA.CargoId
		AND C.CompanyId = ECA.CompanyId
		AND C.Active = 1
	ON  E.Id = ECA.EmployeeId
	AND	E.CompanyId = ECA.CompanyId
	AND ECA.FechaBaja IS NULL
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = E.ModifiedBy
	WHERE
		E.CompanyId = @CompanyId
		--AND E.FechaBaja IS NULL
	ORDER BY D.Name ASC
	
END






