


CREATE PROCEDURE [dbo].[AuditoryCuestionarioObservations_Update]
	@Id bigint,
	@CompanyId int,
	@Text nvarchar(2000),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE AuditoryCuestionarioObservations SET
           [Text] = @Text
           ,[ModifiedBy] = @ApplicationUserId
           ,[ModifiedOn] = GETDATE()
     
	 WHERE
		Id = @Id
	AND CompanyId = @CompanyId
		
END

