
CREATE PROCEDURE [dbo].[EquipmentMaintnanceDefinition_Update]
	@EquipmentMaintenanceDefinitionId bigint,
	@EquipmentId bigint,
	@CompanyId int,
	@Operation nvarchar(200),
	@MaintenanceType int,
	@Periodicity int,
	@Accessories nvarchar(100),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@Differences text,
	@UserId int,
	@FirstDate datetime
AS
BEGIN

	SET NOCOUNT ON;

	-- Corregir fecha por defecto de 1970
	IF @FirstDate < '1970-2-2'
	BEGIN
		SET @FirstDate = NULL
	END

	UPDATE EquipmentMaintenanceDefinition SET
		Operation = @Operation,
		Type = @MaintenanceType,
		Periodicity = @Periodicity,
		Accessories = @Accessories,
		Cost = @Cost,
		FirstDate = @FirstDate,
		ProviderId = @ProviderId,
		ResponsableId = @ResponsableId,
		Active = 1,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EquipmentMaintenanceDefinitionId
	AND	EquipmentId = @EquipmentId
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
		16,
		@EquipmentMaintenanceDefinitionId,
		3,
		GETDATE(),
		@Differences
    )

END
