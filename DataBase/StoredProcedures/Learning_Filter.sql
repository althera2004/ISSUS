





CREATE PROCEDURE [dbo].[Learning_Filter]
	@YearFrom datetime,
	@YearTo datetime,
	@Pendent bit,
	@Started bit,
	@Finished bit,
	@Evaluated bit,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		L.Id,
		L.Description,
		L.DateStimatedDate,
		YEAR(L.DateStimatedDate),
		L.Amount,
		L.[Status],
		ISNULL(L.RealFinish, '1/1/1970')
	FROM Learning L WITH(NOLOCK)
	WHERE
		L.CompanyId = @CompanyId
	AND L.Active = 1
	AND (@YearFrom IS NULL OR L.DateStimatedDate >= @YearFrom)
	AND (@YearTo IS NULL OR L.DateStimatedDate <= @YearTo OR L.DateStimatedDate IS NULL)
	AND (
			(L.Status = 0 AND @Pendent = 1)
			OR
			(L.Status = 1 AND @Started = 1)
			OR
			(L.Status = 2 AND @Finished = 1)
			OR
			(L.Status = 3 AND @Evaluated = 1)
		)
END





