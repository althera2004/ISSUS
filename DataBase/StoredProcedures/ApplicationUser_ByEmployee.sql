





CREATE PROCEDURE [dbo].[ApplicationUser_ByEmployee]
	@EmployeeId bigint,
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
		ISNULL(AU.PrimaryUser,0) AS PrimaryUser
	FROM EmployeeUserAsignation EUA WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	EUA.EmployeeId = @EmployeeId
	AND EUA.UserId = AU.Id
	AND EUA.CompanyId = AU.CompanyId
	AND EUA.CompanyId = @CompanyId
	LEFT JOIN ApplicationUserSecurityGroupMembership G WITH(NOLOCK)
		INNER JOIN SecurityGroup SG WITH(NOLOCK)
		ON SG.Id = G.SecurityGroupId
	ON	G.CompanyId = AU.CompanyId
	AND G.ApplicationUserId = AU.Id
END






