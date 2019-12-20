



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Activate]
	@ObjetivoId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE Objetivo SET
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ObjetivoId
	AND CompanyId  = @CompanyId

END




