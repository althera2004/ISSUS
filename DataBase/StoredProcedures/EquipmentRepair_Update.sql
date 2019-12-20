





CREATE PROCEDURE [dbo].[EquipmentRepair_Update]
	@EquipmentRepairId bigint,
	@EquipmentId bigint,
	@CompanyId int,
	@RepairType int,
    @Date datetime,
    @Description text,
    @Tools text,
    @Observations text,
    @Cost numeric(18,3),
    @ProviderId bigint,
    @ResponsableId int,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentRepair SET
    	RepairType = @RepairType,
		Date = @Date,
		Description = @Description,
		Tools = @Tools,
		Observations = @Observations,
		Cost = @Cost,
		ProviderId = @ProviderId,
		ResponsableId = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EquipmentRepairId
	AND CompanyId = @CompanyId


END






