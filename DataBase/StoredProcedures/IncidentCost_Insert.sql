
CREATE PROCEDURE [dbo].[IncidentCost_Insert]
	@IncidentCostId bigint output,
	@IncidentId bigint,
	@BusinessRiskId bigint,
	@CompanyId int,
	@Description nvarchar(50),
	@Date datetime,
	@Amount numeric(18,3),
	@Quantity numeric(18,3),
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO IncidentCost
	(
		IncidentId,
		BusinessRiskId,
		CompanyId,
		[Description],
		[Date],
		Amount,
		Quantity,
		Responsable,
		CreatedBy,
		CreatedOn,

		ModifiedBy,
		ModifiedOn,
		Active
	)
	VALUES
	(
		@IncidentId,
		@BusinessRiskId,
		@CompanyId,
		@Description,
		@Date,
		@Amount,
		@Quantity,
		@ResponsableId,
		@UserId,
		GETDATE(),

		@UserId,
		GETDATE(),
		1
	)
	
	SET @IncidentCostId = @@IDENTITY


END
