





CREATE PROCEDURE [dbo].[Employee_GetByCompanyWithUser]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,		
		E.Active,
		AU.Id AS UserId,
		AU.Login
	FROM Employee E WITH(NOLOCK)
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
	ON  E.Id = EUA.EmployeeId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EUA.UserId
	WHERE
		E.CompanyId = @CompanyId
	
END






