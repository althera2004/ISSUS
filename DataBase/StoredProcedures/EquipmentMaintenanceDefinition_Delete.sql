





CREATE PROCEDURE [dbo].[EquipmentMaintenanceDefinition_Delete]
	@EquipmentMaintenanceDefinitionId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentMaintenanceDefinition SET
		Active = 0
	WHERE
		Id = @EquipmentMaintenanceDefinitionId
		
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
		16,
		@EquipmentMaintenanceDefinitionId,
		3,
		GETDATE(),
		''
    )	
END






