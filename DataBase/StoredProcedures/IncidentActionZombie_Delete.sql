

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[IncidentActionZombie_Delete]
	@Id bigint,
	@CompanyId int,
	@AuditoryId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM IncidentActionZombie
	WHERE
		Id = @Id
	AND AuditoryId = @AuditoryId
	AND CompanyId = @CompanyId
           


END

