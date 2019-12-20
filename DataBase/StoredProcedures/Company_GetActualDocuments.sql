





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetActualDocuments]
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
		DV.Date
	FROM Document D WITH(NOLOCK)
	INNER JOIN DocumentsVersion DV WITH(NOLOCK)
	ON
		D.CompanyId = @CompanyId
	AND D.CompanyId = DV.Company
	AND D.Id = DV.DocumentId
	AND D.ActualVersion = DV.Version
END






