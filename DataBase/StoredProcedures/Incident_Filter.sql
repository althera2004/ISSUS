





CREATE PROCEDURE [dbo].[Incident_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@StatusIdentified bit,
	@StatusAnalyzed bit,
	@StatusInProcess bit,
	@StatusClosed bit,
	@Origin int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
	*
	FROM
	(
		SELECT
			I.Id AS IncidentId,
			I.WhatHappendOn AS OpenDate,
			ISNULL(I.DepartmentId,0) AS DepartmentId,
			ISNULL(D.Name,'') AS DepartmentName,
			ISNULL(P.Id,0) AS ProviderId,
			ISNULL(P.Description,'') AS ProviderDescription,
			ISNULL(C.Id,0) AS CustomerId,
			ISNULL(C.Description,'') AS CustomerDescription,
			I.Description,
			I.Code,
			I.ClosedOn AS CloseDate,
			ISNULL(IA.Id,0) AS IncidentActionId,
			ISNULL(IA.[Description],'') AS IncidentActionDescription,
			I.Origin,
			SUM(ISNULL(IC.Amount * IC.Quantity,0)) AS Amount,
			CASE	WHEN I.ClosedOn IS NOT NULL  THEN 4 
					WHEN I.ActionsOn IS NOT NULL  THEN 3 
					WHEN I.CausesOn IS NOT NULL  THEN 2 
					WHEN I.WhatHappendOn IS NOT NULL  THEN 1 
			ELSE 0 END AS Status
				
		FROM Incident I WITH(NOLOCK)
		LEFT JOIN Department D WITH(NOLOCK)
		ON	D.Id = I.DepartmentId
		AND D.CompanyId = I.CompanyId
		LEFT JOIN Provider P WITH(NOLOCK)
		ON	P.Id = I.ProviderId
		AND P.CompanyId = I.CompanyId
		LEFT JOIN Customer C WITH(NOLOCK)
		ON	C.Id = CustomerId
		AND	C.CompanyId = I.CompanyId
		LEFT JOIN IncidentAction IA WITH(NOLOCK)
		ON	IA.IncidentId = I.Id
		AND	IA.CompanyId = I.CompanyId
		AND IA.Active = 1
		LEFT JOIN IncidentCost IC WITH(NOLOCK)
		ON	IC.IncidentId = I.Id
		AND	IC.CompanyId = I.CompanyId
		AND IC.Active = 1
		WHERE
			I.CompanyId = @CompanyId
		AND I.Active = 1
		AND	(@DateFrom IS NULL OR I.WhatHappendOn >= @DateFrom)
		AND (@DateTo IS NULL OR I.WhatHappendOn <= @DateTo)
		AND 
		(
			@Origin=0
			OR
			(
				@Origin = 1 AND I.DepartmentId<> 0  AND (I.DepartmentId = @DepartmentId OR @DepartmentId = -2)
			)
			OR
			(
				@Origin = 2 AND I.ProviderId <> 0 AND (I.ProviderId = @ProviderId OR @ProviderId = -2)
			)
			OR
			(
				@Origin = 3 AND I.CustomerId <> 0 AND (I.CustomerId = @CustomerId OR @CustomerId = -2)
			)
		)
		
		GROUP BY
			I.Id ,
			I.WhatHappendOn,
			I.DepartmentId,
			D.Name,
			P.Id,
			P.[Description],
			C.Id,
			C.[Description],
			I.[Description],
			I.Code,
			I.ClosedOn,
			IA.Id,
			IA.[Description],
			I.Origin,
			I.ActionsOn,
			I.CausesOn
	) AS Data
	WHERE
		(@StatusIdentified = 1 AND Data.Status =1)
		OR
		(@StatusAnalyzed = 1 AND Data.Status = 2)
		OR
		(@StatusInProcess = 1 AND Data.Status = 3)
		OR
		(@StatusClosed = 1 AND Data.Status =4)
END






