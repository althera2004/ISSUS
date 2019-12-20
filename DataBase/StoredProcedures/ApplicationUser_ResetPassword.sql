





CREATE PROCEDURE [dbo].[ApplicationUser_ResetPassword]
	@UserId int,
	@CompanyId int,
	@Result int out,
	@UserName nvarchar(50) out,
	@Password nvarchar(50) out,
	@Email nvarchar(50) out
AS
BEGIN

	SELECT *
	FROM ApplicationUser A WITH(NOLOCK)
	WHERE
		A.Id = @UserId
	AND A.CompanyId = @CompanyId
	
	IF @@ROWCOUNT = 0 
	BEGIN
		SET @Result = 0
	END
	ELSE
	BEGIN
		SELECT @Password = [dbo].GeneratePassword(6)
		SET @Result = 1
		UPDATE ApplicationUser SET
			[Password] = @Password,
			MustResetPassword = 1
		WHERE
			Id = @UserId
		AND CompanyId = @CompanyId

		SELECT
			@UserName = [Login],
			@Password = [Password],
			@Email = Email
		FROM ApplicationUser WITH(NOLOCK)
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






