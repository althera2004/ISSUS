





CREATE PROCEDURE [dbo].[Department_Update]
	@DepartmentId int,
	@CompanyId int,
	@Description nvarchar(50),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Department SET
		Name = @Description,
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
		2,
		GETDATE(),
		'Name:' + @Description
    )
	
	
END






