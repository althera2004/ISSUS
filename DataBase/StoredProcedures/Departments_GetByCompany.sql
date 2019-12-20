





CREATE PROCEDURE [dbo].[Departments_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		D.Id,
		D.Name,
		D.CompanyId,
		D.Deleted,
		CASE WHEN C.Id IS NULL THEN 0 ELSE 1 END
	FROM Department D WITH(NOLOCK)
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON C.DepartmentId = D.Id
	WHERE
		D.CompanyId = @CompanyId
	ORDER BY D.Name ASC
	
END






