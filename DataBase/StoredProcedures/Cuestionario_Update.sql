

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Cuestionario_Update]
	@Id bigint,
	@CompanyId int,
	@Description nvarchar(150),
	@NormaId bigint,
	@ProcessId bigint,
	@ApartadoNorma nvarchar(50),
	@Notes nvarchar(2000),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE Cuestionario SET
		[Description] = @Description,
        [NormaId] = @NormaId,
        [ProcessId] = @ProcessId,
        [ApartadoNorma] = @ApartadoNorma,
        [Notes] = @Notes,
        [ModifiedBy] = @ApplicationUserId,
        [ModifiedOn] = GETDATE(),
        [Active] = 1
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END

