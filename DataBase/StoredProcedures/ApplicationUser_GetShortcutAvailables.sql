





CREATE PROCEDURE [dbo].[ApplicationUser_GetShortcutAvailables]
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		MS.Id,
		MS.Label,
		MS.Link,
		MS.Icon
	FROM MenuShortcuts MS WITH(NOLOCK)
	INNER JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.UserId = @UserId
	AND	CAST(AG.ItemId AS nvarchar(50)) = MS.SecurityGroupId
	INNER JOIN ApplicationItem AI WITH(NOLOCK)
	ON	AI.Id = AG.ItemId
		
	
END






