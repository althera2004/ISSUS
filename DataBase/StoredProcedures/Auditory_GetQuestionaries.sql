

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Auditory_GetQuestionaries]
	@AuditoryId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		C.Id AS CuestionarioId,
		C.[Description] AS CuestionarioDescription,
		COUNT(ACP.Id) AS Total,
		SUM(CASE WHEN ACP.Compliant IS NULL THEN 0 ELSE 1 END) AS Completed,
		SUM(CASE WHEN ACP.Compliant = 1 THEN 1 ELSE 0 END) AS Compliant,
		CASE WHEN ACF.Id IS NULL THEN 0 ELSE 1 END AS HasHallazgo
	FROM AuditoryCuestionarioPregunta ACP WITH(NOLOCK)
	INNER JOIN Cuestionario C WITH(NOLOCK)
	ON	C.Id = ACP.CuestionarioId
	LEFT JOIN AuditoryCuestionarioFound ACF WITH(NOLOCK)
	ON	ACF.CuestionarioId = ACP.CuestionarioId
	ANd ACF.AuditoryId = ACP.AuditoryId
	AND	ACF.Active = 1

	WHERE
		ACP.AuditoryId = @AuditoryId
	AND ACP.CompanyId = @CompanyId

	GROUP BY
		C.Id,
		C.[Description],
		ACF.Id


END

