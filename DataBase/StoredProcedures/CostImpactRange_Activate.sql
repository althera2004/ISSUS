
CREATE PROCEDURE [dbo].[CostImpactRange_Activate]
	@Id bigint,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].[CostImpactRange]
	SET
		Active = 1,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @Id
	AND	CompanyId = @CompanyId
END



