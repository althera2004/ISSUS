





CREATE PROCEDURE [dbo].[ApplicationUser_ChangePassword]
	@UserId int,
	@OldPassword nvarchar(50),
	@NewPassword nvarchar(50),
	@CompanyId int,
	@Result int out
AS
BEGIN
	SELECT
	*
	FROM ApplicationUser AU
	WHERE
		AU.Id = @UserId
	AND AU.Password = @OldPassword
	AND AU.CompanyId = @CompanyId
	
	IF @@ROWCOUNT = 0 
	BEGIN
		SET @Result = 0
	END
	ELSE
	BEGIN
		SET @Result = 1
		UPDATE ApplicationUser SET
			Password = @NewPassword,
			MustResetPassword = 0
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
END






