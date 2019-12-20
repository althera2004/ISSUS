





CREATE PROCEDURE [dbo].[Documents_GetInactive]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		D.Id,
		D.Codigo,
		D.Description,
		D.ActualVersion,
		E.Id AS ModifiedById,
		ISNULL(E.Name,'') AS ModifiedByName,
		ISNULL(E.LastName,'') AS ModifiedByLastName,
		D.ModifiedOn
	FROM Document D WITH(NOLOCK)
	INNER JOIN Employee E WITH(NOLOCK)
	ON	D.ModifiedBy = E.Id
	WHERE
		D.CompanyId = @CompanyId
	AND D.FechaBaja IS NOT NULL
	ORDER BY 
		D.FechaBaja DESC
END






