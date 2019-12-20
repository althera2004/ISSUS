





CREATE PROCEDURE [dbo].[EquipmentMaintenanceAct_Update]
	@EquipmentMaintenanceActId bigint,
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

    UPDATE EquipmentMaintenanceAct SET
		Date = @Date,
		Operation = @Operation,
		Observations = @Observations,
		ProviderId = @ProviderId,
		ResponsableId = @ResponsableId,
		Cost = @Cost,
		Vto = @Vto,
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
		2,
		GETDATE(),
		@Operation
    )


END






