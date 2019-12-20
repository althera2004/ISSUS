





CREATE PROCEDURE [dbo].[CostImpactRange_Update]
	@Id int,
	@CompanyId int,
	@Description nvarchar(50),
	@Code int,
	@Type int,
	@UserId int
AS
BEGIN
	UPDATE [dbo].[CostImpactRange]
	SET Description = @Description,
		Code = @Code,
		Type = @Type,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @Id
	AND CompanyId = @CompanyId

END





