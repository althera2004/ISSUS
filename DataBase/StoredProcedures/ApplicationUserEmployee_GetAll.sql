

CREATE PROCEDURE [dbo].[ApplicationUserEmployee_GetAll]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		UserId,
		EmployeeId
	FROM EmployeeUserAsignation EUA WITH(NOLOCK)
	WHERE
		CompanyId = @CompanyId

END
