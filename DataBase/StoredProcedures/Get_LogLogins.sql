





CREATE PROCEDURE [dbo].[Get_LogLogins]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
    L.Result,
    L.Date,
    L.Ip,
    ISNULL(L.CompanyCode,'') AS Companycode,
    AU.Login
    FROM Logins L WITH(NOLOCK)
    LEFT JOIN ApplicationUser AU WITH(NOLOCK)
    ON
		L.UserId = AU.Id
    WHERE
		(@CompanyId IS NULL OR L.CompanyId= @CompanyId)
	ORDER BY Date DESC
END






