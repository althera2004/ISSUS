



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Incident_Anulate]
	@IncidentId int,
	@CompanyId int,
	@EndDate datetime,
	@EndResponsible int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Incident SET
		ClosedOn = @EndDate,
		ClosedBy = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IncidentId
	AND	CompanyId = @CompanyId

END




