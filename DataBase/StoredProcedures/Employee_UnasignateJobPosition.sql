





CREATE PROCEDURE [dbo].[Employee_UnasignateJobPosition]
	@EmployeeId bigint,
	@JobPositionId bigint,
	@Date datetime,
	@CompanyId int,
	@UserId int
AS
BEGIN

	UPDATE EmployeeCargoAsignation SET
		FechaBaja = @Date
	WHERE
		EmployeeId = @EmployeeId
	AND CargoId = @JobPositionId
	AND CompanyId = @CompanyId
	AND FechaBaja IS NULL
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		[DateTime],
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		8,
		@JobPositionId,
		8,
		GETDATE(),
		'JobPoisitionId:' + CAST(@JobPositionId AS NVARCHAR)
	)
END






