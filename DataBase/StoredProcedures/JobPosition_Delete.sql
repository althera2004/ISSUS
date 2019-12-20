





CREATE PROCEDURE [dbo].[JobPosition_Delete]
	@JobPositionId bigint,
	@CompanyId int,
	@Extradata nvarchar(200),
    @UserId int
AS
BEGIN
	UPDATE Cargos SET 
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @JobPositionId
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
		9,
		@JobPositionId,
		3,
		GETDATE(),
		@ExtraData
    )
END






