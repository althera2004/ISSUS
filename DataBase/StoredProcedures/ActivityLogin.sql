





CREATE PROCEDURE [dbo].[ActivityLogin]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
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
		7,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END






