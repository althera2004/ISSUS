




CREATE PROCEDURE [dbo].[ApplicationUser_GetEffectiveGrants]
	@ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		AI.Id,
		AI.Description,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) AS GrantToRead,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToWrite, 0) END AS BIT) AS GrantToWrite,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToDelete, 0) END AS BIT) AS GrantToDelete,
		AI.UrlList
    FROM ApplicationItem AI WITH(NOLOCK)
    LEFT JOIN ApplicationGrant AG
    ON AG.ItemId = Ai.Id
    AND AG.UserId = @ApplicationUserId
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = @ApplicationUserId
    
    WHERE
	    AI.Container = 0    
	OR	AU.Admin = 1
    
    
    ORDER BY Ai.Id
END

