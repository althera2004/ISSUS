





CREATE PROCEDURE [dbo].[JobPosition_GetEmployees]
	@JobPositionId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		E.Id,
		E.Name,
		ISNULL(E.LastName,'') AS LastName,
		E.NIF,
		E.Email,
		E.Phone
	FROM Employee E WITH(NOLOCK)
	INNER JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
	ON	ECA.CargoId = @JobPositionId
	AND ECA.EmployeeId = E.Id
	AND ECA.CompanyId = E.CompanyId
	AND ECA.FechaBaja IS NULL
	WHERE
		E.Active = 1
	AND E.FechaBaja IS NULL
END






