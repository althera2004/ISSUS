



CREATE PROCEDURE [dbo].[UploadFiled_Inactive]
	@Id bigint,
	@CompanyId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM UploadFiles
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId
END



