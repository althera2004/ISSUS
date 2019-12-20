





CREATE PROCEDURE [dbo].[EquipmentScaleDivision_Insert]
	@EquipmentScaleDivisionId bigint output,
	@Description varchar(20),
	@UserId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO EquipmentScaleDivision
    (
		CompanyId,
		Description,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @EquipmentScaleDivisionId = @@IDENTITY
	
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
		19,
		@EquipmentScaleDivisionId,
		1,
		GETDATE(),
		@Description
    )
END






