





CREATE PROCEDURE [dbo].[ApplicationUser_SetGrant]
	@ApplicationUserId int,
	@CompanyId int,
	@SecurityGroupId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE ApplicationUserSecurityGroupMembership
	WHERE
		ApplicationUserId = @ApplicationUserId
	AND CompanyId = @CompanyId
	AND SecurityGroupId = @SecurityGroupId
	
	INSERT INTO ApplicationUserSecurityGroupMembership
	(
		ApplicationUserId,
	    SecurityGroupId,
	    CompanyId
	)
	VALUES
	(
		@ApplicationUserId,
		@SecurityGroupId,
		@CompanyId
	)	
	
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
		2,
		@ApplicationUserId,
		8,
		GETDATE(),
		''
    )
	
END






