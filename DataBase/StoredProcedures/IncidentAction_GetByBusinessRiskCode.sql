
CREATE PROCEDURE [dbo].[IncidentAction_GetByBusinessRiskCode]
	@BusinessRiskCode bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.Description,
		IA.BusinessRiskId,
		IA.WhatHappendOn,
		IA.CausesOn,
		IA.ActionsOn,
		IA.ClosedOn,
		BR.Code
    FROM IncidentAction IA WITH(NOLOCK)
	inner join BusinessRisk3 BR WITH(NOLOCK)
	on BR.Id = IA.BusinessRiskId
	WHERE
		BR.Code = @BusinessRiskCode
	AND IA.CompanyId = @CompanyId
	--AND IA.Active = 1
    
END
