





CREATE PROCEDURE [dbo].[ApplicationUser_ChangeAvatar]
	@UserId int,
	@Avatar nvarchar(50),
	@CompanyId int
AS
BEGIN
	UPDATE ApplicationUser SET
		Avatar = @Avatar
	WHERE
		Id = @UserId
	AND CompanyId = @CompanyId
	
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
		2,
		@UserId,
		4,
		GETDATE(),
		''
	)
END






