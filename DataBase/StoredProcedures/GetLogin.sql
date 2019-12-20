





CREATE PROCEDURE [dbo].[GetLogin]
	@Login nvarchar(50),
	@Password nvarchar(15)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AU.Id,
		AU.Login,
		CASE WHEN C.ID = 1 
			THEN 1000
			ELSE CASE WHEN C.Id = 2 
				THEN 1002
				ELSE AU.Status
			END
		END AS Status,
		AU.Language,
		C.Id AS CompanyId,
		C.Language AS CompanyLanguage,
		AGM.SecurityGroupId,
		0,
		SG.Name,
		AU.Status,
		AU.MustResetPassword,
		0 AS PrimaryUser,
		ISNULL(C.Agreement,0) AS Agreement
	FROM ApplicationUser AU WITH(NOLOCK)
	LEFT JOIN Company C WITH(NOLOCK)
	ON	AU.CompanyId = C.Id
	LEFT JOIN ApplicationUserSecurityGroupMembership AGM WITH(NOLOCK)
	ON	AGM.ApplicationUserId = AU.Id
	AND	AGM.CompanyId = C.Id
	LEFT JOIN SecurityGroup SG WITH(NOLOCK)
	ON SG.Id = AGM.SecurityGroupId
	
	WHERE
		AU.Email = @Login
	AND AU.Password = @Password
	AND AU.[Status] = 1

	ORDER BY C.Name
END






