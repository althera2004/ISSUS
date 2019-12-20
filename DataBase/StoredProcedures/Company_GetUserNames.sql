





CREATE PROCEDURE [dbo].[Company_GetUserNames]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		UPPER(AU.Login) AS UserName,
		AU.Id AS UserId,
		AU.Status
	FROM ApplicationUser AU WITH(NOLOCK)
	WHERE
		AU.CompanyId = @CompanyId
	AND AU.Status <> 0
	
	ORDER BY UPPER(AU.Login) ASC
END






