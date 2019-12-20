
CREATE PROCEDURE [dbo].[IncidentActions_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@StatusIdentified bit,
	@StatusAnalyzed bit,
	@StatusInProcess bit,
	@StatusClosed bit,
	@TypeImpovement bit,
	@TypeFix bit,
	@TypePrevent bit,
	@Origin int,
	@Reporter int
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id AS IncidentActionId,
		I.ActionType,
		I.Origin,
		I.[Description],
		I.WhatHappendOn AS OpenDate,
		I.CausesOn AS CausesDate,
		I.ActionsOn AS ImplementationDate,
		I.ClosedOn AS CloseDate,
		I.Number,
		I.ReporterType,

		ISNULL(AU.Id,
			ISNULL(INC.Id,
				ISNULL(BR.Id,
					ISNULL(OB.Id,
						ISNULL(O.Id,0)
					)
				)
			)
		) AS IncidentId ,
		ISNULL(AU.[Description],
			ISNULL(INC.[Description],
				ISNULL(BR.[Description],
					ISNULL(OB.[Description],
						ISNULL(O.[Description],'')
					)
				)
			)
		) AS IncidentDescription ,

		--ISNULL(AU.Id,(ISNULL(ISNULL(INC.Id,ISNULL(BR.Id, ISNULL(OB.Id,ISNULL(O.Id,0)))))) AS IncidentId,
		--ISNULL(ISNULL(INC.[Description], ISNULL(BR.[Description],ISNULL( OB.[Description],''))), '') AS IncidentCode,
		SUM(ISNULL(IAC.Amount * IAC.Quantity,0)) AS Amount,
		ISNULL(AU.Type,0)
	FROM IncidentAction I WITH(NOLOCK)
	LEFT JOIN Incident INC WITH(NOLOCK)
	ON	INC.Id = I.IncidentId
	AND INC.CompanyId = I.CompanyId
	AND INC.Id > 0
	LEFT JOIN BusinessRisk3 BR WITH(NOLOCK)
	ON	BR.Id = I.BusinessRiskId
	AND BR.CompanyId= I.CompanyId
	LEFT JOIN Oportunity O WITH(NOLOCK)
	ON	O.Id = I.OportunityId
	AND O.CompanyId= I.CompanyId
	LEFT JOIN IncidentActionCost IAC WITH(NOLOCK)
	ON	IAC.IncidentActionId = I.Id
	AND	IAC.CompanyId = I.CompanyId
	AND IAC.Active = 1
	LEFT JOIN Objetivo OB WITH(NOLOCK)
	ON	OB.Id = I.ObjetivoId
	LEFT JOIN Auditory AU WITH(NOLOCK)
	ON	AU.Id = I.AuditoryId

	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1
	AND	(@DateFrom IS NULL OR I.WhatHappendOn >= @DateFrom)
	AND (@DateTo IS NULL OR I.WhatHappendOn <= @DateTo)
	AND
	(
		(@TypeImpovement = 1 AND I.ActionType=1)
		OR
		(@TypeFix = 1 AND I.ActionType=2)
		OR
		(@TypePrevent = 1 AND I.ActionType=3)
	)
	AND
	(
		(@StatusIdentified = 1 AND I.WhatHappendOn IS NOT NULL AND I.CausesOn IS NULL)
		OR
		(@StatusAnalyzed = 1 AND I.CausesOn IS NOT NULL AND I.ActionsOn IS NULL)
		OR
		(@StatusInProcess = 1 AND I.ActionsOn IS NOT NULL AND I.ClosedOn IS NULL)
		OR
		(@StatusClosed = 1 AND I.ClosedOn IS NOT NULL)
	)
	AND (@Origin = I.Origin OR @Origin = -1 OR (@Origin = 4 AND I.Origin=6))
	AND (@Reporter IS NULL OR @Reporter = 0 OR @Reporter = I.ReporterType)

	GROUP BY
		I.Id,
		I.ActionType,
		I.Origin,
		I.[Description],
		I.WhatHappendOn,
		I.CreatedOn,
		I.CausesOn,
		I.ActionsOn,
		I.ClosedOn,
		I.Number,
		I.ReporterType,
		I.OportunityId,
		INC.Id,
		INC.[Description],
		BR.Id,
		BR.[Description],
		OB.Id,
		OB.[Description],
		O.Id,
		O.[Description],
		AU.Id,
		AU.[Description],
		AU.[Type]
	
END
