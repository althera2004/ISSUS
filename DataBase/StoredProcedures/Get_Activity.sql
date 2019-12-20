





CREATE PROCEDURE [dbo].[Get_Activity]
	@CompanyId int,
	@TargetType int,
	@ItemId int,
	@From date,
	@To date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		AL.ActivityId,
		AL.DateTime,
		AL.UserId,
		AL.CompanyId,
		AT.Description AS Target,
		AA.Description AS Action,
		ISNULL(AL.ExtraData,'') AS ExtraData,
		AU.[Login] AS Employee,
		AL.TargetId AS TargetId
	FROM ActivityLog AL WITH(NOLOCK)
	INNER JOIN ActivityTarget AT WITH(NOLOCK)
	ON	AT.ActivityTarget = AL.TargetType
	INNER JOIN ActivityAction AA WITH(NOLOCK)
	ON	AA.ActivityTarget = AL.TargetType
	AND	AA.ActivityAction = AL.ActionId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = AL.UserId
	
	WHERE
		(AL.TargetId = @ItemId OR @ItemId IS NULL)
	AND	(AL.CompanyId = @CompanyId OR @CompanyId IS NULL)
	AND	(AL.TargetType = @TargetType OR @TargetType IS NULL)
	AND (
			(@From IS NULL AND @To IS NULL)
			OR
			(@From IS NULL AND @To > AL.DateTime)
			OR
			(@From < AL.DateTime AND @To IS NULL)
			OR
			(AL.DateTime BETWEEN @From AND @To)
		)
		
	ORDER BY Al.CompanyId ASC, AL.TargetId ASC, Al.DateTime DESC
END






