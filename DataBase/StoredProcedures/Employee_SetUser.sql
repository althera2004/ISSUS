




CREATE PROCEDURE [dbo].[Employee_SetUser]
	@EmployeeId bigint,
	@UserId bigint,
	@CompanyId int
AS
BEGIN

DELETE FROM [EmployeeUserAsignation] 
WHERE
	UserId = @UserId
AND	CompanyId = @CompanyId


INSERT [EmployeeUserAsignation]
           ([UserId]
           ,[EmployeeId]
           ,[CompanyId])
     VALUES
           (@UserId,
           @EmployeeId,
           @CompanyId)


END






