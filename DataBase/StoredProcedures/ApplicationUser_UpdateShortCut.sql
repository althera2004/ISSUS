





CREATE PROCEDURE [dbo].[ApplicationUser_UpdateShortCut]
	@ApplicationUserId int,
	@CompanyId int,
	@Green int,
	@Blue int,
	@Yellow int,
	@Red int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM UserShortCuts
    WHERE
		ApplicationUserId = @ApplicationUserId
	AND CompanyId = @CompanyId
	
	INSERT INTO UserShortCuts
	(
		ApplicationUserId,
		CompanyId,
		ShorcutGreen,
		ShorcutBlue,
		ShortcutYellow,
		ShortcutRed
	)
	VALUES
	(
		@ApplicationUserId,
		@CompanyId,
		@Green,
		@Blue,
		@Yellow,
		@Red
	)
END






