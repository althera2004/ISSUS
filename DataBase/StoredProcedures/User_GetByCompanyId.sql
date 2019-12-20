





CREATE PROCEDURE [dbo].[User_GetByCompanyId]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AU.Id,
		AU.[Login],
		AU.[Status],
		AU.[Language],
		ISNULL(AU.Email,'') AS UserEmail,
		AUSGM.SecurityGroupId,
		ISNULL(E.Id,0) AS EmployeeId,
		ISNULL(E.Name,'') AS EmployeeName,
		ISNULL(E.LastName,'') AS EmployeeLastName,
		ISNULL(E.Email,'') AS EmployeeEmail,
		AU.PrimaryUser,
		AU.[Admin]
	FROM ApplicationUser AU WITH(NOLOCK)
	LEFT JOIN ApplicationUserSecurityGroupMembership AUSGM WITH(NOLOCK)
	ON	AU.Id = AUSGM.ApplicationUserId
	AND	AU.CompanyId = AUSGM.CompanyId
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = EUA.EmployeeId
	ON	 EUA.UserId = AU.Id
		
	WHERE
		AU.CompanyId = @CompanyId
	AND AU.Status <> 0
		
	ORDER BY AU.[Login] ASC
END






