
CREATE PROCEDURE [dbo].[EquipmentMaintenance_Insert]
	@EquipmentMaintenanceId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@Operation nvarchar(50),
	@EquipmentMaintenanceType int,
	@Periodicity int,
	@Accessories nvarchar(50),
	@Cost numeric(18,3),
	@FirstDate datetime,
	@ProviderId bigint,
	@ResponsableId int,
	@UserId int	
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentMaintenanceDefinition
    (
		EquipmentId,
		CompanyId,
		Operation,
		Type,
		Periodicity,
		Accessories,
		Cost,
		FirstDate,
		ProviderId,
		ResponsableId,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@CompanyId,
		@Operation,
		@EquipmentMaintenanceType,
		@Periodicity,
		@Accessories,
		@Cost,
		@FirstDate,
		@ProviderId,
		@ResponsableId,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @EquipmentMaintenanceId = @@IDENTITY
	
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
		@EquipmentMaintenanceId,
		1,
		GETDATE(),
		''
    )

	UPDATE Equipment SET
		IsMaintenance = 1
		WHERE
		Id = @EquipmentId

END

