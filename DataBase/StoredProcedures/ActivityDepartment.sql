





CREATE PROCEDURE [dbo].[ActivityDepartment]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(200)
AS
BEGIN
	SET NOCOUNT ON;
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
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END






