





CREATE PROCEDURE [dbo].[IncidentAction_FromZombie]
	@ZombieId bigint,
	@CompanyId int,
	@ApplicationUserId int,
	@ReporterType int,
	@ProviderId bigint,
	@CustomerId bigint
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Number bigint
	
	SELECT @Number = ISNULL(MAX(Number) ,0) + 1
	FROM IncidentAction I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1



    INSERT INTO IncidentAction
    (
		CompanyId,
		ActionType,
		Description,
		Origin,
		ReporterType,
		DepartmentId,
		ProviderId,
		CustomerId,
		Number,
		IncidentId,
		WhatHappend,
		WhatHappendBy,
		WhatHappendOn,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		BusinessRiskId,
		ObjetivoId,
		OportunityId,
		AuditoryId
	)
    SELECT
		CompanyId,
		ActionType,
		Description,
		1,
		@ReporterType,
		null,
		@ProviderId,
		@CustomerId,
		@Number,
		NULL,
		WhatHappend,
		WhatHappendBy,
		WhatHappendOn,
		1,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		NULL,
		NULL,
		NULL,
		AuditoryId
	FROM IncidentActionZombie
	WHERE
		Id = @ZombieId
	AND CompanyId = @CompanyId



	DELETE FROM IncidentActionZombie
	WHERE
		Id = @ZombieId
	AND CompanyId = @CompanyId

END





