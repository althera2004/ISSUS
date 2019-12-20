





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetDocumentCategories]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		DC.Id,
		DC.Description,
		DC.Editable,
		CAST (CASE WHEN AA.CategoryId IS NULL THEN 1 ELSE 0 END AS BIT) AS Deletable
	FROM Document_Category DC WITH(NOLOCK)
	LEFT JOIN
	(
		SELECT D.CategoryId, D.CompanyId FROM Document D WITH(NOLOCK) WHERE D.Activo = 1 
	) AA
	ON	AA.CompanyId = DC.CompanyId
	AND DC.Id = AA.CategoryId
	WHERE
		DC.CompanyId = @CompanyId
END






