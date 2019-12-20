





CREATE PROCEDURE [dbo].[Grants_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
	-1,
	Id,
	0 AS GrantToRead,
	0 AS GrantToWrite,
	0 AS GrantToWrite,
	AI.UrlList
	FROM  ApplicationItem AI

	ORDER BY AI.Description
END






