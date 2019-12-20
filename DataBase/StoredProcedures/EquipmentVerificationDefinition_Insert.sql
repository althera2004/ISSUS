





CREATE PROCEDURE [dbo].[EquipmentVerificationDefinition_Insert]
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

    INSERT INTO EquipmentVerificationDefinition
    (
		EquipmentId,
		CompanyId,
		Operation,
		VerificationType,
		Periodicity,
		Uncertainty,
		Range,
		Pattern,
		Cost,
		Notes,
		Responsable,
		ProviderId,
		FirstDate,
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
		@VerificationType,
		@Periodicity,
		@Uncertainty,
		@Range,
		@Pattern,
		@Cost,
		@Notes,
		@Responsable,
		@ProviderId,
		@FirstDate,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
     )
     
     SET @EquipmentVerificationDefinitionId = @@IDENTITY
	 
	 UPDATE Equipment SET
		IsVerification = 1
		WHERE
		Id = @EquipmentId
END





