





CREATE PROCEDURE [dbo].[Department_GetById]
	@Id int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		D.Id AS DepartmentId,
		D.Name AS DepartmentDescription,
		D.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		D.ModifiedOn
	FROM Department D WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = D.ModifiedBy
	AND AU.CompanyId = D.CompanyId	
	
	WHERE
		D.Id = @Id
	AND D.CompanyId = @CompanyId
	
		
END






