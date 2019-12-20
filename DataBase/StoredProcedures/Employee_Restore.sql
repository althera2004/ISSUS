





CREATE PROCEDURE [dbo].[Employee_Restore]
	@EmployeeId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Employee SET
		FechaBaja = NULL,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id =@EmployeeId
	AND CompanyId = @CompanyId
	
	/*DECLARE @EmployeeUserId int
	SELECT @EmployeeUserId = EUA.UserId
	FROM EmployeeUserAsignation EUA WITH(NOLOCK)
	WHERE	EUA.EmployeeId = @EmployeeId
	AND		EUA.CompanyId = @CompanyId
	
	UPDATE ApplicationUser SET
		Status = 1
	WHERE
		Id = @EmployeeUserId*/
	
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
		10,
		GETDATE(),
		''
    )
END






