

-- =============================================
-- Author:		Juan Castilla Calderón
-- Create date: 12/02/2019
-- Description:	Activates auditory planning into database
-- =============================================
CREATE PROCEDURE [dbo].[AuditoryPlanning_Activate]
	@Id bigint,
	@CompanyId int,
	@ApplicationUserId int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE AuditoryPlanning SET
		[Active] = 1,
        [ModifiedBy] = @ApplicationUserId,
        [ModifiedOn] = GETDATE()
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END

