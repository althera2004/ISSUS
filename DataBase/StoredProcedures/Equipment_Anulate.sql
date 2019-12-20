



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Equipment_Anulate]
	@EquipmentId int,
	@CompanyId int,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsible int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Equipment SET
		EndDate = @EndDate,
		EndReason = @EndReason,
		EndResponsible = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @EquipmentId
	AND	CompanyId = @CompanyId

END




