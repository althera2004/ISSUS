




CREATE PROCEDURE [dbo].[Severity_GetAll] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		Id,
		[Description],
		Code
	FROM ProbabilitySeverityRange WITH(NOLOCK)
	WHERE
		Active = 1
	AND Type = 1

	ORDER BY Code
END





