





CREATE PROCEDURE [dbo].[Employee_Disable]
	@EmployeeId bigint,
	@CompanyId int,
	@UserId int,
	@FechaBaja date
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Employee SET
		FechaBaja = @FechaBaja,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id =@EmployeeId
	AND CompanyId = @CompanyId
	
	UPDATE EmployeeCargoAsignation SET
		FechaBaja = @FechaBaja
	WHERE
		EmployeeId = @EmployeeId
	AND CompanyId = @CompanyId
	
	DECLARE @EmployeeUserId int
	SELECT @EmployeeUserId = EUA.UserId
	FROM EmployeeUserAsignation EUA WITH(NOLOCK)
	WHERE	EUA.EmployeeId = @EmployeeId
	AND		EUA.CompanyId = @CompanyId
	
	UPDATE ApplicationUser SET
		Status = 0
	WHERE
		Id = @EmployeeUserId

	DELETE FROM EmployeeUserAsignation
	WHERE	EmployeeId = @EmployeeId
	
	/*INSERT INTO ActivityLog
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
		9,
		GETDATE(),
		''
    )*/
END






