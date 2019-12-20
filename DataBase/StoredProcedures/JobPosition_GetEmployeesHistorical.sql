





CREATE PROCEDURE [dbo].[JobPosition_GetEmployeesHistorical]
	@JobPositionId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		COUNT(E.Id)
	FROM Employee E WITH(NOLOCK)
	INNER JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
	ON	ECA.CargoId = @JobPositionId
	AND ECA.EmployeeId = E.Id
	AND ECA.CompanyId = E.CompanyId
	--AND ECA.FechaBaja IS NULL
	WHERE
		E.Active = 1
END






