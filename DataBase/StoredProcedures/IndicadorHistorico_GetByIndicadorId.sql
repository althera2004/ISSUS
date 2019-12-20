
CREATE PROCEDURE [dbo].[IndicadorHistorico_GetByIndicadorId]
	@IndicadorId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		IH.Id,
		IH.IndicadorId,
		IH.ActionDate,
		IH.Reason,
		IH.EmployeeId,
		ISNULL(E.Name,'') AS EmployeeFirstName,
		ISNULL(E.LastName,'') AS EmployeeLastName
	FROM IndicadorHistorico IH WITH(NOLOCK)
	INNER JOIN Employee E WITH(NOLOCK)
	ON	E.Id = IH.EmployeeId

	WHERE
		IH.IndicadorId = @IndicadorId

	ORDER BY 
		IH.ActionDate
END

