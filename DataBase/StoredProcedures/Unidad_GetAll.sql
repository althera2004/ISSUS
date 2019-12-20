



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Unidad_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT distinct
		U.Id,
		U.CompanyId,
		U.[Description],
		U.CreatedBy,
		CB.[Login] AS CreatedByName,
		U.CreatedOn,
		U.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		U.ModifiedOn,
		U.Active,
		CASE WHEN I.Id IS NULL THEN 1 ELSE 0 END AS Deletable
	FROM Unidad U WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = U.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = U.ModifiedBy
	LEFT JOIN Indicador I WITH(NOLOCK)
	ON	I.UnidadId = U.Id
	AND	I.Active = 1

	WHERE
		U.CompanyId = @CompanyId

	ORDER By U.[Description]

END




