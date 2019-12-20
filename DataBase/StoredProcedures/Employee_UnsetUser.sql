





CREATE PROCEDURE [dbo].[Employee_UnsetUser]
	@UserId bigint,
	@CompanyId int
AS
BEGIN

	DELETE FROM EmployeeUserAsignation
	WHERE
		UserId = @UserId
	AND CompanyId = @CompanyId

END






