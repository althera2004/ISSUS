





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetDocumentProcedencias]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		P.Id,
		P.Description,
		P.Editable,
		CAST (CASE WHEN AA.ProcedenciaId IS NULL THEN 1 ELSE 0 END AS BIT) AS Deletable
	FROM Procedencia P WITH(NOLOCK)	
	LEFT JOIN
	(
		SELECT D.ProcedenciaId, D.CompanyId FROM Document D WITH(NOLOCK)
	) AA
	ON	AA.CompanyId = P.CompanyId
	AND P.Id = AA.ProcedenciaId
	WHERE
		P.CompanyId = @CompanyId
	ORDER BY P.Description ASC
END






