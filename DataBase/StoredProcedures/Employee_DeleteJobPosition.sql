





CREATE PROCEDURE [dbo].[Employee_DeleteJobPosition]
	@EmployeeId bigint,
	@JobPositionId bigint
AS
BEGIN
	
		DELETE FROM EmployeeCargoAsignation
		WHERE
			EmployeeId = @EmployeeId
		AND	CargoId = @JobPositionId
END






