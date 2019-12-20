





CREATE PROCEDURE [dbo].[EquipmentVerificationDefinition_Update]
	@EquipmentVerificationDefinitionId bigint output,
	@EquipmentId bigint,
    @CompanyId int,
    @Operation nvarchar(50),
    @VerificationType int,
    @Periodicity int,
    @Uncertainty numeric(18,6),
    @Range nvarchar(50),
    @Pattern nvarchar(50),
    @Cost numeric(18,3),
    @Notes text,
    @Responsable int,
	@ProviderId bigint,
	@FirstDate datetime,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationDefinition SET
		EquipmentId = @EquipmentId,
		VerificationType = @VerificationType,
		Operation = @Operation,
		Periodicity = @Periodicity,
		Uncertainty = @Uncertainty,
		Range = @Range,
		Pattern = @Pattern,
		Cost = @Cost,
		Notes = @Notes,
		Responsable = @Responsable,
		ProviderId = @ProviderId,
		FirstDate = @FirstDate,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
      WHERE
		Id = @EquipmentVerificationDefinitionId
	AND CompanyId = @CompanyId

END






