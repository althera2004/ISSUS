





CREATE PROCEDURE [dbo].[ApplicationUser_ChangeUserName]
	@ApplicationUserId int,
	@CompanyId int,
	@UserName nvarchar(50),
	@extraData nvarchar(200),
	@UserId int,
	@EmployeeId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE ApplicationUser SET
		Login = UPPER(REPLACE(RTRIM(LTRIM(@UserName)),' ',''))
	WHERE
		Id = @ApplicationUserId
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
		4,
		@ApplicationUserId,
		9,
		GETDATE(),
		@extraData
    )

	DECLARE @exists int;
	SELECT @exists = COUNT(*) FROM EmployeeUserAsignation WHERE UserId = @ApplicationUserId;
	IF @exists = 0
	BEGIN
		INSERT INTO EmployeeUserAsignation
		(
			[UserId],
			[EmployeeId],
			[CompanyId]
		)
		VALUES
		(
			@ApplicationUserId,
			@EmployeeId,
			@CompanyId
		)
	END
	ELSE
	BEGIN
		UPDATE EmployeeUserAsignation SET
			EmployeeId = @EmployeeId
		WHERE
			UserId = @ApplicationUserId
		AND	CompanyId = @CompanyId
	END
	
END






