
CREATE PROCEDURE [dbo].[EquipmentMaintenanceAct_Insert]
	@EquipmentMaintenanceActId bigint output,
	@EquipmentMaintenanceDefinitionId bigint,
	@EquipmentId bigint,
	@CompanyId int,
	@Date datetime,
	@Operation nvarchar(200),
	@Observations text,
	@ProviderId bigint,
	@ResponsableId int,
	@Cost numeric(18,3),
	@Vto datetime,
	@UserId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO EquipmentMaintenanceAct
    (
		EquipmentId,
		EquipmentMaintenanceDefinitionId,
		CompanyId,
		Date,
		Operation,
		Observations,
		ProviderId,
		ResponsableId,
		Cost,
		Vto,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@EquipmentMaintenanceDefinitionId,
		@CompanyId,
		@Date,
		@Operation,
		@Observations,
		@ProviderId,
		@ResponsableId,
		@Cost,
		@Vto,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @EquipmentMaintenanceActId = @@IDENTITY
	
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
		1,
		GETDATE(),
		@Operation
    )


END

