





CREATE PROCEDURE [dbo].[Rules_SetLimit]
	@Id bigint out,
	@CompanyId int,
	@Limit int,
	@UserId int
AS
BEGIN
	UPDATE Rules SET 
		Limit = @Limit,
		ModifiedBy = @UserId, 
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id AND CompanyId = @CompanyId
END







