





CREATE PROCEDURE [dbo].[ApplicationUser_GetById]
	@UserId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		AU.Id,
		AU.Login,
		AU.MustResetPassword,
		AU.Status,
		ISNULL(G.SecurityGroupId, 0) AS SecurityGroupId,
		ISNULL(SG.Name,'') AS SecurityGroupName,
		ISNULL(E.Id,0) AS EmployeeId,
		ISNULL(E.Name,'') AS EmployeeName,
		ISNULL(E.LastName,'') AS EmployeeLastName,
		ISNULL(AU.Email,'') AS Email,
		ISNULL(AU.PrimaryUser, 0) AS PrimaryUser,
		ISNULL(AU.[Admin], 0),
		ISNULL(AU.[Language], C.[Language]) AS Language
	FROM ApplicationUser AU WITH(NOLOCK)
	INNER JOIN Company C WITH(NOLOCK)
	ON	 C.Id = AU.CompanyId
	LEFT JOIN ApplicationUserSecurityGroupMembership G WITH(NOLOCK)
		INNER JOIN SecurityGroup SG WITH(NOLOCK)
		ON SG.Id = G.SecurityGroupId
	ON	G.CompanyId = AU.CompanyId
	AND G.ApplicationUserId = AU.Id
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = EUA.EmployeeId
	ON	 EUA.UserId = AU.Id

	WHERE
		AU.Id = @UserId
	AND AU.CompanyId = @CompanyId
END






