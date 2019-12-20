

-- =============================================
-- Author:		Juan Castilla Calderón - jcastilla@openframework.es
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ApartadosNorma_All]
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		C.NormaId,
		C.ApartadoNorma
	FROM Cuestionario C WITH(NOLOCK)
	WHERE
		C.Active = 1
	AND C.CompanyId = @CompanyId
	AND C.ApartadoNorma IS NOT NULL
	AND C.ApartadoNorma <> ''
END

