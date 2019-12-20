

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Auditory_GetRules]
	@Rules nvarchar(200),
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select
	R.Id,
	R.[Description],
	R.Active
	from rules R WITH(NOLOCK)

	WHERE
		R.Id in (select * from  dbo.fn_split(@rules,'|'))
	AND R.CompanyId = @CompanyId

END

