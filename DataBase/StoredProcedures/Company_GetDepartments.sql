





CREATE PROCEDURE [dbo].[Company_GetDepartments]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		D.Id,
		D.Name
	FROM Department D WITH(NOLOCK)
	WHERE
		D.CompanyId = @CompanyId
	AND D.Deleted = 0
	ORDER BY D.Name ASC
END






