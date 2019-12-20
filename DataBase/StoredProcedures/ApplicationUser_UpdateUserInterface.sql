






CREATE PROCEDURE [dbo].[ApplicationUser_UpdateUserInterface]
	@ApplicationUserId int,
	@CompanyId int,
	@Language nvarchar(2),
	@ShowHelp bit
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE ApplicationUser SET
		ShowHelp = @ShowHelp,
		Language = @Language
	WHERE
		Id = @ApplicationUserId
	AND CompanyId = @CompanyId
END






