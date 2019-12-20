

-- =============================================
-- Author:		Juan Castilla Calderón
-- Create date: 12/02/2019
-- Description:	Inserts auditory planning into database
-- =============================================
CREATE PROCEDURE [dbo].[AuditoryPlanning_Update]
	@Id bigint,
	@CompanyId int,
	@AuditoryId bigint,
	@Date datetime,
	@Hour int,
	@Duration int,
	@ProcessId bigint,
	@Auditor int,
	@Audited int,
	@SendMail bit,
	@ProviderEmail nvarchar(150),
	@ApplicationUserId int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE AuditoryPlanning SET
           [Date] = @Date,
           [Hour] =@Hour,
           [Duration] = @Duration,
           [ProcessId] = @ProcessId,
           [Auditor] = @Auditor,
           [Audited] = @Audited,
           [SendMail] = @SendMail,
           [ProviderEmail] = @ProviderEmail,
           [ModifiedBy] = @ApplicationUserId,
           [ModifiedOn] = GETDATE()
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END

