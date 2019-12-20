





CREATE PROCEDURE [dbo].[Employee_Delete]
	@EmployeeId bigint,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;	
	
	UPDATE Employee SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id =@EmployeeId
	AND CompanyId = @CompanyId
	
	UPDATE EmployeeCargoAsignation SET
		FechaBaja = GETDATE()
	WHERE
		EmployeeId = @EmployeeId
	AND CompanyId = @CompanyId

	DELETE FROM EmployeeUserAsignation
	WHERE
		EmployeeId = @EmployeeId
	AND CompanyId = @CompanyId
	
END






