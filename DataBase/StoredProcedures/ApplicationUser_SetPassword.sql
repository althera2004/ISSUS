





CREATE PROCEDURE [dbo].[ApplicationUser_SetPassword]
	@UserId int,
	@Password nvarchar(50)
AS
BEGIN

	UPDATE ApplicationUser SET
		[Password] = @Password,
		[MustResetPassword] = 0
	WHERE
		Id = @UserId
END

