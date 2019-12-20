





CREATE PROCEDURE [dbo].[EquipmentRepair_Insert] 
	@EquipmentRepairId bigint output,
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

    INSERT INTO EquipmentRepair
    (
		EquipmentId,
		CompanyId,
		RepairType,
		Date,
		Description,
		Tools,
		Observations,
		Cost,
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
		@RepairType,
		@Date,
		@Description,
		@Tools,
		@Observations,
		@Cost,
		@ProviderId,
		@ResponsableId,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @EquipmentRepairId = @@IDENTITY

END






