
CREATE PROCEDURE [dbo].[ApplicationUser_GetGrants]
	@ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT
	@ApplicationUserId as UserId,
	AI.Id,
	CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead,0) END AS Bit) AS GrantToRead,
	CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToWrite,0) END AS Bit) AS GrantToWrite,
	CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToWrite,0) END AS Bit) AS GrantToWrite,
	AI.UrlList,
	AI.Description
	FROM ApplicationItem AI WITH(NOLOCK)
	LEFT JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AI.Id = AG.ItemId
	AND  AG.UserId = @ApplicationUserId
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)

	ON	AU.Id = @ApplicationUserId
	


END
