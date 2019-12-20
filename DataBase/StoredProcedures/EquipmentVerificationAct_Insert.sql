





CREATE PROCEDURE [dbo].[EquipmentVerificationAct_Insert]
	@EquipmentVerificationActId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@EquipmentVerificationType int,
	@Operation nvarchar(50),
	@Date datetime,
	@Vto datetime,
	@Result numeric(18,6),
	@MaxResult numeric(18,6),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentVerificationAct
    (
		EquipmentId,
		CompanyId,
		EquipmentVerificationType,
		Operation,
		Date,
		Vto,
		Result,
		MaxResult,
		Cost,
		ProviderId,
		Responsable,
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
		@EquipmentVerificationType,
		@Operation,
		@Date,
		@Vto,
		@Result,
		@MaxResult,
		@Cost,
		@ProviderId,
		@ResponsableId,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @EquipmentVerificationActId = @@IDENTITY

END






