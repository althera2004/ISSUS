



CREATE PROCEDURE [dbo].[ApplicationUser_GetCommpanies]
	@ApplicationUserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		C.Id,
		C.Logo,
		C.Name
	FROM ApplicationUserCompany AUC WITH(NOLOCK)
	INNER JOIN Company C WITH(NOLOCK)
	ON	C.Id = AUC.CompanyId

	WHERE
		AUC.UserId = @ApplicationUserId
END




