






CREATE PROCEDURE [dbo].[IncidentAction_GetByOportunityCode]
	@OportunityCode bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.Description,
		IA.OportunityId,
		IA.WhatHappendOn,
		IA.CausesOn,
		IA.ActionsOn,
		IA.ClosedOn,
		O.Code
    FROM IncidentAction IA WITH(NOLOCK)
	inner join Oportunity O WITH(NOLOCK)
	on O.Id = IA.OportunityId
	WHERE
		O.Code = @OportunityCode
	AND IA.CompanyId = @CompanyId
	--AND IA.Active = 1
    
END






