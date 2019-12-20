





CREATE PROCEDURE [dbo].[Department_Delete]
	@DepartmentId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Department SET
		Deleted = 1,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @DepartmentId
	AND	CompanyId = @CompanyId
	
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
		5,
		@DepartmentId,
		3,
		GETDATE(),
		@Reason
    )
	
	
END






