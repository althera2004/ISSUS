





CREATE PROCEDURE [dbo].[Employee_DesassociateToDepartment]
	@EmployeeId bigint,
	@DepartmentId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM EmployeeDepartmentMembership
    WHERE
		CompanyId = @CompanyId
	AND DepartmentId = @DepartmentId
	AND EmployeeId = @EmployeeId
END






