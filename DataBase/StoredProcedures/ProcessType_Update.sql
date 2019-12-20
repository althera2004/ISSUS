





CREATE PROCEDURE [dbo].[ProcessType_Update]
	@ProcessTypeId int,
	@CompanyId int,
	@Description nvarchar(50),
	@Active bit,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE ProcessType SET
		Description = @Description,
		Active = @Active,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @ProcessTypeId
	AND CompanyId = @CompanyId

END






