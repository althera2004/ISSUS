





CREATE PROCEDURE [dbo].[User_GetById]
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AU.Id,
		AU.[Login],
		AU.[Status],
		AU.[Language],
		AUSGM.SecurityGroupId,
		ISNULL(E.Id, 0) AS EmployeeId,
		ISNULL(MSCG.Label,'') AS GreenLabel,
		ISNULL(MSCG.Icon,'') AS GreenIcon,
		ISNULL(MSCG.Link,'') AS GreenLink,
		ISNULL(MSCB.Label,'') AS BlueLabel,
		ISNULL(MSCB.Icon,'') AS BlueIcon,
		ISNULL(MSCB.Link,'') AS BlueLink,
		ISNULL(MSCR.Label,'') AS RedLabel,
		ISNULL(MSCR.Icon,'') AS RedIcon,
		ISNULL(MSCR.Link,'') AS RedLink,
		ISNULL(MSCY.Label,'') AS YellowLabel,
		ISNULL(MSCY.Icon,'') AS YellowIcon,
		ISNULL(MSCY.Link,'') AS YellowLink,
		ISNULL(E.Name, AU.[Login]) AS EmployeeName,
		ISNULL(E.LastName,'') AS EmployeeLastName,
		AU.ShowHelp,
		USC.ShorcutGreen AS GreenId,
		USC.ShorcutBlue AS BlueId,
		USC.ShortcutYellow AS YellowId,
		USC.ShortcutRed AS RedId,
		ISNULL(AU.Avatar,'avatar2.png') AS Avatar,
		AU.Email,
		ISNULL(AU.PrimaryUser,0) AS PrimaryUser,
		AU.CompanyId AS CompanyId,
		ISNULL(AU.[Admin],0)
	FROM ApplicationUser AU WITH(NOLOCK)
	LEFT JOIN ApplicationUserSecurityGroupMembership AUSGM WITH(NOLOCK)
	ON	AU.Id = AUSGM.ApplicationUserId
	AND	AU.CompanyId = AUSGM.CompanyId
	LEFT JOIN UserShortCuts USC WITH(NOLOCK)
	ON	USC.ApplicationUserId = AU.Id
	AND USC.CompanyId = AU.CompanyId
	LEFT JOIN MenuShortCuts MSCG WITH(NOLOCK)
	ON	MSCG.ID = USC.ShorcutGreen
	LEFT JOIN MenuShortCuts MSCB WITH(NOLOCK)
	ON	MSCB.ID = USC.ShorcutBlue
	LEFT JOIN MenuShortCuts MSCY WITH(NOLOCK)
	ON	MSCY.ID = USC.ShortcutYellow
	LEFT JOIN MenuShortCuts MSCR WITH(NOLOCK)
	ON	MSCR.ID = USC.ShortcutRed
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	EUA.EmployeeId = E.Id
	ON EUA.UserId = AU.Id
	
		
	WHERE
		AU.Id = @UserId;
END






