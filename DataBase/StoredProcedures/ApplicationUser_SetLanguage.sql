




CREATE PROCEDURE [dbo].[ApplicationUser_SetLanguage]
	@ApplicationUserId int,
	@Language nvarchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ApplicationUser SET
		[Language] = @Language
	WHERE
		Id = @ApplicationUserId
END





