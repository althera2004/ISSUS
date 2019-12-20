

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmployeeGrants]
	@ItemId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		EUA.EmployeeId
	FROM EmployeeUserAsignation EUA WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EUA.UserId
	AND EUA.CompanyId = @CompanyId
	LEFT JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.UserId = AU.Id
	AND AG.ItemId = @ItemId

	WHERE
		AU.Admin = 1
	OR	AG.GrantToWrite = 1
END

