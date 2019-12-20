





CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_Update]
	@Id int,
	@CompanyId int,
	@Description nvarchar(50),
	@Code int,
	@Type int,
	@UserId int
AS
BEGIN
	UPDATE [dbo].[ProbabilitySeverityRange]
	SET Description = @Description,
		Code = @Code,
		Type = @Type,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @Id
	AND CompanyId = @CompanyId

END






