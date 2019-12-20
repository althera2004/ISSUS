





CREATE PROCEDURE [dbo].[EquipmentMaintenanceAct_Delete]
	@EquipmentMaintenanceActId bigint,
	@CompanyId int,
	@UserId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE EquipmentMaintenanceAct SET
		Active = 0,
		ModifiedBy = @USerId,
		ModifiedOn = GETDATE()
    WHERE
		Id = @EquipmentMaintenanceActId
	AND CompanyId = @CompanyId
	
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
		17,
		@EquipmentMaintenanceActId,
		3,
		GETDATE(),
		''
    )

END






