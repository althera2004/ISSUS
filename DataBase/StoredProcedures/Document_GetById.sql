





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Document_GetById]
	@DocumentId bigint,
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
		DV.Date,
		ISNULL(DV.Reason,'') AS Reason,
		DV.UserCreate,
		ISNULL(D.CategoryId,0) AS CategoryId,
		ISNULL(DC.Description,'') AS CategoryName,
		ISNULL(P.Id,0) AS ProcedenciaId,
		ISNULL(P.Description,'') AS ProcedenciaName,
		ISNULL(D.Codigo,'') AS Codigo,
		D.FechaAlta,
		D.FechaBaja,
		D.Conservacion,
		D.ConservacionType,
		D.Origen,
		ISNULL(D.Ubicacion,'') AS Ubicacion,
		D.ModifiedOn,
		D.ModifiedBy AS ModifiedByUserId,
		AU.Login,
		ISNULL(D.EndReason,'') AS EndReason
	FROM Document D WITH(NOLOCK)
	INNER JOIN DocumentsVersion DV WITH(NOLOCK)
	ON
		D.CompanyId = @CompanyId
	AND D.Id = @DocumentId
	AND D.CompanyId = DV.Company
	AND D.Id = DV.DocumentId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON
		DV.UserCreate = AU.Id
	LEFT JOIN Document_Category DC WITH(NOLOCK)
	ON
		D.CategoryId = DC.Id
	AND D.CompanyId = DC.CompanyId
	LEFT JOIN Procedencia P WITH(NOLOCK)
	ON
		P.Id = D.ProcedenciaId
	AND P.CompanyId = D.CompanyId
	
	ORDER BY DV.DocumentId ASC, DV.Version ASC
END






