





CREATE PROCEDURE [dbo].[BusinessRisk_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@RulesId bigint,
	@ProcessId bigint,
	@Type int
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
			B.Code,
			B.ModifiedOn AS CloseDate,		
			B.ProcessId,	
			PRO.Description As ProcessDescription,
			B.RuleId,
			R.Description As RuleDescription,
			CASE WHEN B.FinalDate IS NULL THEN ISNULL(B.StartResult,0) ELSE ISNULL(B.FinalResult,0) END AS StartResult,
			--B.FinalResult AS FinalResult,
			CASE WHEN B.FinalDate IS NULL THEN ISNULL(B.StartResult,0) ELSE ISNULL(B.FinalResult,0) END AS FinalResult,
			R.Limit,
			ISNULL(B.StartAction,0) AS StartAction,
			ISNULL(B.FinalAction,0) AS FinalAction,
			B.Assumed,			
			CASE WHEN B.Assumed = 1 OR FinalAction = 1 AND B.FinalDate IS NOT NULL THEN 1	
			ELSE
				CASE WHEN B.StartResult = 0 THEN 4
				ELSE
					CASE WHEN B.FinalResult > 0 AND B.FinalResult >= R.Limit THEN 2
					ELSE 
						CASE WHEN B.StartResult >= R.Limit AND B.FinalDate IS NULL THEN 2
						ELSE
						3
						END
					END
				END
			END AS Status

		FROM BusinessRisk3 B WITH(NOLOCK)
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
		--AND (@Type <> 1 OR @Type IS NULL OR (@Type = 1 AND (B.Assumed = 1 OR B.FinalAction = 1)))
		--AND ((@Type <> 2 OR @Type IS NULL OR (@Type = 2 AND ISNULL(B.FinalResult, B.StartResult) >= R.Limit) AND B.Assumed = 0 AND (B.FinalAction <> 1 OR B.FinalAction IS NULL)))
		--AND ((@Type <> 3 OR @Type IS NULL OR (@Type = 3 AND ISNULL(B.FinalResult, B.StartResult) < R.Limit) AND B.Assumed = 0 AND B.FinalAction <> 1 AND B.StartResult > 0))
		--AND ((@Type <> 4 OR @Type IS NULL OR (@Type = 4 AND B.StartResult = 0) AND B.Assumed = 0 AND B.FinalAction <> 1))
		
	) AS Data

	ORDER BY
		Data.Code
END






