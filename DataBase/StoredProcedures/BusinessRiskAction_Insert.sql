






CREATE PROCEDURE [dbo].[BusinessRiskAction_Insert]
	@IncidentActionId bigint output,
	@CompanyId int,
	@ActionType int,
	@Description nvarchar(50),
	@Origin int,
	@ReporterType int,
	@DeparmentId int,
	@ProviderId int,
	@CustomerId int,
	@Number bigint,
	@BusinessRiskId bigint,
	@WhatHappend nvarchar(255),
	@WhatHappendBy bigint,
	@WhatHappendDate datetime,
	@Causes nvarchar(255),
	@CausesBy bigint,
	@CausesDate datetime,
	@Actions nvarchar(255),
	@ActionsBy bigint,
	@ActionsExecuter int,
	@ActionsSchedule datetime,
	@ActionsDate datetime,
	@Monitoring nvarchar(255),
	@ClosedBy bigint,
	@ClosedDate datetime,
	@ClosedExecutor bigint,
	@ClosedExecutorOn datetime,
	@Notes text,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
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
		BusinessRiskId,
		WhatHappend,
		WhatHappendBy,
		WhatHappendOn,
		Causes,
		CausesBy,
		CausesOn,
		Actions,
		ActionsBy,
		ActionsOn,
		ActionsExecuter,
		ActionsSchedule,
		Monitoring,
		ClosedBy,
		ClosedOn,
		ClosedExecutor,
		ClosedExecutorOn,
		Notes,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
    VALUES
    (
		@CompanyId,
		@ActionType,
		@Description,
		@Origin,
		@ReporterType,
		@DeparmentId,
		@ProviderId,
		@CustomerId,
		@Number,
		@BusinessRiskId,
		@WhatHappend,
		@WhatHappendBy,
		@WhatHappendDate,
		@Causes,
		@CausesBy,
		@CausesDate,
		@Actions,
		@ActionsBy,
		@ActionsDate,
		@ActionsExecuter,
		@ActionsSchedule,
		@Monitoring,
		@ClosedBy,
		@ClosedDate,
		@ClosedExecutor,
		@ClosedExecutorOn,
		@Notes,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()		
	)

	SET @IncidentActionId = @@IDENTITY

END







