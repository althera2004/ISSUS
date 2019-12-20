





CREATE PROCEDURE [dbo].[Rules_Update]
	@Id bigint out,
	@CompanyId int,
	@Description nvarchar(50),
	@Notes nvarchar(500),
	@Limit int,
	@UserId int
AS
BEGIN
	UPDATE Rules SET 
		CompanyId = @CompanyId,
		Description = @Description,
		Notes = @Notes,
		Limit = @Limit,
		ModifiedBy = @UserId, 
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id AND CompanyId = @CompanyId
END







