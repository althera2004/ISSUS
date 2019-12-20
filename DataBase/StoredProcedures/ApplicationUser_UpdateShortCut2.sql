




CREATE PROCEDURE [dbo].[ApplicationUser_UpdateShortCut2]
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
	
	DECLARE @NewId int

    DELETE FROM UserShortCuts
    WHERE
		ApplicationUserId = @ApplicationUserId
	AND CompanyId = @CompanyId
	
	SELECT @NewId = MAX(Id) FROM UserShortCuts
	SET @NewId = @NewId+1
	
	INSERT INTO UserShortCuts
	(
		Id,
		ApplicationUserId,
		CompanyId,
		ShorcutGreen,
		ShorcutBlue,
		ShortcutYellow,
		ShortcutRed
	)
	VALUES
	(
		@NewId,
		@ApplicationUserId,
		@CompanyId,
		@Green,
		@Blue,
		@Yellow,
		@Red
	)

END




