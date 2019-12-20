
CREATE PROCEDURE [dbo].[IncidentActionCost_Insert]
	@IncidentActionCostId bigint output,
	@IncidentActionId bigint,
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
	
	INSERT INTO IncidentActionCost
	(
		IncidentActionId,
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
		@IncidentActionId,
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
	
	SET @IncidentActionCostId = @@IDENTITY


END
