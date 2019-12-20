





CREATE PROCEDURE [dbo].[Alert_Department]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		D.Id,
		D.Name
	FROM Department D WITH(NOLOCK)
	LEFT JOIN EmployeeDepartmentMembership EDM WITH(NOLOCK)
	ON EDM.DepartmentId = D.Id
	WHERE
		D.CompanyId = @CompanyId
	AND EDM.DepartmentId IS NULL
	AND D.Deleted = 0
END






