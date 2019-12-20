





CREATE PROCEDURE [dbo].[Incident_Update]
	@IncidentId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@Code int,
	@Origin int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint,
	@WhatHappend nvarchar(2000),
	@WhatHappendBy bigint,
	@WhatHappendOn datetime,
	@Causes nvarchar(2000),
	@CausesBy bigint,
	@CausesOn datetime,
	@Actions nvarchar(2000),
	@ActionsBy bigint,
	@ActionsOn datetime,
	@ActionsExecuter bigint,
	@ActionsSchedule datetime,
	@ApplyAction bit,
	@Notes nvarchar(2000),
	@Anotations nvarchar(2000),
	@ClosedBy bigint,
	@ClosedOn datetime,
	@UserId int,
	@Differences text
AS
BEGIN
	SET NOCOUNT ON;

					 
	  
				 
	

				   
	  
				 
	

				   
	  
				 
	

    UPDATE Incident SET 
      Description = @Description,
      Origin = @Origin,
      DepartmentId = @DepartmentId,
      ProviderId = @ProviderId,
      CustomerId = @CustomerId,
      WhatHappend = @WhatHappend,
      WhatHappendBy = @WhatHappendBy,
      WhatHappendOn = @WhatHappendOn,
      Causes = @Causes,
      CausesBy = @CausesBy,
      CausesOn = @CausesOn,
      Actions = @Actions,
      ActionsBy = @ActionsBy,
      ActionsOn = @ActionsOn,
      ActionsExecuter = @ActionsExecuter,
      ActionsSchedule = @ActionsSchedule,
      ApplyAction = @ApplyAction,
      Notes = @Notes, 
      Anotations = @Anotations,
      --ClosedBy = @ClosedBy,
      --ClosedOn = @ClosedOn, 
      ModifiedBy = @UserId,
      ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentId
	AND CompanyId = @CompanyId

	IF @ApplyAction = 0
	BEGIN
		UPDATE IncidentAction SET
			Active = 0,
			ModifiedBy = @UserId,
			ModifiedOn = GETDATE()
		WHERE
			Active = 1
		AND	IncidentId = @IncidentId
		AND CompanyId = @CompanyId
	END
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		22,
		@IncidentId,
		1,
		GETDATE(),
		@Differences
	)

END






