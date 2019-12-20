





CREATE PROCEDURE [dbo].[Company_DepartmentMemberShip]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		EDM.DepartmentId,
		EDm.EmployeeId
    FROM EmployeeDepartmentMembership EDM WITH(NOLOCK)
    INNER JOIN Employee E WITH(NOLOCK)
    ON	E.Id = EDM.EmployeeId
    AND E.CompanyId = EDM.CompanyId
    AND E.Active = 1
    INNER JOIN Department D WITH(NOLOCK)
    ON	D.Id = EDM.DepartmentId
    AND D.CompanyId = EDM.CompanyId
    AND D.Deleted = 0
    WHERE
		EDM.CompanyId = @CompanyId
END






