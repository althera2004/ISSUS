

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObjetivoHistorico_GetByObjetivoId]
	@ObjetivoId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		OH.Id,
		OH.ObjetivoId,
		OH.ActionDate,
		OH.Reason,
		OH.EmployeeId,
		ISNULL(E.Name,'') AS EmployeeFirstName,
		ISNULL(E.LastName,'') AS EmployeeLastName
	FROM ObjetivoHistorico OH WITH(NOLOCK)
	INNER JOIN Employee E WITH(NOLOCK)
	ON	E.Id = OH.EmployeeId

	WHERE
		OH.ObjetivoId = @ObjetivoId

	ORDER BY 
		OH.ActionDate
END

