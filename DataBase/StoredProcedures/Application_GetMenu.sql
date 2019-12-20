





CREATE PROCEDURE [dbo].[Application_GetMenu]
	@ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

	/*SELECT
		AI.Id AS ItemId,
		AI.Description,
		AI.Icon,
		AI.UrlList,
		AI.Parent,
		AI.Container,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) END AS GrantToRead,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) END AS GrantToWrite,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) END AS GrantToDelete,
		Au.Admin
	FROM ApplicationItem AI WITH(NOLOCK)
	LEFT JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.ItemId = AI.Id
	AND	AG.UserId = @ApplicationUserId
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	 AU.Id = @ApplicationUserId

	WHERE
	(
		AI.Id IS NOT NULL
		OR
		AI.Container = 1
	)
	AND AI.Parent <> -1
	AND
	(
		AI.Container = 1
		OR
		AG.GrantToRead IS NOT NULL
	)
	
	ORDER BY Parent ASC, [Order] ASC*/

	SELECT
		AI.Id AS ItemId,
		AI.Description,
		AI.Icon,
		AI.UrlList,
		AI.Parent,
		AI.Container,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead,0) END AS BIT) END AS GrantToRead,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToWrite,0) END AS BIT) END AS GrantToWrite,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToDelete,0) END AS BIT) END AS GrantToDelete
		,AU.Admin
	FROM ApplicationItem AI WITH(NOLOCK)
	LEFT JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.ItemId = AI.Id
	AND	AG.UserId = @ApplicationUserId
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = @ApplicationUserId

	WHERE
	/*(
		--AG.UserId = @ApplicationUserId
		--OR
		AI.Container = 1
	)
	AND*/ AI.Parent <> -1
	AND
	(
		AI.Container = 1
		OR
		ISNULL(AG.GrantToRead,0) = 1
		OR
		AU.Admin = 1
	)
	
	ORDER BY Parent ASC, [Order] ASC
END





