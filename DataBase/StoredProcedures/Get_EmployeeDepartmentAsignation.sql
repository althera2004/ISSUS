





CREATE PROCEDURE [dbo].[Get_EmployeeDepartmentAsignation] 
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		EMD.EmployeeId,
		EMD.DepartmentId
	FROM EmployeeDepartmentMembership EMD
	WHERE
		EMD.CompanyId = @CompanyId
END






