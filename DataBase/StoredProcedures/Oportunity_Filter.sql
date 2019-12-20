

CREATE PROCEDURE [dbo].[Oportunity_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@RulesId bigint,
	@ProcessId bigint
AS
BEGIN
	SET NOCOUNT ON;	
	SELECT
	*
	FROM
	(
		SELECT
			B.Id AS BusinessRiskId,
			B.DateStart AS OpenDate,
			B.[Description],
			CAST(ISNULL(B.Code,0) AS nvarchar(4)) AS Code,
			B.AnulateDate AS CloseDate,		
			B.ProcessId,	
			PRO.Description As ProcessDescription,
			B.RuleId,
			R.Description As RuleDescription,
			R.Limit,
			CASE WHEN B.FinalDate IS NULL THEN ISNULL(B.Result, 0) ELSE B.FinalResult END AS Result,
			B.FinalApplyAction,
			B.FinalDate
			

		FROM Oportunity B WITH(NOLOCK)
		Inner Join Proceso PRO With (Nolock)
		On PRO.Id = B.ProcessId
		Inner Join Rules R With (Nolock)
		On R.Id = B.RuleId
		WHERE
			B.CompanyId = @CompanyId
		AND B.Active = 1
		AND	(@DateFrom IS NULL OR B.DateStart >= @DateFrom)
		AND (@DateTo IS NULL OR B.DateStart <= @DateTo)
		AND (@RulesId IS NULL OR B.RuleId = @RulesId)
		AND (@ProcessId IS NULL OR B.ProcessId = @ProcessId)
		AND 
		(
			B.FinalDate IS NULL
			OR
			(B.FinalDate IS NOT NULL AND B.FinalApplyAction = 0)
		)

	) AS Data

	ORDER BY
		Data.Code
END






