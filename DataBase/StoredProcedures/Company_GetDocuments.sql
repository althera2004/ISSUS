





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetDocuments]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		D.Id AS DocumentId,
		DV.Id AS VersionId,
		D.Description,
		DV.Version,
		DV.UserCreate,
		DV.Status,
		DV.Date AS DocumentDateVersion,
		ISNULL(D.Codigo,'') AS Codigo,
		ISNULL(DV.Reason,'') AS Reason,
		ISNULL(DC.Id,0) AS CategoryId,
		ISNULL(DC.Description,'') AS CategoryName,
		CASE WHEN D.Origen = 0 THEN 0 ELSE ISNULL(P.Id,0) END AS ProcedenciaId,
		CASE WHEN D.Origen = 0 THEN '' ELSE ISNULL(P.Description,'') END AS ProcedenciaName,
		ISNULL(AU.Id,0) AS EmployeeCreateId,
		ISNULL(AU.Login, '') AS EmployeeCreateName,
		'' AS EmployeeCreateLastName,
		D.FechaAlta AS StartDate,
		D.FechaBaja AS EndDate,
		D.Conservacion AS ConservationId,
		D.ConservacionType AS ConservationType,
		D.Ubicacion
		
	FROM Document D WITH(NOLOCK)
	INNER JOIN DocumentsVersion DV WITH(NOLOCK)
	ON
		D.CompanyId = @CompanyId
	AND D.CompanyId = DV.Company
	AND D.Id = DV.DocumentId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	DV.UserCreate = AU.Id
	INNER JOIN Document_Category DC WITH(NOLOCK)
	ON	D.CategoryId = DC.Id
	AND D.CompanyId = DC.CompanyId
	LEFT JOIN Procedencia P WITH(NOLOCK)
	ON	D.ProcedenciaId = P.Id
	AND D.CompanyId = P.CompanyId
	WHERE
		D.CompanyId = @CompanyId
	AND D.Activo = 1
						
	
	ORDER BY DV.DocumentId ASC, DV.Version ASC
END






