

-- =============================================
-- Author:		Juan Castilla Calderón
-- Create date: 12/02/2019
-- Description:	Inserts auditory planning into database
-- =============================================
CREATE PROCEDURE [dbo].[AuditoryPlanning_Insert]
	@Id bigint output,
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
	INSERT INTO [dbo].[AuditoryPlanning]
           ([CompanyId]
		   ,[AuditoryId]
           ,[Date]
           ,[Hour]
           ,[Duration]
           ,[ProcessId]
           ,[Auditor]
           ,[Audited]
           ,[SendMail]
           ,[ProviderEmail]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
		   ,@AuditoryId
           ,@Date
           ,@Hour
           ,@Duration
           ,@ProcessId
           ,@Auditor
           ,@Audited
           ,@SendMail
           ,@ProviderEmail
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @Id = @@IDENTITY

END

