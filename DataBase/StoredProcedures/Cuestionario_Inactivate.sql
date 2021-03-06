

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Cuestionario_Inactivate]
	@Id bigint,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE Cuestionario SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId


	UPDATE CuestionarioPregunta SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		CuestionarioId = @Id
	AND CompanyId = @CompanyId

END

