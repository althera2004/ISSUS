






CREATE PROCEDURE [dbo].[CostImpactRange_Insert]
	@Id bigint out,
	@CompanyId int,
	@Description nvarchar(50),
	@Code int,
	@Type int,
	@UserId int
AS
BEGIN
	INSERT INTO [dbo].[CostImpactRange]
	(
           CompanyId,
           Description,
           Code,
           Type,
           CreatedBy,
           CreatedOn,
           ModifiedBy,
           ModifiedOn,
           Active
	)
    VALUES
    (
		@CompanyId,
		@Description,
		@Code,
		@Type,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		1
	)
		   
	SET @Id = @@IDENTITY
END





