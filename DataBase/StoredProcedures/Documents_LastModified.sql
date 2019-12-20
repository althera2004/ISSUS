





CREATE PROCEDURE [dbo].[Documents_LastModified]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 10
		D.Id,
		D.Codigo,
		D.Description,
		V.Version,
		D.ModifiedBy AS ModifiedById,
		AU.[Login] AS ModifiedByUserName,
		D.ModifiedOn
	FROM Document D WITH(NOLOCK)
	INNER JOIN ApplicationUser AU
	ON AU.Id = D.ModifiedBy
	INNER JOIN 
	(
		SELECT 
			DocumentId,
			MAX(Version) Version
		FROM DocumentsVersion WITH(NOLOCK)
		GROUP BY DocumentId
	) AS V
	ON	V.DocumentId = D.Id
	WHERE
		D.CompanyId = @CompanyId
	ORDER BY 
		D.ModifiedOn DESC
END






