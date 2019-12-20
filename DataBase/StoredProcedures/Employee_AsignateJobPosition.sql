





CREATE PROCEDURE [dbo].[Employee_AsignateJobPosition]
	@EmployeeId bigint,
	@JobPositionId bigint,
	@Date datetime,
	@CompanyId int,
	@UserId int
AS
BEGIN

	
		INSERT INTO EmployeeCargoAsignation
		(
			EmployeeId,
			CargoId,
			CompanyId,
			FechaAlta,
			FechaBaja
		)
		VALUES
		(
			@EmployeeId,
			@JobPositionId,
			@CompanyId,
			@Date,
			NULL
		)

		INSERT INTO ActivityLog
		(
			ActivityId,
			UserId,
			CompanyId,
			TargetType,
			TargetId,
			ActionId,
			DateTime,
			ExtraData
		)
		VALUES
		(
			NEWID(),
			@UserId,
			@CompanyId,
			8,
			@EmployeeId,
			6,
			GETDATE(),
			'JobPoisitionId:' + CAST(@JobPositionId AS NVARCHAR)
		)
END






