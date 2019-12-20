

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CustionariosPreguntas_Count]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		C.Id,
		C.NormaId,
		C.ProcessId,
		COUNT(CP.Id)
	FROM Cuestionario C WITH(NOLOCK)
	LEFT JOIN CuestionarioPregunta CP WITH(NOLOCK)
	ON	CP.CuestionarioId = C.Id

	WHERE
		C.Active = 1
	AND C.CompanyId = @CompanyId
	AND CP.Active = 1

	GROUP BY
		C.Id,
		C.NormaId,
		C.ProcessId

END

