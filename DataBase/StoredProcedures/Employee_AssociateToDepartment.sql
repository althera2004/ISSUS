





CREATE PROCEDURE [dbo].[Employee_AssociateToDepartment]
	@EmployeeId bigint,
	@DepartmentId int,
	@CompanyId int
AS
BEGIN
INSERT INTO EmployeeDepartmentMembership
	(
		EmployeeId,
		DepartmentId,
		CompanyId
	)
    VALUES
    (
		@EmployeeId,
		@DepartmentId,
		@CompanyId
	)

END






