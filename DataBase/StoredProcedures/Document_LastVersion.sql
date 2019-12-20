





CREATE PROCEDURE [dbo].[Document_LastVersion]
	@DocumentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		CASE WHEN MAX(Dv.Version) = 0 THEN 1 ELSE MAX(Dv.Version) END
	FROM DocumentsVersion DV WITH(NOLOCK)
	WHERE
		DV.DocumentId = @DocumentId
	AND DV.Company = @CompanyId
END






