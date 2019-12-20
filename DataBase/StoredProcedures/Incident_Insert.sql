





CREATE PROCEDURE [dbo].[Incident_Insert]
	@IncidentId bigint output,
	@CompanyId int,
	@Description nvarchar(100),
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
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Code bigint
	
	SELECT @Code = ISNULL(MAX(I.Code),1) + 1
	FROM Incident I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1

					 
	  
				 
	

				   
	  
				 
	

				   
	  
				 
	
	

    INSERT INTO Incident
    (
		CompanyId,
		Description,
		Code,
		Origin,
		DepartmentId,
		ProviderId,
		CustomerId,
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
		ApplyAction,
		Notes,
		Anotations,
		ClosedBy,
		ClosedOn,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		@Code,
		@Origin,
		@DepartmentId,
		@ProviderId,
		@CustomerId,
		@WhatHappend,
		@WhatHappendBy,
		@WhatHappendOn,
		@Causes,
		@CausesBy,
		@CausesOn,
		@Actions,
		@ActionsBy,
		@ActionsOn,
		@ActionsExecuter,
		@ActionsSchedule,
		@ApplyAction,
		@Notes,
		@Anotations,
		@ClosedBy,
		@ClosedOn,
        1,
        @UserId,
        GETDATE(),
        @UserId,
        GETDATE()
	)

	SET @IncidentId = @@IDENTITY
END






