





CREATE PROCEDURE [dbo].[Company_GetByCode]
	@CompanyCode nvarchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		C.Id
	FROM Company C WITH(NOLOCK)
	WHERE
		C.Code = @CompanyCode
END






