







CREATE PROCEDURE [dbo].[Objetivo_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@Status int
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		O.Id AS ObjetivoId,
		O.Name,
		O.ResponsibleId,
		ISNULL(R.Name,'') AS ResponsibleName,
		ISNULL(R.LastName,'') AS ResponsibleLastname,
		O.StartDate,
		O.PreviewEndDate,
		O.EndDate,
		O.Active
	FROM Objetivo O WITH(NOLOCK)
	LEFT JOIN Employee R WITH(NOLOCK)
	ON	R.Id = O.ResponsibleId
	
	WHERE
		O.CompanyId = @CompanyId
	AND O.Active = 1
	-- AND	(@DateFrom IS NULL OR O.PreviewEndDate >= @DateFrom)
	-- AND (@DateTo IS NULL OR O.PreviewEndDate <= @DateTo)
	AND	(@DateFrom IS NULL OR (O.EndDate >= @DateFrom) OR (O.EndDate IS NULL AND O.PreviewEndDate >= @DateFrom))
	AND (@DateTo IS NULL OR O.StartDate <= @DateTo)
	
	AND
	(
		@Status = 0
		OR
		(O.EndDate > GETDATE() OR O.EndDate IS NULL AND @Status = 1)
		OR
		(
		O.EndDate < GETDATE() AND @Status = 2
		)
	)
	
END








