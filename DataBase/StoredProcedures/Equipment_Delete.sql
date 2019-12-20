





CREATE PROCEDURE [dbo].[Equipment_Delete]
	@EquipmentId bigint,
	@Reason nvarchar(50),
	@UserId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Equipment SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EquipmentId
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
		13,
		@EquipmentId,
		2,
		GETDATE(),
		@Reason
    )
END






