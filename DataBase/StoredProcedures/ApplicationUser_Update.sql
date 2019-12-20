




CREATE PROCEDURE [dbo].[ApplicationUser_Update]
	@ApplicationUserId int,
	@UserName nvarchar(50),
	@Email nvarchar(50),
	@Language nvarchar(50),
	@Admin bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ApplicationUser SET
		[Login] = @UserName,
		[Email] = @Email,
		[Language] = @Language,
		[Admin] = @Admin
	WHERE
		Id = @ApplicationUserId
END





