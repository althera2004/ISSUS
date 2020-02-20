



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObjetivoRegistro_Activate]
	@ObjetivoRegistroId int output,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].ObjetivoRegistro SET 
       [Active] = 1
      ,[ModifiedBy] = @ApplicationUserId
      ,[ModifiedOn] = GETDATE()
	WHERE
		Id = @ObjetivoRegistroId
	AND CompanyId = @CompanyId



END



