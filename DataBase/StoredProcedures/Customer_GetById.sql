





CREATE PROCEDURE [dbo].[Customer_GetById]
	@CustomerId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		C.Id,
		C.CompanyId,
		C.Description,
		C.Active,
		C.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		C.ModifiedOn
    FROM Customer C WITH(NOLOCK)
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = C.ModifiedBy
	WHERE
		C.Id= @CustomerId
	AND C.CompanyId = @CompanyId
END






